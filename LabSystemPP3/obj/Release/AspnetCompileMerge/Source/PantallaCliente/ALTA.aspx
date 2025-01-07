<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ALTA.aspx.cs" Inherits="LabSystemPP3.ALTA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Condicion IVA: "></asp:Label>
            <asp:DropDownList ID="listaIva" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Cuit: "></asp:Label>
            <asp:TextBox ID="txtCuit" runat="server"></asp:TextBox>
            <br />
             <asp:Label ID="Label3" runat="server" Text="Razon social: "></asp:Label>
            <asp:TextBox ID="txtRazonSocial" runat="server"></asp:TextBox>
             <br />
            <asp:Label ID="Label4" runat="server" Text="Nombre: "></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label5" runat="server" Text="Ambito: "></asp:Label>
            <asp:DropDownList ID="listaAmbito" runat="server"></asp:DropDownList>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
