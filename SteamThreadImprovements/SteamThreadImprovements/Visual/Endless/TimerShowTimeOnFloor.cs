using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace SteamThreadImprovements.Visual.Endless;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Visual, ConfigEntryStorage.Name_TimerShowTimeOnFloor)]
[HarmonyPatch]
internal static class TimerShowTimeOnFloor
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(Map), "Update");
        yield return AccessTools.Method(typeof(Elevator), "Update");
    }

    private static readonly FieldInfo TimeField = AccessTools.Field(typeof(BaseGameManager), "time");

    private static int GetDisplayTime(EnvironmentController ec) =>
        Singleton<BaseGameManager>.Instance is EndlessGameManager
            ? (int)(float)TimeField.GetValue(Singleton<BaseGameManager>.Instance)
            : ec.RemainingTime;

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var getter = AccessTools.PropertyGetter(typeof(EnvironmentController), "RemainingTime");
        var replacement = AccessTools.Method(typeof(TimerShowTimeOnFloor), "GetDisplayTime");

        foreach (var code in instructions)
        {
            if (code.Calls(getter))
            {
                var newCode = new CodeInstruction(OpCodes.Call, replacement);
                newCode.labels = code.labels;
                newCode.blocks = code.blocks;

                yield return newCode;
            }
            else
            {
                yield return code;
            }
        }
    }
}
