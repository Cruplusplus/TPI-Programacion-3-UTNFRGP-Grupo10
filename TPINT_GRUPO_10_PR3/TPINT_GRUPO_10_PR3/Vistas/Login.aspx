<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPINT_GRUPO_10_PR3.Vistas.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login</title>
    <link href="~/Css/Login.css" rel="stylesheet" type="text/css" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-wrapper">
            <section class="login-card">
                <h2>Iniciar Sesión</h2>

                <div class="campo">
                    <label for="txtUsername">Usuario</label>
                    <input type="text" id="txtUsername" runat="server" class="input" />
                    <br />
                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" ControlToValidate="txtUsername" ErrorMessage="Debe completar el campo para poder iniciar sesion"></asp:RequiredFieldValidator>
                </div>

                <div class="campo">
                    <label for="txtPassword">Contraseña</label>
                    <input type="password" id="txtPassword" runat="server" class="input" />
                    <br />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Debe completar la contraseña para poder iniciar sesion"></asp:RequiredFieldValidator>
                    <br />
                    <asp:LinkButton ID="lbtnRecovery" runat="server" PostBackUrl="~/Vistas/Recovery.aspx" CssClass="link-volver" CausesValidation="false">¿Olvidaste tu contraseña?</asp:LinkButton>
                </div>

                <div>
                    <input type="submit" id="btnLogin" value="Iniciar Sesión" runat="server" class="btn-login" OnServerClick="btnLogin_Click" />
                </div>
            </section>
        </div>
    </form>
</body>
</html>