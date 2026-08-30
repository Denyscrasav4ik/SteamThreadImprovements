using HarmonyLib;

namespace SteamThreadImprovements.QualityOfLife.FieldTrips;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_AlwaysFieldtripBusPass)]
[HarmonyPatch(typeof(PitstopGameManager), "PrepareLevelData")]
internal static class AlwaysFieldTripBusPass
{
    static void Postfix(PitstopGameManager __instance, LevelData data, ref FieldTripObject ___currentFieldTrip, WeightedFieldTrip[] ___tierOneTrips)
    {
        CoreGameManager core = Singleton<CoreGameManager>.Instance;

        if (core.tripAvailable) return;

        bool hasBusPass = false;

        if (core.currentLockerItems != null)
        {
            foreach (ItemObject item in core.currentLockerItems)
            {
                if (item != null && item.itemType == Items.BusPass)
                {
                    hasBusPass = true;
                    break;
                }
            }
        }

        if (!hasBusPass)
        {
            PlayerManager player = core.GetPlayer(0);
            if (player != null && player.itm != null && player.itm.Has(Items.BusPass))
                hasBusPass = true;
        }

        if (hasBusPass && ___tierOneTrips != null && ___tierOneTrips.Length > 0)
        {
            ___currentFieldTrip = WeightedSelection<FieldTripObject>.RandomSelection(___tierOneTrips);

            if (___currentFieldTrip != null)
            {
                data.roomAssetsPlacements.Add(___currentFieldTrip.tripHub);
                core.tripAvailable = true;
                core.currentFieldTrip = ___currentFieldTrip;
            }
        }
    }
}
