using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoWebMVC.Data;
using ProyectoWebMVC.Models;
using System.Security.Claims;

namespace ProyectoWebMVC.Controllers
{
    [Authorize(Roles = "Usuario")] // Solo accesible para usuarios normales
    public class UsuarioController : Controller
    {
        private readonly ProyectoWebMVCContext _context;

        public UsuarioController(ProyectoWebMVCContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login", "Acceso");

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Correo == userEmail);
            if (usuario == null)
                return RedirectToAction("Login", "Acceso");

            var solicitudes = await _context.Solicitudes
                .Where(s => s.Cedula == usuario.Cedula)
                .ToListAsync();

            ViewBag.Solicitudes = solicitudes;
            return View(usuario);
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditarPerfil(Usuario modelo, IFormFile? fotoPerfil)
        {
            var usuario = await _context.Usuario.FindAsync(modelo.Id);
            if (usuario == null)
                return NotFound();

            usuario.Nombre = modelo.Nombre;
            usuario.Edad = modelo.Edad;
            usuario.Cedula = modelo.Cedula;
            usuario.LugarResidencia = modelo.LugarResidencia;
            usuario.AmbienteFamiliar = modelo.AmbienteFamiliar;

            if (fotoPerfil != null)
            {
                string fileName = Path.GetFileName(fotoPerfil.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", fileName);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads"));
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fotoPerfil.CopyToAsync(stream);
                }

                usuario.FotoPerfilPath = fileName;
            }

            _context.Update(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Perfil");
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EliminarPerfil(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Logout", "Acceso");
        }


    }
}
