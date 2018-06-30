using System.Collections.Generic;

namespace Judge.Model.Core.Contests
{
    public sealed class ContestResult
    {
        public long UserId { get; set; }
        public IEnumerable<ContestTaskResult> TaskResults { get; set; }
    }
}
