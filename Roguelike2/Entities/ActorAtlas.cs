using Roguelike2.Fonts;
using Roguelike2.GameMechanics.Factions;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Roguelike2.Entities
{
    public static class ActorAtlas
    {
        private static readonly Lazy<Dictionary<string, ActorTemplate>> _byId;

        static ActorAtlas()
        {
            _byId = new Lazy<Dictionary<string, ActorTemplate>>(() => typeof(ActorAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(ActorTemplate))
                .Select(p => p.GetValue(null))
                .OfType<ActorTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, ActorTemplate> ById => _byId.Value;

        public static ActorTemplate Goblin => new(
            id: "ACTOR_GOBLIN",
            name: "Goblin",
            glyph: WorldGlyphAtlas.Goblin,
            createComponents: () => new List<object>
            {
                /*new HealthComponent(10),
                new ActorStatComponent(1.2f, 1f, 1f),
                new MeleeAttackerComponent(5),
                new LinearCompositeAiComponent(
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),*/
            },
            FactionAtlas.Goblins.Id,
            unarmedMelee: 3,
            health: 5,
            new List<SubTileTemplate>());

        public static ActorTemplate GoblinArcher => new(
            id: "ACTOR_GOBLIN_ARCHER",
            name: "Goblin archer",
            glyph: WorldGlyphAtlas.GoblinArcher,
            createComponents: () => new List<object>
            {
                /*new HealthComponent(10),
                new ActorStatComponent(1f, 0.7f, 1f),
                new RangedAttackerComponent(5, 4),
                new LinearCompositeAiComponent(
                    new RangedAttackAiComponent(),
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),*/
            },
            FactionAtlas.Goblins.Id,
            unarmedMelee: 2,
            health: 4,
            new List<SubTileTemplate>());

        public static ActorTemplate Warg => new(
            id: "ACTOR_WARG",
            name: "Warg",
            glyph: WorldGlyphAtlas.Warg,
            createComponents: () => new List<object>
            {
                /*new HealthComponent(10),
                new ActorStatComponent(2f, 1.5f, 1f),
                new MeleeAttackerComponent(5),
                new LinearCompositeAiComponent(
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),*/
            },
            FactionAtlas.Goblins.Id,
            unarmedMelee: 8,
            health: 8,
            new List<SubTileTemplate>());

        public static ActorTemplate Troll => new(
            id: "ACTOR_TROLL",
            name: "Troll",
            glyph: WorldGlyphAtlas.Troll_TopLeft,
            createComponents: () => new List<object>
            {
                /*new HealthComponent(50),
                new ActorStatComponent(0.8f, 0.6f, 1f),
                new MeleeAttackerComponent(25),
                new LinearCompositeAiComponent(
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),*/
            },
            FactionAtlas.Goblins.Id,
            unarmedMelee: 20,
            health: 50,
            new List<SubTileTemplate>
            {
                new SubTileTemplate(WorldGlyphAtlas.Troll_TopRight, new Point(1, 0)),
                new SubTileTemplate(WorldGlyphAtlas.Troll_BottomLeft, new Point(0, 1)),
                new SubTileTemplate(WorldGlyphAtlas.Troll_BottomRight, new Point(1, 1)),
            });
    }
}
