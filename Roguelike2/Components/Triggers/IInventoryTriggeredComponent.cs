using GoRogue.Components.ParentAware;
using Roguelike2.Entities;

namespace Roguelike2.Components.Triggers
{
    public interface IInventoryTriggeredComponent : IParentAwareComponent
    {
        public void OnAddedToInventory(Actor inventoryOwner, IDungeonMaster dungeonMaster);

        public void OnRemovedFromInventory(Actor inventoryOwner, IDungeonMaster dungeonMaster);
    }
}
