using SadConsole;
using SadRogue.Primitives;

namespace Roguelike2.Ui.Consoles.MainConsoleOverlays
{
    public class EquipmentConsole : Console
    {
        private readonly DungeonMaster _dm;

        public EquipmentConsole(int width, int height, DungeonMaster dm)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;
            UseMouse = true;
            FocusOnMouseClick = false;

            _dm = dm;

            this.Fill(background: ColorHelper.ControlBack);
            DrawOutline();
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
    }
}
