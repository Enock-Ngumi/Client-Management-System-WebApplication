using Client_Management_System.Data;
using Client_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var persons = _context.Persons.ToList();

            ViewBag.TotalPersons = persons.Count;

            ViewBag.ActiveClients = persons.Count;

            ViewBag.RecentPersons = persons
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToList();

            ViewBag.NewEntries = persons
                .OrderByDescending(p => p.Id)
                .Take(5)
                .Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}