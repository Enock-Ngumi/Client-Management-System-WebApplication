using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}