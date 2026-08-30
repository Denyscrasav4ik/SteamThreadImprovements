using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Visual.AdvancedMap;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Visual, ConfigEntryStorage.Name_AllDoorsOnMap)]
[HarmonyPatch(typeof(Door), "Start")]
internal static class AllDoorsOnMap
{
    private static readonly FieldInfo MapUnlockedSpriteField = AccessTools.Field(typeof(StandardDoor), "mapUnlockedSprite");
    private static readonly FieldInfo MapLockedSpriteField = AccessTools.Field(typeof(StandardDoor), "mapLockedSprite");

    [HarmonyPostfix]
    private static void Postfix(Door __instance)
    {
        if (__instance is not ElevatorDoor && __instance is not FacultyOnlyDoor)
            return;

        EnvironmentController ec = __instance.ec;

        if (ec == null || ec.map == null)
            return;

        if (__instance.transform.Find("MapDoorIconA") != null)
            return;

        StandardDoor standardDoor = Object.FindObjectOfType<StandardDoor>();

        if (standardDoor == null)
            return;

        Sprite unlockedSprite = (Sprite)MapUnlockedSpriteField.GetValue(standardDoor);
        Sprite lockedSprite = (Sprite)MapLockedSpriteField.GetValue(standardDoor);

        if (unlockedSprite == null)
            return;

        MapTile aMapTile = ec.map.AddExtraTile(__instance.aTile.position);
        aMapTile.name = "MapDoorIconA";
        aMapTile.SpriteRenderer.sprite = unlockedSprite;
        aMapTile.SpriteRenderer.color = __instance.aTile.room.color;
        aMapTile.transform.rotation = __instance.direction.ToUiRotation();

        MapTile bMapTile = ec.map.AddExtraTile(__instance.bTile.position);
        bMapTile.name = "MapDoorIconB";
        bMapTile.SpriteRenderer.sprite = unlockedSprite;
        bMapTile.SpriteRenderer.color = __instance.bTile.room.color;
        bMapTile.transform.rotation =
            __instance.direction.GetOpposite().ToUiRotation();
    }
}
