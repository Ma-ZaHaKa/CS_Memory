// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.util.Config
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using Newtonsoft.Json;
using System;
using System.Globalization;

namespace GTA_SA_Chaos.util
{
  public class Config
  {
    public static Config Instance = new Config();
    [JsonIgnore]
    public bool Enabled;
    [JsonIgnore]
    public bool IsTwitchMode;
    [JsonIgnore]
    public int TwitchVotingMode;
    public int MainCooldown;
    public bool ContinueTimer = true;
    public string Seed;
    public bool CrypticEffects;
    public bool MainShowLastEffects;
    public bool TwitchAllowOnlyEnabledEffectsRapidFire;
    public int TwitchVotingTime;
    public int TwitchVotingCooldown;
    public bool TwitchShowLastEffects;
    public bool TwitchMajorityVoting = true;
    public bool Twitch3TimesCooldown;
    public string TwitchChannel;
    public string TwitchUsername;
    public string TwitchOAuthToken;

    public static int GetEffectDuration()
    {
      if (!Config.Instance.IsTwitchMode)
        return Config.Instance.MainCooldown * 3;
      int num = Config.Instance.TwitchVotingCooldown + Config.Instance.TwitchVotingTime;
      return !Config.Instance.Twitch3TimesCooldown ? num : num * 3;
    }

    public static string FToString(float value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }
}
