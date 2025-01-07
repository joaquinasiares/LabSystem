<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestorProveedores.aspx.cs" Inherits="LabSystemPP3.PantallaProveedores.GestorProveedores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="ProveedorEstilos.css"/>
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
                                <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Proveedor"></asp:TextBox>
                                <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                                <asp:Button CssClass="add-button" ID="Button3" runat="server" Text="Agregar" OnClientClick="mostrarModal(); return false;" />
                            </div>

                            <div class="grid-container">
                                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                                <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="ID" ReadOnly="True" SortExpression="ID"></asp:BoundField>
                                        <asp:BoundField DataField="Descripcion" HeaderText=""></asp:BoundField>
                                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Cuit" HeaderText="Cuit"></asp:BoundField>
                                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Nombre" HeaderText="Nombre"></asp:BoundField>

                                        <asp:ButtonField CommandName="Select" Text="seleccionar"></asp:ButtonField>
                                        <asp:ButtonField CommandName="Borrar" Text="Eliminar"></asp:ButtonField>

                                        <asp:HyperLinkField
                                            DataNavigateUrlFields="ID,Cuit,Nombre,Descripcion"
                                            DataNavigateUrlFormatString="Modificar.aspx?ID={0}&Cuit={1}&Nombre={2}&Descripcion={3}"
                                            NavigateUrl="Modifiacar.aspx" Text="Editar" HeaderText="Modificar">
                                        </asp:HyperLinkField>

                                        <asp:HyperLinkField 
                                            DataNavigateUrlFields="ID,Cuit,Nombre,Descripcion"
                                            DataNavigateUrlFormatString="DescripcionProveedor.aspx?ID={0}&Cuit={1}&Nombre={2}&Descripcion={3}"
                                            NavigateUrl="DescripcionProveedor.aspx" Text="Ver" HeaderText="Descripcion">
                                        </asp:HyperLinkField>
                                    </Columns>
                                     <SelectedRowStyle BackColor= "#8ed3e8"></SelectedRowStyle>
                                </asp:GridView>
                            </div>
                    </div>
         </div>

        <div class="modal-container" id="modal-container">
                    <div class="modal-body" id="VentanaUPD">
                        <div class="modal-header">
                            <h3>Agregar Proveedor</h3>
                        </div>

                        <div class="modal-content">
                            <asp:Label CssClass="modalitem" ID="Label3" runat="server" Text="CUIT: "></asp:Label>
                            <asp:TextBox CssClass="modalitem Inp" ID="txtCuit" runat="server"></asp:TextBox>

                            <asp:Label CssClass="modalitem" ID="Label5" runat="server" Text="Nombre: "></asp:Label>
                            <asp:TextBox CssClass="modalitem Inp" ID="txtNombre" runat="server"></asp:TextBox>

                            <asp:Label CssClass="modalitem" ID="Label4" runat="server" Text="Descripción: "></asp:Label>
                            <asp:TextBox CssClass="modalitem Inp" ID="txtDesc" runat="server" TextMode="MultiLine"></asp:TextBox>

                            <asp:Button CssClass="modalitem Inp btn" ID="Button2" runat="server" Text="Guardar" OnClick="Button2_Click" />

                            <asp:Label ID="lblCrear" runat="server" Text=""></asp:Label>
                        </div>

                        <asp:Label CssClass="upd-cerrar" ID="lblCerrar" runat="server" Text="&times;"></asp:Label>
                    </div>
        </div>

          <div class="modal-container" id="modal-containerDesc">

            <div class="modal-body" id="VentanaDesc">
                <div class="modal-header">
                    <h3>Descripcion del proveedor</h3>
                </div>

                <div class="modal-content">
                    <asp:Label CssClass="modalitem" ID="lblDescNom" runat="server" Text="Cuit: "></asp:Label>
                    <asp:Label CssClass="modalitem" ID="lblDescCuit" runat="server" Text="Nombre: "></asp:Label>
                    <asp:Label CssClass="modalitem" ID="lblDescMostrar" runat="server" Text="Descrpcion: "></asp:Label>
                    <asp:Button CssClass="modalitem Inp btn" ID="Button1" runat="server" Text="Button" OnClick="Button2_Click" />
                    <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                </div>
                <asp:Label CssClass="upd-cerrar" ID="lblDescCerrar" runat="server" Text="x"></asp:Label>

            </div>

        </div>


    </form>
    <script src="ProveedorJavaSc.js"></script>
</body>
</html>
