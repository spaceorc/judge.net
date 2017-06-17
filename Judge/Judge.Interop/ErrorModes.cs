using System;

namespace Judge.Interop
{
    [Flags]
    public enum ErrorModes : uint
    {
        SYSTEM_DEFAULT = 0,
        SEM_FAILCRITICALERRORS = 1,
        SEM_NOALIGNMENTFAULTEXCEPT = 4,
        SEM_NOGPFAULTERRORBOX = 2,
        SEM_NOOPENFILEERRORBOX = 32768,
    }
}