using GoRogue.Components.ParentAware;
using GoRogue.DiceNotation;
using System.Runtime.Serialization;

namespace Roguelike2.Components.ItemComponents
{
    [DataContract]
    public class EquippedMeleeWeaponComponent : IEquippedMeleeWeaponComponent
    {
        public EquippedMeleeWeaponComponent(string damage, int speedMod)
        {
            DamageExpression = damage;
            SpeedMod = speedMod;
        }

        public DiceExpression Damage => Dice.Parse(DamageExpression);

        [DataMember]
        public string DamageExpression { get; set; }

        [DataMember]
        public int SpeedMod { get; set; }

        public IObjectWithComponents Parent { get; set; }

        public string GetDescription() => $"Melee damage {DamageExpression}";
    }
}
