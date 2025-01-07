<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestorPedidoVenta.aspx.cs" Inherits="LabSystemPP3.PantallaPedido.GestorPedido" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">Volver</asp:HyperLink>
            <asp:SqlDataSource ID="SqldsProductosYPedido" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="selectProductosALL" InsertCommandType="StoredProcedure" InsertCommand="InsertPedido" 
                OnInserted="SqldsProductosYPedido_Inserted" OnInserting="SqldsProductosYPedido_Inserting">
                <InsertParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="ID_CLIENTE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="ID_ESTADO_PEDIDO" Type="Int32"></asp:Parameter>
                    <asp:Parameter DbType="Date" Name="FECHA_PEDIDO"></asp:Parameter>
                    <asp:Parameter Name="DESCRIPCION" Type="String"></asp:Parameter>
                    <asp:Parameter Name="total" Type="Decimal"></asp:Parameter>
                    <asp:Parameter Name="Viatico" Type="Decimal"></asp:Parameter>
                    <asp:Parameter Name="ID_PEDIDO" Direction="Output" Type="Int32"></asp:Parameter>
                    </InsertParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDetallePedido" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                InsertCommandType="StoredProcedure" InsertCommand="insertDetallePedido" UpdateCommandType="StoredProcedure" UpdateCommand="RestarStock">
                <InsertParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="ID_PEDIDO" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="CANTIDAD" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="subTotal" Type="Decimal"></asp:Parameter>
                    <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
                    <asp:Parameter Name="precio" Type="Decimal"></asp:Parameter>
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="cantPed" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idStock" Type="Int32"></asp:Parameter>
                </UpdateParameters>
            </asp:SqlDataSource>

            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <div>
                <h1>Realizar pedido</h1>
                <p>Fecha </p>
                <asp:Label ID="lblFecha" runat="server" Text=""></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblIdCli" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="Label1" runat="server" Text="Cliente "></asp:Label>
                <asp:Label ID="lblNomCli" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="Label7" runat="server" Text="Ambito "></asp:Label>
                <asp:Label ID="lblAmbito" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="Label6" runat="server" Text="Descripcion"></asp:Label>
                <asp:TextBox ID="tbDesc" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="Label3" runat="server" Text="Productos"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GrillaProductos" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GrillaProductos_SelectedIndexChanged" AllowPaging="True" Width="300px" OnPageIndexChanging="GrillaProductos_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="ID_PRODUCTO" SortExpression="ID_PRODUCTO"></asp:BoundField>
                                <asp:BoundField DataField="ID_PROV" HeaderText="ID_PROV" InsertVisible="False" SortExpression="ID_PROV"></asp:BoundField>
                                <asp:BoundField DataField="ID_STOCK" HeaderText="ID_STOCK" InsertVisible="False" SortExpression="ID_STOCK"></asp:BoundField>
                                <asp:BoundField DataField="ID_TIPO" HeaderText="ID_TIPO" SortExpression="ID_TIPO"></asp:BoundField>
                                <asp:BoundField DataField="IdEstado" HeaderText="IdEstado" SortExpression="IdEstado"></asp:BoundField>
                                <asp:BoundField DataField="NOMBRE_PROV" HeaderText="NOMBRE_PROV" SortExpression="NOMBRE_PROV"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="COD_PROD" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="DESC" HeaderText="Tipo" SortExpression="DESC"></asp:BoundField>
                                <asp:BoundField DataField="FECHA_INGRESO" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Ingreso" SortExpression="FECHA_INGRESO"></asp:BoundField>
                                <asp:BoundField DataField="FECHA_VTO" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Vencimiento" SortExpression="FECHA_VTO"></asp:BoundField>
                                <asp:BoundField DataField="LOTE" HeaderText="LOTE" SortExpression="LOTE"></asp:BoundField>
                                <asp:BoundField DataField="DescEstado" HeaderText="Estado" SortExpression="DescEstado"></asp:BoundField>
                                <asp:BoundField DataField="PrecioVentaPub" HeaderText="Precio publico" SortExpression="PrecioVentaPub"></asp:BoundField>
                                <asp:BoundField DataField="PrecioVentaPriv" HeaderText="Precio privado" SortExpression="PrecioVentaPriv"></asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD" SortExpression="CANTIDAD"></asp:BoundField>
                                <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="Label5" runat="server" Text="Producto"></asp:Label>
                        <br />
                        <asp:Label ID="lblNomProd" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="LblPrecio" runat="server"></asp:Label>
                        <asp:Label ID="Label4" runat="server" Text="cantidad"></asp:Label>
                        <br />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Indique la cantidad de productos a vender" Text="*" ControlToValidate="tbCantidad" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                        <asp:TextBox ID="tbCantidad" runat="server" Text="0" TextMode="Number" Enabled="false"></asp:TextBox>
                        <br />
                        <asp:Button ID="Button1" runat="server" Text="Agregar producto" OnClick="Button1_Click" Enabled="false"/>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GrillaProductoPedido" runat="server" AutoGenerateColumns="False" DataKeyNames="ID,IDStock" OnRowDeleting="GrillaProductoPedido_RowDeleting"
                             AllowPaging="True" Width="300px" OnPageIndexChanging="GrillaProductoPedido_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="ID" ReadOnly="True" SortExpression="ID"></asp:BoundField>
                                <asp:BoundField DataField="IDStock" ReadOnly="True" SortExpression="IDStock"></asp:BoundField>
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" ReadOnly="True" SortExpression="nomProv"></asp:BoundField>
                                <asp:BoundField HeaderText="Precio" DataField="Precio" ReadOnly="True" SortExpression="Precio"></asp:BoundField>
                                <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" ReadOnly="True" SortExpression="cantidad"></asp:BoundField>
                                <asp:BoundField HeaderText="SubTotal" DataField="SubTotal" ReadOnly="True" SortExpression="SubTotal"></asp:BoundField>
                                <asp:CommandField ShowDeleteButton="True"/>
                            </Columns>
                        </asp:GridView>

                        <asp:Label ID="Label8" runat="server" Text="Total: $ "></asp:Label>
                        <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>

                        <br />
                        <asp:Label ID="Label9" runat="server" Text="Viatico"></asp:Label>
                        <br />
                        <asp:Label ID="LblViatico" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Coloque 0 si no se sumaran los viaticos" Text="*" ControlToValidate="TbViatico"
                            MinimumValue="0" MaximumValue="2147483647" Type="Integer"></asp:RangeValidator>
                        <asp:TextBox ID="TbViatico" runat="server" Enabled="false" TextMode="Number" Text="0"></asp:TextBox>
                        <asp:CheckBox ID="CBviatico" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="CBviatico_CheckedChanged" />
                        <br />
                        <asp:Button ID="BtnViatico" runat="server" Text="Agregar viatico" Enabled="false" OnClick="BtnViatico_Click" />
                        <br />
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button ID="Button2" runat="server" Text="Cargar pedido" OnClick="Button2_Click" />
                <br />
                <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="ListadoPedidos.aspx">Lista de pedidos</asp:HyperLink>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
