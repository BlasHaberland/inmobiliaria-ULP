using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioDAO _usuarioDao;

        public UsuarioController(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection");
            _usuarioDao = new UsuarioDAO(connectionString);
        }


        [Authorize(Policy = "Administrador")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]

        [Authorize(Policy = "Administrador")]
        public IActionResult Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {

                usuario.Activo = true; // Por defecto, el usuario se crea activo
                usuario.Avatar = "/avatars/default.jpg";

                bool exito = _usuarioDao.CrearUsuario(usuario);
                if (exito)
                {
                    TempData["Mensaje"] = "Usuario registrado correctamente.";
                    return RedirectToAction("Login", "Autenticacion");
                }
                TempData["Error"] = "No se pudo registrar el usuario.";
            }
            return View(usuario);
        }
    }
}