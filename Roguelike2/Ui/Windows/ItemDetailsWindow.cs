using Roguelike2.GameMechanics.Items;
using Roguelike2.Ui.Controls;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Windows
{
    public class ItemDetailsWindow : NovaControlWindow
    {
        private bool _debounced;

        public ItemDetailsWindow(int width, int height, Item item, DungeonMaster dm)
            : base(width, height)
        {
            CloseOnEscKey = false; // needs to be debounced
            IsModalDefault = true;
            Title = item.Name;

            var background = new Console(Width, Height);
            background.Fill(null, ColorHelper.DarkGreyHighlight, null);

            var coloredDetails = new ColoredString("Details here", DefaultForeground, DefaultBackground);
            background.Surface.Print(2, 2, coloredDetails);

            Children.Add(background);

            const string closeButtonText = "Close";
            var closeButtonWidth = 12;
            var closeButton = new NovaSelectionButton(closeButtonWidth, 1)
            {
                Text = closeButtonText,
                Position = new Point(Width / 2 - closeButtonWidth / 2, 14),
            };
            closeButton.Click += (_, __) =>
            {
                if (_debounced)
                {
                    Hide();
                }
            };

            const string dropButtonText = "Drop";
            var dropButtonWidth = 12;
            var dropButton = new NovaSelectionButton(dropButtonWidth, 1)
            {
                Text = dropButtonText,
                Position = new Point(Width / 2 - dropButtonWidth / 2, 13),
            };
            dropButton.Click += (_, __) =>
            {
                dm.Player.Inventory.DropItem(item, dm);
                dm.Logger.Gameplay($"Dropped {item.Name}.");
                Hide();
            };

            SetupSelectionButtons(dropButton, closeButton);
        }

        public static void Show(int width, int height, Item item, DungeonMaster dm)
        {
            var window = new ItemDetailsWindow(width, height, item, dm);
            window.Show(true);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (!info.IsKeyDown(Keys.Escape) && !info.IsKeyDown(Keys.Enter) && !_debounced)
            {
                _debounced = true;
                return true;
            }

            if (!_debounced)
            {
                return base.ProcessKeyboard(info);
            }

            if (info.IsKeyPressed(Keys.Escape))
            {
                Hide();
                return true;
            }

            return base.ProcessKeyboard(info);
        }
    }
}
