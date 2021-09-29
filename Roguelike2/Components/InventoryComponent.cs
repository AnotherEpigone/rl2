using GoRogue.Components.ParentAware;
using Roguelike2.GameMechanics.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike2.Components
{
    // TODO serialization
    public class InventoryComponent : IInventoryComponent
    {
        private readonly List<Item> _items;

        public InventoryComponent(int capacity, params Item[] items)
        {
            _items = items.ToList();
            Capacity = capacity;
        }

        public event EventHandler ContentsChanged;

        public IObjectWithComponents Parent { get; set; }

        public int FilledCapacity => _items.Count;

        public bool IsFilled => FilledCapacity >= Capacity;

        public int Capacity { get; }

        public void AddItem(Item item, DungeonMaster dungeonMaster)
        {
            _items.Add(item);

            // todo
            /*foreach (var triggeredComponent in item.GetGoRogueComponents<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnAddedToInventory((McEntity)Parent, dungeonMaster, logManager);
            }*/

            ContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(Item item, DungeonMaster dungeonMaster)
        {
            _items.Remove(item);
            /*foreach (var triggeredComponent in item.GetGoRogueComponents<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnRemovedFromInventory((McEntity)Parent, dungeonMaster, logManager);
            }*/

            ContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        public IReadOnlyCollection<Item> GetItems()
        {
            return _items;
        }
    }
}
