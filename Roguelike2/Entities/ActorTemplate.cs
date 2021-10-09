using System;
using System.Collections.Generic;

namespace Roguelike2.Entities
{
    public class ActorTemplate
    {
        public ActorTemplate(
            string id,
            string name,
            int glyph,
            Func<List<object>> createComponents,
            string factionId,
            List<SubTileTemplate> subTiles)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
            CreateComponents = createComponents;
            FactionId = factionId;
            SubTiles = subTiles;
        }

        public string Id { get; }
        public string Name { get; }
        public int Glyph { get; }
        public Func<List<object>> CreateComponents { get; }
        public string FactionId { get; }
        public List<SubTileTemplate> SubTiles { get; }
    }
}
