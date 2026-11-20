using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Datos
{
    public class DaoUsers
    {
        private readonly AccesoDatos ds = new AccesoDatos();

        public DataTable obtenerUsers()
        {
            string sql = "SELECT * FROM Users";
            return ds.ObtenerTabla("Users", sql);
        }

        public bool ExisteUser(int IdUser)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE IdUser = @IdUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUser", IdUser)
            };
            DataTable tabla = ds.ObtenerTabla("Users", sql, parametros);
            return Convert.ToInt32(tabla.Rows[0][0]) > 0;
        }

        public DataTable obtenerUserPorNombre(string nombreUsuario)
        {
            string sql = "SELECT * FROM Users WHERE NombreUser = @NombreUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUser", nombreUsuario)
            };
            return ds.ObtenerTabla("Users", sql, parametros);
        }

        public bool modificarUser(int IdUser, string nombreUsuario, int idRol)
        {
            string sql = "UPDATE Users SET NombreUser = @NombreUser, IdRol = @IdRol WHERE IdUser = @IdUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUser", IdUser),
                new SqlParameter("@NombreUser", nombreUsuario),
                new SqlParameter("@IdRol", idRol)
            };
            return ds.EjecutarComando(sql, parametros) > 0;
        }

        public bool existeNombreUser(string nombreUsuario)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE NombreUser = @NombreUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUser", nombreUsuario)
            };
            DataTable tabla = ds.ObtenerTabla("Users", sql, parametros);
            return Convert.ToInt32(tabla.Rows[0][0]) > 0;
        }

        public DataTable obtenerPassword(string nombreUsuario)
        {
            string sql = "SELECT Passwords FROM Users WHERE NombreUser = @NombreUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUser", nombreUsuario)
            };
            return ds.ObtenerTabla("Users", sql, parametros);
        }

        public bool modificarPassword(string nombreUsuario, string nuevaPassword)
        {
            if (obtenerUserPorNombre(nombreUsuario).Rows.Count == 0)
            {
                throw new Exception("El usuario no existe.");
            }

            string sql = "UPDATE Users SET Passwords = @NuevaPassword WHERE NombreUser = @NombreUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUser", nombreUsuario),
                new SqlParameter("@NuevaPassword", nuevaPassword)
            };
            return ds.EjecutarComando(sql, parametros) > 0;
        }
        public DataTable obtenerIdPorNombre(string nombreUsuario)
        {
            string sql = "SELECT IdUser FROM Users WHERE NombreUser = @NombreUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUser", nombreUsuario)
            };
            return ds.ObtenerTabla("Users", sql, parametros);
        }
        public int obtenerRolPorNombre(string nombreUsuario)
        {
            string sql = "SELECT IdRol FROM Users WHERE NombreUser = @NombreUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUser", nombreUsuario)
            };
            DataTable dt = ds.ObtenerTabla("Users", sql, parametros);
            if (dt.Rows.Count == 0) return -1;
            return Convert.ToInt32(dt.Rows[0]["IdRol"]);
        }
    }
}