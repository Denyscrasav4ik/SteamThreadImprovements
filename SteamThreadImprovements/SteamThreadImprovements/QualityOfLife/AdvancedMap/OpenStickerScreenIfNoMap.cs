using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.AdvancedMap;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_OpenStickerScreenIfNoMap)]
internal static class OpenStickerScreenIfNoMap
{
    internal static bool openedFromPacketOMatic;

    [HarmonyPatch(typeof(CoreGameManager), "Update")]
    [HarmonyPrefix]
    private static bool CoreGameManager_Update_Prefix(CoreGameManager __instance)
    {
        if (Singleton<InputManager>.Instance.GetDigitalInput("Pause", onDown: true))
            __instance.Pause(openScreen: true);

        if (!Singleton<GlobalCam>.Instance.TransitionActive && Singleton<InputManager>.Instance.GetDigitalInput("MapPlus", onDown: true))
            __instance.ToggleYctpScreen(__instance.MapAvaialble);

        return false;
    }

    [HarmonyPatch(typeof(PacketOMatic), "Clicked")]
    [HarmonyPrefix]
    private static void PacketOMatic_Clicked_Prefix() => openedFromPacketOMatic = true;

    [HarmonyPatch(typeof(PacketOMatic), "Clicked")]
    [HarmonyPostfix]
    private static void PacketOMatic_Clicked_Postfix() => openedFromPacketOMatic = false;

    [HarmonyPatch(typeof(StickerScreenController), "OnEnable")]
    [HarmonyPrefix]
    private static void StickerScreenController_OnEnable_Prefix()
    {
        if (openedFromPacketOMatic)
        {
            StickerScreenController.editingAvailable = true;
        }
        else if (!Singleton<CoreGameManager>.Instance.MapAvaialble)
        {
            StickerScreenController.editingAvailable = false;
        }
    }

    [HarmonyPatch(typeof(StickerScreenController), "OnEnable")]
    [HarmonyPostfix]
    private static void StickerScreenController_OnEnable_Postfix(GameObject ___backToMapButton)
    {
        if (!Singleton<CoreGameManager>.Instance.MapAvaialble)
            ___backToMapButton.SetActive(false);
    }
}
