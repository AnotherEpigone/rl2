using Newtonsoft.Json;
using Roguelike2.Entities;
using Roguelike2.Maps;
using SadRogue.Primitives;
using System;
using System.Runtime.Serialization;

namespace Roguelike2.Serialization.Entities
{
    public class ActorJsonConverter : JsonConverter<Actor>
    {
        public override void WriteJson(JsonWriter writer, Actor value, JsonSerializer serializer) => serializer.Serialize(writer, (ActorSerialized)value);

        public override Actor ReadJson(JsonReader reader, Type objectType, Actor existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<ActorSerialized>(reader);
    }

    [DataContract]
    public class ActorSerialized
    {
        [DataMember] public Point Position;
        [DataMember] string TemplateId;
        [DataMember] Guid UnitId;
        [DataMember] string FactionId;

        public static implicit operator ActorSerialized(Actor actor)
        {
            return new ActorSerialized()
            {
                Position = actor.Position,
                TemplateId = actor.TemplateId,
                UnitId = actor.Id,
                FactionId = actor.FactionId,
            };
        }

        public static implicit operator Actor(ActorSerialized serialized)
        {
            var template = ActorAtlas.ById[serialized.TemplateId];
            return new Actor(
                serialized.Position,
                template.Glyph,
                template.Name,
                false,
                true,
                (int)MapLayer.ACTORS,
                serialized.UnitId,
                serialized.FactionId,
                serialized.TemplateId)
            {
            };
        }
    }
}
