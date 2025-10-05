using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laboratorio.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Laboratorio.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        private readonly InmobiliariaDbContext _context;

        public ContratosController(InmobiliariaDbContext context)
        {
            _context = context;
        }

        // GET: Contratos
        public async Task<IActionResult> Index()
        {
            var contratos = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
                .ToListAsync();
            return View(contratos);
        }

        // GET: Contratos Vigentes
        public async Task<IActionResult> Vigentes()
        {
            var fechaActual = DateTime.Now;
            var contratosVigentes = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
                .Where(c => c.Fecha_inicio <= fechaActual && c.Fecha_fin >= fechaActual)
                .ToListAsync();
            return View("Index", contratosVigentes);
        }

        // GET: Contratos por Inmueble
        public async Task<IActionResult> PorInmueble(int inmuebleId)
        {
            var contratos = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
                .Where(c => c.IdInmueble == inmuebleId)
                .ToListAsync();
            return View("Index", contratos);
        }

        // GET: Contratos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var contrato = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
                .Include(c => c.UsuarioCrea)
                .Include(c => c.UsuarioTermina)
                .FirstOrDefaultAsync(m => m.IdContratos == id);
            
            if (contrato == null) return NotFound();

            return View(contrato);
        }

        // GET: Contratos/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Inquilinos = await _context.Inquilinos.ToListAsync();
            ViewBag.Inmuebles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .Where(i => i.Estado == "Disponible")
                .ToListAsync();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                // Validar superposición de fechas
                var superposicion = await _context.Contratos
                    .AnyAsync(c => c.IdInmueble == contrato.IdInmueble &&
                        ((c.Fecha_inicio <= contrato.Fecha_inicio && c.Fecha_fin >= contrato.Fecha_inicio) ||
                         (c.Fecha_inicio <= contrato.Fecha_fin && c.Fecha_fin >= contrato.Fecha_fin) ||
                         (c.Fecha_inicio >= contrato.Fecha_inicio && c.Fecha_fin <= contrato.Fecha_fin)));

                if (superposicion)
                {
                    ModelState.AddModelError("", "El inmueble ya tiene un contrato activo en esas fechas.");
                    ViewBag.Inquilinos = await _context.Inquilinos.ToListAsync();
                    ViewBag.Inmuebles = await _context.Inmuebles
                        .Include(i => i.Propietario)
                        .Where(i => i.Estado == "Disponible")
                        .ToListAsync();
                    return View(contrato);
                }

                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                contrato.IdUsuario_crea = userId;
                contrato.Fecha_fin_original = contrato.Fecha_fin;

                _context.Add(contrato);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Contrato creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Inquilinos = await _context.Inquilinos.ToListAsync();
            ViewBag.Inmuebles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .Where(i => i.Estado == "Disponible")
                .ToListAsync();
            return View(contrato);
        }

        // GET: Contratos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null) return NotFound();

            ViewBag.Inquilinos = await _context.Inquilinos.ToListAsync();
            ViewBag.Inmuebles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .ToListAsync();
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contrato contrato)
        {
            if (id != contrato.IdContratos) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Validar superposición de fechas (excluyendo el contrato actual)
                    var superposicion = await _context.Contratos
                        .AnyAsync(c => c.IdInmueble == contrato.IdInmueble && c.IdContratos != contrato.IdContratos &&
                            ((c.Fecha_inicio <= contrato.Fecha_inicio && c.Fecha_fin >= contrato.Fecha_inicio) ||
                             (c.Fecha_inicio <= contrato.Fecha_fin && c.Fecha_fin >= contrato.Fecha_fin) ||
                             (c.Fecha_inicio >= contrato.Fecha_inicio && c.Fecha_fin <= contrato.Fecha_fin)));

                    if (superposicion)
                    {
                        ModelState.AddModelError("", "El inmueble ya tiene un contrato activo en esas fechas.");
                        ViewBag.Inquilinos = await _context.Inquilinos.ToListAsync();
                        ViewBag.Inmuebles = await _context.Inmuebles
                            .Include(i => i.Propietario)
                            .ToListAsync();
                        return View(contrato);
                    }

                    _context.Update(contrato);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Contrato actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContratoExists(contrato.IdContratos)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Inquilinos = await _context.Inquilinos.ToListAsync();
            ViewBag.Inmuebles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .ToListAsync();
            return View(contrato);
        }

        // GET: Contratos/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contrato = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
                .FirstOrDefaultAsync(m => m.IdContratos == id);
            if (contrato == null) return NotFound();

            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato != null)
            {
                _context.Contratos.Remove(contrato);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Contrato eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Renovar Contrato
        public async Task<IActionResult> Renovar(int? id)
        {
            if (id == null) return NotFound();

            var contratoOriginal = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .FirstOrDefaultAsync(c => c.IdContratos == id);
            
            if (contratoOriginal == null) return NotFound();

            // Crear nuevo contrato pre-cargado
            var nuevoContrato = new Contrato
            {
                IdInquilino = contratoOriginal.IdInquilino,
                IdInmueble = contratoOriginal.IdInmueble,
                Monto_mensual = contratoOriginal.Monto_mensual,
                Fecha_inicio = contratoOriginal.Fecha_fin.AddDays(1),
                Fecha_fin = contratoOriginal.Fecha_fin.AddDays(1).AddYears(1) // 1 año por defecto
            };

            ViewBag.Inquilinos = await _context.Inquilinos.ToListAsync();
            ViewBag.Inmuebles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .ToListAsync();
            ViewBag.ContratoOriginal = contratoOriginal;

            return View("Create", nuevoContrato);
        }

        // POST: Terminar Contrato Anticipadamente
        [HttpPost]
        public async Task<IActionResult> TerminarAnticipadamente(int id, decimal multa)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null) return NotFound();

            var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            contrato.Fecha_termina_anticipada = DateTime.Now;
            contrato.Multa = multa;
            contrato.IdUsuario_termina = userId;

            _context.Update(contrato);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Contrato terminado anticipadamente. Multa registrada.";
            return RedirectToAction("Details", new { id });
        }

        // GET: Buscar Inmuebles Disponibles
        public async Task<IActionResult> BuscarDisponibles(DateTime fechaInicio, DateTime fechaFin)
        {
            var inmueblesDisponibles = await _context.Inmuebles
                .Include(i => i.Propietario)
                .Include(i => i.TipoInmueble)
                .Where(i => i.Estado == "Disponible" && 
                    !_context.Contratos.Any(c => c.IdInmueble == i.IdInmuebles &&
                        ((c.Fecha_inicio <= fechaInicio && c.Fecha_fin >= fechaInicio) ||
                         (c.Fecha_inicio <= fechaFin && c.Fecha_fin >= fechaFin) ||
                         (c.Fecha_inicio >= fechaInicio && c.Fecha_fin <= fechaFin))))
                .ToListAsync();

            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            return View(inmueblesDisponibles);
        }

        private bool ContratoExists(int id)
        {
            return _context.Contratos.Any(e => e.IdContratos == id);
        }
    }
}
