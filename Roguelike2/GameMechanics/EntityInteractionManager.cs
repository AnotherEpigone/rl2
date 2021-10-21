using Roguelike2.Entities;
using Roguelike2.GameMechanics.Combat;
using Roguelike2.Maps;
using System.Linq;

namespace Roguelike2.GameMechanics
{
    public class EntityInteractionManager
    {
        public EntityInteractionManager(IDungeonMaster dm, WorldMap map)
        {
            Dm = dm;
            Map = map;

            foreach (var actor in map.Entities.OfType<Actor>().Append(Dm.Player))
            {
                actor.Bumped += Actor_Bumped;
                actor.RemovedFromMap += Actor_RemovedFromMap;
            }
        }

        private void Actor_RemovedFromMap(object sender, GoRogue.GameFramework.GameObjectCurrentMapChanged e)
        {
            var actor = (Actor)sender;
            actor.Bumped -= Actor_Bumped;
            actor.RemovedFromMap -= Actor_RemovedFromMap;
        }

        private void Actor_Bumped(object sender, EntityBumpedEventArgs e)
        {
            // TODO doors
            //var bumpTriggeredComponent = Map.GetEntities<McEntity>(e.BumpedPosition)
            //        .SelectMany(e =>
            //        {
            //            if (!(e is McEntity entity))
            //            {
            //                return System.Array.Empty<IBumpTriggeredComponent>();
            //            }
            //
            //            return entity.GetGoRogueComponents<IBumpTriggeredComponent>();
            //        })
            //        .FirstOrDefault();
            //bumpTriggeredComponent?.Bump(e.BumpingEntity, _logManager, _dungeonMaster, _rng);

            var attacker = e.BumpingEntity;
            var defender = Map.GetEntityAt<Actor>(e.BumpedPosition);
            if (attacker == null
                || defender == null
                || !Dm.FactMan.AreEnemies(attacker.FactionId, defender.FactionId))
            {
                return;
            }

            e.Outcome = BumpOutcome.Melee;
            var hitResult = Dm.HitMan.GetAttackResult(attacker, defender);
            var damage = Dm.HitMan.GetDamage(attacker, hitResult);
            var defenderName = defender.Name;
            switch (hitResult)
            {
                case AttackResult.Hit:
                    Dm.Logger.Gameplay($"{attacker.Name} hit {defenderName} for {damage:F0} damage.");
                    defender.ApplyDamage(damage, Dm.Logger);
                    break;
                case AttackResult.Glance:
                    Dm.Logger.Gameplay($"{attacker.Name} hit {defenderName} with a glancing blow for {damage:F0} damage.");
                    defender.ApplyDamage(damage, Dm.Logger);
                    break;
                case AttackResult.Miss:
                    Dm.Logger.Gameplay($"{attacker.Name} missed {defenderName}.");
                    break;
                case AttackResult.Crit:
                    Dm.Logger.Gameplay($"{attacker.Name} hit {defenderName} with a critical hit for {damage:F0} damage.");
                    defender.ApplyDamage(damage, Dm.Logger);
                    break;
            }
        }

        public IDungeonMaster Dm { get; }
        public WorldMap Map { get; }
    }
}
