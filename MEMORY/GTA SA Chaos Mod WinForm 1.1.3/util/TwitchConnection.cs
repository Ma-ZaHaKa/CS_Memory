// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.util.TwitchConnection
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using GTA_SA_Chaos.effects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace GTA_SA_Chaos.util
{
  internal class TwitchConnection
  {
    public TwitchClient Client;
    private readonly string Channel;
    private readonly string Username;
    private readonly string Oauth;
    private readonly TwitchConnection.EffectVoting effectVoting = new TwitchConnection.EffectVoting();
    private readonly HashSet<string> rapidFireVoters = new HashSet<string>();
    private int VotingMode;
    private int overrideEffectChoice = -1;
    private int lastChoice = -1;

   public TwitchConnection()
    {
      /*this.Channel = Config.Instance.TwitchChannel;
      this.Username = Config.Instance.TwitchUsername;
      this.Oauth = Config.Instance.TwitchOAuthToken;
      if (this.Channel == null || this.Username == null || this.Oauth == null || this.Channel == "" || this.Username == "" || this.Oauth == "")
        return;
      ConnectionCredentials credentials = new ConnectionCredentials(this.Username, this.Oauth, "wss://irc-ws.chat.twitch.tv:443", false);
      ClientProtocol protocol = (ClientProtocol) 1;
      if (Environment.OSVersion.Version.Major < 10)
        protocol = (ClientProtocol) 0;
      this.TryConnect(credentials, protocol);*/
    }

    /*private void TryConnect(ConnectionCredentials credentials, ClientProtocol protocol = 1)
    {
      if (this.Client != null)
        this.Kill();
      this.Client = new TwitchClient((IClient) null, protocol, (ILogger<TwitchClient>) null);
      this.Client.Initialize(credentials, this.Channel, '!', '!', true);
      this.Client.OnMessageReceived += new EventHandler<OnMessageReceivedArgs>(this.Client_OnMessageReceived);
      this.Client.OnConnected += new EventHandler<OnConnectedArgs>(this.Client_OnConnected);
      this.Client.OnConnectionError += new EventHandler<OnConnectionErrorArgs>(this.Client_OnConnectionError);
      this.Client.Connect();
    }*/

    /*private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
    {
      this.Kill();
      this.Client.Initialize(new ConnectionCredentials(this.Username, this.Oauth, "wss://irc-ws.chat.twitch.tv:443", false), this.Channel, '!', '!', true);
      this.Client.Connect();
    }*/

    private void Client_OnConnected(object sender, OnConnectedArgs e) => this.SendMessage("Connected!");

    public void Kill() => this.Client.Disconnect();

    public void SetVoting(
      int votingMode,
      int untilRapidFire = -1,
      TwitchConnection.VotingElement votingElement = null,
      string username = null)
    {
      this.VotingMode = votingMode;
      if (this.VotingMode == 1)
      {
        this.effectVoting.Clear();
        this.effectVoting.GenerateRandomEffects();
        this.overrideEffectChoice = -1;
        this.lastChoice = -1;
        this.SendMessage("Voting has started! Type 1, 2 or 3 (or #1, #2, #3) to vote for one of the effects!");
        foreach (TwitchConnection.VotingElement votingElement1 in this.effectVoting.VotingElements)
          this.SendMessage(string.Format("#{0}: {1}", (object) (votingElement1.Id + 1), (object) votingElement1.Effect.GetDescription()));
      }
      else if (this.VotingMode == 2)
      {
        this.rapidFireVoters.Clear();
        this.SendMessage("ATTENTION, ALL GAMERS! RAPID-FIRE HAS BEGUN! VALID EFFECTS WILL BE ENABLED FOR 15 SECONDS!");
      }
      else if (votingElement != null)
      {
        this.SendEffectVotingToGame(false);
        string description = votingElement.Effect.GetDescription();
        this.SendMessage(string.Format("Cooldown has started! ({0} until Rapid-Fire) - Enabled effect: {1} voted by {2}", (object) untilRapidFire, (object) description, (object) (username ?? "GTA:SA Chaos")));
        if (untilRapidFire != 1)
          return;
        this.SendMessage("Rapid-Fire is coming up! Get your cheats ready!");
        this.SendMessage("!rapidfire", false);
      }
      else
        this.SendMessage(string.Format("Cooldown has started! ({0} until Rapid-Fire)", (object) untilRapidFire));
    }

    public TwitchConnection.VotingElement GetRandomVotedEffect(out string username)
    {
      if (Config.Instance.TwitchMajorityVoting)
      {
        username = "The Majority";
        TwitchConnection.VotingElement majorityVote = this.effectVoting.GetMajorityVote();
        majorityVote.Effect.ResetVoter();
        this.lastChoice = majorityVote.Id;
        return majorityVote;
      }
      TwitchConnection.VotingElement votingElement = this.effectVoting.GetRandomEffect(out username, out this.lastChoice);
      if (this.overrideEffectChoice >= 0 && this.overrideEffectChoice <= 2)
      {
        username = "lordmau5";
        this.lastChoice = this.overrideEffectChoice;
        votingElement = this.effectVoting.VotingElements[this.overrideEffectChoice];
        votingElement.Effect.SetVoter(username);
      }
      return votingElement;
    }

    private void SendMessage(string message, bool prefix = true)
    {
      if (this.Channel == null || message == null)
        return;
      if (!this.Client.IsConnected)
        this.Client.Connect();
      else if (((IReadOnlyCollection<JoinedChannel>) this.Client.JoinedChannels).Count == 0)
        this.Client.JoinChannel(this.Channel, false);
      else
        this.Client.SendMessage(this.Channel, (prefix ? "[GTA Chaos] " : "") + message, false);
    }

    private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
      string username = ((TwitchLibMessage) e.ChatMessage).Username;
      string str = this.RemoveSpecialCharacters(((ChatMessage) e.ChatMessage).Message);
      if (this.VotingMode == 2)
      {
        if (this.rapidFireVoters.Contains(username))
          return;
        AbstractEffect byWord = EffectDatabase.GetByWord(str);
        if (byWord == null || !byWord.IsRapidFire())
          return;
        this.RapidFireEffect(new TwitchConnection.RapidFireEventArgs()
        {
          Effect = byWord.SetVoter(username)
        });
        this.rapidFireVoters.Add(username);
      }
      else
      {
        if (this.VotingMode != 1)
          return;
        int userChoice = this.TryParseUserChoice(str);
        switch (userChoice)
        {
          case 0:
          case 1:
          case 2:
            TwitchConnection.EffectVoting effectVoting = this.effectVoting;
            if (effectVoting != null)
            {
              effectVoting.TryAddVote(username, userChoice);
              break;
            }
            break;
        }
        if (!username.Equals("lordmau5") || !str.EndsWith("."))
          return;
        this.overrideEffectChoice = userChoice;
      }
    }

    private string RemoveSpecialCharacters(string text) => Regex.Replace(text, "[^A-Za-z0-9]", "");

    private int TryParseUserChoice(string text)
    {
      try
      {
        return int.Parse(text) - 1;
      }
      catch
      {
        return -1;
      }
    }

    public void SendEffectVotingToGame(bool undetermined = true)
    {
      if (this.effectVoting.IsEmpty())
        return;
      string[] effects;
      int[] votes;
      this.effectVoting.GetVotes(out effects, out votes, undetermined);
      ProcessHooker.SendPipeMessage(string.Format("votes:{0};{1};;{2};{3};;{4};{5};;{6}", (object) effects[0], (object) votes[0], (object) effects[1], (object) votes[1], (object) effects[2], (object) votes[2], (object) this.lastChoice));
    }

    public event EventHandler<TwitchConnection.RapidFireEventArgs> OnRapidFireEffect;

    public virtual void RapidFireEffect(TwitchConnection.RapidFireEventArgs e)
    {
      EventHandler<TwitchConnection.RapidFireEventArgs> onRapidFireEffect = this.OnRapidFireEffect;
      if (onRapidFireEffect == null)
        return;
      onRapidFireEffect((object) this, e);
    }

    private class EffectVoting
    {
      public readonly List<TwitchConnection.VotingElement> VotingElements;
      public readonly Dictionary<string, TwitchConnection.VotingElement> Voters;

      public EffectVoting()
      {
        this.VotingElements = new List<TwitchConnection.VotingElement>();
        this.Voters = new Dictionary<string, TwitchConnection.VotingElement>();
      }

      public bool IsEmpty() => this.VotingElements.Count == 0;

      public void Clear()
      {
        this.VotingElements.Clear();
        this.Voters.Clear();
      }

      public bool ContainsEffect(AbstractEffect effect) => this.VotingElements.Any<TwitchConnection.VotingElement>((Func<TwitchConnection.VotingElement, bool>) (e => e.Effect.GetDescription().Equals(effect.GetDescription())));

      public void AddEffect(AbstractEffect effect) => this.VotingElements.Add(new TwitchConnection.VotingElement(this.VotingElements.Count, effect));

      public void GetVotes(out string[] effects, out int[] votes, bool undetermined = false)
      {
        TwitchConnection.VotingElement[] array = this.VotingElements.ToArray();
        effects = new string[3]
        {
          undetermined ? "???" : array[0].Effect.GetDescription(),
          undetermined ? "???" : array[1].Effect.GetDescription(),
          undetermined ? "???" : array[2].Effect.GetDescription()
        };
        votes = new int[3]
        {
          array[0].Voters.Count,
          array[1].Voters.Count,
          array[2].Voters.Count
        };
      }

      public void GenerateRandomEffects()
      {
        int num = Math.Min(3, EffectDatabase.EnabledEffects.Count);
        while (this.VotingElements.Count != num)
        {
          AbstractEffect randomEffect = EffectDatabase.GetRandomEffect(true);
          if (randomEffect.IsTwitchEnabled() && !this.ContainsEffect(randomEffect))
            this.AddEffect(randomEffect);
        }
        while (this.VotingElements.Count < 3)
        {
          AbstractEffect randomEffect = EffectDatabase.GetRandomEffect();
          if (randomEffect.IsTwitchEnabled() && !this.ContainsEffect(randomEffect))
            this.AddEffect(randomEffect);
        }
      }

      public TwitchConnection.VotingElement GetMajorityVote()
      {
        int maxVotes = 0;
        TwitchConnection.VotingElement[] array = this.VotingElements.OrderByDescending<TwitchConnection.VotingElement, int>((Func<TwitchConnection.VotingElement, int>) (e =>
        {
          if (e.Voters.Count > maxVotes)
            maxVotes = e.Voters.Count;
          return e.Voters.Count;
        })).Where<TwitchConnection.VotingElement>((Func<TwitchConnection.VotingElement, bool>) (e => e.Voters.Count == maxVotes)).ToArray<TwitchConnection.VotingElement>();
        return array[new Random().Next(((IEnumerable<TwitchConnection.VotingElement>) array).Count<TwitchConnection.VotingElement>())];
      }

      public void TryAddVote(string username, int effectChoice)
      {
        this.VotingElements.ForEach((Action<TwitchConnection.VotingElement>) (e => e.RemoveVoter(username)));
        this.VotingElements[effectChoice].AddVoter(username);
        this.Voters[username] = this.VotingElements[effectChoice];
      }

      public TwitchConnection.VotingElement GetRandomEffect(
        out string username,
        out int choice)
      {
        username = "N/A";
        Random random = new Random();
        TwitchConnection.VotingElement votingElement = (TwitchConnection.VotingElement) null;
        if (this.Voters.Count > 0)
        {
          username = this.Voters.Keys.ToArray<string>()[random.Next(this.Voters.Count)];
          this.Voters.TryGetValue(username, out votingElement);
        }
        if (votingElement == null)
          votingElement = this.VotingElements.ToArray()[random.Next(this.VotingElements.Count)];
        choice = votingElement.Id;
        votingElement.Effect.SetVoter(username);
        return votingElement;
      }
    }

    public class RapidFireEventArgs : EventArgs
    {
      public AbstractEffect Effect { get; set; }
    }

    public class VotingElement
    {
      public int Id { get; set; }

      public AbstractEffect Effect { get; set; }

      public HashSet<string> Voters { get; set; }

      public VotingElement(int id, AbstractEffect effect)
      {
        this.Id = id;
        this.Effect = effect;
        this.Voters = new HashSet<string>();
      }

      public bool ContainsVoter(string username) => this.Voters.Contains(username);

      public void AddVoter(string username) => this.Voters.Add(username);

      public void RemoveVoter(string username) => this.Voters.Remove(username);
    }
  }
}
