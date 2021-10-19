using System.Runtime.Serialization;

namespace Roguelike2.GameMechanics.Time.Nodes
{
    [DataContract]
    public class WizardTurnNode : ITimeMasterNode
    {
        public WizardTurnNode(long time)
        {
            Time = time;
        }

        [DataMember]
        public long Time { get; }
    }
}
