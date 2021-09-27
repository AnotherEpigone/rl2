﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roguelike2.GameMechanics.Items
{
    public enum EquipCategoryId
    {
        None,
        Staff,
        Cloak,
        Weapon,
        Trinket,
    }

    [DataContract]
    public class EquipCategory
    {
        public EquipCategory(EquipCategoryId id, string name, int slots)
        {
            Id = id;
            Name = name;
            Slots = slots;
            Items = new List<Item>();
        }

        [DataMember]
        public EquipCategoryId Id { get; init; }

        [DataMember]
        public string Name { get; init; }

        [DataMember]
        public int Slots { get; init; }

        [DataMember]
        public List<Item> Items { get; init; }
    }
}
