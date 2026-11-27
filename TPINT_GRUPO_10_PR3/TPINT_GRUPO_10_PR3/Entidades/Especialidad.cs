using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Entidades
{
    public class Especialidad
    {
        public int IdEspecialidad { get; set; }
        public string NombreEspecialidad { get; set; }

        // Sobreescribimos el metodo ToString() que ejecuta Object por defecto para devolver el nombre de la especialidad en vez del tipo (TPINT_GRUPO_10_PR3.Entidades.Especialidad)
        public override string ToString()
        {
            return NombreEspecialidad;
        }
    }
}