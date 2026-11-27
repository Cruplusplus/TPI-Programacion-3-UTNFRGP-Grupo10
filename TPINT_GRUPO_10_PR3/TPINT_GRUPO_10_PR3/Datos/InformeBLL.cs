using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TPINT_GRUPO_10_PR3.Datos
{
    public class InformeBLL
    {
        private readonly InformeDAL _informeDAL = new InformeDAL();

        private void ValidarRangoFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde.Date > fechaHasta.Date)
            {
                throw new ArgumentException("La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.");
            }
        }

        public DataTable ObtenerEstadisticasAsistencia(DateTime fechaDesde, DateTime fechaHasta)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            return _informeDAL.ObtenerEstadisticasAsistencia(fechaDesde, fechaHasta);
        }

        public DataTable ObtenerTurnosPorEspecialidad(DateTime fechaDesde, DateTime fechaHasta)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            return _informeDAL.ObtenerTurnosPorEspecialidad(fechaDesde, fechaHasta);
        }

        public DataTable ObtenerTurnosPorMedico(DateTime fechaDesde, DateTime fechaHasta)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            return _informeDAL.ObtenerTurnosPorMedico(fechaDesde, fechaHasta);
        }
    }
}