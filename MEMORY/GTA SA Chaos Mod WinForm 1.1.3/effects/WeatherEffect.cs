// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.effects.WeatherEffect
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using GTA_SA_Chaos.util;

namespace GTA_SA_Chaos.effects
{
  public class WeatherEffect : AbstractEffect
  {
    private readonly int weatherID;

    public WeatherEffect(string description, string word, int _weatherID)
      : base(Category.Weather, description, word)
    {
      this.weatherID = _weatherID;
    }

    public override void RunEffect() => this.SendEffectToGame("weather", this.weatherID.ToString());
  }
}
