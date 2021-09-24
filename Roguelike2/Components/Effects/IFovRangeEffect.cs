using GoRogue.Components.ParentAware;

namespace Roguelike2.Components.Effects
{
    // TODO describable
    public interface IFovRangeEffect : IParentAwareComponent
    {
        int Modifier { get; }
    }
}
