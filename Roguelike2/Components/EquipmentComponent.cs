using GoRogue.Components.ParentAware;
using Roguelike2.GameMechanics.Items;
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

            dungeonMaster.Logger.Gameplay($"Equipped {item.Name}.");
            category.Items.Add(item);

            EquipmentChanged?.Invoke(this, EventArgs.Empty);

            // TODO
            /*foreach (var triggeredComponent in item.GetGoRogueComponents<IEquipTriggeredComponent>())
            {
                triggeredComponent.OnEquip((McEntity)Parent, dungeonMaster, logManager);
            }*/

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
                dungeonMaster.Logger.Gameplay($"Unequipped {item.Name}.");
            }

            EquipmentChanged?.Invoke(this, EventArgs.Empty);

            // TODO
            /*foreach (var triggeredComponent in item.GetGoRogueComponents<IEquipTriggeredComponent>())
            {
                triggeredComponent.OnUnequip((McEntity)Parent, dungeonMaster, logManager);
            }*/

            return success;
        }
    }
}
