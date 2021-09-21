using SadConsole.Input;

namespace Roguelike2.GameMechanics
{
    public interface IPlayerController
    {
        bool HandleKeyboard(Rl2Game game, Keyboard keyboard);
    }
}