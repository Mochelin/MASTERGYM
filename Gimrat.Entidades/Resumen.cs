namespace Gimrat.Entidades
{
    public class Resumen
    {
        public string TotalClientes { get; set; }
        public string SuscripcionesInactivas { get; set; }
        public string SuscripcionesActivas { get; set; }
        public int TotalMes { get; set; }
        public int TotalAnio { get; set; }


        public int TotalMesEgreso { get; set; }
        public int TotalAnioEgreso { get; set; }

        public List<double> GananciasMensuales { get; set; }
        public List<double> EgresosMensuales { get; set; }

        public int Hombres { get; set; }
        public int Mujeres { get; set; }
        public int Otros { get; set; }
    }
}
