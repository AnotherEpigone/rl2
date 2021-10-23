using GoRogue.Components.ParentAware;
using Roguelike2.Maps;

namespace Roguelike2.Components.Ai
{
    public interface IAiComponent : IParentAwareComponent
    {
        int Run(WorldMap map, IDungeonMaster dungeonMaster);
    }
}
