using BepInEx.Configuration;
using UnityEngine;

namespace SteamThreadImprovements;

public static class ConfigEntryStorage
{
    internal static void InitializeConfigs(ConfigFile config)
    {
        Cfg_SquishedNpcVoiceline = config.Bind(Category_Qol, Name_SquishedNpcVoiceline, true, Desc_SquishedNpcVoiceline);
        Cfg_AlwaysFieldtripBusPass = config.Bind(Category_Qol, Name_AlwaysFieldtripBusPass, true, Desc_AlwaysFieldtripBusPass);
        Cfg_DeafBaldicator = config.Bind(Category_Qol, Name_DeafBaldicator, true, Desc_DeafBaldicator);
        Cfg_ElevatorNotGoOut = config.Bind(Category_Qol, Name_ElevatorNotGoOut, true, Desc_ElevatorNotGoOut);
        Cfg_HappyBaldiNotSpawn = config.Bind(Category_Qol, Name_HappyBaldiNotSpawn, true, Desc_HappyBaldiNotSpawn);
        Cfg_MoveFieldTripItems = config.Bind(Category_Qol, Name_MoveFieldTripItems, true, Desc_MoveFieldTripItems);
        Cfg_MapReturnCamera = config.Bind(Category_Qol, Name_MapReturnCamera, true, Desc_MapReturnCamera);
        Cfg_MapDeleteMarkers = config.Bind(Category_Qol, Name_MapDeleteMarkers, true, Desc_MapDeleteMarkers);
        Cfg_UnapplicableStickers = config.Bind(Category_Qol, Name_UnapplicableStickers, true, Desc_UnapplicableStickers);
        Cfg_BetterStickerStack = config.Bind(Category_Qol, Name_BetterStickerStack, true, Desc_BetterStickerStack);
        Cfg_WhiteTextInDarkness = config.Bind(Category_Qol, Name_WhiteTextInDarkness, true, Desc_WhiteTextInDarkness);
        Cfg_FieldTripLoadingScreenOrderFix = config.Bind(Category_Qol, Name_FieldTripLoadingScreenOrderFix, true, Desc_FieldTripLoadingScreenOrderFix);
        Cfg_FlashingNotebookActivityIcon = config.Bind(Category_Qol, Name_FlashingNotebookActivityIcon, true, Desc_FlashingNotebookActivityIcon);
        Cfg_FlashingOpenElevatorIcon = config.Bind(Category_Qol, Name_FlashingOpenElevatorIcon, true, Desc_FlashingOpenElevatorIcon);
        Cfg_BonusQuestionMapIcon = config.Bind(Category_Qol, Name_BonusQuestionMapIcon, true, Desc_BonusQuestionMapIcon);
        Cfg_PickupIconFix = config.Bind(Category_Qol, Name_PickupIconFix, true, Desc_PickupIconFix);
        Cfg_StickerFallOff = config.Bind(Category_Qol, Name_StickerFallOff, true, Desc_StickerFallOff);
        Cfg_OpenStickerScreenIfNoMap = config.Bind(Category_Qol, Name_OpenStickerScreenIfNoMap, true, Desc_OpenStickerScreenIfNoMap);
        Cfg_MakeNoiseOnBlockedSticker = config.Bind(Category_Qol, Name_MakeNoiseOnBlockedSticker, true, Desc_MakeNoiseOnBlockedSticker);

        Cfg_GravitySlowTime = config.Bind(Category_Gameplay, Name_GravitySlowTime, true, Desc_GravitySlowTime);
        Cfg_TestTeleportTillNoHidden = config.Bind(Category_Gameplay, Name_TestTeleportTillNoHidden, true, Desc_TestTeleportTillNoHidden);
        Cfg_TestDoNothingUntilLookedAtOneSecond = config.Bind(Category_Gameplay, Name_TestDoNothingUntilLookedAtOneSecond, false, Desc_TestDoNothingUntilLookedAtOneSecond);
        Cfg_TestNotFleeUntilOneSecond = config.Bind(Category_Gameplay, Name_TestNotFleeUntilOneSecond, false, Desc_TestNotFleeUntilOneSecond);
        Cfg_TestLessTolerantButMoreCalm = config.Bind(Category_Gameplay, Name_TestLessTolerantButMoreCalm, false, Desc_TestLessTolerantButMoreCalm);
        Cfg_TestRespawn = config.Bind(Category_Gameplay, Name_TestRespawn, true, Desc_TestRespawn);
        Cfg_CraftersRespawnInEndless = config.Bind(Category_Gameplay, Name_CraftersRespawnInEndless, true, Desc_CraftersRespawnInEndless);
        Cfg_AlarmClockForget = config.Bind(Category_Gameplay, Name_AlarmClockForget, true, Desc_AlarmClockForget);
        Cfg_BaldiNotGetAngryWhilePraising = config.Bind(Category_Gameplay, Name_BaldiNotGetAngryWhilePraising, true, Desc_BaldiNotGetAngryWhilePraising);
        Cfg_BaldiSlowOnActivity = config.Bind(Category_Gameplay, Name_BaldiSlowOnActivity, true, Desc_BaldiSlowOnActivity);
        Cfg_EventsKeepHappening = config.Bind(Category_Gameplay, Name_EventsKeepHappening, true, Desc_EventsKeepHappening);
        Cfg_DetentionTimerScaleBack = config.Bind(Category_Gameplay, Name_DetentionTimerScaleBack, true, Desc_DetentionTimerScaleBack);
        Cfg_SodaMachineRefillOnNotebook = config.Bind(Category_Gameplay, Name_SodaMachineRefillOnNotebook, true, Desc_SodaMachineRefillOnNotebook);

        Cfg_AllDoorsOnMap = config.Bind(Category_Visual, Name_AllDoorsOnMap, true, Desc_AllDoorsOnMap);
        Cfg_BaldiOnlyTakeItemsHeWant = config.Bind(Category_Visual, Name_BaldiOnlyTakeItemsHeWant, true, Desc_BaldiOnlyTakeItemsHeWant);
        Cfg_NotebookCounterGoDownWithTV = config.Bind(Category_Visual, Name_NotebookCounterGoDownWithTV, true, Desc_NotebookCounterGoDownWithTV);
        Cfg_SpeedUpStickers = config.Bind(Category_Visual, Name_SpeedUpStickers, true, Desc_SpeedUpStickers);
        Cfg_TimerShowTimeOnFloor = config.Bind(Category_Visual, Name_TimerShowTimeOnFloor, true, Desc_TimerShowTimeOnFloor);

        Cfg_MapReturnCameraKey = config.Bind(Category_Keybinds, Name_MapReturnCameraKey, new KeyboardShortcut(KeyCode.Home), Desc_MapReturnCameraKey);
        Cfg_MapDeleteMarkersKey = config.Bind(Category_Keybinds, Name_MapDeleteMarkersKey, new KeyboardShortcut(KeyCode.Delete), Desc_MapDeleteMarkersKey);
    }

    public const string
        Category_Qol = "Quality of Life",
        Name_SquishedNpcVoiceline = "Squished NPC Voiceline", Desc_SquishedNpcVoiceline = "If set to True, NPCs will have higher pitched voicelines when squished.",
        Name_AlwaysFieldtripBusPass = "Always Fieldtrip Bus Pass", Desc_AlwaysFieldtripBusPass = "If set to True, a Field Trip will always appear if the player has a Bus Pass.",
        Name_DeafBaldicator = "Deaf Baldicator", Desc_DeafBaldicator = "If set to True, the Deaf Baldicator will be shown when Baldi doesn't hear a sound.",
        Name_ElevatorNotGoOut = "Elevators Never Break In Explorer", Desc_ElevatorNotGoOut = "If set to True, elevators will not go out of order in Explorer mode.",
        Name_HappyBaldiNotSpawn = "No Happy Baldi In Explorer", Desc_HappyBaldiNotSpawn = "If set to True, Happy Baldi will not spawn in Explorer mode.",
        Name_MoveFieldTripItems = "Move Field Trip Items", Desc_MoveFieldTripItems = "If set to True, Field Trip items will be moved to the center of the Pit Stop, instead of the Field Trip room.",
        Name_MapReturnCamera = "Map Return Camera", Desc_MapReturnCamera = "If set to True, enables the keybind that returns the map camera to the player's position.",
        Name_MapDeleteMarkers = "Map Delete Markers", Desc_MapDeleteMarkers = "If set to True, enables the keybind that deletes all map markers.",
        Name_UnapplicableStickers = "Unapplicable Stickers", Desc_UnapplicableStickers = "If set to True, Signal Boost and Exploration Bonus stickers will be unapplicable after the map has already been purchased.",
        Name_BetterStickerStack = "Better Sticker Stack", Desc_BetterStickerStack = "If set to True, if you pick up a stack of a sticker, you will visually only pick up one of them.",
        Name_WhiteTextInDarkness = "White Text in Darkness", Desc_WhiteTextInDarkness = "If set to True, item and notebook text fades to white when the HUD darkens.",
        Name_FieldTripLoadingScreenOrderFix = "Field Trip Loading Screen Order Fix", Desc_FieldTripLoadingScreenOrderFix = "If set to True, Johnny will bring the items from the Field Trips after the loading screen finishes, instead of before.",
        Name_FlashingNotebookActivityIcon = "Flashing Notebook Activity Icon", Desc_FlashingNotebookActivityIcon = "If set to True, the notebook icon will flash when the activity is completed but the notebook is not yet collected.",
        Name_FlashingOpenElevatorIcon = "Flashing Open Elevator Icon", Desc_FlashingOpenElevatorIcon = "If set to True, the elevator icon will flash when the elevator is open and won't break.",
        Name_BonusQuestionMapIcon = "Bonus Question Map Icon", Desc_BonusQuestionMapIcon = "If set to True, an icon will appear on the map for activities with an active bonus question.",
        Name_PickupIconFix = "Pickup Icon Fix", Desc_PickupIconFix = "If set to True, the map icon for pickups will appear even if they were created after the intial generation.",
        Name_StickerFallOff = "Sticker Fall Off", Desc_StickerFallOff = "If set to True, once a sticker expires, it will play a sound.",
        Name_OpenStickerScreenIfNoMap = "Open Sticker Screen If No Map", Desc_OpenStickerScreenIfNoMap = "If set to True, the sticker screen will open instead of the map when the map is unavailable.",
        Name_MakeNoiseOnBlockedSticker = "Make Noise On Blocked Sticker", Desc_MakeNoiseOnBlockedSticker = "If set to True, a noise will be played when you fail to apply a sticker.",

        Category_Gameplay = "Gameplay",
        Name_GravitySlowTime = "Gravity Slow Time", Desc_GravitySlowTime = "If set to True, gravity will slow down time instead of completely stopping it.",
        Name_TestTeleportTillNoHidden = "Test Teleport Till No Hidden", Desc_TestTeleportTillNoHidden = "If set to True, if the Test is hidden, it will teleport until it's unhidden.",
        Name_TestDoNothingUntilLookedAtOneSecond = "Test Do Nothing Until Looked At One Second", Desc_TestDoNothingUntilLookedAtOneSecond = "If set to True, the Test will do nothing until the player looks at it for at least 1 second.",
        Name_TestNotFleeUntilOneSecond = "Test Not Flee Until One Second", Desc_TestNotFleeUntilOneSecond = "If set to True, the Test will not flee until the player looked away from it for at least 1 second.",
        Name_TestLessTolerantButMoreCalm = "Test Less Tolerant But More Calm", Desc_TestLessTolerantButMoreCalm = "If set to True, the Test will explode much quicker, but the explosion timer will go down much quicker as well.",
        Name_TestRespawn = "Test Respawn", Desc_TestRespawn = "If set to True, the Test will be able to respawn after it's effect ends.",
        Name_CraftersRespawnInEndless = "Crafters Respawn In Endless", Desc_CraftersRespawnInEndless = "If set to True, Arts and Crafters will respawn in Endless mode after 3 minutes.",
        Name_AlarmClockForget = "Alarm Clock Forget", Desc_AlarmClockForget = "If set to True, the Alarm Clock will clear Baldi's sound queue.",
        Name_BaldiNotGetAngryWhilePraising = "Baldi Not Get Angry While Praising", Desc_BaldiNotGetAngryWhilePraising = "If set to True, Baldi will not get angrier while praising in Endless mode.",
        Name_BaldiSlowOnActivity = "Baldi Slow On Activity", Desc_BaldiSlowOnActivity = "If set to True, Baldi will slow down on activity completion instead of collecting notebooks in Endless mode.",
        Name_EventsKeepHappening = "Events Keep Happening", Desc_EventsKeepHappening = "If set to True, events will keep happening after all them have gone through in Endless mode.",
        Name_DetentionTimerScaleBack = "Detention Timer Scale Back", Desc_DetentionTimerScaleBack = "If set to True, the detention timer will scale back in Endless mode if the player hasn't gotten detention in a while.",
        Name_SodaMachineRefillOnNotebook = "Soda Machine Refill On Notebook", Desc_SodaMachineRefillOnNotebook = "If set to True, a random soda machine will be refilled whenever a notebook is collected in Endless mode.",

        Category_Visual = "Visual",
        Name_AllDoorsOnMap = "All Doors On Map", Desc_AllDoorsOnMap = "If set to True, all doors will be visible on the map.",
        Name_BaldiOnlyTakeItemsHeWant = "Baldi Only Take Items He Wants", Desc_BaldiOnlyTakeItemsHeWant = "If set to True, Baldi will only take items he actually wants in Picnic Panic.",
        Name_NotebookCounterGoDownWithTV = "Notebook Counter Go Down With TV", Desc_NotebookCounterGoDownWithTV = "If set to True, the notebook counter will go down with the Baldi TV.",
        Name_SpeedUpStickers = "Speed Up Stickers", Desc_SpeedUpStickers = "If set to True, sticker animations will speed up as more stickers are queued.",
        Name_TimerShowTimeOnFloor = "Timer Show Time On Floor", Desc_TimerShowTimeOnFloor = "If set to True, the timer will show the time you've spent on floor in Endless mode.",

        Category_Keybinds = "Keybinds",
        Name_MapReturnCameraKey = "Map Return Camera", Desc_MapReturnCameraKey = "Keybind used to return the map camera to the player's position.",
        Name_MapDeleteMarkersKey = "Map Delete Markers", Desc_MapDeleteMarkersKey = "Keybind used to delete all map markers.";

    internal static ConfigEntry<bool>?
        Cfg_SquishedNpcVoiceline,
        Cfg_AlwaysFieldtripBusPass,
        Cfg_DeafBaldicator,
        Cfg_ElevatorNotGoOut,
        Cfg_HappyBaldiNotSpawn,
        Cfg_MoveFieldTripItems,
        Cfg_MapReturnCamera,
        Cfg_MapDeleteMarkers,
        Cfg_UnapplicableStickers,
        Cfg_BetterStickerStack,
        Cfg_WhiteTextInDarkness,
        Cfg_FieldTripLoadingScreenOrderFix,
        Cfg_FlashingNotebookActivityIcon,
        Cfg_FlashingOpenElevatorIcon,
        Cfg_BonusQuestionMapIcon,
        Cfg_PickupIconFix,
        Cfg_StickerFallOff,
        Cfg_OpenStickerScreenIfNoMap,
        Cfg_MakeNoiseOnBlockedSticker,

        Cfg_GravitySlowTime,
        Cfg_TestTeleportTillNoHidden,
        Cfg_TestNotFleeUntilOneSecond,
        Cfg_TestDoNothingUntilLookedAtOneSecond,
        Cfg_TestLessTolerantButMoreCalm,
        Cfg_TestRespawn,
        Cfg_CraftersRespawnInEndless,
        Cfg_AlarmClockForget,
        Cfg_BaldiNotGetAngryWhilePraising,
        Cfg_BaldiSlowOnActivity,
        Cfg_EventsKeepHappening,
        Cfg_DetentionTimerScaleBack,
        Cfg_SodaMachineRefillOnNotebook,

        Cfg_AllDoorsOnMap,
        Cfg_BaldiOnlyTakeItemsHeWant,
        Cfg_NotebookCounterGoDownWithTV,
        Cfg_SpeedUpStickers,
        Cfg_TimerShowTimeOnFloor;

    internal static ConfigEntry<KeyboardShortcut>?
        Cfg_MapReturnCameraKey,
        Cfg_MapDeleteMarkersKey;
}
