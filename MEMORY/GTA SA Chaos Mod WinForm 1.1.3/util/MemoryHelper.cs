using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace GTA_SA_Chaos.util
{
  public static class MemoryHelper
  {
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      [MarshalAs(UnmanagedType.AsAny), Out] object lpBuffer,
      int dwSize,
      out IntPtr lpNumberOfBytesRead);

    public static bool Read<T>(IntPtr lpBaseAddress, out T value) where T : struct
    {
      if (ProcessHooker.HasExited())
      {
        value = default (T);
        return false;
      }
      T[] objArray = new T[Marshal.SizeOf<T>()];
      MemoryHelper.ReadProcessMemory(ProcessHooker.GetHandle(), lpBaseAddress, (object) objArray, Marshal.SizeOf<T>(), out IntPtr _);
      value = ((IEnumerable<T>) objArray).FirstOrDefault<T>();
      return true;
    }

    public static bool Read<T>(IntPtr lpBaseAddress, out T value, List<int> offsets) where T : struct
    {
      IntPtr pointer = lpBaseAddress;
      int offset1 = offsets.Last<int>();
      offsets.RemoveAt(offsets.Count - 1);
      foreach (int offset2 in offsets)
      {
        if (!MemoryHelper.Read<IntPtr>(IntPtr.Add(pointer, offset2), out pointer))
        {
          value = default (T);
          return false;
        }
      }
      return MemoryHelper.Read<T>(IntPtr.Add(pointer, offset1), out value);
    }
  }
}
