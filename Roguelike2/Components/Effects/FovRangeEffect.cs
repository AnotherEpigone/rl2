using GoRogue.Components.ParentAware;
using System.Runtime.Serialization;

namespace Roguelike2.Components.Effects
{
    [DataContract]
    public class FovRangeEffect : IFovRangeEffect
    {
        public FovRangeEffect(int modifier)
        {
            Modifier = modifier;
        }

        [DataMember]
        public int Modifier { get; }

        public IObjectWithComponents Parent { get; set; }
    }
}
