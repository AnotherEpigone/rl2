using Roguelike2.GameMechanics.Time;
using SadConsole.Input;

namespace Roguelike2.GameMechanics
{
    public interface IPlayerController
    {
        bool HandleKeyboard(Keyboard keyboard);
    }
}