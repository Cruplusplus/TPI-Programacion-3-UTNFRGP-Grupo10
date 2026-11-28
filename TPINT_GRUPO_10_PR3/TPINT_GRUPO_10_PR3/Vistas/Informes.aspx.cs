using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPINT_GRUPO_10_PR3.Datos;

namespace TPINT_GRUPO_10_PR3.Vistas
{
    public partial class Informes : System.Web.UI.Page
    {
        private readonly InformeBLL _informeBLL = new InformeBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarComboMedicos();
                LimpiarResultadosGeneral();
                LimpiarResultadosMedico();
            }
        }

        private void CargarComboMedicos()
        {
            try
            {
                DataTable dt = _informeBLL.ListarMedicos();
                ddlMedicos.DataSource = dt;
                ddlMedicos.DataTextField = "NombreCompleto";
                ddlMedicos.DataValueField = "IdMedico";
                ddlMedicos.DataBind();
                ddlMedicos.Items.Insert(0, new ListItem("Seleccione un médico", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje(lblMensajeMed, "Error al cargar médicos: " + ex.Message, false);
            }
        }

        protected void btnGenerarGen_Click(object sender, EventArgs e)
        {
            LimpiarResultadosGeneral();

            if (string.IsNullOrWhiteSpace(txtFechaDesdeGen.Text) || string.IsNullOrWhiteSpace(txtFechaHastaGen.Text))
            {
                MostrarMensaje(lblMensajeGen, "Ingrese ambas fechas correctamente.", false);
                return;
            }

            if (!DateTime.TryParse(txtFechaDesdeGen.Text, out DateTime fechaDesde) ||
                !DateTime.TryParse(txtFechaHastaGen.Text, out DateTime fechaHasta))
            {
                MostrarMensaje(lblMensajeGen, "Ingrese ambas fechas correctamente.", false);
                return;
            }

            if (fechaDesde.Date > fechaHasta.Date)
            {
                MostrarMensaje(lblMensajeGen, "Ingrese ambas fechas correctamente. La fecha desde no puede ser posterior a la fecha hasta.", false);
                return;
            }

            try
            {
                DataTable dt = _informeBLL.ObtenerAsistenciaGeneral(fechaDesde, fechaHasta);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow fila = dt.Rows[0];
                    int total = Convert.ToInt32(fila["TotalTurnos"]);

                    if (total == 0)
                    {
                        MostrarMensaje(lblMensajeGen, "No se encontraron turnos en el período seleccionado.", false);
                        return;
                    }

                    int presentes = Convert.ToInt32(fila["Presentes"]);
                    decimal porcPresentes = Convert.ToDecimal(fila["PorcPresentes"]);
                    int ausentes = Convert.ToInt32(fila["Ausentes"]);
                    decimal porcAusentes = Convert.ToDecimal(fila["PorcAusentes"]);
                    int pendientes = Convert.ToInt32(fila["Pendientes"]);
                    decimal porcPendientes = Convert.ToDecimal(fila["PorcPendientes"]);

                    lblTotalGen.Text = $"Total de turnos: {total}";
                    lblPresentesGen.Text = $"Presentes: {presentes} ({porcPresentes:0.##}%)";
                    lblAusentesGen.Text = $"Ausentes: {ausentes} ({porcAusentes:0.##}%)";
                    lblPendientesGen.Text = $"Pendientes: {pendientes} ({porcPendientes:0.##}%)";

                    MostrarMensaje(lblMensajeGen, "Informe general generado correctamente.", true);
                }
                else
                {
                    MostrarMensaje(lblMensajeGen, "No se encontraron turnos en el período seleccionado.", false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(lblMensajeGen, "Ocurrió un error al generar el informe: " + ex.Message, false);
            }
        }

        protected void btnGenerarMed_Click(object sender, EventArgs e)
        {
            LimpiarResultadosMedico();

            if (ddlMedicos.SelectedValue == "0" || string.IsNullOrEmpty(ddlMedicos.SelectedValue))
            {
                MostrarMensaje(lblMensajeMed, "Debe seleccionar un médico.", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFechaDesdeMed.Text) || string.IsNullOrWhiteSpace(txtFechaHastaMed.Text))
            {
                MostrarMensaje(lblMensajeMed, "Ingrese ambas fechas correctamente.", false);
                return;
            }

            if (!DateTime.TryParse(txtFechaDesdeMed.Text, out DateTime fechaDesde) ||
                !DateTime.TryParse(txtFechaHastaMed.Text, out DateTime fechaHasta))
            {
                MostrarMensaje(lblMensajeMed, "Ingrese ambas fechas correctamente.", false);
                return;
            }

            if (fechaDesde.Date > fechaHasta.Date)
            {
                MostrarMensaje(lblMensajeMed, "Ingrese ambas fechas correctamente. La fecha desde no puede ser posterior a la fecha hasta.", false);
                return;
            }

            try
            {
                int idMedico = Convert.ToInt32(ddlMedicos.SelectedValue);
                DataTable dt = _informeBLL.ObtenerAsistenciaPorMedico(idMedico, fechaDesde, fechaHasta);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow fila = dt.Rows[0];
                    int total = Convert.ToInt32(fila["TotalTurnos"]);

                    if (total == 0)
                    {
                        MostrarMensaje(lblMensajeMed, "No se encontraron turnos para el médico en el período seleccionado.", false);
                        return;
                    }

                    int presentes = Convert.ToInt32(fila["Presentes"]);
                    decimal porcPresentes = Convert.ToDecimal(fila["PorcPresentes"]);
                    int ausentes = Convert.ToInt32(fila["Ausentes"]);
                    decimal porcAusentes = Convert.ToDecimal(fila["PorcAusentes"]);
                    int pendientes = Convert.ToInt32(fila["Pendientes"]);
                    decimal porcPendientes = Convert.ToDecimal(fila["PorcPendientes"]);

                    lblTotalMed.Text = $"Total de turnos: {total}";
                    lblPresentesMed.Text = $"Presentes: {presentes} ({porcPresentes:0.##}%)";
                    lblAusentesMed.Text = $"Ausentes: {ausentes} ({porcAusentes:0.##}%)";
                    lblPendientesMed.Text = $"Pendientes: {pendientes} ({porcPendientes:0.##}%)";

                    MostrarMensaje(lblMensajeMed, "Informe por médico generado correctamente.", true);
                }
                else
                {
                    MostrarMensaje(lblMensajeMed, "No se encontraron turnos para el médico en el período seleccionado.", false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(lblMensajeMed, "Ocurrió un error al generar el informe: " + ex.Message, false);
            }
        }

        private void MostrarMensaje(Label lbl, string mensaje, bool esExito)
        {
            lbl.Text = mensaje;
            lbl.ForeColor = esExito ? Color.Green : Color.Red;
            lbl.Visible = true;
        }

        private void LimpiarResultadosGeneral()
        {
            lblMensajeGen.Text = string.Empty;
            lblTotalGen.Text = "Total de turnos: -";
            lblPresentesGen.Text = "Presentes: -";
            lblAusentesGen.Text = "Ausentes: -";
            lblPendientesGen.Text = "Pendientes: -";
        }

        private void LimpiarResultadosMedico()
        {
            lblMensajeMed.Text = string.Empty;
            lblTotalMed.Text = "Total de turnos: -";
            lblPresentesMed.Text = "Presentes: -";
            lblAusentesMed.Text = "Ausentes: -";
            lblPendientesMed.Text = "Pendientes: -";
        }
    }
}
