using GoRogue.Components.ParentAware;
using GoRogue.SpatialMaps;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Items;
using Roguelike2.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Roguelike2.Components
{
    [DataContract]
    public class InventoryComponent : IInventoryComponent
    {
        /// <summary>Serialization-only, don't use.</summary>
        public InventoryComponent()
        {
        }

        public InventoryComponent(int capacity, params Item[] items)
        {
            Items = items.ToList();
            Capacity = capacity;
        }

        public event EventHandler ContentsChanged;

        public IObjectWithComponents Parent { get; set; }

        public int FilledCapacity => Items.Count;

        public int EmptyCapacity => Capacity - FilledCapacity;

        public bool IsFilled => FilledCapacity >= Capacity;

        [DataMember]
        public int Capacity { get; init; }

        /// <summary>Serialization-only, don't use.</summary>
        [DataMember]
        public List<Item> Items { get; init; }

        public void AddItem(Item item, DungeonMaster dungeonMaster)
        {
            Items.Add(item);

            // todo
            /*foreach (var triggeredComponent in item.GetGoRogueComponents<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnAddedToInventory((McEntity)Parent, dungeonMaster, logManager);
            }*/

            ContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(Item item, DungeonMaster dungeonMaster)
        {
            Items.Remove(item);
            /*foreach (var triggeredComponent in item.GetGoRogueComponents<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnRemovedFromInventory((McEntity)Parent, dungeonMaster, logManager);
            }*/

            ContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DropItem(Item item, DungeonMaster dungeonMaster)
        {
            var parent = (NovaEntity)Parent;
            var position = parent.Position;

            RemoveItem(item, dungeonMaster);
            var existingItem = parent.CurrentMap.GetEntityAt<ItemEntity>(position, LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS));
            if (existingItem != null)
            {
                // combine two items into a new stack
                parent.CurrentMap.RemoveEntity(existingItem);
                var newItemStack = new ItemStackEntity(position, new Item[] { item, existingItem.Item });
                parent.CurrentMap.AddEntity(newItemStack);
                return;
            }

            var existingStack = parent.CurrentMap.GetEntityAt<ItemStackEntity>(position, LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS));
            if (existingStack != null)
            {
                // add to existing stack
                existingStack.Items.Add(item);
                return;
            }

            // nothing here, drop a new item entity
            var droppedItem = new ItemEntity(position, item);
            parent.CurrentMap.AddEntity(droppedItem);
        }

        public IReadOnlyCollection<Item> GetItems()
        {
            return Items;
        }
    }
}
