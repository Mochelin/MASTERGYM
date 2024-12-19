namespace Gimrat.Entidades
{
    public class totalmensual
    {
        public int id_mensual { get; set; }
        public string mes_referente { get; set; }
        public DateTime fecha_referente { get; set; }
        public float total_ingresos { get; set; }
        public float total_egresos { get; set; }
        public bool estado { get; set; }

    }
}
