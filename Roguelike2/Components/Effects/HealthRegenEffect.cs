using GoRogue.Components.ParentAware;
using System.Runtime.Serialization;

namespace Roguelike2.Components.Effects
{
    [DataContract]
    public class HealthRegenEffect : IHealthRegenEffect
    {
        public HealthRegenEffect(float value)
        {
            Value = value;
        }

        [DataMember]
        public float Value { get; }

        public IObjectWithComponents Parent { get; set; }

        public string GetDescription() => $"Health regen {Value}";
    }
}
