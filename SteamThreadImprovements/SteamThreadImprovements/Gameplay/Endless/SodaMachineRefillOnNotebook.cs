using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace SteamThreadImprovements.Gameplay.Endless;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_SodaMachineRefillOnNotebook)]
[HarmonyPatch]
internal static class SodaMachineRefillOnNotebook
{
    public static List<SodaMachine> machines = new List<SodaMachine>();
    public static Dictionary<SodaMachine, Material> originalMaterials = new Dictionary<SodaMachine, Material>();

    [HarmonyPatch(typeof(LevelBuilder), "StartGenerate")]
    [HarmonyPrefix]
    private static void LevelBuilder_StartGenerate_Prefix()
    {
        machines.Clear();
        originalMaterials.Clear();
    }

    [HarmonyPatch(typeof(LevelBuilder), "LoadRoom", new Type[] { typeof(RoomAsset), typeof(IntVector2), typeof(IntVector2), typeof(Direction), typeof(bool), typeof(Texture2D), typeof(Texture2D), typeof(Texture2D) })]
    [HarmonyPostfix]
    private static void LevelBuilder_LoadRoom_Postfix(LevelBuilder __instance)
    {
        List<EnvironmentObject> envObjects = Traverse.Create(__instance).Field("environmentObjects").GetValue<List<EnvironmentObject>>();

        foreach (EnvironmentObject envObj in envObjects)
        {
            if (envObj is SodaMachine machine && !machines.Contains(machine))
            {
                machines.Add(machine);

                MeshRenderer renderer = Traverse.Create(machine).Field("meshRenderer").GetValue<MeshRenderer>();
                if (renderer != null && renderer.sharedMaterials.Length > 1)
                    originalMaterials[machine] = renderer.sharedMaterials[1];
            }
        }
    }

    [HarmonyPatch(typeof(EndlessGameManager), "CollectNotebook")]
    [HarmonyPostfix]
    private static void EndlessGameManager_CollectNotebook_Postfix()
    {
        List<SodaMachine> emptyMachines = new List<SodaMachine>();

        foreach (SodaMachine machine in machines)
        {
            if (machine == null) continue;

            int usesLeft = Traverse.Create(machine).Field("usesLeft").GetValue<int>();
            if (usesLeft <= 0)
                emptyMachines.Add(machine);
        }

        if (emptyMachines.Count > 0)
        {
            SodaMachine chosenMachine = emptyMachines[UnityEngine.Random.Range(0, emptyMachines.Count)];
            Traverse machineTrav = Traverse.Create(chosenMachine);

            machineTrav.Field("usesLeft").SetValue(1);

            MeshRenderer renderer = machineTrav.Field("meshRenderer").GetValue<MeshRenderer>();
            if (renderer != null && originalMaterials.ContainsKey(chosenMachine))
            {
                Material[] mats = renderer.sharedMaterials;
                if (mats.Length > 1)
                {
                    mats[1] = originalMaterials[chosenMachine];
                    renderer.sharedMaterials = mats;
                }
            }
        }
    }
}
