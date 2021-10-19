using Roguelike2.Fonts;
using Roguelike2.Ui.Windows;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System.Linq;

namespace Roguelike2.Maps
{
    public class MapOverlayConsole : Console
    {
        private static readonly Color Highlight = Color.White.SetAlpha(100);
        private static readonly Color StrongHighlight = Color.White.SetAlpha(150);

        private readonly WorldMap _map;
        private readonly DungeonMaster _dm;
        private readonly int _rightPaneWidth;
        private readonly int _bottomRightPaneHeight;
        private Point _cellPosition;

        public MapOverlayConsole(
            int width,
            int height,
            IFont font,
            WorldMap map,
            DungeonMaster dm,
            int rightPaneWidth,
            int bottomRightPaneHeight)
            : base(width, height)
        {
            UseMouse = true;
            FocusOnMouseClick = false;
            Font = font;
            _map = map;
            _dm = dm;
            _rightPaneWidth = rightPaneWidth;
            _bottomRightPaneHeight = bottomRightPaneHeight;
        }

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            if (state.Mouse.RightClicked)
            {
                return HandleRightClick(state);
            }

            return HandleHighlight(state);
        }

        public void Clear()
        {
            Surface.Clear();
        }

        public void DrawHighlight(Point point, Color color)
        {
            Cursor.Position = point;
            var highlight = new ColoredString(((char)WorldGlyphAtlas.Solid).ToString(), color, Color.Transparent);
            Cursor.Print(highlight);
        }

        private bool HandleRightClick(MouseScreenObjectState state)
        {
            if (!state.IsOnScreenObject)
            {
                return true;
            }

            _cellPosition = state.CellPosition;
            Clear();
            DrawHighlight(_cellPosition, StrongHighlight);

            var mapOffset = _map.DefaultRenderer.Surface.View.Position;
            var mapPosition = _cellPosition + mapOffset;

            var tileDetailsWindow = new TileDetailsWindow(_rightPaneWidth, _bottomRightPaneHeight, mapPosition, _map, _dm);
            tileDetailsWindow.Position = new Point(
                (_map.Position.X + _map.DefaultRenderer.WidthPixels) / tileDetailsWindow.Font.GlyphWidth,
                _map.DefaultRenderer.Surface.View.Height - _bottomRightPaneHeight);
            tileDetailsWindow.Closed += (_, __) => Clear();

            tileDetailsWindow.Show(true);
            return true;
        }

        private bool HandleHighlight(MouseScreenObjectState state)
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

            var mapOffset = _map.DefaultRenderer.Surface.View.Position;
            var mapPosition = _cellPosition + mapOffset;
            if (_dm.Player.Position == mapPosition)
            {
                DrawHighlight(_cellPosition, Highlight);
                return true;
            }

            if (!_map.PlayerFOV.CurrentFOV.Contains(mapPosition))
            {
                return true;
            }

            var path = _map.AStar.ShortestPath(_dm.Player.Position, mapPosition);
            foreach (var step in path.Steps)
            {
                DrawHighlight(step - mapOffset, Highlight);
            }

            return true;
        }
    }
}
