using GoRogue.Components.ParentAware;
using Roguelike2.Entities;

namespace Roguelike2.Components.Triggers
{
    public interface IInteractTriggeredComponent : IParentAwareComponent
    {
        void Interact(Actor interactingEntity, IDungeonMaster dungeonMaster);
    }
}
