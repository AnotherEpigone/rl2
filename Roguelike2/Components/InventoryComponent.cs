using GoRogue.Components.ParentAware;
using GoRogue.SpatialMaps;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Items;
using Roguelike2.Maps;
using SadRogue.Primitives;
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

        public void DropItem(Item item, DungeonMaster dungeonMaster)
        {
            var parent = (NovaEntity)Parent;
            var position = parent.Position;
            if (parent.CurrentMap.GetEntityAt<ItemEntity>(position, LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS)) != null)
            {
                var foundSpot = false;
                foreach (var neighborPos in AdjacencyRule.EightWay.Neighbors(position))
                {
                    if (parent.CurrentMap.GetEntityAt<ItemEntity>(neighborPos, LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS)) == null)
                    {
                        position = neighborPos;
                        foundSpot = true;
                        break;
                    }
                }

                if (!foundSpot)
                {
                    // TODO make better... dumb mechanic.
                    dungeonMaster.Logger.Gameplay($"There is no room to drop {item.Name}");
                    return;
                }
            }

            RemoveItem(item, dungeonMaster);
            var droppedItem = new ItemEntity(position, item);
            parent.CurrentMap.AddEntity(droppedItem);
        }

        public IReadOnlyCollection<Item> GetItems()
        {
            return _items;
        }
    }
}
