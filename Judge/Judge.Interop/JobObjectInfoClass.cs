namespace Judge.Interop
{
    public enum JobObjectInfoClass : uint
    {
        JobObjectBasicAccountingInformation = 1,
        JobObjectBasicLimitInformation = 2,
        JobObjectBasicUIRestrictions = 4,
        JobObjectSecurityLimitInformation = 5,
        JobObjectEndOfJobTimeInformation = 6,
        JobObjectAssociateCompletionPortInformation = 7,
        JobObjectExtendedLimitInformation = 9,
    }
}