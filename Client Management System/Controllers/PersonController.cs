using Client_Management_System.Data;
using Client_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client_Management_System.Controllers
{
    [Authorize] 
    public class PersonController : Controller
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Persons.ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Persons p)
        {
            if (!ModelState.IsValid)
                return View(p);

            _context.Persons.Add(p);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var person = _context.Persons.Find(id);

            if (person == null)
                return NotFound();

            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Persons p)
        {
            if (!ModelState.IsValid)
                return View(p);

            _context.Persons.Update(p);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var person = _context.Persons.Find(id);

            if (person == null)
                return NotFound();

            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var person = _context.Persons.Find(id);

            if (person == null)
                return NotFound();

            _context.Persons.Remove(person);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}