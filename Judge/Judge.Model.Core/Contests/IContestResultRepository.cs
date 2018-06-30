using System.Collections.Generic;

namespace Judge.Model.Core.Contests
{
    public interface IContestResultRepository
    {
        IEnumerable<ContestResult> Get(long contestId);
    }
}
