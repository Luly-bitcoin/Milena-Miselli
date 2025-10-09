using Laboratorio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Laboratorio.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly InmobiliariaDbContext _context;

        public UsuariosController(InmobiliariaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string contrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Contrasena == contrasena && u.Estado == "Activo");

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            // Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("IdUsuario", usuario.IdUsuarios.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Estado = "Activo";
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Usuario creado exitosamente.";
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Usuario actualizado exitosamente.";
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Estado = "Inactivo";
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Usuario eliminado exitosamente.";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CambiarContrasena(string contrasenaActual, string nuevaContrasena)
        {
            var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null || usuario.Contrasena != contrasenaActual)
            {
                TempData["Error"] = "La contraseña actual es incorrecta.";
                return RedirectToAction("Perfil");
            }

            usuario.Contrasena = nuevaContrasena;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Contraseña cambiada exitosamente.";
            return RedirectToAction("Perfil");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CambiarAvatar(IFormFile avatar)
        {
            var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null) return NotFound();

            if (avatar != null && avatar.Length > 0)
            {
                var fileName = $"avatar_{userId}_{DateTime.Now.Ticks}.jpg";
                var path = Path.Combine("wwwroot/images/avatars", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                usuario.Avatar = $"/images/avatars/{fileName}";
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Avatar actualizado exitosamente.";
            }

            return RedirectToAction("Perfil");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> QuitarAvatar()
        {
            var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null) return NotFound();

            usuario.Avatar = null;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Avatar eliminado exitosamente.";

            return RedirectToAction("Perfil");
        }
    }
}
