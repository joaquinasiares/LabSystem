<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AltaCliente.aspx.cs" Inherits="LabSystemPP3.ALTA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/PantallaCliente/GestorClientes.aspx">Volver</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">Inicio</asp:HyperLink>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:SqlDataSource ID="SqlContacto" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            InsertCommandType="StoredProcedure" InsertCommand="InsertContactoCliente">
            <InsertParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="ID_CLIENTE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="Contacto" Type="String"></asp:Parameter>
                <asp:Parameter Name="DESCRIPCION" Type="String"></asp:Parameter>
            </InsertParameters>
        </asp:SqlDataSource>
        <div>
            <h1>Agregar cliente</h1>
            <br />
            <asp:Label ID="Label4" runat="server" Text="Nombre: "></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Llena el campo Nombre" Text="*"
                ControlToValidate="txtNombre"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="Label1" runat="server" Text="Condicion IVA: "></asp:Label>
            <asp:DropDownList ID="listaIva" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Cuit: "></asp:Label>
            <asp:TextBox ID="txtCuit" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Llena el campo Cuit" Text="*"
                ControlToValidate="txtCuit"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Razon social: "></asp:Label>
            <asp:TextBox ID="txtRazonSocial" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Llena el campo Razon social" Text="*"
                ControlToValidate="txtRazonSocial"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="Label5" runat="server" Text="Ambito: "></asp:Label>
            <asp:DropDownList ID="listaAmbito" runat="server"></asp:DropDownList>
            <br />
            <asp:Label ID="Label6" runat="server" Text="Contacto: "></asp:Label>
            <asp:TextBox ID="TbContacto" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label7" runat="server" Text="Descripcion: "></asp:Label>
            <asp:TextBox ID="TbDesCont" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="Button2" runat="server" Text="Agregar contacto" OnClick="Button2_Click" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GrillaContacto" runat="server" AllowPaging="true" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Contacto" HeaderText="Contacto" ReadOnly="True" SortExpression="Contacto"></asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Button ID="Button1" runat="server" Text="AgregarCliente" OnClick="Button1_Click" />
            <asp:Label ID="lblResultado" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
