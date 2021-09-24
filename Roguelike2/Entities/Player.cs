using Newtonsoft.Json;
using Roguelike2.Fonts;
using Roguelike2.Maps;
using Roguelike2.Serialization.Entities;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Roguelike2.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(PlayerJsonConverter))]
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
            Moved += Player_Moved;
        }

        public int FovRadius => 8;

        public void CalculateFov()
        {
            CurrentMap?.PlayerFOV.Calculate(Position, FovRadius, CurrentMap.DistanceMeasurement);
        }

        private void Player_Moved(object sender, GoRogue.GameFramework.GameObjectPropertyChanged<Point> e)
        {
            CalculateFov();
        }

        private string DebuggerDisplay => nameof(Player);
    }
}
