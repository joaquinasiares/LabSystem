<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LabSystemPP3.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="loginStyle.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
            <div class="login-container">
                <h2>TECNODIAGNOSTICA</h2>
                <form action="login_process.php" method="POST">
                    
                    <label for="usuario">Usuario:</label>
                    <input type="text" id="usuario" name="usuario" placeholder="Tu usuario" required>

                    <label for="password">Contraseña:</label>
                    <input type="password" id="password" name="password" placeholder="Tu contraseña" required>

                    <div class="remember-container">
                        <input type="checkbox" id="remember" name="remember">
                        <label for="remember">Recordarme</label>
                    </div>

                    <div class="form-group">
                        
                        <asp:HyperLink ID="HyperLink1" href="../Default.aspx" class="btn-ingresar" runat="server" NavigateUrl="">Ingresar</asp:HyperLink>


                        
                    </div>
                </form>

                <div class="footer">
                    <p><a href="#">¿Olvidaste tu contraseña?</a></p>
                    
                </div>
            </div>
            
    </form>
</body>
</html>
