using Roguelike2.Entities;

namespace Roguelike2
{
    public class Rl2Game
    {
        public Rl2Game(Player player)
        {
            Player = player;
        }

        public Player Player { get; }
    }
}
