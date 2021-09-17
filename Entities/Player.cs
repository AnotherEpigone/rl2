using Roguelike2.Fonts;
using Roguelike2.Maps;
using SadRogue.Primitives;
using System;

namespace Roguelike2.Entities
{
    public class Player : Unit
    {
        public Player(
            Point position)
            : base(
                  position,
                  WorldGlyphAtlas.PlayerDefault,
                  "Me",
                  false,
                  true,
                  (int)MapEntityLayer.ACTORS,
                  Guid.NewGuid(),
                  Guid.NewGuid(), // TODO Faction ID
                  "Player")
        {
        }
    }
}
