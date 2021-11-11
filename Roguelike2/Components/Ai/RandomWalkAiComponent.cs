using GoRogue.Components.ParentAware;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Maps;
using SadRogue.Primitives;
using System.Runtime.Serialization;

namespace Roguelike2.Components.Ai
{
    [DataContract]
    public class RandomWalkAiComponent : IAiComponent
    {
        public RandomWalkAiComponent()
            : this(0.5f) { }

        public RandomWalkAiComponent(float restChance)
        {
            RestChance = restChance;
        }

        public IObjectWithComponents Parent { get; set; }

        [DataMember]
        public float RestChance { get; set; }

        public (bool success, int ticks) Run(WorldMap map, IDungeonMaster dungeonMaster)
        {
            if (Parent is not Actor mcParent)
            {
                return (false, -1);
            }

            if (dungeonMaster.Rng.NextDouble() < RestChance)
            {
                return (true, TimeHelper.Wait);
            }

            var directionType = dungeonMaster.Rng.Next(0, 8);
            var direction = (Direction)(Direction.Types)directionType;
            var outcome = mcParent.TryMove(direction);
            return outcome switch
            {
                MoveOutcome.Move => (true, TimeHelper.GetWalkTime(mcParent)),
                MoveOutcome.NoMove => (true, TimeHelper.Wait),
                MoveOutcome.Melee => (true, TimeHelper.GetAttackTime(mcParent)),
                _ => throw new System.NotSupportedException($"Unsupported move outcome {outcome}."),
            };
        }
    }
}
