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

            string passwordBD = tabla.Rows[0]["Passwords"].ToString();

            if (password == passwordBD)
            {
                return true;
            }

            return false;
        }
        public bool Recovery(string nombreUsuario, string password)
        {
            DaoUsers daoUsers = new DaoUsers();
            if(daoUsers.modificarPassword(nombreUsuario, password))
            {
                return true;
            }
            return false;
        }
        public int obtenerIdPorNombre(string nombreUsuario)
        {
            DaoUsers daoUsers = new DaoUsers();
            return Convert.ToInt32(daoUsers.obtenerIdPorNombre(nombreUsuario).Rows[0]["IdUser"]);
        }
        public int obtenerRolPorNombre(string nombreUsuario)
        {
            DaoUsers daoUsers = new DaoUsers();
            return daoUsers.obtenerRolPorNombre(nombreUsuario);
        }
    }
}