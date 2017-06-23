namespace Judge.RunnerInterface
{
    public interface IRunResult
    {
        RunStatus RunStatus { get; }
        int TimeConsumedMilliseconds { get; }
        int TimePassedMilliseconds { get; }
        int PeakMemoryBytes { get; }
        string TextStatus { get; }
        string Description { get; }
        string Output { get; }
    }
}