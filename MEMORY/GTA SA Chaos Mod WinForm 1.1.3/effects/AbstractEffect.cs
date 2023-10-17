using GTA_SA_Chaos.util;

namespace GTA_SA_Chaos.effects
{
  public abstract class AbstractEffect
  {
    public readonly string Id;
    public readonly Category Category;
    private readonly string Description;
    public readonly string Word;
    private string Voter = "N/A";
    private int rapidFire = 1;
    private bool twitchEnabled = true;

    public AbstractEffect(Category category, string description, string word)
    {
      this.Id = category.AddEffectToCategory(this);
      this.Category = category;
      this.Description = description;
      this.Word = word;
    }

    public virtual string GetDescription() => this.Description;

    public AbstractEffect SetVoter(string voter)
    {
      this.Voter = voter;
      return this;
    }

    public AbstractEffect ResetVoter()
    {
      this.Voter = "N/A";
      return this;
    }

    public AbstractEffect DisableRapidFire()
    {
      this.rapidFire = 0;
      return this;
    }

    public AbstractEffect DisableTwitch()
    {
      this.twitchEnabled = false;
      return this;
    }

    public bool IsRapidFire() => this.rapidFire == 1;

    public bool IsTwitchEnabled() => this.twitchEnabled;

    public abstract void RunEffect();

    public void SendEffectToGame(
      string type,
      string function,
      int duration = -1,
      string description = "",
      int multiplier = 1)
    {
      if (duration == -1)
        duration = Config.GetEffectDuration();
      duration *= multiplier;
      if (string.IsNullOrEmpty(description))
        description = this.GetDescription();
            System.Windows.Forms.MessageBox.Show(description);
            ProcessHooker.SendEffectToGame(type, function, duration, description, this.Voter, Config.Instance.TwitchVotingMode == 2 ? this.rapidFire : 0);
    }
  }
}
