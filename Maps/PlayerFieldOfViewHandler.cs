using SadRogue.Integration;
using SadRogue.Integration.FieldOfView;
using SadRogue.Primitives;
using System;

namespace Roguelike2.Maps
{
    public class PlayerFieldOfViewHandler : FieldOfViewHandlerBase
    {
        protected override void UpdateEntitySeen(RogueLikeEntity entity) => entity.IsVisible = true;

        protected override void UpdateEntityUnseen(RogueLikeEntity entity) => entity.IsVisible = false;

        protected override void UpdateTerrainSeen(RogueLikeCell terrain)
        {
            if (terrain is not Terrain rlTerrain)
            {
                throw new ArgumentException($"Unsupported terrain type {terrain.GetType().FullName}");
            }

            rlTerrain.Explored = true;
            rlTerrain.Appearance.IsVisible = true;
            rlTerrain.Appearance.Foreground = Color.White;
        }

        protected override void UpdateTerrainUnseen(RogueLikeCell terrain)
        {
            if (terrain is not Terrain rlTerrain)
            {
                throw new ArgumentException($"Unsupported terrain type {terrain.GetType().FullName}");
            }

            if (rlTerrain.Explored)
            {
                rlTerrain.Appearance.IsVisible = true;
                rlTerrain.Appearance.Foreground = new Color(200, 200, 200);
            }
            else
            {
                rlTerrain.Appearance.IsVisible = false;
            }
        }
    }
}
