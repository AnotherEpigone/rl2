using Newtonsoft.Json;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Items;
using SadRogue.Primitives;
using System;
using System.Runtime.Serialization;

namespace Roguelike2.Serialization.Entities
{
    public class ItemEntityJsonConverter : JsonConverter<ItemEntity>
    {
        public override void WriteJson(JsonWriter writer, ItemEntity value, JsonSerializer serializer) => serializer.Serialize(writer, (ItemEntitySerialized)value);

        public override ItemEntity ReadJson(JsonReader reader, Type objectType, ItemEntity existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<ItemEntitySerialized>(reader);
    }

    [DataContract]
    public class ItemEntitySerialized
    {
        [DataMember] public Point Position;
        [DataMember] public Item Item;
        [DataMember] public Guid Id;

        public static implicit operator ItemEntitySerialized(ItemEntity itemEntity)
        {
            return new ItemEntitySerialized()
            {
                Position = itemEntity.Position,
                Item = itemEntity.Item,
                Id = itemEntity.Id,
            };
        }

        public static implicit operator ItemEntity(ItemEntitySerialized serialized)
        {
            return new ItemEntity(
                serialized.Position,
                serialized.Item,
                serialized.Id)
            {
            };
        }
    }
}
