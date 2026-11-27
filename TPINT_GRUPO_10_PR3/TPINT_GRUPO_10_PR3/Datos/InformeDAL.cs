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
                    WHERE EstadoTurno = 1
                      AND FechaTurno >= @FechaDesde
                      AND FechaTurno <= @FechaHasta
                )
                SELECT 
                    CASE 
                        WHEN t.Asistencia = 1 THEN 'Presente'
                        WHEN t.Asistencia = 0 THEN 'Ausente'
                        ELSE 'Pendiente'
                    END AS Asistencia,
                    COUNT(*) AS CantidadTurnos,
                    CASE 
                        WHEN tot.TotalTurnos > 0 
                        THEN CAST(ROUND((COUNT(*) * 100.0) / tot.TotalTurnos, 2) AS DECIMAL(5,2))
                        ELSE CAST(0.00 AS DECIMAL(5,2))
                    END AS Porcentaje
                FROM Turno t
                CROSS JOIN Totales tot
                WHERE t.EstadoTurno = 1
                  AND t.FechaTurno >= @FechaDesde
                  AND t.FechaTurno <= @FechaHasta
                GROUP BY 
                    CASE 
                        WHEN t.Asistencia = 1 THEN 'Presente'
                        WHEN t.Asistencia = 0 THEN 'Ausente'
                        ELSE 'Pendiente'
                    END,
                    tot.TotalTurnos
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
                    e.NombreEspecialidad AS Especialidad,
                    COUNT(t.IdTurno) AS CantidadTurnos
                FROM Especialidad e
                LEFT JOIN Medico m ON m.IdEspecialidad = e.IdEspecialidad AND m.EstadoMedico = 1
                LEFT JOIN Turno t ON t.IdMedico = m.IdMedico 
                    AND t.EstadoTurno = 1
                    AND t.FechaTurno >= @FechaDesde
                    AND t.FechaTurno <= @FechaHasta
                GROUP BY e.IdEspecialidad, e.NombreEspecialidad
                ORDER BY CantidadTurnos DESC, e.NombreEspecialidad ASC;";

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
                    m.IdMedico,
                    m.Legajo,
                    (p.Apellido + ', ' + p.Nombre) AS Medico,
                    e.NombreEspecialidad AS Especialidad,
                    COUNT(t.IdTurno) AS CantidadTurnos
                FROM Medico m
                INNER JOIN Persona p ON p.IdPersona = m.IdMedico
                INNER JOIN Especialidad e ON e.IdEspecialidad = m.IdEspecialidad
                LEFT JOIN Turno t ON t.IdMedico = m.IdMedico 
                    AND t.EstadoTurno = 1
                    AND t.FechaTurno >= @FechaDesde
                    AND t.FechaTurno <= @FechaHasta
                WHERE m.EstadoMedico = 1
                GROUP BY m.IdMedico, m.Legajo, p.Apellido, p.Nombre, e.NombreEspecialidad
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
