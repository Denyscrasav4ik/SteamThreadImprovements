using HarmonyLib;
using TMPro;
using UnityEngine;
namespace SteamThreadImprovements.QualityOfLife.HUD;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_WhiteTextInDarkness)]
[HarmonyPatch(typeof(HudManager))]
internal static class WhiteTextInDarkness
{
    [HarmonyPatch("ForceUpdateColor")]
    [HarmonyPostfix]
    private static void ForceUpdateColorPostfix(HudManager __instance)
    {
        var traverse = Traverse.Create(__instance);
        float colorValue = traverse.Field("colorValue").GetValue<float>();
        TMP_Text itemTitle = traverse.Field("itemTitle").GetValue<TMP_Text>();
        TMP_Text[] textBox = traverse.Field("textBox").GetValue<TMP_Text[]>();

        Color targetTextColor = Color.Lerp(Color.white, Color.black, colorValue);

        if (itemTitle != null)
            itemTitle.color = targetTextColor;

        if (textBox != null)
        {
            for (int i = 0; i < textBox.Length; i++)
            {
                if (textBox[i] != null)
                    textBox[i].color = targetTextColor;
            }
        }
    }
}
