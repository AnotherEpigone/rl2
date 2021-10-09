using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Roguelike2.Entities
{
    public static class ActorAtlas
    {
        private static readonly Lazy<Dictionary<string, ActorTemplate>> _byId;

        static ActorAtlas()
        {
            _byId = new Lazy<Dictionary<string, ActorTemplate>>(() => typeof(ActorAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(ActorTemplate))
                .Select(p => p.GetValue(null))
                .OfType<ActorTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, ActorTemplate> ById => _byId.Value;
    }
}
