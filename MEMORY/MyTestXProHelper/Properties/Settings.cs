using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MyTestXProHelper.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
  [CompilerGenerated]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default => Settings.defaultInstance;

    [DebuggerNonUserCode]
    [DefaultSettingValue("4D7954657374580000")]
    [UserScopedSetting]
    public string HeadSignature
    {
      get => (string) this[nameof (HeadSignature)];
      set => this[nameof (HeadSignature)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0000000000000000")]
    public string TailSignature
    {
      get => (string) this[nameof (TailSignature)];
      set => this[nameof (TailSignature)] = (object) value;
    }

    [DefaultSettingValue("5B595DC38BC05356E8000000008BDA8BF08BC6E800000000")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public string PatchSignature
    {
      get => (string) this[nameof (PatchSignature)];
      set => this[nameof (PatchSignature)] = (object) value;
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("5B595DC38BC0C356E8000000008BDA8BF08BC6E800000000")]
    [UserScopedSetting]
    public string ReplaceWith
    {
      get => (string) this[nameof (ReplaceWith)];
      set => this[nameof (ReplaceWith)] = (object) value;
    }

    [UserScopedSetting]
    [DefaultSettingValue("30")]
    [DebuggerNonUserCode]
    public string mteOriginalByte
    {
      get => (string) this[nameof (mteOriginalByte)];
      set => this[nameof (mteOriginalByte)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("34")]
    public string mteReplaceWith
    {
      get => (string) this[nameof (mteReplaceWith)];
      set => this[nameof (mteReplaceWith)] = (object) value;
    }

    [DefaultSettingValue("3032e4")]
    [UserScopedSetting]
    [DebuggerNonUserCode]
    public string mteOffset
    {
      get => (string) this[nameof (mteOffset)];
      set => this[nameof (mteOffset)] = (object) value;
    }

    [DefaultSettingValue("FFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFF00000000")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public string SignatureMask
    {
      get => (string) this[nameof (SignatureMask)];
      set => this[nameof (SignatureMask)] = (object) value;
    }

    [DefaultSettingValue("FFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFF00000000")]
    [DebuggerNonUserCode]
    [UserScopedSetting]
    public string ReplaceMask
    {
      get => (string) this[nameof (ReplaceMask)];
      set => this[nameof (ReplaceMask)] = (object) value;
    }
  }
}
