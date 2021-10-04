﻿using Roguelike2.Ui.Themes;
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

        public PlayerStatusConsole(int width)
            : base(width, 7)
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
                DisplayText = "Health 50/50",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_healthBar);

            _focusBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 1),
                Theme = new SimpleProgressBarTheme(Color.DarkBlue.SetAlpha(150), Color.DarkBlue.SetAlpha(150), ColorHelper.Text),
                DisplayText = "Focus 100%",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_focusBar);

            _primeMatterBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 2),
                Theme = new SimpleProgressBarTheme(ColorHelper.DarkGrey2.SetAlpha(150), ColorHelper.DarkGrey2.SetAlpha(150), ColorHelper.Text),
                DisplayText = "Prime Matter 300/300",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_primeMatterBar);

            _virtueBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 3),
                Theme = new SimpleProgressBarTheme(Color.DarkGoldenrod.SetAlpha(150), Color.DarkGoldenrod.SetAlpha(150), ColorHelper.Text),
                DisplayText = "Virtue 50",
                DisplayTextColor = Color.White,
            };
            statBarConsole.Controls.Add(_virtueBar);

            _sanityBar = new ProgressBar(statBarConsole.Width, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 4),
                Theme = new SimpleProgressBarTheme(Color.DarkOliveGreen.SetAlpha(150), Color.DarkOliveGreen.SetAlpha(150), ColorHelper.Text),
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
