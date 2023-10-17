// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.util.RandomHandler
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using System;

namespace GTA_SA_Chaos.util
{
  public class RandomHandler
  {
    private static Random random = new Random();

    public static void SetSeed(string seed)
    {
      if (string.IsNullOrEmpty(seed))
        RandomHandler.random = new Random();
      else
        RandomHandler.random = new Random(seed.GetHashCode());
    }

    public static int Next() => RandomHandler.random.Next();

    public static int Next(int maxValue) => RandomHandler.random.Next(maxValue);

    public static int Next(int minValue, int maxValue) => RandomHandler.random.Next(minValue, maxValue + 1);

    public static double NextDouble() => RandomHandler.random.NextDouble();

    public static void NextBytes(byte[] buffer) => RandomHandler.random.NextBytes(buffer);
  }
}
