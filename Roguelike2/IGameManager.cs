namespace Roguelike2
{
    public interface IGameManager
    {
        void StartNewGame();

        void Save();

        void LoadLatest();

        void Load();

        bool CanLoad();
    }
}
