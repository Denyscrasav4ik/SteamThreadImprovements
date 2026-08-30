using HarmonyLib;

namespace SteamThreadImprovements.QualityOfLife.Stickers;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_UnapplicableStickers)]
[HarmonyPatch(typeof(StickerManager), "StickerCanBeApplied")]
internal static class UnapplicableStickers
{
    private static void Postfix(Sticker sticker, ref bool __result)
    {
        if (!__result)
            return;

        CoreGameManager core = Singleton<CoreGameManager>.Instance;

        if (core == null || core.nextLevel == null)
            return;

        if (core.levelMapHasBeenPurchasedFor != core.nextLevel)
            return;

        if (sticker == Sticker.ExplorationBonus || sticker == Sticker.MapRange)
            __result = false;
    }
}
