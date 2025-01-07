<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoPedidos.aspx.cs" Inherits="LabSystemPP3.PantallaPedido.ListadoPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="pantallaPedido.css" rel="stylesheet" type="text/css" />




    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectCliente">
        <SelectParameters>
            <asp:Parameter DefaultValue="" Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Nombre" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlCliente" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectClienteALL">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlPedido" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectPedidoAll">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDetallePedido" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectDetallePedido"
        UpdateCommandType="StoredProcedure" UpdateCommand="SumarStock">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
        </SelectParameters>

        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idStock" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlPedidosDelCliente" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectPedidoIdClienteAll"
        UpdateCommandType="StoredProcedure" UpdateCommand="UpdateEstadoPed"
        DeleteCommandType="StoredProcedure" DeleteCommand="deletePedido">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idCli" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idEstado" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlBuscarPedidoId" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="buscarPedidoVentaPorId">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlEstadoPedido" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectEstadoPedido">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlBuscarPorEstadoDePedidos" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="BuscarPedidoPorEstado">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Estado" Type="String"></asp:Parameter>
        </SelectParameters>


    </asp:SqlDataSource>

    <div class="contenido-central">

        <div id="encabezado">
            <h1>pedidos de venta</h1>
        </div>
        <h3>Seleccione un cliente</h3>
        <asp:UpdatePanel ID="updBuscarCli" UpdateMode="Always" EnableViewState="true" runat="server">
            <ContentTemplate>
                <div class="search-bar">
                    <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
                    <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" EnableViewState="true" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GrillaClientes" CssClass="lista" runat="server" OnRowCommand="GrillaClientes_RowCommand"
                        AutoGenerateColumns="False" DataKeyNames="ID_CLIENTE,NOM_CLIENTE,CUIT_CLIENTE,RAZON_SOCIAL,DESC_COND_IVA,priv_pub" 
                        AllowPaging="true" OnPageIndexChanging="GrillaClientes_PageIndexChanging" PageSize="5">
                        <Columns>
                            <asp:BoundField DataField="ID_CLIENTE" ReadOnly="True" SortExpression="ID_CLIENTE" Visible="false"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="CUIT_CLIENTE" HeaderText="Cuit"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="NOM_CLIENTE" HeaderText="Nombre"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="RAZON_SOCIAL" HeaderText="Razon social"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="DESC_COND_IVA" HeaderText="IVA"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="listaI" DataField="priv_pub" HeaderText="Ambito"></asp:BoundField>

                            <asp:TemplateField HeaderText="Seleccionar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                        ToolTip="Seleccionar" CommandName="Select"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <div class="boton-agregar">
            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Button CssClass="add-button" ID="BtnNuevoPedido" runat="server" Text="Nuevo pedido" OnClick="BtnNuevoPedido_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div>
            <asp:UpdatePanel ID="UPPCli" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblId" runat="server" Visible="false" Text=""></asp:Label>
                    <asp:Label ID="lblNom" runat="server" Visible="false" Text=""></asp:Label>
                    <asp:Label ID="lblAmb" runat="server" Visible="false" Text=""></asp:Label>
                    <asp:Label ID="lblRs" runat="server" Visible="false" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <h3>Ventas del cliente seleccionado</h3>

        <!--*************************************************************************************************************************-->

        <asp:UpdatePanel ID="udpBtnBuscPedId" UpdateMode="Always" EnableViewState="true" runat="server">
            <ContentTemplate>
                <div class="search-bar">
                    <div class="search-bar">
                        <asp:Button CssClass="search-button" ID="BtnLimp" runat="server" Text="Limpiar busquedas" OnClick="BtnLimp_Click" />
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" DataTextField="DESCRIPCION_ESTADO_PEDIDO" DataValueField="ID_ESTADO_PEDIDO" DataSourceID="SqlEstadoPedido"></asp:DropDownList>
                        <asp:Button CssClass="search-button" ID="btnBuscarPorEstado" runat="server" Text="Buscar estado" OnClick="btnBuscarPorEstado_Click" />
                    </div>
                    <div class="search-bar">
                        <asp:TextBox CssClass="Inp" ID="tbBuscaPedido" runat="server" TextMode="Number" onkeypress="Buscarid(event)" placeholder="Ingrese el codigo del pedido"></asp:TextBox>
                        <asp:Button CssClass="search-button" ID="btnBuscarPedido" runat="server" Text="Buscar" OnClick="btnBuscarPedido_Click" />
                    </div>
                </div>

            </ContentTemplate>

        </asp:UpdatePanel>


        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" EnableViewState="true" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GrillaPedidos" CssClass="lista" runat="server" AutoGenerateColumns="False" OnRowCommand="GrillaPedidos_RowCommand" AllowPaging="True"
                        OnPageIndexChanging="GrillaPedidos_PageIndexChanging" OnRowCancelingEdit="GrillaPedidos_RowCancelingEdit"
                        OnRowDeleting="GrillaPedidos_RowDeleting" DataKeyNames="ID_CLIENTE,DESCRIPCION,ID_PEDIDO,DESCRIPCION_ESTADO_PEDIDO,FECHA_PEDIDO,NOM_CLIENTE,
                        RAZON_SOCIAL,priv_pub,Viatico,TOTAL">
                        <Columns>

                            <asp:BoundField DataField="ID_PEDIDO" HeaderText="ID" SortExpression="ID_PEDIDO"></asp:BoundField>
                            <asp:BoundField DataField="ID_CLIENTE" SortExpression="IdCliente" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="FECHA_PEDIDO" HeaderText="Fecha" SortExpression="Fecha"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>

                            <asp:BoundField DataField="NOM_CLIENTE" HeaderText="Cliente" SortExpression="Cliente"></asp:BoundField>
                            <asp:BoundField DataField="RAZON_SOCIAL" HeaderText="Razón social" SortExpression="Rs"></asp:BoundField>
                            <asp:BoundField DataField="priv_pub" HeaderText="Ambito" SortExpression="Ambito"></asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION_ESTADO_PEDIDO" HeaderText="Estado" SortExpression="Estado"></asp:BoundField>
                            <asp:BoundField DataField="Viatico" HeaderText="Viatico" SortExpression="Viatico"></asp:BoundField>
                            <asp:BoundField DataField="TOTAL" HeaderText="Total" SortExpression="Total"></asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="DESCRIPCION" Visible="false"></asp:BoundField>
                            <asp:TemplateField HeaderText="Confirmar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgConfirm" runat="server" ImageUrl="~/iconos/confirmar.png"
                                        CommandName="Confirmar" ToolTip="Confirmar"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cancelar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgCancelar" runat="server" ImageUrl="~/iconos/CancelarPV.png"
                                        CommandName="Cancel" ToolTip="Cancelar"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/iconos/borrarPV.png"
                                        CommandName="Delete" ToolTip="Eliminar"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Vender">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgVender" runat="server" ImageUrl="~/iconos/Venta.png"
                                        CommandName="Vender" ToolTip="Nueva venta"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Modificar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgModificar" runat="server" ImageUrl="~/iconos/editar.png"
                                        CommandName="Modificar" ToolTip="Modificar"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ver pedido">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgVer" runat="server" ImageUrl="~/iconos/Ver.png"
                                        CommandName="Ver" ToolTip="Ver pedido"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
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
        <script src="Pedidos.js"></script>
    </div>

</asp:Content>
