<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Modificar.aspx.cs" Inherits="LabSystemPP3.PantallaProveedores.Modificar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="ProveedorEstilos.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="modal-container" id="modal-containerDesc" style="visibility: visible; opacity: 1;">

            <div class="modal-body" id="VentanaDesc" style="visibility: visible; opacity: 1;">
                <div class="modal-header">
                    <h3>Modificar proveedor</h3>
                </div>

                <div class="modal-content">
                    <asp:Label CssClass="modalitem" ID="Label1" runat="server" Text="Nombre"></asp:Label>
                    <asp:TextBox ID="TbNom" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label CssClass="modalitem" ID="lblDescNom" runat="server" Text="Cuit"></asp:Label>
                    <asp:TextBox ID="TbCuit" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label CssClass="modalitem" ID="lblDescMostrar" runat="server" Text="Descrpcion"></asp:Label>
                    <asp:TextBox ID="TbDesc" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <asp:Button CssClass="modalitem Inp btn" ID="Button1" runat="server" Text="Modificar" OnClick="Button1_Click" />
                    <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                </div>
                <a href="GestorProveedores.aspx"><asp:Label CssClass="upd-cerrar" ID="lblDescCerrar" runat="server" Text="x"></asp:Label></a>
            </div>
        </div>
        <asp:TextBox ID="TbId" runat="server" Enabled="False" Visible="false"></asp:TextBox>
    </form>
</body>
</html>
