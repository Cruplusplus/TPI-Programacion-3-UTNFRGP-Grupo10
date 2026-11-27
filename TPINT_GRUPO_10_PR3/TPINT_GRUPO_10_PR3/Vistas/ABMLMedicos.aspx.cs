using System;
using System.Web.UI.WebControls;
using TPINT_GRUPO_10_PR3.Entidades;
using TPINT_GRUPO_10_PR3.Negocio;

namespace TPINT_GRUPO_10_PR3
{
    public partial class Medicos : PageBase
    {
        private readonly NegocioMedico medNeg = new NegocioMedico();

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidarRol(0);

            if (!IsPostBack)
            {
                CargarProvincias();
                CargarEspecialidades();
                CargarGrilla();
            }
        }

        private void CargarProvincias()
        {
            ddlProvincia.Items.Clear();
            ddlProvincia.DataSource = medNeg.ListarProvincias();
            ddlProvincia.DataTextField = "NombreProvincia";
            ddlProvincia.DataValueField = "IdProvincia";
            ddlProvincia.DataBind();
            ddlProvincia.Items.Insert(0, new ListItem("-- Seleccione --", ""));

            ddlLocalidad.Items.Clear();
            ddlLocalidad.Items.Insert(0, new ListItem("-- Seleccione provincia --", ""));
        }

        private void CargarLocalidades(int idProvincia)
        {
            ddlLocalidad.Items.Clear();
            ddlLocalidad.DataSource = medNeg.ListarLocalidades(idProvincia);
            ddlLocalidad.DataTextField = "NombreLocalidad";
            ddlLocalidad.DataValueField = "IdLocalidad";
            ddlLocalidad.DataBind();
            ddlLocalidad.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        private void CargarEspecialidades()
        {
            var especialidades = medNeg.ListarEspecialidades();

            ddlEspecialidades.Items.Clear();
            ddlEspecialidades.DataSource = especialidades;
            ddlEspecialidades.DataTextField = "NombreEspecialidad";
            ddlEspecialidades.DataValueField = "IdEspecialidad";
            ddlEspecialidades.DataBind();
            ddlEspecialidades.Items.Insert(0, new ListItem("-- Seleccione --", ""));

            ddlFiltroEspecialidad.Items.Clear();
            ddlFiltroEspecialidad.DataSource = especialidades;
            ddlFiltroEspecialidad.DataTextField = "NombreEspecialidad";
            ddlFiltroEspecialidad.DataValueField = "IdEspecialidad";
            ddlFiltroEspecialidad.DataBind();
            ddlFiltroEspecialidad.Items.Insert(0, new ListItem("-- Todas las especialidades --", "0"));
        }

        private void CargarGrilla()
        {
            string filtro = txtFiltro.Text.Trim();
            int idEspecialidad = !string.IsNullOrEmpty(ddlFiltroEspecialidad.SelectedValue)
                ? int.Parse(ddlFiltroEspecialidad.SelectedValue)
                : 0;
            int dia = !string.IsNullOrEmpty(ddlFiltroDia.SelectedValue)
                ? int.Parse(ddlFiltroDia.SelectedValue)
                : -1;

            GridViewMedicos.DataSource = medNeg.Listar(filtro, idEspecialidad, dia);
            GridViewMedicos.DataBind();
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Medico med = ArmarObjetoDesdeFormulario();

                if (ViewState["IdMedicoEditar"] == null)
                {
                    medNeg.Agregar(med);
                    lblMensaje.Text = "Medico registrado correctamente.";
                }
                else
                {
                    med.IdPersona = (int)ViewState["IdMedicoEditar"];
                    medNeg.Modificar(med);
                    lblMensaje.Text = "Medico modificado correctamente.";
                }

                lblMensaje.ForeColor = System.Drawing.Color.Green;
                LimpiarControles();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        private Medico ArmarObjetoDesdeFormulario()
        {
            Medico med = new Medico();

            // Datos de Usuario
            med.Usuario.NombreUsuario = txtUsuario.Text.Trim();
            med.Usuario.Password = txtContrasenia.Text;
            med.Usuario.Rol.IdRol = 1; // 1 = Medico
            med.Usuario.Estado = true;

            // Datos de Persona (heredados en Medico)
            med.Dni = txtDni.Text.Trim();
            med.Nombre = txtNombre.Text.Trim();
            med.Apellido = txtApellido.Text.Trim();
            med.Sexo = Convert.ToChar(ddlSexo.SelectedValue); // "M", "F" o "X"
            med.Nacionalidad = txtNacionalidad.Text.Trim();
            med.FechaNacimiento = DateTime.Parse(txtFechaNac.Text);
            med.Direccion = txtDireccion.Text.Trim();
            med.IdLocalidad = int.Parse(ddlLocalidad.SelectedValue);
            med.CorreoElectronico = txtCorreo.Text.Trim();
            med.Telefono = txtTelefono.Text.Trim();
            med.Estado = true;

            // Datos propios de Medico
            med.Legajo = txtLegajo.Text.Trim();
            med.Especialidad.IdEspecialidad = int.Parse(ddlEspecialidades.SelectedValue);
            med.EstadoMedico = true;

            // Lista de Horarios
            TimeSpan horaInicio = TimeSpan.Parse(ddlHoraInicio.SelectedValue);
            TimeSpan horaFin = TimeSpan.Parse(ddlHoraFin.SelectedValue);

            foreach (ListItem item in cblDiasAtencion.Items)
            {
                if (item.Selected)
                {
                    HorarioMedico horario = new HorarioMedico();
                    horario.Dia = (DayOfWeek)int.Parse(item.Value);
                    horario.HoraInicio = horaInicio;
                    horario.HoraFin = horaFin;
                    horario.EstadoHorario = true;

                    med.Horarios.Add(horario);
                }
            }

            return med;
        }

        protected void GridViewMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EventoEditar" && e.CommandName != "EventoEliminar")
                return;

            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idMedico = Convert.ToInt32(GridViewMedicos.DataKeys[index].Value);

                if (e.CommandName == "EventoEliminar")
                {
                    medNeg.Eliminar(idMedico);
                    lblMensaje.Text = "Medico dado de baja correctamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    LimpiarControles();
                    CargarGrilla();
                }
                else if (e.CommandName == "EventoEditar")
                {
                    CargarMedicoParaEdicion(idMedico);
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void CargarMedicoParaEdicion(int idMedico)
        {
            Medico med = medNeg.ObtenerPorId(idMedico);
            if (med == null) return;

            ViewState["IdMedicoEditar"] = med.IdPersona;
            lblTituloForm.Text = "Editando Medico - Legajo " + med.Legajo;
            btnGuardar.Text = "Guardar Cambios";
            btnGuardar.OnClientClick = "if (Page_ClientValidate('GrupoMedico')) { return confirm('Esta seguro que desea guardar los cambios realizados?'); } return false;";

            // Bloqueamos campos unicos no editables
            txtLegajo.Text = med.Legajo;
            txtLegajo.Enabled = false;
            txtDni.Text = med.Dni;
            txtDni.Enabled = false;
            txtUsuario.Text = med.Usuario.NombreUsuario;
            txtUsuario.Enabled = false;

            // En edicion la clave nueva es opcional
            rfvPass.Enabled = false;

            txtNombre.Text = med.Nombre;
            txtApellido.Text = med.Apellido;
            ddlSexo.SelectedValue = med.Sexo.ToString();
            txtNacionalidad.Text = med.Nacionalidad;
            txtFechaNac.Text = med.FechaNacimiento.ToString("yyyy-MM-dd");
            txtDireccion.Text = med.Direccion;

            int idProvincia = medNeg.ObtenerProvinciaPorLocalidad(med.IdLocalidad);
            ddlProvincia.SelectedValue = idProvincia.ToString();
            CargarLocalidades(idProvincia);
            ddlLocalidad.SelectedValue = med.IdLocalidad.ToString();

            txtCorreo.Text = med.CorreoElectronico;
            txtTelefono.Text = med.Telefono;
            ddlEspecialidades.SelectedValue = med.Especialidad.IdEspecialidad.ToString();

            // Marcamos dias y horas actuales
            foreach (ListItem item in cblDiasAtencion.Items)
            {
                item.Selected = false;
            }

            if (med.Horarios.Count > 0)
            {
                ddlHoraInicio.SelectedValue = med.Horarios[0].HoraInicio.ToString(@"hh\:mm");
                ddlHoraFin.SelectedValue = med.Horarios[0].HoraFin.ToString(@"hh\:mm");

                foreach (HorarioMedico h in med.Horarios)
                {
                    string diaVal = ((int)h.Dia).ToString();
                    ListItem item = cblDiasAtencion.Items.FindByValue(diaVal);
                    if (item != null) item.Selected = true;
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            lblMensaje.Text = string.Empty;
        }

        private void LimpiarControles()
        {
            ViewState["IdMedicoEditar"] = null;
            lblTituloForm.Text = "Datos del Medico";
            btnGuardar.Text = "Guardar Medico";

            txtLegajo.Text = string.Empty;
            txtLegajo.Enabled = true;
            txtDni.Text = string.Empty;
            txtDni.Enabled = true;
            txtUsuario.Text = string.Empty;
            txtUsuario.Enabled = true;
            rfvPass.Enabled = true;

            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            if (ddlSexo.Items.Count > 0) ddlSexo.SelectedIndex = 0;
            txtNacionalidad.Text = string.Empty;
            txtFechaNac.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            if (ddlProvincia.Items.Count > 0) ddlProvincia.SelectedIndex = 0;
            ddlLocalidad.Items.Clear();
            ddlLocalidad.Items.Insert(0, new ListItem("-- Seleccione provincia --", ""));
            txtCorreo.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            if (ddlEspecialidades.Items.Count > 0) ddlEspecialidades.SelectedIndex = 0;

            foreach (ListItem item in cblDiasAtencion.Items)
            {
                item.Selected = false;
            }

            if (ddlHoraInicio.Items.Count > 0) ddlHoraInicio.SelectedIndex = 0;
            if (ddlHoraFin.Items.Count > 0) ddlHoraFin.SelectedIndex = 0;

            txtContrasenia.Text = string.Empty;
            txtRepetirContrasenia.Text = string.Empty;
            btnGuardar.OnClientClick = string.Empty;
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            GridViewMedicos.PageIndex = 0;
            CargarGrilla();
        }

        protected void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = string.Empty;
            if (ddlFiltroEspecialidad.Items.Count > 0) ddlFiltroEspecialidad.SelectedIndex = 0;
            if (ddlFiltroDia.Items.Count > 0) ddlFiltroDia.SelectedIndex = 0;
            GridViewMedicos.PageIndex = 0;
            CargarGrilla();
        }

        protected void GridViewMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMedicos.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        protected void GridViewMedicos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnEliminar = e.Row.Cells[10].Controls[0] as Button;
                if (btnEliminar != null)
                {
                    btnEliminar.OnClientClick = "return confirm('Esta seguro que desea dar de baja a este medico?');";
                }
            }
        }
    }
}