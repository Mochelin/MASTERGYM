namespace Gimrat.Entidades
{
    public class ingresos
    {
        public int id_ingresos { get; set; }
        public cliente ocliente { get; set; }
        public string descripcion_cliente { get; set; }
        public DateTime fecha_ingreso { get; set; }
        public float total_ingreso { get; set; }
        public bool estado { get; set; }

    }
}
