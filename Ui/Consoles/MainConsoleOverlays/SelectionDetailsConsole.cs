using Roguelike2.Entities;
using Roguelike2.Maps;
using SadConsole;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class SelectionDetailsConsole : Console
    {
        public SelectionDetailsConsole(int width, int height, WorldMapManager mapManager, WorldMap map, Rl2Game game)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;

            UseMouse = false;

            Update(mapManager, map, game);
        }

        public void Update(WorldMapManager mapManager, WorldMap map, Rl2Game game)
        {
            this.Clear();
            this.Fill(background: ColorHelper.ControlBack);
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);
            Cursor.Position = new Point(0, 1);

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);

            if (mapManager.SelectedPoint != Point.None)
            {
                Cursor.Right(2).Print("Selected tile:\r\n", printTemplate, null);
                Cursor.Right(2).Print($"{mapManager.SelectedPoint} {map.GetTerrainAt<Terrain>(mapManager.SelectedPoint).Name}\r\n\r\n", printTemplate, null);
            }

            if (mapManager.SelectedUnit != null)
            {
                Cursor.Right(2).Print("Selected unit:\r\n", printTemplate, null);
                Cursor.Right(2).Print($"{mapManager.SelectedUnit.Name}\r\n", printTemplate, null);
            }
            else if (mapManager.SelectedPoint != Point.None)
            {
                var unit = map.GetEntityAt<Unit>(mapManager.SelectedPoint);
                if (unit != null)
                {
                    Cursor.Right(2).Print($"{unit.Name}\r\n", printTemplate, null);
                }
            }
        }
    }
}
