using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using Judge.Interop;
using Judge.RunnerInterface;
using Microsoft.Win32.SafeHandles;

namespace Judge.LimitedRunner
{
    public class LimitedRunnerService : IRunService
    {
        public RunResult Run(Configuration configuration)
        {
            var job = Pinvoke.CreateJobObject(IntPtr.Zero, null);

            if (job == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            try
            {
                var lpJobObjectInfo1 = new JobObjectExtendedLimitInformation();
                lpJobObjectInfo1.BasicLimitInformation = new JobObjectBasicLimitInformation();
                lpJobObjectInfo1.BasicLimitInformation.PerJobUserTimeLimit = 10000L * (long)configuration.TimeLimitMilliseconds;
                lpJobObjectInfo1.BasicLimitInformation.PriorityClass = (uint)PriorityClass.NORMAL_PRIORITY_CLASS;
                lpJobObjectInfo1.BasicLimitInformation.ActiveProcessLimit = 1U;
                lpJobObjectInfo1.BasicLimitInformation.LimitFlags = (uint)(JobObjectLimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS | JobObjectLimitFlags.JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION | JobObjectLimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE | JobObjectLimitFlags.JOB_OBJECT_LIMIT_PRIORITY_CLASS);
                lpJobObjectInfo1.JobMemoryLimit = (UIntPtr)configuration.MemoryLimitBytes;

                int length = Marshal.SizeOf(typeof(JobObjectExtendedLimitInformation));
                IntPtr extendedInfoPtr = Marshal.AllocHGlobal(length);
                Marshal.StructureToPtr(lpJobObjectInfo1, extendedInfoPtr, false);

                if (!Pinvoke.SetInformationJobObject(job, JobObjectInfoClass.JobObjectExtendedLimitInformation, extendedInfoPtr, (uint)length))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var lpJobObjectInfo2 = new JobObjectBasicUIRestrictions();

                lpJobObjectInfo2.UIRestrictionsClass = UIRestrictionClass.JOB_OBJECT_UILIMIT_DESKTOP | UIRestrictionClass.JOB_OBJECT_UILIMIT_DISPLAYSETTINGS | UIRestrictionClass.JOB_OBJECT_UILIMIT_EXITWINDOWS | UIRestrictionClass.JOB_OBJECT_UILIMIT_GLOBALATOMS | UIRestrictionClass.JOB_OBJECT_UILIMIT_HANDLES | UIRestrictionClass.JOB_OBJECT_UILIMIT_READCLIPBOARD | UIRestrictionClass.JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS | UIRestrictionClass.JOB_OBJECT_UILIMIT_WRITECLIPBOARD;
                if (!Pinvoke.SetInformationJobObject(job, JobObjectInfoClass.JobObjectBasicUIRestrictions, ref lpJobObjectInfo2, (uint)Marshal.SizeOf((object)lpJobObjectInfo2)))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                IntPtr ioCompletionPort = Pinvoke.CreateIoCompletionPort((IntPtr)(-1), IntPtr.Zero, (UIntPtr)1U, 1U);
                if (ioCompletionPort == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                try
                {
                    JobObjectAssociateCompletionPort lpJobObjectInfo3 = new JobObjectAssociateCompletionPort();
                    lpJobObjectInfo3.CompletionKey = 1;
                    lpJobObjectInfo3.CompletionPort = ioCompletionPort;
                    if (!Pinvoke.SetInformationJobObject(job, JobObjectInfoClass.JobObjectAssociateCompletionPortInformation, ref lpJobObjectInfo3, (uint)Marshal.SizeOf((object)lpJobObjectInfo3)))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    StartupInfo startupInfo = new StartupInfo();
                    startupInfo.cb = Marshal.SizeOf((object)startupInfo);
                    startupInfo.dwFlags = 0x100;

                    
                    var input = new FileStream(@"C:\input.txt", FileMode.Open, FileAccess.Read);
                    Pinvoke.SetHandleInformation(input.Handle, 0x00000001, 0x00000001);
                    startupInfo.hStdInput = input.Handle;

                    var output = new FileStream(@"D:\output.txt", FileMode.Create, FileAccess.Write);
                    Pinvoke.SetHandleInformation(output.Handle, 0x00000001, 0x00000001);
                    startupInfo.hStdOutput = output.Handle;

                    Pinvoke.SetErrorMode(ErrorModes.SEM_FAILCRITICALERRORS | ErrorModes.SEM_NOALIGNMENTFAULTEXCEPT | ErrorModes.SEM_NOGPFAULTERRORBOX | ErrorModes.SEM_NOOPENFILEERRORBOX);
                    CreationFlags dwCreationFlags = CreationFlags.CREATE_BREAKAWAY_FROM_JOB | CreationFlags.CREATE_SUSPENDED | CreationFlags.CREATE_SEPARATE_WOW_VDM;
                    SecurityAttributes securityAttributes = new SecurityAttributes();
                    securityAttributes.bInheritHandle = 1;
                    ProcessInformation pi;

                    if (!Pinvoke.CreateProcess(null, configuration.RunString, ref securityAttributes, ref securityAttributes, boolInheritHandles: true, dwCreationFlags: dwCreationFlags, lpEnvironment: IntPtr.Zero, lpszCurrentDir: configuration.Directory, startupInfo: ref startupInfo, pi: out pi))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    try
                    {
                        if (!Pinvoke.AssignProcessToJobObject(job, pi.hProcess))
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        Timer timer = null;
                        try
                        {
                            //if (invokeOptions.ProcessorConsumingCheckPeriod.CompareTo(TimeSpan.Zero) > 0 && invokeOptions.MinProcessorConsumingPercent > new Decimal(0))
                            //{
                            //    timer = new Timer(invokeOptions.ProcessorConsumingCheckPeriod.TotalMilliseconds);
                            //    timer.AutoReset = true;
                            //    TimeSpan prevTime = TimeSpan.Zero;
                            //    timer.Elapsed += (ElapsedEventHandler)((sender, e) =>
                            //    {
                            //        TimeSpan userTimeConsumed = LimitedInvokationService.GetUserTimeConsumed(job);
                            //        Decimal val1 = (Decimal)(100.0 * userTimeConsumed.Subtract(prevTime).TotalMilliseconds / invokeOptions.ProcessorConsumingCheckPeriod.TotalMilliseconds);
                            //        result.MinPeriodProcessorConsumingPercent = Math.Min(val1, result.MinPeriodProcessorConsumingPercent);
                            //        if (val1 < invokeOptions.MinProcessorConsumingPercent)
                            //        {
                            //            result.Status = InvokeResultStatus.LowProcessorConsumingPercent;
                            //            Pinvoke.TerminateJobObject(job, 1U);
                            //        }
                            //        else
                            //            prevTime = userTimeConsumed;
                            //    });
                            //}
                            IntPtr[] lpHandles = new IntPtr[2]
                            {
                                job,
                                pi.hProcess
                            };
                            int num2 = (int)Pinvoke.ResumeThread(pi.hThread);
                            uint dwMilliseconds = (uint)configuration.TimeLimitMilliseconds;
                            if (timer != null)
                                timer.Start();
                            int num3 = (int)Pinvoke.WaitForMultipleObjects(2U, lpHandles, false, dwMilliseconds);
                            if (timer != null)
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
                        uint code;
                        UIntPtr lpCompletionKey;
                        IntPtr lpOverlapped;

                        RunResult result = null;

                        while (Pinvoke.GetQueuedCompletionStatus(ioCompletionPort, out code, out lpCompletionKey, out lpOverlapped, 0U))
                        {
                            switch ((JobObjectMessageType)code)
                            {
                                case JobObjectMessageType.JOB_OBJECT_MSG_END_OF_JOB_TIME:
                                    result = RunResult.Create(RunStatus.TimeLimitExceeded);
                                    break;
                                case JobObjectMessageType.JOB_OBJECT_MSG_ACTIVE_PROCESS_LIMIT:
                                    result = RunResult.Create(RunStatus.SecurityViolation);
                                    break;
                                case JobObjectMessageType.JOB_OBJECT_MSG_ABNORMAL_EXIT_PROCESS:
                                    result = RunResult.Create(RunStatus.RuntimeError);
                                    break;
                                case JobObjectMessageType.JOB_OBJECT_MSG_JOB_MEMORY_LIMIT:
                                    result = RunResult.Create(RunStatus.MemoryLimitExceeded);
                                    break;
                                default:
                                    continue;
                            }
                        }
                        uint exitCode = 0;
                        if (!Pinvoke.GetExitCodeProcess(pi.hProcess, ref exitCode))
                            throw new Win32Exception(Marshal.GetLastWin32Error());

                        //result.ExitCode = (int)exitCode;

                        if (result == null)
                            result = RunResult.Create(exitCode == 0 ? RunStatus.Success : RunStatus.RuntimeError);

                        JobObjectExtendedLimitInformation lpJobObjectInfo4 = new JobObjectExtendedLimitInformation();
                        lpJobObjectInfo4.BasicLimitInformation = new JobObjectBasicLimitInformation();
                        IntPtr lpReturnLength;
                        if (!Pinvoke.QueryInformationJobObject(job, JobObjectInfoClass.JobObjectExtendedLimitInformation, out lpJobObjectInfo4, (uint)Marshal.SizeOf((object)lpJobObjectInfo4), out lpReturnLength))
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        result.PeakMemoryBytes = (int)lpJobObjectInfo4.PeakJobMemoryUsed;
                        result.TimeConsumedMilliseconds = GetUserTimeConsumed(job);
                        return result;
                    }
                    finally
                    {
                        Pinvoke.CloseHandle(input.Handle);
                        Pinvoke.CloseHandle(output.Handle);
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
            JobObjectBasicAccountingInformation lpJobObjectInfo = new JobObjectBasicAccountingInformation();
            IntPtr lpReturnLength;
            if (!Pinvoke.QueryInformationJobObject(job, JobObjectInfoClass.JobObjectBasicAccountingInformation, out lpJobObjectInfo, (uint)Marshal.SizeOf((object)lpJobObjectInfo), out lpReturnLength))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return (int)lpJobObjectInfo.TotalUserTime / 10000;
        }
    }
}
