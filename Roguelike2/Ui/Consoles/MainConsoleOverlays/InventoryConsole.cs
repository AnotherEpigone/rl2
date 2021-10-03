using SadConsole;
using SadRogue.Primitives;
using static SadConsole.ColoredString;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class InventoryConsole : Console
    {
        private readonly DungeonMaster _dm;

        public InventoryConsole(int width, int height, DungeonMaster dm)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;
            UseMouse = true;
            FocusOnMouseClick = false;

            _dm = dm;

            this.Fill(background: ColorHelper.ControlBack);
            DrawOutline();

            var controlsConsole = new InventoryControlsConsole(width - 2, height - 2, dm)
            {
                Position = new Point(1, 1)
            };

            _dm.Player.Inventory.ContentsChanged += Inventory_ContentsChanged;

            Children.Add(controlsConsole);
        }

        private void DrawOutline()
        {
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            var title = $"Carried {_dm.Player.Inventory.FilledCapacity}/{_dm.Player.Inventory.Capacity}";
            Cursor.Position = new Point((Width - title.Length) / 2, 0);
            var coloredTitle = new ColoredString(title, DefaultForeground, DefaultBackground);
            Cursor.Print(coloredTitle);
        }

        private void Inventory_ContentsChanged(object sender, System.EventArgs e)
        {
            DrawOutline();
        }
    }
}
