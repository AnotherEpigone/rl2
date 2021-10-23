using GoRogue.Components.ParentAware;
using Roguelike2.Components.Triggers;
using Roguelike2.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Roguelike2.Components.ItemComponents
{
    [DataContract]
    public class ApplyWhenEquippedComponent : IEquipTriggeredComponent
    {
        public ApplyWhenEquippedComponent()
            : this(Enumerable.Empty<IParentAwareComponent>())
        {
        }

        public ApplyWhenEquippedComponent(IEnumerable<IParentAwareComponent> components)
        {
            Components = components;
        }

        public IObjectWithComponents Parent { get; set; }

        [DataMember]
        public IEnumerable<IParentAwareComponent> Components { get; set; }

        public void OnEquip(Actor equipmentOwner, IDungeonMaster dungeonMaster)
        {
            foreach (var component in Components)
            {
                equipmentOwner.AllComponents.Add(component);
            }
        }

        public void OnUnequip(Actor equipmentOwner, IDungeonMaster dungeonMaster)
        {
            foreach (var component in Components)
            {
                equipmentOwner.AllComponents.Remove(component);
            }
        }
    }
}
