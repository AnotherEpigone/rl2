using Newtonsoft.Json;
using Roguelike2.GameMechanics;
using Roguelike2.Logging;
using Roguelike2.Maps;
using Roguelike2.Serialization.Entities;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Roguelike2.Entities
{
    public enum MoveOutcome
    {
        Move,
        NoMove,
        Melee,
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
            foreach (var component in template.CreateComponents())
            {
                AllComponents.Add(component);
            }
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

        public event EventHandler<EntityBumpedEventArgs> Bumped;

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

        public MoveOutcome TryMove(Direction direction)
        {
            // TODO
            //var tiles = SubTiles
            //    .Select(st => st.Position)
            //    .Append(Position);
            var tiles = new List<Point> { Position };

            var bumpedEvents = new List<EntityBumpedEventArgs>();
            foreach (var tile in tiles)
            {
                var target = tile + direction;
                if (tiles.Contains(target))
                {
                    continue;
                }

                if (!CurrentMap.WalkabilityView[target])
                {
                    var bumpedEventArgs = new EntityBumpedEventArgs(this, target);
                    Bumped?.Invoke(this, bumpedEventArgs);

                    bumpedEvents.Add(bumpedEventArgs);
                }
            }

            if (bumpedEvents.Count == 0)
            {
                // TODO
                //BulkMove(SubTiles.Append(this), direction);
                Position += direction;

                return MoveOutcome.Move;
            }

            return bumpedEvents.Any(be => be.Outcome == BumpOutcome.Melee)
                ? MoveOutcome.Melee
                : MoveOutcome.NoMove;
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
                Remove();
            }
        }

        public void ApplyHealing(float healing)
        {
            Health = Math.Min(MaxHealth, Health + healing);
        }

        public void Remove()
        {
            // TODO
            //foreach (var subTile in SubTiles)
            //{
            //    subTile.Remove();
            //}

            CurrentMap.RemoveEntity(this);
        }

        private string DebuggerDisplay => $"{nameof(Actor)}: {Name}";
    }
}
