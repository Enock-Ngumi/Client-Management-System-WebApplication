using Microsoft.AspNetCore.Mvc;
using Client_Management_System.Services;

namespace Client_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var result = _authService.Login(username, password);

            if (!result.success)
            {
                ViewBag.Error = result.message;
                return View();
            }

            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Role", result.role);
            HttpContext.Session.SetString("UserId", result.userId.ToString());

            if (result.role == "Admin")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "User");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}