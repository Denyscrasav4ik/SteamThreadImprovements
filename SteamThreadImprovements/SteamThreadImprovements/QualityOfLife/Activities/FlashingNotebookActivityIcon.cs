using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.Activities;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_FlashingNotebookActivityIcon)]
[HarmonyPatch(typeof(Activity), "Update")]
internal static class FlashingNotebookActivityIcon
{
    private static readonly AccessTools.FieldRef<Notebook, bool> notebookCollectedField = AccessTools.FieldRefAccess<Notebook, bool>("<collected>k__BackingField");

    private static void Postfix(Activity __instance, Notebook ___notebook)
    {
        if (___notebook == null || ___notebook.icon == null || ___notebook.hidden) return;

        var spriteRenderer = ___notebook.icon.spriteRenderer;

        if (__instance.IsCompleted && !__instance.NotebookCollected && !__instance.InBonusMode)
        {
            float alpha = (Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f) + 1f) * 0.5f;

            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        else
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }

    [HarmonyPatch(typeof(Activity), "ReInit")]
    private static void Postfix(Notebook ___notebook)
    {
        if (___notebook != null)
            notebookCollectedField(___notebook) = false;
    }
}
