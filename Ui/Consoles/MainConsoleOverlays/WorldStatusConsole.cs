using SadConsole;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class WorldStatusConsole : Console
    {
        private readonly Rl2Game _game;

        public WorldStatusConsole(int width, int height, Rl2Game game)
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
            this.Fill(background: ColorHelper.ControlBack);
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);
            Cursor.Position = new Point(0, 1);

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Right(2).Print($"Depth -4 (The Midden)\r\n", printTemplate, null);
            Cursor.Right(2).Print($"Time 0", printTemplate, null); // TODO time from TimeManager
        }
    }
}
