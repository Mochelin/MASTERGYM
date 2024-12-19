namespace Gimrat.Entidades
{
    public class horarioprincipal
    {
        public int id_horario { get; set; }
        public string dia { get; set; }
        public TimeOnly inicio_am { get; set; }
        public TimeOnly fin_am { get; set; }
        public TimeOnly inicio_pm { get; set; }
        public TimeOnly fin_pm { get; set; }

    }
}
