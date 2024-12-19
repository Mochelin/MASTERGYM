using Gimrat.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;


namespace Gimrat.Data
{
    public class TrainerData
    {

        private readonly ConnectionStrings con;
        public TrainerData(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<List<trainer>> Lista()
        {
            List<trainer> lista = new List<trainer>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listarTrainer", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new trainer()
                        {
                            id_trainer = Convert.ToInt32(dr["id_trainer"]),
                            rut_trainer = dr["rut_trainer"].ToString()!,
                            nombre_trainer = dr["nombre_trainer"].ToString()!,
                            apellido_trainer = dr["apellido_trainer"].ToString()!,
                            correo_trainer = dr["correo_trainer"].ToString()!,
                            telefono_trainer = dr["telefono_trainer"].ToString()!,
                            genero_trainer = dr["genero_trainer"].ToString()!,

                        });
                    }
                }
            }
            return lista;
        }
        public async Task<trainer> Obtener(string rut_trainer)
        {
            trainer objeto = new trainer();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerTrainer", conexion);
                cmd.Parameters.AddWithValue("@rut_trainer", rut_trainer);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new trainer()
                        {
                            id_trainer = Convert.ToInt32(dr["id_trainer"]),
                            rut_trainer = dr["rut_trainer"].ToString()!,
                            nombre_trainer = dr["nombre_trainer"].ToString()!,
                            apellido_trainer = dr["apellido_trainer"].ToString()!,
                            correo_trainer = dr["correo_trainer"].ToString()!,
                            telefono_trainer = dr["telefono_trainer"].ToString()!,
                            genero_trainer = dr["genero_trainer"].ToString()!,
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<string> Crear(trainer objeto_cliente)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_crearTrainer", conexion);
                cmd.Parameters.AddWithValue("@rut_trainer", objeto_cliente.rut_trainer);
                cmd.Parameters.AddWithValue("@nombre_trainer", objeto_cliente.nombre_trainer);
                cmd.Parameters.AddWithValue("@apellido_trainer", objeto_cliente.apellido_trainer);
                cmd.Parameters.AddWithValue("@correo_trainer", objeto_cliente.correo_trainer);
                cmd.Parameters.AddWithValue("@telefono_trainer", objeto_cliente.telefono_trainer);
                cmd.Parameters.AddWithValue("@genero_trainer", objeto_cliente.genero_trainer);
                cmd.Parameters.AddWithValue("@estado", Convert.ToInt16(objeto_cliente.estado));
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al procesar";
                }
            }
            return respuesta;
        }

        public async Task<string> Editar(trainer objeto_trainer)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarTrainer", conexion);
                cmd.Parameters.AddWithValue("@id_trainer", objeto_trainer.id_trainer);
                cmd.Parameters.AddWithValue("@rut_trainer", objeto_trainer.rut_trainer);
                cmd.Parameters.AddWithValue("@nombre_trainer", objeto_trainer.nombre_trainer);
                cmd.Parameters.AddWithValue("@apellido_trainer", objeto_trainer.apellido_trainer);
                cmd.Parameters.AddWithValue("@correo_trainer", objeto_trainer.correo_trainer);
                cmd.Parameters.AddWithValue("@telefono_trainer", objeto_trainer.telefono_trainer);
                cmd.Parameters.AddWithValue("@genero_trainer", objeto_trainer.genero_trainer);
                cmd.Parameters.AddWithValue("@estado", Convert.ToInt16(objeto_trainer.estado));
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al procesar";
                }
            }
            return respuesta;
        }

        public async Task<string> Eliminar(int Id)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarTrainer", conexion);
                cmd.Parameters.AddWithValue("@id_trainer", Id);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al procesar";
                }
            }
            return respuesta;
        }



        public bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }


        public async Task<string> RegistrarPago(pagoTrainer pagoTrainer)
        {
            Console.WriteLine($"Boleta: {pagoTrainer.boleta}");
            Console.WriteLine($"ID Trainer: {pagoTrainer.trainer?.id_trainer}"); // MOSTRAR DATOS EN CONSOLA
                                                                                    // NO OLVIDAR TENGO UN ERROR AQUI
            Console.WriteLine($"Descripción: {pagoTrainer.descripcion}");
            Console.WriteLine($"Valor Pago: {pagoTrainer.valor_pago}");
            



            string[] parts = pagoTrainer.boleta.Split(',');
            string pdfData = parts[0];
            string respuesta = "";

            DateTime fecha = Convert.ToDateTime(pagoTrainer.fecha_pago);

            try
            {
                if (!IsBase64String(pdfData))
                {

                    throw new FormatException("The boleta string is not a valid Base-64 string.");
                }
                byte[] pdfBytes = Convert.FromBase64String(pdfData);
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_InsertarPagoTrainer", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_trainer", pagoTrainer.trainer.id_trainer);
                        cmd.Parameters.AddWithValue("@descripcion", pagoTrainer.descripcion);
                        cmd.Parameters.AddWithValue("@valor_pago", pagoTrainer.valor_pago);
                        cmd.Parameters.AddWithValue("@fecha_pago", fecha);
                        cmd.Parameters.AddWithValue("@boleta", pdfBytes);

                        await cmd.ExecuteNonQueryAsync();
                        respuesta = "OK";
                    }
                }
            }
            catch (FormatException ex)
            {
                respuesta = "Error al registrar el pago: " + ex.Message;
            }
            catch (Exception ex)
            {
                respuesta = "Error al registrar el pago: " + ex.Message;
            }

            return respuesta;
        }







        public async Task<List<pagoTrainer>> ObtenerPagosTrainer(string rut_trainer)
        {
            var lista = new List<pagoTrainer>();

            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_obtenerPagosTrainer", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@rut_trainer", rut_trainer);

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new pagoTrainer()
                            {
                                id_pago = Convert.ToInt32(dr["id_pago"]),

                                fecha_pago = dr["fecha_pago"]?.ToString()?? "",
                                descripcion = dr["descripcion"]?.ToString() ?? "",
                                valor_pago = Convert.ToSingle(dr["valor_pago"]),
                                boleta = dr["boleta_base64"]?.ToString() ?? ""
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Error al obtener los pagos del entrenador: " + ex.Message);
              
            }

            return lista;
        }

    }
}
