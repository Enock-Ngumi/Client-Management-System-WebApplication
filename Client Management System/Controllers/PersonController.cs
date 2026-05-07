using Client_Management_System.Models;
using Client_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Client_Management_System.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonService _service;

        public PersonController(PersonService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var data = _service.GetAll();
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
            _service.Add(p);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var person = _service.GetById(id);

            if (person == null)
                return NotFound();

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Persons p)
        {
            _service.Update(p);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var person = _service.GetById(id);

            if (person == null)
                return NotFound();

            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}