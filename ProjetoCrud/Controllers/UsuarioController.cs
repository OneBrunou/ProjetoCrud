using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjetoCrud.Models;
using ProjetoCrud.Repositorio;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ProjetoCrud.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        [HttpGet]
        public IActionResult Logar() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logar(Usuario model)
        {
            if (!ModelState.IsValid) return View(model);
            var usuario = _usuarioRepositorio.Validar(model.Email, model.Senha);
            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,usuario.Nome),
                    new Claim(ClaimTypes.Email,usuario.Email),
                    new Claim("NivelAcesso",usuario.Nivel),
                    new Claim("UsuarioId",usuario.Id.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = false });
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "E-mail ou senha invalidos.");
            return View(model);
        }
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Logar");
        }

        [HttpGet]
        public IActionResult CriarConta() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarConta(LoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                _usuarioRepositorio.CriarConta(usuario);
                return RedirectToAction("Logar");
            }

            return View(usuario);
        }
            
        
    }
}
