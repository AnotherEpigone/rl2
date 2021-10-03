using Newtonsoft.Json;
using Roguelike2.Fonts;
using Roguelike2.GameMechanics.Items;
using Roguelike2.Maps;
using Roguelike2.Serialization.Entities;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Roguelike2.Entities
{
    /// <summary>
    /// Item that represents an item on the ground
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(ItemStackEntityJsonConverter))]
    public class ItemStackEntity : NovaEntity
    {
        public ItemStackEntity(Point position, IEnumerable<Item> items)
            : this(position, items, Guid.NewGuid())
        {
        }

        public ItemStackEntity(Point position, IEnumerable<Item> items, Guid id)
            : base(position, WorldGlyphAtlas.Chest_Small, "Item stack", true, true, (int)MapLayer.ITEMS, id)
        {
            Items = items.ToList();
        }

        public IList<Item> Items { get; }

        private string DebuggerDisplay => $"{nameof(ItemStackEntity)}: {string.Join(", ", Items.Select(i => i.Name))}";
    }
}
