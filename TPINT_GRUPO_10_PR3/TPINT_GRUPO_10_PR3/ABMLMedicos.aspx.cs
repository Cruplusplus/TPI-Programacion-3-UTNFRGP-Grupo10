using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Entidades;
//using Negocio;

namespace TPINT_GRUPO_10_PR3
{
    public partial class Medicos : Page
    {
        //private MedicoNegocio medNeg = new MedicoNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CargarProvincias();
                //CargarEspecialidades();
                //CargarGrilla();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Logica de guardado / modificacion
            lblMensaje.Text = "Registro procesado correctamente.";
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            lblMensaje.Text = string.Empty;
        }

        private void LimpiarControles()
        {
            txtLegajo.Text = string.Empty;
            txtDni.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            if (ddlSexo.Items.Count > 0) ddlSexo.SelectedIndex = 0;
            txtNacionalidad.Text = string.Empty;
            txtFechaNac.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            if (ddlProvincia.Items.Count > 0) ddlProvincia.SelectedIndex = 0;
            if (ddlLocalidad.Items.Count > 0) ddlLocalidad.SelectedIndex = 0;
            txtCorreo.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            if (ddlEspecialidades.Items.Count > 0) ddlEspecialidades.SelectedIndex = 0;

            foreach (ListItem item in cblDiasAtencion.Items)
            {
                item.Selected = false;
            }

            if (ddlHoraInicio.Items.Count > 0) ddlHoraInicio.SelectedIndex = 0;
            if (ddlHoraFin.Items.Count > 0) ddlHoraFin.SelectedIndex = 0;

            txtUsuario.Text = string.Empty;
            txtContrasenia.Text = string.Empty;
            txtRepetirContrasenia.Text = string.Empty;
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Carga en cascada de localidades
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            // Filtrado por LIKE
        }

        protected void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = string.Empty;
            //CargarGrilla();
        }

        protected void GridViewMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMedicos.PageIndex = e.NewPageIndex;
            //CargarGrilla();
        }

        protected void GridViewMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EventoEliminar")
            {
                // Baja logica
            }
        }
    }
}
