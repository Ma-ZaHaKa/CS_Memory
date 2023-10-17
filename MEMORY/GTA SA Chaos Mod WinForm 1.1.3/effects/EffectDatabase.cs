// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.effects.EffectDatabase
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using GTA_SA_Chaos.util;
using System;
using System.Collections.Generic;

namespace GTA_SA_Chaos.effects
{
  internal static class EffectDatabase
  {
    public static List<AbstractEffect> Effects { get; } = new List<AbstractEffect>()
    {
      (AbstractEffect) new FunctionEffect(Category.WeaponsAndHealth, "Weapon Set 1", "ThugsArmoury", "cheat", "weapon_set_1"),
      (AbstractEffect) new FunctionEffect(Category.WeaponsAndHealth, "Weapon Set 2", "ProfessionalsKit", "cheat", "weapon_set_2"),
      (AbstractEffect) new FunctionEffect(Category.WeaponsAndHealth, "Weapon Set 3", "NuttersToys", "cheat", "weapon_set_3"),
      (AbstractEffect) new FunctionEffect(Category.WeaponsAndHealth, "Weapon Set 4", "MinigunMadness", "cheat", "weapon_set_4"),
      (AbstractEffect) new FunctionEffect(Category.WeaponsAndHealth, "Health, Armor, $250k", "INeedSomeHelp", "cheat", "give_health_armor_money"),
      new FunctionEffect(Category.WeaponsAndHealth, "Suicide", "GoodbyeCruelWorld", "cheat", "suicide").DisableRapidFire(),
      (AbstractEffect) new FunctionEffect(Category.WeaponsAndHealth, "Infinite Ammo", "FullClip", "timed_cheat", "infinite_ammo"),
      (AbstractEffect) new FunctionEffect(Category.WantedLevel, "Wanted Level +2 Stars", "TurnUpTheHeat", "cheat", "wanted_plus_two"),
      (AbstractEffect) new FunctionEffect(Category.WantedLevel, "Clear Wanted Level", "TurnDownTheHeat", "cheat", "wanted_clear"),
      (AbstractEffect) new FunctionEffect(Category.WantedLevel, "Never Wanted", "IDoAsIPlease", "timed_cheat", "never_wanted"),
      (AbstractEffect) new FunctionEffect(Category.WantedLevel, "Six Wanted Stars", "BringItOn", "cheat", "wanted_six_stars"),
      (AbstractEffect) new WeatherEffect("Sunny Weather", "PleasantlyWarm", 1),
      (AbstractEffect) new WeatherEffect("Very Sunny Weather", "TooDamnHot", 0),
      (AbstractEffect) new WeatherEffect("Overcast Weather", "DullDullDay", 4),
      (AbstractEffect) new WeatherEffect("Rainy Weather", "StayInAndWatchTV", 16),
      (AbstractEffect) new WeatherEffect("Foggy Weather", "CantSeeWhereImGoing", 9),
      (AbstractEffect) new WeatherEffect("Thunderstorm", "ScottishSummer", 16),
      (AbstractEffect) new WeatherEffect("Sandstorm", "SandInMyEars", 19),
      (AbstractEffect) new FunctionEffect(Category.Spawning, "Get Parachute", "LetsGoBaseJumping", "cheat", "parachute"),
      (AbstractEffect) new FunctionEffect(Category.Spawning, "Get Jetpack", "Rocketman", "cheat", "jetpack"),
      (AbstractEffect) new SpawnVehicleEffect("TimeToKickAss", 432),
      (AbstractEffect) new SpawnVehicleEffect("OldSpeedDemon", 504),
      (AbstractEffect) new SpawnVehicleEffect("DoughnutHandicap", 489),
      (AbstractEffect) new SpawnVehicleEffect("NotForPublicRoads", 502),
      (AbstractEffect) new SpawnVehicleEffect("JustTryAndStopMe", 503),
      (AbstractEffect) new SpawnVehicleEffect("WheresTheFuneral", 442),
      (AbstractEffect) new SpawnVehicleEffect("CelebrityStatus", 409),
      (AbstractEffect) new SpawnVehicleEffect("TrueGrime", 408),
      (AbstractEffect) new SpawnVehicleEffect("18Holes", 457),
      (AbstractEffect) new SpawnVehicleEffect("JumpJet", 520),
      (AbstractEffect) new SpawnVehicleEffect("IWantToHover", 539),
      (AbstractEffect) new SpawnVehicleEffect("OhDude", 425),
      (AbstractEffect) new SpawnVehicleEffect("FourWheelFun", 471),
      (AbstractEffect) new SpawnVehicleEffect("ItsAllBull", 486),
      (AbstractEffect) new SpawnVehicleEffect("FlyingToStunt", 513),
      (AbstractEffect) new SpawnVehicleEffect("MonsterMash", 556),
      (AbstractEffect) new SpawnVehicleEffect("SurpriseDriver", -1),
      (AbstractEffect) new FunctionEffect(Category.Time, "0.25x Game Speed", "MatrixMode", "timed_effect", "quarter_gamespeed"),
      (AbstractEffect) new FunctionEffect(Category.Time, "0.5x Game Speed", "SlowItDown", "timed_effect", "half_gamespeed"),
      (AbstractEffect) new FunctionEffect(Category.Time, "2x Game Speed", "SpeedItUp", "timed_effect", "double_gamespeed"),
      (AbstractEffect) new FunctionEffect(Category.Time, "4x Game Speed", "YoureTooSlow", "timed_effect", "quadruple_gamespeed"),
      (AbstractEffect) new FunctionEffect(Category.Time, "Always Midnight", "NightProwler", "timed_cheat", "always_midnight"),
      (AbstractEffect) new FunctionEffect(Category.Time, "Stop Game Clock, Orange Sky", "DontBringOnTheNight", "timed_cheat", "orange_sky"),
      (AbstractEffect) new FunctionEffect(Category.Time, "Faster Clock", "TimeJustFliesBy", "timed_cheat", "faster_clock"),
      new FunctionEffect(Category.VehiclesTraffic, "Blow Up All Cars", "AllCarsGoBoom", "cheat", "blow_up_all_cars").DisableRapidFire(),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Pink Traffic", "PinkIsTheNewCool", "timed_cheat", "pink_traffic"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Black Traffic", "SoLongAsItsBlack", "timed_cheat", "black_traffic"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Cheap Cars", "EveryoneIsPoor", "timed_cheat", "cheap_cars"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Expensive Cars", "EveryoneIsRich", "timed_cheat", "expensive_cars"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Insane Handling", "StickLikeGlue", "timed_cheat", "insane_handling"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "All Green Lights", "DontTryAndStopMe", "timed_cheat", "all_green_lights"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Cars On Water", "JesusTakeTheWheel", "timed_cheat", "cars_on_water"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Boats Fly", "FlyingFish", "timed_cheat", "boats_fly"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Cars Fly", "ChittyChittyBangBang", "timed_cheat", "cars_fly"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Smash N' Boom", "TouchMyCarYouDie", "timed_cheat", "smash_n_boom"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "All Cars Have Nitro", "SpeedFreak", "timed_cheat", "all_cars_nitro"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Cars Float Away When Hit", "BubbleCars", "timed_cheat", "bubble_cars"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "Reduced Traffic", "GhostTown", "timed_cheat", "reduced_traffic"),
      (AbstractEffect) new FunctionEffect(Category.VehiclesTraffic, "All Taxis Have Nitrous", "SpeedyTaxis", "timed_cheat", "all_taxis_nitro"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Peds Attack Each Other", "RoughNeighbourhood", "timed_cheat", "rough_neighbourhood"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Have A Bounty On Your Head", "StopPickingOnMe", "timed_cheat", "bounty_on_your_head"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Elvis Is Everywhere", "BlueSuedeShoes", "timed_cheat", "elvis_lives"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Peds Attack You With Rockets", "AttackOfTheVillagePeople", "timed_cheat", "village_people"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Gang Members Everywhere", "OnlyHomiesAllowed", "timed_cheat", "only_homies"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Gangs Control The Streets", "BetterStayIndoors", "timed_cheat", "stay_indoors"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Riot Mode", "StateOfEmergency", "timed_cheat", "riot_mode"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Everyone Armed", "SurroundedByNutters", "timed_cheat", "everyone_armed"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Aggressive Drivers", "AllDriversAreCriminals", "timed_cheat", "aggressive_drivers"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Recruit Anyone (9mm)", "WannaBeInMyGang", "timed_cheat", "recruit_9mm"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Recruit Anyone (AK-47)", "NoOneCanStopUs", "timed_cheat", "recruit_ak47"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Recruit Anyone (Rockets)", "RocketMayhem", "timed_cheat", "recruit_rockets"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Prostitutes Pay You", "ReverseHooker", "timed_cheat", "reverse_hooker"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Beach Party", "LifesABeach", "timed_cheat", "beach_party"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Ninja Theme", "NinjaTown", "timed_cheat", "ninja_theme"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Kinky Theme", "LoveConquersAll", "timed_cheat", "kinky_theme"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Funhouse Theme", "CrazyTown", "timed_cheat", "funhouse_theme"),
      (AbstractEffect) new FunctionEffect(Category.PedsAndCo, "Country Traffic", "HicksVille", "timed_cheat", "country_traffic"),
      (AbstractEffect) new FunctionEffect(Category.PlayerModifications, "Weapon Aiming While Driving", "IWannaDriveBy", "timed_cheat", "drive_by"),
      (AbstractEffect) new FunctionEffect(Category.PlayerModifications, "Huge Bunny Hop", "CJPhoneHome", "timed_cheat", "huge_bunny_hop"),
      (AbstractEffect) new FunctionEffect(Category.PlayerModifications, "Mega Jump", "Kangaroo", "timed_cheat", "mega_jump"),
      (AbstractEffect) new FunctionEffect(Category.PlayerModifications, "Infinite Oxygen", "ManFromAtlantis", "timed_cheat", "infinite_oxygen"),
      (AbstractEffect) new FunctionEffect(Category.PlayerModifications, "Mega Punch", "StingLikeABee", "timed_cheat", "mega_punch"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Fat Player", "WhoAteAllThePies", "cheat", "fat_player"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Max Muscle", "BuffMeUp", "cheat", "max_muscle"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Skinny Player", "LeanAndMean", "cheat", "skinny_player"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Max Stamina", "ICanGoAllNight", "effect", "max_stamina"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "No Stamina", "ImAllOutOfBreath", "effect", "no_stamina"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Hitman Level For All Weapons", "ProfessionalKiller", "effect", "max_weapon_skill"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Beginner Level For All Weapons", "BabysFirstGun", "effect", "no_weapon_skill"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Max Driving Skills", "NaturalTalent", "effect", "max_driving_skill"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "No Driving Skills", "BackToDrivingSchool", "effect", "no_driving_skill"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Never Get Hungry", "IAmNeverHungry", "timed_cheat", "never_hungry"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Lock Respect At Max", "WorshipMe", "timed_cheat", "lock_respect"),
      (AbstractEffect) new FunctionEffect(Category.Stats, "Lock Sex Appeal At Max", "HelloLadies", "timed_cheat", "lock_sex_appeal"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Pop Tires Of All Vehicles", "TiresBeGone", "effect", "pop_vehicle_tires"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Set Current Vehicle On Fire", "WayTooHot", "effect", "set_vehicle_on_fire"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Turn Vehicles Around", "TurnAround", "effect", "turn_vehicles_around"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Send Vehicles To Space", "StairwayToHeaven", "effect", "send_vehicles_to_space"),
      new FunctionEffect(Category.CustomEffects, "Get Busted", "GoToJail", "effect", "player_busted").DisableRapidFire(),
      new FunctionEffect(Category.CustomEffects, "Get Wasted", "Hospitality", "effect", "player_wasted").DisableRapidFire(),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "One Hit K.O.", "ILikeToLiveDangerously", "timed_effect", "one_hit_ko"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Inverted Gravity", "BeamMeUpScotty", "timed_effect", "inverted_gravity"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Zero Gravity", "ImInSpaaaaace", "timed_effect", "zero_gravity"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Quarter Gravity", "GroundControlToMajorTom", "timed_effect", "quarter_gravity"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Half Gravity", "ImFeelingLightheaded", "timed_effect", "half_gravity"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Double Gravity", "KilogramOfFeathers", "timed_effect", "double_gravity"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Quadruple Gravity", "KilogramOfSteel", "timed_effect", "quadruple_gravity"),
      new FunctionEffect(Category.CustomEffects, "Insane Gravity", "StraightToHell", "timed_effect", "insane_gravity").DisableRapidFire(),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Experience The Lag", "PacketLoss", "timed_effect", "experience_the_lag"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "To Drive Or Not To Drive", "ToDriveOrNotToDrive", "timed_effect", "to_drive_or_not_to_drive"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Timelapse Mode", "DiscoInTheSky", "timed_effect", "timelapse"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Ghost Rider", "GhostRider", "timed_effect", "ghost_rider"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "To The Left, To The Right", "ToTheLeftToTheRight", "timed_effect", "to_the_left_to_the_right"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Disable HUD", "FullyImmersed", "timed_effect", "disable_hud"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Remove All Weapons", "NoWeaponsAllowed", "effect", "clear_weapons"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Where Is Everybody?", "ImHearingVoices", "timed_effect", "where_is_everybody"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Everybody Bleed Now!", "EverybodyBleedNow", "timed_effect", "everybody_bleed_now"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Set All Peds On Fire", "HotPotato", "effect", "hot_potato"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Kick Player Out Of Vehicle", "ThisAintYourCar", "effect", "kick_out_of_car"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Lock Player Inside Vehicle", "ThereIsNoEscape", "effect", "there_is_no_escape"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Disable Radar Blips", "BlipsBeGone", "timed_effect", "disable_radar_blips"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Disable All Weapon Damage", "TruePacifist", "timed_effect", "true_pacifist"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Let's Take A Break", "LetsTakeABreak", "timed_effect", "lets_take_a_break"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Rainbow Cars", "AllColorsAreBeautiful", "timed_effect", "rainbow_cars"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "High Suspension Damping", "VeryDampNoBounce", "timed_effect", "no_bouncy_vehicles"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Little Suspension Damping", "BouncinUpAndDown", "timed_effect", "bouncy_vehicles"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Zero Suspension Damping", "LowrideAllNight", "timed_effect", "very_bouncy_vehicles"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Long Live The Rich!", "LongLiveTheRich", "timed_effect", "long_live_the_rich"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Inverted Controls", "InvertedControls", "timed_effect", "inverted_controls"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Disable One Movement Key", "DisableOneMovementKey", "timed_effect", "disable_one_movement_key"),
      new FunctionEffect(Category.CustomEffects, "Fail Current Mission", "MissionFailed", "timed_effect", "fail_mission").DisableRapidFire(),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Night Vision", "NightVision", "timed_effect", "night_vision"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Thermal Vision", "ThermalVision", "timed_effect", "thermal_vision"),
      new FunctionEffect(Category.CustomEffects, "Pass Current Mission", "IllTakeAFreePass", "timed_effect", "pass_mission").DisableRapidFire(),
      new FunctionEffect(Category.CustomEffects, "Cryptic Effects", "ZalgoRules", "timed_effect", "cryptic_effects").DisableRapidFire().DisableTwitch(),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Infinite Health (Everyone)", "NoOneCanHurtAnyone", "timed_effect", "infinite_health"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Invisible Vehicles (Only Wheels)", "WheelsOnlyPlease", "timed_effect", "wheels_only_please"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Invisible Vehicles", "InvisibleVehicles", "timed_effect", "invisible_vehicles"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Powerpoint Presentation", "PowerpointPresentation", "timed_effect", "framerate_15"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Smooth Criminal", "SmoothCriminal", "timed_effect", "framerate_60"),
      new FunctionEffect(Category.CustomEffects, "Clear Active Effects", "ClearActiveEffects", "other", "clear_active_effects").DisableTwitch(),
      new FunctionEffect(Category.CustomEffects, "Reload Autosave", "HereWeGoAgain", "timed_effect", "reload_autosave").DisableRapidFire(),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Infinite Health (Player)", "NoOneCanHurtMe", "timed_effect", "infinite_health_player"),
      new FunctionEffect(Category.CustomEffects, "Woozie Mode", "WoozieMode", "timed_effect", "woozie_mode").DisableRapidFire(),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "High Pitched Audio", "CJAndTheChipmunks", "timed_effect", "chipmunks"),
      (AbstractEffect) new FunctionEffect(Category.CustomEffects, "Pitch Shifter", "VocalRange", "timed_effect", "vocal_range"),
      (AbstractEffect) new TeleportationEffect("Teleport Home", "BringMeHome", Location.GrooveStreet),
      (AbstractEffect) new TeleportationEffect("Teleport To A Tower", "BringMeToATower", Location.LSTower),
      (AbstractEffect) new TeleportationEffect("Teleport To A Pier", "BringMeToAPier", Location.LSPier),
      (AbstractEffect) new TeleportationEffect("Teleport To The LS Airport", "BringMeToTheLSAirport", Location.LSAirport),
      (AbstractEffect) new TeleportationEffect("Teleport To The Docks", "BringMeToTheDocks", Location.LSDocks),
      (AbstractEffect) new TeleportationEffect("Teleport To A Mountain", "BringMeToAMountain", Location.MountChiliad),
      (AbstractEffect) new TeleportationEffect("Teleport To The SF Airport", "BringMeToTheSFAirport", Location.SFAirport),
      (AbstractEffect) new TeleportationEffect("Teleport To A Bridge", "BringMeToABridge", Location.SFBridge),
      (AbstractEffect) new TeleportationEffect("Teleport To A Secret Place", "BringMeToASecretPlace", Location.Area52),
      (AbstractEffect) new TeleportationEffect("Teleport To A Quarry", "BringMeToAQuarry", Location.LVQuarry),
      (AbstractEffect) new TeleportationEffect("Teleport To The LV Airport", "BringMeToTheLVAirport", Location.LVAirport),
      (AbstractEffect) new TeleportationEffect("Teleport To Big Ear", "BringMeToBigEar", Location.LVSatellite)
    };

    public static List<AbstractEffect> EnabledEffects { get; } = new List<AbstractEffect>();

    public static AbstractEffect GetByID(string id, bool onlyEnabled = false) => (onlyEnabled ? EffectDatabase.EnabledEffects : EffectDatabase.Effects).Find((Predicate<AbstractEffect>) (e => e.Id == id));

    public static AbstractEffect GetByWord(string word, bool onlyEnabled = false) => (onlyEnabled ? EffectDatabase.EnabledEffects : EffectDatabase.Effects).Find((Predicate<AbstractEffect>) (e => !string.IsNullOrEmpty(e.Word) && string.Equals(e.Word, word, StringComparison.OrdinalIgnoreCase)));

    public static AbstractEffect GetByDescription(
      string description,
      bool onlyEnabled = false)
    {
      return (onlyEnabled ? EffectDatabase.EnabledEffects : EffectDatabase.Effects).Find((Predicate<AbstractEffect>) (e => string.Equals(description, e.GetDescription(), StringComparison.OrdinalIgnoreCase)));
    }

    public static AbstractEffect GetRandomEffect(bool onlyEnabled = false)
    {
      List<AbstractEffect> abstractEffectList = onlyEnabled ? EffectDatabase.EnabledEffects : EffectDatabase.Effects;
      return abstractEffectList.Count == 0 ? (AbstractEffect) null : abstractEffectList[RandomHandler.Next(abstractEffectList.Count)];
    }

    public static AbstractEffect RunEffect(string id, bool onlyEnabled = true) => EffectDatabase.RunEffect(EffectDatabase.GetByID(id, onlyEnabled));

    public static AbstractEffect RunEffect(AbstractEffect effect)
    {
      effect?.RunEffect();
      return effect;
    }

    public static void SetEffectEnabled(AbstractEffect effect, bool enabled)
    {
      if (effect == null)
        return;
      if (!enabled && EffectDatabase.EnabledEffects.Contains(effect))
      {
        EffectDatabase.EnabledEffects.Remove(effect);
      }
      else
      {
        if (!enabled || EffectDatabase.EnabledEffects.Contains(effect))
          return;
        EffectDatabase.EnabledEffects.Add(effect);
      }
    }
  }
}
