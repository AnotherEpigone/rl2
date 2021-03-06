using Newtonsoft.Json;
using Roguelike2.Serialization.Maps;
using SadConsole;
using SadConsole.Input;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Roguelike2.Maps
{
    public enum MapLayer
    {
        TERRAIN,
        TERRAINFEATURES,
        IMPROVEMENTS,
        ITEMS,
        ACTORS,
        EFFECTS,
        GUI,
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(WorldMapJsonConverter))]
    public class WorldMap : RogueLikeMap
    {
        private readonly WorldMapRenderer _renderer;

        public WorldMap(int width, int height, IFont font)
            : base(
                  width,
                  height,
                  null,
                  Enum.GetNames(typeof(MapLayer)).Length,
                  Distance.Chebyshev,
                  entityLayersSupportingMultipleItems: GoRogue.SpatialMaps.LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS, (int)MapLayer.ACTORS, (int)MapLayer.EFFECTS, (int)MapLayer.GUI))
        {
            Font = font;

            _renderer = (WorldMapRenderer)CreateRenderer(CreateWorldMapRenderer, new Point(width, height), font, font.GetFontSize(IFont.Sizes.One));
            DefaultRenderer = _renderer;
        }

        public event EventHandler<MouseScreenObjectState> RightMouseClick;
        public event EventHandler<MouseScreenObjectState> LeftMouseClick;

        private string DebuggerDisplay => string.Format($"{nameof(WorldMap)} ({Width}, {Height})");

        public IFont Font { get; }

        public Point MouseCellPosition => _renderer.MouseCellPosition;

        public IScreenSurface CreateMinimapRenderer(int pixelWidth, int pixelHeight, IFont pixelFont)
        {
            var cellSurface = new MapTerrainCellSurface(this, pixelWidth, pixelHeight);
            return new ScreenSurface(cellSurface, pixelFont);
        }

        private IScreenSurface CreateWorldMapRenderer(
            ICellSurface surface,
            IFont font,
            Point? fontSize)
        {
            var renderer = new WorldMapRenderer(surface, font, fontSize.Value);
            renderer.LeftMouseClick += (s, e) => LeftMouseClick?.Invoke(s, e);
            renderer.RightMouseClick += (s, e) => RightMouseClick?.Invoke(s, e);
            renderer.UseMouse = true;
            return renderer;
        }
    }
}
