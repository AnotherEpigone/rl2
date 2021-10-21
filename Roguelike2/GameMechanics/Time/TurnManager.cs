using Roguelike2.Components.Ai;
using Roguelike2.Components.Effects;
using Roguelike2.Entities;
using Roguelike2.GameMechanics.Time.Nodes;
using Roguelike2.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike2.GameMechanics.Time
{
    public enum State
    {
        PlayerTurn,
        Processing,
    }

    public class TurnManager
    {
        private readonly IDungeonMaster _dm;
        private readonly Dictionary<Guid, NovaEntity> _registeredEntities;

        public TurnManager(IDungeonMaster dm, WorldMap map)
        {
            _dm = dm;
            _registeredEntities = new Dictionary<Guid, NovaEntity>();

            Map = map;

            State = State.PlayerTurn;

            if (_dm.TimeMaster.NodeCount == 0)
            {
                var secondMarkerNode = new SecondMarkerNode((_dm.TimeMaster.JourneyTime.Seconds + 1) * 100);
                _dm.TimeMaster.Enqueue(secondMarkerNode);
            }

            foreach (var actor in Map.Entities.OfType<Actor>())
            {
                RegisterEntity(actor);
            }
        }

        public WorldMap Map { get; private set; }

        public State State { get; private set; }

        public void PostProcessPlayerTurn(int playerTurnTicks)
        {
            var playerTurnNode = new WizardTurnNode(
                _dm.TimeMaster.JourneyTime.Ticks + playerTurnTicks);
            _dm.TimeMaster.Enqueue(playerTurnNode);

            _dm.Player.CalculateFov();

            State = State.Processing;
            var node = _dm.TimeMaster.Dequeue();
            while (node is not WizardTurnNode)
            {
                if (_dm.Player.CurrentMap == null)
                {
                    break;
                }

                switch (node)
                {
                    case EntityTurnNode entityTurnNode:
                        ProcessAiTurn(
                            entityTurnNode.EntityId,
                            _dm.TimeMaster.JourneyTime.Ticks);
                        break;
                    case SecondMarkerNode:
                        ProcessSecondMarker(_dm.TimeMaster.JourneyTime);
                        break;
                    default:
                        throw new NotSupportedException($"Unhandled time master node type: {node.GetType()}");
                }

                node = _dm.TimeMaster.Dequeue();
            }

            State = State.PlayerTurn;
        }

        public void RegisterEntity(NovaEntity entity)
        {
            // TODO
            /*if (entity.IsSubTile)
            {
                return;
            }*/

            // inefficient lookup. If it becomes a problem, use a hashset for IDs.
            if (entity.AllComponents.Contains<IAiComponent>()
                && !_dm.TimeMaster.Nodes.Any(node =>
                    node is EntityTurnNode entityNode
                    && entityNode.EntityId == entity.Id))
            {
                // TODO revisit this starting point of 10 ticks for all AIs.
                // possibly they should go before the player? Possibly the AI should be primed so
                // we don't have a lag after the player's first move.
                var time = _dm.TimeMaster.JourneyTime.Ticks + 10;
                var entityTurnNode = new EntityTurnNode(time, entity.Id);
                _dm.TimeMaster.Enqueue(entityTurnNode);
            }

            _registeredEntities.Add(entity.Id, entity);
            entity.RemovedFromMap += (_, __) => UnregisterEntity(entity);
        }

        private void ProcessAiTurn(Guid entityId, long time)
        {
            if (!_registeredEntities.TryGetValue(entityId, out var entity)
                || entity.CurrentMap == null)
            {
                return;
            }

            var ai = entity.AllComponents.GetFirst<IAiComponent>();
            var (success, ticks) = ai?.Run(Map, _dm) ?? (false, -1);
            if (!success || ticks < 1)
            {
                return;
            }

            var nextTurnNode = new EntityTurnNode(time + ticks, entity.Id);
            _dm.TimeMaster.Enqueue(nextTurnNode);
        }

        private void ProcessSecondMarker(McTimeSpan time)
        {
            foreach (var entity in _registeredEntities.Values.Append(_dm.Player))
            {
                var effects = entity.AllComponents.GetAll<ITimedEffect>();
                foreach (var effect in effects.ToArray())
                {
                    effect.OnTick(time, _dm);
                }
            }

            // TODO apply regen here (OR make it an effect?)

            var secondMarkerNode = new SecondMarkerNode(time.Ticks + 100);
            _dm.TimeMaster.Enqueue(secondMarkerNode);
        }

        private void UnregisterEntity(NovaEntity entity)
        {
            // TODO
            /*if (entity.IsSubTile)
            {
                return;
            }*/

            _registeredEntities.Remove(entity.Id);
        }
    }
}
