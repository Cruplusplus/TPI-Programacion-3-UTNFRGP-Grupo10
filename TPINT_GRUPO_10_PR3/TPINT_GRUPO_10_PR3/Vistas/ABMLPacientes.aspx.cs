using System;
using System.Web.UI.WebControls;
using TPINT_GRUPO_10_PR3.Entidades;
using TPINT_GRUPO_10_PR3.Negocio;

namespace TPINT_GRUPO_10_PR3
{
    public partial class ABMLPacientes : PageBase
    {
        private readonly PacienteBLL pacBll = new PacienteBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidarRol(0);

            if (!IsPostBack)
            {
                CargarProvincias();
                CargarGrilla();
            }
        }

        private void CargarProvincias()
        {
            var provincias = pacBll.ListarProvincias();

            ddlProvincia.Items.Clear();
            ddlProvincia.DataSource = provincias;
            ddlProvincia.DataTextField = "NombreProvincia";
            ddlProvincia.DataValueField = "IdProvincia";
            ddlProvincia.DataBind();
            ddlProvincia.Items.Insert(0, new ListItem("-- Seleccione --", ""));

            ddlLocalidad.Items.Clear();
            ddlLocalidad.Items.Insert(0, new ListItem("-- Seleccione provincia --", ""));

            ddlFiltroProvincia.Items.Clear();
            ddlFiltroProvincia.DataSource = provincias;
            ddlFiltroProvincia.DataTextField = "NombreProvincia";
            ddlFiltroProvincia.DataValueField = "IdProvincia";
            ddlFiltroProvincia.DataBind();
            ddlFiltroProvincia.Items.Insert(0, new ListItem("Todas", "0"));
        }

        private void CargarLocalidades(int idProvincia)
        {
            ddlLocalidad.Items.Clear();
            ddlLocalidad.DataSource = pacBll.ListarLocalidades(idProvincia);
            ddlLocalidad.DataTextField = "NombreLocalidad";
            ddlLocalidad.DataValueField = "IdLocalidad";
            ddlLocalidad.DataBind();
            ddlLocalidad.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProvincia.SelectedValue))
            {
                int idProvincia = int.Parse(ddlProvincia.SelectedValue);
                CargarLocalidades(idProvincia);
            }
            else
            {
                ddlLocalidad.Items.Clear();
                ddlLocalidad.Items.Insert(0, new ListItem("-- Seleccione provincia --", ""));
            }
        }

        private void CargarGrilla()
        {
            string texto = txtBuscar.Text.Trim();
            int idProvincia = !string.IsNullOrEmpty(ddlFiltroProvincia.SelectedValue)
                ? int.Parse(ddlFiltroProvincia.SelectedValue)
                : 0;
            string sexo = ConvertirSexoACharString(ddlFiltroSexo.SelectedValue);

            gvPacientes.DataSource = pacBll.Listar(texto, idProvincia, sexo);
            gvPacientes.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Paciente pac = ArmarObjetoDesdeFormulario();

                if (ViewState["IdPacienteEditar"] == null)
                {
                    pacBll.Agregar(pac);
                    lblMensaje.Text = "Paciente registrado correctamente.";
                }
                else
                {
                    pac.IdPersona = (int)ViewState["IdPacienteEditar"];
                    pacBll.Modificar(pac);
                    lblMensaje.Text = "Paciente modificado correctamente.";
                }

                lblMensaje.ForeColor = System.Drawing.Color.Green;
                LimpiarFormulario();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        private Paciente ArmarObjetoDesdeFormulario()
        {
            if (string.IsNullOrEmpty(ddlSexo.SelectedValue))
                throw new Exception("Debe seleccionar el sexo del paciente.");

            if (string.IsNullOrEmpty(txtFechaNac.Text))
                throw new Exception("Debe ingresar la fecha de nacimiento.");

            if (string.IsNullOrEmpty(ddlLocalidad.SelectedValue))
                throw new Exception("Debe seleccionar una provincia y localidad.");

            Paciente pac = new Paciente();
            pac.Dni = txtDni.Text.Trim();
            pac.Nombre = txtNombre.Text.Trim();
            pac.Apellido = txtApellido.Text.Trim();
            pac.Sexo = ConvertirSexoAChar(ddlSexo.SelectedValue);
            pac.Nacionalidad = txtNacionalidad.Text.Trim();
            pac.FechaNacimiento = DateTime.Parse(txtFechaNac.Text);
            pac.Direccion = txtDireccion.Text.Trim();
            pac.IdLocalidad = int.Parse(ddlLocalidad.SelectedValue);
            pac.CorreoElectronico = txtEmail.Text.Trim();
            pac.Telefono = txtTelefono.Text.Trim();
            pac.Estado = true;
            pac.EstadoPaciente = true;

            return pac;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblMensaje.Text = string.Empty;
        }

        private void LimpiarFormulario()
        {
            ViewState["IdPacienteEditar"] = null;
            lblTituloForm.Text = "Alta de Paciente";
            btnGuardar.Text = "Guardar";
            btnGuardar.OnClientClick = string.Empty;

            txtDni.Text = string.Empty;
            txtDni.Enabled = true;

            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            if (ddlSexo.Items.Count > 0) ddlSexo.SelectedIndex = 0;
            txtNacionalidad.Text = string.Empty;
            txtFechaNac.Text = string.Empty;
            txtDireccion.Text = string.Empty;

            if (ddlProvincia.Items.Count > 0) ddlProvincia.SelectedIndex = 0;
            ddlLocalidad.Items.Clear();
            ddlLocalidad.Items.Insert(0, new ListItem("-- Seleccione provincia --", ""));

            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
        }

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Modificar" && e.CommandName != "Eliminar")
                return;

            try
            {
                string dni = e.CommandArgument.ToString();

                if (e.CommandName == "Modificar")
                {
                    CargarPacienteParaEdicion(dni);
                }
                else if (e.CommandName == "Eliminar")
                {
                    pacBll.EliminarPorDni(dni);
                    lblMensaje.Text = "Paciente dado de baja correctamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    LimpiarFormulario();
                    CargarGrilla();
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void CargarPacienteParaEdicion(string dni)
        {
            Paciente pac = pacBll.ObtenerPorDni(dni);
            if (pac == null) return;

            ViewState["IdPacienteEditar"] = pac.IdPersona;
            lblTituloForm.Text = "Modificación de Paciente - DNI " + pac.Dni;
            btnGuardar.Text = "Guardar Cambios";
            btnGuardar.OnClientClick = "return confirm('¿Está seguro que desea guardar los cambios realizados?');";

            txtDni.Text = pac.Dni;
            txtDni.Enabled = false;

            txtNombre.Text = pac.Nombre;
            txtApellido.Text = pac.Apellido;
            ddlSexo.SelectedValue = ConvertirCharASexoDropdown(pac.Sexo);
            txtNacionalidad.Text = pac.Nacionalidad;
            txtFechaNac.Text = pac.FechaNacimiento.ToString("yyyy-MM-dd");
            txtDireccion.Text = pac.Direccion;

            ddlProvincia.SelectedValue = pac.ProvinciaObj.IdProvincia.ToString();
            CargarLocalidades(pac.ProvinciaObj.IdProvincia);
            ddlLocalidad.SelectedValue = pac.IdLocalidad.ToString();

            txtEmail.Text = pac.CorreoElectronico;
            txtTelefono.Text = pac.Telefono;
            lblMensaje.Text = string.Empty;
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            gvPacientes.PageIndex = 0;
            CargarGrilla();
        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            if (ddlFiltroProvincia.Items.Count > 0) ddlFiltroProvincia.SelectedIndex = 0;
            if (ddlFiltroSexo.Items.Count > 0) ddlFiltroSexo.SelectedIndex = 0;
            gvPacientes.PageIndex = 0;
            CargarGrilla();
        }

        protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPacientes.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        private char ConvertirSexoAChar(string valorDropdown)
        {
            switch (valorDropdown)
            {
                case "Masculino":
                case "M":
                    return 'M';
                case "Femenino":
                case "F":
                    return 'F';
                default:
                    return 'X';
            }
        }

        private string ConvertirSexoACharString(string valorDropdown)
        {
            if (string.IsNullOrEmpty(valorDropdown)) return string.Empty;
            return ConvertirSexoAChar(valorDropdown).ToString();
        }

        private string ConvertirCharASexoDropdown(char sexo)
        {
            switch (sexo)
            {
                case 'M': return "Masculino";
                case 'F': return "Femenino";
                default: return "Otro";
            }
        }
    }
}