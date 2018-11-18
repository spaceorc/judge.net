using Judge.Data.Core;
using Judge.Data.Repository;
using Judge.Model.Core.Account;
using Judge.Model.Core.CheckSolution;
using Judge.Model.Core.Configuration;
using Judge.Model.Core.Contests;
using Judge.Model.Core.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;

namespace Judge.Data
{
    public sealed class DataContainerExtension
    {
        private readonly string _connectionString;
        private readonly Lifestyle _lifestyle;

        public DataContainerExtension(string connectionString, Lifestyle lifestyle)
        {
            _connectionString = connectionString;
            this._lifestyle = lifestyle;
        }

        public void Configure(Container container)
        {
            var options = new DbContextOptionsBuilder().UseSqlServer(_connectionString).Options;
            container.Register<DataContext>(() => new DataContext(options), _lifestyle);

            container.Register<IUnitOfWorkFactory, UnitOfWorkFactory>(_lifestyle);

            container.Register<IUserRepository, UserRepository>(_lifestyle);

            container.Register<ISubmitRepository, SubmitRepository>(_lifestyle);
            container.Register<ISubmitResultRepository, SubmitResultRepository>(_lifestyle);
            container.Register<ILanguageRepository, LanguageRepository>(_lifestyle);
            container.Register<ITaskRepository, TaskRepository>(_lifestyle);
            container.Register<ITaskNameRepository, TaskNameRepository>(_lifestyle);
            container.Register<IContestsRepository, ContestsRepository>(_lifestyle);
            container.Register<IContestTaskRepository, ContestTaskRepository>(_lifestyle);
            container.Register<IContestResultRepository, ContestResultRepository>(_lifestyle);
        }
    }
}
