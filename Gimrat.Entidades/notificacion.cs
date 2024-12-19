using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimrat.Entidades
{
    public class notificacion
    {
        public int id_notificacion { get; set; }
        public cliente cliente { get; set; }
        public suscripcion suscripcion { get; set; }
        public string mensaje { get; set; }
        public string fecha_notificacion {get; set; } 

        public bool estado { get; set; }
    }
}
