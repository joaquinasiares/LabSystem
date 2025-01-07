<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestorUsuario.aspx.cs" Inherits="LabSystemPP3.PantallaUsuarios.GestorUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="VentanaModal.css" />




    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:SqlDataSource ID="SqlUsuarios" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectUsuarios"
        DeleteCommandType="StoredProcedure" DeleteCommand="BorrarUsuario"
        UpdateCommandType="StoredProcedure" UpdateCommand="ModificarUsuario"
        InsertCommandType="StoredProcedure" InsertCommand="CrearUsuario"
        OnInserted="SqlUsuarios_Inserted"
        OnUpdated="SqlUsuarios_Updated">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idUsuario" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="usuario" Type="String"></asp:Parameter>
            <asp:Parameter Name="clave" Type="String"></asp:Parameter>
            <asp:Parameter Name="idTipo" Type="Int32"></asp:Parameter>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idUsuario" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="usuario" Type="String"></asp:Parameter>
            <asp:Parameter Name="clave" Type="String"></asp:Parameter>
            <asp:Parameter Name="idTipo" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlTipoUsuario" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectTipoUsuarios">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlBuscarEmpleado" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleado">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nom" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>




    <div class="contenido-central">
       
        <h1>Usuarios</h1>
        <!-- Contenedor de la grilla con scroll -->
        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" EnablePartialRendering="true">
                <ContentTemplate>
                    <asp:GridView ID="GrillaUsuarios" CssClass="lista" OnRowCommand="GrillaUsuarios_RowCommand" runat="server" AutoGenerateColumns="False"
                        AllowPaging="true" PageSize="5" DataKeyNames="ID_USUARIO" OnPageIndexChanging="GrillaUsuarios_PageIndexChanging" OnRowDeleting="GrillaUsuarios_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ID_USUARIO" visible="false" HeaderText="ID usuario" ReadOnly="True" InsertVisible="False" SortExpression="ID_USUARIO"></asp:BoundField>
                            <asp:BoundField DataField="USUARIO" HeaderText="Usuario" SortExpression="USUARIO"></asp:BoundField>
                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" SortExpression="CLAVE"></asp:BoundField>
                            <asp:BoundField DataField="idTipoUsu" HeaderText="idTipoUsu" Visible="false" SortExpression="idTipoUsu"></asp:BoundField>
                            <asp:BoundField DataField="descTipoUsu" HeaderText="Area" SortExpression="descTipoUsu"></asp:BoundField>

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

        <div class="section">
            <asp:UpdatePanel ID="updpLblResOpe" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label CssClass="modalitem" ID="lblResultadoOperacion" runat="server" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>


        <!-- Botón de agregar debajo de la grilla -->
        <div class="boton-agregar">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <asp:Button CssClass="add-button" ID="BtnAbrirModal" runat="server" Text="Agregar" OnClick="BtnAbrirModal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>

    






    <!--   ----------------------------------------  -->

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
                        <asp:Label ID="lblIdUsuario" runat="server" Text="" Visible="false"></asp:Label>

                        <asp:Label CssClass="modalitem" ID="Label9" runat="server" Text="Nombre "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="TbUsuario" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="empDatos"
                            ErrorMessage="Ingrese datos en el campo usuario" ControlToValidate="TbUsuario" Text="*"></asp:RequiredFieldValidator>

                        <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Clave "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="TbCLave" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="empDatos"
                            ErrorMessage="Ingrese datos en el campo clave" ControlToValidate="TbCLave" Text="*"></asp:RequiredFieldValidator>


                        <asp:Label CssClass="modalitem" ID="Label1" runat="server" Text="Area "></asp:Label>
                         <asp:DropDownList ID="ddlTipoUsuario" CssClass="modalitem Inp" runat="server" DataTextField="descTipoUsu" DataValueField="idTipoUsu" DataSourceID="SqlTipoUsuario"></asp:DropDownList>

                        <asp:Button CssClass="modalitem Inp btn" ID="BtnAgregar" runat="server" Text="Guardar" OnClick="BtnAgregar_Click"/>
                        <asp:Button CssClass="modalitem Inp btn" ID="BtnModificar" runat="server" Text="Modificar" Visible="false" Enabled="false" OnClick="BtnModificar_Click"/>
                        <asp:ValidationSummary CssClass="modalitem" ID="ValidationSummary1" runat="server" ValidationGroup="empDatos" />

                        <asp:Label ID="lblCrear" runat="server" Text=""></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


            <asp:Button ID="BtnCerrarModal" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="BtnCerrarModal_Click"/>
            <br />
            <div class="section">
                <asp:UpdatePanel ID="ModalupdpLblResOpe" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label CssClass="modalitem" ID="ModallblResultadoOperacion" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <script src="Empleado.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="sweetalert2.all.min.js"></script>
</asp:Content>
