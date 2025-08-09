using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Mvc.Models;
using PAWCP2.Mvc.Service;
using System.Security.Claims;

namespace PAWCP2.Mvc.Controllers
{
    public class AccountController(IUserService userService) : Controller
    {
        
        public PartialViewResult LoginPartial() => PartialView("~/Views/Shared/_LoginPartial.cshtml", new LoginViewModel());


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            
            var back = Request.Headers["Referer"].ToString();
            if (string.IsNullOrWhiteSpace(back))
                back = Url.Action("Index", "Home")!;

            if (!ModelState.IsValid)
            {
                TempData["LoginFailed"] = true;
                TempData["LoginError"] = "Please fill the required fields.";
                TempData["LastUser"] = model.Username;
                return Redirect(back);
            }

            var user = await userService.AuthenticateAsync(model.Username, model.Password);

            if (user is null)
            {
                TempData["LoginFailed"] = true;
                TempData["LoginError"] = "Username or password incorrect.";
                TempData["LastUser"] = model.Username;
                return Redirect(back);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
             new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
};

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            HttpContext.Session.SetString("User", user.Username);
            HttpContext.Session.SetString("BasicUser", model.Username);
            HttpContext.Session.SetString("BasicPass", model.Password);


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
