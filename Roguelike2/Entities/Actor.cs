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
    [JsonConverter(typeof(ActorJsonConverter))]
    public class Actor : NovaEntity
    {
        public Actor(
                Point position,
                int glyph,
                string name,
                bool walkable,
                bool transparent,
                int layer,
                Guid id,
                string factionId,
                string templateId)
            : base(position, glyph, name, walkable, transparent, layer, id)
        {
            FactionId = factionId;
            TemplateId = templateId;
            Selected = false;

            LastSelected = DateTime.UtcNow;
        }

        public event EventHandler StatsChanged;

        public string FactionId { get; }
        public string TemplateId { get; }
        public bool Selected { get; private set; }

        public DateTime LastSelected { get; private set; }

        public UnitMovementResult TryMove(Point target)
        {
            if (!CurrentMap.WalkabilityView[target])
            {
                // detect combat
                var targetUnit = CurrentMap.GetEntityAt<Actor>(target);
                if (targetUnit?.FactionId != FactionId)
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

        private string DebuggerDisplay => $"{nameof(Actor)}: {Name}";
    }
}
