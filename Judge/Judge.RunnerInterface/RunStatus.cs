namespace Judge.RunnerInterface
{
    public enum RunStatus
    {
        Success,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        SecurityViolation,
        RuntimeError,
        InvocationFailed,
        IdlenessLimitExceeded
    }
}
