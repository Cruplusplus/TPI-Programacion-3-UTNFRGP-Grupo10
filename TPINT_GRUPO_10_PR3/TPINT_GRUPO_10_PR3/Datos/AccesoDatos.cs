using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Datos
{
    public class AccesoDatos
    {
        string cadenaConexion = "Data Source=localhost\\sqlexpress; Initial Catalog=Clinica_TPI;Integrated Security = True";

        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        // SELECT
        public DataTable ObtenerTabla(string nombreTabla, string sqlQuery, SqlParameter[] parametros = null)
        {
            DataTable dt = new DataTable(nombreTabla);
            using (SqlConnection cn = ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros);
                }
                SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                adaptador.Fill(dt);
            }
            return dt;
        }

        // INSERT / UPDATE / DELETE
        public int EjecutarComando(string sqlQuery, SqlParameter[] parametros = null)
        {
            using (SqlConnection conexion = ObtenerConexion())
            {
                using (SqlCommand sc = new SqlCommand(sqlQuery, conexion))
                {
                    if (parametros != null)
                    {
                        sc.Parameters.AddRange(parametros);
                    }
                    if (conexion.State == ConnectionState.Closed)
                    {
                        conexion.Open();
                    }
                    return sc.ExecuteNonQuery();
                }
            }
        }
    }
}