namespace Roguelike2.Serialization
{
    public interface ISaveManager
    {
        (bool, GameState) Read();
        bool SaveExists();
        void Write(GameState save);
    }
}