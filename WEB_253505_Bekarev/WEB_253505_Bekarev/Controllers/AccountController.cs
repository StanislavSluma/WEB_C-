using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253505_Bekarev.Models;
using WEB_253505_Bekarev.Services.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WEB_253505_Bekarev.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        public async Task Login()
        {
            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel user, [FromServices] IAuthService authService)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var result = await authService.RegisterUserAsync(user.Email, user.Password, user.Avatar);
                if (result.Result)
                {
                    return Redirect(Url.Action("Index", "Home"));
                }
                else return BadRequest(result.ErrorMessage);
            }
            return View(user);
        }

        [HttpPost]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                });
        }
    }
}
