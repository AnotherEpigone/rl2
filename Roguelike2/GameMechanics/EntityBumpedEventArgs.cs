using Roguelike2.Entities;
using SadRogue.Primitives;
using System;

namespace Roguelike2.GameMechanics
{
    public enum BumpOutcome
    {
        Melee,
        Interact,
        None,
    }

    public class EntityBumpedEventArgs : EventArgs
    {
        public EntityBumpedEventArgs(Actor bumpingEntity, Point bumpedPosition)
        {
            BumpingEntity = bumpingEntity;
            BumpedPosition = bumpedPosition;
            Outcome = BumpOutcome.None;
        }

        public BumpOutcome Outcome { get; set; }

        public Actor BumpingEntity { get; init; }
        public Point BumpedPosition { get; init; }
    }
}
