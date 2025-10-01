using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly InmuebleDAO _inmuebleDao;
        private readonly PropietarioDAO _propietarioDao;
        private readonly TipoInmuebleDAO _tipoInmuebleDao;

        public InmuebleController(IConfiguration config)
        {
            String connectionString = config.GetConnectionString("DefaultConnection");
            _inmuebleDao = new InmuebleDAO(connectionString);
            _propietarioDao = new PropietarioDAO(connectionString);
            _tipoInmuebleDao = new TipoInmuebleDAO(connectionString);
        }

        public IActionResult Index()
        {
            var inmuebles = _inmuebleDao.ObtenerTodos();
            return View(inmuebles);
        }

        [HttpGet]
        public IActionResult InmueblePropietario(int id)
        {
            var inmuebles = _inmuebleDao.ObtenerPorIdPropietario(id);
            ViewBag.IdPropietario = id;
            return View("Index", inmuebles);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Crear()

        {
            var propietarios = _propietarioDao.obtenerActivos();
            var tipos = _tipoInmuebleDao.ObtenerTodos();
            ViewBag.Tipos = tipos;
            ViewBag.Propietarios = propietarios;
            return View();

        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Crear(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                _inmuebleDao.Crear(inmueble);
                return RedirectToAction("Index");
            }

            var propietarios = _propietarioDao.obtenerActivos();
            var tipos = _tipoInmuebleDao.ObtenerTodos();
            ViewBag.Tipos = tipos;
            ViewBag.Propietarios = propietarios;
            return View(inmueble);
        }




        public IActionResult Editar(int id)
        {
            var inmueble = _inmuebleDao.ObtenerPorId(id);
            var propietarios = _propietarioDao.obtenerActivos();
            var tipos = _tipoInmuebleDao.ObtenerTodos();
            ViewBag.Tipos = tipos;
            ViewBag.Propietarios = propietarios;
            return View(inmueble);
        }


        [HttpPost]
        public IActionResult Editar(Inmueble inmueble)
        {
            bool exito = _inmuebleDao.Actualizar(inmueble);
            if (exito)
            {
                TempData["Mensaje"] = "Inmueble actualizado correctamente.";
                return RedirectToAction("Index");
            }

            var propietarios = _propietarioDao.obtenerActivos();
            var tipos = _tipoInmuebleDao.ObtenerTodos();
            ViewBag.Tipos = tipos;
            ViewBag.Propietarios = propietarios;

            TempData["Error"] = "No se pudo actualizar el inmueble.";
            return View(inmueble);
        }


        [Authorize("administrador")]
        public IActionResult Eliminar(int id)
        {
            bool exito = _inmuebleDao.Eliminar(id);
            if (exito)
            {
                TempData["Mensaje"] = "Inmueble eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar el inmueble.";
            }
            return RedirectToAction("Index");
        }

        [Authorize("administrador")]
        public IActionResult Alta(int id)
        {
            bool exito = _inmuebleDao.Alta(id);
            if (exito)
            {
                TempData["Mensaje"] = "Inmueble dado de alta correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo dar de alta el inmueble.";
            }
            return RedirectToAction("Index");
        }

    }
}