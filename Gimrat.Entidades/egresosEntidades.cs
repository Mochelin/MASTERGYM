namespace Gimrat.Entidades
{
    public class egresos
    {
        public int id_egreso { get; set; }
        public Usuario ousuario { get; set; }
        public string descripcion { get; set; }
        public float valor { get; set; }
        public bool estado { get; set; }

    }
}
