using Microsoft.AspNetCore.Mvc;
using ProyectoWebMVC.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProyectoWebMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EnviarSolicitudAdopcion()
        {
            return View();
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
