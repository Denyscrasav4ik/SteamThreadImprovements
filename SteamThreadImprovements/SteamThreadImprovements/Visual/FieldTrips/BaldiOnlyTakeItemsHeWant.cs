using HarmonyLib;
using PicnicPanic;

namespace SteamThreadImprovements.Visual.FieldTrips;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Visual, ConfigEntryStorage.Name_BaldiOnlyTakeItemsHeWant)]
[HarmonyPatch(typeof(PlateController), "ApproachBaldi")]
internal static class BaldiOnlyTakeItemsHeWant
{
    private static readonly AccessTools.FieldRef<PlateController, bool> Valid = AccessTools.FieldRefAccess<PlateController, bool>("valid");

    private static bool Prefix(PlateController __instance)
    {
        if (Valid(__instance))
            return false;

        return true;
    }
}
