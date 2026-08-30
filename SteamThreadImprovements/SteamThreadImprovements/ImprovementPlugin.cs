using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace SteamThreadImprovements;

[BepInPlugin("denyscrasav4ik.thedumbfactory.steamthreadimprovements", "Steam Thread Improvements", "1.0.0")]
public class ImprovementPlugin : BaseUnityPlugin
{
    public static ImprovementPlugin? Instance { get; private set; }
    internal static ConfigFile? file;

    public static Sprite? deafBaldicator;
    public static Sprite? bonusQuestionIcon;

    public static SoundObject? stickerFallOff;

    public static TextAsset? localization;
    public static LocalizationData? localizationData;

    public void Awake()
    {
        file = Config;
        Instance = this;

        ConfigEntryStorage.InitializeConfigs(Config);

        LoadAssets();

        new Harmony("denyscrasav4ik.thedumbfactory.steamthreadimprovements").PatchAllConditionals();
    }

    private void LoadAssets()
    {
        string bundleName = true switch
        {
            _ when RuntimeInformation.IsOSPlatform(OSPlatform.Windows) => "SteamThreadImprovements.Resources.assets-win.bundle",
            _ when RuntimeInformation.IsOSPlatform(OSPlatform.OSX) => "SteamThreadImprovements.Resources.assets-mac.bundle",
            _ when RuntimeInformation.IsOSPlatform(OSPlatform.Linux) => "SteamThreadImprovements.Resources.assets-linux.bundle",
            _ => throw new PlatformNotSupportedException("Unsupported Operating System platform.")
        };

        Assembly assembly = typeof(ImprovementPlugin).Assembly;

        using Stream? stream = assembly.GetManifestResourceStream(bundleName);
        using MemoryStream memoryStream = new();

        stream.CopyTo(memoryStream);
        byte[] data = memoryStream.ToArray();
        AssetBundle? bundle = AssetBundle.LoadFromMemory(data);

        deafBaldicator = bundle.LoadAsset<Sprite>("SteamThreadImprovements_BaldicatorDeaf");
        bonusQuestionIcon = bundle.LoadAsset<Sprite>("SteamThreadImprovements_BonusMapIcon");

        stickerFallOff = bundle.LoadAsset<SoundObject>("SteamThreadImprovements_StickerFallOff");

        localization = bundle.LoadAsset<TextAsset>("SteamThreadImprovements_Localization");
        localizationData = JsonUtility.FromJson<LocalizationData>(localization.text);

        bundle.Unload(false);
    }
}

[HarmonyPatch(typeof(LocalizationManager), "LoadLocalizedText")]
public class LocalizationManagerPatches
{
    [HarmonyPostfix]
    static void Postfix(ref Dictionary<string, string> ___localizedText)
    {
        foreach (var item in ImprovementPlugin.localizationData!.items)
        {
            if (___localizedText.ContainsKey(item.key))
                ___localizedText[item.key] = item.value;
            else
                ___localizedText.Add(item.key, item.value);
        }
    }
}
