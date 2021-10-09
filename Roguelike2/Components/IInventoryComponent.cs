using GoRogue.Components.ParentAware;
using Roguelike2.GameMechanics.Items;
using System;
using System.Collections.Generic;

namespace Roguelike2.Components
{
    public interface IInventoryComponent : IParentAwareComponent
    {
        event EventHandler ContentsChanged;

        public int FilledCapacity { get; }

        public int Capacity { get; }
        bool IsFilled { get; }
        int EmptyCapacity { get; }

        IReadOnlyCollection<Item> GetItems();

        void AddItem(Item item, DungeonMaster dungeonMaster);

        void RemoveItem(Item item, DungeonMaster dungeonMaster);

        void DropItem(Item item, DungeonMaster dungeonMaster);
    }
}
