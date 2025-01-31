using Microsoft.AspNetCore.Mvc;
using ProyectoWebMVC.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProyectoWebMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace ProyectoWebMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ProyectoWebMVCContext _proyectoWebMVCContext;

        public HomeController(ILogger<HomeController> logger, ProyectoWebMVCContext proyectoWebMVCContext)
        {
            _logger = logger;
            _proyectoWebMVCContext = proyectoWebMVCContext;
        }

        public async Task<IActionResult> Index()
        {
            var publicaciones = await _proyectoWebMVCContext.Publicaciones.ToListAsync();
            return View(publicaciones);
        }


        [HttpGet]
        public IActionResult EnviarSolicitudAdopcion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarSolicitudAdopcion(Solicitud modelo)
        {
            modelo.Estado = "Pendiente";
            if (modelo == null)
            {
                ViewData["Mensaje"] = "No se pudo enviar la solicitud";
            }

            Solicitud solicitud = new Solicitud()
            {
                Nombre = modelo.Nombre,
                Cedula = modelo.Cedula,
                Edad = modelo.Edad,
                LugarResidencia = modelo.LugarResidencia,
                Ingresos = modelo.Ingresos,
                AmbienteFamiliar = modelo.AmbienteFamiliar,
                Fecha = modelo.Fecha
            };

            await _proyectoWebMVCContext.AddAsync(solicitud);
            await _proyectoWebMVCContext.SaveChangesAsync();

            if (solicitud.IdSolicitud != 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "No se pudo enviar la solicitud";
                return View();
            }
        }

        


        [Authorize(Policy = "OnlySpecificUser")]
        public async Task<IActionResult> EvaluarSolicitudAdopcion()
        {
            var solicitudes = await _proyectoWebMVCContext.Solicitudes.ToListAsync();
            return View(solicitudes);
        }

        [Authorize(Policy = "OnlySpecificUser")]
        [HttpGet]
        public IActionResult Publicar()
        {
            return View();
        }

        [Authorize(Policy = "OnlySpecificUser")]
        [HttpPost]
        public async Task<IActionResult> Publicar(Publicacion modelo)
        {
            Publicacion publicacion = new Publicacion
            {
                Raza = modelo.Raza,
                Sexo = modelo.Sexo,
                Edad = modelo.Edad,
                Peso = modelo.Peso,
                Tama�o = modelo.Tama�o,
                Esterilizacion = modelo.Esterilizacion,
                Enfermedades = modelo.Enfermedades,
                Comportamiento = modelo.Comportamiento
            };

            if (modelo.FotoArchivo != null && modelo.FotoArchivo.Length > 0)
            {
                var fileName = Path.GetFileName(modelo.FotoArchivo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas", fileName);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas"));
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await modelo.FotoArchivo.CopyToAsync(stream);
                }

                // Solo guardamos el nombre del archivo en la base de datos
                publicacion.Foto = fileName;
            }

            await _proyectoWebMVCContext.AddAsync(publicacion);
            await _proyectoWebMVCContext.SaveChangesAsync();

            if (publicacion.Id != 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "No se pudo enviar la solicitud";
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            var publicacion = await _proyectoWebMVCContext.Publicaciones.FindAsync(id);
            if (publicacion != null)
            {
                _proyectoWebMVCContext.Publicaciones.Remove(publicacion);
                await _proyectoWebMVCContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditarPublicacion(int id)
        {
            var publicacion = await _proyectoWebMVCContext.Publicaciones.FindAsync(id);
            if (publicacion == null)
            {
                return NotFound();
            }
            return View(publicacion);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditarPublicacion(Publicacion modelo)
        {
            var publicacion = await _proyectoWebMVCContext.Publicaciones.FindAsync(modelo.Id);
            if (publicacion == null)
            {
                return NotFound();
            }

            publicacion.Raza = modelo.Raza;
            publicacion.Sexo = modelo.Sexo;
            publicacion.Edad = modelo.Edad;
            publicacion.Peso = modelo.Peso;
            publicacion.Tama�o = modelo.Tama�o;
            publicacion.Esterilizacion = modelo.Esterilizacion;
            publicacion.Enfermedades = modelo.Enfermedades;
            publicacion.Comportamiento = modelo.Comportamiento;

            if (modelo.FotoArchivo != null && modelo.FotoArchivo.Length > 0)
            {
                var fileName = Path.GetFileName(modelo.FotoArchivo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas", fileName);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas"));
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await modelo.FotoArchivo.CopyToAsync(stream);
                }

                publicacion.Foto = fileName; // Actualiza la foto
            }

            await _proyectoWebMVCContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [Authorize(Policy = "OnlySpecificUser")]
        [HttpPost]
        public async Task<IActionResult> AprobarSolicitud(int id)
        {
            var solicitud = await _proyectoWebMVCContext.Solicitudes.FindAsync(id);
            if (solicitud != null)
            {
                solicitud.Estado = "Aprobada";
                await _proyectoWebMVCContext.SaveChangesAsync();
            }

            return RedirectToAction("EvaluarSolicitudAdopcion");
        }

        [Authorize(Policy = "OnlySpecificUser")]
        [HttpPost]
        public async Task<IActionResult> RechazarSolicitud(int id)
        {
            var solicitud = await _proyectoWebMVCContext.Solicitudes.FindAsync(id);
            if (solicitud != null)
            {
                solicitud.Estado = "Rechazada";
                await _proyectoWebMVCContext.SaveChangesAsync();
            }

            return RedirectToAction("EvaluarSolicitudAdopcion");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EliminarPerfil()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usuario = await _proyectoWebMVCContext.Usuario.FirstOrDefaultAsync(u => u.Correo == userEmail);

            if (usuario == null)
            {
                return NotFound();
            }

            // Elimina todas las solicitudes asociadas
            var solicitudes = _proyectoWebMVCContext.Solicitudes.Where(s => s.Correo == userEmail);
            _proyectoWebMVCContext.Solicitudes.RemoveRange(solicitudes);

            // Elimina el usuario
            _proyectoWebMVCContext.Usuario.Remove(usuario);
            await _proyectoWebMVCContext.SaveChangesAsync();

            // Cierra sesi�n y redirige a la p�gina de login
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
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
}
