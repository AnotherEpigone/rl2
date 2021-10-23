using GoRogue.Components.ParentAware;
using GoRogue.SpatialMaps;
using Roguelike2.Components.Triggers;
using Roguelike2.Entities;
using Roguelike2.GameMechanics;
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

            foreach (var triggeredComponent in item.GoRogueComponents.GetAll<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnAddedToInventory((Actor)Parent, dungeonMaster);
            }

            ContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(Item item, DungeonMaster dungeonMaster)
        {
            Items.Remove(item);

            foreach (var triggeredComponent in item.GoRogueComponents.GetAll<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnRemovedFromInventory((Actor)Parent, dungeonMaster);
            }

            ContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DropItem(Item item, DungeonMaster dungeonMaster)
        {
            var parent = (NovaEntity)Parent;
            RemoveItem(item, dungeonMaster);
            MapSpawningHelper.SpawnItem(item, (WorldMap)parent.CurrentMap, parent.Position);
        }

        public IReadOnlyCollection<Item> GetItems()
        {
            return Items;
        }
    }
}
