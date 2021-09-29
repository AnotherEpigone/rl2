using SadConsole.Input;
using SadRogue.Primitives;

namespace Roguelike2.GameMechanics
{
    public sealed class PlayerController : IPlayerController
    {
        public bool HandleKeyboard(DungeonMaster game, Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Down) || keyboard.IsKeyPressed(Keys.NumPad2))
            {
                HandlePlayerMovement(game, Direction.Down);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Up) || keyboard.IsKeyPressed(Keys.NumPad8))
            {
                HandlePlayerMovement(game, Direction.Up);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Right) || keyboard.IsKeyPressed(Keys.NumPad6))
            {
                HandlePlayerMovement(game, Direction.Right);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Left) || keyboard.IsKeyPressed(Keys.NumPad4))
            {
                HandlePlayerMovement(game, Direction.Left);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.NumPad7))
            {
                HandlePlayerMovement(game, Direction.UpLeft);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.NumPad9))
            {
                HandlePlayerMovement(game, Direction.UpRight);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.NumPad3))
            {
                HandlePlayerMovement(game, Direction.DownRight);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.NumPad1))
            {
                HandlePlayerMovement(game, Direction.DownLeft);
                return true;
            }

            return false;
        }

        private void HandlePlayerMovement(DungeonMaster game, Direction dir)
        {
            // TODO handle bumps
            game.Player.TryMove(game.Player.Position + dir);
        }
    }
}
