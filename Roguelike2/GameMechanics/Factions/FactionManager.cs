using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roguelike2.GameMechanics.Factions
{
    [DataContract]
    public class FactionManager
    {
        const int DefaultAttitude = -100;

        public FactionManager()
        {
            Factions = new Dictionary<string, Faction>();
        }

        [DataMember]
        public Dictionary<string, Faction> Factions { get; init; }

        public void ChangeAttitude(int mod, string factionIdA, string factionIdB)
        {
            var factionA = Factions[factionIdA];
            if (!factionA.Attitudes.ContainsKey(factionIdB))
            {
                factionA.Attitudes.Add(factionIdB, DefaultAttitude);
            }

            var factionB = Factions[factionIdB];
            if (!factionB.Attitudes.ContainsKey(factionIdA))
            {
                factionB.Attitudes.Add(factionIdA, DefaultAttitude);
            }

            factionA.Attitudes[factionIdB] += mod;
            factionB.Attitudes[factionIdA] += mod;
        }

        public bool AreEnemies(string factionIdA, string factionIdB)
        {
            if (!Factions[factionIdA].Attitudes.TryGetValue(factionIdB, out int attitudeA))
            {
                attitudeA = DefaultAttitude;
            }

            if (!Factions[factionIdB].Attitudes.TryGetValue(factionIdA, out int attitudeB))
            {
                attitudeB = DefaultAttitude;
            }

            return attitudeA < 0 || attitudeB < 0;
        }
    }
}
