using Gimrat.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Gimrat.Data
{
    public class PlanesData
    {
        private readonly ConnectionStrings con;
        public PlanesData(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<List<Planes>> Lista()
        {
            List<Planes> lista = new List<Planes>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listarPlanes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Planes()
                        {
                            id_plan = Convert.ToInt32(dr["id_plan"]),
                            nombre_plan = dr["nombre_plan"].ToString()!,
                            descripcion = dr["descripcion"].ToString()!,
                            plan_dias = Convert.ToInt32(dr["plan_dias"])!,
                            valor_plan = Convert.ToInt32(dr["valor_plan"])!,
                            estado = Convert.ToBoolean(dr["estado"])!

                        });
                    }
                }
            }
            return lista;
        }
        public async Task<Planes> Obtener(string nombre_plan)
        {
            Planes objeto = new Planes();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerPlan", conexion);
                cmd.Parameters.AddWithValue("@nombre_plan", nombre_plan);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new Planes()
                        {
                            id_plan = Convert.ToInt32(dr["id_plan"]),
                            nombre_plan = dr["rut_cliente"].ToString()!,
                            descripcion = dr["nombre_cliente"].ToString()!,
                            plan_dias = Convert.ToInt32(dr["plan_dias"])!,
                            valor_plan = Convert.ToInt32(dr["correo_cliente"])!,
                            estado = Convert.ToBoolean(dr["estado"])!
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<string> Crear(Planes objeto_plan)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_crearPlan", conexion);
                cmd.Parameters.AddWithValue("@nombre_plan", objeto_plan.nombre_plan);
                cmd.Parameters.AddWithValue("@descripcion", objeto_plan.descripcion);
                cmd.Parameters.AddWithValue("@plan_dias", Convert.ToInt32(objeto_plan.plan_dias));
                cmd.Parameters.AddWithValue("@valor_plan", Convert.ToDouble(objeto_plan.valor_plan));
                cmd.Parameters.AddWithValue("@estado", Convert.ToInt32(objeto_plan.estado));
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

        public async Task<string> Editar(Planes objeto_plan)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarPlan", conexion);
                cmd.Parameters.AddWithValue("@id_plan", objeto_plan.id_plan);
                cmd.Parameters.AddWithValue("@nombre_plan", objeto_plan.nombre_plan);
                cmd.Parameters.AddWithValue("@descripcion", objeto_plan.descripcion);
                cmd.Parameters.AddWithValue("@plan_dias", Convert.ToInt32(objeto_plan.plan_dias));
                cmd.Parameters.AddWithValue("@valor_plan", Convert.ToDouble(objeto_plan.valor_plan));
                cmd.Parameters.AddWithValue("@estado", Convert.ToInt32(objeto_plan.estado));
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


