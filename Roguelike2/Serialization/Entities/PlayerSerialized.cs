using Newtonsoft.Json;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Items;
using SadRogue.Primitives;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Roguelike2.Serialization.Entities
{
    public class PlayerJsonConverter : JsonConverter<Player>
    {
        public override void WriteJson(JsonWriter writer, Player value, JsonSerializer serializer) => serializer.Serialize(writer, (PlayerSerialized)value);

        public override Player ReadJson(JsonReader reader, Type objectType, Player existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<PlayerSerialized>(reader);
    }

    [DataContract]
    public class PlayerSerialized
    {
        [DataMember] public Point Position;
        [DataMember] public object[] Components;

        public static implicit operator PlayerSerialized(Player player)
        {
            return new PlayerSerialized()
            {
                Position = player.Position,
                Components = player.AllComponents.Select(pair => pair.Component).ToArray(),
            };
        }

        public static implicit operator Player(PlayerSerialized serialized)
        {
            return new Player(serialized);
        }
    }
}
