using GoRogue.Components.ParentAware;
using Roguelike2.GameMechanics.Time;
using System.Collections.Generic;

namespace Roguelike2.Components.Effects
{
    public interface ITimedEffect : IParentAwareComponent
    {
        void OnTick(McTimeSpan time, IDungeonMaster dungeonMaster);
        IEnumerable<object> ToArray();
    }
}
