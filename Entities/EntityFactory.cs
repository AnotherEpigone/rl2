using Roguelike2.Maps;
using SadRogue.Primitives;
using System;

namespace Roguelike2.Entities
{
    public class EntityFactory : IEntityFactory
    {
        public Unit CreateUnit(Point position, UnitTemplate template, Guid empireId, Color empireColor)
        {
            var unit = new Unit(
                position,
                template.Glyph,
                template.Name,
                false,
                true,
                (int)MapEntityLayer.ACTORS,
                Guid.NewGuid(),
                empireId,
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
