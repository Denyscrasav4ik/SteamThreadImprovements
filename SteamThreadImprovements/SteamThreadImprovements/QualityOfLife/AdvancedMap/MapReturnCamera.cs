using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.AdvancedMap;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_MapReturnCamera)]
[HarmonyPatch(typeof(Map), "Update")]
internal static class MapReturnCameraPatch
{
    private static readonly AccessTools.FieldRef<Map, bool> AdvancedMode = AccessTools.FieldRefAccess<Map, bool>("advancedMode");
    private static readonly AccessTools.FieldRef<Map, Vector3> Position = AccessTools.FieldRefAccess<Map, Vector3>("_position");
    private static readonly AccessTools.FieldRef<Map, Vector3> Rotation = AccessTools.FieldRefAccess<Map, Vector3>("_rotation");
    private static readonly AccessTools.FieldRef<Map, float> Zoom = AccessTools.FieldRefAccess<Map, float>("zoom");
    private static readonly AccessTools.FieldRef<Map, float> DefaultZoom = AccessTools.FieldRefAccess<Map, float>("defaultZoom");

    static void Postfix(Map __instance)
    {
        KeyboardShortcut? keybind = ConfigEntryStorage.Cfg_MapReturnCameraKey?.Value;

        if (keybind == null || !keybind.Value.IsDown() || !AdvancedMode(__instance)) return;

        if (__instance.cams == null || __instance.cams.Count == 0 || __instance.targets == null || __instance.targets.Count == 0 || __instance.targets[0] == null)
            return;

        Camera mapCamera = __instance.cams[0];
        Transform player = __instance.targets[0].transform;

        Vector3 position = Position(__instance);

        position.x = player.position.x / 10f - 0.5f;
        position.y = player.position.z / 10f - 0.5f;
        position.z = -10f;

        Vector3 rotation = Rotation(__instance);
        rotation.z = 0f;

        float defaultZoom = DefaultZoom(__instance);

        Zoom(__instance) = defaultZoom;
        Position(__instance) = position;
        Rotation(__instance) = rotation;

        mapCamera.orthographicSize = defaultZoom;
        mapCamera.transform.position = position;
        mapCamera.transform.localEulerAngles = rotation;
    }
}
