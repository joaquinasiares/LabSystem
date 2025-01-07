<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestorOrdenDeCompra.aspx.cs" Inherits="LabSystemPP3.PantallaOrdendecompra.WebForm1" %>

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
            <div>
                <h1>Crear Orden de compra</h1>

                <h2>Selecciona al Proveedor</h2>
                <div>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="DDlProv" runat="server"></asp:DropDownList>
                            <asp:TextBox ID="TbBucProveedor" runat="server"></asp:TextBox>
                            <asp:Button ID="BtnBuscarProv" runat="server" Text="Buscar" OnClick="BtnBuscarProv_Click" />
                            <asp:Button ID="BtnSelecProv" runat="server" Text="Seleccionar" />
                            <asp:GridView ID="GrillaProv" runat="server" AutoGenerateColumns="False" OnRowCommand="gvItems_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Name" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button HeaderText="Agregar" ID="btnSelect" runat="server" CommandName="Select" CommandArgument='<%# Eval("ID") %>' Text="Seleccionar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <br />
                            <asp:Label ID="Label1" runat="server" Text="Proveedor: "></asp:Label>
                            <asp:Label ID="lblProbID" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblprovNom" runat="server" Text=""></asp:Label>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <br />
                <div>
                    <h3>Productos
                    </h3>

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="TbBuscarProd" runat="server"></asp:TextBox>
                            <asp:Button ID="BtnBuscarProd" runat="server" Text="Buscar" />
                            <asp:GridView ID="GrillaProducto" runat="server" AutoGenerateColumns="False" OnRowCommand="gvItemsProd_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button HeaderText="Agregar" ID="btnSelectprod" runat="server" CommandName="Select" CommandArgument='<%# Eval("ID") %>' Text="Seleccionar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <br />
                <div>
                    <h3>Cantidad</h3>

                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblIdProd" runat="server"></asp:Label>
                            <asp:Label ID="lblProdNom" runat="server" Text=""></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="TbCantidad" runat="server" Width="40px">0</asp:TextBox>
                            <asp:Button ID="mas" runat="server" Text="+" OnClick="menos_Click" />
                            <asp:Button ID="menos" runat="server" Text="-" OnClick="menos_Click" />
                            <asp:Button ID="BtnSelectCant" runat="server" Text="Seleccionar" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div>
                    <h3>Pedido</h3>
                    <asp:GridView ID="GridView2" runat="server"></asp:GridView>
                    <asp:Button ID="BtnPedido" runat="server" Text="Generar pedido" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
