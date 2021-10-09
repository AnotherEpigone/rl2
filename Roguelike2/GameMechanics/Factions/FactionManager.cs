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
            FactionsById = new Dictionary<string, Faction>();
        }

        [DataMember]
        public Dictionary<string, Faction> FactionsById { get; init; }

        public void ChangeAttitude(int mod, string factionIdA, string factionIdB)
        {
            var factionA = FactionsById[factionIdA];
            if (!factionA.Attitudes.ContainsKey(factionIdB))
            {
                factionA.Attitudes.Add(factionIdB, DefaultAttitude);
            }

            var factionB = FactionsById[factionIdB];
            if (!factionB.Attitudes.ContainsKey(factionIdA))
            {
                factionB.Attitudes.Add(factionIdA, DefaultAttitude);
            }

            factionA.Attitudes[factionIdB] += mod;
            factionB.Attitudes[factionIdA] += mod;
        }

        public bool AreEnemies(string factionIdA, string factionIdB)
        {
            if (!FactionsById[factionIdA].Attitudes.TryGetValue(factionIdB, out int attitudeA))
            {
                attitudeA = DefaultAttitude;
            }

            if (!FactionsById[factionIdB].Attitudes.TryGetValue(factionIdA, out int attitudeB))
            {
                attitudeB = DefaultAttitude;
            }

            return attitudeA < 0 || attitudeB < 0;
        }
    }
}
