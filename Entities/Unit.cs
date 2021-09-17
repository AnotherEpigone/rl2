using Newtonsoft.Json;
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
                string templateId)
            : base(position, glyph, name, walkable, transparent, layer, id)
        {
            EmpireId = empireId;
            TemplateId = templateId;
            Selected = false;

            LastSelected = DateTime.UtcNow;
        }

        public event EventHandler StatsChanged;

        public Guid EmpireId { get; }
        public string TemplateId { get; }
        public bool Selected { get; private set; }

        public DateTime LastSelected { get; private set; }

        private string DebuggerDisplay => $"{nameof(Unit)}: {Name}";

        public UnitMovementResult TryMove(Point target, int movementCost)
        {
            if (!CurrentMap.WalkabilityView[target])
            {
                // detect combat
                var targetUnit = CurrentMap.GetEntityAt<Unit>(target);
                if (targetUnit?.EmpireId != EmpireId)
                {
                    StatsChanged?.Invoke(this, EventArgs.Empty);
                    return UnitMovementResult.Combat;
                }

                return UnitMovementResult.Blocked;
            }

            StatsChanged?.Invoke(this, EventArgs.Empty);

            Position = target;
            return UnitMovementResult.Moved;
        }

        public void MagicMove(Point target)
        {
            Position = target;
        }
    }
}
