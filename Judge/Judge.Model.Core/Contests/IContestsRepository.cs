using System.Collections.Generic;

namespace Judge.Model.Core.Contests
{
    public interface IContestsRepository
    {
        IEnumerable<Contest> GetList(ISpecification<Contest> specification);
        Contest Get(int id);
        void Add(Contest contest);
    }
}
