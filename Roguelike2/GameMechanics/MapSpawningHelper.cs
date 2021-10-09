using GoRogue.SpatialMaps;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Items;
using Roguelike2.Maps;
using SadRogue.Primitives;

namespace Roguelike2.GameMechanics
{
    public static class MapSpawningHelper
    {
        public static void SpawnItem(Item item, WorldMap map, Point position)
        {
            var existingItem = map.GetEntityAt<ItemEntity>(position, LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS));
            if (existingItem != null)
            {
                // combine two items into a new stack
                map.RemoveEntity(existingItem);
                var newItemStack = new ItemStackEntity(position, new Item[] { item, existingItem.Item });
                map.AddEntity(newItemStack);
                return;
            }

            var existingStack = map.GetEntityAt<ItemStackEntity>(position, LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS));
            if (existingStack != null)
            {
                // add to existing stack
                existingStack.Items.Add(item);
                return;
            }

            // nothing here, drop a new item entity
            var droppedItem = new ItemEntity(position, item);
            map.AddEntity(droppedItem);
        }
    }
}
