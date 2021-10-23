using GoRogue.Components.ParentAware;
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
    public class EquipmentComponent : IEquipmentComponent
    {
        /// <summary>Serialization-only, don't use.</summary>
        public EquipmentComponent()
            :this(Array.Empty<EquipCategory>())
        {
        }

        public EquipmentComponent(EquipCategory[] categories)
        {
            Equipment = categories.ToDictionary(
                c => c.Id,
                c => c);
        }

        public event EventHandler EquipmentChanged;

        public IObjectWithComponents Parent { get; set; }

        [DataMember]
        public IReadOnlyDictionary<EquipCategoryId, EquipCategory> Equipment { get; }

        public bool CanEquip(Item item, EquipCategoryId categoryId)
        {
            return Equipment.TryGetValue(categoryId, out var category)
                && category.Slots - category.Items.Count >= 1;
        }

        public bool Equip(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster)
        {
            if (!Equipment.TryGetValue(categoryId, out var category))
            {
                return false;
            }

            if (category.Slots - category.Items.Count < 1)
            {
                return false;
            }

            category.Items.Add(item);

            EquipmentChanged?.Invoke(this, EventArgs.Empty);

            foreach (var triggeredComponent in item.GoRogueComponents.GetAll<IEquipTriggeredComponent>())
            {
                triggeredComponent.OnEquip((Actor)Parent, dungeonMaster);
            }

            return true;
        }

        public bool Unequip(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster)
        {
            if (!Equipment.TryGetValue(categoryId, out var category))
            {
                return false;
            }

            var success = category.Items.Remove(item);
            if (success)
            {
                EquipmentChanged?.Invoke(this, EventArgs.Empty);
            }

            foreach (var triggeredComponent in item.GoRogueComponents.GetAll<IEquipTriggeredComponent>())
            {
                triggeredComponent.OnUnequip((Actor)Parent, dungeonMaster);
            }

            return success;
        }

        public bool Drop(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster)
        {
            if (!Unequip(item, categoryId, dungeonMaster))
            {
                return false;
            }

            var parent = (NovaEntity)Parent;
            MapSpawningHelper.SpawnItem(item, (WorldMap)parent.CurrentMap, parent.Position);
            return true;
        }
    }
}
