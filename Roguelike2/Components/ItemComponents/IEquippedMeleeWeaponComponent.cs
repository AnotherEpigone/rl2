using GoRogue.Components.ParentAware;
using GoRogue.DiceNotation;

namespace Roguelike2.Components.ItemComponents
{
    public interface IEquippedMeleeWeaponComponent : IParentAwareComponent
    {
        DiceExpression Damage { get; }

        int SpeedMod { get; }
    }
}
