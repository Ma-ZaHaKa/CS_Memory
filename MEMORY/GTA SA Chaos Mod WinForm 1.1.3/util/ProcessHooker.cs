using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;

namespace GTA_SA_Chaos.util
{
  public static class ProcessHooker
  {
    private static Process Process;

    public static void HookProcess()
    {
      ProcessHooker.CloseProcess();
      ProcessHooker.Process = ((IEnumerable<Process>) Process.GetProcessesByName("gta_sa")).FirstOrDefault<Process>();
      if (ProcessHooker.Process == null)
        ProcessHooker.Process = ((IEnumerable<Process>) Process.GetProcessesByName("gta-sa")).FirstOrDefault<Process>();
      if (ProcessHooker.Process == null)
        return;
      ProcessHooker.Process.EnableRaisingEvents = true;
    }

    public static IntPtr GetHandle() => ProcessHooker.Process != null ? ProcessHooker.Process.Handle : IntPtr.Zero;

    public static void SendPipeMessage(string func) => new Thread((ThreadStart) (() =>
    {
      Thread.CurrentThread.IsBackground = true;
      using (NamedPipeClientStream pipeClientStream = new NamedPipeClientStream("GTASAChaosPipe"))
      {
        try
        {
          if (!pipeClientStream.IsConnected)
            pipeClientStream.Connect(1000);
          using (StreamWriter streamWriter = new StreamWriter((Stream) pipeClientStream))
          {
            if (!streamWriter.AutoFlush)
              streamWriter.AutoFlush = true;
            streamWriter.WriteLine(func);
          }
        }
        catch
        {
        }
      }
    })).Start();

    public static void SendEffectToGame(
      string type,
      string function,
      int duration = -1,
      string description = "N/A",
      string voter = "N/A",
      int rapidfire = 0)
    {
      ProcessHooker.SendPipeMessage(string.Format("{0}:{1}:{2}:{3}:{4}:{5}", (object) type, (object) function, (object) duration, (object) description, (object) voter, (object) rapidfire));
    }

    public static void AttachExitedMethod(EventHandler method)
    {
      if (ProcessHooker.Process == null)
        return;
      ProcessHooker.Process.Exited += method;
    }

    public static bool HasExited() => ProcessHooker.Process == null || ProcessHooker.Process.HasExited;

    public static void CloseProcess()
    {
      try
      {
        ProcessHooker.Process = (Process) null;
      }
      catch
      {
      }
    }
  }
}
