<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestorProveedores.aspx.cs" Inherits="LabSystemPP3.PantallaProveedores.GestorProveedores" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="ProveedorEstilos.css" />

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProveedorALL"
        DeleteCommandType="StoredProcedure" DeleteCommand="deleteproveedor"
        InsertCommandType="StoredProcedure" InsertCommand="InsertContactoProveedor"
        UpdateCommandType="StoredProcedure" UpdateCommand="UpdateContactoProveedor">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="id" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Tel" Type="String"></asp:Parameter>
            <asp:Parameter Name="Email" Type="String"></asp:Parameter>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Tel" Type="String"></asp:Parameter>
            <asp:Parameter Name="Email" Type="String"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProveedor">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>


    <div class="contenido-central">

        <div id="encabezado">
            <h1>Proveedores</h1>

        </div>

        <div class="search-bar">
            <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Proveedor"></asp:TextBox>
            <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

        </div>

        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False"
                        DataKeyNames="ID_PROV" AllowPaging="True" OnPageIndexChanging="Grilla_PageIndexChanging" PageSize="6"
                        OnRowDeleting="Grilla_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ID_PROV" HeaderText="ID_PROV" SortExpression="ID_PROV" Visible="false"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="CUIT" HeaderText="CUIT" SortExpression="CUIT"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="NOMBRE_PROV" HeaderText="Nombre" SortExpression="NOMBRE_PROV"></asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCION" SortExpression="Descripción"></asp:BoundField>
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono"></asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email"></asp:BoundField>

                            <asp:TemplateField HeaderText="Seleccionar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                        ToolTip="Seleccionar proveedor" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Modificar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgUpdate" runat="server" ImageUrl="~/iconos/Modificar.png"
                                        ToolTip="Modificar" CommandName="Modificar" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/iconos/borrar.png"
                                        CommandName="Delete" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <div class="boton-agregar">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <asp:Button CssClass="add-button" ID="btnNuevaOrdComp" runat="server" Text="Nueva orden de compra" OnClick="btnNuevaOrdComp_Click" />
                    <asp:Button CssClass="add-button" ID="Button1" runat="server" Text="Agregar" OnClick="BtnModal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!--hacer que la label se ve tiene color blanco-->
        <div class="section">
            <asp:UpdatePanel ID="updpLblResOpe" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblResultadoOperacion" runat="server" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <div class="modal-container" id="modal-container">
        <div class="modal-body" id="VentanaUPD">
            
                        <asp:UpdatePanel ID="udpCerrarModal" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:Button ID="BtnCerrarModal" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="BtnCerrarModal_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
            <div class="modal-header">
                <asp:UpdatePanel ID="udpTituloModal" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <h3>
                            <asp:Label ID="lblTituloModal" runat="server" Text="">
                            </asp:Label>
                        </h3>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <asp:Label CssClass="modalitem" ID="Label3" runat="server" Text="CUIT: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtCuit" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ErrorMessage="Ingrese datos en el campo Cuit" ControlToValidate="txtCuit" Text="*"></asp:RequiredFieldValidator>

                        <asp:Label CssClass="modalitem" ID="Label5" runat="server" Text="Nombre: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtNombre" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ErrorMessage="Ingrese datos en el campo Nombre" ControlToValidate="txtNombre" Text="*"></asp:RequiredFieldValidator>

                        <asp:Label CssClass="modalitem" ID="Label4" runat="server" Text="Descripción: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtDesc" runat="server"></asp:TextBox>

                        <asp:Label CssClass="modalitem" ID="Label1" runat="server" Text="Telefono: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtTel" runat="server" TextMode="Number"></asp:TextBox>

                        <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Email: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtEmail" runat="server" TextMode="Email"></asp:TextBox>

                        <asp:Button CssClass="modalitem Inp btn" ID="BtnAgregar" runat="server" Text="Guardar" OnClick="BtnAgregar_Click" />
                        <asp:Button CssClass="modalitem Inp btn" ID="BtnModificar" runat="server" Text="Modificar" OnClick="BtnModificar_Click" Visible="false" Enabled="false" />

                        <br />

                        <div class="section">
                            <asp:UpdatePanel ID="ModalupdpLblResOpe" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label CssClass="modalitem" ID="ModallblResultadoOperacion" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <asp:Label ID="lblprov" runat="server" Text="" Visible="false"></asp:Label>

                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>



    <script src="ProveedorJavaSc.js"></script>
</asp:Content>

