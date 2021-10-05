using NUnit.Framework;
using Roguelike2.Components;
using Roguelike2.GameMechanics.Items;
using System.Linq;

namespace Roguelike2Tests.Serialization
{
    public class InventoryComponentSerializationTests
    {
        [Test]
        public void Empty_CapacityPreserved()
        {
            var component = new InventoryComponent(5);

            var serializedComponent = SerializationTestHelper.SerializeDeserialize(component);

            Assert.AreEqual(5, serializedComponent.Capacity);
            Assert.NotNull(serializedComponent.Items);
            Assert.AreEqual(0, serializedComponent.Items.Count);
        }

        [Test]
        public void WithItems_Success()
        {
            var item1 = new Item(ItemAtlas.StarterOakStaff);
            var item2 = new Item(ItemAtlas.StarterHomespunCloak);
            var component = new InventoryComponent(5, item1, item2);

            var serializedComponent = SerializationTestHelper.SerializeDeserialize(component);

            Assert.AreEqual(5, serializedComponent.Capacity);
            Assert.NotNull(serializedComponent.Items);
            Assert.AreEqual(2, serializedComponent.Items.Count);
            Assert.NotNull(serializedComponent.GetItems().Where(i => i.TemplateId == ItemAtlas.StarterOakStaff.Id).Single());
            Assert.NotNull(serializedComponent.GetItems().Where(i => i.TemplateId == ItemAtlas.StarterHomespunCloak.Id).Single());
        }
    }
}
