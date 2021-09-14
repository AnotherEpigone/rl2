using Roguelike2.Maps;
using System.Runtime.Serialization;

namespace Roguelike2.Serialization
{
    [DataContract]
    public class GameState
    {
        [DataMember]
        public WorldMap Map { get; set; }
    }
}
