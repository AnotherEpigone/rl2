using Roguelike2.GameMechanics.Time;
using SadConsole.Input;
using SadRogue.Primitives;

namespace Roguelike2.GameMechanics
{
    public sealed class PlayerController : IPlayerController
    {
        private readonly DungeonMaster _dm;
        private readonly TurnManager _turnManager;

        public PlayerController(DungeonMaster dm, TurnManager turnManager)
        {
            _dm = dm;
            _turnManager = turnManager;
        }

        public bool HandleKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Down) || keyboard.IsKeyPressed(Keys.NumPad2))
            {
                HandlePlayerMovement(_dm, Direction.Down);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Up) || keyboard.IsKeyPressed(Keys.NumPad8))
            {
                HandlePlayerMovement(_dm, Direction.Up);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Right) || keyboard.IsKeyPressed(Keys.NumPad6))
            {
                HandlePlayerMovement(_dm, Direction.Right);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Left) || keyboard.IsKeyPressed(Keys.NumPad4))
            {
                HandlePlayerMovement(_dm, Direction.Left);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.NumPad7))
            {
                HandlePlayerMovement(_dm, Direction.UpLeft);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.NumPad9))
            {
                HandlePlayerMovement(_dm, Direction.UpRight);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.NumPad3))
            {
                HandlePlayerMovement(_dm, Direction.DownRight);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.NumPad1))
            {
                HandlePlayerMovement(_dm, Direction.DownLeft);
                return true;
            }

            return false;
        }

        private void HandlePlayerMovement(DungeonMaster game, Direction dir)
        {
            // TODO handle melee time and blocked movement
            game.Player.TryMove(dir);
            _turnManager.PostProcessPlayerTurn(TimeHelper.GetWalkTime(_dm.Player));
        }
    }
}
