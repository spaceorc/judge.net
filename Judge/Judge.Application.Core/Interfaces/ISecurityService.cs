using Judge.Application.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Judge.Application.Interfaces
{
    public interface ISecurityService
    {
        IdentityResult SignIn(string email, string password, bool isPersistent);
        void SignOut();
        void Register(RegisterViewModel model);
        bool UserExists(string email);
    }
}
