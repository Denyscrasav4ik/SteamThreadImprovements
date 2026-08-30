using HarmonyLib;

namespace SteamThreadImprovements.QualityOfLife.NPCs;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_SquishedNpcVoiceline)]
[HarmonyPatch(typeof(Entity))]
internal static class SquishedNPCsVoicelines
{
    [HarmonyPatch("Squish")]
    [HarmonyPostfix]
    static void Postfix_Squish(Entity __instance)
    {
        if (__instance.propagatedAudioManager != null)
        {
            foreach (PropagatedAudioManager audioManager in __instance.propagatedAudioManager)
            {
                audioManager.pitchModifier = 1.75f;
            }
        }
    }

    [HarmonyPatch("Unsquish")]
    [HarmonyPostfix]
    static void Postfix_Unsquish(Entity __instance)
    {
        if (__instance.propagatedAudioManager != null)
        {
            foreach (PropagatedAudioManager audioManager in __instance.propagatedAudioManager)
            {
                audioManager.pitchModifier = 1f;
            }
        }
    }
}
