using NUnit.Framework;
using Roguelike2.Components.Effects;
using Roguelike2.GameMechanics.Items;

namespace Roguelike2Tests.Serialization
{
    public class Tests
    {
        [Test]
        public void ItemWithComponents_SerializeDeserialize()
        {
            var item = new Item(ItemAtlas.EtheriumShard);
            item.GoRogueComponents.Add(new HealthRegenEffect(2f));
            item.GoRogueComponents.Add(new FovRangeEffect(3));

            var serializedItem = SerializationTestHelper.SerializeDeserialize(item);

            Assert.AreEqual(2, serializedItem.GoRogueComponents.Count);
            Assert.AreEqual(2f, serializedItem.GoRogueComponents.GetFirst<IHealthRegenEffect>().Value);
            Assert.AreEqual(3, serializedItem.GoRogueComponents.GetFirst<IFovRangeEffect>().Modifier);
            Assert.AreEqual(ItemAtlas.EtheriumShard.Id, serializedItem.TemplateId);
            Assert.AreEqual(ItemAtlas.EtheriumShard.Name, serializedItem.Name);
            Assert.AreEqual(ItemAtlas.EtheriumShard.Glyph, serializedItem.Glyph);
        }
    }
}