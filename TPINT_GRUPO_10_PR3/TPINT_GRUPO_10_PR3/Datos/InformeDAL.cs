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

        public DataTable ObtenerEstadisticasAsistencia(DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = @"
                WITH Totales AS (
                    SELECT COUNT(*) AS TotalTurnos
                    FROM Turno
                    WHERE Estado = 1
                      AND Fecha >= @FechaDesde
                      AND Fecha <= @FechaHasta
                )
                SELECT 
                    ISNULL(t.Asistencia, 'Pendiente') AS Asistencia,
                    COUNT(*) AS CantidadTurnos,
                    CASE 
                        WHEN tot.TotalTurnos > 0 
                        THEN CAST(ROUND((COUNT(*) * 100.0) / tot.TotalTurnos, 2) AS DECIMAL(5,2))
                        ELSE CAST(0.00 AS DECIMAL(5,2))
                    END AS Porcentaje
                FROM Turno t
                CROSS JOIN Totales tot
                WHERE t.Estado = 1
                  AND t.Fecha >= @FechaDesde
                  AND t.Fecha <= @FechaHasta
                GROUP BY ISNULL(t.Asistencia, 'Pendiente'), tot.TotalTurnos
                ORDER BY CantidadTurnos DESC;";

            SqlParameter[] parametros =
            {
                new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde.Date },
                new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta.Date }
            };

            return _accesoDatos.ObtenerTabla("EstadisticasAsistencia", sql, parametros);
        }

        public DataTable ObtenerTurnosPorEspecialidad(DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = @"
                SELECT 
                    e.IdEspecialidad,
                    e.Nombre AS Especialidad,
                    COUNT(t.IdTurno) AS CantidadTurnos
                FROM Especialidad e
                LEFT JOIN Medico m ON m.IdEspecialidad = e.IdEspecialidad AND m.Estado = 1
                LEFT JOIN Turno t ON t.IdMedico = m.IdPersona 
                    AND t.Estado = 1
                    AND t.Fecha >= @FechaDesde
                    AND t.Fecha <= @FechaHasta
                GROUP BY e.IdEspecialidad, e.Nombre
                ORDER BY CantidadTurnos DESC, e.Nombre ASC;";

            SqlParameter[] parametros =
            {
                new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde.Date },
                new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta.Date }
            };

            return _accesoDatos.ObtenerTabla("TurnosPorEspecialidad", sql, parametros);
        }

        public DataTable ObtenerTurnosPorMedico(DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = @"
                SELECT 
                    m.IdPersona AS IdMedico,
                    m.Legajo,
                    (p.Apellido + ', ' + p.Nombre) AS Medico,
                    e.Nombre AS Especialidad,
                    COUNT(t.IdTurno) AS CantidadTurnos
                FROM Medico m
                INNER JOIN Persona p ON p.IdPersona = m.IdPersona
                INNER JOIN Especialidad e ON e.IdEspecialidad = m.IdEspecialidad
                LEFT JOIN Turno t ON t.IdMedico = m.IdPersona 
                    AND t.Estado = 1
                    AND t.Fecha >= @FechaDesde
                    AND t.Fecha <= @FechaHasta
                WHERE m.Estado = 1
                GROUP BY m.IdPersona, m.Legajo, p.Apellido, p.Nombre, e.Nombre
                ORDER BY CantidadTurnos DESC, Medico ASC;";

            SqlParameter[] parametros =
            {
                new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde.Date },
                new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta.Date }
            };

            return _accesoDatos.ObtenerTabla("TurnosPorMedico", sql, parametros);
        }
    }
}