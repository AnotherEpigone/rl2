using GoRogue.Pathing;
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
        private readonly DungeonMaster _dm;
        private Point _cellPosition;

        public MapOverlayConsole(
            int width,
            int height,
            IFont font,
            WorldMap map,
            DungeonMaster dm)
            : base(width, height)
        {
            UseMouse = true;
            FocusOnMouseClick = false;
            Font = font;
            _map = map;
            _dm = dm;
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

            var mapOffset = _map.DefaultRenderer.Surface.View.Position;
            var mapPosition = _cellPosition + mapOffset;
            if (_dm.Player.Position == mapPosition)
            {
                DrawHighlight(_cellPosition);
                return true;
            }

            if (!_map.PlayerFOV.CurrentFOV.Contains(mapPosition))
            {
                return true;
            }

            var path = _map.AStar.ShortestPath(_dm.Player.Position, mapPosition);
            foreach (var step in path.Steps)
            {
                DrawHighlight(step - mapOffset);
            }
            
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
