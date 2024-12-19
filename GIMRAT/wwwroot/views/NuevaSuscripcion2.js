let tablaData;
let idEditar = 0;
const controlador = "Suscripcion";
const confirmaRegistro = "Cliente Suscrito!";

let idCliente = 0;

document.addEventListener("DOMContentLoaded", function (event) {
    $.LoadingOverlay("show");

    // Obtener los planes y la fecha actual
    Promise.all([
        fetch(`/Planes/Lista`, { 
            method: "GET",
            headers: { 'Content-Type': 'application/json;charset=utf-8' }
        }).then(response => response.ok ? response.json() : Promise.reject(response)),



        fetch(`/Suscripcion/ObtenerFechaActual`) /
            .then(response => response.ok ? response.json() : Promise.reject(response))
    ])





        .then(([responsePlanes, responseFecha]) => {
            $.LoadingOverlay("hide");

            // Cargar planes
            if (responsePlanes.data.length > 0) {
                responsePlanes.data.forEach((item) => {
                    $("#cboTipoPlan").append($("<option>").val(item.id_plan).text(item.nombre_plan)
                        .data("valor_plan", item.valor_plan)
                        .data("duracion", item.plan_dias)); 
                });
            }

            console.log(responseFecha.data);
            console.log(responsePlanes.data);

            // Establecer la fecha mínima
            const fechaActual = new Date(responseFecha.data.replace(/-/g, '/')); // Reemplazar guiones por barras (NO MANEJO LA EXCEPCION)
            const fechaMinima = new Date();
            fechaMinima.setDate(fechaActual.getDate() - 5);
            const fechaMinimaFormateada = fechaMinima.toISOString().split('T')[0];
            $("#txtFechaInicio").attr("min", fechaMinimaFormateada);



         
            $("#cboTipoPlan").on("change", function () {
                const selectedOption = $(this).find("option:selected");
                const valorPlan = selectedOption.data("valor_plan");
                $("#txtValorPlan").val(valorPlan);
            });

          
            $("#txtFechaInicio").on("change", function () {
                const fechaInicio = new Date($(this).val().replace(/-/g, '/'));
                const selectedOption = $("#cboTipoPlan").find("option:selected");
                const duracionPlan = selectedOption.data("duracion");

                // Calcular la fecha de finalización
                const fechaFin = new Date(fechaInicio);
                fechaFin.setDate(fechaInicio.getDate() + duracionPlan);
                const fechaFin2 = fechaFin.toISOString().split('T')[0];
                console.log(fechaFin2);

                // Formatear la fecha de finalización (dd/mm/aaaa)
                const dia = ("0" + fechaFin.getDate()).slice(-2);
                const mes = ("0" + (fechaFin.getMonth() + 1)).slice(-2);
                const anio = fechaFin.getFullYear();
                const fechaFinFormateada = anio + "/" + mes + "/" + dia;

                console.log (fechaFinFormateada);

                // Establecer la fecha de finalización en el campo "Fecha Fin"
                $("#txtFechaFin").val(fechaFin2); //Alfinal maneje directo la fecha
            });
        })
        .catch((error) => {
            $.LoadingOverlay("hide");
            Swal.fire({
                title: "Error!",
                text: "No se pudo cargar la información.",
                icon: "warning"
            });
        });
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

});

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

    const descuento = $("#txtDescuento").val() == "" ? 0 : parseInt($("#txtDescuento").val());
    const valor = $("#txtValorPlan").val() == "" ? 0 : parseInt($("#txtValorPlan").val());
    const montoTotal = valor - descuento;

    $("#txtDescuento").val(descuento);
    $("#txtMontoTotal").val(montoTotal);
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
            return;
        }
    }

    if ($("#txtMontoTotal").val() == "") {
        Swal.fire({
            title: "Error!",
            text: `Debe completar el detalle de la suscripcion`,
            icon: "warning"
        });
        return;
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
    };

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
    });
});