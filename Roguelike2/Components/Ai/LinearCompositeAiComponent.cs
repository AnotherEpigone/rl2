using GoRogue.Components.ParentAware;
using Roguelike2.Maps;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roguelike2.Components.Ai
{
    /// <summary>
    /// AI component that runs through a set of child AI components until one succeeds.
    /// </summary>
    [DataContract]
    public class LinearCompositeAiComponent : IAiComponent
    {
        private IObjectWithComponents _parent;

        public LinearCompositeAiComponent(params IAiComponent[] components)
        {
            Components = new List<IAiComponent>(components);
        }

        public IObjectWithComponents Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                foreach (var component in Components)
                {
                    component.Parent = Parent;
                }
            }
        }

        [DataMember]
        public List<IAiComponent> Components { get; set; }

        public (bool success, int ticks) Run(WorldMap map, IDungeonMaster dungeonMaster)
        {
            foreach (var component in Components)
            {
                var (success, time) = component.Run(map, dungeonMaster);
                if (success)
                {
                    return (success, time);
                }
            }

            return (false, -1);
        }
    }
}
