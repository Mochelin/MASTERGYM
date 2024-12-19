
let tablaData;
let idEditar = 0;
const controlador = "Trainer";
const confirmaRegistro = "Pago registrado correctamente!";

let idTrainer = 0;

$("#btnBuscar").on("click", function () {

    if ($("#txtRutTrainer").val() == "") {
        Swal.fire({
            title: "Ups!",
            text: "Debe ingresar un numero de Rut.",
            icon: "warning"
        });
        return;
    }

    $("#cardTrainer").LoadingOverlay("show");

    fetch(`/${controlador}/ObtenerTrainer?rut_trainer=${$("#txtRutTrainer").val()}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        console.log("JSON de respuesta:", responseJson);
        $("#cardTrainer").LoadingOverlay("hide");
        if (responseJson.data && responseJson.data.id_trainer != 0) {
            const trainer = responseJson.data;
            idTrainer = trainer.id_trainer;
            $("#txtNombre").val(trainer.nombre_trainer);
            $("#txtApellido").val(trainer.apellido_trainer);
            $("#txtCorreo").val(trainer.correo_trainer);
            $("#txtTelefono").val(trainer.telefono_trainer);
        } else {
            $("#txtNombre").val('');
            $("#txtApellido").val('');
            $("#txtCorreo").val('');
            $("#txtTelefono").val('');

            Swal.fire({
                title: "No se encontró un entrenador registrado",
                text: `Desea registrar manualmente?`,
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Si, continuar",
                cancelButtonText: "No, volver"
            }).then((result) => {
                if (result.isConfirmed) {
                    idTrainer = 0;
                    $("#txtNombre").removeAttr('disabled');
                    $("#txtApellido").removeAttr('disabled');
                    $("#txtCorreo").removeAttr('disabled');
                    $("#txtTelefono").removeAttr('disabled');
                } else {
                    $("#txtRutTrainer").val('');
                }
            });
        }
    }).catch((error) => {
        $("#cardTrainer").LoadingOverlay("hide");
        Swal.fire({
            title: "Error!",
            text: "No se pudo encontrar el entrenador.",
            icon: "warning"
        });
    });
});


$("#btnRegistrar").on("click", async function () {
   
    $.LoadingOverlay("show");

   
    const archivoBoleta = $("#fuBoleta")[0].files[0];


    if ($("#txtDescripcion").val() === "" || $("#txtValorPago").val() === "" || !archivoBoleta) {
        $.LoadingOverlay("hide");
        Swal.fire({
            title: "Error!",
            text: "Por favor, completa todos los campos del formulario.",
            icon: "error"
        });
        return;
    }

    // Convertir el archivo PDF a Base64
    const reader = new FileReader();
    reader.readAsDataURL(archivoBoleta);

    await new Promise(resolve => reader.onload = resolve);

    const base64Boleta = reader.result.split(",")[1];

    // Crear el objeto con los datos del formulario
    const objeto_pago = {
        trainer: {
            id_trainer: idTrainer
        },
        descripcion: $("#txtDescripcion").val().trim(),
        valor_pago: parseFloat($("#txtValorPago").val().trim()),
        fecha_pago: $("#txtFechaPago").val().trim(),
        boleta: base64Boleta
    };

    try {
        // Enviar los datos al controlador
        console.log("Objeto a enviar:", objeto_pago);
        const response = await fetch('/Trainer/CrearPago', {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(objeto_pago)
        });

        // Verificar si la respuesta es correcta
        if (!response.ok) {
            const error = await response.text();
            throw new Error(error || "Error al registrar el pago.");
        }

        // Mostrar un mensaje 
        Swal.fire({
            title: "Listo!",
            text: confirmaRegistro,
            icon: "success"
        });

        // Limpiar el formulario
        $("#txtRutTrainer").val('');
        $("#txtNombre").val('');
        $("#txtApellido").val('');
        $("#txtCorreo").val('');
        $("#txtTelefono").val('');
        $("#txtDescripcion").val('');
        $("#txtValorPago").val('');
        $("#fuBoleta").val('');

    } catch (error) {
        // Mostrar un mensaje de error
        Swal.fire({
            title: "Error!",
            text: error.message,
            icon: "error"
        });
    } finally {
      
        $.LoadingOverlay("hide");
    }
});