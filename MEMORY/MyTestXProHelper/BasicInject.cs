using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MyTestXProHelper
{
  public class BasicInject
  {
    private const int PROCESS_CREATE_THREAD = 2;
    private const int PROCESS_QUERY_INFORMATION = 1024;
    private const int PROCESS_VM_OPERATION = 8;
    private const int PROCESS_VM_WRITE = 32;
    private const int PROCESS_VM_READ = 16;
    private const uint MEM_COMMIT = 4096;
    private const uint MEM_RESERVE = 8192;
    private const uint PAGE_READWRITE = 4;
    private const uint MiniDumpWithFullMemory = 2;

    [DllImport("kernel32.dll")]
    public static extern uint GetLastError();

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(
      int dwDesiredAccess,
      bool bInheritHandle,
      int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr VirtualAllocEx(
      IntPtr hProcess,
      IntPtr lpAddress,
      uint dwSize,
      uint flAllocationType,
      uint flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern int VirtualQueryEx(
      IntPtr hProcess,
      IntPtr lpAddress,
      out BasicInject.MEMORY_BASIC_INFORMATION lpBuffer,
      uint dwLength);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool WriteProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      byte[] lpBuffer,
      uint nSize,
      out UIntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      byte[] lpBuffer,
      uint dwSize,
      out UIntPtr lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    private static extern IntPtr CreateRemoteThread(
      IntPtr hProcess,
      IntPtr lpThreadAttributes,
      uint dwStackSize,
      IntPtr lpStartAddress,
      IntPtr lpParameter,
      uint dwCreationFlags,
      IntPtr lpThreadId);

    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern int memcmp(byte[] b1, byte[] b2, long count);

    [DllImport("kernel32.dll")]
    private static extern void GetSystemInfo(out BasicInject.SYSTEM_INFO lpSystemInfo);

    [DllImport("dbghelp.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern bool MiniDumpWriteDump(
      IntPtr hProcess,
      int processId,
      SafeHandle hFile,
      uint dumpType,
      IntPtr expParam,
      IntPtr userStreamParam,
      IntPtr callbackParam);

    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentThreadId();

    [DllImport("kernel32.dll")]
    public static extern bool CreateProcess(
      string lpApplicationName,
      string lpCommandLine,
      IntPtr lpProcessAttributes,
      IntPtr lpThreadAttributes,
      bool bInheritHandles,
      BasicInject.ProcessCreationFlags dwCreationFlags,
      IntPtr lpEnvironment,
      string lpCurrentDirectory,
      ref BasicInject.STARTUPINFO lpStartupInfo,
      out BasicInject.PROCESS_INFORMATION lpProcessInformation);

    [DllImport("kernel32.dll")]
    public static extern uint ResumeThread(IntPtr hThread);

    [DllImport("kernel32.dll")]
    public static extern uint SuspendThread(IntPtr hThread);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

    public static SortedDictionary<int, string> GetProcessList()
    {
      SortedDictionary<int, string> sortedDictionary = new SortedDictionary<int, string>();
      foreach (Process process in Process.GetProcesses())
        sortedDictionary.Add(process.Id, process.ProcessName);
      return sortedDictionary;
    }

    public static int CreateProcess(string ExecutablePath)
    {
      BasicInject.STARTUPINFO lpStartupInfo = new BasicInject.STARTUPINFO();
      BasicInject.PROCESS_INFORMATION lpProcessInformation = new BasicInject.PROCESS_INFORMATION();
      bool process = BasicInject.CreateProcess(ExecutablePath, (string) null, IntPtr.Zero, IntPtr.Zero, false, BasicInject.ProcessCreationFlags.ZERO_FLAG, IntPtr.Zero, (string) null, ref lpStartupInfo, out lpProcessInformation);
      BasicInject.CloseHandle(lpProcessInformation.hThread);
      BasicInject.CloseHandle(lpProcessInformation.hProcess);
      return !process ? -1 : lpProcessInformation.dwProcessId;
    }

    public static bool PatchProcess(
      int ProcessId,
      IntPtr Offset,
      byte[] OrigOpCode,
      byte[] OpCode)
    {
      bool flag = false;
      Process processById = Process.GetProcessById(ProcessId);
      IntPtr num1 = BasicInject.OpenProcess(1082, false, ProcessId);
      if (num1 == IntPtr.Zero)
        return flag;
      IntPtr lpBaseAddress = new IntPtr(processById.MainModule.BaseAddress.ToInt64() + Offset.ToInt64());
      byte[] numArray = new byte[1];
      UIntPtr num2;
      if (BasicInject.ReadProcessMemory(num1, lpBaseAddress, numArray, (uint) Marshal.SizeOf(typeof (char)), out num2) && BasicInject.ByteArrayCompare(numArray, OrigOpCode) && BasicInject.WriteProcessMemory(num1, lpBaseAddress, OpCode, (uint) (OpCode.Length * Marshal.SizeOf(typeof (char))), out num2))
        flag = (long) num2.ToUInt64() == (long) (uint) (OpCode.Length * Marshal.SizeOf(typeof (char)));
      BasicInject.CloseHandle(num1);
      return flag;
    }

    public static bool DumpProcessMemory(
      SafeHandle FileHandle,
      IntPtr ProcessHandle,
      int ProcessId)
    {
      int currentThreadId = (int) BasicInject.GetCurrentThreadId();
      IntPtr zero = IntPtr.Zero;
      return BasicInject.MiniDumpWriteDump(ProcessHandle, ProcessId, FileHandle, 2U, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
    }

    public static byte[] ExtractFromProcessMemory(
      int ProcessId,
      byte[] HeadSignature,
      byte[] TailSignature)
    {
      byte[] numArray1 = new byte[0];
      IntPtr num1 = BasicInject.OpenProcess(1040, false, Process.GetProcessById(ProcessId).Id);
      if (num1 == IntPtr.Zero)
        return numArray1;
      string path = Path.GetTempPath() + Guid.NewGuid().ToString() + ".dmp";
      FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Write);
      if (!BasicInject.DumpProcessMemory((SafeHandle) fileStream.SafeFileHandle, num1, ProcessId))
        return numArray1;
      fileStream.Flush();
      fileStream.Close();
      byte[] numArray2 = File.ReadAllBytes(path);
      long num2 = -1;
      long num3 = -1;
      for (long index = 0; index < (long) numArray2.Length; ++index)
      {
        bool flag1 = num2 == -1L;
        bool flag2 = num3 == -1L;
        if (flag1 || flag2)
        {
          if (flag1 && (int) numArray2[index] == (int) HeadSignature[0] && (int) numArray2[index + 1L] == (int) HeadSignature[1])
          {
            byte[] b1 = new byte[HeadSignature.Length];
            Buffer.BlockCopy((Array) numArray2, (int) index, (Array) b1, 0, HeadSignature.Length);
            if (BasicInject.ByteArrayCompare(b1, HeadSignature))
              num2 = index;
          }
          else if (!flag1 && flag2 && (int) numArray2[index] == (int) TailSignature[0] && (int) numArray2[index + 1L] == (int) TailSignature[1])
          {
            byte[] b1 = new byte[TailSignature.Length];
            Buffer.BlockCopy((Array) numArray2, (int) index, (Array) b1, 0, TailSignature.Length);
            if (BasicInject.ByteArrayCompare(b1, TailSignature))
              num3 = index;
          }
          if (num3 > 0L && num2 > 0L && num3 > num2)
          {
            long length = num3 - num2;
            numArray1 = new byte[length];
            Buffer.BlockCopy((Array) numArray2, (int) num2, (Array) numArray1, 0, (int) length);
            break;
          }
        }
      }
      File.Delete(path);
      BasicInject.CloseHandle(num1);
      return numArray1;
    }

    public static bool FuzzyByteArrayCompare(byte[] b1, long l1, byte[] b2, long l2) => BasicInject.memcmp(b1, b2, Math.Min(l1, l2)) == 0;

    public static bool ByteArrayCompare(byte[] b1, byte[] b2) => b1.Length == b2.Length && BasicInject.memcmp(b1, b2, (long) b1.Length) == 0;

    public struct SYSTEM_INFO
    {
      public ushort processorArchitecture;
      private ushort reserved;
      public uint pageSize;
      public IntPtr minimumApplicationAddress;
      public IntPtr maximumApplicationAddress;
      public IntPtr activeProcessorMask;
      public uint numberOfProcessors;
      public uint processorType;
      public uint allocationGranularity;
      public ushort processorLevel;
      public ushort processorRevision;
    }

    public struct MEMORY_BASIC_INFORMATION
    {
      public int BaseAddress;
      public int AllocationBase;
      public int AllocationProtect;
      public int RegionSize;
      public int State;
      public int Protect;
      public int lType;
    }

    public struct STARTUPINFO
    {
      public uint cb;
      public string lpReserved;
      public string lpDesktop;
      public string lpTitle;
      public uint dwX;
      public uint dwY;
      public uint dwXSize;
      public uint dwYSize;
      public uint dwXCountChars;
      public uint dwYCountChars;
      public uint dwFillAttribute;
      public uint dwFlags;
      public short wShowWindow;
      public short cbReserved2;
      public IntPtr lpReserved2;
      public IntPtr hStdInput;
      public IntPtr hStdOutput;
      public IntPtr hStdError;
    }

    public struct PROCESS_INFORMATION
    {
      public IntPtr hProcess;
      public IntPtr hThread;
      public int dwProcessId;
      public int dwThreadId;
    }

    [Flags]
    public enum ProcessCreationFlags : uint
    {
      ZERO_FLAG = 0,
      CREATE_BREAKAWAY_FROM_JOB = 16777216, // 0x01000000
      CREATE_DEFAULT_ERROR_MODE = 67108864, // 0x04000000
      CREATE_NEW_CONSOLE = 16, // 0x00000010
      CREATE_NEW_PROCESS_GROUP = 512, // 0x00000200
      CREATE_NO_WINDOW = 134217728, // 0x08000000
      CREATE_PROTECTED_PROCESS = 262144, // 0x00040000
      CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 33554432, // 0x02000000
      CREATE_SEPARATE_WOW_VDM = 4096, // 0x00001000
      CREATE_SHARED_WOW_VDM = CREATE_SEPARATE_WOW_VDM, // 0x00001000
      CREATE_SUSPENDED = 4,
      CREATE_UNICODE_ENVIRONMENT = 1024, // 0x00000400
      DEBUG_ONLY_THIS_PROCESS = 2,
      DEBUG_PROCESS = 1,
      DETACHED_PROCESS = 8,
      EXTENDED_STARTUPINFO_PRESENT = 524288, // 0x00080000
      INHERIT_PARENT_AFFINITY = 65536, // 0x00010000
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct MiniDumpExceptionInformation
    {
      public uint ThreadId;
      public IntPtr ExceptionPointers;
      [MarshalAs(UnmanagedType.Bool)]
      public bool ClientPointers;
    }
  }
}
