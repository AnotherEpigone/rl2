using GoRogue.MapGeneration;
using Roguelike2.Entities;
using Roguelike2.GameMechanics;
using Roguelike2.GameMechanics.Combat;
using Roguelike2.GameMechanics.Factions;
using Roguelike2.GameMechanics.Items;
using Roguelike2.GameMechanics.Time;
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
        private readonly ILogger _logger;
        private readonly ISaveManager _saveManager;

        private DungeonMaster _dm;
        private EntityInteractionManager _entityInteractionManager;

        public GameManager(
            IUiManager uiManager,
            ILogger logger,
            ISaveManager saveManager)
        {
            _uiManager = uiManager;
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
            _dm = new DungeonMaster(gameState.Player, _logger, gameState.TimeMaster, gameState.FactMan, new HitMan(rng));
            var map = gameState.Map;
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                GetViewportSizeInTiles(tilesetFont, defaultFont) - map.DefaultRenderer.Surface.View.Size);
            map.AllComponents.Add(new PlayerFieldOfViewHandler());

            map.AddEntity(gameState.Player);

            var turnManager = new TurnManager(_dm, map);
            var playerController = new PlayerController(_dm, turnManager);
            var mapManager = new WorldMapManager(playerController, _dm, turnManager, map);
            _entityInteractionManager = new EntityInteractionManager(_dm, map);

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, gameState.Map, mapManager, _dm, turnManager);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;

            gameState.Player.CalculateFov();
            mapManager.CenterOnPlayer();
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
                Player = _dm.Player,
                TimeMaster = _dm.TimeMaster,
                FactMan = _dm.FactMan,
            };

            _saveManager.Write(save);
        }

        public void StartNewGame()
        {
            _logger.Debug("Starting new game.");
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;

            var rng = new StandardGenerator();

            var generator = new Generator(100, 100)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");
            var map = new WorldMap(100, 100, tilesetFont);
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                GetViewportSizeInTiles(tilesetFont, defaultFont) - map.DefaultRenderer.Surface.View.Size);
            map.AllComponents.Add(new PlayerFieldOfViewHandler());

            foreach (var position in map.Positions())
            {
                var template = generatedMap[position] ? TerrainAtlas.DirtFloor : TerrainAtlas.BrickWall;
                map.SetTerrain(new Terrain(position, template.Glyph, template.Name, template.Walkable, template.Transparent));
            }

            var playerPosition = map.WalkabilityView.RandomPosition(true, rng);
            var player = new Player(playerPosition);
            map.AddEntity(player);

            for ( int i = 0; i < 25; i++)
            {
                var staffPosition = map.WalkabilityView.RandomPosition(true, rng);
                var staff = new ItemEntity(staffPosition, new Item(ItemAtlas.StarterOakStaff));
                map.AddEntity(staff);

                var cloakPosition = map.WalkabilityView.RandomPosition(true, rng);
                var cloak = new ItemEntity(cloakPosition, new Item(ItemAtlas.StarterHomespunCloak));
                map.AddEntity(cloak);

                var swordPosition = map.WalkabilityView.RandomPosition(true, rng);
                var sword = new ItemEntity(swordPosition, new Item(ItemAtlas.SteelLongsword));
                map.AddEntity(sword);

                var goblinPosition = map.WalkabilityView.RandomPosition(true, rng);
                var goblin = new Actor(goblinPosition, ActorAtlas.Goblin);
                map.AddEntity(goblin);

                var goblinArcherPosition = map.WalkabilityView.RandomPosition(true, rng);
                var goblinArcher = new Actor(goblinArcherPosition, ActorAtlas.GoblinArcher);
                map.AddEntity(goblinArcher);
            }

            _dm = new DungeonMaster(player, _logger, new TimeMaster(), CreateFactMan(), new HitMan(rng));
            var turnManager = new TurnManager(_dm, map);
            var playerController = new PlayerController(_dm, turnManager);
            var mapManager = new WorldMapManager(playerController, _dm, turnManager, map);
            _entityInteractionManager = new EntityInteractionManager(_dm, map);

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, map, mapManager, _dm, turnManager);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;

            _logger.Gameplay("You wake up in a trash heap.");
            player.CalculateFov();
            mapManager.CenterOnPlayer();
        }

        private Point GetMapViewport()
        {
            return new Point(
                _uiManager.ViewPortWidth - MainConsole.LeftPaneWidth - MainConsole.RightPaneWidth,
                _uiManager.ViewPortHeight);
        }

        private Point GetViewportSizeInTiles(IFont tilesetFont, IFont defaultFont)
        {
            var viewportSize = GetMapViewport();
            var tileSizeXFactor = (double)tilesetFont.GlyphWidth / defaultFont.GlyphWidth;
            var tileSizeYFactor = (double)tilesetFont.GlyphHeight / defaultFont.GlyphHeight;
            var viewPortTileWidth = (int)(viewportSize.X / tileSizeXFactor);
            var viewPortTileHeight = (int)(viewportSize.Y / tileSizeYFactor);

            return new Point(viewPortTileWidth, viewPortTileHeight);
        }

        private FactionManager CreateFactMan()
        {
            var factionManager = new FactionManager();
            factionManager.Factions.Add(FactionAtlas.Player.Id, FactionAtlas.Player);
            factionManager.Factions.Add(FactionAtlas.Goblins.Id, FactionAtlas.Goblins);
            return factionManager;
        }
    }
}
