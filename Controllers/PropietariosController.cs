using Laboratorio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Laboratorio.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly LaboratorioContext _context;

        public PropietariosController(LaboratorioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var lista = _context.Propietarios.ToList();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                _context.Propietarios.Add(propietario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }

        public IActionResult Edit(int id)
        {
            var p = _context.Propietarios.Find(id);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                _context.Propietarios.Update(propietario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }

        public IActionResult Eliminar(int id)
        {
            var p = _context.Propietarios.Find(id);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id, Propietario propietario)
        {
            _context.Propietarios.Remove(propietario);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
