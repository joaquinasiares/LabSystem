<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestorPedido.aspx.cs" Inherits="LabSystemPP3.PantallaPedido.GestorPedido1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">Volver</asp:HyperLink>
        <div>
            <h1>Seleccionar pedido</h1>
            <h3>Tipo de pedido</h3>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:DropDownList ID="DDLPedidos" runat="server"></asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <h3>Cliente</h3>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="search-bar">
                        <asp:TextBox ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>

                    <div class="grid-container">
                        <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="ID" ReadOnly="True" SortExpression="ID"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="listaI" DataField="Cuit" HeaderText="Cuit"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="listaI" DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="listaI" DataField="Razon social" HeaderText="Razon social"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="listaI" DataField="IVA" HeaderText="IVA"></asp:BoundField>
                                <asp:BoundField ItemStyle-CssClass="listaI" DataField="Ambito" HeaderText="Ambito"></asp:BoundField>

                                <asp:ButtonField CommandName="Select" Text="seleccionar"></asp:ButtonField>
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
            <asp:Button ID="Button1" runat="server" Text="Realizar el pedido" OnClick="Button1_Click" />
        </div>
    </form>
    <script src="Pedidos.js"></script>
</body>
</html>
