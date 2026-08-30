using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Visual.HUD;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Visual, ConfigEntryStorage.Name_NotebookCounterGoDownWithTV)]
[HarmonyPatch(typeof(HudManager))]
internal static class NotebookCounterGoDownWithTV
{
    private static readonly FieldInfo NotebookDisplayField = AccessTools.Field(typeof(HudManager), "notebookDisplay");

    private static float baseTvY;
    private static Vector2[]? baseNotebookPositions;
    private static bool initialized;

    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    private static void Postfix(HudManager __instance)
    {
        if (__instance.BaldiTv?.GetComponent<RectTransform>() is not { } tvRect)
            return;

        if (NotebookDisplayField.GetValue(__instance) is not GameObject[] notebookDisplay)
            return;

        if (!initialized)
        {
            baseTvY = tvRect.anchoredPosition.y;
            baseNotebookPositions = new Vector2[notebookDisplay.Length];

            for (int i = 0; i < notebookDisplay.Length; i++)
            {
                if (notebookDisplay[i]?.GetComponent<RectTransform>() is { } rect)
                    baseNotebookPositions[i] = rect.anchoredPosition;
            }

            initialized = true;
        }

        float tvOffsetY = tvRect.anchoredPosition.y - baseTvY;

        for (int i = 0; i < notebookDisplay.Length; i++)
        {
            if (notebookDisplay[i]?.GetComponent<RectTransform>() is { } rect)
            {
                rect.anchoredPosition = baseNotebookPositions![i] + new Vector2(0, tvOffsetY);
            }
        }
    }
}
