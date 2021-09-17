using GoRogue.Pathing;
using Roguelike2.Entities;
using Roguelike2.Fonts;
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

        private Unit _selectedUnit;
        private Point _selectedPoint;

        public WorldMapManager(Rl2Game game, WorldMap map)
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
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.WithCenter(SelectedPoint);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Down))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.ChangePosition(Direction.Down);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Up))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.ChangePosition(Direction.Up);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Right))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.ChangePosition(Direction.Right);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Left))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.ChangePosition(Direction.Left);
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

        private void MoveSelectedUnit(Point target)
        {
            // TODO adapt to RL
            if (SelectedUnit == null)
            {
                return;
            }

            var path = _aStar.ShortestPath(SelectedPoint, target);
            if (path == null)
            {
                return;
            }

            foreach (var step in path.Steps)
            {
                var movementCost = GetMovementCost(step);
                var result = SelectedUnit.TryMove(step, movementCost);
                switch (result)
                {
                    case UnitMovementResult.NoMovement:
                    case UnitMovementResult.Blocked:
                        break;
                    case UnitMovementResult.Moved:
                        continue;
                    case UnitMovementResult.Combat:
                        // TODO handle bump
                        break;
                };

                break;
            }

            SelectedPoint = SelectedUnit?.Position ?? Point.None;
        }

        private void Map_RightMouseButtonDown(object sender, MouseScreenObjectState e)
        {
        }

        private void Map_LeftMouseClick(object sender, MouseScreenObjectState e)
        {
        }
    }
}
