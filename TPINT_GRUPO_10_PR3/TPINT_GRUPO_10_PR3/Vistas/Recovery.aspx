<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recovery.aspx.cs" Inherits="TPINT_GRUPO_10_PR3.Vistas.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Recuperar contraseña</title>
    <link href="~/Css/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-wrapper">
            <section class="login-card">
                <h2>Recuperar contraseña</h2>

                <div class="campo">
                    <asp:Label ID="lblUsuario" runat="server" Text="Usuario" AssociatedControlID="txtUsuario" />
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="input" />
                </div>

                <div class="campo">
                    <asp:Label ID="lblNueva" runat="server" Text="Nueva contraseña" AssociatedControlID="txtNueva" />
                    <asp:TextBox ID="txtNueva" runat="server" TextMode="Password" CssClass="input" />
                </div>

                <div class="campo">
                    <asp:Label ID="lblRepetir" runat="server" Text="Repita la contraseña" AssociatedControlID="txtRepetir" />
                    <asp:TextBox ID="txtRepetir" runat="server" TextMode="Password" CssClass="input" />
                </div>
                <asp:Button ID="btnRecuperar" runat="server" Text="Cambiar contraseña" CssClass="btn-login" OnClick="btnRecuperar_Click" />
                <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje" />

                <asp:HyperLink ID="hlVolver" runat="server" NavigateUrl="~/Vistas/Login.aspx" CssClass="link-volver">Volver al login</asp:HyperLink>
            </section>
        </div>
    </form>
</body>
</html>