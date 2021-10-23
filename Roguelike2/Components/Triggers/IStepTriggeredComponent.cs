using GoRogue.Components.ParentAware;
using Roguelike2.Entities;

namespace Roguelike2.Components.Triggers
{
    public interface IStepTriggeredComponent : IParentAwareComponent
    {
        void OnStep(Actor steppingEntity, IDungeonMaster dungeonMaster);
    }
}
