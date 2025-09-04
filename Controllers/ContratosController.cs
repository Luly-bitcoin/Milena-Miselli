using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laboratorio.Models;

namespace Laboratorio.Controllers
{
    public class ContratosController : Controller
    {
        private readonly LaboratorioContext _context;

        public ContratosController(LaboratorioContext context)
        {
            _context = context;
        }

        // GET: Contratos
        public async Task<IActionResult> Index()
        {
            var contratos = await _context.Contratos.ToListAsync();
            return View(contratos);
        }

        // GET: Contratos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var contrato = await _context.Contratos.FirstOrDefaultAsync(m => m.IdContratos == id);
            if (contrato == null) return NotFound();

            return View(contrato);
        }

        // GET: Contratos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdContratos,IdInquilino,IdInmueble,IdUsuario_crea,IdUsuario_termina,Monto_mensual,Fecha_inicio,Fecha_fin,Fecha_fin_original,Fecha_termina_anticipada,Multa")] Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contrato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contrato);
        }

        // GET: Contratos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null) return NotFound();

            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdContratos,IdInquilino,IdInmueble,IdUsuario_crea,IdUsuario_termina,Monto_mensual,Fecha_inicio,Fecha_fin,Fecha_fin_original,Fecha_termina_anticipada,Multa")] Contrato contrato)
        {
            if (id != contrato.IdContratos) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contrato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContratoExists(contrato.IdContratos)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contrato);
        }

        // GET: Contratos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contrato = await _context.Contratos.FirstOrDefaultAsync(m => m.IdContratos == id);
            if (contrato == null) return NotFound();

            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato != null)
            {
                _context.Contratos.Remove(contrato);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ContratoExists(int id)
        {
            return _context.Contratos.Any(e => e.IdContratos == id);
        }
    }
}
