using Laboratorio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Laboratorio.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly dbContext contexto;

        public PropietariosController(dbContext contexto)
        {
            this.contexto = contexto;
        }

        // GET: Propietarios
        public ActionResult Index()
        {
            var lista = contexto.Propietarios.ToList();
            return View(lista);
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Propietarios.Add(propietario);
                    contexto.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(propietario);
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {
            var p = contexto.Propietarios.Find(id);
            return View(p);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                contexto.Propietarios.Update(propietario);
                contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            var p = contexto.Propietarios.Find(id);
            return View(p);
        }

        // POST: Propietarios/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Propietario propietario)
        {
            try
            {
                contexto.Propietarios.Remove(propietario);
                contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}