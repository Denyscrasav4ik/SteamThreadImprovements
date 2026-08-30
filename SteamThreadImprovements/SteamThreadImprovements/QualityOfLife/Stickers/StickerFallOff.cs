using HarmonyLib;

namespace SteamThreadImprovements.QualityOfLife.Stickers;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_StickerFallOff)]
[HarmonyPatch(typeof(StickerManager), "AdvanceStickerUsage")]
internal static class StickerFallOff
{
    [HarmonyPrefix]
    private static void Prefix(StickerManager __instance, int value)
    {
        for (int i = 0; i < __instance.appliedStickerRemainingNotebooks.Length; i++)
        {
            if (!__instance.SlotUpgraded(i) && __instance.activeStickerData[i].sticker != Sticker.Nothing && __instance.appliedStickerRemainingNotebooks[i] > 0 && __instance.appliedStickerRemainingNotebooks[i] - value <= 0)
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(ImprovementPlugin.stickerFallOff);
        }
    }
}
