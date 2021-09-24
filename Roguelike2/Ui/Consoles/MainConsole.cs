﻿using Roguelike2.Maps;
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

        public MainConsole(
            IGameManager gameManager,
            IUiManager uiManager,
            WorldMap map,
            WorldMapManager mapManager,
            DungeonMaster game,
            bool debug)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
            _mapManager = mapManager;

            Map = map;
            Game = game;

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

            var empireStatusConsole = new WorldStatusConsole(RightPaneWidth, 4, game)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth, 15),
            };

            var logConsole = new LogConsole(RightPaneWidth, uiManager.ViewPortHeight - 19)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth, 19),
            };
            Game.Logger.RegisterEventListener(Logging.LogType.Gameplay, m => logConsole.Add(m));

            _transientMessageConsole = new TransientMessageConsole(60)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth - 60, uiManager.ViewPortHeight - 1),
            };

            _alertMessageConsole = new AlertMessageConsole(60)
            {
                Position = new Point(uiManager.ViewPortWidth - RightPaneWidth - 60, uiManager.ViewPortHeight - 2),
            };

            var leftPane = new MainConsoleLeftPane(
                LeftPaneWidth,
                uiManager.ViewPortHeight,
                uiManager,
                gameManager);

            Map.Position = new Point(LeftPaneWidth * leftPane.Font.GlyphWidth, 0);

            Children.Add(Map);
            Children.Add(minimap);
            Children.Add(empireStatusConsole);
            Children.Add(logConsole);
            Children.Add(_transientMessageConsole);
            Children.Add(_alertMessageConsole);
            Children.Add(leftPane);

            if (debug)
            {
                ////SadComponents.Add(new MouseTint());
            }
        }

        public static int RightPaneWidth => 40;

        public static int LeftPaneWidth => 40;

        public WorldMap Map { get; }

        public DungeonMaster Game { get; }

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