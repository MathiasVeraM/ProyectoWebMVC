using Microsoft.AspNetCore.Mvc;
using ProyectoWebMVC.Models;
using ProyectoWebMVC.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ProyectoWebMVC.Controllers
{
    public class AccesoController : Controller
    {
        private readonly ProyectoWebMVCContext _proyectoWebMVCContext;

        public AccesoController(ProyectoWebMVCContext proyectoWebMVCContext)
        {
            _proyectoWebMVCContext = proyectoWebMVCContext;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            if (modelo == null)
            {
                ViewData["Mensaje"] = "No se pudo crear el usuario";
            }

            Usuario usuario = new Usuario()
            {
                Nombre = modelo.Nombre,
                Correo = modelo.Correo,
                Clave  = modelo.Clave,
            };

            await _proyectoWebMVCContext.AddAsync(usuario);
            await _proyectoWebMVCContext.SaveChangesAsync();

            if(usuario.Id != 0)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewData["Mensaje"] = "No se pudo crear el usuario";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login modelo)
        {
            Usuario? usuario_encontrado = await _proyectoWebMVCContext.Usuario.Where(u=>u.Correo == modelo.Correo && u.Clave == modelo.Clave).FirstOrDefaultAsync();
            
            if(usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontro el usuario";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, usuario_encontrado.Correo),
            };


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Acceso");
        }
    }
}
