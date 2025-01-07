<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestorPedido.aspx.cs" Inherits="LabSystemPP3.PantallaPedido.GestorPedido1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="pantallaPedido.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

            <div class="container">

                <div class="sidebar">
                    <h2 class="titulo"><span style="font-weight: bold;">TECNO</span>DIAGNOSTICA</h2>
                    <div class="menu">
                        <a href="/Default">Inicio</a>
                        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/PantallaCliente/GestorClientes.aspx">Gestor de Clientes</asp:HyperLink>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/PantallaProveedores/GestorProveedores.aspx">Gestor de Proveedores</asp:HyperLink>
                        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/PantallaProductos/GestorProductos.aspx">Gestor de Productos</asp:HyperLink>
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/PantallaPedido/ListadoPedidos.aspx">Gestor de Pedido</asp:HyperLink>
                        <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/PantallaVenta/ListaVentasCliente.aspx">Ventas</asp:HyperLink>
                        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Pantalla Informes/GestorInformes.aspx">Ver Informes</asp:HyperLink>
                        <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/PantallaEmpleados/GestorEmpleados.aspx">Ver Empleados</asp:HyperLink>
                        <asp:HyperLink ID="HyperLink4" runat="server">Modificación</asp:HyperLink>
                        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/PantallaCliente/Listado.aspx">Listar</asp:HyperLink>

                        <div class="logout-container">
                            <asp:HyperLink ID="HyperLinkLogout" href="Logout.aspx" class="btn-logout" runat="server" NavigateUrl="">Cerrar sesión</asp:HyperLink>
                        </div>

                    </div>
                </div>

                            <div class="contenido-central">
                                <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
                                <div>
                                    <h1>Seleccionar pedido</h1>
        
                                    <h3>Tipo de pedido</h3>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="DDLPedidos" runat="server" CssClass="drop-down-list"></asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <br />

                                    <h3>Cliente</h3>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <!-- Barra de búsqueda -->
                                            <div class="search-bar">
                                                <asp:TextBox CssClass="input-search" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
                                                <asp:Button CssClass="btn-search" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                                            </div>

                                            <!-- Grilla de datos -->
                                            <div class="grid-container">
                                                <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" ReadOnly="True" SortExpression="ID"></asp:BoundField>
                                                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Cuit" HeaderText="Cuit"></asp:BoundField>
                                                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Razon social" HeaderText="Razon social"></asp:BoundField>
                                                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="IVA" HeaderText="IVA"></asp:BoundField>
                                                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Ambito" HeaderText="Ambito"></asp:BoundField>
                                                        <asp:ButtonField CommandName="Select" Text="Seleccionar"></asp:ButtonField>
                                                    </Columns>

                                                    <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <br />

                                    <asp:UpdatePanel ID="UPPCli" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblId" runat="server" Visible="False" Text=""></asp:Label>
                                            <asp:Label ID="lblNom" runat="server" Visible="False" Text=""></asp:Label>
                                            <asp:Label ID="lblAmb" runat="server" Visible="False" Text=""></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <!-- Botón de acción -->
                                    <asp:Button ID="Button1" runat="server" Text="Realizar el pedido" CssClass="btn-action" OnClick="Button1_Click" />
                                </div>
                            </div>

           </div>
    </form>
    <script src="Pedidos.js"></script>
</body>
</html>
