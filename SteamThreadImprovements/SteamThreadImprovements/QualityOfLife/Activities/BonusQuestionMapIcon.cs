using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.Activities;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_BonusQuestionMapIcon)]
[HarmonyPatch(typeof(Activity))]
internal static class BonusQuestionMapIconPatch
{
    private static readonly AccessTools.FieldRef<Activity, Notebook> notebookField = AccessTools.FieldRefAccess<Activity, Notebook>("notebook");
    private static readonly AccessTools.FieldRef<Activity, bool> completedField = AccessTools.FieldRefAccess<Activity, bool>("completed");
    private static readonly Dictionary<MapIcon, Sprite> originalSprites = new Dictionary<MapIcon, Sprite>();

    [HarmonyPatch("SetBonusMode")]
    [HarmonyPostfix]
    private static void SetBonusMode_Postfix(Activity __instance) => UpdateBonusIcon(__instance);

    [HarmonyPatch("Completed", new Type[] { typeof(int) })]
    [HarmonyPostfix]
    private static void Completed_Postfix1(Activity __instance) => UpdateBonusIcon(__instance);

    [HarmonyPatch("Completed", new Type[] { typeof(int), typeof(bool) })]
    [HarmonyPostfix]
    private static void Completed_Postfix2(Activity __instance) => UpdateBonusIcon(__instance);

    [HarmonyPatch("ReInit")]
    [HarmonyPostfix]
    private static void ReInit_Postfix(Activity __instance) => UpdateBonusIcon(__instance);

    private static void UpdateBonusIcon(Activity activity)
    {
        Notebook notebook = notebookField(activity);
        if (notebook == null || notebook.icon == null || notebook.icon.spriteRenderer == null) return;

        MapIcon icon = notebook.icon;

        if (!originalSprites.ContainsKey(icon))
            originalSprites[icon] = icon.spriteRenderer.sprite;

        if (activity.InBonusMode)
        {
            if (!completedField(activity))
            {
                icon.spriteRenderer.sprite = ImprovementPlugin.bonusQuestionIcon;
                icon.spriteRenderer.enabled = true;
                Color col = icon.spriteRenderer.color;
                col.a = 1f;
                icon.spriteRenderer.color = col;
            }
            else
                icon.spriteRenderer.enabled = false;
        }
        else
        {
            if (originalSprites.TryGetValue(icon, out Sprite origSprite))
                icon.spriteRenderer.sprite = origSprite;
        }
    }
}
