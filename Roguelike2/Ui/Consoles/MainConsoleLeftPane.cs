using Roguelike2.Ui.Consoles.MainConsoleOverlays;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles
{
    public class MainConsoleLeftPane : NovaControlsConsole
    {
        private readonly PlayerStatusConsole _playerConsole;

        public MainConsoleLeftPane(
            int width,
            int height,
            IUiManager uiManager,
            IGameManager gameManager)
            : base(width, height)
        {
            FocusOnMouseClick = false;
            IsFocused = false;

            var mainMenuButton = new Button(Width / 2)
            {
                Text = "Menu (Esc)",
                Position = new Point(0, 0),
                Theme = ThemeHelper.ButtonThemeNoEnds(),
            };
            mainMenuButton.Click += (_, __) => uiManager.CreatePopupMenu(gameManager).Show(true);

            var gameMenuButton = new Button(Width / 2)
            {
                Text = "Help (H)",
                Position = new Point(Width / 2, 0),
                Theme = ThemeHelper.ButtonThemeNoEnds(),
            };

            _playerConsole = new PlayerStatusConsole(width, height - 1)
            {
                Position = new Point(0, 1),
            };

            Controls.Add(mainMenuButton);
            Controls.Add(gameMenuButton);

            Children.Add(_playerConsole);
        }

        public void Update(DungeonMaster dm)
        {
            _playerConsole.Update(dm);
        }
    }
}
