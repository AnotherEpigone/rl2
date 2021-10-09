using Roguelike2.Maps;
using SadRogue.Primitives;
using System;

namespace Roguelike2.Entities
{
    public class EntityFactory : IEntityFactory
    {
        public Actor CreateActor(Point position, ActorTemplate template, string factionId)
        {
            var unit = new Actor(
                position,
                template.Glyph,
                template.Name,
                false,
                true,
                (int)MapLayer.ACTORS,
                Guid.NewGuid(),
                factionId,
                template.Id);

            return unit;
        }

        public TerrainFeature CreateTerrainFeature(Point position, TerrainFeatureTemplate template)
        {
            var feature = new TerrainFeature(
                position,
                template.Glyph,
                template.Name,
                template.Transparent,
                template.MovementCost);
            return feature;
        }
    }
}
