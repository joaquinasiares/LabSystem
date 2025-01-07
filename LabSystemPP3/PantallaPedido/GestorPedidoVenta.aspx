<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestorPedidoVenta.aspx.cs" Inherits="LabSystemPP3.PantallaPedido.GestorPedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="gestorpedidoventa.css" rel="stylesheet" type="text/css" />



    <asp:SqlDataSource ID="SqldsProductosYPedido" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProductosParaVenta" InsertCommandType="StoredProcedure"
        InsertCommand="InsertPedido"
        OnInserted="SqldsProductosYPedido_Inserted">
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID_CLIENTE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID_ESTADO_PEDIDO" Type="Int32"></asp:Parameter>
            <asp:Parameter DbType="Date" Name="FECHA_PEDIDO"></asp:Parameter>
            <asp:Parameter Name="DESCRIPCION" Type="String"></asp:Parameter>
            <asp:Parameter Name="total" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="Viatico" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="ID_PEDIDO" Direction="Output" Type="Int32"></asp:Parameter>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ambito" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDetallePedido" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        InsertCommandType="StoredProcedure" InsertCommand="insertDetallePedido" UpdateCommandType="StoredProcedure" UpdateCommand="RestarStock">
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID_PEDIDO" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="CANTIDAD" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="subTotal" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
            <asp:Parameter Name="precio" Type="Decimal"></asp:Parameter>
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idStock" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>


    <asp:SqlDataSource ID="SqlStock" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        DeleteCommandType="StoredProcedure" DeleteCommand="RestarStock">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idStock" Type="Int32"></asp:Parameter>
        </DeleteParameters>
    </asp:SqlDataSource>

    <!--sqldatasoruce: busca el producto por nombre o codigo-->
    <asp:SqlDataSource ID="SqlBuscarproductoNom" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProductos">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nom" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlBuscarproductoCod" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProductosCod">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cod" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>



    <asp:ScriptManager ID="ScriptManager1" runat="server" />



    <div class="contenido-central">
        <div id="encabezado">
            <h1>Nuevo Pedido</h1>
            <div class="datos-container">
                <asp:Label ID="lblFecha" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblIdCli" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="Label1" runat="server" Text="Cliente "></asp:Label>
                <asp:Label ID="lblNomCli" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label10" runat="server" Text="Razón social "></asp:Label>
                <asp:Label ID="lblRs" runat="server" Text=""></asp:Label>

                <!--<asp:Label ID="Label7" runat="server" Text="Ambito "></asp:Label>
                        <asp:Label ID="lblAmbito" runat="server" Text=""></asp:Label>

                        <asp:Label ID="Label6" runat="server" Text="Descripcion" Visible="false"></asp:Label>
                        <asp:TextBox ID="tbDesc" runat="server" Enabled="false" Visible="false"></asp:TextBox>-->
            </div>
        </div>

        <!-- Barra de búsqueda -->
        <asp:UpdatePanel ID="UdpBuscar" runat="server" UpdateMode="Always" EnableViewState="true">
            <ContentTemplate>
                <div class="search-bar">
                    <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Descripción de Producto"></asp:TextBox>
                    <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>

                <div>
                    <asp:RadioButton ID="RbBuscDesc" runat="server" Text="Descripción" ForeColor="Black" Checked="true" GroupName="Busqueda" />
                    <asp:RadioButton ID="RbBuscCod" runat="server" Text="Codigo" ForeColor="Black" GroupName="Busqueda" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <div class="contenido-flex">

            <!-- Grid de productos -->
            <div class="grid-container">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <!--El indice sirve para ubicar los productos en la grilla al cambiar de pagina-->
                        <asp:Label ID="IndiceGrillaProducto" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:GridView ID="GrillaProductos" CssClass="lista" runat="server" AutoGenerateColumns="False"
                            OnSelectedIndexChanged="GrillaProductos_SelectedIndexChanged" OnRowCommand="GrillaProductos_RowCommand"
                            AllowPaging="True" OnPageIndexChanging="GrillaProductos_PageIndexChanging" PageSize="6"
                            DataKeyNames="ID_PRODUCTO,ID_PROV,NOMBRE_PROV,PRECIO_COMPRA,IdEstado,ID_TIPO,ID_STOCK">
                            <Columns>
                                <asp:BoundField DataField="ID_PRODUCTO" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False" SortExpression="ID_PRODUCTO"
                                    Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="ID_PROV" HeaderText="ID_PROV" ReadOnly="True" InsertVisible="False" SortExpression="ID_PROV"
                                    Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="NOMBRE_PROV" HeaderText="NOMBRE_PROV" SortExpression="NOMBRE_PROV"
                                    Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="PRECIO_COMPRA" HeaderText="PRECIO_COMPRA" SortExpression="PRECIO_COMPRA"
                                    Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="IdEstado" HeaderText="IdEstado" SortExpression="IdEstado"
                                    Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="ID_TIPO" HeaderText="ID_TIPO" SortExpression="ID_TIPO"
                                    Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="ID_STOCK" HeaderText="ID_STOCK" ReadOnly="True" InsertVisible="False"
                                    SortExpression="ID_STOCK" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="DESC" HeaderText="Tipo" SortExpression="DESC"></asp:BoundField>
                                <asp:BoundField DataField="FECHA_INGRESO" HeaderText="Ingreso" SortExpression="FECHA_INGRESO"
                                    DataFormatString="{0:dd-MM-yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="FECHA_VTO" HeaderText="Vencimiento" SortExpression="FECHA_VTO"
                                    DataFormatString="{0:dd-MM-yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="LOTE" HeaderText="Lote" SortExpression="LOTE"></asp:BoundField>
                                <asp:BoundField DataField="DescEstado" HeaderText="Estado" SortExpression="DescEstado"></asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_MIN" HeaderText="Cantidad minima" SortExpression="CANTIDAD_MIN"></asp:BoundField>
                                <asp:BoundField DataField="precio" HeaderText="Precio" ReadOnly="True" SortExpression="precio"></asp:BoundField>

                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                            ToolTip="Seleccionar" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Descripción del pedido -->
            <div id="descPedido">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="Label5" runat="server" Text="Producto"></asp:Label>
                        <br />
                        <asp:Label ID="lblNomProd" runat="server" Text=""></asp:Label>
                        <br />
                        <div>
                            <asp:Label ID="LblPrecio" runat="server"></asp:Label>
                            <asp:Label ID="Label4" runat="server" Text="cantidad"></asp:Label>

                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Indique la cantidad de productos a vender" Text="*" ControlToValidate="tbCantidad" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                            <asp:TextBox ID="tbCantidad" runat="server" Text="0" TextMode="Number" Enabled="false"></asp:TextBox>
                        </div>

                        <br />
                        <asp:Button ID="Button1" runat="server" Text="Agregar producto" OnClick="Button1_Click" Enabled="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Más controles de la descripción aquí -->
                <div class="section">
                    <asp:UpdatePanel ID="updpLblResOpe" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblResultadoOperacion" runat="server" Text=""></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="grid-container-pedido">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="GrillaProductoPedido" CssClass="lista" runat="server" AutoGenerateColumns="False" DataKeyNames="ID,IDStock" OnRowDeleting="GrillaProductoPedido_RowDeleting"
                                    AllowPaging="True" Width="300px" OnPageIndexChanging="GrillaProductoPedido_PageIndexChanging" PageSize="3">
                                    <Columns>
                                        <asp:BoundField DataField="ID" ReadOnly="True" SortExpression="ID"></asp:BoundField>
                                        <asp:BoundField DataField="IDStock" ReadOnly="True" SortExpression="IDStock"></asp:BoundField>
                                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" ReadOnly="True" SortExpression="nomProv"></asp:BoundField>
                                        <asp:BoundField HeaderText="Precio" DataField="Precio" ReadOnly="True" SortExpression="Precio"></asp:BoundField>
                                        <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" ReadOnly="True" SortExpression="cantidad"></asp:BoundField>
                                        <asp:BoundField HeaderText="SubTotal" DataField="SubTotal" ReadOnly="True" SortExpression="SubTotal"></asp:BoundField>
                                        <asp:CommandField ShowDeleteButton="True" />
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <asp:UpdatePanel ID="udpCBViatico" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="display: flex;">
                                <asp:Label ID="Label11" CssClass="datosCli" runat="server" Text="Viatico"></asp:Label>
                                <asp:CheckBox ID="CBviatico" runat="server" Checked="false" Enabled="false" AutoPostBack="true" OnCheckedChanged="CBviatico_CheckedChanged" />

                            </div>
                            <asp:TextBox ID="TbViatico" runat="server" Text="0" Enabled="false" onkeypress="return isNumberKey(event)"></asp:TextBox>

                            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Coloque 0 si no se sumaran los viaticos" Text="*" ControlToValidate="TbViatico"
                                MinimumValue="0" MaximumValue="2147483647" Type="Integer"></asp:RangeValidator>
                            <br />

                            <asp:Button ID="btnSumar" runat="server" Enabled="false" Text="Sumar al precio" OnClick="btnSumar_Click" />
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="udpPrecioViatico" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label13" CssClass="datosCli" runat="server" Text="Total Viatico: $ "></asp:Label>
                            <asp:Label ID="LblViatico" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>



                    <asp:UpdatePanel ID="udpLblTotalProd" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label12" CssClass="datosCli" runat="server" Text="Total productos: $ "></asp:Label>
                            <asp:Label ID="lblTotProd" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="udpLblTotal" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label14" CssClass="datosCli" runat="server" Text="Total: $ "></asp:Label>
                            <asp:Label ID="lblTotal" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnCalcularTotal" runat="server" Enabled="false" Text="Calcular total" OnClick="btnCalcularTotal_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="udpInsertarPedido" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnInsertpedido" runat="server" Text="Cargar pedido" Enabled="false" OnClick="BtnInsertpedido_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <!-- Resultado de la operación -->

            <br />

            <div class="section">
                <asp:UpdatePanel ID="updresultCalculos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="labelresultadoCalculos" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
    </div>

    <script src="Pedidos.js"></script>
</asp:Content>
