using Moq;
using NUnit.Framework;
using Roguelike2.Entities;
using Roguelike2.Logging;
using SadRogue.Primitives;

namespace Roguelike2Tests.Serialization
{
    public class PlayerSerializationTests
    {
        [Test]
        public void Default_Preserved()
        {
            var position = new Point(3, 5);
            var player = new Player(position);

            var serializedPlayer = SerializationTestHelper.SerializeDeserialize(player);

            Assert.AreEqual(player.Health, serializedPlayer.Health);
            Assert.AreEqual(player.MaxHealth, serializedPlayer.MaxHealth);
            Assert.AreEqual(player.FactionId, serializedPlayer.FactionId);
            Assert.AreEqual(player.Name, serializedPlayer.Name);
            Assert.AreEqual(player.TemplateId, serializedPlayer.TemplateId);
            Assert.AreEqual(player.Id, serializedPlayer.Id);
            Assert.AreEqual(player.Position, serializedPlayer.Position);
        }

        [Test]
        public void Damaged_HealthPreserved()
        {
            var logger = new Mock<ILogger>(MockBehavior.Loose);
            var position = new Point(3, 5);
            var player = new Player(position);
            player.ApplyDamage(2, logger.Object);

            var serializedPlayer = SerializationTestHelper.SerializeDeserialize(player);

            Assert.AreEqual(player.Health, serializedPlayer.Health);
            Assert.AreEqual(player.MaxHealth, serializedPlayer.MaxHealth);
            Assert.AreEqual(player.FactionId, serializedPlayer.FactionId);
            Assert.AreEqual(player.Name, serializedPlayer.Name);
            Assert.AreEqual(player.TemplateId, serializedPlayer.TemplateId);
            Assert.AreEqual(player.Id, serializedPlayer.Id);
            Assert.AreEqual(player.Position, serializedPlayer.Position);
        }
    }
}
