using Gimrat.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Gimrat.Data
{


    public class UsuarioData
    {

        private static UsuarioData instancia = null;


        public UsuarioData() { }

        public static UsuarioData Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new UsuarioData();
                }

                return instancia;
            }
        }




        private readonly ConnectionStrings con;
        public UsuarioData(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<Usuario> Obtener(string correo, string clave)
        {
            Usuario objeto = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerUsuario", conexion);
                cmd.Parameters.AddWithValue("@correo_usuario", correo);
                cmd.Parameters.AddWithValue("@clave_usuario", clave);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new Usuario()
                        {
                            id_usuario = Convert.ToInt32(dr["id_usuario"].ToString()!),
                            nombre_usuario = dr["nombre_usuario"].ToString()!,

                            correo_usuario = dr["correo_usuario"].ToString()!
                        };
                    }
                }
            }
            return objeto;
        }
    }
}
