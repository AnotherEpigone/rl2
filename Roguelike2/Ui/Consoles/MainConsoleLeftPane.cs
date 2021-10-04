using Roguelike2.Ui.Consoles.MainConsoleOverlays;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles
{
    public class MainConsoleLeftPane : NovaControlsConsole
    {
        private readonly PlayerStatusConsole _playerConsole;
        private readonly InventoryConsole _inventoryConsole;

        public MainConsoleLeftPane(
            int width,
            int height,
            IUiManager uiManager,
            IGameManager gameManager,
            DungeonMaster dm)
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

            _playerConsole = new PlayerStatusConsole(width)
            {
                Position = new Point(0, 1),
            };

            var memoryConsole = new MemoryConsole(width, 16, dm)
            {
                Position = new Point(0, _playerConsole.Height + 1),
            };

            var equipmentConsole = new EquipmentConsole(width, 16, dm)
            {
                Position = new Point(0, _playerConsole.Height + memoryConsole.Height + 1),
            };

            _inventoryConsole = new InventoryConsole(width, 16, dm)
            {
                Position = new Point(0, equipmentConsole.Position.Y + equipmentConsole.Height),
            };

            Controls.Add(mainMenuButton);
            Controls.Add(gameMenuButton);

            Children.Add(_playerConsole);
            Children.Add(memoryConsole);
            Children.Add(equipmentConsole);
            Children.Add(_inventoryConsole);
        }

        public void Update(DungeonMaster dm)
        {
            _playerConsole.Update(dm);
        }
    }
}
