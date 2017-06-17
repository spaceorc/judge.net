using System.Runtime.InteropServices;

namespace Judge.Interop
{
    public struct TokenPrivileges
    {
        public uint PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LuidAndAttributes[] Privileges;
    }
}