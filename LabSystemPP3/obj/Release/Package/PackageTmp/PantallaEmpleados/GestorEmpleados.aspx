<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestorEmpleados.aspx.cs" Inherits="LabSystemPP3.PantallaEmpleados.GestorEmpleados" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="VentanaModal.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container">

            <div class="search-bar">
                <asp:TextBox ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                <asp:Button ID="Button3" runat="server" Text="Agregar" OnClientClick="mostrarModal(); return false;" />
            </div>

            <div class="grid-container">
                <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="ID" ReadOnly="True" SortExpression="ID"></asp:BoundField>
                        <asp:BoundField DataField="idIva" HeaderText=""></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Cuit" HeaderText="Cuit"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Razon social" HeaderText="Razon social"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="IVA" HeaderText="IVA"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="listaI" DataField="Ambito" HeaderText="Ambito"></asp:BoundField>

                        <asp:ButtonField CommandName="Borrar" Text="Eliminar"></asp:ButtonField>

                        <asp:HyperLinkField
                            DataNavigateUrlFields="ID,idIva,Cuit,Nombre,Razon social,IVA,Ambito"
                            DataNavigateUrlFormatString="Update.aspx?ID={0}&idIva={1}&Cuit={2}&Nombre={3}&Razon social={4}&IVA={5}&Ambito={6}"
                            NavigateUrl="Modifiacar.aspx" Text="IR" HeaderText="Modifi"></asp:HyperLinkField>
                    </Columns>

                    <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                </asp:GridView>
            </div>
        </div>


        <!--   ----------------------------------------  -->

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="modal-container" id="modal-container">

                    <div class="modal-body" id="VentanaUPD">
                        <div class="modal-header">
                            <h3>Agregar usuario</h3>
                        </div>

                        <div class="modal-content" style="overflow: auto;">
                            <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Nombre: "></asp:Label>
                            <asp:TextBox ID="TbNom" runat="server"></asp:TextBox>
                            <asp:Label CssClass="modalitem" ID="Label3" runat="server" Text="Apellido: "></asp:Label>
                            <asp:TextBox CssClass="modalitem Inp" ID="TbApe" runat="server"></asp:TextBox>
                            <asp:Label CssClass="modalitem" ID="Label4" runat="server" Text="DNI: "></asp:Label>
                            <asp:TextBox CssClass="modalitem Inp" ID="TbDNI" runat="server"></asp:TextBox>

                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    
                                    <asp:Label CssClass="modalitem" ID="Label10" runat="server" Text="Direccion "></asp:Label>
                                    <asp:CheckBox ID="CbDireccion" runat="server" Checked="false" />
                                    <div class="DirecCont">
                                        <div>
                                            <asp:Label CssClass="modalitem" ID="Label1" runat="server" Text="Provincia: "></asp:Label>
                                        
                                            <asp:DropDownList ID="DDLProvincia" runat="server"></asp:DropDownList>
                                        </div>

                                        
                                        <asp:Label CssClass="modalitem" ID="Label7" runat="server" Text="Localidad: "></asp:Label>
                                        
                                        <asp:TextBox ID="TbLocalidad" runat="server"></asp:TextBox>
                                        
                                        <asp:Label CssClass="modalitem" ID="Label8" runat="server" Text="Calle: "></asp:Label>
                                        
                                        <asp:TextBox ID="TbCalle" runat="server"></asp:TextBox>
                                        
                                        <asp:Label CssClass="modalitem" ID="Label9" runat="server" Text="Altura: "></asp:Label>
                                        
                                        <asp:TextBox ID="TbAltura" runat="server"></asp:TextBox>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:Label CssClass="modalitem" ID="Label5" runat="server" Text="Contacto: "></asp:Label>
                            <asp:TextBox CssClass="modalitem Inp" ID="TbContacto" runat="server"></asp:TextBox>


                            <asp:Label ID="lblCrear" runat="server" Text=""></asp:Label>
                        </div>
                        <asp:Button CssClass="modalitem Inp btn" ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
                        <asp:Label CssClass="upd-cerrar" ID="lblCerrar" runat="server" Text="x"></asp:Label>

                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
    <script src="Empleado.js"></script>
</body>
</html>
