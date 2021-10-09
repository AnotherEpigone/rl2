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
        private readonly ProgressBar _corruptionBar;
        private readonly ProgressBar _madnessBar;

        public PlayerStatusConsole(int width)
            : base(width, 12)
        {
            DefaultBackground = ColorHelper.ControlBack;
            UseMouse = false;

            this.Fill(background: ColorHelper.ControlBack);
            DrawOutline();

            var statBarConsole = new NovaControlsConsole(width - 2, 5)
            {
                Position = new Point(1, 1),
            };
            
            _healthBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 0),
                Theme = new SimpleProgressBarTheme(Color.DarkRed.SetAlpha(150), Color.DarkRed.SetAlpha(150), ColorHelper.Text),
                DisplayText = "Health 100/100",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_healthBar);

            _focusBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 1),
                Theme = new SimpleProgressBarTheme(Color.DarkBlue.SetAlpha(150), Color.DarkBlue.SetAlpha(150), ColorHelper.Text),
                DisplayText = "Focus 100/100",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_focusBar);

            _primeMatterBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 2),
                Theme = new SimpleProgressBarTheme(ColorHelper.DarkGrey2.SetAlpha(150), ColorHelper.DarkGrey2.SetAlpha(150), ColorHelper.Text),
                DisplayText = "Prime Matter 100/100",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_primeMatterBar);

            _corruptionBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 3),
                Theme = new SimpleProgressBarTheme(Color.Black.SetAlpha(150), Color.Black.SetAlpha(150), ColorHelper.Text),
                DisplayText = "Corruption 0/100",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_corruptionBar);

            _madnessBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 4),
                Theme = new SimpleProgressBarTheme(ColorHelper.MadnessPurple.SetAlpha(150), ColorHelper.MadnessPurple.SetAlpha(150), ColorHelper.Text),
                DisplayText = "Madness 0/100",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_madnessBar);

            Children.Add(statBarConsole);
        }

        public void Update(DungeonMaster dm)
        {
            // TODO update based on stats
            _healthBar.Progress = 1f;
            _focusBar.Progress = 1f;
            _primeMatterBar.Progress = 1f;
            _corruptionBar.Progress = 0.5f;
            _madnessBar.Progress = 0.5f;
        }

        private void DrawOutline()
        {
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            var title = $"Player";
            Cursor.Position = new Point((Width - title.Length) / 2, 0);
            var coloredTitle = new ColoredString(title, DefaultForeground, DefaultBackground);
            Cursor.Print(coloredTitle);
        }
    }
}
