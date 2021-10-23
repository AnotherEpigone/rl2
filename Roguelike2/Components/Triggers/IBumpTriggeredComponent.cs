using GoRogue.Components.ParentAware;
using Roguelike2.Entities;

namespace Roguelike2.Components.Triggers
{
    public interface IBumpTriggeredComponent : IParentAwareComponent
    {
        void Bump(Actor bumpingEntity, IDungeonMaster dungeonMaster);
    }
}
