using System;
using System.Runtime.InteropServices;

namespace Judge.Interop
{
    public static class Pinvoke
    {
        public const uint WAIT_TIMEOUT = 0x00000102;
        public const uint STARTF_USESTDHANDLES = 0x00000100;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateJobObject([In] ref SecurityAttributes lpJobAttributes, string lpName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateJobObject(IntPtr ptr, string lpName);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool InitializeSecurityDescriptor(IntPtr pSecurityDescriptor, uint dwRevision);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool QueryInformationJobObject(IntPtr hJob, JobObjectInfoClass JobObjectInformationClass, out JobObjectExtendedLimitInformation lpJobObjectInfo, uint cbJobObjectInfoLength, out IntPtr lpReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool QueryInformationJobObject(IntPtr hJob, JobObjectInfoClass JobObjectInformationClass, out JobObjectBasicAccountingInformation lpJobObjectInfo, uint cbJobObjectInfoLength, out IntPtr lpReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoClass JobObjectInfoClass, ref JobObjectExtendedLimitInformation lpJobObjectInfo, uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoClass JobObjectInfoClass, ref JobObjectBasicUIRestrictions lpJobObjectInfo, uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoClass JobObjectInfoClass, ref JobObjectAssociateCompletionPort lpJobObjectInfo, uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll")]
        public static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoClass infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool TerminateJobObject(IntPtr hJob, uint uExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetExitCodeProcess(IntPtr process, ref uint exitCode);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr handle, uint milliseconds);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern ErrorModes SetErrorMode(ErrorModes uMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForMultipleObjects(uint nCount, IntPtr[] lpHandles, bool bWaitAll, uint dwMilliseconds);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CreateProcessWithLogonW(string userName, string domain, string password, LogonFlags logonFlags, string applicationName, string commandLine, CreationFlags creationFlags, uint environment, string currentDirectory, ref StartupInfo startupInfo, out ProcessInformation processInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CreateProcess(string imageName, string cmdLine, ref SecurityAttributes lpProcessAttributes, ref SecurityAttributes lpThreadAttributes, bool boolInheritHandles, CreationFlags dwCreationFlags, IntPtr lpEnvironment, string lpszCurrentDir, ref StartupInfo startupInfo, out ProcessInformation pi);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine, ref SecurityAttributes lpProcessAttributes, ref SecurityAttributes lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref StartupInfo lpStartupInfo, out ProcessInformation lpProcessInformation);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, ref TokenPrivileges NewState, uint BufferLength, ref TokenPrivileges PreviousState, IntPtr ReturnLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, ref TokenPrivilegesSingle NewState, uint BufferLength, ref TokenPrivilegesSingle PreviousState, IntPtr ReturnLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, ref TokenPrivilegesSingle NewState, uint BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out Luid lpLuid);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateTokenEx(IntPtr hExistingToken, uint dwDesiredAccess, ref SecurityAttributes lpTokenAttributes, SecurityImpersonationLevel ImpersonationLevel, TokenType TokenType, out IntPtr phNewToken);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateIoCompletionPort(IntPtr FileHandle, IntPtr ExistingCompletionPort, UIntPtr CompletionKey, uint NumberOfConcurrentThreads);

        [DllImport("kernel32.dll")]
        public static extern bool GetQueuedCompletionStatus(IntPtr CompletionPort, out uint code, out UIntPtr lpCompletionKey, out IntPtr lpOverlapped, uint dwMilliseconds);

        [DllImport("kernel32.dll")]
        public static extern bool SetHandleInformation(IntPtr hObject, uint dwMask, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName, DesiredAccess dwDesiredAccess, uint dwShareMode, ref SecurityAttributes lpSecurityAttributes, CreationDisposition dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    }
}
