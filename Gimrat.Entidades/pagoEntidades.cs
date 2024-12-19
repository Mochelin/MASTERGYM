using System.ComponentModel.DataAnnotations;

namespace Gimrat.Entidades
{
    public class pagoTrainer
    {
        public int id_pago { get; set; }

        [Required]
        public trainer trainer { get; set; } // Agregar la propiedad id_trainer

        [Required]
        public string descripcion { get; set; }

        public float valor_pago { get; set; }
        public String fecha_pago { get; set; }

        [Required]
        public string boleta { get; set; }
    }
}