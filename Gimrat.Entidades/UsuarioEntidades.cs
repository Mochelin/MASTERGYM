namespace Gimrat.Entidades
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombre_usuario { get; set; } = null!;
        public string apellido_usuario { get; set; } = null!;
        public string correo_usuario { get; set; } = null!;
        public string clave_usuario { get; set; } = null!;
    }
}
