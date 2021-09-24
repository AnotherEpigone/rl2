using SadConsole;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class PlayerStatusConsole : Console
    {
        public PlayerStatusConsole(int width, int height)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;
            UseMouse = false;

            this.Fill(background: ColorHelper.ControlBack);
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);
        }
    }
}
