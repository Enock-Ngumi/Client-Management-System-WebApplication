using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client_Management_System.Controllers
{
    [Authorize(Roles = "User")]
    [Route("user")]
    public class UserController : Controller
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}