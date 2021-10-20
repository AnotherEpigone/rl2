using Roguelike2.GameMechanics.Time;
using Roguelike2.Maps;
using Roguelike2.Ui.Consoles;
using Roguelike2.Ui.Windows;
using SadRogue.Primitives;

namespace Roguelike2.Ui
{
    public interface IUiManager
    {
        int ViewPortHeight { get; }
        int ViewPortWidth { get; }
        string TileFontPath { get; }
        string TileFontName { get; }
        string MiniMapFontPath { get; }
        string MiniMapFontName { get; }

        void ShowMainMenu(IGameManager gameManager);

        PopupMenuWindow CreatePopupMenu(IGameManager gameManager);

        MainConsole CreateMapScreen(
            IGameManager gameManager,
            WorldMap map,
            WorldMapManager mapManager,
            DungeonMaster game,
            TurnManager turnManager);

        void ToggleFullScreen();

        void SetViewport(int width, int height);

        int GetSidePanelWidth();

        Point GetMapConsoleSize();

        Point GetCentralWindowSize();
    }
}
