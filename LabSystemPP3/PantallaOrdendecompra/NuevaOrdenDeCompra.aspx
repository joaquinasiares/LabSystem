<%@ Page Title="Nueva orden de compra" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevaOrdenDeCompra.aspx.cs" Inherits="LabSystemPP3.PantallaOrdendecompra.NuevaOrdenDeCompra" Async="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="OrdenDeCompra.css" />
    <asp:SqlDataSource ID="SqlProductos" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProductosProveedor"
        InsertCommandType="StoredProcedure" InsertCommand="InsertCompra">
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idOrdComp" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="fechaSolicitud" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="idEstado" Type="Int32"></asp:Parameter>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>
    <!--sqldatasoruce: busca el producto por nombre o codigo-->
    <asp:SqlDataSource ID="SqlBuscarproductoNom" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProductosProveedorNom">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nom" Type="String"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlBuscarproductoCod" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProductosProveedorCod">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cod" Type="String"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlOrdenDeCompra" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        InsertCommandType="StoredProcedure" InsertCommand="InsertOrdenDeCompra">
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
            <asp:Parameter DbType="Date" Name="fecha"></asp:Parameter>
            <asp:Parameter Name="total" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="deliveri" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="iva" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="precioIva" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="autorizacion" Type="String"></asp:Parameter>
            <asp:Parameter Name="instruccion" Type="String"></asp:Parameter>
            <asp:Parameter Direction="Output" Name="IDOrd" Type="Int32"></asp:Parameter>
        </InsertParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="Sqldireccion" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        InsertCommandType="StoredProcedure" InsertCommand="InsertDireccionesDeOrdenDeCompra">
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idOrdeCompra" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="telefono" Type="String"></asp:Parameter>
            <asp:Parameter Name="direccion" Type="String"></asp:Parameter>
            <asp:Parameter Name="provincia" Type="String"></asp:Parameter>
            <asp:Parameter Name="ciudad" Type="String"></asp:Parameter>
            <asp:Parameter Name="codigoPostal" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="tipo" Type="Int32"></asp:Parameter>
        </InsertParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDetalleOrdCom" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        InsertCommandType="StoredProcedure" InsertCommand="InsertDetalleOrdenDeCompra">
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID_ORD_COMP" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID_PROV" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="IdProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="CANTIDAD" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="PrecioUnitario" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="Total" Type="Decimal"></asp:Parameter>
        </InsertParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlEmpleado" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleadoCompra">
            <SelectParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>

    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="contenido-central">
        <div id="encabezadoPedido">
            <h1>Nueva orden de compra</h1>

        </div>
        <div class="datosOC">
            <!--datos del proveedor-->
            <div class="provTecno">
                <div class="tec">
                    <h2>PROVEEDOR</h2>
                    <div>
                        <asp:Label ID="Label1" CssClass="lbldatos" runat="server" Text="Empresa"></asp:Label>
                        <asp:Label ID="lblRs" runat="server" Text=""></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="Label2" CssClass="lbldatos" runat="server" Text="Teléfono"></asp:Label>
                        <asp:TextBox ID="TbTelefonoProv" TextMode="Number" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Label ID="Label3" runat="server" CssClass="lbldatos" Text="Dirección"></asp:Label>
                        <asp:TextBox ID="TbDireccionProv" CssClass="inpDireccion" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ErrorMessage="Ingrese la direccion del proveedor" Text="*" ControlToValidate="TbDireccionProv"></asp:RequiredFieldValidator>
                    </div>
                    <asp:UpdatePanel ID="udpProvinciaProv" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div>
                                <asp:Label ID="Label4" CssClass="lbldatos" runat="server" Text="Provincia"></asp:Label>
                                <asp:DropDownList ID="DropDownListProvinciasProveedor" CssClass="dp" OnSelectedIndexChanged="DropDownListProvincias_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <br />
                            <div>
                                <asp:Label ID="Label5" CssClass="lbldatos" runat="server" Text="Ciudad"></asp:Label>
                                <asp:DropDownList ID="DropDownListDepartamentosProveedor" CssClass="dp" runat="server"></asp:DropDownList>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DropDownListProvinciasProveedor" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <div>
                        <asp:Label ID="Label6" CssClass="lbldatos" runat="server" Text="Código postal"></asp:Label>
                        <asp:TextBox ID="tbCodPostalProv" TextMode="Number" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ErrorMessage="Ingrese el codigo posta del proveedor" Text="*" ControlToValidate="tbCodPostalProv"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="tec">
                    <h2>TECNODIAGNOSTICA</h2>
                    <div>
                        <asp:Label ID="Label9" runat="server" CssClass="lbldatos" Text="Teléfono"></asp:Label>
                        <asp:TextBox ID="TbTelefonoTecnoDiag" TextMode="Number" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Label ID="Label10" runat="server" CssClass="lbldatos" Text="Dirección"></asp:Label>
                        <asp:TextBox ID="TbDireccionTecnoDiag" CssClass="inpDireccion" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                            ErrorMessage="Ingrese la direccion de Tecnodiagnostica" Text="*" ControlToValidate="TbDireccionTecnoDiag"></asp:RequiredFieldValidator>
                    </div>
                    <asp:UpdatePanel ID="udpProvinciaTecnoDiag" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div>
                                <asp:Label ID="Label15" runat="server" CssClass="lbldatos" Text="Provincia"></asp:Label>
                                <asp:DropDownList ID="DropDownListProvincasTecnoDiag" CssClass="dp" OnSelectedIndexChanged="DropDownListProvincasTecnoDiag_SelectedIndexChanged" runat="server"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <br />
                            <div>
                                <asp:Label ID="Label16" runat="server" CssClass="lbldatos" Text="Ciudad"></asp:Label>
                                <asp:DropDownList ID="DropDownListDepartamentosTecnoDiag" CssClass="dp" runat="server"></asp:DropDownList>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div>
                        <asp:Label ID="Label17" runat="server" CssClass="lbldatos" Text="Código postal"></asp:Label>
                        <asp:TextBox ID="tbCodPostalTecnoDiag" TextMode="Number" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                            ErrorMessage="Ingrese el codigo postal de Tecnodiagnostica" Text="*" ControlToValidate="tbCodPostalTecnoDiag"></asp:RequiredFieldValidator>
                    </div>
                    <asp:UpdatePanel ID="UpdCookeDatos" EnableViewState="true" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="GuardarDatos" runat="server" Text="Guardar datos" OnClick="GuardarDatos_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
            <div class="entrega">
                <h2>Direccion de entrega</h2>
                <div>
                    <asp:Label ID="Label8" runat="server" CssClass="lbldatos" Text="Direccion"></asp:Label>
                    <asp:TextBox ID="TbDireccionEntrega" CssClass="inpDireccion" runat="server"></asp:TextBox>
                </div>
                <asp:UpdatePanel ID="UdpEntrega" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>

                        <div>
                            <asp:Label ID="Label18" runat="server" CssClass="lbldatos" Text="Provincia"></asp:Label>
                            <asp:DropDownList ID="DropDownListEntrgaProvincia" CssClass="dp" OnSelectedIndexChanged="DropDownListEntrgaProvincia_SelectedIndexChanged" runat="server"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="Label19" runat="server" CssClass="lbldatos" Text="Ciudad"></asp:Label>
                            <asp:DropDownList ID="DropDownListEntrgaCiudad" CssClass="dp" runat="server"></asp:DropDownList>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div>
                    <asp:Label ID="Label20" runat="server" CssClass="lbldatos" Text="Código postal"></asp:Label>
                    <asp:TextBox ID="tbCodPostalEntrega" TextMode="Number" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="medio">

            <!-- Panel de actualización de la grilla Productos-->
            <asp:UpdatePanel ID="UpdatePanelProductos" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <!-- Barra de búsqueda -->
                    <div class="search-bar">
                        <div>
                            <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Descripción de Producto"></asp:TextBox>
                            <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        </div>

                        <div class="radibutton">
                            <asp:RadioButton ID="RbBuscDesc" runat="server" Text="Descripción" ForeColor="Black" Checked="true" GroupName="Busqueda" />
                            <asp:RadioButton ID="RbBuscCod" runat="server" Text="Codigo" ForeColor="Black" GroupName="Busqueda" />
                        </div>
                    </div>

                    <br />
                    <!-- Contenedor de la grilla con scroll -->
                    <div class="grid-container">
                        <asp:Label ID="IndiceGrillaProducto" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:GridView ID="GrillaProductos" CssClass="lista" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" OnPageIndexChanging="GrillaProducto_PageIndexChanging" PageSize="5"
                            OnRowCommand="GrillaProducto_RowCommand"
                            DataKeyNames="ID_PRODUCTO">
                            <Columns>
                                <asp:BoundField DataField="ID_PRODUCTO" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False" SortExpression="ID_PRODUCTO"
                                    Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="PRECIO_COMPRA" HeaderText="Precio de compra" SortExpression="PRECIO_COMPRA"></asp:BoundField>
                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                            ToolTip="Seleccionar proveedor" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


            <br />
            <div id="cantProd">
                <h3>Cantidad</h3>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblIdProd" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblProdNom" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />

                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="TbCantidad" CssClass="Inp" runat="server" TextMode="Number" Enabled="false" Width="100px">0</asp:TextBox>
                        <asp:Button ID="BtnSelectCant" CssClass="search-button" runat="server" Text="Seleccionar" OnClick="BtnSelectCant_Click" Enabled="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>



        <div class="grillaPedido">
            <h3>Pedido</h3>
            <div class="grid-container">
                <asp:UpdatePanel ID="UdpProdPedido" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GrillaProductoPedido" CssClass="lista" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                            DataKeyNames="ID_PRODUCTO" PageSize="3" OnPageIndexChanging="GrillaProductoPedido_PageIndexChanging" OnRowDeleting="GrillaProductoPedido_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="ID_PRODUCTO" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False"
                                    SortExpression="ID_PRODUCTO" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="PRECIO_COMPRA" HeaderText="Precio de compra" SortExpression="PRECIO_COMPRA"></asp:BoundField>
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad"></asp:BoundField>
                                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total"></asp:BoundField>
                                <asp:CommandField ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="inferior">
            <div class="inferiorIzq">
                <asp:UpdatePanel ID="udpCBViatico" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="display: flex;">
                            <asp:Label ID="Label11" CssClass="datosCli" runat="server" Text="Delivery"></asp:Label>
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
                <asp:UpdatePanel ID="UdpIva" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Label ID="Label7" runat="server" Text="Iva"></asp:Label>
                            <asp:CheckBox ID="cbIva" AutoPostBack="true" Enabled="false" runat="server" OnCheckedChanged="cbIva_CheckedChanged" />

                        </div>
                        <asp:TextBox ID="TbIva" TextMode="Number" Text="0" Enabled="false" runat="server"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnIva" runat="server" Enabled="false" Text="Agregar iva" OnClick="btnIva_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="udpLblTotalProd" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label12" CssClass="datosCli" runat="server" Text="Total: $ "></asp:Label>
                        <asp:Label ID="lblTotProd" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="udpPrecioViatico" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label13" CssClass="datosCli" runat="server" Text="Delivery: $ "></asp:Label>
                        <asp:Label ID="LblViatico" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <asp:UpdatePanel ID="UpdPrecioIva" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label21" CssClass="datosCli" runat="server" Text="Iva: $ "></asp:Label>
                        <asp:Label ID="lblIva" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <asp:UpdatePanel ID="udpLblTotal" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label14" CssClass="datosCli" runat="server" Text="Sub total: $ "></asp:Label>
                        <asp:Label ID="lblTotal" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnCalcularTotal" runat="server" Enabled="false" Text="Calcular total" OnClick="btnCalcularTotal_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdBtnOrdenCom" UpdateMode="Always" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="BtnOrdenCom" runat="server" CssClass="genOrden" Text="Generar orden " OnClick="BtnOrdenCom_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </div>
            <div class="inferiorDer">
                <div>
                    <asp:Label ID="Label22" runat="server" Text="Autorizado por"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddlAutorizacion" runat="server" DataTextField="empleado" DataValueField="ID_EMPLEADO_SERVICIO" DataSourceID="SqlEmpleado"></asp:DropDownList>
                </div>
                <div id="txtMulti">
                    <asp:Label ID="Label23" runat="server" Text="Instrucciones"></asp:Label>
                    <br />
                    <asp:TextBox ID="tbInstrucciones" TextMode="MultiLine" Rows="5" runat="server" CssClass="txtM"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <script src="OrdenDeCompraScripts.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>
