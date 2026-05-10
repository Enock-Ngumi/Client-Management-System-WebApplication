using Client_Management_System.Data;
using Client_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client_Management_System.Controllers
{
    [Authorize]
    [Route("persons")]
    public class PersonController : Controller
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string search)
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

            var result = await data
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

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

            FormatPerson(p);

            p.CreatedBy = User.Identity?.Name ?? "Admin";
            p.UserId = int.TryParse(User.FindFirst("UserId")?.Value, out var uid) ? uid : null;
            p.CreatedDate = DateTime.Now;

            p.NormalizePhone();

            _context.Persons.Add(p);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var person = await _context.Persons
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
                return NotFound();

            return View(person);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Persons p)
        {
            if (!ModelState.IsValid)
                return View(p);

            var existing = await _context.Persons
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
                return NotFound();

            FormatPerson(p);

            existing.FirstName = p.FirstName;
            existing.LastName = p.LastName;
            existing.Email = p.Email;
            existing.PhoneNumber = p.PhoneNumber;
            existing.Dob = p.Dob;

            existing.NormalizePhone();

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
                return NotFound();

            person.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("undodelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UndoDelete(int id)
        {
            var person = await _context.Persons
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return NotFound();

            person.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("bulkdelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkDelete(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest();

            var persons = await _context.Persons
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            foreach (var person in persons)
            {
                person.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("bulkundo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkUndo(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest();

            var persons = await _context.Persons
                .IgnoreQueryFilters()
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            foreach (var person in persons)
            {
                person.IsDeleted = false;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        private void FormatPerson(Persons p)
        {
            if (!string.IsNullOrWhiteSpace(p.FirstName))
                p.FirstName = ToTitleCase(p.FirstName);

            if (!string.IsNullOrWhiteSpace(p.LastName))
                p.LastName = ToTitleCase(p.LastName);
        }

        private string ToTitleCase(string value)
        {
            return System.Globalization.CultureInfo.CurrentCulture
                .TextInfo
                .ToTitleCase(value.ToLower());
        }
    }
}