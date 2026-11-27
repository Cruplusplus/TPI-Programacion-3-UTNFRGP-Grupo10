using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Entidades
{
    public class HorarioMedico
    {
        public int IdHorarioMedico { get; set; }
        public int IdMedico { get; set; }
        public DayOfWeek Dia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool EstadoHorario { get; set; }

        public string DiaNombre
        {
            get
            {
                switch (Dia)
                {
                    case DayOfWeek.Sunday: return "Domingo";
                    case DayOfWeek.Monday: return "Lunes";
                    case DayOfWeek.Tuesday: return "Martes";
                    case DayOfWeek.Wednesday: return "Miércoles";
                    case DayOfWeek.Thursday: return "Jueves";
                    case DayOfWeek.Friday: return "Viernes";
                    case DayOfWeek.Saturday: return "Sábado";
                    default: return string.Empty;
                }
            }
        }
    }
}