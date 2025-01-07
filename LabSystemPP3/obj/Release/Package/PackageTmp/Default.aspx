<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LabSystemPP3._Default" %>

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Página Principal</title>

    <link href="style.css" rel="stylesheet" type="text/css" />
</head>

    <body>
     <div class="container">
        <div class="sidebar">
            <h2 class="titulo"><span style="font-weight: bold;">TECNO</span>DIAGNOSTICA</h2>
            <div class="menu">
                <a href="/Default">Inicio</a>
                <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/PantallaCliente/GestorClientes.aspx">Gestor de Clientes</asp:HyperLink>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/PantallaProveedores/GestorProveedores.aspx">Gestor de Proveedores</asp:HyperLink>
                <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/PantallaProductos/GestorProductos.aspx">Gestor de Productos</asp:HyperLink>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/PantallaPedido/ListadoPedidos.aspx">Gestor de Pedido</asp:HyperLink>
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="#">Ver Informes</asp:HyperLink>
                <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/PantallaEmpleados/GestorEmpleados.aspx">Ver Empleados</asp:HyperLink>
                <asp:HyperLink ID="HyperLink4" runat="server">Modificación</asp:HyperLink>
                <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/PantallaCliente/Listado.aspx">Listar</asp:HyperLink>

                <div class="logout-container">
                    <asp:HyperLink ID="HyperLinkLogout" href="Logout.aspx" class="btn-logout" runat="server" NavigateUrl="">Cerrar sesión</asp:HyperLink>
                </div>

            </div>
        </div>

        <div class="contenido-central">
            <img src="images/labsystem.png" alt="Alternate Text" />
        </div>
    </div>



    <!-- Incluir Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
