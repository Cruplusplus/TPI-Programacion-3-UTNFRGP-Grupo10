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

        public DataTable ListarMedicos()
        {
            return _informeDAL.ListarMedicos();
        }

        private void ValidarRangoFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde.Date > fechaHasta.Date)
            {
                throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta.");
            }
        }

        public DataTable ObtenerAsistenciaGeneral(DateTime fechaDesde, DateTime fechaHasta)
        {
            ValidarRangoFechas(fechaDesde, fechaHasta);
            return _informeDAL.ObtenerAsistenciaGeneral(fechaDesde, fechaHasta);
        }

        public DataTable ObtenerAsistenciaPorMedico(int idMedico, DateTime fechaDesde, DateTime fechaHasta)
        {
            if (idMedico <= 0)
            {
                throw new ArgumentException("Debe seleccionar un médico.");
            }
            ValidarRangoFechas(fechaDesde, fechaHasta);
            return _informeDAL.ObtenerAsistenciaPorMedico(idMedico, fechaDesde, fechaHasta);
        }
    }
}
