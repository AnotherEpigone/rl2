using GoRogue.Pathing;
using Roguelike2.Entities;
using Roguelike2.Fonts;
using Roguelike2.GameMechanics;
using Roguelike2.Logging;
using SadConsole.Input;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System;
using System.Linq;

namespace Roguelike2.Maps
{
    /// <summary>
    /// Handles input and movement logic for a world map
    /// </summary>
    public class WorldMapManager
    {
        private readonly WorldMap _map;
        private readonly AStar _aStar;
        private readonly IPlayerController _playerController;
        private readonly DungeonMaster _game;

        private Unit _selectedUnit;
        private Point _selectedPoint;

        public WorldMapManager(DungeonMaster game, WorldMap map)
            : this(new PlayerController(), game, map)
        {
        }

        public WorldMapManager(
            IPlayerController playerController,
            DungeonMaster game,
            WorldMap map)
        {
            _map = map;
            _map.RightMouseButtonDown += Map_RightMouseButtonDown;
            _map.LeftMouseClick += Map_LeftMouseClick;

            _aStar = new AStar(
                _map.WalkabilityView,
                Distance.Chebyshev,
                new LambdaGridView<double>(
                    _map.Width,
                    _map.Height,
                    p => (double)GetMovementCost(p) + 1d),
                    1d);
            _playerController = playerController;
            _game = game;
            _game.Player.Moved += Player_Moved;
        }

        public event EventHandler SelectionChanged;
        public event EventHandler SelectionStatsChanged;

        public Point SelectedPoint
        {
            get => _selectedPoint;
            private set
            {
                _selectedPoint = value;
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Unit SelectedUnit
        {
            get => _selectedUnit;
            private set
            {
                if (_selectedUnit != null)
                {
                    _selectedUnit.StatsChanged -= SelectionStatsChanged;
                }

                _selectedUnit = value;
                if (_selectedUnit != null)
                {
                    _selectedUnit.StatsChanged += SelectionStatsChanged;
                }

                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool HandleKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.C))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.WithCenter(_game.Player.Position);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.G))
            {
                PickUpItem();
                return true;
            }

            if (_playerController.HandleKeyboard(_game, keyboard))
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

        private void PickUpItem()
        {
            var itemEntity = _map.GetEntityAt<ItemEntity>(_game.Player.Position);
            if (itemEntity == null)
            {
                return;
            }

            if (_game.Player.Inventory.IsFilled)
            {
                _game.Logger.Gameplay($"Can't pick up {itemEntity.Name}. Inventory is full.");
                return;
            }

            _map.RemoveEntity(itemEntity);
            _game.Player.Inventory.AddItem(itemEntity.Item, _game);
            _game.Logger.Gameplay($"Picked up {itemEntity.Name}.");
        }

        private void Map_RightMouseButtonDown(object sender, MouseScreenObjectState e)
        {
        }

        private void Map_LeftMouseClick(object sender, MouseScreenObjectState e)
        {
            if (e.CellPosition == _game.Player.Position)
            {
                PickUpItem();
            }
        }

        private void Player_Moved(object sender, GoRogue.GameFramework.GameObjectPropertyChanged<Point> e)
        {
            _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.WithCenter(_game.Player.Position);
        }
    }
}
