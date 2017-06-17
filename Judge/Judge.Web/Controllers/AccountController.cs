using System.Web;
using System.Web.Mvc;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Account;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Judge.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(ISecurityService securityService, IAuthenticationManager authenticationManager)
        {
            _securityService = securityService;
            _authenticationManager = authenticationManager;
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Logout()
        {
            _securityService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!_securityService.UserExists(model.Email))
            {
                ModelState.AddModelError(string.Empty, Resources.UserNotFound);
                return View(model);
            }

            var result = _securityService.SignIn(model.Email, model.Password, model.RememberMe);
            if (result == SignInStatus.Success)
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, Resources.IncorrectPassword);
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (_securityService.UserExists(model.Email))
            {
                ModelState.AddModelError(string.Empty, Resources.UserWithEmailIsAlreadyRegistered);
                return View(model);
            }

            _securityService.Register(model);

            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "~/";

            var loginInfo = _authenticationManager.GetExternalLoginInfo();
            if (loginInfo == null)
                return RedirectToAction("Login");

            if (_securityService.UserExists(loginInfo.Email))
            {
                _securityService.SignIn(loginInfo);
            }

            var providerKey = loginInfo.Login.ProviderKey;

            // Aplication specific code goes here.
            //var userBus = new busUser();
            //var user = userBus.ValidateUserWithExternalLogin(providerKey);
            //if (user == null)
            //{
            //    return RedirectToAction("LogOn", new
            //    {
            //        message = "Unable to log in with " + loginInfo.Login.LoginProvider +
            //                  ". " + userBus.ErrorMessage
            //    });
            //}

            //// store on AppUser
            //AppUserState appUserState = new AppUserState();
            //appUserState.FromUser(user);

            //// write the authentication cookie
            //IdentitySignin(appUserState, providerKey, isPersistent: true);

            return Redirect(returnUrl);
        }

        private sealed class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; }
            public string RedirectUri { get; }
            public string UserId { get; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                    properties.Dictionary["UserId"] = UserId;

                var owin = context.HttpContext.GetOwinContext();
                owin.Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}