using Newtonsoft.Json;
using Roguelike2.GameMechanics.Items;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Roguelike2.Serialization.Entities
{
    public class ItemJsonConverter : JsonConverter<Item>
    {
        public override void WriteJson(JsonWriter writer, Item value, JsonSerializer serializer) => serializer.Serialize(writer, (ItemSerialized)value);

        public override Item ReadJson(JsonReader reader, Type objectType, Item existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<ItemSerialized>(reader);
    }

    [DataContract]
    public class ItemSerialized
    {
        [DataMember] public object[] Components;
        [DataMember] public string TemplateId;
        [DataMember] public string Name;
        [DataMember] public int Glyph;

        public static implicit operator ItemSerialized(Item item)
        {
            return new ItemSerialized()
            {
                TemplateId = item.TemplateId,
                Name = item.Name,
                Glyph = item.Glyph,
                Components = item.GoRogueComponents.Select(pair => pair.Component).ToArray(),
            };
        }

        public static implicit operator Item(ItemSerialized serialized)
        {
            var template = ItemAtlas.ItemsById[serialized.TemplateId];
            var item = new Item(template);
            item.GoRogueComponents.Clear();
            foreach (var component in serialized.Components)
            {
                item.GoRogueComponents.Add(component);
            }

            return item;
        }
    }
}
