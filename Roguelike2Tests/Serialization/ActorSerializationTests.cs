using Moq;
using NUnit.Framework;
using Roguelike2.Entities;
using Roguelike2.Logging;
using SadRogue.Primitives;

namespace Roguelike2Tests.Serialization
{
    public class ActorSerializationTests
    {
        [Test]
        public void Default_Preserved()
        {
            var position = new Point(3, 5);
            var goblin = new Actor(position, ActorAtlas.Goblin);

            var serializedGoblin = SerializationTestHelper.SerializeDeserialize(goblin);

            Assert.AreEqual(goblin.Health, serializedGoblin.Health);
            Assert.AreEqual(goblin.MaxHealth, serializedGoblin.MaxHealth);
            Assert.AreEqual(goblin.FactionId, serializedGoblin.FactionId);
            Assert.AreEqual(goblin.Name, serializedGoblin.Name);
            Assert.AreEqual(goblin.TemplateId, serializedGoblin.TemplateId);
            Assert.AreEqual(goblin.Id, serializedGoblin.Id);
            Assert.AreEqual(goblin.Position, serializedGoblin.Position);
        }

        [Test]
        public void Damaged_HealthPreserved()
        {
            var logger = new Mock<ILogger>(MockBehavior.Loose);
            var position = new Point(3, 5);
            var goblin = new Actor(position, ActorAtlas.Goblin);
            goblin.ApplyDamage(2, logger.Object);

            var serializedGoblin = SerializationTestHelper.SerializeDeserialize(goblin);

            Assert.AreEqual(goblin.Health, serializedGoblin.Health);
            Assert.AreEqual(goblin.MaxHealth, serializedGoblin.MaxHealth);
            Assert.AreEqual(goblin.FactionId, serializedGoblin.FactionId);
            Assert.AreEqual(goblin.Name, serializedGoblin.Name);
            Assert.AreEqual(goblin.TemplateId, serializedGoblin.TemplateId);
            Assert.AreEqual(goblin.Id, serializedGoblin.Id);
            Assert.AreEqual(goblin.Position, serializedGoblin.Position);
        }
    }
}
