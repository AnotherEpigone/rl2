using SadRogue.Primitives;
using System;

namespace Roguelike2.Entities
{
    public interface IEntityFactory
    {
        Unit CreateUnit(Point position, UnitTemplate template, Guid empireId, Color factionColor);
        TerrainFeature CreateTerrainFeature(Point position, TerrainFeatureTemplate template);
    }
}