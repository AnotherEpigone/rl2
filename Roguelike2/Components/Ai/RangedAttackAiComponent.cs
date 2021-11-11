using GoRogue.Components.ParentAware;
using Roguelike2.Components.Combat;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Maps;

namespace Roguelike2.Components.Ai
{
    public class RangedAttackAiComponent : IAiComponent
    {
        public IObjectWithComponents Parent { get; set; }

        public (bool success, int ticks) Run(WorldMap map, IDungeonMaster dungeonMaster)
        {
            if (Parent is not Actor mcParent)
            {
                return (false, -1);
            }

            var rangedAttackComponent = mcParent.AllComponents.GetFirstOrDefault<IRangedAttackComponent>();
            return rangedAttackComponent?.TryAttack(map, dungeonMaster) ?? false
                ? (true, TimeHelper.GetAttackTime(mcParent))
                : (false, -1);
        }
    }
}
