using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.Elevators;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_FlashingOpenElevatorIcon)]
[HarmonyPatch(typeof(Elevator), "Update")]
internal static class FlashingOpenElevatorIcon
{
    private static void Postfix(Elevator __instance, MapIcon ___mapIcon, ElevatorManager ___manager)
    {
        if (___mapIcon == null || ___mapIcon.spriteRenderer == null || ___manager == null) return;

        bool isOpen = __instance.CurrentState == ElevatorState.Open || __instance.CurrentState == ElevatorState.OpenForExit;
        bool willNotFail = !___manager.ShouldFail(__instance);

        if (isOpen && willNotFail)
        {
            Color color = ___mapIcon.spriteRenderer.color;
            color.a = (Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f) + 1f) * 0.5f;
            ___mapIcon.spriteRenderer.color = color;
        }
    }
}
