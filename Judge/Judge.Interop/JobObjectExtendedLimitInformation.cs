namespace Judge.Interop
{
    public struct JobObjectExtendedLimitInformation
    {
        public JobObjectBasicLimitInformation BasicLimitInformation;
        public IOCounters IoInfo;
        public uint ProcessMemoryLimit;
        public uint JobMemoryLimit;
        public uint PeakProcessMemoryUsed;
        public uint PeakJobMemoryUsed;
    }
}