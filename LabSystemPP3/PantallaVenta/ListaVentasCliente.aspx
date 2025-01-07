<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListaVentasCliente.aspx.cs" Inherits="LabSystemPP3.PantallaVenta.ListaVentasCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="ventas.css" rel="stylesheet" type="text/css" />

    <!--scriptmanager y sqldatasource-->
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Label ID="lblIdCli" runat="server" Text=""></asp:Label>
    <asp:SqlDataSource
        ID="SqlVentas" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectVentasCliente">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:ControlParameter ControlID="lblIdCli" PropertyName="Text" Name="idCli" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlVentasTotales" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectVentasAll">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDeudas" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectVentasAdeudadasCliente"
        UpdateCommandType="StoredProcedure" UpdateCommand="ventaAdeudada">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idCli" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <!--Voy a usar este procedimiento para comprobar si la venta cuenta con algun equipo-->
    <asp:SqlDataSource ID="SqlOperacionesV" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        DeleteCommandType="StoredProcedure" DeleteCommand="DeleteVenta"
        UpdateCommandType="StoredProcedure" UpdateCommand="UpdateEstadoVenta"
        SelectCommandType="StoredProcedure" SelectCommand="SeVendioUnEquipo">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idVenta" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idVenta" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPedido" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idVenta" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idEstado" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlCliente" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectClienteALL">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlBuscarCliente" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectCliente">
        <SelectParameters>
            <asp:Parameter DefaultValue="" Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Nombre" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlPedidoServicio" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        InsertCommandType="StoredProcedure" InsertCommand="InsertPedidoServicio"
        SelectCommandType="StoredProcedure" SelectCommand="SelectEquipParaMantPreven"
        OnInserted="SqlPedidoServicio_Inserted">
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idCli" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idtipo" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="calle" Type="String"></asp:Parameter>
            <asp:Parameter Name="altura" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="titulo" Type="String"></asp:Parameter>
            <asp:Parameter Name="desc" Type="String"></asp:Parameter>
            <asp:Parameter Name="fechaPed" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="fechaSer" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="idVenta" Type="Int32"></asp:Parameter>
            <asp:Parameter Direction="Output" Name="IDPedido" Type="Int32"></asp:Parameter>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPedidoVent" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDetallePedidoServicio" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        InsertCommandType="StoredProcedure" InsertCommand="InsertDetallePedidoServicio">
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="IdPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantidad" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
            <asp:Parameter Name="codigo" Type="String"></asp:Parameter>
        </InsertParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlVentasId" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="BuscarVentaPorID">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idVen" Type="Int32"></asp:Parameter>
        </SelectParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlPedidosDelCliente" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>"
        ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        UpdateCommandType="StoredProcedure" UpdateCommand="UpdateEstadoPed"
        DeleteCommandType="StoredProcedure" DeleteCommand="deletePedido"
        SelectCommandType="StoredProcedure" SelectCommand="BuscarVentaPorIDPedido">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Idped" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idEstado" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <div class="contenido-central">

        <div id="encabezado">
            <h1>VENTAS REALIZADAS</h1>
            <h3>Seleccione un cliente</h3>
        </div>

        <asp:UpdatePanel ID="updBuscarCli" UpdateMode="Always" EnableViewState="true" runat="server">
            <ContentTemplate>
                <div class="search-bar">
                    <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
                    <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="grid-container-1">
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GrillaClientes" CssClass="lista" runat="server" OnSelectedIndexChanged="GrillaClientes_SelectedIndexChanged"
                        AutoGenerateColumns="False" DataKeyNames="ID_CLIENTE" AllowPaging="true" PageSize="4" OnPageIndexChanging="GrillaClientes_PageIndexChanging">
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
            <asp:UpdatePanel ID="UpdBtnVerDeudas" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnVerdeudas" CssClass="add-button" runat="server" Text="Ver deudas" OnClick="btnVerdeudas_Click" Enabled="false" Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:UpdatePanel ID="UpdIdCli" UpdateMode="Always" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblId" runat="server" Visible="false" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="udpBtnBuscPedId" UpdateMode="Always" EnableViewState="true" runat="server">
            <ContentTemplate>
                <div class="search-bar">
                    <asp:TextBox CssClass="Inp" ID="tbBuscaPedido" runat="server" TextMode="Number" onkeypress="Buscarid(event)" placeholder="Ingrese el codigo del pedido"></asp:TextBox>
                    <asp:Button CssClass="search-button" ID="btnBuscarPedido" runat="server" Text="Buscar" OnClick="btnBuscarPedido_Click" />
                </div>
                <div>
                    <asp:RadioButton ID="RbBuscIdVenta" runat="server" Text="Id venta" ForeColor="Black" Checked="true" GroupName="Busqueda" />
                    <asp:RadioButton ID="RbBuscIdPedido" runat="server" Text="Id pedido" ForeColor="Black" GroupName="Busqueda" />
                </div>
            </ContentTemplate>

        </asp:UpdatePanel>

        <h2>Ventas</h2>
        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:Label ID="lblIdPedido" runat="server" Visible="false" Text=""></asp:Label>
                    <asp:GridView ID="GrillaVentas" CssClass="lista" runat="server" AllowPaging="True" PageSize="7" AutoGenerateColumns="False"
                        OnPageIndexChanging="GrillaVentas_PageIndexChanging"
                        OnRowCommand="GrillaVentas_RowCommand" DataSourceID="SqlVentasTotales" DataKeyNames="ID_VENTA,ID_PEDIDO,ID_Pago,id_estado_ven,idEmpleado,
                        FECHA_VENTA,Fecha_Pago,Fecha_Confirmacion,Total">
                        <Columns>
                            <asp:BoundField DataField="ID_PEDIDO" HeaderText="Id de pedido" SortExpression="ID_PEDIDO" Visible="true"></asp:BoundField>
                            <asp:BoundField DataField="ID_Pago" HeaderText="ID_Pago" SortExpression="ID_Pago" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="id_estado_ven" HeaderText="id_estado_ven" SortExpression="id_estado_ven" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="ID_CLIENTE" HeaderText="ID_CLIENTE" SortExpression="ID_CLIENTE" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="ID_VENTA" HeaderText="Id de venta" ReadOnly="True" SortExpression="ID_VENTA"></asp:BoundField>
                            <asp:BoundField DataField="NOM_CLIENTE" HeaderText="Cliente" ReadOnly="True" SortExpression="NOM_CLIENTE"></asp:BoundField>
                            <asp:BoundField DataField="FECHA_VENTA" HeaderText="Fecha de venta" SortExpression="FECHA_VENTA"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="Fecha_Pago" HeaderText="Fecha a pagar" SortExpression="Fecha_Pago"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="Fecha_Confirmacion" HeaderText="Fecha del pago" SortExpression="Fecha_Confirmacion"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="DESC_MP" HeaderText="medio de pago" SortExpression="DESC_MP"></asp:BoundField>
                            <asp:BoundField DataField="desc_estado_ven" HeaderText="estado" SortExpression="desc_estado_ven"></asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total"></asp:BoundField>
                            <asp:BoundField DataField="idEmpleado" HeaderText="idEmpleado" Visible="false" SortExpression="idEmpleado"></asp:BoundField>
                            <asp:BoundField DataField="Registrado por" HeaderText="Registrado por" SortExpression="Registrado por"></asp:BoundField>


                            <asp:TemplateField HeaderText="Abonar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgAbonar" runat="server" ImageUrl="~/iconos/pagar.png"
                                        CommandName="Abonar" ToolTip="Abonar venta"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cancelar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgCancelar" runat="server" ImageUrl="~/iconos/CancelarPV.png"
                                        CommandName="Cancelar" ToolTip="Cancelar venta"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ver venta">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgVerVenta" runat="server" ImageUrl="~/iconos/Ver.png"
                                        CommandName="VerVenta" ToolTip="Ver datos de la venta"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Modificar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgModificar" runat="server" ImageUrl="~/iconos/editar.png"
                                        CommandName="Modificar" ToolTip="Modificar datos de la venta"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Borrar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBorrar" runat="server" ImageUrl="~/iconos/borrarPV.png"
                                        CommandName="Borrar" ToolTip="Borrar datos de la venta"
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>


        <div class="modal-container" id="modal-containerVerDeudas">
            <div class="modal-body-deudas" id="VentanaVerDeudas">
                <div class="modal-header">
                    <h3>Deudas del cliente</h3>
                </div>

                <div class="modal-content">
                    <div class="grid-container">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvDeuda" CssClass="lista" runat="server" AllowPaging="True" PageSize="7" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gvDeuda_PageIndexChanging"
                                    DataKeyNames="ID_VENTA,ID_PEDIDO,ID_Pago,id_estado_ven,ID_CLIENTE"
                                    Visible="false" Enabled="false">
                                    <Columns>
                                        <asp:BoundField DataField="ID_PEDIDO" HeaderText="ID_PEDIDO" SortExpression="ID_PEDIDO" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="ID_Pago" HeaderText="ID_Pago" SortExpression="ID_Pago" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="id_estado_ven" HeaderText="id_estado_ven" SortExpression="id_estado_ven" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="ID_CLIENTE" HeaderText="ID_CLIENTE" SortExpression="ID_CLIENTE" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="ID_VENTA" HeaderText="Id de venta" ReadOnly="True" SortExpression="ID_VENTA"></asp:BoundField>
                                        <asp:BoundField DataField="NOM_CLIENTE" HeaderText="Cliente" ReadOnly="True" SortExpression="NOM_CLIENTE"></asp:BoundField>
                                        <asp:BoundField DataField="FECHA_VENTA" HeaderText="Fecha de venta" SortExpression="FECHA_VENTA"
                                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="Fecha_Pago" HeaderText="Fecha a pagar" SortExpression="Fecha_Pago"
                                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="Fecha_Confirmacion" HeaderText="Fecha del pago" SortExpression="Fecha_Confirmacion"
                                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                        <asp:BoundField DataField="DESC_MP" HeaderText="medio de pago" SortExpression="DESC_MP"></asp:BoundField>
                                        <asp:BoundField DataField="desc_estado_ven" HeaderText="estado" SortExpression="desc_estado_ven"></asp:BoundField>
                                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total"></asp:BoundField>
                                        <asp:BoundField DataField="idEmpleado" HeaderText="idEmpleado" Visible="false" SortExpression="idEmpleado"></asp:BoundField>
                                        <asp:BoundField DataField="Registrado por" HeaderText="Registrado por" SortExpression="Registrado por"></asp:BoundField>

                                    </Columns>
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Button ID="btnCerrarDudas" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="btnCerrarDudas_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    </div>
    <script src="VentasJS.js"></script>
</asp:Content>
