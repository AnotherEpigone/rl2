using Roguelike2.Entities;
using Roguelike2.Maps;
using System.Runtime.Serialization;

namespace Roguelike2.Serialization
{
    [DataContract]
    public class GameState
    {
        [DataMember]
        public WorldMap Map { get; set; }

        [DataMember]
        public Player Player { get; set; }
    }
}
