using System;
using System.Linq.Expressions;

namespace Judge.Model.Core.CheckSolution
{
    public sealed class AllTasksSpecification : ISpecification<Task>
    {
        public static AllTasksSpecification Instance { get; } = new AllTasksSpecification();

        private AllTasksSpecification()
        {

        }
        public Expression<Func<Task, bool>> IsSatisfiedBy { get; } = x => true;
    }
}
