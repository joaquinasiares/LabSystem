<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestorEmpleados.aspx.cs" Inherits="LabSystemPP3.PantallaEmpleados.GestorEmpleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="VentanaModal.css" />




    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:SqlDataSource ID="SqlEmpleado" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleadoAll"
        DeleteCommandType="StoredProcedure" DeleteCommand="DeleteEmpleado"
        UpdateCommandType="StoredProcedure" UpdateCommand="UpdateEmpleado"
        InsertCommandType="StoredProcedure" InsertCommand="InsertEmpleado"
        OnInserted="SqlEmpleado_Inserted"
        OnUpdated="SqlEmpleado_Updated">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idEmpleado" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="dni" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
            <asp:Parameter Name="apellido" Type="String"></asp:Parameter>
            <asp:Parameter Name="telefono" Type="String"></asp:Parameter>
            <asp:Parameter Name="email" Type="String"></asp:Parameter>
            <asp:Parameter Name="idEmpleado" Type="Int32" Direction="Output" />
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idEmp" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="dni" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
            <asp:Parameter Name="apellido" Type="String"></asp:Parameter>
            <asp:Parameter Name="telefono" Type="String"></asp:Parameter>
            <asp:Parameter Name="email" Type="String"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlBuscarEmpleado" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleado">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nom" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqltipoEmp" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectTipoEmpleado">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>


    <div class="contenido-central">

        <h1>Empleados</h1>
        <!-- Barra de búsqueda -->
        <div class="search-bar">
            <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
            <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

        </div>

        <asp:SqlDataSource ID="SqlAreas" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>"
            InsertCommandType="StoredProcedure" InsertCommand="InsertAreaEmpleado"
            SelectCommandType="StoredProcedure" SelectCommand="SelectAreaEmpleado"
            UpdateCommandType="StoredProcedure" UpdateCommand="ModificarAreaEmpleado">
            <InsertParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idEmp" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idArea" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="visible" Type="Int32"></asp:Parameter>
            </InsertParameters>
            <SelectParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idEmp" Type="Int32"></asp:Parameter>
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idEmp" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idArea" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="visible" Type="Int32"></asp:Parameter>
            </UpdateParameters>
        </asp:SqlDataSource>

        <!-- Contenedor de la grilla con scroll -->
        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" EnablePartialRendering="true">
                <ContentTemplate>
                    <asp:GridView ID="GrillaEmpleado" CssClass="lista" runat="server" OnRowCommand="GrillaEmpleado_RowCommand" AutoGenerateColumns="False"
                        AllowPaging="true" PageSize="5" OnPageIndexChanging="GrillaEmpleado_PageIndexChanging" OnRowDeleting="GrillaEmpleado_RowDeleting" 
                        DataKeyNames="ID_EMPLEADO_SERVICIO,ID_CONTACTO,DNI_EMP,NOMBRE_EMP,APELLIDO_EMP,Telefono,Email">
                        <Columns>
                            <asp:BoundField DataField="ID_EMPLEADO_SERVICIO" HeaderText="ID" ReadOnly="True" InsertVisible="False" SortExpression="ID_EMPLEADO_SERVICIO"></asp:BoundField>
                            <asp:BoundField DataField="DNI_EMP" HeaderText="Dni" SortExpression="DNI_EMP"></asp:BoundField>
                            <asp:BoundField DataField="NOMBRE_EMP" HeaderText="Nombre" SortExpression="NOMBRE_EMP"></asp:BoundField>
                            <asp:BoundField DataField="APELLIDO_EMP" HeaderText="Apellido" SortExpression="APELLIDO_EMP"></asp:BoundField>
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono"></asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email"></asp:BoundField>
                            <asp:BoundField DataField="ID_CONTACTO" HeaderText="ID_CONTACTO" ReadOnly="True" InsertVisible="False" Visible="false" SortExpression="ID_CONTACTO"></asp:BoundField>

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

                            <asp:TemplateField HeaderText="Areas">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgAreas" runat="server" ImageUrl="~/iconos/Ver.png"
                                        ToolTip="Ver areas" CommandName="Areas" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
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
                        <asp:Label CssClass="modalitem" ID="Label1" runat="server" Text="Area "></asp:Label>
                        <div class="modalitem Inp cbTipoEmp">
                            <asp:CheckBoxList ID="CheckBoxList1" CssClass="" runat="server" DataTextField="descTipoEmp" DataValueField="idTipoEmp"></asp:CheckBoxList>
                        </div>
                        <asp:Label CssClass="modalitem" ID="Label9" runat="server" Text="Dni "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" MaxLength="8" ID="TbDni" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="empDatos"
                            ErrorMessage="Ingrese datos en el campo Dni" ControlToValidate="TbDni" Text="*"></asp:RequiredFieldValidator>

                        <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Nombre "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="Tbnom" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="empDatos"
                            ErrorMessage="Ingrese datos en el campo Nombre" ControlToValidate="Tbnom" Text="*"></asp:RequiredFieldValidator>

                        <asp:Label CssClass="modalitem" ID="Label3" runat="server" Text="Apellido "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="TbApe" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="empDatos"
                            ErrorMessage="Ingrese datos en el campo Apelllido" ControlToValidate="TbApe" Text="*"></asp:RequiredFieldValidator>

                        <asp:Label CssClass="modalitem" ID="Label4" runat="server" Text="Telefono "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" MaxLength="10" ID="TbTel" runat="server"></asp:TextBox>


                        <asp:Label CssClass="modalitem" ID="Label5" runat="server" Text="Email "></asp:Label>
                        <asp:TextBox CssClass="modalitem Inp" ID="TbEmail" TextMode="Email" runat="server"></asp:TextBox>

                        <asp:Button CssClass="modalitem Inp btn" ID="BtnAgregar" runat="server" Text="Guardar" OnClick="BtnAgregar_Click" />
                        <asp:Button CssClass="modalitem Inp btn" ID="BtnModificar" runat="server" Text="Modificar" OnClick="BtnModificar_Click" Visible="false" Enabled="false" />
                        <asp:ValidationSummary CssClass="modalitem" ID="ValidationSummary1" runat="server" ValidationGroup="empDatos" />

                        <asp:Label ID="lblMail" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblTel" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label CssClass="modalitem" ID="lblError" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblIdEmp" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Button ID="BtnCerrarModal" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="BtnCerrarModal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>

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


    <!--**********************************************************************************-->

    <div class="modal-container" id="modal-containerVerAreas">
        <div class="modal-body-areas" id="VentanaVerAreas">
            <div class="modal-header">
                <h4>Areas del empleado</h4>
            </div>

            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:GridView ID="GridViewAreas" CssClass="lista" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="idTipoEmp" HeaderText="idÁreas" Visible="false"/>
                                <asp:BoundField DataField="descTipoEmp" HeaderText="Áreas" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>


            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Button ID="btnCerrarAreas" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="btnCerrarAreas_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <script src="Empleado.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>
