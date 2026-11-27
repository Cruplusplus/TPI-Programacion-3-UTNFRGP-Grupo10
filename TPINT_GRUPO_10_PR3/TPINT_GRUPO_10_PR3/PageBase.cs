using System;
using System.Web.UI;
using TPINT_GRUPO_10_PR3.Negocio;

namespace TPINT_GRUPO_10_PR3
{
    public class PageBase : Page
    {
        public string NombreUsuarioLogueado
        {
            get { return Session["NombreUser"] as string; }
        }

        public int RolUsuarioLogueado
        {
            get
            {
                if (Session["IdRol"] == null && !string.IsNullOrEmpty(NombreUsuarioLogueado))
                {
                    NegocioUser negUser = new NegocioUser();
                    Session["IdRol"] = negUser.obtenerRolPorNombre(NombreUsuarioLogueado);
                }
                return Session["IdRol"] != null ? (int)Session["IdRol"] : -1;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            #if DEBUG
            if (Session["NombreUser"] == null)
            {
                Session["NombreUser"] = "AdminTest";
                Session["IdRol"] = 0;
            }
            #endif

            base.OnInit(e);

            // Si no paso por el Login, redirigimos al WebForm1 de Vistas
            if (string.IsNullOrEmpty(NombreUsuarioLogueado))
            {
                Response.Redirect("~/Vistas/Login.aspx", true);
            }
        }

        protected void ValidarRol(int idRolPermitido)
        {
            if (RolUsuarioLogueado != idRolPermitido)
            {
                Response.Redirect("~/Vistas/Login.aspx", true);
            }
        }
    }
}