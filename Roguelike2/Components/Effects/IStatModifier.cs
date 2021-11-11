using GoRogue.Components.ParentAware;

namespace Roguelike2.Components.Effects
{
    public enum Stat
    {
        WalkSpeed,
        AttackSpeed,
        CastSpeed,
        Deflect,
    }

    public interface IStatModifier : IParentAwareComponent
    {
        Stat Stat { get; }

        float Modifier { get; }
    }
}
