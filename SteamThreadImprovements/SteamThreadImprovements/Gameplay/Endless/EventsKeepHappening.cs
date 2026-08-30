using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SteamThreadImprovements.Gameplay.Endless;

[ConditionalPatchConfig(ConfigEntryStorage.Category_Gameplay, ConfigEntryStorage.Name_EventsKeepHappening)]
[HarmonyPatch(typeof(RandomEvent), "End")]
internal static class EventsKeepHappening
{
    static readonly AccessTools.FieldRef<RandomEvent, EnvironmentController> EC = AccessTools.FieldRefAccess<RandomEvent, EnvironmentController>("ec");
    static readonly AccessTools.FieldRef<EnvironmentController, AudioManager> AUD = AccessTools.FieldRefAccess<EnvironmentController, AudioManager>("audMan");
    static readonly AccessTools.FieldRef<EnvironmentController, SoundObject> NOTIFY = AccessTools.FieldRefAccess<EnvironmentController, SoundObject>("audEventNotification");
    static readonly AccessTools.FieldRef<EnvironmentController, List<RandomEvent>> EVENTS = AccessTools.FieldRefAccess<EnvironmentController, List<RandomEvent>>("currentEvents");
    static readonly AccessTools.FieldRef<EnvironmentController, List<RandomEventType>> TYPES = AccessTools.FieldRefAccess<EnvironmentController, List<RandomEventType>>("currentEventTypes");

    [HarmonyPostfix]
    static void Postfix(RandomEvent __instance)
    {
        if (Singleton<BaseGameManager>.Instance is EndlessGameManager && EC(__instance) is EnvironmentController ec)
            ec.StartCoroutine(Restart(__instance, ec));
    }

    static IEnumerator Restart(RandomEvent e, EnvironmentController ec)
    {
        float t = UnityEngine.Random.Range(60f, 120f);
        while ((t -= Time.deltaTime * ec.EnvironmentTimeScale) > 0f) yield return null;
        while (EVENTS(ec).Count > 0) yield return null;

        e.SetEventTime(new System.Random(UnityEngine.Random.Range(int.MinValue, int.MaxValue)));
        AUD(ec).PlaySingle(e.EventJingleOverride ?? NOTIFY(ec));
        Singleton<CoreGameManager>.Instance.GetHud(0).BaldiTv.AnnounceEvent(e.EventIntro);

        t = 3f;
        while ((t -= Time.deltaTime * ec.EnvironmentTimeScale) > 0f) yield return null;
        while (EVENTS(ec).Count > 0) yield return null;

        e.Begin();
        if (!EVENTS(ec).Contains(e)) EVENTS(ec).Add(e);
        if (!TYPES(ec).Contains(e.Type)) TYPES(ec).Add(e.Type);
    }
}
