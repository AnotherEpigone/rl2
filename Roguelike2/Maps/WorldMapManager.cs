using GoRogue.Pathing;
using Roguelike2.Entities;
using Roguelike2.GameMechanics;
using Roguelike2.GameMechanics.Time;
using Roguelike2.Serialization.Settings;
using Roguelike2.Ui.Windows;
using SadConsole.Input;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System;

namespace Roguelike2.Maps
{
    /// <summary>
    /// Handles input and movement logic for a world map
    /// </summary>
    public class WorldMapManager
    {
        private readonly WorldMap _map;
        private readonly IAppSettings _appSettings;
        private readonly AStar _aStar;
        private readonly IPlayerController _playerController;
        private readonly DungeonMaster _dm;
        private readonly TurnManager _turnManager;

        public WorldMapManager(
            IPlayerController playerController,
            DungeonMaster dm,
            TurnManager turnManager,
            WorldMap map,
            IAppSettings appSettings)
        {
            _map = map;
            _appSettings = appSettings;
            _map.RightMouseClick += Map_RightMouseButtonDown;
            _map.LeftMouseClick += Map_LeftMouseClick;

            // TODO use the terrain-cost astar?
            _aStar = new AStar(
                _map.WalkabilityView,
                Distance.Chebyshev,
                new LambdaGridView<double>(
                    _map.Width,
                    _map.Height,
                    p => (double)GetMovementCost(p) + 1d),
                    1d);
            _playerController = playerController;
            _dm = dm;
            _turnManager = turnManager;

            _dm.Player.Moved += Player_Moved;
        }

        public void CenterOnPlayer()
        {
            _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.WithCenter(_dm.Player.Position);
        }

        public bool HandleKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.C))
            {
                CenterOnPlayer();
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.G))
            {
                InteractSelf();
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.F) && _appSettings.Debug)
            {
                ToggleFov();
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.P) && _appSettings.Debug)
            {
                _turnManager.SuppressAi = !_turnManager.SuppressAi;
                _dm.Logger.Debug($"Suppress AI toggled. New value: {_turnManager.SuppressAi}");
                _dm.Logger.Gameplay($"DEBUG: Suppress AI: {_turnManager.SuppressAi}");
                return true;
            }

            if (_playerController.HandleKeyboard(keyboard))
            {
                return true;
            }

            return false;
        }

        public void Update()
        {
        }

        private int GetMovementCost(Point target)
        {
            var cost = 1;
            var feature = _map.GetEntityAt<TerrainFeature>(target);
            if (feature != null)
            {
                cost += feature.MovementCost;
            }

            return cost;
        }

        // Interact with anything at the player's location.
        private void InteractSelf()
        {
            if (PickupItem())
            {
                return;
            }

            if (ItemStackInteract())
            {
                return;
            }
        }

        private bool PickupItem()
        {
            var itemEntity = _map.GetEntityAt<ItemEntity>(_dm.Player.Position);
            if (itemEntity == null)
            {
                return false;
            }

            if (_dm.Player.Inventory.IsFilled)
            {
                _dm.Logger.Gameplay($"Can't pick up {itemEntity.Name}. Inventory is full.");
                return false;
            }

            _map.RemoveEntity(itemEntity);
            _dm.Player.Inventory.AddItem(itemEntity.Item, _dm);
            _dm.Logger.Gameplay($"Picked up {itemEntity.Name}.");

            _turnManager.PostProcessPlayerTurn(TimeHelper.Interact);

            return true;
        }

        private bool ItemStackInteract()
        {
            var itemStackEntity = _map.GetEntityAt<ItemStackEntity>(_dm.Player.Position);
            if (itemStackEntity == null)
            {
                return false;
            }

            if (_dm.Player.Inventory.IsFilled)
            {
                _dm.Logger.Gameplay($"Can't take anything from an item stack. Inventory is full.");
                return false;
            }

            var itemStackWindow = new ItemStackInteractWindow(itemStackEntity, _dm, _turnManager);
            itemStackWindow.Position = _map.Position / itemStackWindow.FontSize;
            itemStackWindow.Show(true);
            return true;
        }

        private void ToggleFov()
        {
            var fovHandler = _map.AllComponents.GetFirst<PlayerFieldOfViewHandler>();
            if (fovHandler.IsEnabled)
            {
                fovHandler.CurrentState = SadRogue.Integration.FieldOfView.FieldOfViewHandlerBase.State.DisabledResetVisibility;
            }
            else
            {
                fovHandler.CurrentState = SadRogue.Integration.FieldOfView.FieldOfViewHandlerBase.State.Enabled; ;
            }

            _dm.Logger.Debug($"FOV toggled. New value: {fovHandler.IsEnabled}");
            _dm.Logger.Gameplay($"DEBUG: FOV: {fovHandler.IsEnabled}");
        }

        private void Map_RightMouseButtonDown(object sender, MouseScreenObjectState e)
        {
        }

        private void Map_LeftMouseClick(object sender, MouseScreenObjectState e)
        {
            if (e.CellPosition == _dm.Player.Position)
            {
                InteractSelf();
            }
        }

        private void Player_Moved(object sender, GoRogue.GameFramework.GameObjectPropertyChanged<Point> e)
        {
            _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.WithCenter(_dm.Player.Position);
        }
    }
}
