using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Entidades
{
    public class Medico : Persona
    {
        public int IdMedico
        {
            // value es una palabra reservada dentro de los set para un dato asignado por fuera de la clase ( medico.IdMedico = 15; )
            get { return IdPersona; }
            set { IdPersona = value; }
        }

        public string Legajo { get; set; }
        public Especialidad Especialidad { get; set; }
        public Usuario Usuario { get; set; }
        public List<HorarioMedico> Horarios { get; set; }
        public bool EstadoMedico { get; set; }

        public Medico()
        {
            Especialidad = new Especialidad();
            Usuario = new Usuario();
            Horarios = new List<HorarioMedico>();
        }

        // Propiedades auxiliares para mostrar en el GridView
        public string NombreEspecialidad => Especialidad != null ? Especialidad.NombreEspecialidad : string.Empty;

        public string DiasAtencion
        {
            get
            {
                if (Horarios == null || Horarios.Count == 0) return "Sin horarios";
                return string.Join(", ", Horarios.OrderBy(h => h.Dia).Select(h => h.DiaNombre));
            }
        }

        public string Horario
        {
            get
            {
                if (Horarios == null || Horarios.Count == 0) return "-";
                var primero = Horarios[0];
                return $"{primero.HoraInicio:hh\\:mm} a {primero.HoraFin:hh\\:mm}";
            }
        }
    }
}