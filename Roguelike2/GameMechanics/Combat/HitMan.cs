using Roguelike2.Components.Effects;
using Roguelike2.Components.ItemComponents;
using Roguelike2.Entities;
using System;
using System.Linq;
using Troschuetz.Random;

namespace Roguelike2.GameMechanics.Combat
{
    public enum AttackResult
    {
        Crit,
        Hit,
        Glance,
        Miss,
    }

    public class HitMan
    {
        private const int BaseCritChance = 1;
        private const int BaseHitChance = 100;
        private const int BaseGlanceChance = 10;
        private const int BaseMissChance = 100;

        private readonly IGenerator _rng;

        public HitMan(IGenerator rng)
        {
            _rng = rng;
        }

        public AttackResult GetAttackResult(Actor attacker, Actor defender)
        {
            var glanceChance = BaseGlanceChance;
            foreach (var deflectEffect in defender.AllComponents.GetAll<IStatModifier>().Where(c => c.Stat == Stat.Deflect))
            {
                glanceChance += (int)deflectEffect.Modifier;
            }

            glanceChance = Math.Max(0, glanceChance);

            var totalChance = BaseHitChance + BaseMissChance;
            var result = _rng.Next(totalChance);
            if (result < BaseCritChance)
            {
                return AttackResult.Crit;
            }

            var hitThreshold = BaseHitChance - glanceChance;
            if (result < hitThreshold)
            {
                return AttackResult.Hit;
            }

            var glanceThreshold = BaseHitChance;
            if (result < glanceThreshold)
            {
                return AttackResult.Glance;
            }

            return AttackResult.Miss;
        }

        public int GetDeflect(Actor entity)
        {
            var deflect = BaseGlanceChance;
            foreach (var deflectEffect in entity.AllComponents.GetAll<IStatModifier>().Where(c => c.Stat == Stat.Deflect))
            {
                deflect += (int)deflectEffect.Modifier;
            }

            return Math.Max(0, deflect);
        }

        public float GetDamage(Actor attacker, AttackResult attackResult)
        {
            var baseDamage = attacker.UnarmedMelee;
            var weapon = attacker.AllComponents.GetFirstOrDefault<IEquippedMeleeWeaponComponent>();
            if (weapon != null)
            {
                baseDamage = weapon.Damage.Roll(_rng);
            }

            var multiplier = attackResult switch
            {
                AttackResult.Hit => 1f,
                AttackResult.Crit => 2f,
                AttackResult.Glance => 0.25f,
                AttackResult.Miss => 0f,
                _ => throw new NotImplementedException(),
            };

            return baseDamage * multiplier;
        }
    }
}
