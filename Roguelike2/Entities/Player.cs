using Newtonsoft.Json;
using Roguelike2.Components;
using Roguelike2.Fonts;
using Roguelike2.GameMechanics.Factions;
using Roguelike2.GameMechanics.Items;
using Roguelike2.Maps;
using Roguelike2.Serialization.Entities;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Roguelike2.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(PlayerJsonConverter))]
    public class Player : Actor
    {
        public Player(
            Point position)
            : base(
                  position,
                  WorldGlyphAtlas.PlayerDefault,
                  "Me",
                  false,
                  true,
                  (int)MapLayer.ACTORS,
                  Guid.NewGuid(),
                  FactionAtlas.Player.Id,
                  "Player")
        {
            Moved += Player_Moved;

            Inventory = new InventoryComponent(5);
            AllComponents.Add(Inventory);

            Equipment = new EquipmentComponent(new EquipCategory[]
            {
                new EquipCategory(EquipCategoryId.Staff, "Staff", 1),
                new EquipCategory(EquipCategoryId.Weapon, "Weapon", 1),
                new EquipCategory(EquipCategoryId.Cloak, "Cloak", 1),
                new EquipCategory(EquipCategoryId.Trinket, "Trinket", 1),
                new EquipCategory(EquipCategoryId.Pack, "Pack", 1),
            });
            AllComponents.Add(Equipment);
        }

        public Player(PlayerSerialized serialized)
            : base(
                  serialized.Position,
                  WorldGlyphAtlas.PlayerDefault,
                  "Me",
                  false,
                  true,
                  (int)MapLayer.ACTORS,
                  Guid.NewGuid(),
                  FactionAtlas.Player.Id, // TODO Faction ID
                  "Player")
        {
            foreach (var component in serialized.Components)
            {
                AllComponents.Add(component);
            }

            Inventory = AllComponents.GetFirst<IInventoryComponent>();
            Equipment = AllComponents.GetFirst<IEquipmentComponent>();
        }

        public int FovRadius => 8;

        public IInventoryComponent Inventory { get; }

        public IEquipmentComponent Equipment { get; }

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
