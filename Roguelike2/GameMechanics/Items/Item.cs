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
        public Item(ItemTemplate template)
        {
            TemplateId = template.Id;
            Name = template.Name;
            Glyph = template.Glyph;
            GoRogueComponents = new ComponentCollection()
            {
                ParentForAddedComponents = this,
            };

            foreach (var component in template.CreateComponents())
            {
                GoRogueComponents.Add(component);
            }
        }

        public string TemplateId { get; }

        public string Name { get; }

        public int Glyph { get; }

        public IComponentCollection GoRogueComponents { get; }

        private string DebuggerDisplay => $"{nameof(Item)}: {Name}";
    }
}
