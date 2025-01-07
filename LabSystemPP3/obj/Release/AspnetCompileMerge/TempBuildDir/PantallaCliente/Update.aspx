<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="LabSystemPP3.PantallaCliente.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--<link href="VentanaModal.css" rel="stylesheet" />-->
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="modal-container" id="modal-container">

                <div class="modal-body" id="VentanaUPD">
                    <div class="modal-header">
                        <h3>Modificar usuario</h3>
                    </div>


                    <div class="modal-content">
                        <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Condicion IVA: "></asp:Label>
                        <asp:DropDownList CssClass="modalitem Inp" ID="listaIva" runat="server">
                        </asp:DropDownList>
                        <asp:Label CssClass="modalitem" ID="Label3" runat="server" Text="Cuit: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtCuit" runat="server"></asp:TextBox>
                        <asp:Label CssClass="modalitem" ID="Label4" runat="server" Text="Razon social: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtRazonSocial" runat="server"></asp:TextBox>
                        <asp:Label CssClass="modalitem" ID="Label5" runat="server" Text="Nombre: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtNombre" runat="server"></asp:TextBox>
                        <asp:Label CssClass="modalitem" ID="Label6" runat="server" Text="Ambito: "></asp:Label>
                        <asp:DropDownList CssClass="modalitem Inp" ID="listaAmbito" runat="server"></asp:DropDownList>
                        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click1" />
                        <asp:Label ID="lblID" CssClass="usuID" runat="server"></asp:Label>
                        <asp:Label ID="lblIdIVA" CssClass="usuID" runat="server"></asp:Label>
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </div>
                    </div>
                </div>
            </div>
    </form>
</body>
</html>
