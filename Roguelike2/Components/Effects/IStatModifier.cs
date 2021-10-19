using GoRogue.Components.ParentAware;

namespace Roguelike2.Components.Effects
{
    public enum Stat
    {
        Speed,
    }

    public interface IStatModifier : IParentAwareComponent
    {
        Stat Stat { get; }

        float Modifier { get; }
    }
}
