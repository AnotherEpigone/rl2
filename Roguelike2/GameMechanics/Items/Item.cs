using GoRogue.Components;
using GoRogue.Components.ParentAware;
using Newtonsoft.Json;
using Roguelike2.Serialization.Entities;
using System.Diagnostics;

namespace Roguelike2.GameMechanics.Items
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Item : IObjectWithComponents
    {
        private readonly IComponentCollection _components;

        public Item(ItemTemplate template)
         : this(
               template.Id,
               template.Name,
               template.Glyph)
        { }

        public Item(
            string templateId,
            string name,
            int glyph)
        {
            TemplateId = templateId;
            Name = name;
            Glyph = glyph;

            _components = new ComponentCollection()
            {
                ParentForAddedComponents = this,
            };
        }

        public string TemplateId { get; }

        public string Name { get; }

        public int Glyph { get; }

        public IComponentCollection GoRogueComponents => _components;

        private string DebuggerDisplay => nameof(Item);
    }
}
