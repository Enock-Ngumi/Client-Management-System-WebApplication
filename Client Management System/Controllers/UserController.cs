using Microsoft.AspNetCore.Mvc;

namespace Client_Management_System.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}
