<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerModificarVenta.aspx.cs" Inherits="LabSystemPP3.PantallaVenta.VerModificarVenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="ventas2.css" rel="stylesheet" type="text/css" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        <!--hago el update en este sql-->
        <asp:SqlDataSource ID="SqlPedido" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            SelectCommandType="StoredProcedure" SelectCommand="SelectPedidoID"
            UpdateCommandType="StoredProcedure" UpdateCommand="UpdateVenta">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idPedido" Type="Int32"></asp:Parameter>
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="ID_VENTA" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="Fecha_Pago" Type="DateTime"></asp:Parameter>
                <asp:Parameter Name="ID_Pago" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="id_estado_ven" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idEmp" Type="Int32"></asp:Parameter>
            </UpdateParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlProductos" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>" SelectCommandType="StoredProcedure" SelectCommand="SelectDetallePedido">
            <SelectParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlMediodPago" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>" SelectCommand="SELECT ID_PAGO, DESC_MP FROM MEDIODEPAGO"></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlFactura" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            InsertCommandType="StoredProcedure" InsertCommand="InsertFactura" SelectCommandType="StoredProcedure" SelectCommand="selectFacturasVenta"
            DeleteCommandType="StoredProcedure" DeleteCommand="DeleteFacturaVenta">
            <DeleteParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idVenta" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="idFactura" Type="Int32"></asp:Parameter>
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="id_venta" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="ID_CLIENTE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="url_ubicacion" Type="String"></asp:Parameter>
                <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
            </InsertParameters>
            <SelectParameters>
                <asp:Parameter Name="idVenta" Type="Int32"></asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlEmpleado" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleadoVenta">
            <SelectParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div class="encabezado">
        <h1>Registrar Venta</h1>
    </div>
    <div class="datos-venta">
        <!-- Detalles de la Venta -->
        <h3>Detalles de la Venta</h3>
        <asp:Label ID="lblIdVenta" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblEstadoVen" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblIdPed" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblIdCli" runat="server" Visible="false"></asp:Label>

         <br />
        <asp:Label ID="Label5" runat="server" Text="Fecha " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblFecha" runat="server" Text="Label" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Cliente " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblNom" runat="server" Text="Label" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Ambito " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblAmbito" runat="server" Text="Label" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Viatico " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblViatico" runat="server" Text="Label" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label6" runat="server" Text="Total " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblTotal" runat="server" Text="Label" CssClass="lblDetalle"></asp:Label>
        <br />
        <asp:Label ID="Label8" runat="server" Text="Descripcion: " CssClass="lblDetalle"></asp:Label>
        <asp:Label ID="lblDesc" runat="server" Text="Label" CssClass="lblDetalle"></asp:Label>
    <br />
    <div id="empleadoVenta">
        <h3>Empleado de venta</h3>
        <asp:DropDownList ID="ddlEmpleado" CssClass="dp" runat="server" DataTextField="empleado" DataValueField="ID_EMPLEADO_SERVICIO" DataSourceID="SqlEmpleado"></asp:DropDownList>
    </div>

    <!-- Productos -->
    <div class="grid-container">
        <h3>Productos</h3>
        <asp:GridView ID="GrillaProductos" CssClass="lista" runat="server" DataSourceID="SqlProductos" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="ID_DETALLE_PEDIDO,IdProd">
            <Columns>
                <asp:BoundField DataField="ID_DETALLE_PEDIDO" HeaderText="ID_DETALLE_PEDIDO" ReadOnly="True" InsertVisible="False" SortExpression="ID_DETALLE_PEDIDO" Visible="False"></asp:BoundField>
                <asp:BoundField DataField="IdProd" HeaderText="IdProd" SortExpression="IdProd" Visible="False"></asp:BoundField>
                <asp:BoundField DataField="ID_STOCK" HeaderText="ID_STOCK" ReadOnly="True" InsertVisible="False" SortExpression="ID_STOCK" Visible="False"></asp:BoundField>
                <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                <asp:BoundField DataField="precio" HeaderText="Precio" SortExpression="precio"></asp:BoundField>
                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                <asp:BoundField DataField="SubTotal" HeaderText="Subtotal" SortExpression="SubTotal"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>


    <!-- Medio de Pago -->
    <div id="mediosPago">
        <h3>Medio de Pago</h3>
        <asp:DropDownList ID="DropDownList1" CssClass="dp" runat="server" DataTextField="DESC_MP" DataValueField="ID_PAGO" DataSourceID="SqlMediodPago"></asp:DropDownList>
        <br />
        <h3>Agregar Facturas</h3>
        <asp:FileUpload ID="FileUpload1" CssClass="file-upload" runat="server" />
        <asp:Button ID="btnSubirFacturas" CssClass="btnAgregarFacturas" runat="server" Text="Agregar Facturas" OnClick="btnSubirFacturas_Click" />
    </div>
    <div class="grid-container-1">
        <h3>Facturas</h3>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="GrillaFacturas" CssClass="lista" runat="server" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="ID_FACTURA"
                    OnRowCommand="GrillaFacturas_RowCommand"
                    OnRowDeleting="GrillaFacturas_RowDeleting"
                    OnPageIndexChanging="GrillaFacturas_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="ID_FACTURA" HeaderText="" ReadOnly="True" SortExpression="" Visible="false"></asp:BoundField>
                        <asp:BoundField DataField="url_ubicacion" HeaderText="Url" ReadOnly="True" SortExpression="url_ubicacion"></asp:BoundField>
                        <asp:BoundField DataField="nombre" HeaderText="Nombre" ReadOnly="True" SortExpression="nombre"></asp:BoundField>
                        <asp:ButtonField CommandName="VerVenta" Text="Ver PDF" ButtonType="Button"></asp:ButtonField>
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Fecha de Pago -->
    <div class="contenido-inferior">
        <div class="datos-registro">
            <h3>Fecha de Pago</h3>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:CheckBox ID="CbFecha" CssClass="lbldatos" runat="server" Text="Agregar fecha" AutoPostBack="true" Checked="false" OnCheckedChanged="CbFecha_CheckedChanged" />
                    <asp:TextBox ID="TbFecha" CssClass="textbox-fecha" runat="server" TextMode="Date" Visible="false"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="boton-agregar">
                <asp:Button ID="btnUpdate" runat="server" Text="Modificar Venta" OnClick="btnVenta_Click" />
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
