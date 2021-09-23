using Newtonsoft.Json;
using Roguelike2.Serialization.Maps;
using SadRogue.Integration;
using SadRogue.Primitives;
using System.Diagnostics;

namespace Roguelike2.Maps
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(TerrainJsonConverter))]
    public class Terrain : RogueLikeCell
    {
        public Terrain(
            Point position,
            int glyph,
            string name,
            bool walkable,
            bool transparent)
            : base(position, Color.White, Color.Black, glyph, 0, walkable, transparent)
        {
            Glyph = glyph;
            Name = name;
        }

        public int Glyph { get; }
        public string Name { get; }
        public bool Explored { get; set; }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(Terrain)} ({Name})");
            }
        }
    }
}
