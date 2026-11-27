using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Entidades
{
    public class Localidad
    {
        public int IdLocalidad { get; set; }
        public int IdProvincia { get; set; }
        public string NombreLocalidad { get; set; }

        public override string ToString()
        {
            return NombreLocalidad;
        }
    }
}