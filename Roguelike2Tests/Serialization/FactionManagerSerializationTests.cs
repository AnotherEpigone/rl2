using NUnit.Framework;
using Roguelike2.GameMechanics.Factions;

namespace Roguelike2Tests.Serialization
{
    public class FactionManagerSerializationTests
    {
        [Test]
        public void FactionMan_Default_RelationshipsPreserved()
        {
            var factMan = new FactionManager();
            factMan.FactionsById.Add(FactionAtlas.Player.Id, FactionAtlas.Player);
            factMan.FactionsById.Add(FactionAtlas.Goblins.Id, FactionAtlas.Goblins);

            var sFactMan = SerializationTestHelper.SerializeDeserialize(factMan);

            Assert.True(sFactMan.AreEnemies(FactionAtlas.Player.Id, FactionAtlas.Goblins.Id));
        }

        [Test]
        public void FactionMan_Changed_RelationshipsPreserved()
        {
            var factMan = new FactionManager();
            factMan.FactionsById.Add(FactionAtlas.Player.Id, FactionAtlas.Player);
            factMan.FactionsById.Add(FactionAtlas.Goblins.Id, FactionAtlas.Goblins);

            factMan.ChangeAttitude(120, FactionAtlas.Player.Id, FactionAtlas.Goblins.Id);

            var sFactMan = SerializationTestHelper.SerializeDeserialize(factMan);

            Assert.False(sFactMan.AreEnemies(FactionAtlas.Player.Id, FactionAtlas.Goblins.Id));
        }
    }
}
