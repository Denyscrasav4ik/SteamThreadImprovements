using HarmonyLib;

namespace SteamThreadImprovements.Gameplay.Endless;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_BaldiSlowOnActivity)]
[HarmonyPatch]
internal static class BaldiSlowOnActivity
{
    internal static bool lastWasNoActivity = false;

    [HarmonyPatch(typeof(Activity), "Completed", new System.Type[] { typeof(int) })]
    [HarmonyPostfix]
    static void ActivityCompleted_Postfix(Activity __instance)
    {
        if (__instance is NoActivity) return;

        if (Singleton<BaseGameManager>.Instance is EndlessGameManager endlessManager)
            endlessManager.AngerBaldi(-1f);
    }

    [HarmonyPatch(typeof(EndlessGameManager), "CollectNotebook")]
    [HarmonyPrefix]
    internal static void CollectNotebook_Prefix(Notebook notebook) => lastWasNoActivity = notebook.activity is NoActivity;

    [HarmonyPatch(typeof(EndlessGameManager), "CollectNotebooks")]
    [HarmonyPostfix]
    internal static void CollectNotebooks_Postfix(EndlessGameManager __instance, int count)
    {
        if (!lastWasNoActivity)
            __instance.AngerBaldi(1f * count);

        lastWasNoActivity = false;
    }
}
