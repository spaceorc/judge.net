using System;

namespace Judge.Interop
{
    [Flags]
    public enum LogonFlags
    {
        LOGON_WITH_PROFILE = 1,
        LOGON_NETCREDENTIALS_ONLY = 2,
    }
}