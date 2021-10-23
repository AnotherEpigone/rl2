using GoRogue.Components.ParentAware;
using Roguelike2.Components.ItemComponents;
using Roguelike2.Fonts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Roguelike2.GameMechanics.Items
{
    public static class ItemAtlas
    {
        static ItemAtlas()
        {
            ItemsById = typeof(ItemAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<ItemTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i);
        }

        public static Dictionary<string, ItemTemplate> ItemsById { get; }

        public static ItemTemplate SteelLongsword => new ItemTemplate(
            id: "ITEM_STEEL_LONGSWORD",
            name: "Steel longsword",
            glyph: WorldGlyphAtlas.SteelLongsword,
            equipCategoryId: EquipCategoryId.Weapon,
            createComponents: () => new List<IParentAwareComponent>
            {
                new ApplyWhenEquippedComponent(new List<IParentAwareComponent>
                {
                    new EquippedMeleeWeaponComponent("1d6+2", 0),
                }),
            });
        public static ItemTemplate EtheriumShard => new ItemTemplate(
            id: "ITEM_ETHERIUM_SHARD",
            name: "Etherium shard",
            glyph: WorldGlyphAtlas.EtheriumShard,
            equipCategoryId: EquipCategoryId.Trinket,
            createComponents: () => new List<IParentAwareComponent>
            {
                // TODO
                /*new ApplyWhenEquippedComponent(new List<ISerializableComponent>
                {
                    new HealthRegenEffect(0.1f),
                    new EndowmentRegenEffect(1),
                })*/
            });
        public static ItemTemplate StarterOakStaff => new ItemTemplate(
            id: "ITEM_STARTER_OAKSTAFF",
            name: "Oak staff",
            glyph: WorldGlyphAtlas.Staff_Oak,
            equipCategoryId: EquipCategoryId.Staff,
            createComponents: () => new List<IParentAwareComponent>
            {
                // TODO
                /*new ApplyWhenEquippedComponent(new List<ISerializableComponent>
                {
                    new FovRangeEffect(3),
                }),*/
            });
        public static ItemTemplate StarterHomespunCloak => new ItemTemplate(
            id: "ITEM_STARTER_HOMESPUN_CLOAK",
            name: "Homespun cloak",
            glyph: WorldGlyphAtlas.Cloak_Homespun,
            equipCategoryId: EquipCategoryId.Cloak,
            createComponents: () => new List<IParentAwareComponent>
            {
                /*new ApplyWhenEquippedComponent(new List<ISerializableComponent>
                {
                    new DeflectEffect(30),
                })*/
            });
        public static ItemTemplate TrollShroom => new ItemTemplate(
            id: "ITEM_TROLLSHROOM",
            name: "Trollshroom",
            glyph: WorldGlyphAtlas.Trollshroom_Small,
            equipCategoryId: EquipCategoryId.None,
            createComponents: () => new List<IParentAwareComponent>
            {
                /*new ApplyInInventoryEffectsComponent(new List<ISerializableComponent>
                {
                    new SpawnActorAtParentComponent(ActorAtlas.Troll.Id, 1000),
                }),*/
            });
    }
}
