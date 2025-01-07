<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestorCompra.aspx.cs" Inherits="LabSystemPP3.PantallaCompra.GestorCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Compras.css" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:SqlDataSource ID="SqlCompra" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectCompras"
        UpdateCommandType="StoredProcedure" UpdateCommand="ModificarEstadoCompra"
        DeleteCommandType="StoredProcedure" DeleteCommand="DeleteCompras">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idCompra" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idCompra" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="estado" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idEmp" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="fecha" Type="DateTime"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDetalleOrdCom" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectDetalleOrdenComp"
        UpdateCommandType="StoredProcedure" UpdateCommand="SumarStockCompra">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="IdOc" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantidad" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlBuscarCompraId" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="BuscarComprasId">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idOrdcom" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlEmpleado" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleadoCompra">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <div class="contenido-central">

        <div id="encabezado">
            <h1>Administrar compra</h1>
        </div>
        <h3>Ordenes de compra</h3>

        <asp:UpdatePanel ID="updBuscarCom" UpdateMode="Always" EnableViewState="true" runat="server">
            <ContentTemplate>
                <div class="search-bar">
                    <asp:TextBox CssClass="Inp" ID="txtBuscar" TextMode="Number" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese el codigo"></asp:TextBox>
                    <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="Grilla" CssClass="lista" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        DataKeyNames="ID_COMPRA,ID_PROV,IdEstadoCom,ID_ORD_COMP" OnRowCommand="Grilla_RowCommand" PageSize="5"
                        OnPageIndexChanging="Grilla_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="ID_COMPRA" HeaderText="ID_COMPRA" ReadOnly="True" Visible="false" InsertVisible="False" SortExpression="ID_COMPRA"></asp:BoundField>
                            <asp:BoundField DataField="ID_ORD_COMP" HeaderText="Numero de orden" SortExpression="ID_ORD_COMP"></asp:BoundField>
                            <asp:BoundField DataField="ID_PROV" HeaderText="ID_PROV" Visible="false" SortExpression="ID_PROV"></asp:BoundField>
                            <asp:BoundField DataField="IdEstadoCom" HeaderText="IdEstadoCom" Visible="false" SortExpression="IdEstadoCom"></asp:BoundField>
                            <asp:BoundField DataField="NOMBRE_PROV" HeaderText="Proveedor" SortExpression="NOMBRE_PROV"></asp:BoundField>
                            <asp:BoundField DataField="DescEstadoCom" HeaderText="Estado" SortExpression="DescEstadoCom"></asp:BoundField>
                            <asp:BoundField DataField="FECHA_SOLICITUD" HeaderText="Fecha de solicitud" SortExpression="FECHA_SOLICITUD"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="FechaConfirmacion" HeaderText="Fecha de confirmación" SortExpression="FechaConfirmacion"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="Registrado por" HeaderText="Registrado por" SortExpression="Registrado por"></asp:BoundField>
                            <asp:BoundField DataField="ID_EMPLEADO_SERVICIO" HeaderText="Id empleado" SortExpression="id empleado" Visible="false"></asp:BoundField>

                            <asp:TemplateField HeaderText="Ver orden de compra">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgVer" runat="server" ImageUrl="~/iconos/Ver.png"
                                        CommandName="Ver" ToolTip="Ver Orden"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cancelar orden">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgCancel" runat="server" ImageUrl="~/iconos/CancelarPV.png"
                                        CommandName="Cancelar" ToolTip="Cancelar compra"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Borrar orden">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBorrar" runat="server" ImageUrl="~/iconos/borrarPV.png"
                                        CommandName="Borrar" ToolTip="Borrar compra"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Confirmar orden">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgConfirmar" runat="server" ImageUrl="~/iconos/confirmar.png"
                                        CommandName="Confirmar" ToolTip="Confirmar compra"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Modificar Orden">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgModificar" runat="server" ImageUrl="~/iconos/editar.png"
                                        CommandName="Modificar" ToolTip="Modificar Orden"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <div class="modal-container" id="modal-container">
            <div class="modal-body" id="VentanaCompra">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" EnableViewState="true">
                    <ContentTemplate>
                        <asp:Button ID="BtnCerrarModalAgregar" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="BtnCerrarModal_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-header">
                    <h3>Confirmar Compra</h3>
                </div>

                <div class="modal-content">

                    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server" EnableViewState="true">
                        <ContentTemplate>

                            <h5>Numero de orden
                                <asp:Label ID="lblNumOrd" runat="server" Text=""></asp:Label></h5>
                            <asp:Label ID="Label22" runat="server" Text="Registrado por"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlEmpleado" runat="server" DataTextField="empleado" DataValueField="ID_EMPLEADO_SERVICIO" DataSourceID="SqlEmpleado"></asp:DropDownList>

                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <div class="section">
                        <asp:UpdatePanel ID="uspConfirmar" runat="server" UpdateMode="Always" EnableViewState="true">
                            <ContentTemplate>
                                <asp:Button CssClass="modalitem Inp btn" ID="btnConfirmarCom" runat="server" Text="Registrar" OnClick="btnConfirmarCom_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:Label CssClass="modalitem" ID="LblResultado" runat="server" Text=""></asp:Label>

                </div>

            </div>
        </div>


    </div>
    <script src="Compra.js"></script>

</asp:Content>
