using GTA_SA_Chaos.util;

namespace GTA_SA_Chaos.effects
{
  public class FunctionEffect : AbstractEffect
  {
    private readonly string type;
    private readonly string function;
    private readonly int duration;
    private readonly int multiplier;

    public FunctionEffect(
      Category category,
      string description,
      string word,
      string _type,
      string _function,
      int _duration = -1,
      int _multiplier = 1)
      : base(category, description, word)
    {
      this.type = _type;
      this.function = _function;
      this.duration = _duration;
      this.multiplier = _multiplier;
    }

    public override void RunEffect()
    {
      this.SendEffectToGame("set_seed", RandomHandler.Next(9999999).ToString());
      this.SendEffectToGame("cryptic_effects", (Config.Instance.CrypticEffects ? 1 : 0).ToString());
      this.SendEffectToGame(this.type, this.function, this.duration, multiplier: this.multiplier);
    }
  }
}
