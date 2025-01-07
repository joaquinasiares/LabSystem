<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModificarProducto.aspx.cs" Inherits="LabSystemPP3.PantallaProductos.ModificarProducto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link rel="stylesheet" href="ProductoEstilos.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="modal-container" id="modal-container" style="visibility: visible; opacity: 1;">

            <div class="modal-body" id="VentanaInsert" style="visibility: visible; opacity: 1;">
                <div class="modal-header">
                    <h3>Modificar producto</h3>
                </div>

                <div class="modal-content">
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Calendar ID="Calendar1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:Label ID="Label8" runat="server" Text="Precio de compra"></asp:Label>
                    <asp:TextBox ID="TbPreciCom" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label9" runat="server" Text="Lote"></asp:Label>
                    <asp:TextBox ID="TbLote" runat="server"></asp:TextBox>
                    <asp:Label ID="Label3" runat="server" Text="Cantidad"></asp:Label>
                    <asp:TextBox ID="TbCantidad" runat="server"></asp:TextBox>
                    <asp:Label ID="Label7" runat="server" Text="Precio de venta"></asp:Label>
                    <br/>
                    <asp:Label ID="Label4" runat="server" Text="Publico"></asp:Label>
                    <asp:TextBox ID="TbPreVenPub" runat="server"></asp:TextBox>
                    <asp:Label ID="Label5" runat="server" Text="Privado"></asp:Label>
                    <asp:TextBox ID="TbPreVenPriv" runat="server"></asp:TextBox>
                    <br />
                </div>

        <asp:Label CssClass="insert-cerrar" ID="lblCerrar" runat="server" Text="x"></asp:Label>
        <asp:Button CssClass="modalitem Inp btn" ID="BtnInsert" runat="server" Text="Modificar" OnClick="Button1_Click" />
                <asp:Label ID="LblId" runat="server" Text="Label" Visible="True"></asp:Label>
    </div>
            
</div>
    </form>
</body>
</html>
