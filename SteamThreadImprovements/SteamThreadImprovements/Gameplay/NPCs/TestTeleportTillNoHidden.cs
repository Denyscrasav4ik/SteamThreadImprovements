using HarmonyLib;

namespace SteamThreadImprovements.Gameplay.NPCs;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_TestTeleportTillNoHidden)]
[HarmonyPatch(typeof(LookAtGuy), "VirtualUpdate")]
internal static class TestTeleportTillNoHidden
{
    public static void Postfix(LookAtGuy __instance)
    {
        if (__instance.Navigator != null && __instance.Navigator.Entity != null && __instance.Navigator.Entity.Hidden)
        {
            Cell randomCell = __instance.ec.ControlledRandomCell(false, false, true, null);
            if (randomCell != null)
            {
                __instance.Navigator.Entity.Teleport(randomCell.FloorWorldPosition);
            }
        }
    }
}
