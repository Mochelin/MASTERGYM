using Gimrat.Data;
using Gimrat.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Cliente.Data
{
    public class ClienteData
    {
        private readonly ConnectionStrings con;
        public ClienteData(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<List<cliente>> Lista()
        {
            List<cliente> lista = new List<cliente>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaCliente", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new cliente()
                        {
                            id_cliente = Convert.ToInt32(dr["id_cliente"]),
                            rut_cliente = dr["rut_cliente"].ToString()!,
                            nombre_cliente = dr["nombre_cliente"].ToString()!,
                            apellido_cliente = dr["apellido_cliente"].ToString()!,

                            correo_cliente = dr["correo_cliente"].ToString()!,
                            telefono_cliente = dr["telefono_cliente"].ToString()!,
                            genero_cliente = dr["genero_cliente"].ToString()!,
                            fecha_registro = dr["fecha_registro"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }
        public async Task<cliente> Obtener(string rut_cliente)
        {
            cliente objeto = new cliente();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerCliente", conexion);
                cmd.Parameters.AddWithValue("@rut_cliente", rut_cliente);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new cliente()
                        {
                            id_cliente = Convert.ToInt32(dr["id_cliente"]),
                            rut_cliente = dr["rut_cliente"].ToString()!,
                            nombre_cliente = dr["nombre_cliente"].ToString()!,
                            apellido_cliente = dr["apellido_cliente"].ToString()!,
                            correo_cliente = dr["correo_cliente"].ToString()!,
                            telefono_cliente = dr["telefono_cliente"].ToString()!,
                            genero_cliente = dr["genero_cliente"].ToString()!,
                            fecha_registro = dr["fecha_registro"].ToString()!
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<string> Crear(cliente objeto_cliente)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_crearCliente", conexion);
                cmd.Parameters.AddWithValue("@rut_cliente", objeto_cliente.rut_cliente);
                cmd.Parameters.AddWithValue("@nombre_cliente", objeto_cliente.nombre_cliente);
                cmd.Parameters.AddWithValue("@apellido_cliente", objeto_cliente.apellido_cliente);
                cmd.Parameters.AddWithValue("@correo_cliente", objeto_cliente.correo_cliente);
                cmd.Parameters.AddWithValue("@telefono_cliente", objeto_cliente.telefono_cliente);
                cmd.Parameters.AddWithValue("@fecha_registro", objeto_cliente.fecha_registro);
                cmd.Parameters.AddWithValue("@genero_cliente", objeto_cliente.genero_cliente);
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

        public async Task<string> Editar(cliente objeto_cliente)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarCliente", conexion);
                cmd.Parameters.AddWithValue("@id_cliente", objeto_cliente.id_cliente);
                cmd.Parameters.AddWithValue("@rut_cliente", objeto_cliente.rut_cliente);
                cmd.Parameters.AddWithValue("@nombre_cliente", objeto_cliente.nombre_cliente);
                cmd.Parameters.AddWithValue("@apellido_cliente", objeto_cliente.apellido_cliente);
                cmd.Parameters.AddWithValue("@correo_cliente", objeto_cliente.correo_cliente);
                cmd.Parameters.AddWithValue("@telefono_cliente", objeto_cliente.telefono_cliente);
                cmd.Parameters.AddWithValue("@fecha_registro", objeto_cliente.fecha_registro);
                cmd.Parameters.AddWithValue("@genero_cliente", objeto_cliente.genero_cliente);
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

        public async Task<string> Eliminar(int Id)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarCliente", conexion);
                cmd.Parameters.AddWithValue("@id_cliente", Id);
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
    }
}
