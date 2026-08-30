using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Gameplay.NPCs;

[HarmonyPatch(typeof(LookAtGuy), "Respawn")]
internal static class TestRespawn
{
    public static void Postfix(LookAtGuy __instance)
    {
        var spriteRotator = AccessTools.Field(typeof(LookAtGuy), "spriteRotator").GetValue(__instance) as AnimatedSpriteRotator;

        var headTransform = AccessTools.Field(typeof(LookAtGuy), "headTransform").GetValue(__instance) as Transform;

        if (spriteRotator != null)
            spriteRotator.enabled = true;

        if (headTransform != null)
            headTransform.gameObject.SetActive(true);

        __instance.Navigator.Entity.SetVisible(true);
        __instance.Navigator.Entity.SetInteractionState(true);

        __instance.behaviorStateMachine.ChangeState(new LookAtGuy_Inactive(__instance));
    }
}
