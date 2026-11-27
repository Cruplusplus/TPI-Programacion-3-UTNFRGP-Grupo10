using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Entidades
{
    public class Paciente : Persona
    {
        public int IdPaciente
        {
            get { return IdPersona; }
            set { IdPersona = value; }
        }

        public Provincia ProvinciaObj { get; set;}
        public Localidad LocalidadObj { get; set; }
        public bool EstadoPaciente { get; set; }

        public Paciente()
        {
            ProvinciaObj = new Provincia();
            LocalidadObj = new Localidad();
        }

        public string DNI => Dni;
        public string Email => CorreoElectronico;
        public string Provincia => ProvinciaObj != null ? ProvinciaObj.NombreProvincia : string.Empty;
        public string Localidad => LocalidadObj != null ? LocalidadObj.NombreLocalidad : string.Empty;
    }
}