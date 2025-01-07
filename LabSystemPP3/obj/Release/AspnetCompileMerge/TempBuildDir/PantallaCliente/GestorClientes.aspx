<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestorClientes.aspx.cs" Inherits="LabSystemPP3.PantallaCliente.WebForm1" EnableEventValidation="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="VentanaModal.css" />
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
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/PantallaPedido/GestorPedido.aspx">Gestor de Pedido</asp:HyperLink>
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


                <div class="search-bar">
                    <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
                    <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    <asp:Button Class="add-button" ID="Button3" runat="server" Text="Agregar" OnClientClick="mostrarModal(); return false;" />
                </div>

                <div class="grid-container">
                    <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="ID" ReadOnly="True" SortExpression="ID"></asp:BoundField>
                            <asp:BoundField DataField="idIva" HeaderText=""></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="Cuit" HeaderText="Cuit"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="Razon social" HeaderText="Razon social"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="IVA" HeaderText="IVA"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="Ambito" HeaderText="Ambito"></asp:BoundField>

                            <asp:ButtonField CommandName="Select" Text="seleccionar"></asp:ButtonField>
                            <asp:ButtonField CommandName="Borrar" Text="Eliminar"></asp:ButtonField>

                            <asp:HyperLinkField
                                DataNavigateUrlFields="ID,idIva,Cuit,Nombre,Razon social,IVA,Ambito"
                                DataNavigateUrlFormatString="Update.aspx?ID={0}&idIva={1}&Cuit={2}&Nombre={3}&Razon social={4}&IVA={5}&Ambito={6}"
                                NavigateUrl="Modifiacar.aspx" Text="IR" HeaderText="Modifi"></asp:HyperLinkField>

                            <asp:HyperLinkField
                                DataNavigateUrlFields="ID,Nombre,Ambito"
                                DataNavigateUrlFormatString="~/PantallaPedido/GestorPedidoVenta.aspx?ID={0}&Nombre={1}&Ambito={2}"
                                NavigateUrl="~/PantallaPedido/GestorPedidoVenta.aspx" Text="Vender" HeaderText="Venta"></asp:HyperLinkField>
                        </Columns>

                        <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                    </asp:GridView>
                </div>

            </div>
        </div>


        <!--   ----------------------------------------  -->


        <div class="modal-container" id="modal-container">
            <div class="modal-body" id="VentanaUPD">
                <div class="modal-header">
                    <h3>Agregar Usuario</h3>
                </div>

                <div class="modal-content">
                    <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Condición IVA: "></asp:Label>
                    <asp:DropDownList CssClass="modalitem Inp" ID="listaIva" runat="server"></asp:DropDownList>

                    <asp:Label CssClass="modalitem" ID="Label3" runat="server" Text="CUIT: "></asp:Label>
                    <asp:TextBox CssClass="modalitem Inp" ID="txtCuit" runat="server"></asp:TextBox>

                    <asp:Label CssClass="modalitem" ID="Label4" runat="server" Text="Razón Social: "></asp:Label>
                    <asp:TextBox CssClass="modalitem Inp" ID="txtRazonSocial" runat="server"></asp:TextBox>

                    <asp:Label CssClass="modalitem" ID="Label5" runat="server" Text="Nombre: "></asp:Label>
                    <asp:TextBox CssClass="modalitem Inp" ID="txtNombre" runat="server"></asp:TextBox>

                    <asp:Label CssClass="modalitem" ID="Label6" runat="server" Text="Ámbito: "></asp:Label>
                    <asp:DropDownList CssClass="modalitem Inp" ID="listaAmbito" runat="server"></asp:DropDownList>

                    <asp:Button CssClass="modalitem Inp btn" ID="Button2" runat="server" Text="Guardar" OnClick="Button2_Click" />

                    <asp:Label ID="lblCrear" runat="server" Text=""></asp:Label>
                </div>

                <asp:Label CssClass="upd-cerrar" ID="lblCerrar" runat="server" Text="&times;"></asp:Label>
            </div>
        </div>


    </form>
    <script src="ClienteJavSc.js"></script>
</body>
</html>
