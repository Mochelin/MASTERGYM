namespace Gimrat.Entidades
{
    public class suscripcion
    {
        public int id_suscripcion { get; set; }
        public cliente cliente { get; set; }
        public Planes planes { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public double valor_total { get; set; }
        public bool estado { get; set; }
        public List<suscripcionDetalle> suscripcionDetalles { get; set; } = null!;
    }
}
