let tablaData;
let idEditar = 0;
const controlador = "Suscripcion";
const confirmaRegistro = "Cliente Suscrito!";


let idCliente = 0;

document.addEventListener("DOMContentLoaded", function (event) {
    $.LoadingOverlay("show");
    fetch(`/Planes/Lista`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        $.LoadingOverlay("hide");
        if (responseJson.data.length > 0) {
            responseJson.data.forEach((item) => {
                $("#cboTipoPlan").append($("<option>").val(item.id_plan).text(item.nombre_plan).data("valor_plan", item.valor_plan));
            });
        }

        $("#cboTipoPlan").on("change", function () {
        const selectedOption = $(this).find("option:selected");
        const valorPlan = selectedOption.data("valor_plan");
        $("#txtValorPlan").val(valorPlan);
        });
        
    }).catch((error) => {
        $.LoadingOverlay("hide");
        Swal.fire({
            title: "Error!",
            text: "No se pudo eliminar.",
            icon: "warning"
        });
    })
});


$("#btnBuscar").on("click", function () {

    if ($("#txtNroDocumento").val() == "") {
        Swal.fire({
            title: "Ups!",
            text: "Debe ingresar un numero de Rut.",
            icon: "warning"
        });
        return;
    }

    $("#cardCliente").LoadingOverlay("show");

    fetch(`/${controlador}/ObtenerCliente?rut_cliente=${$("#txtNroDocumento").val()}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        $("#cardCliente").LoadingOverlay("hide");
        if (responseJson.data.id_cliente != 0) {

            const cliente = responseJson.data;
            id_cliente = cliente.id_cliente;
            $("#txtNombre").val(cliente.nombre_cliente);
            $("#txtApellido").val(cliente.apellido_cliente);
            $("#txtCorreo").val(cliente.correo_cliente);
            $("#txtTelefono").val(cliente.telefono_cliente);


        } else {
            $("#txtNombre").val('');
            $("#txtApellido").val('');
            $("#txtCorreo").val('');
            $("#txtTelefono").val('');
            Swal.fire({
                title: "No se encontro un cliente registrado",
                text: `Desea registrar manualmente?`,
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Si, continuar",
                cancelButtonText: "No, volver"
            }).then((result) => {
                if (result.isConfirmed) {
                    idCliente = 0;
                    $("#txtNombre").removeAttr('disabled');
                    $("#txtApellido").removeAttr('disabled');
                    $("#txtCorreo").removeAttr('disabled');
                    $("#txtTelefono").removeAttr('disabled');
                }
            });
        }
    }).catch((error) => {
        $("#cardCliente").LoadingOverlay("hide");
        Swal.fire({
            title: "Error!",
            text: "No se pudo eliminar.",
            icon: "warning"
        });
    })

})

$("#btnCalcular").on("click", function () {
    const inputsPlan = $(".data-plan").serializeArray();
    const inputText = inputsPlan.find((e) => e.value == "");

    if (inputText != undefined) {
        Swal.fire({
            title: "Error!",
            text: `Debe completar el campo: ${inputText.name.replaceAll("_", " ")}`,
            icon: "warning"
        });
        return;
    }

    const descuento = $("#txtDescuento").val() == "" ? 0 : parseFloat($("#txtDescuento").val()); 
    const valor = $("#txtValorPlan").val() == "" ? 0 : parseFloat($("#txtValorPlan").val());
    const montoTotal = valor - descuento;

    $("#txtDescuento").val(descuento.toFixed(0)); //QUITAR DECIMALES --- NO FUNCIONO 
    $("#txtMontoTotal").val(montoTotal.toFixed(0));
});

$("#btnRegistrar").on("click", function () {
    const inputs = $(".data-in").serializeArray();
    const inputText = inputs.find((e) => e.value == "");

    if (idCliente == 0) {
        if (inputText != undefined) {
            Swal.fire({
                title: "Error!",
                text: `Debe completar el campo: ${inputText.name.replaceAll("_", " ")}`,
                icon: "warning"
            });
            return
        }
    }


    if ($("#txtMontoTotal").val() == "") {
        Swal.fire({
            title: "Error!",
            text: `Debe completar el detalle del prestamo`,
            icon: "warning"
        });
        return
    }


    const objeto = {
        cliente: {
            id_cliente: id_cliente,
            rut_cliente: $("#txtNroDocumento").val(),
            nombre_cliente: $("#txtNombre").val(),
            apellido_cliente: $("#txtApellido").val(),
            correo_cliente: $("#txtCorreo").val(),
            telefono_cliente: $("#txtTelefono").val()
        },
        Planes: {
            id_plan: $("#cboTipoPlan").val()
         
        },
        fecha_inicio: moment($("#txtFechaInicio").val()).format("DD/MM/YYYY"),
        fecha_fin: moment($("#txtFechaFin").val()).format("DD/MM/YYYY"),
        valor_total: $("#txtMontoTotal").val()
    }

    fetch(`/${controlador}/Crear`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(objeto)
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data == "") {

            idCliente = 0;
            $(".data-in").val("");
            $(".data-suscripcion").val("");
            $("#cboTipoPlan").val($("#cboTipoPlan option:first").val());
          

            Swal.fire({
                title: "Listo!",
                text: confirmaRegistro,
                icon: "success"
            });
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
})
