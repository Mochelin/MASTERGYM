

document.addEventListener("DOMContentLoaded", function () {
    
    $.LoadingOverlay("show");

// Obtener la fecha actual
fetch('/Home/ObtenerFechaActual')
    .then(response => response.ok ? response.json() : Promise.reject(response))
    .then(responseJson => {
        const fechaActual = responseJson.data; // Obtener la fecha 
        console.log("Fecha actual del servidor:", fechaActual);

        // Obtener las notificaciones
        fetch(`/Home/ObtenerNotificacionesPorFecha`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json;charset=utf-8' },
            body: JSON.stringify(fechaActual)  // Enviar la fecha como string
        })
            .then(response => response.ok ? response.json() : Promise.reject(response))
            .then(responseJson => {
                if (responseJson.length > 0) {
                    responseJson.forEach(notificacion => {
             
                        const mensaje = `
                            Hola ${notificacion.cliente.nombre_cliente} ${notificacion.cliente.apellido_cliente},<br>
                            ${notificacion.mensaje} (Codigo de suscripcion: ${notificacion.suscripcion.id_suscripcion})
                        `;

                        // Manejo y agregacion del toast(notifiacion) al html
                        const toastHtml = `
                    <div class="toast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-delay="5000">
                        <div class="toast-header">
                            <strong class="mr-auto">Nueva notificación</strong>
                            <small class="text-muted">${notificacion.fecha_notificacion}</small> 
                            <button type="button" class="ml-2 mb-1 close" data-bs-dismiss="toast" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="toast-body">
                            ${mensaje} 
                        </div>
                    </div>
                `;

                        // Agregar el toast al html
                        $('.toast-container')[0].innerHTML += toastHtml;

                        // cargar el toast
                        const toastElement = $('.toast-container .toast').last()[0];
                        const toast = new bootstrap.Toast(toastElement);
                        toast.show();
                    });
                }
            })
            .catch(error => {
                console.error("Error al obtener las notificaciones:", error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Hubo un problema al obtener las notificaciones.'
                });
            });
    })
    .catch(error => {
        console.error("Error al obtener la fecha actual:", error);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Hubo un problema al obtener la fecha actual.'
        });
    })
    .finally(() => {
       
        $.LoadingOverlay("hide");
    });

                     });