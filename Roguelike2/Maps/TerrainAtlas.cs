using Roguelike2.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Roguelike2.Maps
{
    public static class TerrainAtlas
    {
        private static readonly Lazy<Dictionary<string, TerrainTemplate>> _byId;

        static TerrainAtlas()
        {
            _byId = new Lazy<Dictionary<string, TerrainTemplate>>(() => typeof(TerrainAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(TerrainTemplate))
                .Select(p => p.GetValue(null))
                .OfType<TerrainTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, TerrainTemplate> ById => _byId.Value;

        public static TerrainTemplate DirtFloor => new TerrainTemplate(
            "TERRAIN_DIRTFLOOR",
            "Dirt floor",
            WorldGlyphAtlas.Ground_Dirt,
            true,
            true);

        public static TerrainTemplate BrickWall => new TerrainTemplate(
            "TERRAIN_BRICKWALL",
            "Brick wall",
            WorldGlyphAtlas.Wall_Brick,
            false,
            false);
    }
}
