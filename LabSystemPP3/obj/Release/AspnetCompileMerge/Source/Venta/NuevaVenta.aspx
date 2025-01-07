<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevaVenta.aspx.cs" Inherits="LabSystemPP3.Venta.NuevaVenta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div>
            <asp:SqlDataSource ID="SqlProductos" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>" SelectCommandType="StoredProcedure" SelectCommand="SelectDetallePedido">
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlMediodPago" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>" SelectCommand="SELECT ID_PAGO, DESC_MP FROM MEDIODEPAGO"></asp:SqlDataSource>


            <h1>Registar venta</h1>
            <h3>Detalles de la venta</h3>
            <asp:Label ID="lblIdPed" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblIdCli" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="Label1" runat="server" Text="Cliente "></asp:Label>
            <asp:Label ID="lblNom" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Ambito "></asp:Label>
            <asp:Label ID="lblAmbito" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label4" runat="server" Text="Viatico "></asp:Label>
            <asp:Label ID="lblViatico" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label6" runat="server" Text="Total "></asp:Label>
            <asp:Label ID="lblTotal" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label8" runat="server" Text="Descripcion"></asp:Label>
            <br />
            <asp:Label ID="lblDesc" runat="server" Text="Label"></asp:Label>
            <h3>Productos</h3>
            <asp:GridView ID="GrillaProductos" runat="server" DataSourceID="SqlProductos" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="ID_DETALLE_PEDIDO,IdProd">
                <Columns>
                    <asp:BoundField DataField="ID_DETALLE_PEDIDO" HeaderText="ID_DETALLE_PEDIDO" ReadOnly="True" InsertVisible="False" SortExpression="ID_DETALLE_PEDIDO" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="IdProd" HeaderText="IdProd" SortExpression="IdProd" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="ID_STOCK" HeaderText="ID_STOCK" ReadOnly="True" InsertVisible="False" SortExpression="ID_STOCK" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                    <asp:BoundField DataField="precio" HeaderText="Precio" SortExpression="precio"></asp:BoundField>
                    <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                    <asp:BoundField DataField="SubTotal" HeaderText="Subtotal" SortExpression="SubTotal"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <br />
            <h3>Medio de pago</h3>
            <asp:DropDownList ID="DropDownList1" runat="server" DataTextField="DESC_MP" DataValueField="ID_PAGO" DataSourceID="SqlMediodPago"></asp:DropDownList>
            <br />
            <h3>Facturas</h3>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <br />
            <asp:Button ID="btnSubirFacturas" runat="server" Text="Agregar facturas" OnClick="btnSubirFacturas_Click" />
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GrillaFacturas" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Url">
                        <Columns>
                            <asp:BoundField DataField="Url" HeaderText="Url" ReadOnly="True" SortExpression="Url" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="NombreFactura" HeaderText="Nombre" ReadOnly="True" SortExpression="Nombre"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <h3>Fecha de pago</h3>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:CheckBox ID="CbFecha" runat="server" Text="Agregar fecha" AutoPostBack="true" Checked="false" OnCheckedChanged="CbFecha_CheckedChanged" />
                    <br />
                    <asp:TextBox ID="TbFecha" runat="server" TextMode="Date" Visible="false"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
