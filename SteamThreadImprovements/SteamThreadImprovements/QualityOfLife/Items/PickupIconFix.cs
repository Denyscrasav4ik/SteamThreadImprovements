using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.items;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_PickupIconFix)]
[HarmonyPatch]
internal static class PickupIconFix
{
    static readonly FieldInfo MapIconsField = AccessTools.Field(typeof(Map), "icons");
    static readonly Dictionary<MapIcon, Pickup> pickupIcons = new();

    [HarmonyPatch(typeof(Pickup), "Start")]
    [HarmonyPostfix]
    static void Postfix(Pickup __instance)
    {
        if (__instance.icon != null || __instance.iconPre == null)
            return;

        Map map = Object.FindObjectOfType<Map>();
        if (map == null)
            return;

        __instance.icon = map.AddIcon(__instance.iconPre, __instance.transform, Color.white);
        pickupIcons[__instance.icon] = __instance;
    }

    [HarmonyPatch(typeof(Map), "Update")]
    [HarmonyPrefix]
    static void MapUpdatePrefix(Map __instance)
    {
        if (pickupIcons.Count == 0 ||
            MapIconsField.GetValue(__instance) is not List<MapIcon> icons)
            return;

        List<MapIcon> remove = null!;

        foreach (var (icon, pickup) in pickupIcons)
        {
            if (pickup == null || icon == null || !icons.Contains(icon))
                (remove ??= new()).Add(icon!);
        }

        if (remove == null)
            return;

        foreach (MapIcon icon in remove)
        {
            if (icon != null)
            {
                icons.Remove(icon);
                Object.Destroy(icon.gameObject);
            }

            pickupIcons.Remove(icon!);
        }
    }
}
