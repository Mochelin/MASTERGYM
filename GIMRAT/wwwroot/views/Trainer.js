let tablaData;
let idEditar = 0;
let estado1 = true;
let estado0 = false;
const controlador = "Trainer";
const modal = "mdData";
const preguntaEliminar = "¿Desea eliminar al entrenador?";
const confirmaEliminar = "El entrenador fue eliminado.";
const confirmaRegistro = "Entrenador registrado!";

document.addEventListener("DOMContentLoaded", function (event) {

    tablaData = $('#tbData').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": `/${controlador}/Lista`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "", "data": "id_trainer", visible: false },
            { title: "Rut", "data": "rut_trainer" },
            { title: "Nombre", "data": "nombre_trainer" },
            { title: "Apellido", "data": "apellido_trainer" },
            { title: "Correo", "data": "correo_trainer" },
            { title: "Telefono", "data": "telefono_trainer" },
            { title: "Genero", "data": "genero_trainer" },
            {
                title: "Acciones", "data": null, width: "150px", render: function (data, type, row) {
                    return `<button class="btn btn-warning me-2 btn-editar"><i class="fa-solid fa-pen"></i></button>` +
                        `<button class="btn btn-danger btn-eliminar"><i class="fa-solid fa-trash"></i></button>` +
                        `<button class="btn btn-info btn-pagos"><i class="fa-solid fa-dollar"></i></button>`
                }
            }
        ],
        "order": [0, 'desc'],
        fixedColumns: {
            start: 0,
            end: 1
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

    // Control clic en el botón "Pagos"
    $('#tbData tbody').on('click', '.btn-pagos', function () {
       
        var filaSeleccionada = $(this).closest('tr');
        var data = tablaData.row(filaSeleccionada).data();

        idEditar = data.id_trainer;

        rutTrainer=data.rut_trainer;

        mostrarPagos(rutTrainer);
    });
});


$("#tbData tbody").on("click", ".btn-editar", function () {
    var filaSeleccionada = $(this).closest('tr');
    var data = tablaData.row(filaSeleccionada).data();

    idEditar = data.id_trainer;
    $("#txtRut").val(data.rut_trainer);
    $("#txtNombre").val(data.nombre_trainer);
    $("#txtApellido").val(data.apellido_trainer);
    $("#txtCorreo").val(data.correo_trainer);
    $("#txtTelefono").val(data.telefono_trainer);
    $("#txtGenero").val(data.genero_trainer);
    $(`#${modal}`).modal('show');
});


$("#btnNuevo").on("click", function () {
    idEditar = 0;
    $("#txtRut").val("");
    $("#txtNombre").val("");
    $("#txtApellido").val("");
    $("#txtCorreo").val("");
    $("#txtTelefono").val("");
    $("#txtGenero").val("");
    $(`#${modal}`).modal('show');
});

$("#tbData tbody").on("click", ".btn-eliminar", function () {
    var filaSeleccionada = $(this).closest('tr');
    var data = tablaData.row(filaSeleccionada).data();

    Swal.fire({
        text: `${preguntaEliminar} ${data.nombre_trainer} ${data.apellido_trainer}?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/${controlador}/Eliminar?Id=${data.id_trainer}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == "") {
                    Swal.fire({
                        title: "Listo!",
                        text: confirmaEliminar,
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo eliminar.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo eliminar.",
                    icon: "warning"
                });
            });
        }
    });
});

$("#btnGuardar").on("click", function () {
    const inputs = $(".data-in").serializeArray();
    const inputText = inputs.find((e) => e.value == "");

    if (inputText != undefined) {
        Swal.fire({
            title: "Error!",
            text: `Debe completar el campo: ${inputText.name}`,
            icon: "warning"
        });
        return;
    }

    let objeto_trainer = {
        id_trainer: idEditar,
        rut_trainer: $("#txtRut").val().trim(),
        nombre_trainer: $("#txtNombre").val().trim(),
        apellido_trainer: $("#txtApellido").val().trim(),
        correo_trainer: $("#txtCorreo").val().trim(),
        genero_trainer: $("#txtGenero").val().trim(),
        telefono_trainer: $("#txtTelefono").val().trim(),
        estado: estado1
    };

    if (idEditar != 0) {
        fetch(`/${controlador}/Editar`, {
            method: "PUT",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto_trainer)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                idEditar = 0;
                Swal.fire({
                    text: "Se guardaron los cambios!",
                    icon: "success"
                });
                $(`#${modal}`).modal('hide');
                tablaData.ajax.reload();
            } else {
                Swal.fire({
                    title: "Error!",
                    text: responseJson.data,
                    icon: "warning"
                });
            }
        }).catch((error) => {
            Swal.fire({
                title: "Error!",
                text: "No se pudo editar.",
                icon: "warning"
            });
        });
    } else {
        fetch(`/${controlador}/Crear`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto_trainer)
        }).then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.data == "") {
                Swal.fire({
                    text: confirmaRegistro,
                    icon: "success"
                });
                $(`#${modal}`).modal('hide');
                tablaData.ajax.reload();
            } else {
                Swal.fire({
                    title: "Error!",
                    text: responseJson.data,
                    icon: "warning"
                });
            }
        }).catch((error) => {
            Swal.fire({
                title: "Error!",
                text: "No se pudo registrar.",
                icon: "warning"
            });
        });
    }
});

// Función para descargar la boleta
function descargarBoleta(base64Boleta) {
    const enlace = document.createElement('a');
    const byteCharacters = atob(base64Boleta);
    const byteArrays = new Uint8Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteArrays[i] = byteCharacters.charCodeAt(i);
    }
    const blob = new Blob([byteArrays], { type: 'application/pdf' });
    enlace.href = URL.createObjectURL(blob);
    enlace.download = "boleta.pdf";
    enlace.click();
}

function mostrarPagos(rutTrainer) {
    $.ajax({
        url: `/Trainer/ObtenerPagosTrainer?rut_trainer=${rutTrainer}`,
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response.data && response.data.length > 0) {
                var pagos = response.data;
                var tabla = $('#tablaPagos').DataTable();
                tabla.clear().draw();

                pagos.forEach(function (pago) {
                    const fechaPagoStr = pago.fecha_pago; 
                    const fechaPago = moment(fechaPagoStr, 'YYYY-MM-DD');
                    const fechaFormateada = fechaPago.format('DD/MM/YYYY');
                  
                    tabla.row.add([
                        fechaFormateada,
                        pago.valor_pago,
                        pago.descripcion,
                        `<a href="#" onclick="descargarBoleta('${pago.boleta}')">Descargar</a>`
                    ]).draw(false);
                });

                $('#modalPagos').modal('show');
            } else {
                Swal.fire({
                    title: "Información",
                    text: "No se encontraron pagos para este entrenador.",
                    icon: "info"
                });
            }
        },
        error: function (error) {
            console.error("Error al obtener los pagos:", error);
            Swal.fire({
                title: "Error!",
                text: "No se pudieron obtener los pagos del entrenador.",
                icon: "error"
            });
        }
    });
}