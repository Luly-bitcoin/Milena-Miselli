using Laboratorio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Laboratorio.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly dbContext contexto;

        public InquilinosController(dbContext contexto)
        {
            this.contexto = contexto;
        }

        // GET: Inquilinos
        public ActionResult Index()
        {
            var lista = contexto.Inquilinos.ToList();
            return View(lista);
        }

        // GET: Inquilinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Inquilinos.Add(inquilino);
                    contexto.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(inquilino);
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Edit/5
        public ActionResult Edit(int id)
        {
            var i = contexto.Inquilinos.Find(id);
            return View(i);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                contexto.Inquilinos.Update(inquilino);
                contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            var i = contexto.Inquilinos.Find(id);
            return View(i);
        }

        // POST: Inquilinos/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Inquilino inquilino)
        {
            try
            {
                contexto.Inquilinos.Remove(inquilino);
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