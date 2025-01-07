<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevaVenta.aspx.cs" Inherits="LabSystemPP3.Venta.NuevaVenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="ventas2.css" rel="stylesheet" type="text/css" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        <asp:SqlDataSource ID="SqlProductos" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            SelectCommandType="StoredProcedure" SelectCommand="SelectDetallePedido">
            <SelectParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlMediodPago" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>" SelectCommand="SELECT ID_PAGO, DESC_MP FROM MEDIODEPAGO"></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlFactura" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            InsertCommandType="StoredProcedure" InsertCommand="InsertFactura">
            <InsertParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="id_venta" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="ID_CLIENTE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="url_ubicacion" Type="String"></asp:Parameter>
                <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
            </InsertParameters>
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

        <asp:SqlDataSource ID="SqlVenta" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>"
            ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            InsertCommandType="StoredProcedure"
            InsertCommand="InsertVenta">
            <InsertParameters>
                <asp:Parameter Name="ID_CLIENTE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="ID_PEDIDO" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="FECHA_VENTA" Type="DateTime"></asp:Parameter>
                <asp:Parameter Name="Fecha_Pago" Type="DateTime"></asp:Parameter>
                <asp:Parameter Name="fechaConfirmPago" Type="DateTime"></asp:Parameter>
                <asp:Parameter Name="ID_Pago" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="id_estado_ven" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="total" Type="Decimal"></asp:Parameter>
                <asp:Parameter Name="ID_VENTA" Type="Int32" Direction="Output"></asp:Parameter>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idemp" Type="Int32"></asp:Parameter>
            </InsertParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlEmpleado" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleadoVenta">
            <SelectParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlPedidosDelCliente" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>"
            ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            UpdateCommandType="StoredProcedure" UpdateCommand="UpdateEstadoPed">
            <UpdateParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idEstado" Type="Int32"></asp:Parameter>
            </UpdateParameters>
        </asp:SqlDataSource>

    </div>
    <div class="encabezado">
        <h1>Registrar venta</h1>
    </div>
    <div class="datos-venta">
        <h3>Detalles de la venta</h3>
        <asp:Label ID="lblIdPed" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblIdCli" runat="server" Visible="false"></asp:Label>

        <asp:Label ID="Label5" runat="server" Text="Fecha" CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblFecha" runat="server" Text="" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Cliente " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblNom" runat="server" Text="" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Ambito " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblAmbito" runat="server" Text="" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Viatico " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblViatico" runat="server" Text="" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label6" runat="server" Text="Total " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblTotal" runat="server" Text="" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label8" runat="server" Text="Descripcion" CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblDesc" runat="server" Text="" CssClass="lblDetalle"></asp:Label>
        <br />
        <div id="empleadoVenta">
            <h3>Empleado de venta</h3>
            <asp:DropDownList ID="ddlEmpleado" runat="server" DataTextField="empleado" CssClass="dp" DataValueField="ID_EMPLEADO_SERVICIO" DataSourceID="SqlEmpleado"></asp:DropDownList>
        </div>

        <div class="grid-container">
            <h3>Productos</h3>
            <asp:GridView ID="GrillaProductos" CssClass="lista" runat="server" DataSourceID="SqlProductos" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="ID_DETALLE_PEDIDO,IdProd,ID_TIPO">
                <Columns>
                    <asp:BoundField DataField="ID_DETALLE_PEDIDO" HeaderText="ID_DETALLE_PEDIDO" ReadOnly="True" InsertVisible="False" SortExpression="ID_DETALLE_PEDIDO" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="ID_TIPO" HeaderText="ID_TIPO" SortExpression="ID_TIPO" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="IdProd" HeaderText="IdProd" SortExpression="IdProd" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="ID_STOCK" HeaderText="ID_STOCK" ReadOnly="True" InsertVisible="False" SortExpression="ID_STOCK" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                    <asp:BoundField DataField="precio" HeaderText="Precio" SortExpression="precio"></asp:BoundField>
                    <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                    <asp:BoundField DataField="SubTotal" HeaderText="Subtotal" SortExpression="SubTotal"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>

        <div id="mediosPago">
            <h3>Medios de pago</h3>
            <asp:DropDownList ID="DropDownList1" CssClass="dp" runat="server" DataTextField="DESC_MP" DataValueField="ID_PAGO" DataSourceID="SqlMediodPago"></asp:DropDownList>
            <br />
            <br />
            <h3>Agregar facturas</h3>
            <asp:FileUpload ID="FileUpload1" CssClass="file-upload" runat="server" />
            <asp:Button ID="btnSubirFacturas" CssClass="btnAgregarFacturas" runat="server" Text="Agregar facturas" OnClick="btnSubirFacturas_Click" />
        </div>

        <div class="grid-container-1">
            <h3>Facturas</h3>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GrillaFacturas" runat="server" CssClass="lista" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Url"
                        OnRowCommand="GrillaFacturas_RowCommand"
                        OnRowDeleting="GrillaFacturas_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="Url" HeaderText="Url" ReadOnly="True" SortExpression="Url"></asp:BoundField>
                            <asp:BoundField DataField="NombreFactura" HeaderText="Nombre" ReadOnly="True" SortExpression="Nombre"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnView" Text="Ver" CommandName="Select" CommandArgument='<%# Eval("Ver") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="contenido-inferior">
            <div class="datos-registro">
                <h3>Fecha de pago</h3>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>

                        <asp:CheckBox ID="CbFecha" CssClass="lbldatos" runat="server" Text="Agregar fecha" AutoPostBack="true" Checked="false" OnCheckedChanged="CbFecha_CheckedChanged" />
                        <asp:TextBox ID="TbFecha" CssClass="textbox-fecha" runat="server" TextMode="Date" Visible="false"></asp:TextBox>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="boton-agregar">
                    <asp:Button ID="btnVenta" runat="server" Text="Registrar venta" OnClick="btnVenta_Click" />
                </div>
            </div>
            <div class="section">
                <asp:UpdatePanel ID="updpLblResOpe" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblResultadoOperacion" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>

    </div>

</asp:Content>
