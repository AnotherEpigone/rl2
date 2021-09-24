﻿using Roguelike2.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Roguelike2.Entities
{
    public static class UnitAtlas
    {
        private static readonly Lazy<Dictionary<string, UnitTemplate>> _byId;

        static UnitAtlas()
        {
            _byId = new Lazy<Dictionary<string, UnitTemplate>>(() => typeof(UnitAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(UnitTemplate))
                .Select(p => p.GetValue(null))
                .OfType<UnitTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, UnitTemplate> ById => _byId.Value;
    }
}