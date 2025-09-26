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

    [HttpPost]
    public IActionResult Rescindir(int id)
    {
        var contrato = _contratoDAO.ObtenerPorId(id);
        if (contrato == null || contrato.Estado == "finalizado" || contrato.Estado == "rescindido")
        {
            TempData["Error"] = "No se encontró el contrato o ya fue finalizado/rescindido.";
            return RedirectToAction("Index");
        }


        //Callculo fechas
        DateTime fechaHoy = DateTime.Now;
        var fechaInicio = contrato.Fecha_Inicio;
        var fechaFin = contrato.Fecha_Fin_Original;
        var mesesTotales = ((fechaFin.Year - fechaInicio.Year) * 12) + fechaFin.Month - fechaInicio.Month;
        var mesesTranscurridos = ((fechaHoy.Year - fechaInicio.Year) * 12) + fechaHoy.Month - fechaInicio.Month;



        //Calculo multa
        //pueden ser 1 o 2
        //ej: mesesTranscurridos = 5 y mesesTotales = 12 ==> se le asignan 2 meses de multa
        int mesesMulta = mesesTranscurridos < (mesesTotales / 2) ? 2 : 1;
        decimal multa = mesesMulta * contrato.Monto_Mensual;

        var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "Id");

        var pagosPendientes = _pagoDAO.ObtenerPorContrato(contrato.Id_Contrato);
        foreach (var pago in pagosPendientes)
        {
            pago.Estado = "anulado";
            pago.Fecha_Pago = fechaHoy;
            pago.Id_Usuario_Anulador = int.Parse(idUsuario.Value);
            pago.Detalle = "anulado por rescisión de contrato";
            _pagoDAO.Actualizar(pago);
        }

        //Actualizo contrato
        contrato.Estado = "rescindido";
        contrato.Fecha_Fin_Anticipada = fechaHoy;
        contrato.Multa = multa;
        contrato.Id_Usuario_Finalizador = int.Parse(idUsuario.Value);
        _contratoDAO.Actualizar(contrato);

        //creo el pago de la multa
        var pagos = _pagoDAO.ObtenerPorContrato(contrato.Id_Contrato);
        Console.WriteLine("multa: " + multa);
        var pagoMulta = new Pago
        {
            Id_Contrato = contrato.Id_Contrato,
            Numero_Pago = pagosPendientes.Count + 1,
            Fecha_Vencimiento = fechaHoy,
            Fecha_Pago = fechaHoy,
            Detalle = "Multa por rescisión anticipada",
            Importe = multa,
            Estado = "pendiente",
            Id_Usuario_Creador = int.Parse(idUsuario.Value)
        };

        _pagoDAO.AgregarPago(pagoMulta);
        return RedirectToAction("Detalle", new { id = contrato.Id_Contrato });
    }

}
