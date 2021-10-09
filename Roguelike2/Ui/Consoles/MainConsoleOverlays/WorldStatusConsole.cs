using SadConsole;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class WorldStatusConsole : Console
    {
        private readonly DungeonMaster _game;

        public WorldStatusConsole(int width, int height, DungeonMaster game)
            : base(width, height)
        {
            _game = game;

            DefaultBackground = ColorHelper.ControlBack;

            UseMouse = false;

            Refresh();
        }

        public void Refresh()
        {
            this.Clear();
            DrawOutline();
            Cursor.Position = new Point(0, 1);

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Right(2).Print($"Depth -4 (The Midden)\r\n", printTemplate, null);
            Cursor.Right(2).Print($"Time 0", printTemplate, null); // TODO time from TimeManager
        }
        private void DrawOutline()
        {
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            var title = "World";
            Cursor.Position = new Point((Width - title.Length) / 2, 0);
            var coloredTitle = new ColoredString(title, DefaultForeground, DefaultBackground);
            Cursor.Print(coloredTitle);
        }
    }
}
