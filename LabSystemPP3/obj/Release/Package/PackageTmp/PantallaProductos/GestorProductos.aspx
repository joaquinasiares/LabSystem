<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestorProductos.aspx.cs" Inherits="LabSystemPP3.PantallaProductos.GestorProductos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="ProductoEstilos.css" />
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

                    <asp:TextBox ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Descripcion de Producto"></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    <asp:Button ID="Button3" runat="server" Text="Agregar" OnClientClick="mostrarModal(); return false;" />
                </div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="grid-container">

                            
                            <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="I" ReadOnly="True" SortExpression="I"></asp:BoundField>
                                    <asp:BoundField DataField="IDStock" ReadOnly="True" SortExpression="IDStock"></asp:BoundField>
                                    <asp:BoundField DataField="productoID" ReadOnly="True" SortExpression="ID"></asp:BoundField>
                                    <asp:BoundField DataField="IDProv" ReadOnly="True" SortExpression="IDProv"></asp:BoundField>
                                    <asp:BoundField DataField="IDTipo" ReadOnly="True" SortExpression="IDTipo"></asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" ReadOnly="True" SortExpression="Nombre"></asp:BoundField>
                                    <asp:BoundField DataField="Codigo" HeaderText="Codigo" ReadOnly="True" SortExpression="Codigo"></asp:BoundField>
                                    <asp:BoundField DataField="Lote" HeaderText="Lote" ReadOnly="True" SortExpression="Lote"></asp:BoundField>
                                    <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" ReadOnly="True" SortExpression="Proveedor"></asp:BoundField>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="True" SortExpression="Tipo"></asp:BoundField>
                                    <asp:BoundField DataField="Fecha de Ingreso" HeaderText="Fecha de Ingreso" ReadOnly="True" SortExpression="Fecha de Ingreso"></asp:BoundField>
                                    <asp:BoundField DataField="Fecha de Vencimiento" HeaderText="Fecha de Vencimiento" ReadOnly="True" SortExpression="Fecha de Vencimiento"></asp:BoundField>
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="True" SortExpression="Estado"></asp:BoundField>
                                    <asp:BoundField DataField="Precio de compra" HeaderText="Precio de compra" ReadOnly="True" SortExpression="Precio de compra"></asp:BoundField>
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ReadOnly="True" SortExpression="Cantidad"></asp:BoundField>
                                    <asp:BoundField DataField="PreVenPub" HeaderText="Precio Publico" ReadOnly="True" SortExpression="PreVenPub"></asp:BoundField>
                                    <asp:BoundField DataField="PreVenPriv" HeaderText="Precio Privado" ReadOnly="True" SortExpression="PreVenPriv"></asp:BoundField>
                                    <asp:ButtonField CommandName="Seleccionar" Text="seleccionar"></asp:ButtonField>
                                    <asp:ButtonField CommandName="Borrar" Text="Eliminar"></asp:ButtonField>
                                    <asp:HyperLinkField
                                        DataNavigateUrlFields="I,IDStock,productoID,IDProv,IDTipo,Nombre,Codigo,Lote,Proveedor,Tipo,Fecha de Ingreso,Fecha de Vencimiento,Estado,Precio de compra,Cantidad,PreVenPub,PreVenPriv"
                                        DataNavigateUrlFormatString="ModificarProducto.aspx?
                                            I={0}&IDStock={1}&productoID={2}&IDProv={3}&IDTipo={4}&Nombre={5}&Codigo={6}&Lote={7}&Proveedor={8}&Tipo={9}&Fecha de Ingreso={10}&Fecha de Vencimiento={11}&Estado={12}&Precio de compra={13}&Cantidad={14}&PreVenPub={15}&PreVenPriv={16}"
                                        NavigateUrl="ModificarProducto.aspx" Text="IR" HeaderText="Modifi"></asp:HyperLinkField>
                                    <asp:ButtonField CommandName="Select" Text="Bot&#243;n"></asp:ButtonField>
                                </Columns>
                                <SelectedRowStyle BackColor= "#8ed3e8"></SelectedRowStyle>
                            </asp:GridView>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="modal-container" id="modal-container">
            <div class="modal-body" id="VentanaInsert">
                <div class="modal-header">
                    <h3>Agregar producto</h3>
                </div>

                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label CssClass="modalitem" ID="Label1" runat="server" Text="Proveedor: "></asp:Label>
                            <asp:DropDownList ID="DDLProv" runat="server"></asp:DropDownList>
                            <br />
                            <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Tipo: "></asp:Label>
                            <asp:DropDownList ID="DDLTipo" runat="server"></asp:DropDownList>
                            <br />
                            <asp:Label ID="Label6" runat="server" Text="Codigo"></asp:Label>
                            <asp:TextBox ID="TbCod" runat="server"></asp:TextBox>
                            <br />
                            <asp:Label CssClass="modalitem" ID="lblDescNom" runat="server" Text="Nombre: "></asp:Label>
                            <asp:TextBox ID="TbNom" runat="server"></asp:TextBox>
                            <br />
                            <asp:Label ID="LblVenc" runat="server" Text="Fecha de vencimiento"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Calendar ID="Calendar1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label8" runat="server" Text="Precio de compra"></asp:Label>
                            <asp:TextBox ID="TbPreciCom" runat="server"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label9" runat="server" Text="Lote"></asp:Label>
                            <asp:TextBox ID="TbLote" runat="server"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label3" runat="server" Text="Cantidad"></asp:Label>
                            <asp:TextBox ID="TbCantidad" runat="server"></asp:TextBox>
                            <asp:Label ID="Label4" runat="server" Text="Precio de venta"></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text="Publico"></asp:Label>
                            <asp:TextBox ID="tbPreVentPub" runat="server"></asp:TextBox>
                            <asp:Label ID="Label7" runat="server" Text="Privado"></asp:Label>
                            <asp:TextBox ID="tbPreVentPriv" runat="server"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <asp:Label CssClass="insert-cerrar" ID="lblCerrar" runat="server" Text="x"></asp:Label>
                <asp:Button CssClass="modalitem Inp btn" ID="BtnInsert" runat="server" Text="Agregar" OnClick="Button1_Click" />
            </div>
        </div>




    </form>

    <script src="ProductoJS.js"></script>
</body>
</html>
