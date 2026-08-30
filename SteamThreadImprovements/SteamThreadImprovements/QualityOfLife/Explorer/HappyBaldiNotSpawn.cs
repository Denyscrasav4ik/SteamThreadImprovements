using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.QualityOfLife.Explorer;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Qol, ConfigEntryStorage.Name_HappyBaldiNotSpawn)]
[HarmonyPatch(typeof(MainGameManager))]
internal static class HappyBaldiNotSpawn
{
    [HarmonyPatch("CreateHappyBaldi")]
    [HarmonyPrefix]
    private static bool Start(MainGameManager __instance)
    {
        if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Free)
        {
            Singleton<BaseGameManager>.Instance.BeginSpoopMode();
            Singleton<BaseGameManager>.Instance.Ec.SpawnNPCs();
            Baldi baldi = GameObject.FindObjectOfType<Baldi>();
            if (baldi != null)
            {
                baldi.Despawn();
            }
            Singleton<BaseGameManager>.Instance.Ec.StartEventTimers();
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(MainGameManager), "BeginPlay")]
    [HarmonyPostfix]
    private static void Postfix(MainGameManager __instance)
    {
        if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Free)
            Singleton<MusicManager>.Instance.StopMidi();
    }
}
