using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Entidades
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public int IdLocalidad { get; set; }
        public char dni { get; set; }
        public char nombre { get; set; }
        public char apellido { get; set; }
        public char sexo { get; set; }
        public char nacionalidad { get; set; }
        public char fechaNacimiento { get; set; }
        public char dirrecion { get; set; }
        public char correElectronico { get; set; }
        public char telefono { get; set; }
        public bool estado { get; set; }


    }
}