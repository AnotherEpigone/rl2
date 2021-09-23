using Roguelike2.Ui.Consoles.MainConsoleOverlays;
using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles
{
    public class MainConsoleLeftPane : NovaControlsConsole
    {
        public MainConsoleLeftPane(int width, int height)
            : base(width, height)
        {
            var mainMenuButton = new Button(Width / 2)
            {
                Text = "Menu (Esc)",
                Position = new Point(0, 0),
            };

            var gameMenuButton = new Button(Width / 2)
            {
                Text = "Game (G)",
                Position = new Point(Width / 2, 0),
            };

            var playerConsole = new PlayerStatusConsole(width, height - 1)
            {
                Position = new Point(0, 1),
            };

            Controls.Add(mainMenuButton);
            Controls.Add(gameMenuButton);

            Children.Add(playerConsole);
        }
    }
}
