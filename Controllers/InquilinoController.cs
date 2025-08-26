using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly InquilinoDAO _inquilinoDao;
        public InquilinoController(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection");
            _inquilinoDao = new InquilinoDAO(connectionString);
        }

        public IActionResult Index()
        {
            var lista = _inquilinoDao.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Eliminar(int id)
        {
            bool exito = _inquilinoDao.eliminarInquilino(id);
            if (exito)
            {
                TempData["Mensaje"] = "Inquilino eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar el inquilino.";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Alta(int id)
        {
            bool exito = _inquilinoDao.altaInquilino(id);
            if (exito)
            {
                TempData["Mensaje"] = "Inquilino dado de alta correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo dar de alta el inquilino.";
            }
            return RedirectToAction("Index");
        }

        //Editar iniquilino
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var inquilino = _inquilinoDao.obtenerPorId(id);
            return View(inquilino);
        }

        [HttpPost]
        public IActionResult Editar(Inquilino inquilino)
        {
            bool exito = _inquilinoDao.actualizarInquilino(inquilino);
            if (exito)
            {
                TempData["Mensaje"] = "Inquilino actualizado correctamente.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "No se pudo actualizar el inquilino.";
            return View(inquilino);

        }

        //Crear inquilino
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Inquilino inquilino)
        {
            bool exito = _inquilinoDao.crearInquilino(inquilino);
            if (exito)
            {
                TempData["Mensaje"] = "Inquilino creado correctamente.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "No se pudo crear el inquilino.";
            return View(inquilino);
        }
    }
}