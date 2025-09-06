using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace inmobiliaria.Controllers
{
    public class AutenticacionController : Controller
    {
        private readonly UsuarioDAO _usuarioDAO;

        public AutenticacionController(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection");
            _usuarioDAO = new UsuarioDAO(connectionString);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var usuarioDb = _usuarioDAO.Login(usuario.Email, usuario.Contrasena);
            if (usuarioDb != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarioDb.Nombre),
                    new Claim(ClaimTypes.Role, usuarioDb.Rol),
                    new Claim("Id", usuarioDb.Id_Usuario.ToString()),
                    new Claim(ClaimTypes.GivenName, usuarioDb.Nombre),
                    new Claim(ClaimTypes.Surname, usuarioDb.Apellido),
                    new Claim("Avatar", usuarioDb.Avatar ?? "")
                };
                var claimsIdentity = new ClaimsIdentity(claims, "authCookie");
                await HttpContext.SignInAsync("authCookie", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Mensaje = "Login inválido";
            return View();
        }
    }
}