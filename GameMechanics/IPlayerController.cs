using SadConsole.Input;

namespace Roguelike2.GameMechanics
{
    public interface IPlayerController
    {
        bool HandleKeyboard(DungeonMaster game, Keyboard keyboard);
    }
}