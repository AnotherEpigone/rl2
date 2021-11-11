using GoRogue.Components.ParentAware;
using Roguelike2;
using Roguelike2.Components.Ai;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Maps;
using SadRogue.Primitives;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Components.AiComponents
{
    [DataContract]
    public class WalkTowardEnemyAiComponent : IAiComponent
    {
        /// <summary>For serialization only.</summary>
        public WalkTowardEnemyAiComponent()
        { }

        public WalkTowardEnemyAiComponent(int range)
        {
            Range = range;
        }

        public IObjectWithComponents Parent { get; set; }

        [DataMember]
        public int Range { get; set; }

        public (bool success, int ticks) Run(WorldMap map, IDungeonMaster dungeonMaster)
        {
            if (Parent is not Actor mcParent)
            {
                return (false, -1);
            }

            var outcome = TryGetDirectionAndMove(dungeonMaster, map, mcParent);
            if (!outcome.success)
            {
                return (false, -1);
            }

            return outcome.move switch
            {
                MoveOutcome.Move => (true, TimeHelper.GetWalkTime(mcParent)),
                MoveOutcome.NoMove => (true, TimeHelper.Wait),
                MoveOutcome.Melee => (true, TimeHelper.GetAttackTime(mcParent)),
                _ => throw new System.NotSupportedException($"Unsupported move outcome {outcome}."),
            };
        }

        // TODO check for enemies in vision instead of picking on the player
        public (bool success, MoveOutcome move) TryGetDirectionAndMove(IDungeonMaster dungeonMaster, WorldMap map, Actor mcParent)
        {
            if (Distance.Chebyshev.Calculate(mcParent.Position, dungeonMaster.Player.Position) > Range)
            {
                return (false, MoveOutcome.NoMove);
            }

            SetWalkability(mcParent, true);
            try
            {
                var subTileOffsets = mcParent.SubTiles.Select(st => st.Position - mcParent.Position);
                var algorithm = new McAStar(map.WalkabilityView, Distance.CHEBYSHEV, subTileOffsets);
                var path = algorithm.ShortestPath(Parent.Position, map.Player.Position);

                Direction direction;
                if (path == null || path.Length > Range)
                {
                    return MoveOutcome.NoMove;
                }
                else
                {
                    direction = Direction.GetDirection(path.Steps.First() - Parent.Position);
                }

                return mcParent.Move(direction);
            }
            finally
            {
                SetWalkability(mcParent, false);
            }
        }

        // Multi-track drifting!!
        // we don't want to get blocked by our own subtiles, they'll move with us
        private void SetWalkability(Actor mcParent, bool value)
        {
            foreach (var tile in mcParent.SubTiles)
            {
                tile.IsWalkable = value;
            }

            mcParent.IsWalkable = value;
        }
    }
}
