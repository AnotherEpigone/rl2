using Newtonsoft.Json;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Items;
using SadRogue.Primitives;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Roguelike2.Serialization.Entities
{
    public class ItemStackEntityJsonConverter : JsonConverter<ItemStackEntity>
    {
        public override void WriteJson(JsonWriter writer, ItemStackEntity value, JsonSerializer serializer) => serializer.Serialize(writer, (ItemStackEntitySerialized)value);

        public override ItemStackEntity ReadJson(JsonReader reader, Type objectType, ItemStackEntity existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<ItemStackEntitySerialized>(reader);
    }

    [DataContract]
    public class ItemStackEntitySerialized
    {
        [DataMember] public Point Position;
        [DataMember] public Item[] Items;
        [DataMember] public Guid Id;

        public static implicit operator ItemStackEntitySerialized(ItemStackEntity itemStackEntity)
        {
            return new ItemStackEntitySerialized()
            {
                Position = itemStackEntity.Position,
                Items = itemStackEntity.Items.ToArray(),
                Id = itemStackEntity.Id,
            };
        }

        public static implicit operator ItemStackEntity(ItemStackEntitySerialized serialized)
        {
            return new ItemStackEntity(
                serialized.Position,
                serialized.Items,
                serialized.Id)
            {
            };
        }
    }
}
