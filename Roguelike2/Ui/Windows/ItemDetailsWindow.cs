using Roguelike2.GameMechanics.Items;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Ui.Controls;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Windows
{
    public class ItemDetailsWindow : NovaControlWindow
    {
        private bool _debounced;

        public ItemDetailsWindow(
            int width,
            int height,
            Item item,
            DungeonMaster dm,
            TurnManager turnManager,
            bool equipped)
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

            const int buttonWidth = 12;
            var closeButton = new NovaSelectionButton(buttonWidth, 1)
            {
                Text = "Close",
                Position = new Point(Width / 2 - buttonWidth / 2, 14),
            };
            closeButton.Click += (_, __) =>
            {
                if (_debounced)
                {
                    Hide();
                }
            };

            var dropButton = new NovaSelectionButton(buttonWidth, 1)
            {
                Text = "Drop",
                Position = new Point(Width / 2 - buttonWidth / 2, 13),
            };
            dropButton.Click += (_, __) =>
            {
                if (equipped)
                {
                    var categoryId = ItemAtlas.ItemsById[item.TemplateId].EquipCategoryId;
                    dm.Player.Equipment.Drop(item, categoryId, dm);
                }
                else
                {
                    dm.Player.Inventory.DropItem(item, dm);
                }

                dm.Logger.Gameplay($"Dropped {item.Name}.");
                Hide();
            };

            var equipButton = new NovaSelectionButton(buttonWidth, 1)
            {
                IsVisible = !equipped,
                Text = "Equip",
                Position = new Point(Width / 2 - buttonWidth / 2, 12),
            };
            equipButton.Click += (_, __) =>
            {
                var categoryId = ItemAtlas.ItemsById[item.TemplateId].EquipCategoryId;
                if (!dm.Player.Equipment.CanEquip(item, categoryId))
                {
                    dm.Logger.Gameplay($"Can't equip {item.Name}.");
                    return;
                }

                dm.Logger.Gameplay($"Equipped {item.Name}.");

                dm.Player.Inventory.RemoveItem(item, dm);
                dm.Player.Equipment.Equip(item, categoryId, dm);

                turnManager.PostProcessPlayerTurn(TimeHelper.Equip);

                Hide();
            };

            var unequipButton = new NovaSelectionButton(buttonWidth, 1)
            {
                IsVisible = equipped,
                Text = "Unequip",
                Position = new Point(Width / 2 - buttonWidth / 2, 12),
            };
            unequipButton.Click += (_, __) =>
            {
                if (dm.Player.Inventory.IsFilled)
                {
                    dm.Logger.Gameplay($"Can't unequip {item.Name}. Inventory is full.");
                    return;
                }

                dm.Logger.Gameplay($"Unequipped {item.Name}.");

                var categoryId = ItemAtlas.ItemsById[item.TemplateId].EquipCategoryId;
                dm.Player.Equipment.Unequip(item, categoryId, dm);
                dm.Player.Inventory.AddItem(item, dm);

                turnManager.PostProcessPlayerTurn(TimeHelper.Unequip);

                Hide();
            };

            SetupSelectionButtons(equipButton, unequipButton, dropButton, closeButton);
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

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            if (_debounced
                && !state.IsOnScreenObject
                && state.Mouse.LeftClicked)
            {
                Hide();
            }

            return base.ProcessMouse(state);
        }
    }
}
