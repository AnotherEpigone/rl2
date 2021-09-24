using Newtonsoft.Json;
using NUnit.Framework;
using Roguelike2.Components.Effects;
using Roguelike2.GameMechanics.Items;
using Roguelike2.Serialization;

namespace Roguelike2Tests
{
    public class Tests
    {
        [Test]
        public void ItemWithComponents_SerializeDeserialize()
        {
            var item = new Item();
            item.GoRogueComponents.Add(new HealthRegenEffect(2f));
            item.GoRogueComponents.Add(new FovRangeEffect(3));

            var settings = new SaveManager().JsonSettings;
            var payload = JsonConvert.SerializeObject(item, settings);
            var serializedItem = JsonConvert.DeserializeObject<Item>(payload, settings);

            Assert.AreEqual(2, serializedItem.GoRogueComponents.Count);
            Assert.AreEqual(2f, serializedItem.GoRogueComponents.GetFirst<IHealthRegenEffect>().Value);
            Assert.AreEqual(3, serializedItem.GoRogueComponents.GetFirst<IFovRangeEffect>().Modifier);
        }
    }
}