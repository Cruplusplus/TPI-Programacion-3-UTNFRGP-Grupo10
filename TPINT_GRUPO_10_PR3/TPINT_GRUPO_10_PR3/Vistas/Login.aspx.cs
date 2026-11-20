using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPINT_GRUPO_10_PR3.Vistas
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string nombreUsuario =txtUsername.Value;
            string password =txtPassword.Value;
            Negocio.NegocioUser negocioUser = new Negocio.NegocioUser();
            bool esValido = negocioUser.ValidarUsuario(nombreUsuario, password);
            if (esValido)
            {
                Session["NombreUsuario"] = nombreUsuario;
                Response.Redirect("Site.Master");
            }
        }

        protected void lbtnRecovery_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Vistas/Recovery.aspx");
        }
    }
}