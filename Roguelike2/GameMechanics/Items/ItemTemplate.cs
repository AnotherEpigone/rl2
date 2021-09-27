using GoRogue.Components.ParentAware;
using System;
using System.Collections.Generic;

namespace Roguelike2.GameMechanics.Items
{
    public class ItemTemplate
    {
        public ItemTemplate(
            string id,
            string name,
            int glyph,
            EquipCategoryId equipCategoryId,
            Func<List<IParentAwareComponent>> createComponents)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
            CreateComponents = createComponents;
            EquipCategoryId = equipCategoryId;
        }

        public string Id { get; }

        public string Name { get; }

        public int Glyph { get; }

        public EquipCategoryId EquipCategoryId { get; }

        public Func<List<IParentAwareComponent>> CreateComponents { get; }
    }
}
