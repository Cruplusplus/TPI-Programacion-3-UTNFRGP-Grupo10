using System;
using System.Collections.Generic;
using System.Data;
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
            string nombreUsuario = txtUsername.Value;
            string password = txtPassword.Value;
            Negocio.NegocioUser negocioUser = new Negocio.NegocioUser();

            if (negocioUser.ValidarUsuario(nombreUsuario, password))
            {
                Session["NombreUser"] = nombreUsuario;

                int rol = negocioUser.obtenerRolPorNombre(nombreUsuario);

                if (rol == 0)
                    Response.Redirect("~/Vistas/ABMLMedicos.aspx");   // admin
                else if (rol == 1)
                    Response.Redirect("~/ABMLPacientes.aspx");   // médico
                else if (rol == 2)
                    Response.Redirect("~/ABMLPacientes.aspx"); // paciente
                else
                    Response.Write("Rol no reconocido: " + rol);
            }
            else
            {
                Response.Write("Usuario o contraseña incorrectos");
            }
        }
        protected void lbtnRecovery_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Vistas/Recovery.aspx");
        }
    }
}