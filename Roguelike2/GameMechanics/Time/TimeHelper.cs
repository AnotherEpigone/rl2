using Roguelike2.Components.Effects;
using Roguelike2.Entities;
using System;
using System.Linq;

namespace Roguelike2.GameMechanics.Time
{
    public class TimeHelper
    {
        public const int Walk = 100;
        public const int Wait = 100;
        public const int Attack = 200;
        public const int Interact = 50;
        public const int Unequip = 50;
        public const int Equip = 100;

        public static float GetWalkSpeed(NovaEntity entity)
        {
            // TODO walk speed
            //var speed = entity.GetGoRogueComponent<IActorStatComponent>()?.WalkSpeed ?? 1;
            var speed = 1;
            var modifiers = entity.AllComponents.GetAll<IStatModifier>().Where(c => c.Stat == Stat.WalkSpeed);
        
            var negativeModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m < 0)
                .Sum(m => m);
        
            var positiveModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m > 0)
                .Sum(m => m);
        
            return speed * positiveModifier * negativeModifier;
        }

        public static int GetWalkTime(NovaEntity entity)
        {
            var effectiveSpeed = GetWalkSpeed(entity);

            return (int)(Walk / effectiveSpeed);
        }

        public static float GetAttackSpeed(Actor entity)
        {
            // TODO
            //var speed = entity.GetGoRogueComponent<IActorStatComponent>()?.AttackSpeed ?? 1;
            var speed = 1;
            var modifiers = entity.AllComponents.GetAll<IStatModifier>().Where(c => c.Stat == Stat.AttackSpeed);
        
            var negativeModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m < 0)
                .Sum(m => m);
            negativeModifier = Math.Max(0.1f, negativeModifier);
        
            var positiveModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m > 0)
                .Sum(m => m);
        
            return speed * positiveModifier * negativeModifier;
        }
        
        public static int GetAttackTime(Actor entity)
        {
            var effectiveSpeed = GetAttackSpeed(entity);
        
            return (int)(Attack / effectiveSpeed);
        }

        //public static float GetCastSpeed(McEntity entity)
        //{
        //    var speed = entity.GetGoRogueComponent<IActorStatComponent>()?.CastSpeed ?? 1;
        //    var modifiers = entity.GetGoRogueComponents<ISpeedModifier>();
        //
        //    var negativeModifier = 1 + modifiers
        //        .Select(m => m.Modifier)
        //        .Where(m => m < 0)
        //        .Sum(m => m);
        //
        //    var positiveModifier = 1 + modifiers
        //        .Select(m => m.Modifier)
        //        .Where(m => m > 0)
        //        .Sum(m => m);
        //
        //    return speed * positiveModifier * negativeModifier;
        //}
        //
        //public static int GetCastTime(McEntity entity, SpellTemplate spell)
        //{
        //    var effectiveSpeed = GetCastSpeed(entity);
        //
        //    return (int)(spell.BaseCastTime / effectiveSpeed);
        //}
    }
}
