namespace Gimrat.Entidades
{
    public class suscripcionDetalle
    {

        public int id_suscripcion_detalle { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public double valor_total { get; set; }
        public bool estado { get; set; }
    }
}
