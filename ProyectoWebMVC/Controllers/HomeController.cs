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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EnviarSolicitudAdopcion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarSolicitudAdopcion(Solicitud modelo)
        {
            if(modelo == null)
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

        
        public IActionResult Publicar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Publicar(Publicacion publicacion, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null && foto.Length > 0)
                {
                    var fileName = Path.GetFileName(foto.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas", fileName);


                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas")))
                    {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Subidas"));
                    }


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }


                    publicacion.Foto = fileName;
                }


                _proyectoWebMVCContext.Publicaciones.Add(publicacion);
                await _proyectoWebMVCContext.SaveChangesAsync();


                return RedirectToAction("Index");
            }


            return View(publicacion);
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
