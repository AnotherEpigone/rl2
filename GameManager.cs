using GoRogue.MapGeneration;
using Roguelike2.Entities;
using Roguelike2.Logging;
using Roguelike2.Maps;
using Roguelike2.Serialization;
using Roguelike2.Ui;
using Roguelike2.Ui.Consoles;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using Troschuetz.Random.Generators;

namespace Roguelike2
{
    internal sealed class GameManager : IGameManager
    {
        private readonly IUiManager _uiManager;
        private readonly IEntityFactory _entityFactory;
        private readonly ILogger _logger;
        private readonly ISaveManager _saveManager;

        public GameManager(
            IUiManager uiManager,
            IEntityFactory entityFactory,
            ILogger logger,
            ISaveManager saveManager)
        {
            _uiManager = uiManager;
            _entityFactory = entityFactory;
            _logger = logger;
            _saveManager = saveManager;
        }

        public bool CanLoad()
        {
            return _saveManager.SaveExists();
        }

        public void Load()
        {
            var (success, gameState) = _saveManager.Read();
            if (!success)
            {
                throw new System.IO.IOException("Failed to load save file.");
            }

            var rng = new StandardGenerator();
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;
            var game = new Rl2Game();
            var map = gameState.Map;
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                GetViewportSizeInTiles(tilesetFont, defaultFont) - map.DefaultRenderer.Surface.View.Size);

            var mapManager = new WorldMapManager(game, map);

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, gameState.Map, mapManager, game);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;
        }

        public void LoadLatest()
        {
            Load();
        }

        public void Save()
        {
            if (Game.Instance.Screen is not MainConsole mainConsole)
            {
                return;
            }

            var save = new GameState()
            {
                Map = mainConsole.Map,
            };

            _saveManager.Write(save);
        }

        public void StartNewGame()
        {
            _logger.Debug("Starting new game.");
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;

            var rng = new StandardGenerator();

            var generator = new Generator(40, 40)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");
            var map = new WorldMap(40, 40, tilesetFont);
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                GetViewportSizeInTiles(tilesetFont, defaultFont) - map.DefaultRenderer.Surface.View.Size);

            foreach (var position in map.Positions())
            {
                var template = generatedMap[position] ? TerrainAtlas.Grassland : TerrainAtlas.Ocean;
                map.SetTerrain(new Terrain(position, template.Glyph, template.Name, template.Walkable, template.Transparent));
            }

            var game = new Rl2Game();

            var mapManager = new WorldMapManager(game, map);

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, map, mapManager, game);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;
        }

        private Point GetViewportSizeInTiles(IFont tilesetFont, IFont defaultFont)
        {
            var tileSizeXFactor = (double)tilesetFont.GlyphWidth / defaultFont.GlyphWidth;
            var tileSizeYFactor = (double)tilesetFont.GlyphHeight / defaultFont.GlyphHeight;
            var viewPortTileWidth = (int)(_uiManager.ViewPortWidth / tileSizeXFactor);
            var viewPortTileHeight = (int)(_uiManager.ViewPortHeight / tileSizeYFactor);

            var rightPaneWidth = (int)(MainConsole.RightPaneWidth / tileSizeXFactor);
            var mapWidth = viewPortTileWidth - rightPaneWidth;

            return new Point(mapWidth, viewPortTileHeight);
        }
    }
}
