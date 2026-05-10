using Client_Management_System.Data;
using Client_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Management_System.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [Route("persons")]
    public class PersonController : Controller
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index(string search)
        {
            var data = _context.Persons
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(p =>
                    p.FirstName.Contains(search) ||
                    p.LastName.Contains(search) ||
                    p.Email.Contains(search) ||
                    p.PhoneNumber.Contains(search));
            }

            var result = data
                .OrderByDescending(p => p.CreatedDate ?? DateTime.MinValue)
                .ToList();

            return View(result);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Persons p)
        {
            if (!ModelState.IsValid)
                return View(p);

            p.CreatedDate = DateTime.Now;
            p.UserId = int.TryParse(User.FindFirst("UserId")?.Value, out int userId) ? userId : 0;
            p.CreatedBy = User.Identity?.Name ?? "Admin";
            p.IsDeleted = false;

            _context.Persons.Add(p);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var person = _context.Persons.Find(id);

            if (person == null || person.IsDeleted)
                return NotFound();

            return View(person);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Persons p)
        {
            if (!ModelState.IsValid)
                return View(p);

            var existing = _context.Persons.Find(p.Id);

            if (existing == null || existing.IsDeleted)
                return NotFound();

            p.UserId = existing.UserId;
            p.CreatedBy = existing.CreatedBy;
            p.CreatedDate = existing.CreatedDate;
            p.IsDeleted = existing.IsDeleted;

            _context.Entry(existing).CurrentValues.SetValues(p);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var person = _context.Persons.Find(id);

            if (person != null && !person.IsDeleted)
            {
                person.IsDeleted = true;
                _context.SaveChanges();

                TempData["UndoId"] = id;
                TempData["Success"] = "Person deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("undodelete")]
        [ValidateAntiForgeryToken]
        public IActionResult UndoDelete(int id)
        {
            var person = _context.Persons.Find(id);

            if (person != null && person.IsDeleted)
            {
                person.IsDeleted = false;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}