<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListadoPedidos.aspx.cs" Inherits="LabSystemPP3.PantallaPedido.ListadoPedidos" %>

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
    <h1>Lista de pedidos</h1>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GrillaPedidos" runat="server" AutoGenerateColumns="False" OnRowCommand="GrillaPedidos_RowCommand" AllowPaging="True"
                OnPageIndexChanging="GrillaPedidos_PageIndexChanging" Width="300px">
                <Columns>
                    
                    <asp:BoundField DataField="IdPedido" HeaderText="ID" SortExpression="IdPedido"></asp:BoundField>
                    <asp:BoundField DataField="IdCliente" SortExpression="IdCliente"></asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha"></asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"></asp:BoundField>
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente"></asp:BoundField>
                    <asp:BoundField DataField="Ambito" HeaderText="Ambito" SortExpression="Ambito"></asp:BoundField>
                    <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado"></asp:BoundField>
                    <asp:BoundField DataField="Viatico" HeaderText="Viatico" SortExpression="Viatico"></asp:BoundField>
                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total"></asp:BoundField>
                    <asp:ButtonField CommandName="Confirmar" Text="Confirmar"></asp:ButtonField>
                    <asp:ButtonField CommandName="Cancel" Text="Cancelar"></asp:ButtonField>
                    <asp:ButtonField CommandName="Delete" Text="Eliminar"></asp:ButtonField>
                    <asp:HyperLinkField
                        DataNavigateUrlFields="IdPedido,IdCliente,Cliente,Ambito,Viatico,Total,Descripcion"
                        DataNavigateUrlFormatString="~/Venta/NuevaVenta.aspx?IdPedido={0}&IdCliente={1}&Cliente={2}&Ambito={3}&Viatico={4}&Total={5}&Descripcion={6}"
                        NavigateUrl="~/Venta/NuevaVenta.aspx" Text="Vender" HeaderText="Venta"></asp:HyperLinkField>
                    <asp:HyperLinkField
                        DataNavigateUrlFields="IdPedido,IdCliente,Cliente,Ambito,Viatico,Total,Descripcion"
                        DataNavigateUrlFormatString="~/PantallaPedido/PedidoVentaModificar.aspx?IdPedido={0}&IdCliente={1}&Cliente={2}&Ambito={3}&Viatico={4}&Total={5}&Descripcion={6}"
                        NavigateUrl="~/PantallaPedido/PedidoVentaModificar.aspx" Text="Modificar" HeaderText="Modificar"></asp:HyperLinkField>
                </Columns>

            </asp:GridView>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
    </form>
</body>
</html>
