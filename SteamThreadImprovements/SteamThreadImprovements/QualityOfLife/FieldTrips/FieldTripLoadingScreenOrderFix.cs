using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace SteamThreadImprovements.QualityOfLife.FieldTrips;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_FieldTripLoadingScreenOrderFix)]
[HarmonyPatch(typeof(PitstopGameManager), "SendJohnny")]
internal static class FieldTripLoadingScreenOrderFix
{
    [HarmonyPrefix]
    static void Prefix(PitstopGameManager __instance, out List<Pickup> __state)
    {
        __state = null!;

        if (!Singleton<CoreGameManager>.Instance.tripPlayed)
        {
            __state = new List<Pickup>(__instance.fieldTripPickups);
            __instance.fieldTripPickups.Clear();
        }
    }

    [HarmonyPostfix]
    static void Postfix(PitstopGameManager __instance, List<Pickup> __state)
    {
        if (__state != null)
        {
            __instance.fieldTripPickups.AddRange(__state);
            __instance.StartCoroutine(SpawnItemsAfterTransition(__instance));
        }
    }

    private static IEnumerator SpawnItemsAfterTransition(PitstopGameManager manager)
    {
        yield return null;

        while (Time.timeScale == 0f)
        {
            yield return null;
        }

        manager.fieldTripPickups.Shuffle();
        for (int i = 0; i < manager.fieldTripPickups.Count; i++)
        {
            manager.fieldTripPickups[i].transform.position += manager.itemsToLobbyOffset;
            manager.fieldTripPickups[i].gameObject.SetActive(i < 3);
        }
    }
}
