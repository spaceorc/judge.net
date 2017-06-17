namespace Judge.Interop
{
    public struct JobObjectBasicLimitInformation
    {
        public ulong PerProcessUserTimeLimit;
        public ulong PerJobUserTimeLimit;
        public JobObjectLimitFlags LimitFlags;
        public uint MinimumWorkingSetSize;
        public uint MaximumWorkingSetSize;
        public uint ActiveProcessLimit;
        public uint Affinity;
        public uint PriorityClass;
        public uint SchedulingClass;
    }
}