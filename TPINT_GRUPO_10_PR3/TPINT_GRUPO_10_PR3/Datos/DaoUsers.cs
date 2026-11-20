using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Datos
{
    public class DaoUsers
    {
        private readonly AccesoDatos ds = new AccesoDatos();
        public DataTable obtenerUsers()
        {
           string sql = "SELECT * FROM Users";
            return ds.ObtenerTabla("Usuario", sql);
        }
        public bool ExisteUser(int IdUser)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE IdUser = @IdUser";

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUser", IdUser)
            };

            DataTable tabla = ds.ObtenerTabla("Users", sql, parametros);

            if (Convert.ToInt32(tabla.Rows[0][0]) > 0) return true;
            return false;
        }
        public DataTable obtenerUserPorNombre(string nombreUsuario)
        {
            string sql = "SELECT * FROM Users WHERE NombreUsuario = @NombreUsuario";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUsuario", nombreUsuario)
            };
            return ds.ObtenerTabla("Users", sql, parametros);
        }
        public bool modificarUser(int IdUser, string nombreUsuario, int idRol)
        {
            string sql = "UPDATE Users SET NombreUsuario = @NombreUsuario, IdRol = @IdRol WHERE IdUser = @IdUser";
            SqlParameter[] parametros =
            {
                new SqlParameter("@IdUser", IdUser),
                new SqlParameter("@NombreUsuario", nombreUsuario),
                new SqlParameter("@IdRol", idRol)
            };
            return ds.EjecutarComando(sql, parametros) > 0;
        }
        public bool existeNombreUser(string nombreUsuario)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE NombreUsuario = @NombreUsuario";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUsuario", nombreUsuario)
            };
            DataTable tabla = ds.ObtenerTabla("Users", sql, parametros);
            if (Convert.ToInt32(tabla.Rows[0][0]) > 0) return true;
            return false;
        }
        public DataTable obtenerPassword(string nombreUsuario)
        {
            string sql = "SELECT Password FROM Users WHERE NombreUsuario = @NombreUsuario";
            SqlParameter[] parametros =
            {
                new SqlParameter("@NombreUsuario", nombreUsuario)
            };
            return ds.ObtenerTabla("Users", sql, parametros);
        }
    }
}