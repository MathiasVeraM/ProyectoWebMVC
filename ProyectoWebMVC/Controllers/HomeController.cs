using Microsoft.AspNetCore.Mvc;
using ProyectoWebMVC.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProyectoWebMVC.Data;

namespace ProyectoWebMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ProyectoWebMVCContext _proyectoWebMVCContext;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public HomeController(ProyectoWebMVCContext proyectoWebMVCContext)
        {
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
                ViewData["Mensaje"] = "No se pudo crear el usuario";
                return View();
            }
        }

        [Authorize(Policy = "OnlySpecificUser")]
        public IActionResult EvaluarSolicitudAdopcion()
        {
            return View();
        }

        [Authorize(Policy = "OnlySpecificUser")]
        public IActionResult Publicar()
        {
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
}
