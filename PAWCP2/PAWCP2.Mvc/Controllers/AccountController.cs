using Microsoft.AspNetCore.Mvc;
using PAWCP2.Mvc.Models;

namespace PAWCP2.Mvc.Controllers
{
    public class AccountController : Controller
    {
        public PartialViewResult LoginPartial()
        {
            return PartialView("~/Views/Shared/_LoginPartial.cshtml", new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid && model.Username == "admin" && model.Password == "1234")
            {
                HttpContext.Session.SetString("User", model.Username);
                return RedirectToAction("Index", "Home");
            }

            TempData["LoginFailed"] = true;
            return RedirectToAction("Index", "Home");
        }
    }
}
