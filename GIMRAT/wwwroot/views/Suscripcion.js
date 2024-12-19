let tablaData;
let idSuscripcion = 0;
const controlador = "Suscripcion";
const modal = "mdData";
const detalleDummy = [
    { id_suscripcion_detalle: 1, fecha_inicio: "2024-01-01", fecha_fin: "2024-12-31", valor_total: 10000, estado: true },
    { id_suscripcion_detalle: 2, fecha_inicio: "2023-01-01", fecha_fin: "2023-12-31", valor_total: 20000, estado: false }
];

document.addEventListener("DOMContentLoaded", function () {
    // Crea el datatable
    tablaData = $('#tbData').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": `/${controlador}/ObtenerSuscripcion?id_suscripcion=0&rut_cliente=`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Nro. suscripcion", "data": "id_suscripcion" },
            {
                title: "Cliente", "data": "cliente", render: function (data) {
                   
                    return `${data?.nombre_cliente || ""} ${data?.apellido_cliente || ""}`;
                }
            },
            {
                title: "Planes", "data": "planes.nombre_plan", render: function (data) {
                    
                    return data || "Sin plan";
                }
            },
            {
                title: "Estado", "data": "estado", render: function (data) {
                  
                    return data ? '<span class="badge bg-success p-2">Activa</span>' : '<span class="badge bg-danger p-2">Pendiente</span>';
                }
            },
            {
                title: "Acciones", "data": "id_suscripcion", width: "120px", render: function () {
                  
                    return `<button class="btn btn-dark me-2 btn-detalle"><i class="fa-solid fa-list-ol"></i> Ver detalle</button>`;
                }
            }
        ],
        "order": [[0, 'desc']],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

$("#tbData tbody").on("click", ".btn-detalle", function () {
    const filaSeleccionada = $(this).closest('tr');
    const suscripcion = tablaData.row(filaSeleccionada).data();

    if (!suscripcion) {
        console.error("No se pudo obtener la información de la suscripción.");
        return;
    }

    const detalle = suscripcion.suscripcionDetalles || [];
    idSuscripcion = suscripcion.id_suscripcion;

    
    $("#txtid_suscripcion").text(`Nro. Suscripcion: ${suscripcion.id_suscripcion}`);
    $("#txtTipo").val(suscripcion?.planes?.nombre_plan || "Sin plan");
    $("#txtDuracion").val(suscripcion?.planes?.plan_dias || "No especificado");
    $("#txtMontoTotal").val(suscripcion.valor_total || "0");

    $("#tbDetalle tbody").html("");
    detalle.forEach(function (e) {
        const estadoTexto = e.estado ? "Activo" : "Inactivo";
        $("#tbDetalle tbody").append(`
            <tr>
                <td>${e.id_suscripcion_detalle || ""}</td>
                <td>${e.fecha_inicio || ""}</td>
                <td>${e.fecha_fin || ""}</td>
                <td>${e.valor_total || "0"}</td>
                <td>${estadoTexto}</td>
            </tr>`);
    });

    // Muestra el modal
    $(`#${modal}`).modal('show');
});
$("#btnImprimir").on("click", function () {
  window.open(`/Suscripcion/ImprimirSuscripcion?id_suscripcion=${idSuscripcion}`, "_blank");
})