namespace Judge.Interop
{
    public struct JobObjectBasicAccountingInformation
    {
        public ulong TotalUserTime;
        public ulong TotalKernelTime;
        public ulong ThisPeriodTotalUserTime;
        public ulong ThisPeriodTotalKernelTime;
        public uint TotalPageFaultCount;
        public uint TotalProcesses;
        public uint ActiveProcesses;
        public uint TotalTerminatedProcesses;
    }
}