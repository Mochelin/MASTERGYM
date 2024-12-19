namespace Gimrat.Entidades
{
    public class evtrainer
    {
        public int id_eventoT { get; set; }
        public trainer otrainer { get; set; }
        public eventos oevento { get; set; }
        public DateTime fecha_evento { get; set; }

    }
}
