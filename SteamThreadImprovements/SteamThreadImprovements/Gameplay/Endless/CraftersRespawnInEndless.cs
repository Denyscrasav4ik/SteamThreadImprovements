using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Gameplay.Endless;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_CraftersRespawnInEndless)]
[HarmonyPatch(typeof(ArtsAndCrafters), "DisappearForever")]
internal static class CraftersRespawnInEndless
{
    private const float RespawnDelay = 180f;

    private static readonly AccessTools.FieldRef<ArtsAndCrafters, List<Cell>> SpawnTilesRef = AccessTools.FieldRefAccess<ArtsAndCrafters, List<Cell>>("spawnTiles");

    public static void Postfix(ArtsAndCrafters __instance)
    {
        EndlessGameManager? endlessManager = Singleton<BaseGameManager>.Instance as EndlessGameManager;

        if (endlessManager == null) return;

        endlessManager.StartCoroutine(RespawnCoroutine(__instance));
    }

    private static IEnumerator RespawnCoroutine(ArtsAndCrafters crafters)
    {
        yield return new WaitForSeconds(RespawnDelay);

        if (crafters == null || Singleton<BaseGameManager>.Instance is not EndlessGameManager)
            yield break;

        List<Cell> spawnTiles = SpawnTilesRef(crafters);

        if (spawnTiles == null || spawnTiles.Count == 0)
            yield break;

        Cell spawnCell = spawnTiles[Random.Range(0, spawnTiles.Count)];

        crafters.state = new ArtsAndCrafters_Waiting(crafters);
        crafters.behaviorStateMachine.ChangeState(crafters.state);

        crafters.gameObject.SetActive(true);
        crafters.Hide(false);
        crafters.Teleport(spawnCell.position);
        crafters.SpawnAt(spawnCell.position);
    }
}
