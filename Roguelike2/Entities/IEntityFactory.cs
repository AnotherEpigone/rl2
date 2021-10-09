using SadRogue.Primitives;
using System;

namespace Roguelike2.Entities
{
    public interface IEntityFactory
    {
        Actor CreateActor(Point position, ActorTemplate template, string factionId);
        TerrainFeature CreateTerrainFeature(Point position, TerrainFeatureTemplate template);
    }
}