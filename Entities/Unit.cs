using GoRogue.GameFramework;
using Newtonsoft.Json;
using Roguelike2.Fonts;
using Roguelike2.Maps;
using Roguelike2.Serialization.Entities;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Roguelike2.Entities
{
    public enum UnitMovementResult
    {
        Moved,
        NoMovement,
        Combat,
        Blocked,
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(UnitJsonConverter))]
    public class Unit : NovaEntity
    {
        private float _remainingHealth;

        public Unit(
                Point position,
                int glyph,
                string name,
                bool walkable,
                bool transparent,
                int layer,
                Guid id,
                Guid empireId,
                Color empireColor,
                string templateId,
                float movement,
                int maxHealth,
                int strength)
            : base(position, glyph, name, walkable, transparent, layer, id)
        {
            EmpireId = empireId;
            EmpireColor = empireColor;
            TemplateId = templateId;
            Movement = movement;
            RemainingMovement = movement;
            MaxHealth = maxHealth;
            Strength = strength;
            RemainingHealth = maxHealth;
            Selected = false;

            LastSelected = DateTime.UtcNow;
        }

        public event EventHandler StatsChanged;

        public Guid EmpireId { get; }
        public Color EmpireColor { get; }
        public string TemplateId { get; }
        public float Movement { get; }
        public float RemainingMovement { get; set; }
        public int MaxHealth { get; }
        public float RemainingHealth
        {
            get => _remainingHealth;
            set
            {
                _remainingHealth = value;
                StatsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public int Strength { get; }
        public bool Selected { get; private set; }

        public DateTime LastSelected { get; private set; }
        public int EffectiveStrength => Strength - (int)((MaxHealth - RemainingHealth) / MaxHealth * Strength * 0.8);

        private string DebuggerDisplay => $"{nameof(Unit)}: {Name}";

        public UnitMovementResult TryMove(Point target, int movementCost)
        {
            if (RemainingMovement < 0.01)
            {
                return UnitMovementResult.NoMovement;
            }

            if (!CurrentMap.WalkabilityView[target])
            {
                // detect combat
                var targetUnit = CurrentMap.GetEntityAt<Unit>(target);
                if (targetUnit?.EmpireId != EmpireId)
                {
                    RemainingMovement = Math.Max(0, RemainingMovement - movementCost);
                    StatsChanged?.Invoke(this, EventArgs.Empty);
                    return UnitMovementResult.Combat;
                }

                return UnitMovementResult.Blocked;
            }

            RemainingMovement = Math.Max(0, RemainingMovement - movementCost);
            StatsChanged?.Invoke(this, EventArgs.Empty);

            Position = target;
            return UnitMovementResult.Moved;
        }

        public void MagicMove(Point target)
        {
            Position = target;
        }

        public bool HasMovement() => RemainingMovement > 0.01;
    }
}
