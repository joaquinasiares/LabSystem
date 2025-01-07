<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestorClientes.aspx.cs" Inherits="LabSystemPP3.PantallaCliente.WebForm1" EnableEventValidation="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="VentanaModal.css" rel="stylesheet" type="text/css" />

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectClienteALL"
        DeleteCommandType="StoredProcedure" DeleteCommand="deleteCliente">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="id" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectCliente">
        <SelectParameters>
            <asp:Parameter DefaultValue="" Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Nombre" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>




    <div class="contenido-central">
        <div id="encabezado">
            <h1>Administrar clientes</h1>

        </div>
        <!-- Barra de búsqueda -->
        <div class="search-bar">
            <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
            <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

        </div>

        <!-- Contenedor de la grilla con scroll -->
        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" EnablePartialRendering="true">
                <ContentTemplate>
                    <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False"
                        DataKeyNames="ID_CLIENTE" AllowPaging="true" PageSize="7" OnPageIndexChanging="Grilla_PageIndexChanging" OnRowDeleting="Grilla_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ID_CLIENTE" HeaderText="ID_CLIENTE" ReadOnly="True" InsertVisible="False" SortExpression="ID_CLIENTE" Visible="False"></asp:BoundField>

                            <asp:BoundField DataField="CUIT_CLIENTE" HeaderText="Cuit" SortExpression="CUIT_CLIENTE"></asp:BoundField>
                            <asp:BoundField DataField="NOM_CLIENTE" HeaderText="Nombre" SortExpression="NOM_CLIENTE"></asp:BoundField>
                            <asp:BoundField DataField="RAZON_SOCIAL" HeaderText="Razon social" SortExpression="RAZON_SOCIAL"></asp:BoundField>
                            <asp:BoundField DataField="priv_pub" HeaderText="Ambito" ReadOnly="True" SortExpression="priv_pub"></asp:BoundField>
                            <asp:BoundField DataField="DESC_COND_IVA" HeaderText="IVA" SortExpression="DESC_COND_IVA"></asp:BoundField>
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono"></asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email"></asp:BoundField>

                            <asp:TemplateField HeaderText="Modificar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgUpdate" runat="server" ImageUrl="~/iconos/Modificar.png"
                                        ToolTip="Modificar" CommandName="Modificar" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' OnClick="imgUpdate_Click" />
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

        <!-- Botón de agregar debajo de la grilla -->
        <br />
        <div class="boton-agregar">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <asp:Button CssClass="add-button" ID="Button1" runat="server" Text="Agregar" OnClick="BtnModal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
    <div class="section">
        <asp:UpdatePanel ID="updpLblResOpe" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label CssClass="modalitem" ID="lblResultadoOperacion" runat="server" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>







    <!--   ----------------------------------------  -->

    <asp:SqlDataSource ID="SqlDataSourceContacto" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" InsertCommandType="StoredProcedure"
        InsertCommand="InsertContactoCliente" UpdateCommandType="StoredProcedure" UpdateCommand="UpdateContactoCliente">
        <InsertParameters>
            <asp:Parameter Name="RETURN_VALUE" Direction="ReturnValue" Type="Int32" />
            <asp:Parameter Name="idCli" Type="Int32" />
            <asp:ControlParameter ControlID="TxtTelefono" Name="Tel" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="TxtEmail" Name="Email" PropertyName="Text" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idCli" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Tel" Type="String"></asp:Parameter>
            <asp:Parameter Name="Email" Type="String"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>
    <div class="modal-container" id="modal-container">
        <div class="modal-body" id="VentanaUPD">
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
                        <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Condición IVA: "></asp:Label>
                        <asp:DropDownList CssClass="modalitem Inp" ID="listaIva" runat="server"></asp:DropDownList>

                        <asp:Label CssClass="modalitem" ID="Label3" runat="server" Text="CUIT: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtCuit" TextMode="Number" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ErrorMessage="Ingrese datos en el campo Cuit" ControlToValidate="txtCuit" Text="*"></asp:RequiredFieldValidator>

                        <asp:Label CssClass="modalitem" ID="Label4" runat="server" Text="Razón Social: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtRazonSocial" runat="server"></asp:TextBox>


                        <asp:Label CssClass="modalitem" ID="Label5" runat="server" Text="Nombre: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="txtNombre" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ErrorMessage="Ingrese datos en el campo Nombre" ControlToValidate="txtNombre" Text="*"></asp:RequiredFieldValidator>

                        <asp:Label CssClass="modalitem" ID="Label6" runat="server" Text="Ámbito: "></asp:Label>
                        <asp:DropDownList CssClass="modalitem Inp" ID="listaAmbito" runat="server"></asp:DropDownList>

                        <asp:Label CssClass="modalitem" ID="Label1" runat="server" Text="Telefono: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="TxtTelefono" TextMode="Number" runat="server"></asp:TextBox>


                        <asp:Label CssClass="modalitem" ID="Label7" runat="server" Text="Email: "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="TxtEmail" TextMode="Email" runat="server"></asp:TextBox>

                        <asp:Button CssClass="modalitem Inp btn" ID="BtnAgregar" runat="server" Text="Guardar" OnClick="BtnAgregar_Click" />
                        <asp:Button CssClass="modalitem Inp btn" ID="BtnModificar" runat="server" Text="Modificar" OnClick="BtnModificar_Click" Visible="false" Enabled="false" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />

                        <asp:Label ID="lblMail" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblTel" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblIdCli" runat="server" Text="" Visible="false"></asp:Label>

                        <asp:Label ID="lblCrear" runat="server" Text=""></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="udpCerrarModal" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Button ID="BtnCerrarModal" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="BtnCerrarModal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <div class="section">
                <asp:UpdatePanel ID="ModalupdpLblResOpe" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label CssClass="modalitem" ID="ModallblResultadoOperacion" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>



    <script src="ClienteJavSc.js">

    </script>

</asp:Content>

