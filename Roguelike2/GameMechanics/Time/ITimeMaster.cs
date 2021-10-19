using System;
using System.Collections.Generic;

namespace Roguelike2.GameMechanics.Time
{
    public interface ITimeMaster
    {
        McTimeSpan JourneyTime { get; }

        IEnumerable<ITimeMasterNode> Nodes { get; }

        event EventHandler<McTimeSpan> TimeUpdated;

        void Enqueue(ITimeMasterNode node);

        ITimeMasterNode Dequeue();

        void ClearNodes();
    }
}