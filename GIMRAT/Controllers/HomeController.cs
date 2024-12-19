using Gimrat.Data;
using Gimrat.Entidades;
using GIMRAT.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GIMRAT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ResumenData _resumenData;

        public HomeController(ILogger<HomeController> logger, ResumenData resumenData)
        {
            _logger = logger;
            _resumenData = resumenData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen()
        {
            try
            {
                // Obtener datos del resumen
                (int activas, int inactivas) = await _resumenData.ContarSuscripcionesPorEstadoAsync();
                double gananciasAnio = await _resumenData.ObtenerGananciasAnioActualAsync();
                double gananciasMes = await _resumenData.ObtenerGananciasMesActualAsync();
                double egresosMes = await _resumenData.ObtenerEgresosMesActualAsync();
                double egresosAnio = await _resumenData.ObtenerEgresosAnioActualAsync();

                var gananciasMensuales = await _resumenData.ObtenerGananciasPorMesAsync();

                var egresosMensuales = await _resumenData.ObtenerEgresosPorMesAsync();

                (int hombres, int mujeres, int otros) = await _resumenData.CargarResumenGeneroAsync();

                // Calcular el total de clientes
                int totalClientes = activas + inactivas;

                // Crear objeto Resumen con los datos obtenidos
                var resumen = new Resumen
                {
                    TotalClientes = totalClientes.ToString(),
                    SuscripcionesActivas = activas.ToString(),
                    SuscripcionesInactivas = inactivas.ToString(),
                    TotalMes = Convert.ToInt32(gananciasMes),
                    TotalAnio = Convert.ToInt32(gananciasAnio),
                    TotalMesEgreso = Convert.ToInt32(egresosMes),
                    TotalAnioEgreso = Convert.ToInt32(egresosAnio),
                    Hombres = hombres,
                    Mujeres = mujeres,
                    Otros = otros,
                    GananciasMensuales = gananciasMensuales,
                    EgresosMensuales = egresosMensuales


                };

                return StatusCode(StatusCodes.Status200OK, new { data = resumen });
            }
            catch (Exception ex)
            {
                // Manejar la excepción (registrarla con _logger, etc.)
                _logger.LogError(ex, "Error al obtener el resumen.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al obtener el resumen." });
            }
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    






            [HttpGet]
        public IActionResult ObtenerFechaActual()
        {
            // Obtener la fecha y hora actual del servidor
            DateTime fechaActual = DateTime.Now;

            // Devolver la fecha en formato ISO 8601
            return Ok(new { data = fechaActual.ToString("yyyy-MM-dd") });
        }









        [HttpPost]
        public async Task<IActionResult> ObtenerNotificacionesPorFecha([FromBody] string fechaActual)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fechaActual))
                {
                    return BadRequest(new { mensaje = "La fecha enviada no es válida." });
                }

                Console.WriteLine($"Fecha recibida en el controlador: {fechaActual}");

                // Llamada al servicio para obtener la lista de notificaciones
                List<notificacion> notificaciones = await _resumenData.Lista(fechaActual);

                if (notificaciones == null || notificaciones.Count == 0)
                {
                    return Ok(new { mensaje = "No se encontraron notificaciones para la fecha proporcionada." });
                }

                // Retornar la lista como respuesta JSON
                return Ok(notificaciones);
            }
            catch (Exception ex)
            {
                // Manejar excepciones y retornar error con detalles
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener las notificaciones.", detalle = ex.Message });
            }
        }

    }
}



