using GoRogue.Components.ParentAware;

namespace Roguelike2.Components.Effects
{
    public enum Stat
    {
        Speed,
        Deflect,
    }

    public interface IStatModifier : IParentAwareComponent
    {
        Stat Stat { get; }

        float Modifier { get; }
    }
}
