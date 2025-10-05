using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Laboratorio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly InmobiliariaDbContext _context;

    public HomeController(ILogger<HomeController> logger, InmobiliariaDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Estadísticas para el dashboard
        var estadisticas = new
        {
            TotalPropietarios = await _context.Propietarios.CountAsync(),
            TotalInquilinos = await _context.Inquilinos.CountAsync(),
            TotalInmuebles = await _context.Inmuebles.CountAsync(),
            InmueblesDisponibles = await _context.Inmuebles.CountAsync(i => i.Estado == "Disponible"),
            ContratosVigentes = await _context.Contratos.CountAsync(c => c.Fecha_inicio <= DateTime.Now && c.Fecha_fin >= DateTime.Now),
            TotalPagos = await _context.Pagos.CountAsync(p => p.Estado == "Activo")
        };

        ViewBag.Estadisticas = estadisticas;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
