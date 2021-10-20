using Roguelike2.GameMechanics.Items;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Text;
using Roguelike2.Ui.Controls;
using Roguelike2.Ui.Windows;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class InventoryControlsConsole : NovaControlsConsole
    {
        private readonly DungeonMaster _dm;
        private readonly TurnManager _turnManager;

        public InventoryControlsConsole(
            int width,
            int height,
            DungeonMaster dm,
            TurnManager turnManager)
            : base(width, height)
        {
            UseMouse = true;
            FocusOnMouseClick = false;

            _dm = dm;
            _turnManager = turnManager;
            _dm.Player.Inventory.ContentsChanged += Inventory_ContentsChanged;
        }

        private void OnItemSelected(Item item)
        {
            var detailWindow = new ItemDetailsWindow(Width + 2, Height + 2, item, _dm, _turnManager, false)
            {
                Position = new Point(Width + 2, Parent.Position.Y),
            };
            detailWindow.Show(true);
        }

        private void RefreshControls(IEnumerable<Item> items)
        {
            Controls.Clear();

            var yCount = 0;
            var itemButtons = items.ToDictionary(
                i =>
                {
                    var button = new NovaSelectionButton(Width, 1)
                    {
                        Text = TextHelper.TruncateString(i.Name, Width - 5),
                        Position = new Point(0, yCount++),
                    };
                    button.Click += (_, __) => OnItemSelected(i);
                    return button;
                },
                i => (System.Action)(() => { }));

            SetupSelectionButtons(itemButtons);
        }

        private void Inventory_ContentsChanged(object sender, System.EventArgs e)
        {
            RefreshControls(_dm.Player.Inventory.GetItems());
        }
    }
}
