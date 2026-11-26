using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPINT_GRUPO_10_PR3.Entidades;

namespace TPINT_GRUPO_10_PR3
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["NombreUser"] != null)
            {
                string nombreUsuario = Session["NombreUser"].ToString();
                lblUsuarioLogueado.Text = nombreUsuario;
            }
            else
            {
                Response.Redirect("~/Vistas/Login.aspx");
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Vistas/Login.aspx");
        }
    }
}