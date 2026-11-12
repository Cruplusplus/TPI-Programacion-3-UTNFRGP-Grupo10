<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPINT_GRUPO_10_PR3.Vistas.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login</title>
    <link href="~/Css/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-wrapper">
            <section class="login-card">
                <h2>Iniciar Sesión</h2>

                <div class="campo">
                    <label for="txtUsuario">Usuario</label>
                    <input type="text" id="txtUsuario" runat="server" class="input" />
                </div>

                <div class="campo">
                    <label for="txtPassword">Contraseña</label>
                    <input type="password" id="txtPassword" runat="server" class="input" />
                    <br />
                      <asp:LinkButton ID="lbtnRecovery" runat="server" PostBackUrl="~/Vistas/Recovery.aspx" CssClass="link-volver" CausesValidation="false">¿Olvidaste tu contraseña?</asp:LinkButton>
                </div>

                <div>
                    <input type="submit" value="Iniciar Sesión" runat="server" class="btn-login" />
                </div>
            </section>
        </div>
    </form>
</body>
</html>