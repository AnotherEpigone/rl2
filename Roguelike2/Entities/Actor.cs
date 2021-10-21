using Newtonsoft.Json;
using Roguelike2.Logging;
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
    [JsonConverter(typeof(ActorJsonConverter))]
    public class Actor : NovaEntity
    {
        private const float DeadThreshold = 0.001f;
        private float _health;

        public Actor(Point position, ActorTemplate template)
            : this(
                  position,
                  template.Glyph,
                  template.Name,
                  false,
                  true,
                  (int)MapLayer.ACTORS,
                  Guid.NewGuid(),
                  template.FactionId,
                  template.UnarmedMelee,
                  template.Health,
                  template.Health,
                  template.Id)
        {
        }

        public Actor(
                Point position,
                int glyph,
                string name,
                bool walkable,
                bool transparent,
                int layer,
                Guid id,
                string factionId,
                int unarmedMelee,
                int maxHealth,
                float health,
                string templateId)
            : base(position, glyph, name, walkable, transparent, layer, id)
        {
            FactionId = factionId;
            UnarmedMelee = unarmedMelee;
            MaxHealth = maxHealth;
            TemplateId = templateId;
            Selected = false;

            _health = health;
        }

        /// <summary>
        /// e = previous health
        /// </summary>
        public event EventHandler<float> HealthChanged;

        public string FactionId { get; }
        public int UnarmedMelee { get; }
        public int MaxHealth { get; }
        public string TemplateId { get; }
        public bool Selected { get; private set; }
        public bool Dead => Health < DeadThreshold;

        public float Health
        {
            get { return _health; }
            private set
            {
                if (value == _health)
                {
                    return;
                }

                var prevHealth = _health;
                _health = value;
                HealthChanged?.Invoke(this, prevHealth);
            }
        }

        public UnitMovementResult TryMove(Point target)
        {
            if (!CurrentMap.WalkabilityView[target])
            {
                // detect combat
                //var targetUnit = CurrentMap.GetEntityAt<Actor>(target);
                //if (check the faction manager here)
                //{
                //    return UnitMovementResult.Combat;
                //}

                return UnitMovementResult.Blocked;
            }

            Position = target;
            return UnitMovementResult.Moved;
        }

        public void MagicMove(Point target)
        {
            Position = target;
        }

        public void ApplyDamage(float damage, ILogger logger)
        {
            Health = Math.Max(0, Health - damage);
            if (Dead)
            {
                logger.Gameplay($"{Name} was slain.");
                CurrentMap.RemoveEntity(this);
            }
        }

        public void ApplyHealing(float healing)
        {
            Health = Math.Min(MaxHealth, Health + healing);
        }

        private string DebuggerDisplay => $"{nameof(Actor)}: {Name}";
    }
}
