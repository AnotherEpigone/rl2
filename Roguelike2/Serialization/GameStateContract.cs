using Newtonsoft.Json.Serialization;
using Roguelike2.Entities;
using Roguelike2.Serialization.Entities;
using System;

namespace Roguelike2.Serialization
{
    public class GameStateContract : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType == typeof(Actor))
            {
                // Can't let the default CreateContract call on Entity or it will throw
                return new JsonObjectContract(objectType)
                {
                    Converter = new ActorJsonConverter()
                };
            }

            return base.CreateContract(objectType);
        }
    }
}
