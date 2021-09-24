using GoRogue.Components.ParentAware;

namespace Roguelike2.Components.Effects
{
    public interface IHealthRegenEffect : IParentAwareComponent
    {
        float Value { get; }
    }
}
