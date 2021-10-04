using Moq;
using NUnit.Framework;
using Roguelike2;
using Roguelike2.Components;
using Roguelike2.GameMechanics.Items;
using Roguelike2.Logging;

namespace Roguelike2Tests.Serialization
{
    public class EquipmentComponentSerializationTests
    {
        [Test]
        public void ComponentWithSlots_SlotsPreserved()
        {
            var component = new EquipmentComponent(new EquipCategory[]
            {
                new EquipCategory(EquipCategoryId.Staff, "Staff", 1),
                new EquipCategory(EquipCategoryId.Weapon, "Weapon", 1),
                new EquipCategory(EquipCategoryId.Cloak, "Cloak", 1),
                new EquipCategory(EquipCategoryId.Trinket, "Trinket", 1),
            });

            var serializedComponent = SerializationTestHelper.SerializeDeserialize(component);

            Assert.AreEqual(4, serializedComponent.Equipment.Count);
            Assert.True(serializedComponent.Equipment.ContainsKey(EquipCategoryId.Staff));
            Assert.True(serializedComponent.Equipment.ContainsKey(EquipCategoryId.Weapon));
            Assert.True(serializedComponent.Equipment.ContainsKey(EquipCategoryId.Cloak));
            Assert.True(serializedComponent.Equipment.ContainsKey(EquipCategoryId.Trinket));
        }

        [Test]
        public void SlotsWithItems_ItemsPreserved()
        {
            var component = new EquipmentComponent(new EquipCategory[]
            {
                new EquipCategory(EquipCategoryId.Staff, "Staff", 1),
                new EquipCategory(EquipCategoryId.Weapon, "Weapon", 1),
                new EquipCategory(EquipCategoryId.Cloak, "Cloak", 1),
                new EquipCategory(EquipCategoryId.Trinket, "Trinket", 1),
            });

            var logger = new Mock<ILogger>();
            var dm = new Mock<IDungeonMaster>();
            dm.Setup(dm => dm.Logger).Returns(logger.Object);

            var item = new Item(ItemAtlas.EtheriumShard);
            component.Equip(item, EquipCategoryId.Trinket, dm.Object);

            var serializedComponent = SerializationTestHelper.SerializeDeserialize(component);

            Assert.AreEqual(4, serializedComponent.Equipment.Count);
            Assert.AreEqual(1, serializedComponent.Equipment[EquipCategoryId.Trinket].Items.Count);
        }
    }
}
