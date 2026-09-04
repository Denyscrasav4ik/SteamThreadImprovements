using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Gameplay.Endless;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_DetentionTimerScaleBack)]
[HarmonyPatch]
internal static class DetentionTimerScaleBack
{
    private static float timeSinceLastCatch = 0f;
    private static float timeToScaleBack = 120f;

    [HarmonyPatch(typeof(Principal), "SendToDetention")]
    [HarmonyPostfix]
    private static void PrincipalSendToDetention_Postfix() => timeSinceLastCatch = 0f;

    [HarmonyPatch(typeof(EndlessGameManager), "VirtualUpdate")]
    [HarmonyPostfix]
    private static void EndlessGameManagerVirtualUpdate_Postfix(EndlessGameManager __instance)
    {
        timeSinceLastCatch += Time.deltaTime * __instance.Ec.EnvironmentTimeScale;

        if (timeSinceLastCatch >= timeToScaleBack)
        {
            timeSinceLastCatch = 0f;
            foreach (NPC npc in __instance.Ec.Npcs)
            {
                if (npc is Principal principal)
                {
                    var detentionLevelField = Traverse.Create(principal).Field("detentionLevel");
                    int currentLevel = detentionLevelField.GetValue<int>();

                    if (currentLevel > 0)
                        detentionLevelField.SetValue(currentLevel - 1);
                }
            }
        }
    }
}
