using Roguelike2.Ui.Themes;
using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class PlayerStatusConsole : Console
    {
        private readonly ProgressBar _healthBar;
        private readonly ProgressBar _focusBar;
        private readonly ProgressBar _primeMatterBar;
        private readonly ProgressBar _virtueBar;
        private readonly ProgressBar _sanityBar;

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

            var statBarConsole = new NovaControlsConsole(width - 2, 6)
            {
                Position = new Point(1, 4),
            };
            
            _healthBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 0),
                Theme = new SimpleProgressBarTheme(Color.DarkRed, Color.DarkRed, ColorHelper.Text),
                DisplayText = "Health 50/50",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_healthBar);

            _focusBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 1),
                Theme = new SimpleProgressBarTheme(Color.DarkBlue, Color.DarkBlue, ColorHelper.Text),
                DisplayText = "Focus 100%",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_focusBar);

            _primeMatterBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 2),
                Theme = new SimpleProgressBarTheme(ColorHelper.DarkGrey2, ColorHelper.DarkGrey2, ColorHelper.Text),
                DisplayText = "Prime Matter 300/300",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_primeMatterBar);

            _virtueBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 4),
                Theme = new SimpleProgressBarTheme(Color.DarkGoldenrod, Color.DarkGoldenrod, ColorHelper.Text),
                DisplayText = "Virtue 50",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_virtueBar);

            _sanityBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 5),
                Theme = new SimpleProgressBarTheme(Color.DarkOliveGreen, Color.DarkOliveGreen, ColorHelper.Text),
                DisplayText = "Sanity 100%",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_sanityBar);

            Children.Add(statBarConsole);
        }

        public void Update(DungeonMaster dm)
        {
            // TODO update based on stats
            _healthBar.Progress = 1f;
            _focusBar.Progress = 1f;
            _primeMatterBar.Progress = 1f;
            _virtueBar.Progress = 1f;
            _sanityBar.Progress = 1f;
        }
    }
}
