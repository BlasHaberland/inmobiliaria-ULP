using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly UsuarioDAO _usuarioDao;

        public UsuarioController(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection");
            _usuarioDao = new UsuarioDAO(connectionString);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Index()
        {
            var usuarios = _usuarioDao.ObtenerTodos();
            return View(usuarios);
        }


        [Authorize(Policy = "Administrador")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]

        [Authorize(Policy = "Administrador")]
        public IActionResult Registro(Usuario usuario, IFormFile? AvatarFile)
        {
            if (ModelState.IsValid)
            {

                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(AvatarFile.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        AvatarFile.CopyTo(stream);
                    }
                    usuario.Avatar = fileName;
                }
                else
                {
                    usuario.Avatar = null;
                }

                usuario.Activo = true; // Por defecto el usuario esta activo al registrarse

                bool exito = _usuarioDao.CrearUsuario(usuario);
                if (exito)
                {
                    TempData["Mensaje"] = "Usuario registrado correctamente.";
                    return RedirectToAction("Index", "Usuario");
                }
                TempData["Error"] = "No se pudo registrar el usuario.";
            }
            return View(usuario);
        }



        public IActionResult Actualizar(int id)
        {
            var usuario = _usuarioDao.ObtenerPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]

        public IActionResult Actualizar(Usuario usuario, IFormFile? AvatarFile)
        {
            // Si es empleado y no es su propio perfil lo bloquea
            if (User.IsInRole("Empleado") && User.FindFirst("Id")?.Value != usuario.Id_Usuario.ToString())
            {
                TempData["Error"] = "No tiene permisos para editar otros perfiles.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {

                var usuarioActual = _usuarioDao.ObtenerPorId(usuario.Id_Usuario);

                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    // elimino la imagen anterior si existe
                    if (!string.IsNullOrEmpty(usuarioActual.Avatar))
                    {
                        var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", usuarioActual.Avatar);
                        if (System.IO.File.Exists(rutaAnterior))
                        {
                            System.IO.File.Delete(rutaAnterior);
                        }
                    }

                    // Guardar la nueva imagen
                    var fileName = Guid.NewGuid() + Path.GetExtension(AvatarFile.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        AvatarFile.CopyTo(stream);
                    }
                    usuario.Avatar = fileName;
                }
                else
                {
                    usuario.Avatar = usuarioActual.Avatar;
                }

                bool exito = _usuarioDao.Actualizar(usuario);
                if (exito)
                {
                    TempData["Mensaje"] = "Usuario actualizado correctamente.";
                    if (User.IsInRole("administrador"))
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(usuario);
        }

    }
}