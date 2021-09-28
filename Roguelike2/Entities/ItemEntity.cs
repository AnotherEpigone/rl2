using Newtonsoft.Json;
using Roguelike2.GameMechanics.Items;
using Roguelike2.Maps;
using Roguelike2.Serialization.Entities;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Roguelike2.Entities
{
    /// <summary>
    /// Item that represents an item on the ground
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(ItemEntityJsonConverter))]
    public class ItemEntity : NovaEntity
    {
        public ItemEntity(Point position, Item item)
            : this(position, item, Guid.NewGuid())
        {
        }

        public ItemEntity(Point position, Item item, Guid id)
            : base(position, item.Glyph, item.Name, true, true, (int)MapEntityLayer.ITEMS, id)
        {
            Item = item;
        }

        public Item Item { get; }

        private string DebuggerDisplay => $"{nameof(ItemEntity)}: {Name}";
    }
}
