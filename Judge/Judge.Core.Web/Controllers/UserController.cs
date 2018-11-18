using Judge.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Controllers
{
    public sealed class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        public ActionResult Statistics(long id)
        {
            var user = _userService.GetUserInfo(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}