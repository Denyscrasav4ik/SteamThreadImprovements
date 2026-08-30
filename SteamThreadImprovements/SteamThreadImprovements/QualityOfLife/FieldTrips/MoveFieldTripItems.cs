using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace SteamThreadImprovements.QualityOfLife.FieldTrips;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_MoveFieldTripItems)]
[HarmonyPatch(typeof(FieldTripBaseRoomFunction))]
internal static class MoveFieldTripItems
{
    [HarmonyPrefix]
    [HarmonyPatch("OnPlayerExit")]
    private static bool Prefix(FieldTripBaseRoomFunction __instance)
    {
        PitstopGameManager pitstop = Singleton<BaseGameManager>.Instance.GetComponent<PitstopGameManager>();

        if (pitstop == null)
            return true;

        FieldInfo pickupsField = AccessTools.Field(typeof(FieldTripBaseRoomFunction), "pickups");

        List<Pickup>? pickups = pickupsField.GetValue(__instance) as List<Pickup>;

        if (pickups == null)
            return true;

        foreach (Pickup pickup in pickups)
            pickup.transform.position += pitstop.itemsToLobbyOffset;

        return false;
    }
}
