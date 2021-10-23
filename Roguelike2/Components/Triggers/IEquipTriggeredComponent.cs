using GoRogue.Components.ParentAware;
using Roguelike2.Entities;

namespace Roguelike2.Components.Triggers
{
    interface IEquipTriggeredComponent : IParentAwareComponent
    {
        public void OnEquip(Actor equipmentOwner, IDungeonMaster dungeonMaster);

        public void OnUnequip(Actor equipmentOwner, IDungeonMaster dungeonMaster);
    }
}
