using Roguelike2.Entities;
using Roguelike2.Logging;

namespace Roguelike2
{
    public interface IDungeonMaster
    {
        ILogger Logger { get; }
        Player Player { get; }
    }
}