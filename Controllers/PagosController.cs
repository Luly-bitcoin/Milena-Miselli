using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laboratorio.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Laboratorio.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly InmobiliariaDbContext _context;

        public PagosController(InmobiliariaDbContext context)
        {
            _context = context;
        }

        // GET: Pagos por Contrato
        public async Task<IActionResult> PorContrato(int contratoId)
        {
            var pagos = await _context.Pagos
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Inquilino)
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Inmueble)
                .Include(p => p.UsuarioCrea)
                .Include(p => p.UsuarioAnula)
                .Where(p => p.IdContrato == contratoId)
                .OrderBy(p => p.Fecha_pago)
                .ToListAsync();

            ViewBag.Contrato = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
                .FirstOrDefaultAsync(c => c.IdContratos == contratoId);

            return View("Index", pagos);
        }

        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pago = await _context.Pagos
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Inquilino)
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Inmueble)
                .Include(p => p.UsuarioCrea)
                .Include(p => p.UsuarioAnula)
                .FirstOrDefaultAsync(m => m.IdPagos == id);
            
            if (pago == null) return NotFound();

            return View(pago);
        }

        // GET: Pagos/Create
        public async Task<IActionResult> Create(int contratoId)
        {
            var contrato = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
                .FirstOrDefaultAsync(c => c.IdContratos == contratoId);

            if (contrato == null) return NotFound();

            // Obtener el siguiente número de pago
            var ultimoPago = await _context.Pagos
                .Where(p => p.IdContrato == contratoId)
                .OrderByDescending(p => p.Numero_pago)
                .FirstOrDefaultAsync();

            var nuevoPago = new Pagos
            {
                IdContrato = contratoId,
                Numero_pago = (ultimoPago?.Numero_pago ?? 0) + 1,
                Fecha_pago = DateTime.Now,
                Importe = contrato.Monto_mensual,
                Concepto = "Alquiler mensual",
                Estado = "Activo"
            };

            ViewBag.Contrato = contrato;
            return View(nuevoPago);
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pagos pago)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                pago.Id_usuario_crea = userId;
                pago.Estado = "Activo";

                _context.Add(pago);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Pago registrado exitosamente.";
                return RedirectToAction("PorContrato", new { contratoId = pago.IdContrato });
            }

            var contrato = await _context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
                .FirstOrDefaultAsync(c => c.IdContratos == pago.IdContrato);
            ViewBag.Contrato = contrato;
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pago = await _context.Pagos
                .Include(p => p.Contrato)
                .FirstOrDefaultAsync(p => p.IdPagos == id);
            
            if (pago == null) return NotFound();

            return View(pago);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pagos pago)
        {
            if (id != pago.IdPagos) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Pago actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.IdPagos)) return NotFound();
                    else throw;
                }
                return RedirectToAction("PorContrato", new { contratoId = pago.IdContrato });
            }
            return View(pago);
        }

        // GET: Pagos/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pago = await _context.Pagos
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Inquilino)
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Inmueble)
                .FirstOrDefaultAsync(m => m.IdPagos == id);
            
            if (pago == null) return NotFound();

            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Pago eliminado exitosamente.";
            }
            return RedirectToAction("PorContrato", new { contratoId = pago?.IdContrato });
        }

        // POST: Anular Pago
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Anular(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();

            var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            pago.Estado = "Anulado";
            pago.Id_usuario_anula = userId;

            _context.Update(pago);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Pago anulado exitosamente.";
            return RedirectToAction("PorContrato", new { contratoId = pago.IdContrato });
        }

        // POST: Pagar Multa
        [HttpPost]
        public async Task<IActionResult> PagarMulta(int contratoId, decimal monto, string concepto)
        {
            var contrato = await _context.Contratos.FindAsync(contratoId);
            if (contrato == null) return NotFound();

            // Obtener el siguiente número de pago
            var ultimoPago = await _context.Pagos
                .Where(p => p.IdContrato == contratoId)
                .OrderByDescending(p => p.Numero_pago)
                .FirstOrDefaultAsync();

            var pagoMulta = new Pagos
            {
                IdContrato = contratoId,
                Numero_pago = (ultimoPago?.Numero_pago ?? 0) + 1,
                Fecha_pago = DateTime.Now,
                Importe = monto,
                Concepto = concepto,
                Estado = "Activo",
                Id_usuario_crea = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0")
            };

            _context.Add(pagoMulta);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Pago de multa registrado exitosamente.";
            return RedirectToAction("PorContrato", new { contratoId });
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.IdPagos == id);
        }
    }
}
