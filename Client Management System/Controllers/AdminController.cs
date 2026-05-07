using Microsoft.AspNetCore.Mvc;

namespace Client_Management_System.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}
