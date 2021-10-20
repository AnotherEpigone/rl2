using Roguelike2.Entities;
using Roguelike2.GameMechanics.Items;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Text;
using Roguelike2.Ui.Controls;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike2.Ui.Windows
{
    public class ItemStackInteractWindow : NovaControlWindow
    {
        private const int DefaultWidth = 30;

        // 5 top (2 static buttons, 2 space, 1 border), 2 bottom (1 space, 1 border)
        private const int DefaultHeightPadding = 7;

        private readonly ItemStackEntity _itemStack;
        private readonly DungeonMaster _dm;

        private bool _debounced;

        public ItemStackInteractWindow(ItemStackEntity itemStack, DungeonMaster dm, TurnManager turnManager)
            : base(DefaultWidth, itemStack.Items.Count + DefaultHeightPadding)
        {
            CloseOnEscKey = false; // needs to be debounced
            IsModalDefault = true;
            Title = "Item stack";

            _itemStack = itemStack;
            _dm = dm;

            var background = new Console(Width, Height);
            background.Fill(null, ColorHelper.DarkGreyHighlight, null);

            Children.Add(background);

            var buttonTextWidth = Width - 4;

            const string closeButtonText = "Close";
            var closeButton = new NovaSelectionButton(buttonTextWidth, 1)
            {
                Text = closeButtonText,
                Position = new Point(Width / 2 - buttonTextWidth / 2, 2),
            };
            closeButton.Click += (_, __) =>
            {
                if (_debounced)
                {
                    Hide();
                }
            };

            const string takeAllButtonText = "Take all";
            var takeAllButton = new NovaSelectionButton(buttonTextWidth, 1)
            {
                Text = takeAllButtonText,
                Position = new Point(Width / 2 - buttonTextWidth / 2, 3),
                IsEnabled = dm.Player.Inventory.EmptyCapacity >= itemStack.Items.Count,
            };
            takeAllButton.Click += (_, __) =>
            {
                foreach (var item in itemStack.Items.ToArray()) // toarray avoids collection modified during enumeration
                {
                    TakeItem(item);
                }

                RefreshStackStatus();

                turnManager.PostProcessPlayerTurn(TimeHelper.Interact);

                Hide();
            };

            var yCount = 0;
            var buttons = new List<NovaSelectionButton>() { closeButton, takeAllButton };
            foreach (var item in itemStack.Items)
            {
                var button = new NovaSelectionButton(buttonTextWidth, 1)
                {
                    Text = TextHelper.TruncateString(item.Name, buttonTextWidth - 4),
                    Position = new Point(2, 5 + yCount++),
                };
                button.Click += (_, __) =>
                {
                    TakeItem(item);
                    RefreshStackStatus();

                    turnManager.PostProcessPlayerTurn(TimeHelper.Interact);

                    Hide();
                };
                buttons.Add(button);
            }

            SetupSelectionButtons(buttons.ToArray());
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

        private void TakeItem(Item item)
        {
            _dm.Player.Inventory.AddItem(item, _dm);
            _itemStack.Items.Remove(item);
            _dm.Logger.Gameplay($"Picked up {item.Name}.");
        }

        private void RefreshStackStatus()
        {
            if (_itemStack.Items.Count == 1)
            {
                // switch back to single item
                var map = _itemStack.CurrentMap;
                map.RemoveEntity(_itemStack);
                var remainingItem = _itemStack.Items.Single();
                var remainingItemEntity = new ItemEntity(_itemStack.Position, remainingItem);
                map.AddEntity(remainingItemEntity);
                return;
            }

            if (_itemStack.Items.Count == 0)
            {
                var map = _itemStack.CurrentMap;
                map.RemoveEntity(_itemStack);
            }
        }
    }
}
