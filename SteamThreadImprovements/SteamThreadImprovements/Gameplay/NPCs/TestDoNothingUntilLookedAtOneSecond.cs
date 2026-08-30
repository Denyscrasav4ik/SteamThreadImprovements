using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Gameplay.NPCs;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_TestDoNothingUntilLookedAtOneSecond)]
[HarmonyPatch(typeof(LookAtGuy_Active))]
internal static class TestDoNothingUntilLookedAtOneSecond
{
    private static readonly Dictionary<LookAtGuy_Active, float> SightStartTimes = new();

    [HarmonyPrefix]
    [HarmonyPatch("PlayerInSight")]
    private static bool PlayerInSight_Prefix(LookAtGuy_Active __instance, PlayerManager player)
    {
        if (!SightStartTimes.TryGetValue(__instance, out float startTime))
        {
            SightStartTimes[__instance] = Time.time;
            return false;
        }

        if (Time.time - startTime < 1f)
            return false;

        SightStartTimes.Remove(__instance);
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch("PlayerLost")]
    private static void PlayerLost_Prefix(LookAtGuy_Active __instance) =>
        SightStartTimes.Remove(__instance);

    [HarmonyPrefix]
    [HarmonyPatch("Exit")]
    private static void Exit_Prefix(LookAtGuy_Active __instance) =>
        SightStartTimes.Remove(__instance);
}
