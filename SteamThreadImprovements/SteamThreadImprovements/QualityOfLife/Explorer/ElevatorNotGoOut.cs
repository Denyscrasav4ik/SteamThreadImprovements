using HarmonyLib;

namespace SteamThreadImprovements.QualityOfLife.Explorer;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_ElevatorNotGoOut)]
[HarmonyPatch(typeof(ElevatorManager))]
internal static class ElevatorNotGoOut
{
    [HarmonyPatch("ShouldFail")]
    [HarmonyPrefix]
    private static bool ShouldFailPrefix(ref bool __result)
    {
        if (Singleton<CoreGameManager>.Instance != null && Singleton<CoreGameManager>.Instance.currentMode == Mode.Free)
        {
            __result = false;
            return false;
        }
        return true;
    }

    [HarmonyPatch("ExitAvailable", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool ExitAvailablePrefix(ref bool __result)
    {
        if (Singleton<CoreGameManager>.Instance != null && Singleton<CoreGameManager>.Instance.currentMode == Mode.Free)
        {
            __result = Singleton<BaseGameManager>.Instance.AllNotebooksFound;
            return false;
        }
        return true;
    }
}
