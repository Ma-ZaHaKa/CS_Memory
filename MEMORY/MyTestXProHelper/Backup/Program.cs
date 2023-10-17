// Decompiled with JetBrains decompiler
// Type: MyTestXProHelper.Program
// Assembly: MyTestXProHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DE82430E-B2FC-40C1-987E-04A94D7B8D5B
// Assembly location: D:\SCH\MyTestXProHelper\MyTestXProHelper.exe

using System;
using System.Windows.Forms;

namespace MyTestXProHelper
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MyTestXProHelper.MyTestXProHelper());
    }
  }
}
