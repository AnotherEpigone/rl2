using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Roguelike2.GameMechanics.Factions
{
    [DataContract]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class Faction
    {
        public Faction(string id, Dictionary<string, int> attitudes)
        {
            Id = id;
            Attitudes = attitudes;
        }

        [DataMember]
        public string Id { get; init; }

        [DataMember]
        public Dictionary<string, int> Attitudes { get; init; }

        private string DebuggerDisplay => nameof(Faction);
    }
}
