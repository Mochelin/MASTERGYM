using Gimrat.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Gimrat.Data
{
    public class ResumenData
    {
        private readonly ConnectionStrings con;

        public ResumenData(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<(int activas, int inactivas)> ContarSuscripcionesPorEstadoAsync()
        {
            int activas = 0;
            int inactivas = 0;

            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();

                    // Contar suscripciones activas
                    using (SqlCommand cmd = new SqlCommand("sp_ContarSuscripcionesPorEstado", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@estado", 1);
                        cmd.Parameters.Add("@cantidad", SqlDbType.Int).Direction = ParameterDirection.Output;

                        await cmd.ExecuteNonQueryAsync();
                        activas = (int)cmd.Parameters["@cantidad"].Value;
                    }

                    // Contar suscripciones inactivas
                    using (SqlCommand cmd = new SqlCommand("sp_ContarSuscripcionesPorEstado", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@estado", 0);
                        cmd.Parameters.Add("@cantidad", SqlDbType.Int).Direction = ParameterDirection.Output;

                        await cmd.ExecuteNonQueryAsync();
                        inactivas = (int)cmd.Parameters["@cantidad"].Value;
                    }
                }
            }
            catch (SqlException ex)
            {
             
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
            }
            catch (Exception ex)
            {
             
                Console.WriteLine($"Error general: {ex.Message}");
            }

            return (activas, inactivas);
        }


        public async Task<double> ObtenerGananciasAnioActualAsync()
        {
            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ObtenerGananciasAnioActual", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    return (double)await cmd.ExecuteScalarAsync();
                }
            }
            catch (SqlException ex)
            {
              
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error general: {ex.Message}");
                return 0;
            }
        }

        public async Task<double> ObtenerGananciasMesActualAsync()
        {
            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ObtenerGananciasMesActual", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    return (double)await cmd.ExecuteScalarAsync();
                }
            }
            catch (SqlException ex)
            {
           
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
            
                Console.WriteLine($"Error general: {ex.Message}");
                return 0;
            }

        }







        public async Task<List<double>> ObtenerGananciasPorMesAsync()
        {
            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ObtenerGananciasPorMes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Inicializamos la lista para almacenar las ganancias de cada mes
                        var gananciasMensuales = new List<double>(new double[12]);

                        while (await reader.ReadAsync())
                        {
                            int mes = reader.GetInt32(0); // Número de mes
                            double totalGanancias = reader.IsDBNull(1) ? 0 : reader.GetDouble(1); // Total ganancias del mes
                            gananciasMensuales[mes - 1] = totalGanancias; // Ajustamos el índice
                        }

                        return gananciasMensuales;
                    }
                }
            }
            catch (SqlException ex)
            {
                
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return new List<double>(new double[12]); 
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error general: {ex.Message}");
                return new List<double>(new double[12]);
            }
        }











        public async Task<double> ObtenerEgresosAnioActualAsync()
        {
            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ObtenerEgresosAnioActual", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    return (double)await cmd.ExecuteScalarAsync();
                }
            }
            catch (SqlException ex)
            {
                // Manejar la excepción de SQL Server (registrarla, etc.)
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones (registrarla, etc.)
                Console.WriteLine($"Error general: {ex.Message}");
                return 0;
            }
        }







        public async Task<double> ObtenerEgresosMesActualAsync()
        {
            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ObtenerEgresosMesActual", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    return (double)await cmd.ExecuteScalarAsync();
                }
            }
            catch (SqlException ex)
            {
                // Manejar la excepción de SQL Server (registrarla, etc.)
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones (registrarla, etc.)
                Console.WriteLine($"Error general: {ex.Message}");
                return 0;
            }

        }

















        public async Task<List<double>> ObtenerEgresosPorMesAsync()
        {
            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ObtenerEgresosPorMes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Inicializamos la lista para almacenar los egresos de cada mes
                        var egresosMensuales = new List<double>(new double[12]);

                        while (await reader.ReadAsync())
                        {
                            int mes = reader.GetInt32(0); // Número de mes (1 = enero, 2 = febrero, ...)
                            double totalGanancias = reader.IsDBNull(1) ? 0 : reader.GetDouble(1); // Total ganancias del mes
                            egresosMensuales[mes - 1] = totalGanancias; // Ajustamos el índice para 0-based
                        }

                        return egresosMensuales;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Manejar excepciones específicas de SQL Server
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
                return new List<double>(new double[12]); // Devuelve una lista con 0 en todos los meses
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                Console.WriteLine($"Error general: {ex.Message}");
                return new List<double>(new double[12]);
            }
        }








        public async Task<(int hombres, int mujeres, int otros)> CargarResumenGeneroAsync()
        {
            int hombres = 0;
            int mujeres = 0;
            int otros = 0;
            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ContarClientesPorGenero", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string genero = reader["Genero"].ToString();
                            int cantidad = Convert.ToInt32(reader["Cantidad"]);

                            // Asignar los valores a las propiedades de la clase Resumen usando if-else if
                            if (genero == "HOMBRE")
                            {
                                 hombres = cantidad;
                            }
                            else if (genero == "MUJER")
                            {
                                 mujeres = cantidad;
                            }

                            else
                            {
                                 otros = cantidad;
                            }
                            }
                        }
                    }
                
            }
            catch (SqlException ex)
            {
                // Manejar excepciones específicas de SQL Server
                Console.WriteLine($"Error de SQL Server: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                Console.WriteLine($"Error general: {ex.Message}");
            }

            return(hombres, mujeres, otros);
        }










        public async Task<List<notificacion>> Lista(string fecha_actual)
        {
            List<notificacion> lista = new List<notificacion>();

            try
            {
                // Parsear la fecha (con manejo de errores)
                if (!DateOnly.TryParse(fecha_actual, out DateOnly fecha))
                {
                    throw new ArgumentException("El formato de la fecha no es válido.");
                }

                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();

                    // Configurar el comando SQL
                    SqlCommand cmd = new SqlCommand("sp_ActualizarSuscripciones", conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@fecha_actual", fecha);

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            try
                            {
                                // Leer y mapear los datos (con manejo de nulos)
                                lista.Add(new notificacion()
                                {
                                    id_notificacion = dr["id_notificacion"] != DBNull.Value
                                        ? Convert.ToInt32(dr["id_notificacion"])
                                        : 0,

                                    cliente = new cliente
                                    {
                                        id_cliente = dr["id_cliente"] != DBNull.Value
                                            ? Convert.ToInt32(dr["id_cliente"])
                                            : 0,
                                        nombre_cliente = dr["nombre_cliente"]?.ToString() ?? "N/A",
                                        apellido_cliente = dr["apellido_cliente"]?.ToString() ?? "N/A"
                                    },
                                    suscripcion = new suscripcion
                                    {
                                        id_suscripcion = dr["id_suscripcion"] != DBNull.Value
                                            ? Convert.ToInt32(dr["id_suscripcion"])
                                            : 0
                                    },
                                    mensaje = dr["mensaje"]?.ToString() ?? string.Empty,
                                    fecha_notificacion = dr["fecha_notificacion"]?.ToString() ?? string.Empty,
                                    estado = dr["estado"] != DBNull.Value && Convert.ToBoolean(dr["estado"])
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al procesar una fila: {ex.Message}");
                                // Puedes omitir esta fila si hay problemas específicos
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Lista: {ex.Message}");
                // Manejar errores generales o lanzarlos según sea necesario
                throw;
            }

            return lista;
        }
    }
}



