let tablaData;
let idEditar = 0;
let estado1 = true;
let estado0 = false;
const controlador = "Cliente";
const modal = "mdData";
const preguntaEliminar = "Desea eliminar la cliente";
const confirmaEliminar = "El cliente fue eliminado.";
const confirmaRegistro = "Cliente registrado!";

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
            { title: "", "data": "id_cliente", visible: false },
            { title: "Rut", "data": "rut_cliente" },
            { title: "Nombre", "data": "nombre_cliente" },
            { title: "Apellido", "data": "apellido_cliente" },
            { title: "Correo", "data": "correo_cliente" },
            { title: "Telefono", "data": "telefono_cliente" },
            { title: "Genero", "data": "genero_cliente" },
            { title: "Fecha Creacion", "data": "fecha_registro" },
            {
                title: "", "data": "id_cliente", width: "100px", render: function (data, type, row) {
                    return `<button class="btn btn-warning me-2 btn-editar"><i class="fa-solid fa-pen"></i></button>` +
                        `<button class="btn btn-danger btn-eliminar"><i class="fa-solid fa-trash"></i></button>`
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

});


$("#tbData tbody").on("click", ".btn-editar", function () {
    var filaSeleccionada = $(this).closest('tr');
    var data = tablaData.row(filaSeleccionada).data();

    idEditar = data.id_cliente;
    $("#txtRut").val(data.rut_cliente);
    $("#txtNombre").val(data.nombre_cliente);
    $("#txtApellido").val(data.apellido_cliente);
    $("#txtCorreo").val(data.correo_cliente);
    $("#txtTelefono").val(data.telefono_cliente);
    $("#txtGenero").val(data.genero_cliente);
    $(`#${modal}`).modal('show');
})


$("#btnNuevo").on("click", function () {
    idEditar = 0;
    $("#txtRut").val("");
    $("#txtNombre").val("");
    $("#txtApellido").val("");
    $("#txtCorreo").val("");
    $("#txtTelefono").val("");
    $("#txtGenero").val("");
    $("#txtFechaCreacion").val("");
    $(`#${modal}`).modal('show');
})

$("#tbData tbody").on("click", ".btn-eliminar", function () {
    var filaSeleccionada = $(this).closest('tr');
    var data = tablaData.row(filaSeleccionada).data();


    Swal.fire({
        text: `${preguntaEliminar} ${data.nombre_cliente} ${data.apellido_cliente}?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/${controlador}/Eliminar?Id=${data.id_cliente}`, {
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
            })
        }
    });
})



$("#btnGuardar").on("click", function () {
    const inputs = $(".data-in").serializeArray();
    const inputText = inputs.find((e) => e.value == "");

    if (inputText != undefined) {
        Swal.fire({
            title: "Error!",
            text: `Debe completar el campo: ${inputText.name}`,
            icon: "warning"
        });
        return
    }

    let objeto_cliente = {
        id_cliente: idEditar,
        rut_cliente: $("#txtRut").val().trim(),
        nombre_cliente: $("#txtNombre").val().trim(),
        apellido_cliente: $("#txtApellido").val().trim(),
        correo_cliente: $("#txtCorreo").val().trim(),
        genero_cliente: $("#txtGenero").val().trim(),
        telefono_cliente: $("#txtTelefono").val().trim(),
        fecha_registro: $("#txtFechaCreacion").val().trim() ,
        estado: estado1
    }

    if (idEditar != 0) {

        fetch(`/${controlador}/Editar`, {
            method: "PUT",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto_cliente)
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
        })
    } else {
        fetch(`/${controlador}/Crear`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto_cliente)
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
        })
    }
});