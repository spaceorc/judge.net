using Judge.Application.Interfaces;
using Judge.Data;
using Judge.Model.Core.Entities;
using Microsoft.AspNetCore.Identity;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;

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

            container.RegisterInstance<IEnumerable<IPasswordValidator<User>>>(Array.Empty<IPasswordValidator<User>>());
            container.Collection.Register(typeof(IUserValidator<>), new[] { typeof(UserValidator<>) });
            //container.Collection.Register(typeof(IPasswordValidator<>), new[] { typeof(PasswordValidator<>) });
            container.Register<SignInManager<User>>(new AsyncScopedLifestyle());
            container.Register<IUserStore<User>, UserStore>(new AsyncScopedLifestyle());
            container.Register<ISecurityService, SecurityService>(new AsyncScopedLifestyle());
            container.Register<IPasswordHasher<User>, PasswordHasher<User>>(new AsyncScopedLifestyle());
            container.Register<ILookupNormalizer, UpperInvariantLookupNormalizer>(new AsyncScopedLifestyle());
            container.Register<IdentityErrorDescriber>(new AsyncScopedLifestyle());
            container.Register<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User>>(new AsyncScopedLifestyle());
            container.Register<UserManager<User>, UserManager<User>>(new AsyncScopedLifestyle());
        }
    }
}
