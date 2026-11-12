using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using TPINT_GRUPO_10_PR3.Entidades;

namespace TPINT_GRUPO_10_PR3
{
    public class PageBase : Page
    {
        public Usuario UsuarioLogueado => Session["UsuarioLogueado"] as Usuario;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (UsuarioLogueado == null)
            {
                Session["UsuarioLogueado"] = new Usuario
                {
                    IdUsuario = 1,
                    NombreUsuario = "Admin_Prototipo",
                    Rol = new Rol { IdRol = 1, Nombre = "Administrador" }
                };
            }
        }

    }
}