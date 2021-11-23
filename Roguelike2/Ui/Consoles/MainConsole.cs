using Roguelike2.GameMechanics.Time;
using Roguelike2.Maps;
using Roguelike2.Ui.Consoles.MainConsoleOverlays;
using Roguelike2.Ui.Windows;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Roguelike2.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MainConsole : ScreenObject
    {
        private readonly IGameManager _gameManager;
        private readonly IUiManager _uiManager;
        private readonly WorldMapManager _mapManager;
        private readonly TransientMessageConsole _transientMessageConsole;
        private readonly AlertMessageConsole _alertMessageConsole;
        private readonly MainConsoleLeftPane _leftPane;
        private readonly MapOverlayConsole _mapOverlay;

        public MainConsole(
            IGameManager gameManager,
            IUiManager uiManager,
            WorldMap map,
            WorldMapManager mapManager,
            DungeonMaster dm,
            TurnManager turnManager,
            bool debug)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
            _mapManager = mapManager;

            Map = map;
            Dm = dm;
            TurnManager = turnManager;
            Debug = debug;
            UseMouse = false;
            UseKeyboard = true;

            var minimap = new MinimapScreenSurface(
                Map,
                new MinimapTerrainCellSurface(Map, 320, 240),
                SadConsole.Game.Instance.Fonts[uiManager.MiniMapFontName]);
            var minimapGlyphPosition = new Point(uiManager.ViewPortWidth - 40, 0);
            minimap.Position = new Point(
                minimapGlyphPosition.X * SadConsole.Game.Instance.DefaultFont.GlyphWidth,
                minimapGlyphPosition.Y * SadConsole.Game.Instance.DefaultFont.GlyphHeight);



            var memoryConsole = new MemoryConsole(RightPaneWidth, 16, dm)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth, 15),
            };

            var logConsole = new LogConsole(RightPaneWidth, uiManager.ViewPortHeight - 31)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth, 31),
            };
            Dm.Logger.RegisterEventListener(Logging.LogType.Gameplay, m => logConsole.Add(m));

            _transientMessageConsole = new TransientMessageConsole(60)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth - 60, uiManager.ViewPortHeight - 1),
            };

            _alertMessageConsole = new AlertMessageConsole(60)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth - 60, uiManager.ViewPortHeight - 2),
            };

            _leftPane = new MainConsoleLeftPane(
                LeftPaneWidth,
                uiManager.ViewPortHeight,
                uiManager,
                gameManager,
                Dm,
                TurnManager);

            Map.Position = new Point(LeftPaneWidth * _leftPane.Font.GlyphWidth, 0);

            _mapOverlay = new MapOverlayConsole(
                Map.Width,
                Map.Height,
                Map.Font,
                Map,
                dm,
                RightPaneWidth,
                uiManager.ViewPortHeight - (minimap.HeightPixels / SadConsole.Game.Instance.DefaultFont.GlyphHeight))
            {
                Position = Map.Position / Map.Font.GetFontSize(IFont.Sizes.One),
            };

            dm.Player.Moved += Player_Moved;
            dm.Player.Died += Player_Died;

            Children.Add(Map);
            Children.Add(_mapOverlay);
            Children.Add(minimap);
            Children.Add(memoryConsole);
            Children.Add(logConsole);
            Children.Add(_transientMessageConsole);
            Children.Add(_alertMessageConsole);
            Children.Add(_leftPane);

            if (debug)
            {
                ////SadComponents.Add(new MouseTint());
            }
        }

        public static int RightPaneWidth => 40;

        public static int LeftPaneWidth => 40;

        public WorldMap Map { get; }

        public DungeonMaster Dm { get; }
        public TurnManager TurnManager { get; }
        public bool Debug { get; }

        private string DebuggerDisplay => string.Format($"{nameof(MainConsole)} ({Position.X}, {Position.Y})");

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
            _mapManager.Update();
            _leftPane.Update(Dm);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                _uiManager.CreatePopupMenu(_gameManager).Show(true);
                return true;
            }

            if (_mapManager.HandleKeyboard(info))
            {
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        private void Player_Moved(object sender, GoRogue.GameFramework.GameObjectPropertyChanged<Point> e)
        {
            _mapOverlay.Clear();
        }

        private void Player_Died(object sender, EventArgs e)
        {
            new DeathWindow(_uiManager, _gameManager).Show("You died.");
        }
    }
}
