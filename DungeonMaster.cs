using Roguelike2.Entities;
using Roguelike2.Logging;

namespace Roguelike2
{
    public class DungeonMaster
    {
        public DungeonMaster(
            Player player,
            ILogger logger)
        {
            Player = player;
            Logger = logger;
        }

        public Player Player { get; }
        public ILogger Logger { get; }
    }
}
