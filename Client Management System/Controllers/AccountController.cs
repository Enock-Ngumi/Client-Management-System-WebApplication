using Client_Management_System.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Client_Management_System.Controllers
{
    [AllowAnonymous]
    [Route("")]
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/admin/index");

            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = _authService.Login(username, password);

            if (!result.success)
            {
                ViewBag.Error = result.message;
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, result.role),
                new Claim("UserId", result.userId.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            if (result.role == "Admin")
                return Redirect("/admin/index");

            if (result.role == "User")
                return Redirect("/user/index");

            return Redirect("/login");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            HttpContext.Session.Clear();

            return Redirect("/login");
        }
    }
}