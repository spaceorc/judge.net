using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace Judge.Application
{
    public class HttpContextPrinciple : IPrincipal
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextPrinciple(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public IIdentity Identity => _contextAccessor.HttpContext.User.Identity;
        public bool IsInRole(string role) => _contextAccessor.HttpContext.User.IsInRole(role);
    }
}
