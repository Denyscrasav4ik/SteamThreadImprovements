using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Gameplay.NPCs;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_TestLessTolerantButMoreCalm)]
[HarmonyPatch(typeof(LookAtGuy_Active))]
internal static class TestLessTolerantButMoreCalm
{
    private const float NewMaxPressure = 10f;
    private const float NewRecoveryRate = 2f;

    [HarmonyPatch("Update")]
    [HarmonyPrefix]
    private static void UpdatePrefix(LookAtGuy_Active __instance) =>
        Traverse.Create(__instance).Field("maxPressure").SetValue(NewMaxPressure);

    [HarmonyPatch("Update")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Ldc_R4 &&
                instruction.operand is float value &&
                Mathf.Approximately(value, 0.2f))
            {
                instruction.operand = NewRecoveryRate;
            }

            yield return instruction;
        }
    }
}
