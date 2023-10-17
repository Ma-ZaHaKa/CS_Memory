// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.effects.TeleportationEffect
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using GTA_SA_Chaos.util;

namespace GTA_SA_Chaos.effects
{
  public class TeleportationEffect : AbstractEffect
  {
    private readonly Location location;

    public TeleportationEffect(string description, string word, Location _location)
      : base(Category.Teleportation, description, word)
    {
      this.location = _location;
      this.DisableRapidFire();
    }

    public override void RunEffect() => this.SendEffectToGame("teleport", this.location.ToString());
  }
}
