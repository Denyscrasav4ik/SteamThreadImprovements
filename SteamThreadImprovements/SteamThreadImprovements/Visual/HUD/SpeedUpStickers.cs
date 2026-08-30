using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Visual.HUD;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Visual, ConfigEntryStorage.Name_SpeedUpStickers)]
[HarmonyPatch(typeof(StickerPacketAnimationManager))]
internal static class SpeedUpStickers
{
    private static readonly FieldInfo QueueField = AccessTools.Field(typeof(StickerPacketAnimationManager), "stickerSpriteQueue");

    [HarmonyPostfix, HarmonyPatch("ShowQueuedSticker")]
    private static void ShowQueuedSticker_Postfix(StickerPacketAnimationManager __instance)
    {
        if (QueueField.GetValue(__instance) is Queue<Sprite> queue)
            __instance.GetComponent<Animator>().speed = 1f + queue.Count * 0.5f;
    }

    [HarmonyPostfix, HarmonyPatch("Stop")]
    private static void Stop_Postfix(StickerPacketAnimationManager __instance) => __instance.GetComponent<Animator>().speed = 1f;
}
