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

        [Authorize(Policy = "Administrador")]
        public IActionResult Crear()

        {
            var propietarios = _propietarioDao.obtenerTodos();
            var tipos = _tipoInmuebleDao.ObtenerTodos();
            ViewBag.Tipos = tipos;
            ViewBag.Propietarios = propietarios;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public IActionResult Crear(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                _inmuebleDao.Crear(inmueble);
                return RedirectToAction("Index");
            }

            var propietarios = _propietarioDao.obtenerTodos();
            var tipos = _tipoInmuebleDao.ObtenerTodos();
            ViewBag.Tipos = tipos;
            ViewBag.Propietarios = propietarios;
            return View(inmueble);
        }


    }
}