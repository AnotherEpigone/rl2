﻿using Roguelike2.Entities;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Logging;

namespace Roguelike2
{
    public interface IDungeonMaster
    {
        ILogger Logger { get; }
        Player Player { get; }
        ITimeMaster TimeMaster { get; }
    }
}