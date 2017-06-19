using System.Runtime.InteropServices;

namespace Judge.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IOCounters
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }
}