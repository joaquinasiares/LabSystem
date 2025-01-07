<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LabSystemPP3.Login" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="loginStyle.css" rel="stylesheet" type="text/css" />
    <title></title>

    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
</head>

<body>
    <asp:SqlDataSource ID="SqlBuscarUsuario" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectUsuarioPorNombreYContr">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="usuario" Type="String"></asp:Parameter>
            <asp:Parameter Name="contrasenia" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>


    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="login-container">
            <h2>TECNODIAGNOSTICA</h2>

            <label for="usuario">Usuario:</label>
            <asp:TextBox ID="usuario" runat="server" placeholder="Tu usuario"></asp:TextBox>

            <label for="password">Contraseña:</label>
            <asp:TextBox ID="password" TextMode="Password" placeholder="Tu contraseña" runat="server"></asp:TextBox>

            <div class="remember-container">
                <asp:CheckBox ID="recordar" runat="server" />

                <label for="recordar">Recordarme</label>
            </div>

            <div class="g-recaptcha" data-sitekey="6LeUk50qAAAAAGYMNOoX5lVQ82ci54KEvC50G1pV"></div>
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" EnableViewState="true" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnLogin" runat="server" class="btn-ingresar" Text="Iniciar Sesión" OnClick="btnLogin_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="footer">
                <p><a href="#">¿Olvidaste tu contraseña?</a></p>

            </div>
        </div>

    </form>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</body>
</html>