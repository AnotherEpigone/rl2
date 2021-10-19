using Roguelike2.Entities;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Logging;

namespace Roguelike2
{
    public class DungeonMaster : IDungeonMaster
    {
        public DungeonMaster(
            Player player,
            ILogger logger,
            ITimeMaster timeMaster)
        {
            Player = player;
            Logger = logger;
            TimeMaster = timeMaster;
        }

        public Player Player { get; }
        public ILogger Logger { get; }
        public ITimeMaster TimeMaster { get; }
    }
}
