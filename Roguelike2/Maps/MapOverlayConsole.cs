using Roguelike2.Fonts;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System.Linq;

namespace Roguelike2.Maps
{
    public class MapOverlayConsole : Console
    {
        private readonly WorldMap _map;
        private Point _cellPosition;

        public MapOverlayConsole(int width, int height, IFont font, WorldMap map)
            : base(width, height)
        {
            UseMouse = true;
            FocusOnMouseClick = false;
            Font = font;
            _map = map;
        }

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            var newCellPosition = state.IsOnScreenObject
                ? state.CellPosition
                : Point.None;
            if (newCellPosition == _cellPosition)
            {
                return true;
            }

            _cellPosition = newCellPosition;
            Clear();

            if (_cellPosition == Point.None)
            {
                return true;
            }

            var mapPosition = _cellPosition + _map.DefaultRenderer.Surface.View.Position;
            if (!_map.PlayerFOV.CurrentFOV.Contains(mapPosition))
            {
                return true;
            }

            DrawHighlight(_cellPosition);
            return true;
        }

        public void Clear()
        {
            Surface.Clear();
        }

        public void DrawHighlight(Point point)
        {
            Cursor.Position = point;
            var highlight = new ColoredString(((char)WorldGlyphAtlas.Solid).ToString(), Color.White.SetAlpha(100), Color.Transparent);
            Cursor.Print(highlight);
        }
    }
}
