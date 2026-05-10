using Client_Management_System.Data;
using Client_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Client_Management_System.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
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

            ViewBag.NewEntries = _context.Persons
                .OrderByDescending(p => p.Id)
                .Take(5)
                .Count();

            ViewBag.Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            ViewBag.ActivePersons = persons.Count(p => !p.IsDeleted);
            ViewBag.DeletedPersons = persons.Count(p => p.IsDeleted);

            ViewBag.ThisWeek = persons.Count(p =>
                p.CreatedDate >= DateTime.Now.AddDays(-7));

            ViewBag.Monthly = persons
                .GroupBy(p => new { p.CreatedDate.Year, p.CreatedDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Label = new DateTime(g.Key.Year, g.Key.Month, 1)
                        .ToString("MMM yyyy"),
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}