using Cliente.Data;

using Gimrat.Data;
using Gimrat.Entidades;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace GIMRAT.Controllers
{
    public class SuscripcionController : Controller
    {
        private readonly ClienteData _clienteData;
        private readonly SuscripcionData _suscripcionData;
        public SuscripcionController(ClienteData clienteData, SuscripcionData suscripcionData)
        {
            _clienteData = clienteData;
            _suscripcionData = suscripcionData;
        }
        public IActionResult clienteSuscrito()
        {
            return View();
        }

        public IActionResult Nuevo()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCliente(string rut_cliente)
        {
            cliente objeto = await _clienteData.Obtener(rut_cliente);
            return StatusCode(StatusCodes.Status200OK, new { data = objeto });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] suscripcion objeto)
        {
            string respuesta = await _suscripcionData.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSuscripcion(int id_suscripcion, string rut_cliente)
        {
            List<suscripcion> objeto = await _suscripcionData.ObtenerSuscripcion(id_suscripcion, rut_cliente == null ? "" : rut_cliente);
            return StatusCode(StatusCodes.Status200OK, new { data = objeto });
        }




        [HttpGet]
        public IActionResult ObtenerFechaActual()
        {
            // Obtener la fecha y hora actual del servidor
            DateTime fechaActual = DateTime.Now;

            // Devolver la fecha en formato ISO 8601
            return Ok(new { data = fechaActual.ToString("yyyy-MM-dd") });
        }



        [HttpGet]
        public async Task<IActionResult> ImprimirSuscripcion(int id_suscripcion)
        {
            List<suscripcion> lista = await _suscripcionData.ObtenerSuscripcion(id_suscripcion, "");
            suscripcion objeto = lista[0];

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {

                    page.Margin(30);

                    page.Header().ShowOnce().Background("#D5D5D5").Padding(3).Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text("Suscripción").Bold().FontSize(14);
                        row.RelativeItem().AlignRight().Text($"Nro: {objeto.id_suscripcion}").Bold().FontSize(14);
                    });

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Spacing(18);

                        col1.Item().Column(col2 =>
                        {
                            col2.Spacing(5);
                            col2.Item().Row(row =>
                            {
                                row.RelativeItem().BorderBottom(1).AlignLeft().Text("Datos del Cliente").Bold().FontSize(12);
                            });
                            col2.Item().Row(row =>
                            {

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Numero Documento: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.cliente.rut_cliente).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Nombre: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.cliente.nombre_cliente).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Apellido: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.cliente.apellido_cliente).FontSize(12);
                                    });
                                });
                                row.ConstantItem(50);
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Correo: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.cliente.correo_cliente).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Telefono: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.cliente.telefono_cliente).FontSize(12);
                                    });
                                });
                            });
                        });

                        col1.Item().Column(col2 =>
                        {
                            col2.Spacing(5);
                            col2.Item().Row(row =>
                            {
                                row.RelativeItem().BorderBottom(1).AlignLeft().Text("Datos de la Suscripción").Bold().FontSize(12);
                            });

                            col2.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Plan: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.planes.nombre_plan).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Duración").SemiBold().FontSize(12);
                                        txt.Span(Convert.ToString(objeto.planes.plan_dias)).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Fecha de inicio: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.fecha_inicio.ToString()).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Fecha de finalización: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.fecha_fin).FontSize(12);
                                    });
                                    col.Item().Text(txt =>
                                    {
                                        txt.Span("Total: ").SemiBold().FontSize(12);
                                        txt.Span(objeto.valor_total.ToString()).FontSize(12);
                                    });
                                });
                                row.ConstantItem(50);

                            });
                        });


                        col1.Item().Column(col2 =>
                        {
                            col2.Spacing(5);
                            col2.Item().Row(row =>
                            {
                                row.RelativeItem().BorderBottom(1).AlignLeft().Text("Detalle de la suscripción").Bold().FontSize(12);
                            });
                            col2.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#D5D5D5")
                                    .Padding(4).Text("Nro. Suscripcion").FontColor("#000");

                                    header.Cell().Background("#D5D5D5")
                                   .Padding(4).Text("Fecha de inicio").FontColor("#000");

                                    header.Cell().Background("#D5D5D5")
                                   .Padding(4).Text("Fecha de finalización").FontColor("#000");

                                    header.Cell().Background("#D5D5D5")
                                   .Padding(4).Text("Estado").FontColor("#000");
                                });

                                foreach (var item in objeto.suscripcionDetalles)
                                {


                                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                                        .Padding(4).Text(item.id_suscripcion_detalle.ToString()).FontSize(12);

                                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                                     .Padding(4).Text(item.fecha_inicio).FontSize(12);

                                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                                     .Padding(4).Text(item.fecha_fin).FontSize(12);


                                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                                     .Padding(4).AlignRight().Text($"{item.estado}").FontSize(12);
                                }

                            });
                        });

                    });


                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();


            Stream pdfStream = new MemoryStream(pdf);
            return File(pdfStream, "application/pdf");
        }
    }
}
