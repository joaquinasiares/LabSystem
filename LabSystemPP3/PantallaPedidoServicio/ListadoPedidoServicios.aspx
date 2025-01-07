<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoPedidoServicios.aspx.cs" Inherits="LabSystemPP3.PantallaPedidoServivio.ListadoPedidoServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="PedidoServicioCss.css" rel="stylesheet" type="text/css" />

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:SqlDataSource ID="SqlBuscarCliente" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectCliente">
        <SelectParameters>
            <asp:Parameter DefaultValue="" Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Nombre" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlPedido" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectPedidoServicioAll"
        UpdateCommandType="StoredProcedure" UpdateCommand="UpdateEstadoServicio"
        DeleteCommandType="StoredProcedure" DeleteCommand="deletePedidoServicio">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idEstado" Type="Int32"></asp:Parameter>
        </UpdateParameters>
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
        SelectCommandType="StoredProcedure" SelectCommand="SelectPedidoServicioClienteId">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idCli" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlCliente" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectClienteALL">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlBuscarPedidoId" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="BuscarPedidoServicioID">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="IDPedido" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>


    <div class="contenido-central">

        <div class="encabezado">
            <h1>pedidos de servicio</h1>
        </div>
        <h3>Seleccione un cliente: </h3>

        <asp:UpdatePanel ID="updBuscarCli" UpdateMode="Always" EnableViewState="true" runat="server">
            <ContentTemplate>
                <div class="search-bar">
                    <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese cliente"></asp:TextBox>
                    <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GrillaClientes" CssClass="lista" runat="server" AutoGenerateColumns="False"
                        AllowPaging="true" PageSize="5" DataKeyNames="ID_CLIENTE" OnSelectedIndexChanged="GrillaClientes_SelectedIndexChanged"
                        OnPageIndexChanging="GrillaClientes_PageIndexChanging" OnRowDeleting="GrillaClientes_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ID_CLIENTE" HeaderText="ID_CLIENTE" ReadOnly="True" Visible="false" InsertVisible="False" SortExpression="ID_CLIENTE"></asp:BoundField>
                            <asp:BoundField DataField="CUIT_CLIENTE" HeaderText="Cuit" SortExpression="CUIT_CLIENTE"></asp:BoundField>
                            <asp:BoundField DataField="NOM_CLIENTE" HeaderText="Nombre" SortExpression="NOM_CLIENTE"></asp:BoundField>
                            <asp:BoundField DataField="RAZON_SOCIAL" HeaderText="Razon social" SortExpression="RAZON_SOCIAL"></asp:BoundField>
                            <asp:BoundField DataField="priv_pub" HeaderText="Ambito" ReadOnly="True" SortExpression="priv_pub"></asp:BoundField>
                            <asp:BoundField DataField="DESC_COND_IVA" HeaderText="IVA" SortExpression="DESC_COND_IVA"></asp:BoundField>
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono"></asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email"></asp:BoundField>

                            <asp:TemplateField HeaderText="Seleccionar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                        ToolTip="Seleccionar" CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

        <div class="boton-agregar">
            <asp:UpdatePanel ID="udpNuevoServ" UpdateMode="Conditional" EnableViewState="true" runat="server">
                <ContentTemplate>
                    <asp:Button CssClass="add-button" ID="BtnNuevoPedido" runat="server" Text="Nuevo pedido"
                        OnClick="BtnNuevoPedido_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Label ID="lblPedido" runat="server" Text=""></asp:Label>

            <asp:UpdatePanel ID="updpLblErrorCliente" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Label CssClass="lblErrores" ID="lblErrorCliente" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="encabezado">
            <h1>Listado de pedidos</h1>
        </div>
        <div>
            <br />
            <asp:UpdatePanel ID="UPPCli" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblIdCli" runat="server" Visible="False" Text="0"></asp:Label>
                    <asp:Label ID="lblNom" runat="server" Visible="False" Text=""></asp:Label>
                    <asp:Label ID="lblRs" runat="server" Visible="False" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

                <asp:UpdatePanel ID="udpBtnBuscPedId" UpdateMode="Always" EnableViewState="true" runat="server">
            <ContentTemplate>
                <div class="search-bar">
                    <asp:TextBox CssClass="Inp" ID="tbBuscaPedido" runat="server" TextMode="Number" onkeypress="Buscarid(event)" placeholder="Ingrese el codigo del pedido"></asp:TextBox>
                    <asp:Button CssClass="search-button" ID="btnBuscarPedido" runat="server" Text="Buscar" OnClick="btnBuscarPedido_Click" />
                </div>
            </ContentTemplate>

        </asp:UpdatePanel>

        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" EnableViewState="true" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GrillaPedidos" CssClass="lista" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        PageSize="5" OnRowCommand="GrillaPedidos_RowCommand" OnPageIndexChanging="GrillaPedidos_PageIndexChanging"
                        DataKeyNames="ID_PEDIDO,ID_CLIENTE,ID_TIPO_PEDIDO,ID_ESTADO_PEDIDO,IdVenta">
                        <Columns>
                            <asp:BoundField DataField="ID_PEDIDO" HeaderText="ID_PEDIDO" ReadOnly="True" InsertVisible="False" SortExpression="ID_PEDIDO" Visible="true"></asp:BoundField>
                            <asp:BoundField DataField="ID_CLIENTE" HeaderText="ID_CLIENTE" ReadOnly="True" InsertVisible="False" SortExpression="ID_CLIENTE" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="ID_TIPO_PEDIDO" HeaderText="ID_TIPO_PEDIDO" SortExpression="ID_TIPO_PEDIDO" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="ID_ESTADO_PEDIDO" HeaderText="ID_ESTADO_PEDIDO" SortExpression="ID_ESTADO_PEDIDO" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="IdVenta" HeaderText="IdVenta" SortExpression="IdVenta" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="FECHA_PEDIDO" HeaderText="Fecha de solicitud" SortExpression="FECHA_PEDIDO" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="DESC_TIPO_PEDIDO" HeaderText="Tipo" SortExpression="DESC_TIPO_PEDIDO"></asp:BoundField>
                            <asp:BoundField DataField="NOM_CLIENTE" HeaderText="Cliente" SortExpression="NOM_CLIENTE"></asp:BoundField>
                            <asp:BoundField DataField="TITULO" HeaderText="Titulo" SortExpression="TITULO"></asp:BoundField>
                            <asp:BoundField DataField="FECHA_SERVICIO" HeaderText="Fecha a realizar" SortExpression="FECHA_SERVICIO" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION_ESTADO_PEDIDO" HeaderText="Estado" SortExpression="DESCRIPCION_ESTADO_PEDIDO"></asp:BoundField>


                            <asp:TemplateField HeaderText="Confirmar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgConfirmar" runat="server" ImageUrl="~/iconos/confirmar.png"
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
                                    <asp:ImageButton ID="imgeli" runat="server" ImageUrl="~/iconos/borrarPV.png"
                                        CommandName="del" ToolTip="Cancelar"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>



                            <asp:TemplateField HeaderText="Ver">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgVer" runat="server" ImageUrl="~/iconos/Ver.png"
                                        CommandName="Ver" ToolTip="Ver pedido"
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

                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>

    <script src="PedidoServicioJS.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>
