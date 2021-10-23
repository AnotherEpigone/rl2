using GoRogue.Components.ParentAware;
using Roguelike2.Components.Triggers;
using Roguelike2.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Roguelike2.Components.ItemComponents
{
    [DataContract]
    public class ApplyInInventoryEffectsComponent : IInventoryTriggeredComponent
    {
        public ApplyInInventoryEffectsComponent()
            : this (Enumerable.Empty<IParentAwareComponent>())
        { }

        public ApplyInInventoryEffectsComponent(IEnumerable<IParentAwareComponent> components)
        {
            Components = components;
        }

        public IObjectWithComponents Parent { get; set; }

        [DataMember]
        public IEnumerable<IParentAwareComponent> Components { get; set; }

        public void OnAddedToInventory(Actor inventoryOwner, IDungeonMaster dungeonMaster)
        {
            foreach (var component in Components)
            {
                inventoryOwner.AllComponents.Add(component);
            }
        }

        public void OnRemovedFromInventory(Actor inventoryOwner, IDungeonMaster dungeonMaster)
        {
            foreach (var component in Components)
            {
                inventoryOwner.AllComponents.Remove(component);
            }
        }
    }
}
