// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.effects.SpawnVehicleEffect
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using GTA_SA_Chaos.util;

namespace GTA_SA_Chaos.effects
{
  internal class SpawnVehicleEffect : AbstractEffect
  {
    private readonly int vehicleID;

    public SpawnVehicleEffect(string word, int _vehicleID)
      : base(Category.Spawning, "Spawn Vehicle", word)
    {
      this.vehicleID = _vehicleID;
      if (this.vehicleID != 569)
        return;
      this.vehicleID = 537;
    }

    public override string GetDescription()
    {
      string str = "Random Vehicle";
      return "Spawn " + (this.vehicleID == -1 ? str : VehicleNames.GetVehicleName(this.vehicleID));
    }

    public override void RunEffect()
    {
      int modelID = this.vehicleID;
      if (modelID == -1)
        modelID = RandomHandler.Next(400, 611);
      string description = "Spawn " + VehicleNames.GetVehicleName(modelID);
      this.SendEffectToGame("spawn_vehicle", modelID.ToString(), description: description);
    }
  }
}
