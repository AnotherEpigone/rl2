using GoRogue.Components.ParentAware;
using Roguelike2.Maps;

namespace Roguelike2.Components.Combat
{
    /// <summary>
    /// Provides the parent a ranged attack
    /// </summary>
    public interface IRangedAttackComponent : IParentAwareComponent
    {
        bool TryAttack(WorldMap map, IDungeonMaster dungeonMaster);
    }
}
