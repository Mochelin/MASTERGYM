using Gimrat.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.Xml.Linq;
namespace Gimrat.Data
{
    public class SuscripcionData
    {
        private readonly ConnectionStrings con;
        public SuscripcionData(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<string> Crear(suscripcion objeto)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_crearSuscripcion", conexion);
                cmd.Parameters.AddWithValue("@id_cliente", objeto.cliente.id_cliente);
                cmd.Parameters.AddWithValue("@rut_cliente", objeto.cliente.rut_cliente);
                cmd.Parameters.AddWithValue("@nombre_cliente", objeto.cliente.nombre_cliente);
                cmd.Parameters.AddWithValue("@apellido_cliente", objeto.cliente.apellido_cliente);
                cmd.Parameters.AddWithValue("@correo_cliente", objeto.cliente.correo_cliente);
                cmd.Parameters.AddWithValue("@telefono_cliente", objeto.cliente.telefono_cliente);
                cmd.Parameters.AddWithValue("@id_plan", objeto.planes.id_plan);
                cmd.Parameters.AddWithValue("@fecha_inicio_sub", objeto.fecha_inicio);
                cmd.Parameters.AddWithValue("@fecha_fin_sub", objeto.fecha_fin);
                cmd.Parameters.AddWithValue("@valor_total", objeto.valor_total);
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

        public async Task<List<suscripcion>> ObtenerSuscripcion(int id_suscripcion, string rut_cliente)
            {
            var lista = new List<suscripcion>();

            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    using (var cmd = new SqlCommand("sp_obtenerSuscripcion", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_suscripcion", id_suscripcion);
                        cmd.Parameters.AddWithValue("@rut_cliente", rut_cliente);

                        using (var dr = await cmd.ExecuteXmlReaderAsync())
                        {
                            if (await dr.ReadAsync())
                            {
                                var doc = XDocument.Load(dr);

                                lista = (from suscripcion in doc.Element("Suscripciones")?.Elements("Suscripcion") ?? Enumerable.Empty<XElement>()
                                         select new suscripcion
                                         {
                                             id_suscripcion = Convert.ToInt32(suscripcion.Element("id_suscripcion")?.Value),
                                             cliente = new cliente
                                             {
                                                 id_cliente = Convert.ToInt32(suscripcion.Element("id_cliente")?.Value),
                                                 rut_cliente = suscripcion.Element("rut_cliente")?.Value,
                                                 nombre_cliente = suscripcion.Element("nombre_cliente")?.Value,
                                                 apellido_cliente = suscripcion.Element("apellido_cliente")?.Value,
                                                 correo_cliente = suscripcion.Element("correo_cliente")?.Value,
                                                 telefono_cliente = suscripcion.Element("telefono_cliente")?.Value
                                             },
                                             planes = new Planes
                                             {
                                                 id_plan = Convert.ToInt32(suscripcion.Element("id_plan")?.Value),
                                                 nombre_plan = suscripcion.Element("nombre_plan")?.Value,
                                                 plan_dias = Convert.ToInt32(suscripcion.Element("plan_dias")?.Value)
                                             },
                                             fecha_inicio = suscripcion.Element("fecha_inicio")?.Value,
                                             fecha_fin = suscripcion.Element("fecha_fin")?.Value,
                                             valor_total = Convert.ToDouble(suscripcion.Element("valor_total")?.Value),
                                             estado = suscripcion.Element("estado")?.Value == "1" ? true : false,  
                                             suscripcionDetalles = (from detalle in suscripcion.Element("detalles")?
                                                     .Element("SuscripcionDetalle")?
                                                     .Elements("Detalle") ?? Enumerable.Empty<XElement>()
                                                                    select new suscripcionDetalle
                                                                    {
                                                                        id_suscripcion_detalle = Convert.ToInt32(detalle.Element("id_suscripcion_detalle")?.Value),
                                                                        fecha_inicio = detalle.Element("fecha_inicio")?.Value,
                                                                        fecha_fin = detalle.Element("fecha_fin")?.Value,
                                                                        valor_total = Convert.ToDouble(detalle.Element("valor_total")?.Value),
                                                                        estado = detalle.Element("estado")?.Value == "1"
                                                                    }).ToList()
                                         }).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
           
                Console.WriteLine("Error al ejecutar la tarea: " + ex.Message);
                throw;  
            }

            return lista;
        }
    }
}



