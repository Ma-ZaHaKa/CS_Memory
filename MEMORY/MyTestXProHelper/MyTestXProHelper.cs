using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MyTestXProHelper
{
    public partial class MyTestXProHelper : Form
    {

        private SortedDictionary<int, string> ProcessList;
        private IContainer components;
        private GroupBox processBox;
        private Button refreshButton;
        private ListBox processListBox;
        private RadioButton radioIsEditor;
        private RadioButton radioIsStudent;
        private GroupBox mtsGroupBox;
        private Label dumpPath;
        private Button dumpButton;
        private Button browseButton;
        private TextBox pathTextBox;
        private SaveFileDialog saveFileDialog;
        private GroupBox modeBox;
        private GroupBox mteGroupBox;
        private LinkLabel linkLabel;
        private Button patchButton;
        private Button exeBrowseButton;
        private Label exeLabel;
        private TextBox exeTextBox;
        private OpenFileDialog openFileDialog;
        private Button settingsButton;
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }



        public MyTestXProHelper() => this.InitializeComponent();

        private void refreshButton_Click(object sender, EventArgs e)
        {
            this.processListBox.Items.Clear();
            this.mteGroupBox.Enabled = false;
            this.PopulateProcessListBox();
        }

        private void MyTestXProHelper_Load(object sender, EventArgs e) => this.PopulateProcessListBox();

        private void PopulateProcessListBox()
        {
            this.ProcessList = BasicInject.GetProcessList();
            foreach (KeyValuePair<int, string> process in this.ProcessList)
                this.processListBox.Items.Add((object)(process.Key.ToString() + " - " + process.Value));
        }

        private void passBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.pathTextBox.Text = this.saveFileDialog.FileName;
        }

        private void exeBrowseButton_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.exeTextBox.Text = this.openFileDialog.FileName;
        }

        private void processListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioIsEditor.Checked)
            {
                this.mteGroupBox.Enabled = true;
                this.mtsGroupBox.Enabled = false;
            }
            else
            {
                if (!this.radioIsStudent.Checked)
                    return;
                this.mteGroupBox.Enabled = false;
                this.mtsGroupBox.Enabled = true;
            }
        }

        private void dumpButton_Click(object sender, EventArgs e)
        {
            string headSignature = Properties.Settings.Default.HeadSignature;
            string tailSignature = Properties.Settings.Default.TailSignature;
            string patchSignature = Properties.Settings.Default.PatchSignature;
            string signatureMask = Properties.Settings.Default.SignatureMask;
            string replaceWith = Properties.Settings.Default.ReplaceWith;
            string replaceMask = Properties.Settings.Default.ReplaceMask;
            if (headSignature.Length == 0 || tailSignature.Length == 0 || patchSignature.Length == 0 || replaceWith.Length == 0 || signatureMask.Length == 0 || replaceMask.Length == 0)
            {
                int num1 = (int)MessageBox.Show("Erroneous MyTestStudent parameters, please check settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (replaceWith.Length != replaceMask.Length || patchSignature.Length != signatureMask.Length)
            {
                int num2 = (int)MessageBox.Show("Erroneous mask or data size", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.pathTextBox.TextLength == 0)
            {
                int num3 = (int)MessageBox.Show("Please, specify test save path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                string text1 = this.pathTextBox.Text;
                string text2 = this.exeTextBox.Text;
                int selectedIndex = this.processListBox.SelectedIndex;
                if (text2.Length == 0 && selectedIndex == -1)
                {
                    int num4 = (int)MessageBox.Show("Please, specify either test to launch or running process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    int ProcessId = -1;
                    if (text2.Length > 0)
                    {
                        string str = Path.GetTempPath() + Guid.NewGuid().ToString() + ".exe";
                        File.Copy(text2, str);
                        byte[] byteArray1 = MyTestXProHelper.StringToByteArray(patchSignature);
                        byte[] byteArray2 = MyTestXProHelper.StringToByteArray(signatureMask);
                        byte[] byteArray3 = MyTestXProHelper.StringToByteArray(replaceWith);
                        byte[] byteArray4 = MyTestXProHelper.StringToByteArray(replaceMask);
                        byte[] OriginalData = File.ReadAllBytes(str);
                        if (OriginalData.Length == 0)
                        {
                            int num5 = (int)MessageBox.Show("Failed to read executable data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        int subArrayMasked = MyTestXProHelper.FindSubArrayMasked(OriginalData, byteArray1, byteArray2);
                        if (subArrayMasked == -1)
                        {
                            int num6 = (int)MessageBox.Show("Failed to find signature in .exe-file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        MyTestXProHelper.ReplaceArrayMasked(ref OriginalData, subArrayMasked, byteArray3, byteArray4);
                        File.WriteAllBytes(str, OriginalData);
                        ProcessId = BasicInject.CreateProcess(str);
                        if (ProcessId == -1)
                        {
                            int num7 = (int)MessageBox.Show("Failed to launch specified process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        Thread.Sleep(500);
                    }
                    else if (selectedIndex > -1)
                    {
                        int num8 = 0;
                        foreach (KeyValuePair<int, string> process in this.ProcessList)
                        {
                            if (num8++ == selectedIndex)
                                ProcessId = process.Key;
                        }
                        if (num8 == -1)
                        {
                            int num9 = (int)MessageBox.Show("No such element in the process list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                    byte[] byteArray5 = MyTestXProHelper.StringToByteArray(headSignature);
                    byte[] byteArray6 = MyTestXProHelper.StringToByteArray(tailSignature);
                    byte[] fromProcessMemory = BasicInject.ExtractFromProcessMemory(ProcessId, byteArray5, byteArray6);
                    if (fromProcessMemory.Length == 0)
                    {
                        int num10 = (int)MessageBox.Show("Can't find test data in memory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        File.WriteAllBytes(text1, fromProcessMemory);
                        int num11 = (int)MessageBox.Show("OK!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void radioIsEditor_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioIsEditor.Checked && this.processListBox.SelectedIndex != -1)
            {
                this.mteGroupBox.Enabled = true;
                this.mtsGroupBox.Enabled = false;
            }
            else
            {
                if (!this.radioIsEditor.Checked || this.processListBox.SelectedIndex != -1)
                    return;
                this.radioIsStudent.Checked = true;
                this.radioIsEditor.Checked = false;
                int num = (int)MessageBox.Show("Please, select MyTestEditor process first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void radioIsStudent_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.radioIsStudent.Checked || this.processListBox.SelectedIndex == -1)
                return;
            this.mteGroupBox.Enabled = false;
            this.mtsGroupBox.Enabled = true;
        }

        private void patchButton_Click(object sender, EventArgs e)
        {
            string mteOriginalByte = Properties.Settings.Default.mteOriginalByte;
            string mteReplaceWith = Properties.Settings.Default.mteReplaceWith;
            string mteOffset = Properties.Settings.Default.mteOffset;
            if (mteOriginalByte.Length == 0 || mteReplaceWith.Length == 0 || mteOffset.Length == 0)
            {
                int num1 = (int)MessageBox.Show("Erroneous MyTestEditor parameters, please check settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                IntPtr Offset = new IntPtr(int.Parse(mteOffset, NumberStyles.AllowHexSpecifier));
                byte[] byteArray1 = MyTestXProHelper.StringToByteArray(mteOriginalByte);
                byte[] byteArray2 = MyTestXProHelper.StringToByteArray(mteReplaceWith);
                int selectedIndex = this.processListBox.SelectedIndex;
                int num2 = 0;
                int ProcessId = -1;
                foreach (KeyValuePair<int, string> process in this.ProcessList)
                {
                    if (num2++ == selectedIndex)
                        ProcessId = process.Key;
                }
                if (ProcessId == -1)
                {
                    int num3 = (int)MessageBox.Show("No such element in the process list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else if (BasicInject.PatchProcess(ProcessId, Offset, byteArray1, byteArray2))
                {
                    int num4 = (int)MessageBox.Show("OK!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    int num5 = (int)MessageBox.Show("Failed to patch process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start("https://kaimi.ru/");

        private void settingsButton_Click(object sender, EventArgs e)
        {
            int num = (int)new Settings().ShowDialog();
        }

        public static byte[] StringToByteArray(string hex)
        {
            byte[] numArray = hex.Length % 2 != 1 ? new byte[hex.Length >> 1] : throw new Exception("The binary key cannot have an odd number of digits");
            for (int index = 0; index < hex.Length >> 1; ++index)
                numArray[index] = (byte)((MyTestXProHelper.GetHexVal(hex[index << 1]) << 4) + MyTestXProHelper.GetHexVal(hex[(index << 1) + 1]));
            return numArray;
        }

        public static int GetHexVal(char hex)
        {
            int num = (int)hex;
            return num - (num < 58 ? 48 : (num < 97 ? 55 : 87));
        }

        public static int FindSubArrayMasked(byte[] Hay, byte[] Needle, byte[] Mask)
        {
            int num = -1;
            for (int index1 = 0; index1 < Hay.Length; ++index1)
            {
                bool flag = false;
                for (int index2 = 0; index2 < Needle.Length && index1 + index2 < Hay.Length && (Mask[index2] != byte.MaxValue || (int)Hay[index1 + index2] == (int)Needle[index2]); ++index2)
                    flag = index2 == Needle.Length - 1;
                if (flag)
                {
                    num = index1;
                    break;
                }
            }
            return num;
        }

        public static void ReplaceArrayMasked(
          ref byte[] OriginalData,
          int OriginalDataOffset,
          byte[] ReplaceWithData,
          byte[] ReplaceMask)
        {
            int num = Math.Min(OriginalData.Length - OriginalDataOffset, ReplaceWithData.Length);
            for (int index = 0; index < num; ++index)
            {
                if (ReplaceMask[index] == byte.MaxValue)
                    OriginalData[OriginalDataOffset + index] = ReplaceWithData[index];
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MyTestXProHelper));
            this.processBox = new GroupBox();
            this.refreshButton = new Button();
            this.processListBox = new ListBox();
            this.radioIsEditor = new RadioButton();
            this.radioIsStudent = new RadioButton();
            this.mtsGroupBox = new GroupBox();
            this.exeBrowseButton = new Button();
            this.exeLabel = new Label();
            this.exeTextBox = new TextBox();
            this.dumpButton = new Button();
            this.pathTextBox = new TextBox();
            this.dumpPath = new Label();
            this.browseButton = new Button();
            this.saveFileDialog = new SaveFileDialog();
            this.modeBox = new GroupBox();
            this.mteGroupBox = new GroupBox();
            this.patchButton = new Button();
            this.linkLabel = new LinkLabel();
            this.openFileDialog = new OpenFileDialog();
            this.settingsButton = new Button();
            this.processBox.SuspendLayout();
            this.mtsGroupBox.SuspendLayout();
            this.modeBox.SuspendLayout();
            this.mteGroupBox.SuspendLayout();
            this.SuspendLayout();
            this.processBox.Controls.Add((Control)this.refreshButton);
            this.processBox.Controls.Add((Control)this.processListBox);
            this.processBox.Location = new Point(12, 12);
            this.processBox.Name = "processBox";
            this.processBox.Size = new Size(227, 291);
            this.processBox.TabIndex = 0;
            this.processBox.TabStop = false;
            this.processBox.Text = "Pick Process";
            this.refreshButton.Location = new Point(146, 260);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new Size(75, 23);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new EventHandler(this.refreshButton_Click);
            this.processListBox.FormattingEnabled = true;
            this.processListBox.Location = new Point(6, 19);
            this.processListBox.Name = "processListBox";
            this.processListBox.Size = new Size(215, 238);
            this.processListBox.TabIndex = 0;
            this.processListBox.SelectedIndexChanged += new EventHandler(this.processListBox_SelectedIndexChanged);
            this.radioIsEditor.AutoSize = true;
            this.radioIsEditor.Location = new Point(109, 19);
            this.radioIsEditor.Name = "radioIsEditor";
            this.radioIsEditor.Size = new Size(87, 17);
            this.radioIsEditor.TabIndex = 1;
            this.radioIsEditor.Text = "MyTestEditor";
            this.radioIsEditor.UseVisualStyleBackColor = true;
            this.radioIsEditor.CheckedChanged += new EventHandler(this.radioIsEditor_CheckedChanged);
            this.radioIsStudent.AutoSize = true;
            this.radioIsStudent.Checked = true;
            this.radioIsStudent.Location = new Point(6, 19);
            this.radioIsStudent.Name = "radioIsStudent";
            this.radioIsStudent.Size = new Size(97, 17);
            this.radioIsStudent.TabIndex = 2;
            this.radioIsStudent.TabStop = true;
            this.radioIsStudent.Text = "MyTestStudent";
            this.radioIsStudent.UseVisualStyleBackColor = true;
            this.radioIsStudent.CheckedChanged += new EventHandler(this.radioIsStudent_CheckedChanged);
            this.mtsGroupBox.Controls.Add((Control)this.exeBrowseButton);
            this.mtsGroupBox.Controls.Add((Control)this.exeLabel);
            this.mtsGroupBox.Controls.Add((Control)this.exeTextBox);
            this.mtsGroupBox.Controls.Add((Control)this.dumpButton);
            this.mtsGroupBox.Controls.Add((Control)this.pathTextBox);
            this.mtsGroupBox.Controls.Add((Control)this.dumpPath);
            this.mtsGroupBox.Controls.Add((Control)this.browseButton);
            this.mtsGroupBox.Location = new Point(245, 67);
            this.mtsGroupBox.Name = "mtsGroupBox";
            this.mtsGroupBox.Size = new Size(371, 106);
            this.mtsGroupBox.TabIndex = 3;
            this.mtsGroupBox.TabStop = false;
            this.mtsGroupBox.Text = "MyTestStudent";
            this.exeBrowseButton.Location = new Point(337, 17);
            this.exeBrowseButton.Name = "exeBrowseButton";
            this.exeBrowseButton.Size = new Size(28, 23);
            this.exeBrowseButton.TabIndex = 15;
            this.exeBrowseButton.Text = "...";
            this.exeBrowseButton.UseVisualStyleBackColor = true;
            this.exeBrowseButton.Click += new EventHandler(this.exeBrowseButton_Click);
            this.exeLabel.AutoSize = true;
            this.exeLabel.Location = new Point(7, 22);
            this.exeLabel.Name = "exeLabel";
            this.exeLabel.Size = new Size(53, 13);
            this.exeLabel.TabIndex = 14;
            this.exeLabel.Text = "EXE Path";
            this.exeTextBox.Location = new Point(71, 19);
            this.exeTextBox.Name = "exeTextBox";
            this.exeTextBox.Size = new Size(260, 20);
            this.exeTextBox.TabIndex = 13;
            this.dumpButton.Location = new Point(290, 72);
            this.dumpButton.Name = "dumpButton";
            this.dumpButton.Size = new Size(75, 23);
            this.dumpButton.TabIndex = 7;
            this.dumpButton.Text = "Dump Test";
            this.dumpButton.UseVisualStyleBackColor = true;
            this.dumpButton.Click += new EventHandler(this.dumpButton_Click);
            this.pathTextBox.Location = new Point(71, 45);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new Size(260, 20);
            this.pathTextBox.TabIndex = 5;
            this.dumpPath.AutoSize = true;
            this.dumpPath.Location = new Point(7, 48);
            this.dumpPath.Name = "dumpPath";
            this.dumpPath.Size = new Size(59, 13);
            this.dumpPath.TabIndex = 4;
            this.dumpPath.Text = "Dump path";
            this.browseButton.Location = new Point(337, 43);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new Size(28, 23);
            this.browseButton.TabIndex = 6;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new EventHandler(this.browseButton_Click);
            this.saveFileDialog.DefaultExt = "mtx";
            this.saveFileDialog.Filter = "MTX files (*.mtx)|*.mtx|All files (*.*)|*.*";
            this.modeBox.Controls.Add((Control)this.settingsButton);
            this.modeBox.Controls.Add((Control)this.radioIsStudent);
            this.modeBox.Controls.Add((Control)this.radioIsEditor);
            this.modeBox.Location = new Point(245, 12);
            this.modeBox.Name = "modeBox";
            this.modeBox.Size = new Size(371, 49);
            this.modeBox.TabIndex = 4;
            this.modeBox.TabStop = false;
            this.modeBox.Text = "Mode";
            this.mteGroupBox.Controls.Add((Control)this.patchButton);
            this.mteGroupBox.Enabled = false;
            this.mteGroupBox.Location = new Point(245, 179);
            this.mteGroupBox.Name = "mteGroupBox";
            this.mteGroupBox.Size = new Size(371, 57);
            this.mteGroupBox.TabIndex = 5;
            this.mteGroupBox.TabStop = false;
            this.mteGroupBox.Text = "MyTestEditor";
            this.patchButton.Location = new Point(290, 19);
            this.patchButton.Name = "patchButton";
            this.patchButton.Size = new Size(75, 23);
            this.patchButton.TabIndex = 7;
            this.patchButton.Text = "Patch";
            this.patchButton.UseVisualStyleBackColor = true;
            this.patchButton.Click += new EventHandler(this.patchButton_Click);
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new Point(569, 290);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new Size(47, 13);
            this.linkLabel.TabIndex = 8;
            ((Label)this.linkLabel).TabStop = true;
            this.linkLabel.Text = "(c) Kaimi";
            this.linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            this.openFileDialog.DefaultExt = "exe";
            this.openFileDialog.Filter = "EXE files (*.exe)|*.exe|All files (*.*)|*.*";
            this.settingsButton.Location = new Point(290, 16);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new Size(75, 23);
            this.settingsButton.TabIndex = 21;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new EventHandler(this.settingsButton_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(628, 312);
            this.Controls.Add((Control)this.linkLabel);
            this.Controls.Add((Control)this.mteGroupBox);
            this.Controls.Add((Control)this.modeBox);
            this.Controls.Add((Control)this.mtsGroupBox);
            this.Controls.Add((Control)this.processBox);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            //this.Name = nameof (MyTestXProHelper);
            this.StartPosition = FormStartPosition.CenterScreen;
            //this.Text = nameof (MyTestXProHelper);
            this.Load += new EventHandler(this.MyTestXProHelper_Load);
            this.processBox.ResumeLayout(false);
            this.mtsGroupBox.ResumeLayout(false);
            this.mtsGroupBox.PerformLayout();
            this.modeBox.ResumeLayout(false);
            this.modeBox.PerformLayout();
            this.mteGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

    }
}
