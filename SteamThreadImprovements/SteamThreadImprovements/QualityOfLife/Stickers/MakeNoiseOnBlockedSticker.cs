using HarmonyLib;
using UnityEngine;
using System.Linq;

namespace SteamThreadImprovements.QualityOfLife.Stickers;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_MakeNoiseOnBlockedSticker)]
internal static class MakeNoiseOnBlockedSticker
{
    private static SoundObject? audFail;

    [HarmonyPatch(typeof(StickerScreenController), "ApplyHeldSticker")]
    [HarmonyPrefix]
    static void ApplyHeldSticker_Prefix(ref bool ___holdingSticker, out bool __state) => __state = ___holdingSticker;

    [HarmonyPatch(typeof(StickerScreenController), "ApplyHeldSticker")]
    [HarmonyPostfix]
    static void ApplyHeldSticker_Postfix(ref bool ___holdingSticker, bool __state)
    {
        if (__state && ___holdingSticker && StickerScreenController.editingAvailable)
            PlayFailSound();
    }

    [HarmonyPatch(typeof(StickerScreenController), "PickUpSticker")]
    [HarmonyPostfix]
    static void PickUpSticker_Postfix(bool __result)
    {
        if (!__result && StickerScreenController.editingAvailable)
            PlayFailSound();
    }

    private static void PlayFailSound()
    {
        if (audFail == null)
            audFail = Resources.FindObjectsOfTypeAll<SoundObject>().FirstOrDefault(s => s.name == "ErrorMaybe");
        if (audFail != null)
            Singleton<MusicManager>.Instance.PlaySoundEffect(audFail);
    }
}
