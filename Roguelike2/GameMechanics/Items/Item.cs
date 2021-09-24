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

        public Item()
        {
            _components = new ComponentCollection()
            {
                ParentForAddedComponents = this,
            };
        }

        public IComponentCollection GoRogueComponents => _components;

        private string DebuggerDisplay => nameof(Item);
    }
}
