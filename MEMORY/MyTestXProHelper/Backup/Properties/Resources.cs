// Decompiled with JetBrains decompiler
// Type: MyTestXProHelper.Properties.Resources
// Assembly: MyTestXProHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DE82430E-B2FC-40C1-987E-04A94D7B8D5B
// Assembly location: D:\SCH\MyTestXProHelper\MyTestXProHelper.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MyTestXProHelper.Properties
{
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (MyTestXProHelper.Properties.Resources.resourceMan == null)
          MyTestXProHelper.Properties.Resources.resourceMan = new ResourceManager("MyTestXProHelper.Properties.Resources", typeof (MyTestXProHelper.Properties.Resources).Assembly);
        return MyTestXProHelper.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => MyTestXProHelper.Properties.Resources.resourceCulture;
      set => MyTestXProHelper.Properties.Resources.resourceCulture = value;
    }
  }
}
