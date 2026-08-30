using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using SteamThreadImprovements.CustomComponents;

namespace SteamThreadImprovements.QualityOfLife.HUD;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_DeafBaldicator)]
[HarmonyPatch]
internal static class HUDDeafBaldicator
{
    [HarmonyPatch(typeof(HudManager), "Awake"), HarmonyPostfix]
    private static void Awake(HudManager __instance) => DeafBaldicator.Create(__instance);

    [HarmonyPatch(typeof(EnvironmentController), "MakeNoise", typeof(GameObject), typeof(Vector3), typeof(int), typeof(bool))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> MakeNoiseTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        CodeMatcher matcher = new CodeMatcher(instructions, generator);

        matcher.MatchForward(false, new CodeMatch(OpCodes.Mul), new CodeMatch(i => i.opcode == OpCodes.Bge_Un || i.opcode == OpCodes.Bge_Un_S), new CodeMatch(OpCodes.Ret));
        matcher.Advance(2);

        Label blockedLabel = generator.DefineLabel();
        matcher.Insert(Transpilers.EmitDelegate(() => DeafBaldicator.Instance?.Activate()));
        matcher.AddLabels(new[] { blockedLabel });

        matcher.MatchForward(false, new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(EnvironmentController), "silent")), new CodeMatch(i => i.opcode == OpCodes.Brtrue || i.opcode == OpCodes.Brtrue_S));
        matcher.Advance(1);
        matcher.SetOperandAndAdvance(blockedLabel);

        matcher.MatchForward(false, new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Cell), nameof(Cell.Silent))), new CodeMatch(i => i.opcode == OpCodes.Brtrue || i.opcode == OpCodes.Brtrue_S));
        matcher.Advance(1);
        matcher.SetOperandAndAdvance(blockedLabel);

        return matcher.InstructionEnumeration();
    }
}
