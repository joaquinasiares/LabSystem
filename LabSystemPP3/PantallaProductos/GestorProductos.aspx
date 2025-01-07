<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestorProductos.aspx.cs" Inherits="LabSystemPP3.PantallaProductos.GestorProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="ProductoEstilos.css" />


    <!--sqldatasoruces-->

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <!--sqldatasoruce: mostrar, modificar y eliminar productos-->
    <asp:SqlDataSource ID="SqlProductos" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProductosALL"
        InsertCommandType="StoredProcedure" InsertCommand="InsertProducto"
        DeleteCommandType="StoredProcedure" DeleteCommand="deleteProductos"
        UpdateCommandType="StoredProcedure" UpdateCommand="updateProductos"
        OnInserted="SqlProductos_Inserted">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="id" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idTipo" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idEstado" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="codigo" Type="String"></asp:Parameter>
            <asp:Parameter Name="descripcion" Type="String"></asp:Parameter>
            <asp:Parameter Name="fechaIngreso" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="fechaVto" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="precioCompra" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="lote" Type="String"></asp:Parameter>
            <asp:Parameter Name="cantidad" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantidadMin" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="precioVentaPub" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="precioVentaPriv" Type="Decimal"></asp:Parameter>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>

        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="id" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="codigo" Type="String"></asp:Parameter>
            <asp:Parameter Name="descripcion" Type="String"></asp:Parameter>
            <asp:Parameter Name="precioCompra" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="lote" Type="String"></asp:Parameter>
            <asp:Parameter Name="cantidad" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantidadMin" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="preVentPub" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="preVentPriv" Type="Decimal"></asp:Parameter>
        </UpdateParameters>
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

    <asp:SqlDataSource ID="SqlBuscarProv" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProveedor"
        UpdateCommandType="StoredProcedure" UpdateCommand="UpdatePorcentajePrecioCompra">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProv" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="sumOResta" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="precio" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <!--sqldatasoruce: mostrar el tipo de productos y los proveedores (ventana de modificar producto) y de insert en la ventana modal -->
    <asp:SqlDataSource ID="SqlTipoProductos" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectTipoProducto">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlmodificarProductoProveedor" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProveedorConProducto">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlInsertarProductoProveedor" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProveedorALL">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>





    <div class="contenido-central">

        <div id="encabezado">
            <h1>Administrar productos</h1>
        </div>
        <!-- Barra de búsqueda -->
        <div class="search-bar">
            <asp:TextBox CssClass="Inp" ID="txtBuscar" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Descripción de Producto"></asp:TextBox>
            <asp:Button CssClass="search-button" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
        </div>

        <div>
            <asp:RadioButton ID="RbBuscDesc" runat="server" Text="Descripción" ForeColor="Black" Checked="true" GroupName="Busqueda" />
            <asp:RadioButton ID="RbBuscCod" runat="server" Text="Codigo" ForeColor="Black" GroupName="Busqueda" />
        </div>

        <div class="grid-container">
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="Grilla" CssClass="lista" runat="server" OnRowCommand="Grilla_RowCommand" AutoGenerateColumns="False"
                        DataKeyNames="ID_PRODUCTO,ID_PROV,IdEstado,ID_TIPO,PRECIO_COMPRA,PrecioVentaPub,PrecioVentaPriv"
                        AllowPaging="true" PageSize="5" OnPageIndexChanging="Grilla_PageIndexChanging"
                        OnRowDeleting="Grilla_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ID_PRODUCTO" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False" SortExpression="ID_PRODUCTO" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="ID_PROV" HeaderText="ID_PROV" ReadOnly="True" InsertVisible="False" SortExpression="ID_PROV" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="COD_PROD" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                            <asp:BoundField DataField="LOTE" HeaderText="Lote" InsertVisible="False" SortExpression="LOTE"></asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="DESCRIPCION"></asp:BoundField>
                            <asp:BoundField DataField="DESC" HeaderText="Producto" SortExpression="DESC"></asp:BoundField>
                            <asp:BoundField DataField="NOMBRE_PROV" HeaderText="Proveedor" SortExpression="NOMBRE_PROV"></asp:BoundField>
                            <asp:BoundField DataField="FECHA_INGRESO" DataFormatString="{0:dd-MM-yyyy}" InsertVisible="False" HeaderText="Ingreso" SortExpression="FECHA_INGRESO"></asp:BoundField>
                            <asp:BoundField DataField="FECHA_VTO" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Vencimiento" SortExpression="FECHA_VTO"></asp:BoundField>
                            <asp:BoundField DataField="DescEstado" HeaderText="Estado" Visible="false" SortExpression="DescEstado"></asp:BoundField>
                            <asp:BoundField DataField="PRECIO_COMPRA" HeaderText="Precio de Compra" SortExpression="PRECIO_COMPRA"></asp:BoundField>
                            <asp:BoundField DataField="PrecioVentaPub" HeaderText="Precio Clientes Publicos" SortExpression="PrecioVentaPub"></asp:BoundField>
                            <asp:BoundField DataField="PrecioVentaPriv" HeaderText="Precio Clientes Privados" SortExpression="PrecioVentaPriv"></asp:BoundField>
                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                            <asp:BoundField DataField="CANTIDAD_MIN" HeaderText="Cantidad minima" InsertVisible="False" SortExpression="CANTIDAD_MIN"></asp:BoundField>
                            <asp:BoundField DataField="IdEstado" HeaderText="IdEstado" SortExpression="IdEstado" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="ID_TIPO" HeaderText="ID_TIPO" SortExpression="ID_TIPO" Visible="false"></asp:BoundField>

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

        <div class="boton-agregar">
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <asp:Button CssClass="add-button" ID="BtnVentanaModalPrecio" runat="server" Text="Modificar Precios" OnClick="BtnVentanaModalPrecio_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:Button CssClass="add-button" ID="BtnVentanaModal" runat="server" Text="Agregar" OnClick="BtnVentanaModal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="section">
            <asp:UpdatePanel ID="updpLblResOpe" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblResultadoOperacion" runat="server" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <!--ventana modal-->

    <div class="modal-container" id="modal-container">
        <div class="modal-body" id="VentanaInsert">

            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                <ContentTemplate>
                    <asp:Button ID="BtnCerrarModalAgregar" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="BtnCerrarModal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="modal-header">
                <h3>Agregar producto</h3>
            </div>

            <div class="modal-content">

                <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblIdProd" runat="server" Visible="false"></asp:Label>
                        <asp:Label CssClass="modalitem" ID="Label1" runat="server" Text="Proveedor: "></asp:Label>
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel12" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:GridView CssClass="lista" ID="GrillaProveedoreInUp" runat="server" DataKeyNames="ID_PROV" AllowPaging="true" PageSize="2"
                                    OnPageIndexChanging="GrillaProveedoreInUp_PageIndexChanging" Width="450px"
                                    AutoGenerateColumns="false" OnSelectedIndexChanged="GrillaProveedoreInUp_SelectedIndexChanged">
                                    <Columns>
                                        <asp:BoundField DataField="ID_PROV" HeaderText="ID_PROV" ReadOnly="True" InsertVisible="False" SortExpression="ID_PROV" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_PROV" HeaderText="Proveedor" SortExpression="NOMBRE_PROV"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Seleccionar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgUpdate" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                                    ToolTip="Modificar" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="lblIDProvInUp" Visible="false" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />

                        <asp:Label CssClass="modalitem" ID="Label2" runat="server" Text="Tipo: "></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDLTipo" runat="server" OnSelectedIndexChanged="DDLTipo_SelectedIndexChanged"
                            DataTextField="DESC" DataValueField="ID_TIPO" DataSourceID="SqlTipoProductos" AutoPostBack="true">
                        </asp:DropDownList>

                        <br />

                        <asp:Label CssClass="modalitem" ID="Label6" runat="server" Text="Codigo"></asp:Label>
                        <br />
                        <asp:TextBox ID="TbCod" runat="server" ToolTip="Codigo"></asp:TextBox>
                        <asp:Label CssClass="modalitem" ID="Label13" runat="server" Text="*"></asp:Label>

                        <br />
                        <asp:Label CssClass="modalitem" ID="lblDescNom" runat="server" Text="Nombre: "></asp:Label>
                        <br />
                        <asp:TextBox ID="TbNom" runat="server" ToolTip="Nombre"></asp:TextBox>

                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label CssClass="modalitem" ID="LblVenc" runat="server" Text="Fecha de vencimiento"></asp:Label>
                        <br />
                        <asp:TextBox ID="TbFecha" runat="server" TextMode="Date"></asp:TextBox>
                        <asp:Label CssClass="modalitem" ID="LblfechaV" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label CssClass="modalitem" ID="Label8" runat="server" Text="Precio de compra"></asp:Label>
                        <br />
                        <asp:TextBox ID="TbPreciCom" runat="server" ToolTip="Precio" onkeypress="return isNumberKey(event)"></asp:TextBox>

                        <br />
                        <asp:Label CssClass="modalitem" ID="Label9" runat="server" Text="Lote"></asp:Label>
                        <br />
                        <asp:TextBox ID="TbLote" runat="server" ToolTip="Lote"></asp:TextBox>


                        <br />
                        <asp:Label CssClass="modalitem" ID="Label3" runat="server" Text="Cantidad"></asp:Label>
                        <br />
                        <asp:TextBox ID="TbCantidad" runat="server" ToolTip="Cantidad" onkeypress="return isNumberKeyPress(event)"></asp:TextBox>

                        <br />
                        <asp:Label CssClass="modalitem" ID="Label10" runat="server" Text="Cantidad minima"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbCantidadMin" runat="server" ToolTip="Cantidad minima" onkeypress="return isNumberKeyPress(event)"></asp:TextBox>

                        <br />
                        <asp:Label CssClass="modalitem" ID="Label4" runat="server" Text="Precio de venta publico" ToolTip="Precio venta"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbPreVentPub" runat="server" ToolTip="Precio venta publico" onkeypress="return isNumberKey(event)"></asp:TextBox>

                        <br />
                        <asp:Label CssClass="modalitem" ID="Label7" runat="server" Text="Precio venta Privado"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbPreVentPriv" runat="server" ToolTip="Precio venta privado" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>

                        <div class="section">
                            <asp:Button CssClass="modalitem Inp btn" ID="BtnInsert" runat="server" Text="Cargar" OnClick="BtnInsert_Click" />
                            <asp:Button CssClass="modalitem Inp btn" ID="BtnUpdate" runat="server" Text="Modificar" Visible="false" Enabled="false" OnClick="BtnUpdate_Click" />
                            <asp:UpdatePanel ID="ModalupdpLblResOpe" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label CssClass="modalitem" ID="ModallblResultadoOperacion" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:Label CssClass="modalitem" ID="LblResultado" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>

    <!--*************-->

    <div class="modal-precio-container" id="modal-containerPrecio">
        <div class="modal-precio-body" id="VentanaPrecio">
            <!-- Botón de cierre -->

            <asp:UpdatePanel ID="UpdatePanel13" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <asp:Button ID="BtnCerrarModalPrecio" CssClass="upd-cerrar" runat="server" Text="&times;" OnClick="BtnCerrarModal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>


            <!-- Encabezado del modal -->
            <div class="modal-header">
                <h3>Modificar porcentaje de precios</h3>
            </div>

            <!-- Contenido del modal con scrollbar -->
            <div class="modal-content">
                <!-- Búsqueda de proveedor -->
                <asp:UpdatePanel ID="UpdatePanel9" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <label class="modalitem" for="tbBuscarProv">Proveedor:</label>
                        <asp:TextBox class="search-input" ID="tbBuscarProv" runat="server" placeholder="Ingrese el nombre del proveedor"></asp:TextBox>

                        <asp:Button class="modalitem btn" ID="btnBuscarProv" runat="server" Text="Buscar" OnClick="btnBuscarProv_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>

                <!-- Grilla de proveedores -->
                <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView CssClass="lista" ID="GrillaProveedores" runat="server" AllowPaging="true" PageSize="2"
                            AutoGenerateColumns="false" OnPageIndexChanging="GrillaProveedores_PageIndexChanging"
                            OnRowCommand="GrillaProveedores_RowCommand" DataKeyNames="ID_PROV">
                            <Columns>
                                <asp:BoundField DataField="ID_PROV" HeaderText="ID_PROV" ReadOnly="True" InsertVisible="False" SortExpression="ID_PROV" Visible="true"></asp:BoundField>
                                <asp:BoundField DataField="NOMBRE_PROV" HeaderText="Proveedor"></asp:BoundField>
                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgselectProv" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                            ToolTip="Seleccionar" CommandName="Seleccionar" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                        </asp:GridView>
                        <asp:Label ID="lblProvID" runat="server" Text="" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <!-- Modificación del precio -->
                <label class="modalitem">Modificación:</label>
                <asp:RadioButton class="modalitem" ID="rbAumento" runat="server" Text="Aumento" GroupName="Precio" Checked="true" />
                <asp:RadioButton class="modalitem" ID="rbDescuento" runat="server" Text="Descuento" GroupName="Precio" />

                <!-- Ingreso de porcentaje -->
                <asp:UpdatePanel ID="UpdatePanel10" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <label class="modalitem" for="TbPorcentaje">%</label>
                        <asp:TextBox ID="TbPorcentaje" runat="server" TextMode="Number" class="percentage-input"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Ingrese un porcentaje válido" MinimumValue="1" MaximumValue="100" ControlToValidate="TbPorcentaje"></asp:RangeValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <!-- Botón para modificar porcentaje -->
                <asp:UpdatePanel ID="UpdatePanel11" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Button class="modalitem btn" ID="btnPorcentaje" runat="server" Text="Modificar porcentaje" OnClick="btnPorcentaje_Click" />
                        <asp:Label ID="lblPrecioResultado" runat="server" class="modalitem"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <script src="ProductoJS.js"></script>
</asp:Content>
