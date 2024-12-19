document.addEventListener("DOMContentLoaded", function () {
   
    $.LoadingOverlay("show");

    // Realizar la petición fetch al controlador
    fetch(`/Home/ObtenerResumen`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    })
        .then(response => {
        
            if (!response.ok) {
                
                return Promise.reject(response);
            }
            return response.json(); 
        })
        .then(responseJson => {
            // Ocultar overlay de carga
            $.LoadingOverlay("hide");
            console.log("Datos obtenidos:", responseJson);

            if (responseJson && responseJson.data) {
                const data = responseJson.data;

               
                const gananciasMensuales = data.gananciasMensuales || [];

                const egresosMensuales = data.egresosMensuales || [];

                
                console.log("Ganancias mensuales:", gananciasMensuales);


                const hombres = data.hombres || 0;
                const mujeres = data.mujeres || 0;
                const otros = data.otros || 0;
                const total = hombres + mujeres + otros;
                console.log("Hombres:", hombres);   
                console.log("Mujeres:", mujeres);
                console.log("Otros:", otros);
                // Agregar valores en el html
                $("#TotalClientes").text(data.totalClientes || "N/A");
                $("#SuscripcionesOFF").text(data.suscripcionesInactivas || "N/A");
                $("#SuscripcionesON").text(data.suscripcionesActivas || "N/A");
                $("#TotalMensual").text("$ " + (data.totalMes || "N/A"));
                $("#TotalAnual").text("$ " + (data.totalAnio || "N/A"));
                $("#TotalMensualEgresos").text("$ " + (data.totalMesEgreso || "N/A"));
                $("#TotalAnualEgresos").text("$ " + (data.totalAnioEgreso || "N/A"));

                // Calcular el porcentaje
                const totalClientes = parseInt(data.totalClientes || 0, 10);
                const porcentaje = totalClientes > 0 ? (totalClientes * 100) / 100 : 0;
                const texto = `${porcentaje.toFixed(2)}%`;
                $("#capacidad").text(texto);

                // Actualizar la barra de progreso (Capacidad gimnacio)
                const progressBarWidth = `${porcentaje}%`;
                $("#progressBar").css("width", progressBarWidth);
                $("#progressBar").attr("aria-valuenow", porcentaje);

                // Crear el gráfico de barras
                const ctx = document.getElementById('myBarChart').getContext('2d');
                new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: [
                            'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo',
                            'Junio', 'Julio', 'Agosto', 'Septiembre',
                            'Octubre', 'Noviembre', 'Diciembre'
                        ],
                        datasets: [{
                            label: 'Ganancias Mensuales',
                            data: gananciasMensuales,
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            borderColor: 'rgba(75, 192, 192, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            legend: { display: true },
                            title: { display: true, text: 'Ganancias Mensuales del Año Actual' }
                        },
                        scales: {
                            y: { beginAtZero: true }
                        }
                    }
                });



                const ctx2 = document.getElementById('myBarChartHorizontal').getContext('2d');
                new Chart(ctx2, {
                    type: 'bar',
                    data: {
                        labels: ['Total', 'Hombres', 'Mujeres', 'Otros'],
                        datasets: [{
                            label: 'Distribución por género',
                            data: [total, hombres, mujeres, otros], 
                            backgroundColor: ['#36A2EB', '#FF6384', '#4BC0C0', '#FFCE56'],
                            borderColor: ['#36A2EB', '#FF6384', '#4BC0C0', '#FFCE56'],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        indexAxis: 'y', // GRAFICO COMO HORIZONTAL
                        responsive: true,
                        scales: {
                            x: {
                                beginAtZero: true
                            }
                        }
                    }
                });













            } else {
                // Mostrar alerta si no hay datos
                Swal.fire({
                    title: "Advertencia!",
                    text: "No se encontraron datos para mostrar.",
                    icon: "warning"
                });
            }
        })
        .catch(error => {
            // Ocultar overlay de carga en caso de error
            $.LoadingOverlay("hide");
            console.error("Error al obtener datos:", error);

            // Mostrar alerta de error con SweetAlert
            Swal.fire({
                title: "Error!",
                text: "No se pudo cargar la información.",
                icon: "error"
            });
        });







});




/*
    fetch(`/Home/ObtenerResumen`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            // Ocultar el overlay de carga
            $.LoadingOverlay("hide");
            console.log("Datos obtenidos:", responseJson);

            if (responseJson) {
                console.log("Response completa:", responseJson);
    // Configurar el gráfico de área
    const ctxArea = document.getElementById("myAreaChart").getContext("2d");
    const myAreaChart = new Chart(ctxArea, {
        type: 'line',
        data: {
            labels: responseJson.meses, // Asegúrate de que responseJson.meses contiene las etiquetas de los meses
            datasets: [{
                label: "Suscripciones",
                data: responseJson.suscripcionesMensuales, // Asegúrate de que responseJson.suscripcionesMensuales contiene los datos
                backgroundColor: "rgba(78, 115, 223, 0.05)",
                borderColor: "rgba(78, 115, 223, 1)",
                pointRadius: 3,
                pointBackgroundColor: "rgba(78, 115, 223, 1)",
                pointBorderColor: "rgba(78, 115, 223, 1)",
                pointHoverRadius: 3,
                pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                pointHitRadius: 10,
                pointBorderWidth: 2,
            }],
        },
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0
                }
            },
            scales: {
                xAxes: [{
                    time: {
                        unit: 'date'
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    ticks: {
                        maxTicksLimit: 7
                    }
                }],
                yAxes: [{
                    ticks: {
                        maxTicksLimit: 5,
                        padding: 10,
                        // Include a dollar sign in the ticks
                        callback: function (value, index, values) {
                            return number_format(value);
                        }
                    },
                    gridLines: {
                        color: "rgb(234, 236, 244)",
                        zeroLineColor: "rgb(234, 236, 244)",
                        drawBorder: false,
                        borderDash: [2],
                        zeroLineBorderDash: [2]
                    }
                }],
            },
            legend: {
                display: false
            },
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                titleMarginBottom: 10,
                titleFontColor: '#6e707e',
                titleFontSize: 14,
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                intersect: false,
                mode: 'index',
                caretPadding: 10,
                callbacks: {
                    label: function (tooltipItem, chart) {
                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                        return datasetLabel + ': ' + number_format(tooltipItem.yLabel);
                    }
                }
            }
        }
    });

    // Configurar el gráfico de pastel
    const ctxPie = document.getElementById("myPieChart").getContext("2d");
    const myPieChart = new Chart(ctxPie, {
        type: 'doughnut',
        data: {
            labels: ["Primer Mes", "Tercer Mes", "Cuarto Mes+"],
            datasets: [{
                data: responseJson.renovaciones, // Asegúrate de que responseJson.renovaciones contiene los datos
                backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc'],
                hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf'],
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }],
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            legend: {
                display: false
            },
            cutoutPercentage: 80,
        },
    });
} else {
    // Mostrar advertencia si no hay datos
    Swal.fire({
        title: "Advertencia!",
        text: "No se encontraron datos para mostrar.",
        icon: "warning"
    });
}
        })
    .catch((error) => {
        // Ocultar el overlay de carga si ocurre un error
        $.LoadingOverlay("hide");
        console.error("Error al obtener datos:", error);
        Swal.fire({
            title: "Error!",
            text: "No se pudo cargar la información.",
            icon: "error"
        });
        
});*/