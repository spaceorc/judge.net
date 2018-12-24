using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Account;
using Judge.Model.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Judge.Application
{
    internal sealed class SecurityService : ISecurityService
    {
        private readonly SignInManager<User> _signInManager;

        public SecurityService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public SignInResult SignIn(string email, string password, bool isPersistent)
        {
            return _signInManager.PasswordSignInAsync(email, password, isPersistent, false).Result;
        }

        public void SignOut()
        {
            _signInManager.SignOutAsync().Wait();
        }

        public void Register(RegisterViewModel model)
        {
            _signInManager.UserManager.CreateAsync(new User { UserName = model.UserName, Email = model.Email }, model.Password).Wait();
        }

        public bool UserExists(string email)
        {
            return _signInManager.UserManager.FindByEmailAsync(email).Result != null;
        }
    }
}
