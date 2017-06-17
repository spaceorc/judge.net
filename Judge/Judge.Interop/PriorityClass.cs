namespace Judge.Interop
{
    public enum PriorityClass : uint
    {
        NORMAL_PRIORITY_CLASS = 32,
        IDLE_PRIORITY_CLASS = 64,
        REALTIME_PRIORITY_CLASS = 256,
        BELOW_NORMAL_PRIORITY_CLASS = 16384,
        PROCESS_MODE_BACKGROUND_BEGIN = 1048576,
        PROCESS_MODE_BACKGROUND_END = 2097152,
    }
}
