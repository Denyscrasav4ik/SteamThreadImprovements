using HarmonyLib;

namespace SteamThreadImprovements.Gameplay.Items;

[HarmonyPatch(typeof(ITM_AlarmClock), "Timer", MethodType.Enumerator)]
internal static class AlarmClockForget
{
    [HarmonyPrefix]
    public static void Prefix(ITM_AlarmClock __instance, ref bool __state)
    {
        __state = Traverse.Create(__instance)
            .Field("finished")
            .GetValue<bool>();
    }

    [HarmonyPostfix]
    public static void Postfix(ITM_AlarmClock __instance, bool __state)
    {
        bool finished = Traverse.Create(__instance)
            .Field("finished")
            .GetValue<bool>();

        if (__state || !finished)
            return;

        EnvironmentController ec = Traverse.Create(__instance)
            .Field("ec")
            .GetValue<EnvironmentController>();

        ec.GetBaldi()?.ClearSoundLocations(true, __instance.transform.position);
    }
}
