// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.util.Category
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using GTA_SA_Chaos.effects;
using System.Collections.Generic;

namespace GTA_SA_Chaos.util
{
  public sealed class Category
  {
    public readonly string Name;
    public readonly string Prefix;
    public readonly List<AbstractEffect> Effects;
    public static readonly Category WeaponsAndHealth = new Category("Weapons & Health", "HE");
    public static readonly Category WantedLevel = new Category("Wanted Level", "WA");
    public static readonly Category Weather = new Category(nameof (Weather), "WE");
    public static readonly Category Spawning = new Category(nameof (Spawning), "SP");
    public static readonly Category Time = new Category(nameof (Time), "TI");
    public static readonly Category VehiclesTraffic = new Category("Vehicles & Traffic", "VE");
    public static readonly Category PedsAndCo = new Category("Peds & Co.", "PE");
    public static readonly Category PlayerModifications = new Category("Player Modifications", "MO");
    public static readonly Category Stats = new Category(nameof (Stats), "ST");
    public static readonly Category CustomEffects = new Category("Custom Effects", "CE");
    public static readonly Category Teleportation = new Category(nameof (Teleportation), "TP");

    private Category(string name, string prefix)
    {
      this.Name = name;
      this.Prefix = prefix;
      this.Effects = new List<AbstractEffect>();
    }

    public string AddEffectToCategory(AbstractEffect effect)
    {
      this.Effects.Add(effect);
      return this.Prefix + (object) this.Effects.Count;
    }
  }
}
