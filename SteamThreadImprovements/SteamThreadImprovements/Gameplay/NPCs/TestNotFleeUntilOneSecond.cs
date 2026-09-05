using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Gameplay.NPCs;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_TestNotFleeUntilOneSecond)]
[HarmonyPatch]
internal static class TestNotFleeUntilOneSecond
{
    private const float LookAwayTime = 1f;

    private sealed class State
    {
        public float timer = -1f;
        public PlayerManager? player;
    }

    private static readonly ConditionalWeakTable<LookAtGuy_Active, State> States = new();

    [HarmonyPatch(typeof(LookAtGuy_Active), "PlayerLost")]
    [HarmonyPrefix]
    static bool LookAtGuyActivePlayerLost_Prefix(LookAtGuy_Active __instance, PlayerManager player)
    {
        var state = Traverse.Create(__instance);
        var data = States.GetOrCreateValue(__instance);

        if (state.Field("playerSaw").GetValue<bool>())
        {
            state.Field("seesPlayer").SetValue(false);
            state.Field("playerSees").SetValue(false);

            data.player = player;
            data.timer = LookAwayTime;
        }

        return false;
    }

    [HarmonyPatch(typeof(LookAtGuy_Active), "Update")]
    [HarmonyPostfix]
    static void LookAtGuyActiveUpdate_Postfix(LookAtGuy_Active __instance)
    {
        var state = Traverse.Create(__instance);
        var data = States.GetOrCreateValue(__instance);

        if (state.Field("playerSees").GetValue<bool>())
        {
            data.timer = -1f;
            data.player = null;
            return;
        }

        if (data.timer <= 0f) return;

        data.timer -= Time.deltaTime;

        if (data.timer <= 0f)
        {
            var test = state.Field("theTest").GetValue<LookAtGuy>();
            var player = data.player;

            data.timer = -1f;
            data.player = null;

            if (test != null && player != null)
            {
                test.FleePlayer(player);
            }
        }
    }
}
