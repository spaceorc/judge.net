using System.Web;
using Microsoft.AspNetCore.Http;

namespace Judge.Web.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public int GetSelectedLanguage()
        {
            var value = httpContextAccessor.HttpContext.Session.GetInt32("SelectedLanguage");
            if (value != null)
            {
                return value.Value;
            }
            return 0;
        }

        public void SaveSelectedLanguage(int value)
        {
            httpContextAccessor.HttpContext.Session.SetInt32("SelectedLanguage", value);
        }
    }
}