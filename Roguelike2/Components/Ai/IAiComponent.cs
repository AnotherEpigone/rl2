using GoRogue.Components.ParentAware;
using Roguelike2.Maps;

namespace Roguelike2.Components.Ai
{
    public interface IAiComponent : IParentAwareComponent
    {
        (bool success, int ticks) Run(WorldMap map, IDungeonMaster dungeonMaster);
    }
}
