using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly PagoDAO _pagoDao;
        private readonly ContratoDAO _contratoDao;
        public PagoController(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection");
            _pagoDao = new PagoDAO(connectionString);
            _contratoDao = new ContratoDAO(connectionString);
        }

        [HttpPost]
        public IActionResult Pagar(int id, string detalle)
        {
            var pago = _pagoDao.ObtenerPorId(id);
            if (pago == null || pago.Estado != "pendiente")
            {
                TempData["Error"] = "No se encontró el pago o ya fue realizado/anulado.";
                return RedirectToAction("Detalle", "Contrato", new { id = pago.Id_Contrato });
            }

            pago.Estado = "realizado";
            pago.Fecha_Pago = DateTime.Now;
            pago.Detalle = detalle;
            _pagoDao.Actualizar(pago);

            var pagos = _pagoDao.ObtenerPorContrato(pago.Id_Contrato);

            //Verifico que sea el ultiomo pago
            bool esUltimoPago = pago.Numero_Pago == pagos.Max(p => p.Numero_Pago);

            if (esUltimoPago)
            {
                TempData["UltimoPago"] = true;
                var contrato = _contratoDao.ObtenerPorId(pago.Id_Contrato);
                if (contrato != null)
                {
                    contrato.Estado = "finalizado";
                    _contratoDao.Actualizar(contrato);
                }
            }
            return RedirectToAction("Detalle", "Contrato", new { id = pago.Id_Contrato });
        }

        [HttpPost]
        public IActionResult Anular(int id)
        {
            var pago = _pagoDao.ObtenerPorId(id);
            if (pago == null || pago.Estado != "pendiente")
            {
                TempData["Error"] = "No se encontró el pago o ya fue realizado/anulado.";
                return RedirectToAction("Detalle", "Contrato", new { id = pago.Id_Contrato });
            }

            var idUsuarioCreador = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (idUsuarioCreador == null)
            {
                TempData["Error"] = "No se pudo identificar al usuario.";
                return RedirectToAction("Detalle", "Contrato", new { id = pago.Id_Contrato });
            }


            pago.Estado = "anulado";
            pago.Fecha_Pago = DateTime.Now;
            pago.Id_Usuario_Anulador = int.Parse(idUsuarioCreador.Value);
            pago.Detalle = "Pago anulado";
            _pagoDao.Actualizar(pago);
            return RedirectToAction("Detalle", "Contrato", new { id = pago.Id_Contrato });
        }
    }
}

