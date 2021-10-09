using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Roguelike2.GameMechanics.Factions
{
    public class FactionAtlas
    {
        static FactionAtlas()
        {
            FactionsById = typeof(FactionAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<Faction>()
                .ToDictionary(
                i => i.Id,
                i => i);
        }

        public static Dictionary<string, Faction> FactionsById { get; }

        public static Faction Player => new Faction(
            "FACTION_PLAYER",
            new Dictionary<string, int>());

        public static Faction Goblins => new Faction(
            "FACTION_GOBLINS",
            new Dictionary<string, int>
            {
            });
    }
}
