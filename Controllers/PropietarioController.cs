using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private readonly PropietarioDAO _propietarioDao;
        public PropietarioController(IConfiguration config)
        {
            //string connectionString = "server=localhost;database=inmobiliaria;user=root;password=;";
            string connectionString = config.GetConnectionString("DefaultConnection");
            _propietarioDao = new PropietarioDAO(connectionString);
        }
        public IActionResult Index()
        {
            var lista = _propietarioDao.obtenerTodos();
            return View(lista);
        }

        //TODO: crear un metodo Destruir() para eliminar un proipietario de la faz de la base de datos
        [Authorize("administrador")]
        public IActionResult Eliminar(int id)

        {
            bool exito = _propietarioDao.eliminarPropietario(id);
            if (exito)
            {
                TempData["Mensaje"] = "Propietario eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar el propietario.";
            }
            return RedirectToAction("Index");
        }
        [Authorize("administrador")]
        public IActionResult Alta(int id)
        {
            bool exito = _propietarioDao.altaPropietario(id);
            if (exito)
            {
                TempData["Mensaje"] = "Propietario dado de alta correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo dar de alta el propietario.";
            }
            return RedirectToAction("Index");
        }

        // Modificar un propietario
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var propietario = _propietarioDao.obtenerPorId(id);
            return View(propietario);
        }

        [HttpPost]
        public IActionResult Editar(Propietario propietario)
        {
            bool exito = _propietarioDao.actualizarPropietario(propietario);
            if (exito)
            {
                TempData["Mensaje"] = "Propietario actualizado correctamente.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "No se pudo actualizar el propietario.";
            return View(propietario);

        }

        // Crear un propietario
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Propietario propietario)
        {
            bool exito = _propietarioDao.crearPropietario(propietario);
            if (exito)
            {
                TempData["Mensaje"] = "Propietario creado correctamente.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "No se pudo crear el propietario.";
            return View(propietario);
        }
    }
}