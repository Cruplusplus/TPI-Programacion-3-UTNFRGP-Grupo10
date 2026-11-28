using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TPINT_GRUPO_10_PR3.Datos
{
    public class InformeDAL
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();

        public DataTable ListarMedicos()
        {
            string sql = @"
                SELECT 
                    m.IdMedico,
                    (p.Apellido + ', ' + p.Nombre) AS NombreCompleto
                FROM Medico m
                INNER JOIN Persona p ON p.IdPersona = m.IdMedico
                WHERE m.EstadoMedico = 1
                ORDER BY p.Apellido, p.Nombre;";

            return _accesoDatos.ObtenerTabla("Medicos", sql);
        }

        public DataTable ObtenerAsistenciaGeneral(DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = @"
                WITH Conteos AS (
                    SELECT 
                        COUNT(*) AS TotalTurnos,
                        SUM(CASE WHEN Asistencia = 1 THEN 1 ELSE 0 END) AS Presentes,
                        SUM(CASE WHEN Asistencia = 0 THEN 1 ELSE 0 END) AS Ausentes,
                        SUM(CASE WHEN Asistencia IS NULL THEN 1 ELSE 0 END) AS Pendientes
                    FROM Turno
                    WHERE EstadoTurno = 1
                      AND FechaTurno >= @FechaDesde
                      AND FechaTurno <= @FechaHasta
                )
                SELECT 
                    TotalTurnos,
                    Presentes,
                    CASE WHEN TotalTurnos > 0 THEN CAST(ROUND((Presentes * 100.0) / TotalTurnos, 2) AS DECIMAL(5,2)) ELSE 0.00 END AS PorcPresentes,
                    Ausentes,
                    CASE WHEN TotalTurnos > 0 THEN CAST(ROUND((Ausentes * 100.0) / TotalTurnos, 2) AS DECIMAL(5,2)) ELSE 0.00 END AS PorcAusentes,
                    Pendientes,
                    CASE WHEN TotalTurnos > 0 THEN CAST(ROUND((Pendientes * 100.0) / TotalTurnos, 2) AS DECIMAL(5,2)) ELSE 0.00 END AS PorcPendientes
                FROM Conteos;";

            SqlParameter[] parametros =
            {
                new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde.Date },
                new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta.Date }
            };

            return _accesoDatos.ObtenerTabla("AsistenciaGeneral", sql, parametros);
        }

        public DataTable ObtenerAsistenciaPorMedico(int idMedico, DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = @"
                WITH Conteos AS (
                    SELECT 
                        COUNT(*) AS TotalTurnos,
                        SUM(CASE WHEN Asistencia = 1 THEN 1 ELSE 0 END) AS Presentes,
                        SUM(CASE WHEN Asistencia = 0 THEN 1 ELSE 0 END) AS Ausentes,
                        SUM(CASE WHEN Asistencia IS NULL THEN 1 ELSE 0 END) AS Pendientes
                    FROM Turno
                    WHERE EstadoTurno = 1
                      AND IdMedico = @IdMedico
                      AND FechaTurno >= @FechaDesde
                      AND FechaTurno <= @FechaHasta
                )
                SELECT 
                    TotalTurnos,
                    Presentes,
                    CASE WHEN TotalTurnos > 0 THEN CAST(ROUND((Presentes * 100.0) / TotalTurnos, 2) AS DECIMAL(5,2)) ELSE 0.00 END AS PorcPresentes,
                    Ausentes,
                    CASE WHEN TotalTurnos > 0 THEN CAST(ROUND((Ausentes * 100.0) / TotalTurnos, 2) AS DECIMAL(5,2)) ELSE 0.00 END AS PorcAusentes,
                    Pendientes,
                    CASE WHEN TotalTurnos > 0 THEN CAST(ROUND((Pendientes * 100.0) / TotalTurnos, 2) AS DECIMAL(5,2)) ELSE 0.00 END AS PorcPendientes
                FROM Conteos;";

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdMedico", SqlDbType.Int) { Value = idMedico },
                new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde.Date },
                new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta.Date }
            };

            return _accesoDatos.ObtenerTabla("AsistenciaMedico", sql, parametros);
        }
    }
}
