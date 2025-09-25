using inmobiliaria.DAO;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    public class TestController : Controller
    {
        private readonly ContratoDAO _contratoDAO;
        public TestController(IConfiguration configuration)
        {
            _contratoDAO = new ContratoDAO(configuration.GetConnectionString("DefaultConnection"));
        }
        public IActionResult TestCrearContrato()
        {
            try
            {
                var contrato = new Contrato
                {
                    Id_Inquilino = 1,
                    Id_Inmueble = 2,
                    Fecha_Inicio = new DateTime(2025, 10, 1),
                    Fecha_Fin_Original = new DateTime(2026, 3, 1),
                    Monto_Mensual = 50000,
                    Id_Usuario_Creador = 8,
                    Estado = "vigente",
                    Multa = 0
                };
                bool resultado = _contratoDAO.Crear(contrato);
                return Content(resultado ? "Contrato y pagos creados correctamente" : "Error al crear contrato " + resultado);
            }
            catch (Exception ex)
            {
                return Content("Excepción: " + ex.Message);
            }
        }
    }
}