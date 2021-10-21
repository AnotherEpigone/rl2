using GoRogue.Components.ParentAware;
using GoRogue.DiceNotation;

namespace Roguelike2.Components.Effects
{
    public interface IEquippedMeleeWeaponComponent : IParentAwareComponent
    {
        DiceExpression Damage { get; }

        int SpeedMod { get; }
    }
}
