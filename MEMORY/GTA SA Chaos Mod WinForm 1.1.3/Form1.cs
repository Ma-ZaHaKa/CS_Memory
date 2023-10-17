// Decompiled with JetBrains decompiler
// Type: GTA_SA_Chaos.Form1
// Assembly: GTA SA Chaos Mod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A549775-59B7-46E5-8017-A55669158853
// Assembly location: C:\Users\MaZaHaKa\Desktop\111111111111111111111111\chaosmod113\GTA SA Chaos Mod.exe

using GTA_SA_Chaos.effects;
using GTA_SA_Chaos.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using TwitchLib.Client.Events;

namespace GTA_SA_Chaos
{
  public class Form1 : Form
  {
    private readonly string ConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "config.cfg");
    private readonly Stopwatch Stopwatch;
    private readonly Dictionary<string, Form1.EffectTreeNode> IdToEffectNodeMap = new Dictionary<string, Form1.EffectTreeNode>();
    private TwitchConnection Twitch;
    private int elapsedCount;
    private readonly System.Timers.Timer AutoStartTimer;
    private int introState = 1;
    private int timesUntilRapidFire;
    private IContainer components;
    private Button buttonMainToggle;
    private ProgressBar progressBarMain;
    private TabControl tabSettings;
    private TabPage tabMain;
    private TabPage tabEffects;
    private TreeView enabledEffectsView;
    private Label label1;
    private ComboBox presetComboBox;
    private ListBox listLastEffectsMain;
    private ComboBox comboBoxMainCooldown;
    private Label label2;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem loadPresetToolStripMenuItem;
    private ToolStripMenuItem savePresetToolStripMenuItem;
    private ToolStripMenuItem exitToolStripMenuItem;
    private TabPage tabTwitch;
    private Label label5;
    private Label label4;
    private TextBox textBoxTwitchOAuth;
    private TextBox textBoxTwitchUsername;
    private Button buttonConnectTwitch;
    private ListBox listLastEffectsTwitch;
    private ToolTip toolTipHandler;
    private ComboBox comboBoxVotingCooldown;
    private Label label7;
    private Label label6;
    private ComboBox comboBoxVotingTime;
    private ProgressBar progressBarTwitch;
    private Button buttonTwitchToggle;
    private Button buttonAutoStart;
    private System.Windows.Forms.Timer timerMain;
    private Button buttonSwitchMode;
    private Label labelTwitchCurrentMode;
    private TabPage tabPage1;
    private Label label8;
    private TextBox textBoxSeed;
    private TabPage tabDebug;
    private Button buttonTestSeed;
    private Label labelTestSeed;
    private Button buttonGenericTest;
    private Button buttonResetMain;
    private CheckBox checkBoxContinueTimer;
    private CheckBox checkBoxCrypticEffects;
    private CheckBox checkBoxShowLastEffectsMain;
    private CheckBox checkBoxShowLastEffectsTwitch;
    private CheckBox checkBoxTwitchAllowOnlyEnabledEffects;
    private CheckBox checkBoxTwitchMajorityVoting;
    private Button buttonResetTwitch;
    private Label label3;
    private TextBox textBoxTwitchChannel;
    private CheckBox checkBoxTwitch3TimesCooldown;

    public Form1()
    {
      this.InitializeComponent();
      this.Text = "GTA:SA Chaos v1.1.3";
      this.tabSettings.TabPages.Remove(this.tabDebug);
      this.Stopwatch = new Stopwatch();
      this.AutoStartTimer = new System.Timers.Timer()
      {
        Interval = 50.0,
        AutoReset = true
      };
      this.AutoStartTimer.Elapsed += new ElapsedEventHandler(this.AutoStartTimer_Elapsed);
      this.PopulateEffectTreeList();
      this.PopulateMainCooldowns();
      this.PopulatePresets();
      this.tabSettings.TabPages.Remove(this.tabTwitch);
      this.PopulateVotingTimes();
      this.PopulateVotingCooldowns();
      this.TryLoadConfig();
      this.timesUntilRapidFire = new Random().Next(10, 15);
    }

    private void AutoStartTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      if (ProcessHooker.HasExited() || Config.Instance.Enabled)
        return;
      int num1;
      MemoryHelper.Read<int>((IntPtr) 10808580, out num1);
      int num2;
      MemoryHelper.Read<int>((IntPtr) 12045188, out num2);
      if (this.introState == 0 && num1 == 1 && num2 < 60000)
        this.buttonAutoStart.Invoke((Action)(() => this.SetAutostart()));
      this.introState = num1;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.SaveConfig();
      ProcessHooker.CloseProcess();
    }

    private void TryLoadConfig()
    {
      try
      {
        using (StreamReader streamReader = new StreamReader(this.ConfigPath))
        {
          using (JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) streamReader))
          {
            Config.Instance = new JsonSerializer().Deserialize<Config>(jsonReader);
            RandomHandler.SetSeed(Config.Instance.Seed);
            this.UpdateInterface();
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void SaveConfig()
    {
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(this.ConfigPath))
        {
          using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) streamWriter))
            new JsonSerializer().Serialize((JsonWriter) jsonTextWriter, (object) Config.Instance);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void UpdateInterface()
    {
      foreach (Form1.MainCooldownComboBoxItem cooldownComboBoxItem in this.comboBoxMainCooldown.Items)
      {
        if (cooldownComboBoxItem.Time == Config.Instance.MainCooldown)
        {
          this.comboBoxMainCooldown.SelectedItem = (object) cooldownComboBoxItem;
          break;
        }
      }
      this.checkBoxTwitchAllowOnlyEnabledEffects.Checked = Config.Instance.TwitchAllowOnlyEnabledEffectsRapidFire;
      foreach (Form1.VotingTimeComboBoxItem timeComboBoxItem in this.comboBoxVotingTime.Items)
      {
        if (timeComboBoxItem.VotingTime == Config.Instance.TwitchVotingTime)
        {
          this.comboBoxVotingTime.SelectedItem = (object) timeComboBoxItem;
          break;
        }
      }
      foreach (Form1.VotingCooldownComboBoxItem cooldownComboBoxItem in this.comboBoxVotingCooldown.Items)
      {
        if (cooldownComboBoxItem.VotingCooldown == Config.Instance.TwitchVotingCooldown)
        {
          this.comboBoxVotingCooldown.SelectedItem = (object) cooldownComboBoxItem;
          break;
        }
      }
      this.textBoxTwitchChannel.Text = Config.Instance.TwitchChannel;
      this.textBoxTwitchUsername.Text = Config.Instance.TwitchUsername;
      this.textBoxTwitchOAuth.Text = Config.Instance.TwitchOAuthToken;
      this.checkBoxContinueTimer.Checked = Config.Instance.ContinueTimer;
      this.checkBoxCrypticEffects.Checked = Config.Instance.CrypticEffects;
      this.checkBoxShowLastEffectsMain.Checked = Config.Instance.MainShowLastEffects;
      this.checkBoxShowLastEffectsTwitch.Checked = Config.Instance.TwitchShowLastEffects;
      this.checkBoxTwitchMajorityVoting.Checked = Config.Instance.TwitchMajorityVoting;
      this.checkBoxTwitch3TimesCooldown.Checked = Config.Instance.Twitch3TimesCooldown;
      this.textBoxSeed.Text = Config.Instance.Seed;
    }

    public void AddEffectToListBox(AbstractEffect effect)
    {
      string str = "Invalid";
      if (effect != null)
      {
        str = effect.GetDescription();
        if (!string.IsNullOrEmpty(effect.Word))
          str = str + " (" + effect.Word + ")";
      }
      ListBox listBox = Config.Instance.IsTwitchMode ? this.listLastEffectsTwitch : this.listLastEffectsMain;
      listBox.Items.Insert(0, (object) str);
      if (listBox.Items.Count <= 7)
        return;
      listBox.Items.RemoveAt(7);
    }

    private void ButtonAutoStart_Click(object sender, EventArgs e) => this.TrySetupAutostart();

    private void CallEffect(AbstractEffect effect = null)
    {
      if (effect == null)
      {
        effect = EffectDatabase.RunEffect(EffectDatabase.GetRandomEffect(true));
        effect.ResetVoter();
      }
      else
        EffectDatabase.RunEffect(effect);
      this.AddEffectToListBox(effect);
    }

    private void TrySetupAutostart()
    {
      if (ProcessHooker.HasExited())
        ProcessHooker.HookProcess();
      if (ProcessHooker.HasExited())
      {
        int num = (int) MessageBox.Show("The game needs to be running!", "Error");
        this.buttonAutoStart.Enabled = Config.Instance.IsTwitchMode && this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
        this.buttonAutoStart.Text = "Auto-Start";
        if (Config.Instance.ContinueTimer)
          return;
        this.SetEnabled(false);
        this.elapsedCount = 0;
        this.Stopwatch.Reset();
        this.buttonMainToggle.Enabled = true;
        this.buttonTwitchToggle.Enabled = this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
      }
      else
      {
        ProcessHooker.AttachExitedMethod((EventHandler) ((sender, e) => this.buttonAutoStart.Invoke((Action)(() =>
        {
          this.buttonAutoStart.Enabled = Config.Instance.IsTwitchMode && this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
          this.buttonAutoStart.Text = "Auto-Start";
          if (!Config.Instance.ContinueTimer)
          {
            this.SetEnabled(false);
            this.elapsedCount = 0;
            this.Stopwatch.Reset();
            this.buttonMainToggle.Enabled = true;
            this.buttonTwitchToggle.Enabled = this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
          }
          ProcessHooker.CloseProcess();
        }))));
        this.buttonAutoStart.Enabled = false;
        this.buttonAutoStart.Text = "Waiting...";
        Config.Instance.Enabled = false;
        this.AutoStartTimer.Start();
        this.buttonMainToggle.Enabled = false;
        this.buttonTwitchToggle.Enabled = this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
      }
    }

    private void OnTimerTick(object sender, EventArgs e)
    {
      if (Config.Instance.IsTwitchMode)
        this.TickTwitch();
      else
        this.TickMain();
    }

    private void TickMain()
    {
      if (!Config.Instance.Enabled)
        return;
      int val1 = Math.Max(1, (int) this.Stopwatch.ElapsedMilliseconds);
      this.progressBarMain.Value = Math.Min(val1, this.progressBarMain.Maximum);
      this.progressBarMain.Value = Math.Min(val1 - 1, this.progressBarMain.Maximum);
      if (this.Stopwatch.ElapsedMilliseconds - (long) this.elapsedCount > 100L)
      {
        ProcessHooker.SendEffectToGame("time", ((int) ((double) Math.Max(0L, (long) Config.Instance.MainCooldown - this.Stopwatch.ElapsedMilliseconds) / (double) Config.Instance.MainCooldown * 1000.0)).ToString());
        this.elapsedCount = (int) this.Stopwatch.ElapsedMilliseconds;
      }
      if (this.Stopwatch.ElapsedMilliseconds < (long) Config.Instance.MainCooldown)
        return;
      this.progressBarMain.Value = 0;
      this.CallEffect();
      this.elapsedCount = 0;
      this.Stopwatch.Restart();
    }

    private void TickTwitch()
    {
      if (!Config.Instance.Enabled)
        return;
      if (Config.Instance.TwitchVotingMode == 1)
      {
        if (this.progressBarTwitch.Maximum != Config.Instance.TwitchVotingTime)
          this.progressBarTwitch.Maximum = Config.Instance.TwitchVotingTime;
        int num = Math.Max(1, (int) this.Stopwatch.ElapsedMilliseconds);
        this.progressBarTwitch.Value = Math.Max(this.progressBarTwitch.Maximum - num, 0);
        this.progressBarTwitch.Value = Math.Max(this.progressBarTwitch.Maximum - num - 1, 0);
        if (this.Stopwatch.ElapsedMilliseconds - (long) this.elapsedCount > 100L)
        {
          ProcessHooker.SendEffectToGame("time", ((int) ((double) Math.Max(0L, (long) Config.Instance.TwitchVotingTime - this.Stopwatch.ElapsedMilliseconds) / (double) Config.Instance.TwitchVotingTime * 1000.0)).ToString());
          this.Twitch?.SendEffectVotingToGame();
          this.elapsedCount = (int) this.Stopwatch.ElapsedMilliseconds;
        }
        if (this.Stopwatch.ElapsedMilliseconds < (long) Config.Instance.TwitchVotingTime)
          return;
        ProcessHooker.SendEffectToGame("time", "0");
        this.elapsedCount = 0;
        this.progressBarTwitch.Value = 0;
        this.progressBarTwitch.Maximum = Config.Instance.TwitchVotingCooldown;
        this.Stopwatch.Restart();
        Config.Instance.TwitchVotingMode = 0;
        this.labelTwitchCurrentMode.Text = "Current Mode: Cooldown";
        if (this.Twitch == null)
          return;
        string username;
        TwitchConnection.VotingElement randomVotedEffect = this.Twitch.GetRandomVotedEffect(out username);
        this.Twitch.SetVoting(0, this.timesUntilRapidFire, randomVotedEffect, username);
        this.CallEffect(randomVotedEffect.Effect);
      }
      else if (Config.Instance.TwitchVotingMode == 2)
      {
        if (this.progressBarTwitch.Maximum != 10000)
          this.progressBarTwitch.Maximum = 10000;
        int num = Math.Max(1, (int) this.Stopwatch.ElapsedMilliseconds);
        this.progressBarTwitch.Value = Math.Max(this.progressBarTwitch.Maximum - num, 0);
        this.progressBarTwitch.Value = Math.Max(this.progressBarTwitch.Maximum - num - 1, 0);
        if (this.Stopwatch.ElapsedMilliseconds - (long) this.elapsedCount > 100L)
        {
          ProcessHooker.SendEffectToGame("time", ((int) ((double) Math.Max(0L, 10000L - this.Stopwatch.ElapsedMilliseconds) / 10000.0 * 1000.0)).ToString());
          this.elapsedCount = (int) this.Stopwatch.ElapsedMilliseconds;
        }
        if (this.Stopwatch.ElapsedMilliseconds < 10000L)
          return;
        ProcessHooker.SendEffectToGame("time", "0");
        this.elapsedCount = 0;
        this.progressBarTwitch.Value = 0;
        this.progressBarTwitch.Maximum = Config.Instance.TwitchVotingCooldown;
        this.Stopwatch.Restart();
        Config.Instance.TwitchVotingMode = 0;
        this.labelTwitchCurrentMode.Text = "Current Mode: Cooldown";
        this.Twitch?.SetVoting(0, this.timesUntilRapidFire);
      }
      else
      {
        if (Config.Instance.TwitchVotingMode != 0)
          return;
        if (this.progressBarTwitch.Maximum != Config.Instance.TwitchVotingCooldown)
          this.progressBarTwitch.Maximum = Config.Instance.TwitchVotingCooldown;
        int val1 = Math.Max(1, (int) this.Stopwatch.ElapsedMilliseconds);
        this.progressBarTwitch.Value = Math.Min(val1 + 1, this.progressBarTwitch.Maximum);
        this.progressBarTwitch.Value = Math.Min(val1, this.progressBarTwitch.Maximum);
        if (this.Stopwatch.ElapsedMilliseconds - (long) this.elapsedCount > 100L)
        {
          ProcessHooker.SendEffectToGame("time", Math.Min(1000, 1000 - (int) ((double) Math.Max(0L, (long) Config.Instance.TwitchVotingCooldown - this.Stopwatch.ElapsedMilliseconds) / (double) Config.Instance.TwitchVotingCooldown * 1000.0)).ToString());
          this.elapsedCount = (int) this.Stopwatch.ElapsedMilliseconds;
        }
        if (this.Stopwatch.ElapsedMilliseconds < (long) Config.Instance.TwitchVotingCooldown)
          return;
        this.elapsedCount = 0;
        if (--this.timesUntilRapidFire == 0)
        {
          this.progressBarTwitch.Value = this.progressBarTwitch.Maximum = 10000;
          this.timesUntilRapidFire = new Random().Next(10, 15);
          Config.Instance.TwitchVotingMode = 2;
          this.labelTwitchCurrentMode.Text = "Current Mode: Rapid-Fire";
          this.Twitch?.SetVoting(2, this.timesUntilRapidFire);
        }
        else
        {
          this.progressBarTwitch.Value = this.progressBarTwitch.Maximum = Config.Instance.TwitchVotingTime;
          Config.Instance.TwitchVotingMode = 1;
          this.labelTwitchCurrentMode.Text = "Current Mode: Voting";
          this.Twitch?.SetVoting(1, this.timesUntilRapidFire);
        }
        this.Stopwatch.Restart();
      }
    }

    private void PopulateEffectTreeList()
    {
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.WeaponsAndHealth));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.WantedLevel));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.Weather));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.Spawning));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.Time));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.VehiclesTraffic));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.PedsAndCo));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.PlayerModifications));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.Stats));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.CustomEffects));
      this.enabledEffectsView.Nodes.Add((TreeNode) new Form1.CategoryTreeNode(Category.Teleportation));
      foreach (AbstractEffect effect in EffectDatabase.Effects)
      {
        TreeNode treeNode = ((IEnumerable<TreeNode>) this.enabledEffectsView.Nodes.Find(effect.Category.Name, false)).FirstOrDefault<TreeNode>();
        Form1.EffectTreeNode effectTreeNode1 = new Form1.EffectTreeNode(effect);
        effectTreeNode1.Checked = true;
        Form1.EffectTreeNode effectTreeNode2 = effectTreeNode1;
        treeNode.Nodes.Add((TreeNode) effectTreeNode2);
        this.IdToEffectNodeMap.Add(effect.Id, effectTreeNode2);
      }
    }

    private void PopulatePresets()
    {
      this.presetComboBox.Items.Add((object) new Form1.PresetComboBoxItem("Speedrun", false, new string[125]
      {
        "HE1",
        "HE2",
        "HE3",
        "HE4",
        "HE5",
        "HE7",
        "WA1",
        "WA2",
        "WA3",
        "WA4",
        "WE1",
        "WE2",
        "WE3",
        "WE4",
        "WE5",
        "WE6",
        "WE7",
        "SP1",
        "SP2",
        "SP19",
        "TI1",
        "TI2",
        "TI3",
        "TI4",
        "TI5",
        "TI6",
        "TI7",
        "VE1",
        "VE2",
        "VE3",
        "VE4",
        "VE5",
        "VE6",
        "VE7",
        "VE8",
        "VE9",
        "VE10",
        "VE11",
        "VE12",
        "VE13",
        "VE14",
        "PE1",
        "PE2",
        "PE3",
        "PE4",
        "PE5",
        "PE6",
        "PE7",
        "PE8",
        "PE9",
        "PE10",
        "PE11",
        "PE12",
        "PE14",
        "PE15",
        "PE16",
        "PE17",
        "PE18",
        "MO1",
        "MO2",
        "MO3",
        "MO4",
        "MO5",
        "ST1",
        "ST2",
        "ST3",
        "ST4",
        "ST5",
        "ST6",
        "ST7",
        "ST8",
        "ST9",
        "ST10",
        "ST11",
        "ST12",
        "CE1",
        "CE2",
        "CE3",
        "CE4",
        "CE5",
        "CE6",
        "CE7",
        "CE8",
        "CE9",
        "CE10",
        "CE11",
        "CE12",
        "CE13",
        "CE14",
        "CE16",
        "CE17",
        "CE18",
        "CE19",
        "CE21",
        "CE22",
        "CE23",
        "CE24",
        "CE25",
        "CE26",
        "CE27",
        "CE28",
        "CE29",
        "CE30",
        "CE31",
        "CE32",
        "CE33",
        "CE34",
        "CE35",
        "CE36",
        "CE37",
        "CE38",
        "CE39",
        "CE40",
        "CE41",
        "CE43",
        "CE44",
        "CE45",
        "CE46",
        "CE47",
        "CE48",
        "CE49",
        "CE50",
        "CE51",
        "CE52",
        "TP1"
      }));
      this.presetComboBox.Items.Add((object) new Form1.PresetComboBoxItem("Harmless", false, new string[55]
      {
        "HE1",
        "HE2",
        "HE3",
        "HE4",
        "HE5",
        "HE7",
        "WA2",
        "WA3",
        "WE1",
        "WE2",
        "VE2",
        "VE3",
        "VE4",
        "VE5",
        "VE7",
        "VE8",
        "VE11",
        "VE12",
        "VE13",
        "VE14",
        "VE15",
        "PE3",
        "PE5",
        "PE8",
        "PE10",
        "PE11",
        "PE12",
        "PE13",
        "PE14",
        "PE15",
        "PE16",
        "PE17",
        "MO1",
        "MO2",
        "MO3",
        "MO4",
        "MO5",
        "ST2",
        "ST4",
        "ST6",
        "ST8",
        "ST10",
        "ST11",
        "ST12",
        "CE11",
        "CE12",
        "CE22",
        "CE23",
        "CE30",
        "CE40",
        "CE46",
        "CE47",
        "CE49",
        "CE51",
        "CE52"
      }));
      this.presetComboBox.Items.Add((object) new Form1.PresetComboBoxItem("Harmful", false, new string[107]
      {
        "HE6",
        "WA1",
        "WA4",
        "WE3",
        "WE4",
        "WE5",
        "WE6",
        "WE7",
        "SP1",
        "SP2",
        "SP3",
        "SP4",
        "SP5",
        "SP6",
        "SP7",
        "SP8",
        "SP9",
        "SP10",
        "SP11",
        "SP12",
        "SP13",
        "SP14",
        "SP15",
        "SP16",
        "SP17",
        "SP18",
        "SP19",
        "TI1",
        "TI2",
        "TI3",
        "TI4",
        "TI5",
        "TI6",
        "TI7",
        "VE1",
        "VE6",
        "VE9",
        "VE10",
        "PE1",
        "PE2",
        "PE4",
        "PE6",
        "PE7",
        "PE9",
        "PE18",
        "ST1",
        "ST3",
        "ST5",
        "ST7",
        "ST9",
        "CE1",
        "CE2",
        "CE3",
        "CE4",
        "CE5",
        "CE6",
        "CE7",
        "CE8",
        "CE9",
        "CE10",
        "CE11",
        "CE12",
        "CE13",
        "CE14",
        "CE15",
        "CE16",
        "CE17",
        "CE18",
        "CE19",
        "CE20",
        "CE21",
        "CE22",
        "CE23",
        "CE24",
        "CE25",
        "CE26",
        "CE27",
        "CE28",
        "CE29",
        "CE30",
        "CE31",
        "CE32",
        "CE33",
        "CE34",
        "CE35",
        "CE36",
        "CE37",
        "CE38",
        "CE39",
        "CE41",
        "CE43",
        "CE44",
        "CE45",
        "CE48",
        "CE50",
        "TP1",
        "TP2",
        "TP3",
        "TP4",
        "TP5",
        "TP6",
        "TP7",
        "TP8",
        "TP9",
        "TP10",
        "TP11",
        "TP12"
      }));
      this.presetComboBox.Items.Add((object) new Form1.PresetComboBoxItem("Good Luck", false, new string[95]
      {
        "HE6",
        "WA4",
        "WE5",
        "WE6",
        "WE7",
        "SP10",
        "SP11",
        "SP15",
        "SP16",
        "SP17",
        "SP19",
        "TI1",
        "TI2",
        "TI3",
        "TI4",
        "TI5",
        "TI6",
        "TI7",
        "VE1",
        "VE4",
        "VE6",
        "VE7",
        "VE9",
        "VE10",
        "VE14",
        "PE1",
        "PE2",
        "PE6",
        "PE7",
        "PE8",
        "PE9",
        "PE18",
        "ST1",
        "ST3",
        "ST5",
        "ST7",
        "ST9",
        "CE1",
        "CE2",
        "CE3",
        "CE4",
        "CE5",
        "CE6",
        "CE7",
        "CE8",
        "CE9",
        "CE10",
        "CE11",
        "CE12",
        "CE13",
        "CE14",
        "CE15",
        "CE16",
        "CE17",
        "CE18",
        "CE19",
        "CE20",
        "CE21",
        "CE22",
        "CE23",
        "CE24",
        "CE25",
        "CE26",
        "CE27",
        "CE28",
        "CE29",
        "CE30",
        "CE31",
        "CE32",
        "CE33",
        "CE34",
        "CE35",
        "CE36",
        "CE37",
        "CE38",
        "CE39",
        "CE41",
        "CE42",
        "CE43",
        "CE44",
        "CE45",
        "CE48",
        "CE50",
        "TP1",
        "TP2",
        "TP3",
        "TP4",
        "TP5",
        "TP6",
        "TP7",
        "TP8",
        "TP9",
        "TP10",
        "TP11",
        "TP12"
      }));
      this.presetComboBox.Items.Add((object) new Form1.PresetComboBoxItem("Everything", true, new string[0]));
      this.presetComboBox.Items.Add((object) new Form1.PresetComboBoxItem("Twitch Voting", true, new string[1]
      {
        "CE41"
      }));
      this.presetComboBox.Items.Add((object) new Form1.PresetComboBoxItem("Nothing", false, new string[0]));
      this.presetComboBox.SelectedIndex = 0;
    }

    private void PresetComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      Form1.PresetComboBoxItem selectedItem = (Form1.PresetComboBoxItem) this.presetComboBox.SelectedItem;
      this.LoadPreset(selectedItem.Reversed, selectedItem.EnabledEffects);
    }

    private void PopulateMainCooldowns()
    {
      this.comboBoxMainCooldown.Items.Add((object) new Form1.MainCooldownComboBoxItem("10 seconds", 10000));
      this.comboBoxMainCooldown.Items.Add((object) new Form1.MainCooldownComboBoxItem("20 seconds", 20000));
      this.comboBoxMainCooldown.Items.Add((object) new Form1.MainCooldownComboBoxItem("30 seconds", 30000));
      this.comboBoxMainCooldown.Items.Add((object) new Form1.MainCooldownComboBoxItem("1 minute", 60000));
      this.comboBoxMainCooldown.Items.Add((object) new Form1.MainCooldownComboBoxItem("2 minutes", 120000));
      this.comboBoxMainCooldown.Items.Add((object) new Form1.MainCooldownComboBoxItem("5 minutes", 300000));
      this.comboBoxMainCooldown.Items.Add((object) new Form1.MainCooldownComboBoxItem("10 minutes", 600000));
      this.comboBoxMainCooldown.SelectedIndex = 3;
      Config.Instance.MainCooldown = 60000;
    }

    private void MainCooldownComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      Form1.MainCooldownComboBoxItem selectedItem = (Form1.MainCooldownComboBoxItem) this.comboBoxMainCooldown.SelectedItem;
      Config.Instance.MainCooldown = selectedItem.Time;
      if (Config.Instance.Enabled)
        return;
      this.progressBarMain.Value = 0;
      this.progressBarMain.Maximum = Config.Instance.MainCooldown;
      this.elapsedCount = 0;
      this.Stopwatch.Reset();
    }

    private void PopulateVotingTimes()
    {
      this.comboBoxVotingTime.Items.Add((object) new Form1.VotingTimeComboBoxItem("5 seconds", 5000));
      this.comboBoxVotingTime.Items.Add((object) new Form1.VotingTimeComboBoxItem("10 seconds", 10000));
      this.comboBoxVotingTime.Items.Add((object) new Form1.VotingTimeComboBoxItem("15 seconds", 15000));
      this.comboBoxVotingTime.Items.Add((object) new Form1.VotingTimeComboBoxItem("20 seconds", 20000));
      this.comboBoxVotingTime.Items.Add((object) new Form1.VotingTimeComboBoxItem("30 seconds", 30000));
      this.comboBoxVotingTime.Items.Add((object) new Form1.VotingTimeComboBoxItem("1 minute", 60000));
      this.comboBoxVotingTime.SelectedIndex = 2;
      Config.Instance.TwitchVotingTime = 15000;
    }

    private void ComboBoxVotingTime_SelectedIndexChanged(object sender, EventArgs e)
    {
      Form1.VotingTimeComboBoxItem selectedItem = (Form1.VotingTimeComboBoxItem) this.comboBoxVotingTime.SelectedItem;
      Config.Instance.TwitchVotingTime = selectedItem.VotingTime;
    }

    private void PopulateVotingCooldowns()
    {
      this.comboBoxVotingCooldown.Items.Add((object) new Form1.VotingCooldownComboBoxItem("10 seconds", 10000));
      this.comboBoxVotingCooldown.Items.Add((object) new Form1.VotingCooldownComboBoxItem("30 seconds", 30000));
      this.comboBoxVotingCooldown.Items.Add((object) new Form1.VotingCooldownComboBoxItem("1 minute", 60000));
      this.comboBoxVotingCooldown.Items.Add((object) new Form1.VotingCooldownComboBoxItem("2 minutes", 120000));
      this.comboBoxVotingCooldown.Items.Add((object) new Form1.VotingCooldownComboBoxItem("5 minutes", 300000));
      this.comboBoxVotingCooldown.Items.Add((object) new Form1.VotingCooldownComboBoxItem("10 minutes", 600000));
      this.comboBoxVotingCooldown.SelectedIndex = 2;
      Config.Instance.TwitchVotingCooldown = 120000;
    }

    private void ComboBoxVotingCooldown_SelectedIndexChanged(object sender, EventArgs e)
    {
      Form1.VotingCooldownComboBoxItem selectedItem = (Form1.VotingCooldownComboBoxItem) this.comboBoxVotingCooldown.SelectedItem;
      Config.Instance.TwitchVotingCooldown = selectedItem.VotingCooldown;
    }

    private void SetAutostart()
    {
      this.buttonAutoStart.Enabled = Config.Instance.IsTwitchMode && this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
      this.buttonAutoStart.Text = "Auto-Start";
      this.Stopwatch.Reset();
      this.SetEnabled(true);
    }

    private void SetEnabled(bool enabled)
    {
      Config.Instance.Enabled = enabled;
      if (Config.Instance.Enabled)
        this.Stopwatch.Start();
      else
        this.Stopwatch.Stop();
      this.AutoStartTimer.Stop();
      this.buttonMainToggle.Enabled = true;
      (Config.Instance.IsTwitchMode ? (Control) this.buttonTwitchToggle : (Control) this.buttonMainToggle).Text = Config.Instance.Enabled ? "Stop / Pause" : "Start / Resume";
      this.comboBoxMainCooldown.Enabled = this.buttonSwitchMode.Enabled = this.buttonResetMain.Enabled = this.buttonResetTwitch.Enabled = !Config.Instance.Enabled;
      this.comboBoxVotingTime.Enabled = this.comboBoxVotingCooldown.Enabled = !Config.Instance.Enabled;
    }

    private void ButtonMainToggle_Click(object sender, EventArgs e) => this.SetEnabled(!Config.Instance.Enabled);

    private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
    {
      foreach (TreeNode node in treeNode.Nodes)
      {
        node.Checked = nodeChecked;
        if (node is Form1.EffectTreeNode effectTreeNode2)
          EffectDatabase.SetEffectEnabled(effectTreeNode2.Effect, effectTreeNode2.Checked);
        if (node.Nodes.Count > 0)
          this.CheckAllChildNodes(node, nodeChecked);
      }
    }

    private void EnabledEffectsView_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (e.Action == TreeViewAction.Unknown)
        return;
      if (e.Node is Form1.EffectTreeNode node1)
        EffectDatabase.SetEffectEnabled(node1.Effect, node1.Checked);
      if (e.Node.Nodes.Count > 0)
        this.CheckAllChildNodes(e.Node, e.Node.Checked);
      foreach (Form1.CategoryTreeNode node2 in this.enabledEffectsView.Nodes)
        node2.UpdateCategory();
    }

    private void LoadPreset(bool reversed, string[] enabledEffects)
    {
      foreach (TreeNode node in this.enabledEffectsView.Nodes)
      {
        node.Checked = !reversed;
        this.CheckAllChildNodes(node, reversed);
      }
      foreach (string enabledEffect in enabledEffects)
      {
        Form1.EffectTreeNode effectTreeNode;
        if (this.IdToEffectNodeMap.TryGetValue(enabledEffect, out effectTreeNode))
        {
          effectTreeNode.Checked = !reversed;
          EffectDatabase.SetEffectEnabled(effectTreeNode.Effect, !reversed);
        }
      }
      foreach (Form1.CategoryTreeNode node in this.enabledEffectsView.Nodes)
        node.UpdateCategory();
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void LoadPresetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = "Preset File|*.cfg";
      openFileDialog1.Title = "Save Preset";
      OpenFileDialog openFileDialog2 = openFileDialog1;
      int num = (int) openFileDialog2.ShowDialog();
      if (openFileDialog2.FileName != "")
      {
        string[] strArray = File.ReadAllText(openFileDialog2.FileName).Split(',');
        List<string> stringList = new List<string>();
        foreach (string str in strArray)
          stringList.Add(str);
        this.LoadPreset(false, stringList.ToArray());
      }
      openFileDialog2.Dispose();
    }

    private void SavePresetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<string> stringList = new List<string>();
      foreach (Form1.EffectTreeNode effectTreeNode in this.IdToEffectNodeMap.Values)
      {
        if (effectTreeNode.Checked)
          stringList.Add(effectTreeNode.Effect.Id);
      }
      string contents = string.Join(",", (IEnumerable<string>) stringList);
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.Filter = "Preset File|*.cfg";
      saveFileDialog1.Title = "Save Preset";
      SaveFileDialog saveFileDialog2 = saveFileDialog1;
      int num = (int) saveFileDialog2.ShowDialog();
      if (saveFileDialog2.FileName != "")
        File.WriteAllText(saveFileDialog2.FileName, contents);
      saveFileDialog2.Dispose();
    }

    private void ButtonConnectTwitch_Click(object sender, EventArgs e)
    {
      if (this.Twitch != null && this.Twitch.Client.IsConnected)
      {
        this.Twitch?.Kill();
        this.Twitch = (TwitchConnection) null;
        this.comboBoxVotingTime.Enabled = true;
        this.comboBoxVotingCooldown.Enabled = true;
        this.textBoxTwitchChannel.Enabled = true;
        this.textBoxTwitchUsername.Enabled = true;
        this.textBoxTwitchOAuth.Enabled = true;
        this.buttonConnectTwitch.Text = "Connect to Twitch";
        if (this.tabSettings.TabPages.Contains(this.tabEffects))
          return;
        this.tabSettings.TabPages.Insert(this.tabSettings.TabPages.IndexOf(this.tabTwitch), this.tabEffects);
      }
      else
      {
        if (!(Config.Instance.TwitchChannel != "") || !(Config.Instance.TwitchUsername != "") || !(Config.Instance.TwitchOAuthToken != ""))
          return;
        this.buttonConnectTwitch.Enabled = false;
        this.Twitch = new TwitchConnection();
        this.Twitch.OnRapidFireEffect += (EventHandler<TwitchConnection.RapidFireEventArgs>) ((_sender, rapidFireArgs) => this.Invoke((Action) (() =>
        {
          if (Config.Instance.TwitchVotingMode != 2)
            return;
          rapidFireArgs.Effect.RunEffect();
          this.AddEffectToListBox(rapidFireArgs.Effect);
        })));
        this.Twitch.Client.OnIncorrectLogin += (EventHandler<OnIncorrectLoginArgs>) ((_sender, _e) =>
        {
          int num = (int) MessageBox.Show("There was an error trying to log in to the account. Wrong username / OAuth token?", "Twitch Login Error");
          this.Invoke((Action) (() => this.buttonConnectTwitch.Enabled = true));
          this.Twitch.Kill();
        });
        this.Twitch.Client.OnConnected += (EventHandler<OnConnectedArgs>) ((_sender, _e) => this.Invoke((Action) (() =>
        {
          this.buttonConnectTwitch.Enabled = true;
          this.buttonTwitchToggle.Enabled = true;
          this.buttonAutoStart.Enabled = true;
          this.buttonConnectTwitch.Text = "Disconnect";
          this.textBoxTwitchChannel.Enabled = false;
          this.textBoxTwitchUsername.Enabled = false;
          this.textBoxTwitchOAuth.Enabled = false;
        })));
      }
    }

    private void UpdateConnectTwitchState()
    {
      this.buttonConnectTwitch.Enabled = this.textBoxTwitchChannel.Text != "" && this.textBoxTwitchUsername.Text != "" && this.textBoxTwitchOAuth.Text != "";
      this.textBoxTwitchChannel.Enabled = this.textBoxTwitchUsername.Enabled = this.textBoxTwitchOAuth.Enabled = true;
    }

    private void TextBoxTwitchChannel_TextChanged(object sender, EventArgs e)
    {
      Config.Instance.TwitchChannel = this.textBoxTwitchChannel.Text;
      this.UpdateConnectTwitchState();
    }

    private void TextBoxUsername_TextChanged(object sender, EventArgs e)
    {
      Config.Instance.TwitchUsername = this.textBoxTwitchUsername.Text;
      this.UpdateConnectTwitchState();
    }

    private void TextBoxOAuth_TextChanged(object sender, EventArgs e)
    {
      Config.Instance.TwitchOAuthToken = this.textBoxTwitchOAuth.Text;
      this.UpdateConnectTwitchState();
    }

    private void ButtonSwitchMode_Click(object sender, EventArgs e)
    {
      if (Config.Instance.IsTwitchMode)
      {
        Config.Instance.IsTwitchMode = false;
        this.buttonSwitchMode.Text = "Twitch";
        this.tabSettings.TabPages.Insert(0, this.tabMain);
        this.tabSettings.SelectedIndex = 0;
        this.tabSettings.TabPages.Remove(this.tabTwitch);
        this.listLastEffectsMain.Items.Clear();
        this.progressBarMain.Value = 0;
        this.elapsedCount = 0;
        this.Stopwatch.Reset();
        this.SetEnabled(false);
      }
      else
      {
        Config.Instance.IsTwitchMode = true;
        this.buttonSwitchMode.Text = "Main";
        this.buttonAutoStart.Enabled = this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
        this.tabSettings.TabPages.Insert(0, this.tabTwitch);
        this.tabSettings.SelectedIndex = 0;
        this.tabSettings.TabPages.Remove(this.tabMain);
        this.listLastEffectsTwitch.Items.Clear();
        this.progressBarTwitch.Value = 0;
        this.elapsedCount = 0;
        this.Stopwatch.Reset();
        this.SetEnabled(false);
      }
    }

    private void ButtonTwitchToggle_Click(object sender, EventArgs e) => this.SetEnabled(!Config.Instance.Enabled);

    private void TextBoxSeed_TextChanged(object sender, EventArgs e)
    {
      Config.Instance.Seed = this.textBoxSeed.Text;
      RandomHandler.SetSeed(Config.Instance.Seed);
    }

    private void ButtonTestSeed_Click(object sender, EventArgs e) => this.labelTestSeed.Text = string.Format("{0}", (object) RandomHandler.Next(100, 999));

    private void ButtonGenericTest_Click(object sender, EventArgs e)
    {
      ProcessHooker.SendEffectToGame("effect", "set_vehicle_on_fire", 60000, "Set Vehicle On Fire");
      ProcessHooker.SendEffectToGame("timed_effect", "one_hit_ko", 60000, "One Hit K.O.", "25characterusernamehanice");
    }

    private void ButtonResetMain_Click(object sender, EventArgs e)
    {
      this.SetEnabled(false);
      this.Stopwatch.Reset();
      this.elapsedCount = 0;
      this.progressBarMain.Value = 0;
      this.buttonMainToggle.Enabled = true;
      this.buttonMainToggle.Text = "Start / Resume";
      this.buttonAutoStart.Enabled = true;
      this.buttonAutoStart.Text = "Auto-Start";
    }

    private void CheckBoxContinueTimer_CheckedChanged(object sender, EventArgs e) => Config.Instance.ContinueTimer = this.checkBoxContinueTimer.Checked;

    private void CheckBoxCrypticEffects_CheckedChanged(object sender, EventArgs e) => Config.Instance.CrypticEffects = this.checkBoxCrypticEffects.Checked;

    private void CheckBoxShowLastEffectsMain_CheckedChanged(object sender, EventArgs e) => Config.Instance.MainShowLastEffects = this.listLastEffectsMain.Visible = this.checkBoxShowLastEffectsMain.Checked;

    private void CheckBoxShowLastEffectsTwitch_CheckedChanged(object sender, EventArgs e) => Config.Instance.TwitchShowLastEffects = this.listLastEffectsTwitch.Visible = this.checkBoxShowLastEffectsTwitch.Checked;

    private void CheckBoxTwitchAllowOnlyEnabledEffects_CheckedChanged(object sender, EventArgs e) => Config.Instance.TwitchAllowOnlyEnabledEffectsRapidFire = this.checkBoxTwitchAllowOnlyEnabledEffects.Checked;

    private void CheckBoxTwitchMajorityVoting_CheckedChanged(object sender, EventArgs e) => Config.Instance.TwitchMajorityVoting = this.checkBoxTwitchMajorityVoting.Checked;

    private void ButtonResetTwitch_Click(object sender, EventArgs e)
    {
      this.SetEnabled(false);
      this.Stopwatch.Reset();
      this.elapsedCount = 0;
      this.progressBarTwitch.Value = 0;
      this.buttonTwitchToggle.Enabled = this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
      this.buttonTwitchToggle.Text = "Start / Resume";
      this.buttonAutoStart.Enabled = this.Twitch != null && this.Twitch.Client != null && this.Twitch.Client.IsConnected;
      this.buttonAutoStart.Text = "Auto-Start";
    }

    private void CheckBoxTwitch3TimesCooldown_CheckedChanged(object sender, EventArgs e) => Config.Instance.Twitch3TimesCooldown = this.checkBoxTwitch3TimesCooldown.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.buttonMainToggle = new Button();
      this.progressBarMain = new ProgressBar();
      this.tabSettings = new TabControl();
      this.tabMain = new TabPage();
      this.checkBoxShowLastEffectsMain = new CheckBox();
      this.buttonResetMain = new Button();
      this.label2 = new Label();
      this.comboBoxMainCooldown = new ComboBox();
      this.listLastEffectsMain = new ListBox();
      this.tabTwitch = new TabPage();
      this.label3 = new Label();
      this.textBoxTwitchChannel = new TextBox();
      this.buttonResetTwitch = new Button();
      this.checkBoxTwitchMajorityVoting = new CheckBox();
      this.checkBoxTwitchAllowOnlyEnabledEffects = new CheckBox();
      this.checkBoxShowLastEffectsTwitch = new CheckBox();
      this.labelTwitchCurrentMode = new Label();
      this.buttonTwitchToggle = new Button();
      this.comboBoxVotingCooldown = new ComboBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.comboBoxVotingTime = new ComboBox();
      this.progressBarTwitch = new ProgressBar();
      this.listLastEffectsTwitch = new ListBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.textBoxTwitchOAuth = new TextBox();
      this.textBoxTwitchUsername = new TextBox();
      this.buttonConnectTwitch = new Button();
      this.tabEffects = new TabPage();
      this.enabledEffectsView = new TreeView();
      this.label1 = new Label();
      this.presetComboBox = new ComboBox();
      this.tabPage1 = new TabPage();
      this.checkBoxCrypticEffects = new CheckBox();
      this.checkBoxContinueTimer = new CheckBox();
      this.label8 = new Label();
      this.textBoxSeed = new TextBox();
      this.tabDebug = new TabPage();
      this.buttonGenericTest = new Button();
      this.buttonTestSeed = new Button();
      this.labelTestSeed = new Label();
      this.menuStrip1 = new MenuStrip();
      this.fileToolStripMenuItem = new ToolStripMenuItem();
      this.loadPresetToolStripMenuItem = new ToolStripMenuItem();
      this.savePresetToolStripMenuItem = new ToolStripMenuItem();
      this.exitToolStripMenuItem = new ToolStripMenuItem();
      this.toolTipHandler = new ToolTip(this.components);
      this.buttonAutoStart = new Button();
      this.timerMain = new System.Windows.Forms.Timer(this.components);
      this.buttonSwitchMode = new Button();
      this.checkBoxTwitch3TimesCooldown = new CheckBox();
      this.tabSettings.SuspendLayout();
      this.tabMain.SuspendLayout();
      this.tabTwitch.SuspendLayout();
      this.tabEffects.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabDebug.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.buttonMainToggle.Location = new Point(6, 6);
      this.buttonMainToggle.Name = "buttonMainToggle";
      this.buttonMainToggle.Size = new Size(94, 23);
      this.buttonMainToggle.TabIndex = 0;
      this.buttonMainToggle.Text = "Start / Resume";
      this.buttonMainToggle.UseVisualStyleBackColor = true;
      this.buttonMainToggle.Click += new EventHandler(this.ButtonMainToggle_Click);
      this.progressBarMain.Location = new Point(206, 6);
      this.progressBarMain.Maximum = 60;
      this.progressBarMain.Name = "progressBarMain";
      this.progressBarMain.Size = new Size(240, 23);
      this.progressBarMain.Step = 1;
      this.progressBarMain.TabIndex = 1;
      this.tabSettings.Controls.Add((Control) this.tabMain);
      this.tabSettings.Controls.Add((Control) this.tabTwitch);
      this.tabSettings.Controls.Add((Control) this.tabEffects);
      this.tabSettings.Controls.Add((Control) this.tabPage1);
      this.tabSettings.Controls.Add((Control) this.tabDebug);
      this.tabSettings.Location = new Point(12, 56);
      this.tabSettings.Name = "tabSettings";
      this.tabSettings.SelectedIndex = 0;
      this.tabSettings.Size = new Size(460, 257);
      this.tabSettings.TabIndex = 4;
      this.tabMain.BackColor = Color.Transparent;
      this.tabMain.Controls.Add((Control) this.checkBoxShowLastEffectsMain);
      this.tabMain.Controls.Add((Control) this.buttonResetMain);
      this.tabMain.Controls.Add((Control) this.label2);
      this.tabMain.Controls.Add((Control) this.comboBoxMainCooldown);
      this.tabMain.Controls.Add((Control) this.buttonMainToggle);
      this.tabMain.Controls.Add((Control) this.listLastEffectsMain);
      this.tabMain.Controls.Add((Control) this.progressBarMain);
      this.tabMain.Location = new Point(4, 22);
      this.tabMain.Name = "tabMain";
      this.tabMain.Padding = new Padding(3);
      this.tabMain.Size = new Size(452, 231);
      this.tabMain.TabIndex = 0;
      this.tabMain.Text = "Main";
      this.checkBoxShowLastEffectsMain.AutoSize = true;
      this.checkBoxShowLastEffectsMain.Checked = true;
      this.checkBoxShowLastEffectsMain.CheckState = CheckState.Checked;
      this.checkBoxShowLastEffectsMain.Location = new Point(6, 107);
      this.checkBoxShowLastEffectsMain.Name = "checkBoxShowLastEffectsMain";
      this.checkBoxShowLastEffectsMain.Size = new Size(112, 17);
      this.checkBoxShowLastEffectsMain.TabIndex = 8;
      this.checkBoxShowLastEffectsMain.Text = "Show Last Effects";
      this.checkBoxShowLastEffectsMain.UseVisualStyleBackColor = true;
      this.checkBoxShowLastEffectsMain.CheckedChanged += new EventHandler(this.CheckBoxShowLastEffectsMain_CheckedChanged);
      this.buttonResetMain.Location = new Point(106, 6);
      this.buttonResetMain.Name = "buttonResetMain";
      this.buttonResetMain.Size = new Size(94, 23);
      this.buttonResetMain.TabIndex = 7;
      this.buttonResetMain.Text = "Reset";
      this.buttonResetMain.UseVisualStyleBackColor = true;
      this.buttonResetMain.Click += new EventHandler(this.ButtonResetMain_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(262, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(57, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Cooldown:";
      this.comboBoxMainCooldown.FormattingEnabled = true;
      this.comboBoxMainCooldown.Location = new Point(325, 35);
      this.comboBoxMainCooldown.Name = "comboBoxMainCooldown";
      this.comboBoxMainCooldown.Size = new Size(121, 21);
      this.comboBoxMainCooldown.TabIndex = 5;
      this.comboBoxMainCooldown.SelectedIndexChanged += new EventHandler(this.MainCooldownComboBox_SelectedIndexChanged);
      this.listLastEffectsMain.FormattingEnabled = true;
      this.listLastEffectsMain.Location = new Point(6, 130);
      this.listLastEffectsMain.Name = "listLastEffectsMain";
      this.listLastEffectsMain.Size = new Size(440, 95);
      this.listLastEffectsMain.TabIndex = 4;
      this.tabTwitch.BackColor = Color.Transparent;
      this.tabTwitch.Controls.Add((Control) this.checkBoxTwitch3TimesCooldown);
      this.tabTwitch.Controls.Add((Control) this.label3);
      this.tabTwitch.Controls.Add((Control) this.textBoxTwitchChannel);
      this.tabTwitch.Controls.Add((Control) this.buttonResetTwitch);
      this.tabTwitch.Controls.Add((Control) this.checkBoxTwitchMajorityVoting);
      this.tabTwitch.Controls.Add((Control) this.checkBoxTwitchAllowOnlyEnabledEffects);
      this.tabTwitch.Controls.Add((Control) this.checkBoxShowLastEffectsTwitch);
      this.tabTwitch.Controls.Add((Control) this.labelTwitchCurrentMode);
      this.tabTwitch.Controls.Add((Control) this.buttonTwitchToggle);
      this.tabTwitch.Controls.Add((Control) this.comboBoxVotingCooldown);
      this.tabTwitch.Controls.Add((Control) this.label7);
      this.tabTwitch.Controls.Add((Control) this.label6);
      this.tabTwitch.Controls.Add((Control) this.comboBoxVotingTime);
      this.tabTwitch.Controls.Add((Control) this.progressBarTwitch);
      this.tabTwitch.Controls.Add((Control) this.listLastEffectsTwitch);
      this.tabTwitch.Controls.Add((Control) this.label5);
      this.tabTwitch.Controls.Add((Control) this.label4);
      this.tabTwitch.Controls.Add((Control) this.textBoxTwitchOAuth);
      this.tabTwitch.Controls.Add((Control) this.textBoxTwitchUsername);
      this.tabTwitch.Controls.Add((Control) this.buttonConnectTwitch);
      this.tabTwitch.Location = new Point(4, 22);
      this.tabTwitch.Name = "tabTwitch";
      this.tabTwitch.Size = new Size(452, 231);
      this.tabTwitch.TabIndex = 2;
      this.tabTwitch.Text = "Twitch";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(49, 13);
      this.label3.TabIndex = 23;
      this.label3.Text = "Channel:";
      this.textBoxTwitchChannel.Location = new Point(83, 3);
      this.textBoxTwitchChannel.Name = "textBoxTwitchChannel";
      this.textBoxTwitchChannel.Size = new Size(205, 20);
      this.textBoxTwitchChannel.TabIndex = 1;
      this.textBoxTwitchChannel.TextChanged += new EventHandler(this.TextBoxTwitchChannel_TextChanged);
      this.buttonResetTwitch.Enabled = false;
      this.buttonResetTwitch.Location = new Point(294, 58);
      this.buttonResetTwitch.Name = "buttonResetTwitch";
      this.buttonResetTwitch.Size = new Size(155, 23);
      this.buttonResetTwitch.TabIndex = 21;
      this.buttonResetTwitch.Text = "Reset";
      this.buttonResetTwitch.UseVisualStyleBackColor = true;
      this.buttonResetTwitch.Click += new EventHandler(this.ButtonResetTwitch_Click);
      this.checkBoxTwitchMajorityVoting.AutoSize = true;
      this.checkBoxTwitchMajorityVoting.Checked = true;
      this.checkBoxTwitchMajorityVoting.CheckState = CheckState.Checked;
      this.checkBoxTwitchMajorityVoting.Location = new Point(354, 87);
      this.checkBoxTwitchMajorityVoting.Name = "checkBoxTwitchMajorityVoting";
      this.checkBoxTwitchMajorityVoting.RightToLeft = RightToLeft.Yes;
      this.checkBoxTwitchMajorityVoting.Size = new Size(95, 17);
      this.checkBoxTwitchMajorityVoting.TabIndex = 20;
      this.checkBoxTwitchMajorityVoting.Text = "Majority Voting";
      this.toolTipHandler.SetToolTip((Control) this.checkBoxTwitchMajorityVoting, "When enabled the effect that has the most votes will be enabled.");
      this.checkBoxTwitchMajorityVoting.UseVisualStyleBackColor = true;
      this.checkBoxTwitchMajorityVoting.CheckedChanged += new EventHandler(this.CheckBoxTwitchMajorityVoting_CheckedChanged);
      this.checkBoxTwitchAllowOnlyEnabledEffects.AutoSize = true;
      this.checkBoxTwitchAllowOnlyEnabledEffects.Location = new Point(137, 81);
      this.checkBoxTwitchAllowOnlyEnabledEffects.Name = "checkBoxTwitchAllowOnlyEnabledEffects";
      this.checkBoxTwitchAllowOnlyEnabledEffects.RightToLeft = RightToLeft.Yes;
      this.checkBoxTwitchAllowOnlyEnabledEffects.Size = new Size(148, 17);
      this.checkBoxTwitchAllowOnlyEnabledEffects.TabIndex = 19;
      this.checkBoxTwitchAllowOnlyEnabledEffects.Text = "Only Enabled Effects (RF)";
      this.toolTipHandler.SetToolTip((Control) this.checkBoxTwitchAllowOnlyEnabledEffects, "Only allow effects that are enabled\r\nin the currently active preset during Rapid-Fire.");
      this.checkBoxTwitchAllowOnlyEnabledEffects.UseVisualStyleBackColor = true;
      this.checkBoxTwitchAllowOnlyEnabledEffects.CheckedChanged += new EventHandler(this.CheckBoxTwitchAllowOnlyEnabledEffects_CheckedChanged);
      this.checkBoxShowLastEffectsTwitch.AutoSize = true;
      this.checkBoxShowLastEffectsTwitch.Checked = true;
      this.checkBoxShowLastEffectsTwitch.CheckState = CheckState.Checked;
      this.checkBoxShowLastEffectsTwitch.Location = new Point(3, 81);
      this.checkBoxShowLastEffectsTwitch.Name = "checkBoxShowLastEffectsTwitch";
      this.checkBoxShowLastEffectsTwitch.Size = new Size(112, 17);
      this.checkBoxShowLastEffectsTwitch.TabIndex = 18;
      this.checkBoxShowLastEffectsTwitch.Text = "Show Last Effects";
      this.toolTipHandler.SetToolTip((Control) this.checkBoxShowLastEffectsTwitch, "When enabled the effects won't be sent to the game but instead only to announced in chat.");
      this.checkBoxShowLastEffectsTwitch.UseVisualStyleBackColor = true;
      this.checkBoxShowLastEffectsTwitch.CheckedChanged += new EventHandler(this.CheckBoxShowLastEffectsTwitch_CheckedChanged);
      this.labelTwitchCurrentMode.AutoSize = true;
      this.labelTwitchCurrentMode.Location = new Point(5, 208);
      this.labelTwitchCurrentMode.Name = "labelTwitchCurrentMode";
      this.labelTwitchCurrentMode.Size = new Size(0, 13);
      this.labelTwitchCurrentMode.TabIndex = 17;
      this.buttonTwitchToggle.Enabled = false;
      this.buttonTwitchToggle.Location = new Point(294, 29);
      this.buttonTwitchToggle.Name = "buttonTwitchToggle";
      this.buttonTwitchToggle.Size = new Size(155, 23);
      this.buttonTwitchToggle.TabIndex = 15;
      this.buttonTwitchToggle.Text = "Start / Resume";
      this.buttonTwitchToggle.UseVisualStyleBackColor = true;
      this.buttonTwitchToggle.Click += new EventHandler(this.ButtonTwitchToggle_Click);
      this.comboBoxVotingCooldown.FormattingEnabled = true;
      this.comboBoxVotingCooldown.Location = new Point(294, 178);
      this.comboBoxVotingCooldown.Name = "comboBoxVotingCooldown";
      this.comboBoxVotingCooldown.Size = new Size(155, 21);
      this.comboBoxVotingCooldown.TabIndex = 14;
      this.comboBoxVotingCooldown.SelectedIndexChanged += new EventHandler(this.ComboBoxVotingCooldown_SelectedIndexChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(291, 162);
      this.label7.Name = "label7";
      this.label7.Size = new Size(90, 13);
      this.label7.TabIndex = 13;
      this.label7.Text = "Voting Cooldown:";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(291, 122);
      this.label6.Name = "label6";
      this.label6.Size = new Size(66, 13);
      this.label6.TabIndex = 12;
      this.label6.Text = "Voting Time:";
      this.comboBoxVotingTime.FormattingEnabled = true;
      this.comboBoxVotingTime.Location = new Point(294, 138);
      this.comboBoxVotingTime.Name = "comboBoxVotingTime";
      this.comboBoxVotingTime.Size = new Size(155, 21);
      this.comboBoxVotingTime.TabIndex = 11;
      this.comboBoxVotingTime.SelectedIndexChanged += new EventHandler(this.ComboBoxVotingTime_SelectedIndexChanged);
      this.progressBarTwitch.Location = new Point(3, 205);
      this.progressBarTwitch.Name = "progressBarTwitch";
      this.progressBarTwitch.Size = new Size(446, 23);
      this.progressBarTwitch.TabIndex = 10;
      this.listLastEffectsTwitch.FormattingEnabled = true;
      this.listLastEffectsTwitch.Location = new Point(3, 104);
      this.listLastEffectsTwitch.Name = "listLastEffectsTwitch";
      this.listLastEffectsTwitch.Size = new Size(282, 95);
      this.listLastEffectsTwitch.TabIndex = 8;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(5, 61);
      this.label5.Name = "label5";
      this.label5.Size = new Size(74, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "OAuth Token:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(5, 34);
      this.label4.Name = "label4";
      this.label4.Size = new Size(58, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "Username:";
      this.textBoxTwitchOAuth.Location = new Point(83, 58);
      this.textBoxTwitchOAuth.Name = "textBoxTwitchOAuth";
      this.textBoxTwitchOAuth.PasswordChar = '*';
      this.textBoxTwitchOAuth.Size = new Size(205, 20);
      this.textBoxTwitchOAuth.TabIndex = 3;
      this.textBoxTwitchOAuth.TextChanged += new EventHandler(this.TextBoxOAuth_TextChanged);
      this.textBoxTwitchUsername.Location = new Point(83, 29);
      this.textBoxTwitchUsername.Name = "textBoxTwitchUsername";
      this.textBoxTwitchUsername.Size = new Size(205, 20);
      this.textBoxTwitchUsername.TabIndex = 2;
      this.textBoxTwitchUsername.TextChanged += new EventHandler(this.TextBoxUsername_TextChanged);
      this.buttonConnectTwitch.Enabled = false;
      this.buttonConnectTwitch.Location = new Point(294, 2);
      this.buttonConnectTwitch.Name = "buttonConnectTwitch";
      this.buttonConnectTwitch.Size = new Size(155, 23);
      this.buttonConnectTwitch.TabIndex = 1;
      this.buttonConnectTwitch.Text = "Connect to Twitch";
      this.buttonConnectTwitch.UseVisualStyleBackColor = true;
      this.buttonConnectTwitch.Click += new EventHandler(this.ButtonConnectTwitch_Click);
      this.tabEffects.BackColor = Color.Transparent;
      this.tabEffects.Controls.Add((Control) this.enabledEffectsView);
      this.tabEffects.Controls.Add((Control) this.label1);
      this.tabEffects.Controls.Add((Control) this.presetComboBox);
      this.tabEffects.Location = new Point(4, 22);
      this.tabEffects.Name = "tabEffects";
      this.tabEffects.Padding = new Padding(3);
      this.tabEffects.Size = new Size(452, 231);
      this.tabEffects.TabIndex = 1;
      this.tabEffects.Text = "Effects";
      this.enabledEffectsView.CheckBoxes = true;
      this.enabledEffectsView.Location = new Point(6, 6);
      this.enabledEffectsView.Name = "enabledEffectsView";
      this.enabledEffectsView.Size = new Size(440, 163);
      this.enabledEffectsView.TabIndex = 3;
      this.enabledEffectsView.AfterCheck += new TreeViewEventHandler(this.EnabledEffectsView_AfterCheck);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 178);
      this.label1.Name = "label1";
      this.label1.Size = new Size(45, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Presets:";
      this.presetComboBox.FormattingEnabled = true;
      this.presetComboBox.Location = new Point(57, 175);
      this.presetComboBox.Name = "presetComboBox";
      this.presetComboBox.Size = new Size(389, 21);
      this.presetComboBox.TabIndex = 1;
      this.presetComboBox.SelectedIndexChanged += new EventHandler(this.PresetComboBox_SelectedIndexChanged);
      this.tabPage1.BackColor = Color.Transparent;
      this.tabPage1.Controls.Add((Control) this.checkBoxCrypticEffects);
      this.tabPage1.Controls.Add((Control) this.checkBoxContinueTimer);
      this.tabPage1.Controls.Add((Control) this.label8);
      this.tabPage1.Controls.Add((Control) this.textBoxSeed);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(452, 231);
      this.tabPage1.TabIndex = 3;
      this.tabPage1.Text = "Settings";
      this.checkBoxCrypticEffects.AutoSize = true;
      this.checkBoxCrypticEffects.Location = new Point(6, 185);
      this.checkBoxCrypticEffects.Name = "checkBoxCrypticEffects";
      this.checkBoxCrypticEffects.Size = new Size(94, 17);
      this.checkBoxCrypticEffects.TabIndex = 5;
      this.checkBoxCrypticEffects.Text = "Cryptic Effects";
      this.toolTipHandler.SetToolTip((Control) this.checkBoxCrypticEffects, "Sends all effects to the game as cryptic ones.");
      this.checkBoxCrypticEffects.UseVisualStyleBackColor = true;
      this.checkBoxCrypticEffects.CheckedChanged += new EventHandler(this.CheckBoxCrypticEffects_CheckedChanged);
      this.checkBoxContinueTimer.AutoSize = true;
      this.checkBoxContinueTimer.Checked = true;
      this.checkBoxContinueTimer.CheckState = CheckState.Checked;
      this.checkBoxContinueTimer.Location = new Point(6, 208);
      this.checkBoxContinueTimer.Name = "checkBoxContinueTimer";
      this.checkBoxContinueTimer.Size = new Size(210, 17);
      this.checkBoxContinueTimer.TabIndex = 4;
      this.checkBoxContinueTimer.Text = "Continue Timer on Game Close / Crash";
      this.checkBoxContinueTimer.UseVisualStyleBackColor = true;
      this.checkBoxContinueTimer.CheckedChanged += new EventHandler(this.CheckBoxContinueTimer_CheckedChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 9);
      this.label8.Name = "label8";
      this.label8.Size = new Size(35, 13);
      this.label8.TabIndex = 3;
      this.label8.Text = "Seed:";
      this.label8.TextAlign = ContentAlignment.TopRight;
      this.textBoxSeed.Location = new Point(47, 6);
      this.textBoxSeed.Name = "textBoxSeed";
      this.textBoxSeed.Size = new Size(399, 20);
      this.textBoxSeed.TabIndex = 1;
      this.textBoxSeed.TextChanged += new EventHandler(this.TextBoxSeed_TextChanged);
      this.tabDebug.BackColor = Color.Transparent;
      this.tabDebug.Controls.Add((Control) this.buttonGenericTest);
      this.tabDebug.Controls.Add((Control) this.buttonTestSeed);
      this.tabDebug.Controls.Add((Control) this.labelTestSeed);
      this.tabDebug.Location = new Point(4, 22);
      this.tabDebug.Name = "tabDebug";
      this.tabDebug.Padding = new Padding(3);
      this.tabDebug.Size = new Size(452, 231);
      this.tabDebug.TabIndex = 4;
      this.tabDebug.Text = "Debug";
      this.buttonGenericTest.Location = new Point(6, 202);
      this.buttonGenericTest.Name = "buttonGenericTest";
      this.buttonGenericTest.Size = new Size(91, 23);
      this.buttonGenericTest.TabIndex = 10;
      this.buttonGenericTest.Text = "Test Something";
      this.buttonGenericTest.UseVisualStyleBackColor = true;
      this.buttonGenericTest.Click += new EventHandler(this.ButtonGenericTest_Click);
      this.buttonTestSeed.Location = new Point(3, 6);
      this.buttonTestSeed.Name = "buttonTestSeed";
      this.buttonTestSeed.Size = new Size(75, 23);
      this.buttonTestSeed.TabIndex = 9;
      this.buttonTestSeed.Text = "Test Seed";
      this.buttonTestSeed.UseVisualStyleBackColor = true;
      this.buttonTestSeed.Click += new EventHandler(this.ButtonTestSeed_Click);
      this.labelTestSeed.AutoSize = true;
      this.labelTestSeed.Location = new Point(84, 11);
      this.labelTestSeed.Name = "labelTestSeed";
      this.labelTestSeed.Size = new Size(13, 13);
      this.labelTestSeed.TabIndex = 8;
      this.labelTestSeed.Text = "0";
      this.labelTestSeed.TextAlign = ContentAlignment.TopRight;
      this.menuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.fileToolStripMenuItem
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(482, 24);
      this.menuStrip1.TabIndex = 5;
      this.menuStrip1.Text = "menuStrip1";
      this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.loadPresetToolStripMenuItem,
        (ToolStripItem) this.savePresetToolStripMenuItem,
        (ToolStripItem) this.exitToolStripMenuItem
      });
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      this.loadPresetToolStripMenuItem.Name = "loadPresetToolStripMenuItem";
      this.loadPresetToolStripMenuItem.Size = new Size(135, 22);
      this.loadPresetToolStripMenuItem.Text = "Load Preset";
      this.loadPresetToolStripMenuItem.Click += new EventHandler(this.LoadPresetToolStripMenuItem_Click);
      this.savePresetToolStripMenuItem.Name = "savePresetToolStripMenuItem";
      this.savePresetToolStripMenuItem.Size = new Size(135, 22);
      this.savePresetToolStripMenuItem.Text = "Save Preset";
      this.savePresetToolStripMenuItem.Click += new EventHandler(this.SavePresetToolStripMenuItem_Click);
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new Size(135, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new EventHandler(this.ExitToolStripMenuItem_Click);
      this.buttonAutoStart.Location = new Point(12, 27);
      this.buttonAutoStart.Name = "buttonAutoStart";
      this.buttonAutoStart.Size = new Size(75, 23);
      this.buttonAutoStart.TabIndex = 6;
      this.buttonAutoStart.Text = "Auto-Start";
      this.buttonAutoStart.UseVisualStyleBackColor = true;
      this.buttonAutoStart.Click += new EventHandler(this.ButtonAutoStart_Click);
      this.timerMain.Enabled = true;
      this.timerMain.Interval = 10;
      this.timerMain.Tick += new EventHandler(this.OnTimerTick);
      this.buttonSwitchMode.Location = new Point(393, 27);
      this.buttonSwitchMode.Name = "buttonSwitchMode";
      this.buttonSwitchMode.Size = new Size(75, 23);
      this.buttonSwitchMode.TabIndex = 7;
      this.buttonSwitchMode.Text = "Twitch";
      this.buttonSwitchMode.UseVisualStyleBackColor = true;
      this.buttonSwitchMode.Click += new EventHandler(this.ButtonSwitchMode_Click);
      this.checkBoxTwitch3TimesCooldown.AutoSize = true;
      this.checkBoxTwitch3TimesCooldown.Location = new Point(362, 104);
      this.checkBoxTwitch3TimesCooldown.Name = "checkBoxTwitch3TimesCooldown";
      this.checkBoxTwitch3TimesCooldown.RightToLeft = RightToLeft.Yes;
      this.checkBoxTwitch3TimesCooldown.Size = new Size(87, 17);
      this.checkBoxTwitch3TimesCooldown.TabIndex = 24;
      this.checkBoxTwitch3TimesCooldown.Text = "3x Cooldown";
      this.toolTipHandler.SetToolTip((Control) this.checkBoxTwitch3TimesCooldown, "When enabled effects will have 3x their cooldown.\r\n(Cooldown in this case is the Voting Time + Voting Cooldown)");
      this.checkBoxTwitch3TimesCooldown.UseVisualStyleBackColor = true;
      this.checkBoxTwitch3TimesCooldown.CheckedChanged += new EventHandler(this.CheckBoxTwitch3TimesCooldown_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(482, 325);
      this.Controls.Add((Control) this.buttonSwitchMode);
      this.Controls.Add((Control) this.buttonAutoStart);
      this.Controls.Add((Control) this.tabSettings);
      this.Controls.Add((Control) this.menuStrip1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MainMenuStrip = this.menuStrip1;
      this.MaximizeBox = false;
      //this.Name = nameof (Form1);
      this.Name = "Chaos";
      this.Text = "GTA:SA Chaos";
      this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
      this.tabSettings.ResumeLayout(false);
      this.tabMain.ResumeLayout(false);
      this.tabMain.PerformLayout();
      this.tabTwitch.ResumeLayout(false);
      this.tabTwitch.PerformLayout();
      this.tabEffects.ResumeLayout(false);
      this.tabEffects.PerformLayout();
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabDebug.ResumeLayout(false);
      this.tabDebug.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class PresetComboBoxItem
    {
      public readonly string Text;
      public readonly bool Reversed;
      public readonly string[] EnabledEffects;

      public PresetComboBoxItem(string text, bool reversed, string[] enabledEffects)
      {
        this.Text = text;
        this.Reversed = reversed;
        this.EnabledEffects = enabledEffects;
      }

      public override string ToString() => this.Text;
    }

    private class MainCooldownComboBoxItem
    {
      public readonly string Text;
      public readonly int Time;

      public MainCooldownComboBoxItem(string text, int time)
      {
        this.Text = text;
        this.Time = time;
      }

      public override string ToString() => this.Text;
    }

    private class VotingTimeComboBoxItem
    {
      public readonly int VotingTime;
      public readonly string Text;

      public VotingTimeComboBoxItem(string text, int votingTime)
      {
        this.Text = text;
        this.VotingTime = votingTime;
      }

      public override string ToString() => this.Text;
    }

    private class VotingCooldownComboBoxItem
    {
      public readonly int VotingCooldown;
      public readonly string Text;

      public VotingCooldownComboBoxItem(string text, int votingCooldown)
      {
        this.Text = text;
        this.VotingCooldown = votingCooldown;
      }

      public override string ToString() => this.Text;
    }

    private class CategoryTreeNode : TreeNode
    {
      private readonly Category category;

      public CategoryTreeNode(Category _category)
      {
        this.category = _category;
        this.Name = this.Text = this.category.Name;
      }

      public void UpdateCategory()
      {
        bool flag = true;
        int num = 0;
        foreach (TreeNode node in this.Nodes)
        {
          if (node.Checked)
            ++num;
          else
            flag = false;
        }
        this.Checked = flag;
        this.Text = this.Name + " (" + (object) num + "/" + (object) this.Nodes.Count + ")";
      }
    }

    private class EffectTreeNode : TreeNode
    {
      public readonly AbstractEffect Effect;

      public EffectTreeNode(AbstractEffect effect)
      {
        this.Effect = effect;
        this.Name = this.Text = effect.GetDescription();
      }
    }
  }
}
