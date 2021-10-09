using NUnit.Framework;
using Roguelike2.GameMechanics.Factions;
using System.Collections.Generic;

namespace Roguelike2Tests.Serialization
{
    public class FactionSerializationTests
    {
        [Test]
        public void Faction_PropertiesPreserved()
        {
            var faction = new Faction(
                "n00b ID",
                new Dictionary<string, int>
                {
                    { "goblins", -100 },
                    { "kobolds", 99 },
                });

            var sFaction = SerializationTestHelper.SerializeDeserialize(faction);

            Assert.AreEqual("n00b ID", sFaction.Id);
            Assert.AreEqual(-100, sFaction.Attitudes["goblins"]);
            Assert.AreEqual(99, sFaction.Attitudes["kobolds"]);
        }
    }
}
