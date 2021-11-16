using GoRogue.Components.ParentAware;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Maps;
using Roguelike2.Maps.Pathing;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Roguelike2.Components.Ai
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

        public (bool success, MoveOutcome move) TryGetDirectionAndMove(IDungeonMaster dungeonMaster, WorldMap map, Actor mcParent)
        {
            // TODO check for enemies in vision instead of picking on the player
            var targetPosition = dungeonMaster.Player.Position;

            if (Distance.Chebyshev.Calculate(mcParent.Position, targetPosition) > Range)
            {
                return (false, MoveOutcome.NoMove);
            }

            SetWalkability(mcParent, true);
            try
            {
                // todo subtiles
                // var subTileOffsets = mcParent.SubTiles.Select(st => st.Position - mcParent.Position);
                var subTileOffsets = new List<Point>();
                var algorithm = new McAStar(map.WalkabilityView, Distance.Chebyshev, subTileOffsets);
                var path = algorithm.ShortestPath(mcParent.Position, targetPosition);

                Direction direction;
                if (path == null || path.Length > Range)
                {
                    return (false, MoveOutcome.NoMove);
                }
                else
                {
                    direction = Direction.GetDirection(path.Steps.First() - mcParent.Position);
                }

                return (true, mcParent.TryMove(direction));
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
            // todo subtiles
            /*foreach (var tile in mcParent.SubTiles)
            {
                tile.IsWalkable = value;
            }*/
        }
    }
}
