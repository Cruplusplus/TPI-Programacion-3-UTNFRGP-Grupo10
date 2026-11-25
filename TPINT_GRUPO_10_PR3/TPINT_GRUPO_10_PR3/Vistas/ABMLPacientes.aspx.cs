using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace TPINT_GRUPO_10_PR3
{
    public partial class ABMLPacientes : PageBase
    {
        [Serializable]
        public class PacienteVisual
        {
            public string DNI { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Sexo { get; set; }
            public string Nacionalidad { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string Direccion { get; set; }
            public string Provincia { get; set; }
            public string Localidad { get; set; }
            public string Email { get; set; }
            public string Telefono { get; set; }
        }

        private List<PacienteVisual> Pacientes
        {
            get
            {
                if (Session["PacientesVisual"] == null)
                {
                    Session["PacientesVisual"] = new List<PacienteVisual>();
                }
                return (List<PacienteVisual>)Session["PacientesVisual"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProvincias();
                CargarLocalidades("");
                CargarGrilla();
            }
        }

        private void CargarProvincias()
        {
            string[] provincias = { "Buenos Aires", "CABA", "Córdoba", "Santa Fe" };
            ddlProvincia.Items.Clear();
            ddlFiltroProvincia.Items.Clear();

            ddlProvincia.Items.Add(new ListItem("-- Seleccione --", ""));
            ddlFiltroProvincia.Items.Add(new ListItem("Todas", ""));

            foreach (var p in provincias)
            {
                ddlProvincia.Items.Add(new ListItem(p, p));
                ddlFiltroProvincia.Items.Add(new ListItem(p, p));
            }
        }

        private void CargarLocalidades(string provincia)
        {
            ddlLocalidad.Items.Clear();
            ddlLocalidad.Items.Add(new ListItem("-- Seleccione --", ""));

            if (provincia == "Buenos Aires")
            {
                ddlLocalidad.Items.Add("General Pacheco");
                ddlLocalidad.Items.Add("San Miguel");
                ddlLocalidad.Items.Add("Grand Bourg");
            }
            else if (provincia == "CABA")
            {
                ddlLocalidad.Items.Add("Almagro");
                ddlLocalidad.Items.Add("Belgrano");
            }
            else if (provincia == "Córdoba")
            {
                ddlLocalidad.Items.Add("Córdoba Capital");
                ddlLocalidad.Items.Add("Villa Carlos Paz");
            }
            else if (provincia == "Santa Fe")
            {
                ddlLocalidad.Items.Add("Rosario");
                ddlLocalidad.Items.Add("Santa Fe Capital");
            }
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarLocalidades(ddlProvincia.SelectedValue);
        }

        private void CargarGrilla()
        {
            IEnumerable<PacienteVisual> query = Pacientes;

            string filtroTexto = txtBuscar.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(filtroTexto))
            {
                query = query.Where(p => p.DNI.Contains(filtroTexto) ||
                                         p.Nombre.ToLower().Contains(filtroTexto) ||
                                         p.Apellido.ToLower().Contains(filtroTexto));
            }

            if (!string.IsNullOrEmpty(ddlFiltroProvincia.SelectedValue))
                query = query.Where(p => p.Provincia == ddlFiltroProvincia.SelectedValue);

            if (!string.IsNullOrEmpty(ddlFiltroSexo.SelectedValue))
                query = query.Where(p => p.Sexo == ddlFiltroSexo.SelectedValue);

            gvPacientes.DataSource = query.ToList();
            gvPacientes.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDni.Text) || string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                lblMensaje.Text = "Complete al menos DNI, Nombre y Apellido.";
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                return;
            }

            DateTime fechaNac;
            DateTime.TryParse(txtFechaNac.Text, out fechaNac);

            if (ViewState["EditandoDNI"] == null)
            {
                if (Pacientes.Any(p => p.DNI == txtDni.Text.Trim()))
                {
                    lblMensaje.Text = "Ya existe un paciente con ese DNI.";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                Pacientes.Insert(0, new PacienteVisual
                {
                    DNI = txtDni.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Sexo = ddlSexo.SelectedValue,
                    Nacionalidad = txtNacionalidad.Text.Trim(),
                    FechaNacimiento = fechaNac == DateTime.MinValue ? DateTime.Today : fechaNac,
                    Direccion = txtDireccion.Text.Trim(),
                    Provincia = ddlProvincia.SelectedValue,
                    Localidad = ddlLocalidad.SelectedValue,
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim()
                });
                lblMensaje.Text = "Paciente agregado correctamente.";
            }
            else
            {
                string dni = ViewState["EditandoDNI"].ToString();
                var pac = Pacientes.FirstOrDefault(p => p.DNI == dni);
                if (pac != null)
                {
                    pac.Nombre = txtNombre.Text.Trim();
                    pac.Apellido = txtApellido.Text.Trim();
                    pac.Sexo = ddlSexo.SelectedValue;
                    pac.Nacionalidad = txtNacionalidad.Text.Trim();
                    if (fechaNac != DateTime.MinValue) pac.FechaNacimiento = fechaNac;
                    pac.Direccion = txtDireccion.Text.Trim();
                    pac.Provincia = ddlProvincia.SelectedValue;
                    pac.Localidad = ddlLocalidad.SelectedValue;
                    pac.Email = txtEmail.Text.Trim();
                    pac.Telefono = txtTelefono.Text.Trim();
                }
                lblMensaje.Text = "Paciente modificado correctamente.";
            }

            lblMensaje.ForeColor = System.Drawing.Color.Green;
            LimpiarFormulario();
            CargarGrilla();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblMensaje.Text = "";
        }

        private void LimpiarFormulario()
        {
            ViewState["EditandoDNI"] = null;
            txtDni.Text = "";
            txtDni.ReadOnly = false;
            txtNombre.Text = "";
            txtApellido.Text = "";
            ddlSexo.SelectedIndex = 0;
            txtNacionalidad.Text = "";
            txtFechaNac.Text = "";
            txtDireccion.Text = "";
            ddlProvincia.SelectedIndex = 0;
            CargarLocalidades("");
            txtEmail.Text = "";
            txtTelefono.Text = "";
            lblTituloForm.Text = "Alta de Paciente";
            btnGuardar.Text = "Guardar";
        }

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string dni = e.CommandArgument.ToString();

            if (e.CommandName == "Modificar")
            {
                var pac = Pacientes.FirstOrDefault(p => p.DNI == dni);
                if (pac != null)
                {
                    ViewState["EditandoDNI"] = pac.DNI;
                    txtDni.Text = pac.DNI;
                    txtDni.ReadOnly = true;
                    txtNombre.Text = pac.Nombre;
                    txtApellido.Text = pac.Apellido;
                    ddlSexo.SelectedValue = pac.Sexo;
                    txtNacionalidad.Text = pac.Nacionalidad;
                    txtFechaNac.Text = pac.FechaNacimiento.ToString("yyyy-MM-dd");
                    txtDireccion.Text = pac.Direccion;
                    ddlProvincia.SelectedValue = pac.Provincia;
                    CargarLocalidades(pac.Provincia);
                    ddlLocalidad.SelectedValue = pac.Localidad;
                    txtEmail.Text = pac.Email;
                    txtTelefono.Text = pac.Telefono;

                    lblTituloForm.Text = "Modificar Paciente (DNI: " + pac.DNI + ")";
                    btnGuardar.Text = "Actualizar";
                    lblMensaje.Text = "";
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                var pac = Pacientes.FirstOrDefault(p => p.DNI == dni);
                if (pac != null)
                {
                    Pacientes.Remove(pac);
                    lblMensaje.Text = "Paciente eliminado.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    CargarGrilla();
                }
            }
        }

        protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPacientes.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            gvPacientes.PageIndex = 0;
            CargarGrilla();
        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            ddlFiltroProvincia.SelectedIndex = 0;
            ddlFiltroSexo.SelectedIndex = 0;
            gvPacientes.PageIndex = 0;
            CargarGrilla();
        }
    }
}