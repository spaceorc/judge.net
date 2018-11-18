using Judge.Application.Interfaces;
using Judge.Data;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Judge.Application
{
    public sealed class ApplicationExtension
    {
        private readonly string _connectionString;

        public ApplicationExtension(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Configure(Container container)
        {
            new DataContainerExtension(_connectionString, new AsyncScopedLifestyle()).Configure(container);

            container.Register<IAdminService, AdminService>(new AsyncScopedLifestyle());
            container.Register<IUserService, UserService>(new AsyncScopedLifestyle());
            container.Register<IContestsService, ContestsService>(new AsyncScopedLifestyle());
            container.Register<IProblemsService, ProblemsService>(new AsyncScopedLifestyle());
            container.Register<ISubmitSolutionService, SubmitSolutionService>(new AsyncScopedLifestyle());
        }
    }
}
