<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Modifiacar.aspx.cs" Inherits="LabSystemPP3.PantallaCliente.Modifiacr" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Id"></asp:Label>
            <br />
            <asp:TextBox ID="tbId" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Nombre"></asp:Label>
            <br />
            <asp:TextBox ID="tbNom" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Cuit"></asp:Label>
            <br />
            <asp:TextBox ID="tbCuit" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label4" runat="server" Text="RazonSocial"></asp:Label>
            <br />
            <asp:TextBox ID="tbRS" runat="server"></asp:TextBox>
            <br />
            <asp:DropDownList ID="listaIva" runat="server"></asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="Label6" runat="server" Text="Ambito"></asp:Label>
            <br />
            <asp:DropDownList ID="listaAmbito" runat="server"></asp:DropDownList>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
