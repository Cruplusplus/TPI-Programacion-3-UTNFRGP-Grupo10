using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPINT_GRUPO_10_PR3.Vistas
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Trim() == "" || txtNueva.Text == "" || txtRepetir.Text == "")
            {
                lblMensaje.Text = "Completá todos los campos.";
                return;
            }

            if (txtNueva.Text != txtRepetir.Text)
            {
                lblMensaje.Text = "Las contraseñas no coinciden.";
                return;
            }

            // Acá llamás a Negocio para actualizar la contraseña
            lblMensaje.Text = "";
        }
    }
}