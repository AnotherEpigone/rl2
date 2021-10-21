using Roguelike2.Entities;
using Roguelike2.GameMechanics.Combat;
using Roguelike2.GameMechanics.Factions;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Logging;

namespace Roguelike2
{
    public class DungeonMaster : IDungeonMaster
    {
        public DungeonMaster(
            Player player,
            ILogger logger,
            ITimeMaster timeMaster,
            FactionManager factMan,
            HitMan hitMan)
        {
            Player = player;
            Logger = logger;
            TimeMaster = timeMaster;
            FactMan = factMan;
            HitMan = hitMan;
        }

        public Player Player { get; }
        public ILogger Logger { get; }
        public ITimeMaster TimeMaster { get; }
        public FactionManager FactMan { get; }
        public HitMan HitMan { get; }
    }
}
