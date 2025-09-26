using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers;

[Authorize]
public class ContratoController : Controller
{
    private readonly ContratoDAO _contratoDAO;
    private readonly PagoDAO _pagoDAO;
    private readonly InquilinoDAO _inquilinoDAO;
    private readonly TipoInmuebleDAO _tipoInmuebleDAO;
    private readonly InmuebleDAO _inmuebleDAO;

    public ContratoController(IConfiguration configuration)
    {
        _contratoDAO = new ContratoDAO(configuration.GetConnectionString("DefaultConnection"));
        _pagoDAO = new PagoDAO(configuration.GetConnectionString("DefaultConnection"));
        _inquilinoDAO = new InquilinoDAO(configuration.GetConnectionString("DefaultConnection"));
        _tipoInmuebleDAO = new TipoInmuebleDAO(configuration.GetConnectionString("DefaultConnection"));
        _inmuebleDAO = new InmuebleDAO(configuration.GetConnectionString("DefaultConnection"));
    }

    public IActionResult Index(int? id)
    {
        var contratos = _contratoDAO.ObtenerTodos();

        if (id.HasValue && id.Value > 0)
        {
            var filtrados = new List<Contrato>();
            foreach (var contrato in contratos)
            {
                if (contrato.Id_Contrato == id.Value)
                {
                    filtrados.Add(contrato);
                }
            }
            contratos = filtrados;
        }
        return View(contratos);
    }

    public IActionResult Detalle(int id)
    {
        var contrato = _contratoDAO.ObtenerPorId(id);
        var pagos = _pagoDAO.ObtenerPorContrato(id);

        ViewBag.Contrato = contrato;
        ViewBag.Pagos = pagos;

        return View(contrato);
    }


    public IActionResult Crear()
    {
        ViewBag.Inquilinos = _inquilinoDAO.ObtenerTodos();
        ViewBag.TiposInmueble = _tipoInmuebleDAO.ObtenerTodos();
        ViewBag.Inmuebles = _inmuebleDAO.ObtenerDisponibles();
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Contrato contrato)
    {
        var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "Id");
        if (idUsuario == null)
        {
            return RedirectToAction("Login", "Autenticacion");
        }

        contrato.Id_Usuario_Creador = int.Parse(idUsuario.Value);
        if (ModelState.IsValid)
        {
            _contratoDAO.Crear(contrato);
            _inmuebleDAO.CambiarEstado(contrato.Id_Inmueble, "ocupado");
            return RedirectToAction("Index");
        }

        // por si rompe
        ViewBag.Inquilinos = _inquilinoDAO.ObtenerTodos();
        ViewBag.TiposInmueble = _tipoInmuebleDAO.ObtenerTodos();
        ViewBag.Inmuebles = _inmuebleDAO.ObtenerActivos();
        return View(contrato);
    }

    [HttpPost]
    public IActionResult Finalizar(int id)
    {
        var contrato = _contratoDAO.ObtenerPorId(id);
        if (contrato == null || contrato.Estado == "finalizado")
        {
            TempData["Error"] = "No se encontró el contrato o ya fue finalizado.";
            return RedirectToAction("Index");
        }

        var idUsuarioCreador = User.Claims.FirstOrDefault(c => c.Type == "Id");
        if (idUsuarioCreador == null)
        {
            TempData["Error"] = "No se pudo identificar al usuario.";
            return RedirectToAction("Index");
        }


        contrato.Estado = "finalizado";
        contrato.Id_Usuario_Finalizador = int.Parse(idUsuarioCreador.Value);
        _contratoDAO.Actualizar(contrato);

        var inmueble = _inmuebleDAO.ObtenerPorId(contrato.Id_Inmueble);
        if (inmueble != null)
        {
            inmueble.Estado = "disponible";
            _inmuebleDAO.Actualizar(inmueble);
        }
        TempData["Success"] = "Contrato finalizado y el inmueble está disponible.";
        return RedirectToAction("Detalle", new { id = contrato.Id_Contrato });

    }

    [HttpPost]
    public IActionResult Extender(Contrato nuevoContrato)
    {
        var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "Id");
        nuevoContrato.Id_Usuario_Creador = int.Parse(idUsuario.Value);
        nuevoContrato.Estado = "vigente";

        if (ModelState.IsValid)
        {
            _contratoDAO.Crear(nuevoContrato);
            _inmuebleDAO.CambiarEstado(nuevoContrato.Id_Inmueble, "ocupado");
            TempData["Success"] = "Nuevo contrato creado correctamente.";
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Verifique los datos ingresados.";
        return RedirectToAction("Detalle", new { id = nuevoContrato.Id_Inquilino });
    }

}
