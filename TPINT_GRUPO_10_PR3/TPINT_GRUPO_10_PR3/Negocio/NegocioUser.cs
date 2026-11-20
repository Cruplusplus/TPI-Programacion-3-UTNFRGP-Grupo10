using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TPINT_GRUPO_10_PR3.Datos;

namespace TPINT_GRUPO_10_PR3.Negocio
{
    public class NegocioUser
    {
        public bool ValidarUsuario(string nombreUsuario, string password)
        {
            DataTable tabla = new DaoUsers().obtenerUserPorNombre(nombreUsuario);
            if (tabla.Rows.Count == 0)
            {
                return false;
            }

            string passwordBD = tabla.Rows[0]["Password"].ToString();

            if (password == passwordBD)
            {
                return true;
            }

            return false;
        }
    }
}