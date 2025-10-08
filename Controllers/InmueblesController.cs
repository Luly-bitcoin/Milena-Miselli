using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laboratorio.Models;
using Microsoft.AspNetCore.Authorization;

namespace Laboratorio.Controllers
{
    [Authorize]
    public class InmueblesController : Controller
    {
        private readonly InmobiliariaDbContext _context;

        public InmueblesController(InmobiliariaDbContext context)
        {
            _context = context;
        }

        // GET: Inmuebles
        public async Task<IActionResult> Index()
        {
            var inmuebles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .Include(i => i.TipoInmueble)
                .Where(i => i.Estado == "Disponible")
                .ToListAsync();
            return View(inmuebles);
        }

        // GET: Inmuebles Disponibles
        public async Task<IActionResult> Disponibles()
        {
            var inmueblesDisponibles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .Include(i => i.TipoInmueble)
                .Where(i => i.Estado == "Disponible")
                .ToListAsync();
            return View("Index", inmueblesDisponibles);
        }

        // GET: Inmuebles por Propietario
        public async Task<IActionResult> PorPropietario(int propietarioId)
        {
            var inmuebles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .Include(i => i.TipoInmueble)
                .Where(i => i.IdPropietario == propietarioId)
                .ToListAsync();
            return View("Index", inmuebles);
        }

        // GET: Inmuebles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var inmueble = await _context.Inmuebles
                .Include(i => i.Propietario)
                .Include(i => i.TipoInmueble)
                .FirstOrDefaultAsync(m => m.IdInmuebles == id);

            if (inmueble == null) return NotFound();

            // Obtener contratos del inmueble
            var contratos = await _context.Contratos
                .Include(c => c.Inquilino)
                .Where(c => c.IdInmueble == id)
                .OrderByDescending(c => c.Fecha_inicio)
                .ToListAsync();

            ViewBag.Contratos = contratos;
            return View(inmueble);
        }

        // GET: Inmuebles/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Propietarios = await _context.Propietarios.ToListAsync();
            ViewBag.TiposInmueble = await _context.TiposInmueble.ToListAsync();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inmueble);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Inmueble creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Propietarios = await _context.Propietarios.ToListAsync();
            ViewBag.TiposInmueble = await _context.TiposInmueble.ToListAsync();
            return View(inmueble);
        }

        // GET: Inmuebles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inmueble = await _context.Inmuebles.FindAsync(id);
            if (inmueble == null) return NotFound();

            ViewBag.Propietarios = await _context.Propietarios.ToListAsync();
            ViewBag.TiposInmueble = await _context.TiposInmueble.ToListAsync();
            return View(inmueble);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inmueble inmueble)
        {
            if (id != inmueble.IdInmuebles) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inmueble);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Inmueble actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InmuebleExists(inmueble.IdInmuebles))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Propietarios = await _context.Propietarios.ToListAsync();
            ViewBag.TiposInmueble = await _context.TiposInmueble.ToListAsync();
            return View(inmueble);
        }

        // GET: Inmuebles/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var inmueble = await _context.Inmuebles
                .Include(i => i.Propietario)
                .Include(i => i.TipoInmueble)
                .FirstOrDefaultAsync(m => m.IdInmuebles == id);

            if (inmueble == null) return NotFound();

            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inmueble = await _context.Inmuebles.FindAsync(id);
            if (inmueble != null)
            {
                _context.Inmuebles.Remove(inmueble);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Inmueble eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InmuebleExists(int id)
        {
            return _context.Inmuebles.Any(e => e.IdInmuebles == id);
        }
    }
}
