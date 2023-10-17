// Decompiled with JetBrains decompiler
// Type: MyTestXProHelper.Settings
// Assembly: MyTestXProHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DE82430E-B2FC-40C1-987E-04A94D7B8D5B
// Assembly location: D:\SCH\MyTestXProHelper\MyTestXProHelper.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MyTestXProHelper
{
  public class Settings : Form
  {
    private IContainer components;
    private GroupBox mtsGroupBox;
    private GroupBox mteGroupBox;
    private TextBox headSignatureTextBox;
    private Label headSignatureLabel;
    private Label tailSignatureLabel;
    private TextBox tailSignatureTextBox;
    private Label patchSignatureLabel;
    private TextBox patchSignatureTextBox;
    private TextBox mtsReplaceTextBox;
    private Label mtsReplaceLabel;
    private Label mtePatchSignatureLabel;
    private TextBox mteReplaceTextBox;
    private TextBox mteOriginalByteTextBox;
    private Label mteReplaceWithLabel;
    private Button okButton;
    private Button cancelButton;
    private Label mteOffsetLabel;
    private TextBox mteOffsetTextBox;
    private Label maskLabel;
    private TextBox maskBox;
    private Label replaceMaskLabel;
    private TextBox replaceMaskTextBox;

    public Settings()
    {
      this.InitializeComponent();
      this.headSignatureTextBox.Text = MyTestXProHelper.Properties.Settings.Default.HeadSignature;
      this.tailSignatureTextBox.Text = MyTestXProHelper.Properties.Settings.Default.TailSignature;
      this.patchSignatureTextBox.Text = MyTestXProHelper.Properties.Settings.Default.PatchSignature;
      this.mtsReplaceTextBox.Text = MyTestXProHelper.Properties.Settings.Default.ReplaceWith;
      this.maskBox.Text = MyTestXProHelper.Properties.Settings.Default.SignatureMask;
      this.replaceMaskTextBox.Text = MyTestXProHelper.Properties.Settings.Default.ReplaceMask;
      this.mteOriginalByteTextBox.Text = MyTestXProHelper.Properties.Settings.Default.mteOriginalByte;
      this.mteReplaceTextBox.Text = MyTestXProHelper.Properties.Settings.Default.mteReplaceWith;
      this.mteOffsetTextBox.Text = MyTestXProHelper.Properties.Settings.Default.mteOffset;
    }

    private void hex_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Control && !e.Alt && !e.Shift && (this.IsActionKey(e.KeyCode) || Regex.IsMatch(((char) e.KeyCode).ToString(), "^[0-9a-fA-F]+$")))
        return;
      e.SuppressKeyPress = true;
    }

    private bool IsActionKey(Keys inKey) => inKey == Keys.Delete || inKey == Keys.Back || inKey == Keys.Tab || inKey == Keys.Return || inKey == Keys.Left || inKey == Keys.Right;

    private void okButton_Click(object sender, EventArgs e)
    {
      MyTestXProHelper.Properties.Settings.Default.HeadSignature = this.headSignatureTextBox.Text;
      MyTestXProHelper.Properties.Settings.Default.TailSignature = this.tailSignatureTextBox.Text;
      MyTestXProHelper.Properties.Settings.Default.PatchSignature = this.patchSignatureTextBox.Text;
      MyTestXProHelper.Properties.Settings.Default.ReplaceWith = this.mtsReplaceTextBox.Text;
      MyTestXProHelper.Properties.Settings.Default.SignatureMask = this.maskBox.Text;
      MyTestXProHelper.Properties.Settings.Default.ReplaceMask = this.replaceMaskTextBox.Text;
      MyTestXProHelper.Properties.Settings.Default.mteOriginalByte = this.mteOriginalByteTextBox.Text;
      MyTestXProHelper.Properties.Settings.Default.mteReplaceWith = this.mteReplaceTextBox.Text;
      MyTestXProHelper.Properties.Settings.Default.mteOffset = this.mteOffsetTextBox.Text;
      this.Close();
    }

    private void cancelButton_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Settings));
      this.mtsGroupBox = new GroupBox();
      this.mtsReplaceLabel = new Label();
      this.mtsReplaceTextBox = new TextBox();
      this.patchSignatureLabel = new Label();
      this.patchSignatureTextBox = new TextBox();
      this.tailSignatureLabel = new Label();
      this.tailSignatureTextBox = new TextBox();
      this.headSignatureTextBox = new TextBox();
      this.headSignatureLabel = new Label();
      this.mteGroupBox = new GroupBox();
      this.mteReplaceTextBox = new TextBox();
      this.mtePatchSignatureLabel = new Label();
      this.mteOriginalByteTextBox = new TextBox();
      this.mteReplaceWithLabel = new Label();
      this.okButton = new Button();
      this.cancelButton = new Button();
      this.mteOffsetTextBox = new TextBox();
      this.mteOffsetLabel = new Label();
      this.maskBox = new TextBox();
      this.maskLabel = new Label();
      this.replaceMaskTextBox = new TextBox();
      this.replaceMaskLabel = new Label();
      this.mtsGroupBox.SuspendLayout();
      this.mteGroupBox.SuspendLayout();
      this.SuspendLayout();
      this.mtsGroupBox.Controls.Add((Control) this.replaceMaskLabel);
      this.mtsGroupBox.Controls.Add((Control) this.replaceMaskTextBox);
      this.mtsGroupBox.Controls.Add((Control) this.maskLabel);
      this.mtsGroupBox.Controls.Add((Control) this.maskBox);
      this.mtsGroupBox.Controls.Add((Control) this.mtsReplaceLabel);
      this.mtsGroupBox.Controls.Add((Control) this.mtsReplaceTextBox);
      this.mtsGroupBox.Controls.Add((Control) this.patchSignatureLabel);
      this.mtsGroupBox.Controls.Add((Control) this.patchSignatureTextBox);
      this.mtsGroupBox.Controls.Add((Control) this.tailSignatureLabel);
      this.mtsGroupBox.Controls.Add((Control) this.tailSignatureTextBox);
      this.mtsGroupBox.Controls.Add((Control) this.headSignatureTextBox);
      this.mtsGroupBox.Controls.Add((Control) this.headSignatureLabel);
      this.mtsGroupBox.Location = new Point(12, 12);
      this.mtsGroupBox.Name = "mtsGroupBox";
      this.mtsGroupBox.Size = new Size(526, 188);
      this.mtsGroupBox.TabIndex = 0;
      this.mtsGroupBox.TabStop = false;
      this.mtsGroupBox.Text = "MyTestStudent";
      this.mtsReplaceLabel.AutoSize = true;
      this.mtsReplaceLabel.Location = new Point(6, 126);
      this.mtsReplaceLabel.Name = "mtsReplaceLabel";
      this.mtsReplaceLabel.Size = new Size(72, 13);
      this.mtsReplaceLabel.TabIndex = 7;
      this.mtsReplaceLabel.Text = "Replace With";
      this.mtsReplaceTextBox.Location = new Point(95, 123);
      this.mtsReplaceTextBox.Name = "mtsReplaceTextBox";
      this.mtsReplaceTextBox.Size = new Size(425, 20);
      this.mtsReplaceTextBox.TabIndex = 6;
      this.mtsReplaceTextBox.KeyDown += new KeyEventHandler(this.hex_KeyDown);
      this.patchSignatureLabel.AutoSize = true;
      this.patchSignatureLabel.Location = new Point(6, 74);
      this.patchSignatureLabel.Name = "patchSignatureLabel";
      this.patchSignatureLabel.Size = new Size(83, 13);
      this.patchSignatureLabel.TabIndex = 5;
      this.patchSignatureLabel.Text = "Patch Signature";
      this.patchSignatureTextBox.Location = new Point(95, 71);
      this.patchSignatureTextBox.Name = "patchSignatureTextBox";
      this.patchSignatureTextBox.Size = new Size(425, 20);
      this.patchSignatureTextBox.TabIndex = 4;
      this.patchSignatureTextBox.KeyDown += new KeyEventHandler(this.hex_KeyDown);
      this.tailSignatureLabel.AutoSize = true;
      this.tailSignatureLabel.Location = new Point(6, 48);
      this.tailSignatureLabel.Name = "tailSignatureLabel";
      this.tailSignatureLabel.Size = new Size(72, 13);
      this.tailSignatureLabel.TabIndex = 3;
      this.tailSignatureLabel.Text = "Tail Signature";
      this.tailSignatureTextBox.Location = new Point(95, 45);
      this.tailSignatureTextBox.Name = "tailSignatureTextBox";
      this.tailSignatureTextBox.Size = new Size(425, 20);
      this.tailSignatureTextBox.TabIndex = 2;
      this.tailSignatureTextBox.KeyDown += new KeyEventHandler(this.hex_KeyDown);
      this.headSignatureTextBox.Location = new Point(95, 19);
      this.headSignatureTextBox.Name = "headSignatureTextBox";
      this.headSignatureTextBox.Size = new Size(425, 20);
      this.headSignatureTextBox.TabIndex = 1;
      this.headSignatureTextBox.KeyDown += new KeyEventHandler(this.hex_KeyDown);
      this.headSignatureLabel.AutoSize = true;
      this.headSignatureLabel.Location = new Point(6, 22);
      this.headSignatureLabel.Name = "headSignatureLabel";
      this.headSignatureLabel.Size = new Size(81, 13);
      this.headSignatureLabel.TabIndex = 0;
      this.headSignatureLabel.Text = "Head Signature";
      this.mteGroupBox.Controls.Add((Control) this.mteOffsetLabel);
      this.mteGroupBox.Controls.Add((Control) this.mteOffsetTextBox);
      this.mteGroupBox.Controls.Add((Control) this.mteReplaceWithLabel);
      this.mteGroupBox.Controls.Add((Control) this.mtePatchSignatureLabel);
      this.mteGroupBox.Controls.Add((Control) this.mteReplaceTextBox);
      this.mteGroupBox.Controls.Add((Control) this.mteOriginalByteTextBox);
      this.mteGroupBox.Location = new Point(12, 206);
      this.mteGroupBox.Name = "mteGroupBox";
      this.mteGroupBox.Size = new Size(526, 101);
      this.mteGroupBox.TabIndex = 1;
      this.mteGroupBox.TabStop = false;
      this.mteGroupBox.Text = "MyTestEditor";
      this.mteReplaceTextBox.Location = new Point(95, 66);
      this.mteReplaceTextBox.MaxLength = 2;
      this.mteReplaceTextBox.Name = "mteReplaceTextBox";
      this.mteReplaceTextBox.Size = new Size(425, 20);
      this.mteReplaceTextBox.TabIndex = 1;
      this.mteReplaceTextBox.KeyDown += new KeyEventHandler(this.hex_KeyDown);
      this.mtePatchSignatureLabel.AutoSize = true;
      this.mtePatchSignatureLabel.Location = new Point(6, 43);
      this.mtePatchSignatureLabel.Name = "mtePatchSignatureLabel";
      this.mtePatchSignatureLabel.Size = new Size(66, 13);
      this.mtePatchSignatureLabel.TabIndex = 2;
      this.mtePatchSignatureLabel.Text = "Original Byte";
      this.mteOriginalByteTextBox.Location = new Point(95, 40);
      this.mteOriginalByteTextBox.MaxLength = 2;
      this.mteOriginalByteTextBox.Name = "mteOriginalByteTextBox";
      this.mteOriginalByteTextBox.Size = new Size(425, 20);
      this.mteOriginalByteTextBox.TabIndex = 0;
      this.mteOriginalByteTextBox.KeyDown += new KeyEventHandler(this.hex_KeyDown);
      this.mteReplaceWithLabel.AutoSize = true;
      this.mteReplaceWithLabel.Location = new Point(6, 69);
      this.mteReplaceWithLabel.Name = "mteReplaceWithLabel";
      this.mteReplaceWithLabel.Size = new Size(72, 13);
      this.mteReplaceWithLabel.TabIndex = 3;
      this.mteReplaceWithLabel.Text = "Replace With";
      this.okButton.Location = new Point(463, 313);
      this.okButton.Name = "okButton";
      this.okButton.Size = new Size(75, 23);
      this.okButton.TabIndex = 2;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new EventHandler(this.okButton_Click);
      this.cancelButton.Location = new Point(382, 313);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(75, 23);
      this.cancelButton.TabIndex = 3;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new EventHandler(this.cancelButton_Click);
      this.mteOffsetTextBox.Location = new Point(95, 14);
      this.mteOffsetTextBox.MaxLength = 8;
      this.mteOffsetTextBox.Name = "mteOffsetTextBox";
      this.mteOffsetTextBox.Size = new Size(425, 20);
      this.mteOffsetTextBox.TabIndex = 4;
      this.mteOffsetTextBox.KeyDown += new KeyEventHandler(this.hex_KeyDown);
      this.mteOffsetLabel.AutoSize = true;
      this.mteOffsetLabel.Location = new Point(6, 17);
      this.mteOffsetLabel.Name = "mteOffsetLabel";
      this.mteOffsetLabel.Size = new Size(35, 13);
      this.mteOffsetLabel.TabIndex = 5;
      this.mteOffsetLabel.Text = "Offset";
      this.maskBox.Location = new Point(95, 97);
      this.maskBox.Name = "maskBox";
      this.maskBox.Size = new Size(425, 20);
      this.maskBox.TabIndex = 8;
      this.maskLabel.AutoSize = true;
      this.maskLabel.Location = new Point(6, 100);
      this.maskLabel.Name = "maskLabel";
      this.maskLabel.Size = new Size(81, 13);
      this.maskLabel.TabIndex = 9;
      this.maskLabel.Text = "Signature Mask";
      this.replaceMaskTextBox.Location = new Point(95, 149);
      this.replaceMaskTextBox.Name = "replaceMaskTextBox";
      this.replaceMaskTextBox.Size = new Size(425, 20);
      this.replaceMaskTextBox.TabIndex = 10;
      this.replaceMaskLabel.AutoSize = true;
      this.replaceMaskLabel.Location = new Point(6, 152);
      this.replaceMaskLabel.Name = "replaceMaskLabel";
      this.replaceMaskLabel.Size = new Size(76, 13);
      this.replaceMaskLabel.TabIndex = 11;
      this.replaceMaskLabel.Text = "Replace Mask";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(550, 348);
      this.Controls.Add((Control) this.cancelButton);
      this.Controls.Add((Control) this.okButton);
      this.Controls.Add((Control) this.mteGroupBox);
      this.Controls.Add((Control) this.mtsGroupBox);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (Settings);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (Settings);
      this.mtsGroupBox.ResumeLayout(false);
      this.mtsGroupBox.PerformLayout();
      this.mteGroupBox.ResumeLayout(false);
      this.mteGroupBox.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
