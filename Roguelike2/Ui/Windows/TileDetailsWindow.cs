using Roguelike2.Entities;
using Roguelike2.Maps;
using Roguelike2.Ui.Controls;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Windows
{
    public class TileDetailsWindow : NovaControlWindow
    {
        private bool _debounced;

        public TileDetailsWindow(
            int width,
            int height,
            Point tilePosition,
            WorldMap map,
            DungeonMaster dm)
            : base(width, height)
        {
            CloseOnEscKey = false; // needs to be debounced
            IsModalDefault = true;
            Title = "Tile details";

            var background = new Console(Width, Height);

            var y = 2;
            var positionDetails = new ColoredString($"Position: ({tilePosition.X}, {tilePosition.Y})", DefaultForeground, DefaultBackground);
            background.Surface.Print(2, y++, positionDetails);

            var terrain = map.GetTerrainAt<Terrain>(tilePosition);
            var terrainDetails = new ColoredString($"Terrain: {terrain.Name}", DefaultForeground, DefaultBackground);
            background.Surface.Print(2, y++, terrainDetails);

            y++;
            var actor = map.GetEntityAt<Actor>(tilePosition);
            if (actor != null)
            {
                var actorName = new ColoredString(actor.Name, DefaultForeground, DefaultBackground);
                background.Surface.Print(2, y, actorName);
                y += 2;
            }

            var item = map.GetEntityAt<ItemEntity>(tilePosition);
            if (item != null)
            {
                var itemName = new ColoredString(item.Name, DefaultForeground, DefaultBackground);
                background.Surface.Print(2, y, itemName);
                y += 2;
            }

            var itemStack = map.GetEntityAt<ItemStackEntity>(tilePosition);
            if (itemStack != null)
            {
                background.Surface.Print(2, y++, new ColoredString("Item stack:", DefaultForeground, DefaultBackground));
                foreach (var stackedItem in itemStack.Items)
                {
                    var itemName = new ColoredString(stackedItem.Name, DefaultForeground, DefaultBackground);
                    background.Surface.Print(4, y++, itemName);
                }

                y += 2;
            }

            Children.Add(background);

            const int buttonWidth = 12;
            var closeButton = new NovaSelectionButton(buttonWidth, 1)
            {
                Text = "Close",
                Position = new Point(Width / 2 - buttonWidth / 2, y + 1),
            };
            closeButton.Click += (_, __) =>
            {
                if (_debounced)
                {
                    Hide();
                }
            };

            SetupSelectionButtons(closeButton);
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
                &&!state.IsOnScreenObject
                && state.Mouse.LeftClicked)
            {
                Hide();
            }

            return base.ProcessMouse(state);
        }
    }
}
