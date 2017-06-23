using Judge.RunnerInterface;

namespace Judge.LimitedRunner
{
    internal sealed class RunResult : IRunResult
    {
        public RunStatus RunStatus { get; }
        public int TimeConsumedMilliseconds { get; }
        public int TimePassedMilliseconds { get; }
        public int PeakMemoryBytes { get; }
        public string TextStatus { get; }
        public string Description { get; }
        public string Output { get; }

        public RunResult(RunStatus runStatus, int timeConsumedMilliseconds, int timePassedMilliseconds, int peakMemoryBytes)
        {
            RunStatus = runStatus;
            TimeConsumedMilliseconds = timeConsumedMilliseconds;
            TimePassedMilliseconds = timePassedMilliseconds;
            PeakMemoryBytes = peakMemoryBytes;
        }
    }
}
