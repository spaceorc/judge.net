using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using Judge.Interop;
using Judge.RunnerInterface;

namespace Judge.LimitedRunner
{
    public class LimitedRunnerService : IRunService
    {
        public IRunResult Run(Configuration configuration)
        {
            var job = Pinvoke.CreateJobObject(IntPtr.Zero, null);

            if (job == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            try
            {
                var jobObjectExtendedLimitInformation = new JobObjectExtendedLimitInformation
                {
                    BasicLimitInformation = new JobObjectBasicLimitInformation
                    {
                        PerJobUserTimeLimit = 10000L * configuration.TimeLimitMilliseconds,
                        PriorityClass = PriorityClass.NORMAL_PRIORITY_CLASS,
                        ActiveProcessLimit = 1U,
                        LimitFlags = JobObjectLimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS |
                                     JobObjectLimitFlags.JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION |
                                     JobObjectLimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE |
                                     JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS |
                                     JobObjectLimitFlags.JOB_OBJECT_LIMIT_JOB_TIME
                    },
                    JobMemoryLimit = (UIntPtr)configuration.MemoryLimitBytes
                };

                var length = Marshal.SizeOf(typeof(JobObjectExtendedLimitInformation));
                var extendedInfoPtr = Marshal.AllocHGlobal(length);
                Marshal.StructureToPtr(jobObjectExtendedLimitInformation, extendedInfoPtr, false);

                if (!Pinvoke.SetInformationJobObject(job, JobObjectInfoClass.JobObjectExtendedLimitInformation, extendedInfoPtr, (uint)length))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var uiRestrictions = new JobObjectBasicUIRestrictions
                {
                    UIRestrictionsClass = UIRestrictionClass.JOB_OBJECT_UILIMIT_DESKTOP |
                                          UIRestrictionClass.JOB_OBJECT_UILIMIT_DISPLAYSETTINGS |
                                          UIRestrictionClass.JOB_OBJECT_UILIMIT_EXITWINDOWS |
                                          UIRestrictionClass.JOB_OBJECT_UILIMIT_GLOBALATOMS |
                                          UIRestrictionClass.JOB_OBJECT_UILIMIT_HANDLES |
                                          UIRestrictionClass.JOB_OBJECT_UILIMIT_READCLIPBOARD |
                                          UIRestrictionClass.JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS |
                                          UIRestrictionClass.JOB_OBJECT_UILIMIT_WRITECLIPBOARD
                };

                if (!Pinvoke.SetInformationJobObject(job, JobObjectInfoClass.JobObjectBasicUIRestrictions, ref uiRestrictions, (uint)Marshal.SizeOf(uiRestrictions)))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                var ioCompletionPort = Pinvoke.CreateIoCompletionPort((IntPtr)(-1), IntPtr.Zero, (UIntPtr)1U, 1U);
                if (ioCompletionPort == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                try
                {
                    var lpJobObjectInfo3 = new JobObjectAssociateCompletionPort
                    {
                        CompletionKey = 1,
                        CompletionPort = ioCompletionPort
                    };

                    if (!Pinvoke.SetInformationJobObject(job, JobObjectInfoClass.JobObjectAssociateCompletionPortInformation, ref lpJobObjectInfo3, (uint)Marshal.SizeOf((object)lpJobObjectInfo3)))
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    var startupInfo = new StartupInfo();
                    startupInfo.cb = Marshal.SizeOf((object)startupInfo);
                    startupInfo.dwFlags = Pinvoke.STARTF_USESTDHANDLES;

                    var securityAttributes = new SecurityAttributes { bInheritHandle = true };

                    var inputFile = Path.Combine(configuration.Directory, configuration.InputFile);
                    var outputFile = Path.Combine(configuration.Directory, configuration.OutputFile);

                    var input = Pinvoke.CreateFile(inputFile, DesiredAccess.GENERIC_READ, 0U, ref securityAttributes,
                        CreationDisposition.OPEN_EXISTING, 0, IntPtr.Zero);

                    var output = Pinvoke.CreateFile(outputFile, DesiredAccess.GENERIC_WRITE, 0, ref securityAttributes,
                        CreationDisposition.CREATE_NEW, 0, IntPtr.Zero);

                    startupInfo.hStdInput = input;
                    startupInfo.hStdOutput = output;

                    var errorModes = ErrorModes.SEM_FAILCRITICALERRORS |
                                     ErrorModes.SEM_NOALIGNMENTFAULTEXCEPT |
                                     ErrorModes.SEM_NOGPFAULTERRORBOX |
                                     ErrorModes.SEM_NOOPENFILEERRORBOX;

                    Pinvoke.SetErrorMode(errorModes);

                    var dwCreationFlags = CreationFlags.CREATE_BREAKAWAY_FROM_JOB |
                                          CreationFlags.CREATE_SUSPENDED |
                                          CreationFlags.CREATE_SEPARATE_WOW_VDM |
                                          CreationFlags.CREATE_NO_WINDOW;

                    ProcessInformation pi;

                    var low = false;
                    decimal t = 100;
                    if (!Pinvoke.CreateProcess(null, configuration.RunString, ref securityAttributes, ref securityAttributes, boolInheritHandles: true, dwCreationFlags: dwCreationFlags, lpEnvironment: IntPtr.Zero, lpszCurrentDir: configuration.Directory, startupInfo: ref startupInfo, pi: out pi))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    try
                    {
                        if (!Pinvoke.AssignProcessToJobObject(job, pi.hProcess))
                            throw new Win32Exception(Marshal.GetLastWin32Error());

                        Timer timer = null;
                        uint waitResult;
                        try
                        {
                            var interval = 50;

                            timer = new Timer(interval); //TODO: change
                            timer.AutoReset = true;

                            var prevTime = 0;
                            timer.Elapsed += (sender, e) =>
                            {
                                var userTimeConsumed = GetUserTimeConsumed(job);
                                var val1 = (decimal)(100.0 * (userTimeConsumed - prevTime) / interval);
                                t = Math.Min(val1, t);
                                if (val1 < 10)
                                {
                                    low = true;
                                    Pinvoke.TerminateJobObject(job, 1U);
                                }
                                else
                                    prevTime = userTimeConsumed;
                            };

                            var lpHandles = new[]
                            {
                                job,
                                pi.hProcess
                            };
                            Pinvoke.ResumeThread(pi.hThread);
                            var dwMilliseconds = (uint)configuration.TimeLimitMilliseconds;
                            timer.Start();
                            waitResult = Pinvoke.WaitForMultipleObjects(2U, lpHandles, false, dwMilliseconds);

                            timer.Stop();
                        }
                        finally
                        {
                            if (timer != null)
                            {
                                if (timer.Enabled)
                                    timer.Stop();
                                timer.Close();
                            }
                        }

                        var status = (RunStatus)(-1);

                        if (waitResult == Pinvoke.WAIT_TIMEOUT)
                        {
                            status = RunStatus.TimeLimitExceeded;
                        }

                        if (low)
                        {
                            status = RunStatus.IdlenessLimitExceeded;
                        }

                        uint code;
                        UIntPtr lpCompletionKey;
                        IntPtr lpOverlapped;

                        while (Pinvoke.GetQueuedCompletionStatus(ioCompletionPort, out code, out lpCompletionKey, out lpOverlapped, 0U))
                        {
                            switch ((JobObjectMessageType)code)
                            {
                                case JobObjectMessageType.JOB_OBJECT_MSG_END_OF_JOB_TIME:
                                    status = RunStatus.TimeLimitExceeded;
                                    break;
                                case JobObjectMessageType.JOB_OBJECT_MSG_ACTIVE_PROCESS_LIMIT:
                                    status = RunStatus.SecurityViolation;
                                    break;
                                case JobObjectMessageType.JOB_OBJECT_MSG_ABNORMAL_EXIT_PROCESS:
                                    status = RunStatus.RuntimeError;
                                    break;
                                case JobObjectMessageType.JOB_OBJECT_MSG_JOB_MEMORY_LIMIT:
                                    status = RunStatus.MemoryLimitExceeded;
                                    break;
                                default:
                                    continue;
                            }
                        }
                        uint exitCode = 0;
                        if (!Pinvoke.GetExitCodeProcess(pi.hProcess, ref exitCode))
                            throw new Win32Exception(Marshal.GetLastWin32Error());

                        if (status == (RunStatus)(-1) && exitCode != 0)
                            status = RunStatus.RuntimeError;

                        IntPtr lpReturnLength;
                        if (!Pinvoke.QueryInformationJobObject(job, JobObjectInfoClass.JobObjectExtendedLimitInformation, out jobObjectExtendedLimitInformation, (uint)Marshal.SizeOf(jobObjectExtendedLimitInformation), out lpReturnLength))
                            throw new Win32Exception(Marshal.GetLastWin32Error());

                        var peakMemoryBytes = (int)jobObjectExtendedLimitInformation.PeakJobMemoryUsed;
                        var timeConsumedMilliseconds = GetUserTimeConsumed(job);

                        if (peakMemoryBytes > configuration.MemoryLimitBytes)
                            status = RunStatus.MemoryLimitExceeded;

                        if (timeConsumedMilliseconds > configuration.TimeLimitMilliseconds)
                            status = RunStatus.TimeLimitExceeded;

                        if (status == (RunStatus)(-1))
                            status = RunStatus.Success;

                        if (status == RunStatus.TimeLimitExceeded)
                            timeConsumedMilliseconds = Math.Max(timeConsumedMilliseconds, configuration.TimeLimitMilliseconds);

                        var result = new RunResult(status, timeConsumedMilliseconds, timeConsumedMilliseconds, peakMemoryBytes);

                        return result;
                    }
                    finally
                    {
                        Pinvoke.CloseHandle(input);
                        Pinvoke.CloseHandle(output);
                        Pinvoke.CloseHandle(pi.hThread);
                        Pinvoke.CloseHandle(pi.hProcess);
                    }
                }
                finally
                {
                    Pinvoke.CloseHandle(ioCompletionPort);
                }
            }
            finally
            {
                Pinvoke.CloseHandle(job);
            }
        }

        private static int GetUserTimeConsumed(IntPtr job)
        {
            var lpJobObjectInfo = new JobObjectBasicAccountingInformation();

            IntPtr lpReturnLength;

            if (!Pinvoke.QueryInformationJobObject(job, JobObjectInfoClass.JobObjectBasicAccountingInformation, out lpJobObjectInfo, (uint)Marshal.SizeOf(lpJobObjectInfo), out lpReturnLength))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return (int)lpJobObjectInfo.TotalUserTime / 10000;
        }
    }
}
