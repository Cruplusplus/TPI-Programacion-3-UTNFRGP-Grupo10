using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Entidades
{
    public class Provincia
    {
        public int IdProvincia { get; set; }
        public string NombreProvincia { get; set; }

        public override string ToString()
        {
            return NombreProvincia;
        }
    }
}