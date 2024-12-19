namespace Gimrat.Entidades
{
    public class trainer
    {
        public int id_trainer { get; set; }
        public Usuario ousuario { get; set; }
        public string rut_trainer { get; set; }
        public string nombre_trainer { get; set; }
        public string apellido_trainer { get; set; }
        public string correo_trainer { get; set; }
        public string telefono_trainer { get; set; }
        public string genero_trainer { get; set; }
        public bool estado { get; set; }


    }
}
