using System.Collections.Generic;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.AdvancedMap;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_MapDeleteMarkers)]
[HarmonyPatch(typeof(Map), "Update")]
internal static class MapDeleteMarkersPatch
{
    private static readonly AccessTools.FieldRef<Map, List<MapMarker>> Markers = AccessTools.FieldRefAccess<Map, List<MapMarker>>("markers");
    private static readonly AccessTools.FieldRef<Map, bool> AdvancedMode = AccessTools.FieldRefAccess<Map, bool>("advancedMode");
    private static readonly AccessTools.FieldRef<Map, bool> PlacingMarker = AccessTools.FieldRefAccess<Map, bool>("placingMarker");
    private static readonly AccessTools.FieldRef<Map, bool> PlacingSecondPin = AccessTools.FieldRefAccess<Map, bool>("placingSecondPin");
    private static readonly AccessTools.FieldRef<Map, MapMarker> CurrentMapMarker = AccessTools.FieldRefAccess<Map, MapMarker>("currentMapMarker");
    private static readonly AccessTools.FieldRef<Map, SpriteRenderer> MarkerCursor = AccessTools.FieldRefAccess<Map, SpriteRenderer>("markerCursor");
    private static readonly AccessTools.FieldRef<Map, GameObject> MarkerIndicator = AccessTools.FieldRefAccess<Map, GameObject>("markerIndicator");

    static void Postfix(Map __instance)
    {
        KeyboardShortcut? keybind = ConfigEntryStorage.Cfg_MapDeleteMarkersKey?.Value;

        if (keybind == null || !keybind.Value.IsDown() || !AdvancedMode(__instance)) return;

        DeleteAllMarkers(__instance);
    }

    private static void DeleteAllMarkers(Map map)
    {
        PlacingMarker(map) = false;
        PlacingSecondPin(map) = false;

        MapMarker currentMarker = CurrentMapMarker(map);

        if (currentMarker != null)
        {
            currentMarker.UnHighlight();
            CurrentMapMarker(map) = null!;
        }

        List<MapMarker> markers = Markers(map);

        while (markers.Count > 0)
        {
            MapMarker marker = markers[0];

            if (marker == null)
            {
                markers.RemoveAt(0);
                continue;
            }

            map.DestroyMarker(marker);
        }

        if (MarkerCursor(map) != null)
            MarkerCursor(map).gameObject.SetActive(false);

        if (MarkerIndicator(map) != null)
            MarkerIndicator(map).gameObject.SetActive(false);
    }
}
