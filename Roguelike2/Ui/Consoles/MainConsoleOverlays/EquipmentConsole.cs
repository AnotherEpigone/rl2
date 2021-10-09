using Roguelike2.Ui.Windows;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using System.Linq;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class EquipmentConsole : Console
    {
        private readonly DungeonMaster _dm;
        private readonly NovaControlsConsole _buttonConsole;

        public EquipmentConsole(int width, int height, DungeonMaster dm)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;
            UseMouse = true;
            FocusOnMouseClick = false;

            _dm = dm;

            _buttonConsole = new NovaControlsConsole(width - 2, height - 2)
            {
                FocusOnMouseClick = false,
                Position = new Point(1, 1),
            };
            Children.Add(_buttonConsole);

            RefreshEquipment();
            DrawOutline();

            _dm.Player.Equipment.EquipmentChanged += Equipment_EquipmentChanged;
        }

        private void DrawOutline()
        {
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            var title = $"Equipment";
            Cursor.Position = new Point((Width - title.Length) / 2, 0);
            var coloredTitle = new ColoredString(title, DefaultForeground, DefaultBackground);
            Cursor.Print(coloredTitle);
        }

        private void RefreshEquipment()
        {
            Surface.Clear();
            Cursor.Position = new Point(0, 1);

            _buttonConsole.Controls.Clear();

            var categories = _dm.Player.Equipment.Equipment.Values.ToList();
            foreach (var category in categories)
            {
                Cursor.Right(2).Print(new ColoredString(category.Name + ":\r\n", DefaultForeground, DefaultBackground));

                var printedItems = 0;
                foreach (var item in category.Items)
                {
                    printedItems++;
                    var itemButton = new Button(item.Name.Length)
                    {
                        Text = item.Name,
                        Position = new Point(4, Cursor.Row - 1),
                        Theme = ThemeHelper.ButtonThemeNoEnds(),
                    };
                    itemButton.Click += (_, __) =>
                    {
                        var detailWindow = new ItemDetailsWindow(Width, Height + 2, item, _dm, true)
                        {
                            Position = new Point(Width + 2, Position.Y),
                        };
                        detailWindow.Show(true);
                    };

                    _buttonConsole.Controls.Add(itemButton);
                    Cursor.Print("\r\n");
                }

                for (int i = printedItems; i < category.Slots; i++)
                {
                    Cursor.Right(4).Print(new ColoredString("--\r\n", DefaultForeground, DefaultBackground));
                }
            }
        }

        private void Equipment_EquipmentChanged(object sender, System.EventArgs e)
        {
            RefreshEquipment();
            DrawOutline();
        }
    }
}
