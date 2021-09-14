using Roguelike2.Maps;
using Roguelike2.Ui.Consoles.MainConsoleOverlays;
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

        private bool _firstUpdate;
        private DateTime _lastEndTurnAttempt;
        private DateTime _lastReadyToEndTurnCheck;

        public MainConsole(
            IGameManager gameManager,
            IUiManager uiManager,
            WorldMap map,
            WorldMapManager mapManager,
            Rl2Game game,
            bool debug)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
            _mapManager = mapManager;

            Map = map;
            Game = game;

            UseMouse = false;
            UseKeyboard = true;

            _lastEndTurnAttempt = DateTime.MinValue;
            _lastReadyToEndTurnCheck = DateTime.MinValue;

            _firstUpdate = true;

            var minimap = new MinimapScreenSurface(
                Map,
                new MinimapTerrainCellSurface(Map, 320, 240),
                SadConsole.Game.Instance.Fonts[uiManager.MiniMapFontName]);
            var minimapGlyphPosition = new Point(uiManager.ViewPortWidth - 40, 0);
            minimap.Position = new Point(
                minimapGlyphPosition.X * SadConsole.Game.Instance.DefaultFont.GlyphWidth,
                minimapGlyphPosition.Y * SadConsole.Game.Instance.DefaultFont.GlyphHeight);

            var empireStatusConsole = new EmpireStatusConsole(RightPaneWidth, 5, game)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth, 15),
            };

            var selectionDetailsConsole = new SelectionDetailsConsole(RightPaneWidth, 15, _mapManager, Map, Game)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth, 20),
            };

            var logConsole = new LogConsole(RightPaneWidth, uiManager.ViewPortHeight - 36)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth, 35),
            };

            _transientMessageConsole = new TransientMessageConsole(60)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth - 60, uiManager.ViewPortHeight - 1),
            };

            _alertMessageConsole = new AlertMessageConsole(60)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth - 60, uiManager.ViewPortHeight - 2),
            };

            _mapManager.SelectionChanged += (_, __) => selectionDetailsConsole.Update(_mapManager, Map, Game);
            _mapManager.SelectionStatsChanged += (_, __) => selectionDetailsConsole.Update(_mapManager, Map, Game);

            Children.Add(Map);
            Children.Add(minimap);
            Children.Add(empireStatusConsole);
            Children.Add(selectionDetailsConsole);
            Children.Add(logConsole);
            Children.Add(_transientMessageConsole);
            Children.Add(_alertMessageConsole);

            if (debug)
            {
                ////SadComponents.Add(new MouseTint());
            }
        }

        public static int RightPaneWidth => 40;

        public WorldMap Map { get; }

        public Rl2Game Game { get; }

        private string DebuggerDisplay => string.Format($"{nameof(MainConsole)} ({Position.X}, {Position.Y})");

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
            _mapManager.Update();
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
    }
}
