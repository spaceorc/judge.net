using System.Runtime.InteropServices;

namespace Judge.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct JobObjectBasicUIRestrictions
    {
        public UIRestrictionClass UIRestrictionsClass;
    }
}