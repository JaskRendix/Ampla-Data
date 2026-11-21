using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AmplaData.Web.Authentication;
using AmplaData.Web.Authentication.Forms;
using AmplaData.Web.Models;
using AmplaData.Web.Sessions;

namespace AmplaData.Web.Controllers
{
    /// <summary>
    /// Account Controller to provide access
    /// </summary>
    public class AccountController : BootstrapBaseController
    {
        private readonly IAmplaUserService amplaUserService;
        private readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly IAmplaSessionStorage amplaSessionStorage;

        public AccountController(
            IAmplaUserService amplaUserService,
            IFormsAuthenticationService formsAuthenticationService,
            IAmplaSessionStorage amplaSessionStorage)
        {
            this.amplaUserService = amplaUserService;
            this.formsAuthenticationService = formsAuthenticationService;
            this.amplaSessionStorage = amplaSessionStorage;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // GET: /Account/User
        [HttpGet]
        [ActionName("User")]
        [Authorize]
        public IActionResult CurrentUser()
        {
            string session = amplaSessionStorage.GetAmplaSession();

            AmplaUser user = amplaUserService.RenewSession(session);

            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            UserModel model = new UserModel
            {
                UserName = user.UserName,
                Session = user.Session,
                Started = user.LoginTime,
                LastActivity = user.LastActivity,
                LoginType = user.LoginType
            };

            return View("CurrentUser", model);
        }

        // POST: /Account/LoginIntegrated
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult LoginIntegrated(string returnUrl)
        {
            IntegratedLoginModel model = new IntegratedLoginModel
            {
                UseIntegratedSecurity = true
            };

            if (ModelState.IsValid)
            {
                string message;

                if (!model.UseIntegratedSecurity)
                {
                    return RedirectToAction(nameof(Login));
                }

                AmplaUser amplaUser =
                    amplaUserService.IntegratedLogin(out message);

                if (amplaUser != null)
                {
                    amplaSessionStorage.SetAmplaSession(amplaUser.Session);

                    formsAuthenticationService.StoreUserTicket(
                        amplaUser,
                        model.RememberMe);

                    if (UrlIsLocal(returnUrl))
                    {
                        Information("Login successful.");
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                Error(message);
                ModelState.AddModelError(string.Empty, message);
            }

            return View("Login");
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(
            SimpleLoginModel model,
            string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string message;

                AmplaUser amplaUser =
                    amplaUserService.SimpleLogin(
                        model.UserName,
                        model.Password,
                        out message);

                if (amplaUser != null)
                {
                    amplaSessionStorage.SetAmplaSession(amplaUser.Session);

                    formsAuthenticationService.StoreUserTicket(
                        amplaUser,
                        model.RememberMe);

                    if (UrlIsLocal(returnUrl))
                    {
                        Information("Login successful.");
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                Error(message);
                ModelState.AddModelError(string.Empty, message);
            }

            return View(model);
        }

        private bool UrlIsLocal(string url)
        {
            return
                !string.IsNullOrEmpty(url)
                && (Url == null || Url.IsLocalUrl(url))
                && url.StartsWith("/")
                && !url.StartsWith("//")
                && !url.StartsWith("/\\");
        }

        // POST: /Account/Logout
        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            var ticket = formsAuthenticationService.GetUserTicket();

            if (ticket != null)
            {
                string user = ticket.UserName;

                amplaUserService.Logout(user);

                Information("Logout successful.");
            }

            amplaSessionStorage.SetAmplaSession(null);

            formsAuthenticationService.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}