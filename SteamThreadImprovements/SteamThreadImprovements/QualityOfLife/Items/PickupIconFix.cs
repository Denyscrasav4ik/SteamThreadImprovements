using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.items;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_PickupIconFix)]
[HarmonyPatch(typeof(Pickup), "Start")]
internal static class PickupIconFix
{
    static void Postfix(Pickup __instance)
    {
        if (__instance.icon != null) return;

        Map map = Object.FindObjectOfType<Map>();

        if (map != null && __instance.iconPre != null)
            __instance.icon = map.AddIcon(__instance.iconPre, __instance.transform, Color.white);
    }
}
