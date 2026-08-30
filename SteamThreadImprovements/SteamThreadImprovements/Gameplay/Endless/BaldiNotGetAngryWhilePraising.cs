using HarmonyLib;

namespace SteamThreadImprovements.Gameplay.Endless;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_BaldiNotGetAngryWhilePraising)]
[HarmonyPatch(typeof(Baldi), "GetAngry")]
internal static class BaldiNotGetAngryWhilePraising
{
    internal static bool Prefix(Baldi __instance, float value)
    {
        if (Singleton<BaseGameManager>.Instance is EndlessGameManager)
        {
            if (__instance.behaviorStateMachine.CurrentState is Baldi_Praise && value > 0f)
                return false;
        }
        return true;
    }
}
