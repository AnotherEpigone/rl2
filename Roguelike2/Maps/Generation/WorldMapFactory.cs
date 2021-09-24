using GoRogue.MapGeneration;
using Roguelike2.Entities;
using Roguelike2.Maps.Generation.Steps;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System;
using Troschuetz.Random;

namespace Roguelike2.Maps.Generation
{
    public class WorldMapFactory
    {
        public WorldMap Create(
            MapGenerationSettings settings,
            IFont tilesetFont,
            Point viewportSize,
            IGenerator rng,
            IEntityFactory entityFactory)
        {
            var tileCount = settings.Width * settings.Height;
            var landmassIterations = (int)((settings.Width + settings.Height) * 1.5);
            GenerationStep continentsStep = settings.ContinentGeneratorStyle switch
            {
                ContinentGeneratorStyle.Continents => new ContinentsGenerationStep(rng, landmassIterations, 0.3F),
                ContinentGeneratorStyle.Pangaea => new PangaeaGenerationStep(rng, landmassIterations, 0.3F),
                _ => throw new ArgumentException("Unsupported continent generator style."),
            };
            var forestStep = new LandFeatureGenerationStep("Forests", rng, tileCount / 6, tileCount / 3, (INovaGenerationStep)continentsStep);
            var hillStep = new LandFeatureGenerationStep("Hills", rng, tileCount / 6, tileCount / 3, (INovaGenerationStep)continentsStep);

            var mapGenerator = new Generator(settings.Width, settings.Height)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(continentsStep, forestStep, hillStep);
                });

            var map = new WorldMap(settings.Width, settings.Height, tilesetFont);
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                viewportSize - map.DefaultRenderer.Surface.View.Size);

            var continentsMap = mapGenerator.Context.GetFirst<ISettableGridView<bool>>(((INovaGenerationStep)continentsStep).ComponentTag);
            foreach (var position in map.Positions())
            {
                var template = continentsMap[position] ? TerrainAtlas.DirtFloor : TerrainAtlas.BrickWall;
                map.SetTerrain(new Terrain(position, template.Glyph, template.Name, template.Walkable, template.Transparent));
            }


            return map;
        }
    }
}
