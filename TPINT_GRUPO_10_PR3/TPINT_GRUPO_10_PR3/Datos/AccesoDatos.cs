using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Datos
{
    public class AccesoDatos
    {
        private readonly string _cadenaConexion = "Data Source=localhost\\sqlexpress; Initial Catalog=Clinica_TPI;Integrated Security = True";
        private SqlConnection _conexion;
        private SqlCommand _comando;
        private SqlDataReader _lector;
        private SqlTransaction _transaccion;

        // Solo lectura
        public SqlDataReader Lector => _lector;

        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_cadenaConexion);
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
         
        // Metodos para objetos y transacciones
        public void SetearConsulta(string consulta)
        {
            if (_comando == null)
            {
                _comando = new SqlCommand();
            }
            _comando.Parameters.Clear();
            _comando.CommandType = CommandType.Text;
            _comando.CommandText = consulta;
        }

        public void SetearParametro(string nombre, object valor)
        {
            _comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
        }

        public void IniciarTransaccion()
        {
            _conexion = ObtenerConexion();
            _conexion.Open();
            _transaccion = _conexion.BeginTransaction();
            if (_comando == null) _comando = new SqlCommand();
            _comando.Connection = _conexion;
            _comando.Transaction = _transaccion;
        }

        public void ConfirmarTransaccion()
        {
            try
            {
                _transaccion?.Commit();
            }
            finally
            {
                LimpiarEstadoTransaccional();
            }
        }

        public void CancelarTransaccion()
        {
            try
            {
                _transaccion.Rollback();
            }
            catch
            {

            }
            finally
            {
                LimpiarEstadoTransaccional();
            }
        }

        public void LimpiarEstadoTransaccional()
        {
            _transaccion = null;
            if (_comando != null)
            {
                _comando.Parameters.Clear();
                _comando.Transaction = null;
            }
            if(_conexion != null && _conexion.State != ConnectionState.Closed)
            {
                _conexion.Close();
            }
        }

        public void EjecutarLectura()
        {
            if (_transaccion == null)
            {
                _conexion = ObtenerConexion();
                _comando.Connection = _conexion;
                _conexion.Open();
            }
            _lector = _comando.ExecuteReader();
        }

        public void EjecutarAccion()
        {
            if (_transaccion == null)
            {
                _conexion = ObtenerConexion();
                _comando.Connection = _conexion;
                _conexion.Open();
            }
            _comando.ExecuteNonQuery();
        }

        public int EjecutarScalar()
        {
            if (_transaccion == null)
            {
                _conexion = ObtenerConexion();
                _comando.Connection = _conexion;
                _conexion.Open();
            }
            return Convert.ToInt32(_comando.ExecuteScalar());
        }

        public void CerrarConexion()
        {
            if (_lector != null && !_lector.IsClosed)
            {
                _lector.Close();
            }
            if (_transaccion == null && _conexion != null && _conexion.State != ConnectionState.Closed)
            {
                _conexion.Close();
            }
        }
    }
}