using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SteamThreadImprovements;

public static class HarmonyExtensions
{
    public static void PatchAllConditionals(this Harmony harmony, Assembly assembly)
    {
        foreach (var type in AccessTools.GetTypesFromAssembly(assembly))
        {
            var conditionalPatch = type
                .GetCustomAttributes<ConditionalPatch>(false)
                .FirstOrDefault();

            if (conditionalPatch == null || conditionalPatch.ShouldPatch())
                harmony.CreateClassProcessor(type).Patch();
        }
    }

    public static void PatchAllConditionals(this Harmony harmony)
    {
        harmony.PatchAllConditionals(Assembly.GetCallingAssembly());
    }
}

public abstract class ConditionalPatch : Attribute
{
    public abstract bool ShouldPatch();
}

public sealed class ConditionalPatchConfig : ConditionalPatch
{
    private readonly string _category;
    private readonly string _name;

    public ConditionalPatchConfig(string category, string name)
    {
        _category = category;
        _name = name;
    }

    public override bool ShouldPatch()
    {
        var instance = ImprovementPlugin.Instance;

        var definition = new ConfigDefinition(_category, _name);

        if (!instance!.Config.TryGetEntry(
                definition,
                out ConfigEntry<bool> entry))
        {
            Debug.LogWarning($"Cannot find config: {_category}, {_name}");
            return false;
        }

        return entry.Value;
    }
}
