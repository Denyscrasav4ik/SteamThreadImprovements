using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace SteamThreadImprovements.Gameplay.Events;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_GravitySlowTime)]
[HarmonyPatch(typeof(PlayerEntity))]
[HarmonyPatch("FlipAnimation", MethodType.Enumerator)]
internal static class GravitySlowTime
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);

        for (int i = 3; i < codes.Count; i++)
        {
            if (codes[i].opcode == OpCodes.Newobj &&
                codes[i].operand is MethodBase method &&
                method.DeclaringType?.Name == "TimeScaleModifier")
            {
                if (codes[i - 1].opcode == OpCodes.Ldc_R4 &&
                    codes[i - 2].opcode == OpCodes.Ldc_R4 &&
                    codes[i - 3].opcode == OpCodes.Ldc_R4)
                {
                    codes[i - 1].operand = 0.5f;
                    codes[i - 2].operand = 0.5f;
                    codes[i - 3].operand = 0.5f;
                }
            }
        }
        return codes;
    }
}
