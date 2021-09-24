using SadConsole;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class LogConsole : Console
    {
        private readonly Console _messageConsole;

        public LogConsole(int width, int height)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;
            UseMouse = false;

            this.Fill(background: ColorHelper.ControlBack);
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            _messageConsole = new Console(width - 2, height - 2)
            {
                Position = new Point(1, 1),
                DefaultBackground = DefaultBackground,
                UseMouse = false,
            };

            Children.Add(_messageConsole);
        }

        public void Add(string message)
        {
            var coloredMessage = new ColoredString($"> {message}\r\n", DefaultForeground, DefaultBackground);
            _messageConsole.Cursor.Print(coloredMessage);
        }
    }
}
