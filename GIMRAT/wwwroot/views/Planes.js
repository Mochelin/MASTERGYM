let tablaData;
let idEditar = 0;
let estado1 = true;
let estado0 = false;
const controlador = "Planes";
const modal = "mdData";
const confirmaRegistro = "Plan registrado con exito!";

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
            { title: "", "data": "id_plan", visible: false },
            { title: "Plan", "data": "nombre_plan" },
            { title: "Descripción", "data": "descripcion" },
            { title: "Duracion (dias)", "data": "plan_dias" },
            { title: "Valor", "data": "valor_plan" },
            {
                title: "Estado", "data": "estado", render: function (data, type, row) {
                    return data == 0 ? '<span class="badge bg-danger p-2">No activo</span>' : '<span class="badge bg-success p-2">Activo</span>'
                }
            },
           
            {
                title: "", "data": "id_plan", width: "100px", render: function (data, type, row) {
                    return `<button class="btn btn-warning me-2 btn-editar"><i class="fa-solid fa-pen"></i></button>` 
                       
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

    idEditar = data.id_plan;
    $("#txtPlan").val(data.nombre_plan);
    $("#txtDescripcion").val(data.descripcion);
    $("#txtDias").val(data.plan_dias);
    $("#txtValor").val(data.valor_plan);
    $("#txtEstado").val(data.estado);
    $(`#${modal}`).modal('show');
})


$("#btnNuevo").on("click", function () {
    idEditar = 0;
    $("#txtPlan").val("");
    $("#txtDescripcion").val("");
    $("#txtDias").val("");
    $("#txtValor").val("");
    $("#txtEstado").val(1);

    $(`#${modal}`).modal('show');
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

    let objeto_plan = {
        id_plan: idEditar,
        nombre_plan: $("#txtPlan").val().trim(),
        descripcion: $("#txtDescripcion").val().trim(),
        plan_dias: $("#txtDias").val().trim(),
        valor_plan: $("#txtValor").val().trim(),
        estado: parseInt($("#txtEstado").val()) == 1 ? true : false
    }

    if (idEditar != 0) {

        fetch(`/${controlador}/Editar`, {
            method: "PUT",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto_plan)
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
            body: JSON.stringify(objeto_plan)
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