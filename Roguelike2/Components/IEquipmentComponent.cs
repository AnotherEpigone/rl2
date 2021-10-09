using GoRogue.Components.ParentAware;
using Roguelike2.GameMechanics.Items;
using System;
using System.Collections.Generic;

namespace Roguelike2.Components
{
    public interface IEquipmentComponent : IParentAwareComponent
    {
        event EventHandler EquipmentChanged;

        IReadOnlyDictionary<EquipCategoryId, EquipCategory> Equipment { get; }

        bool CanEquip(Item item, EquipCategoryId categoryId);

        bool Equip(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster);

        bool Unequip(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster);
        bool Drop(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster);
    }
}
