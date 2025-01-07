<%@ Page Title="Modificar pedido" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="PedidoVentaModificar.aspx.cs" Inherits="LabSystemPP3.PantallaPedido.PedidoVentaModificar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="gestorpedidoventa.css" rel="stylesheet" type="text/css" />

    <asp:SqlDataSource ID="SqldsProductosYPedido" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectProductosParaVenta" UpdateCommandType="StoredProcedure" UpdateCommand="UpdatePedido">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ambito" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="desc" Type="String"></asp:Parameter>
            <asp:Parameter Name="total" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="viatico" Type="Decimal"></asp:Parameter>
        </UpdateParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDetalle" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectDetallePedido" DeleteCommandType="StoredProcedure" DeleteCommand="DeleteDetallePedido"
        InsertCommandType="StoredProcedure" InsertCommand="insertDetallePedido" UpdateCommandType="StoredProcedure" UpdateCommand="UpdateDetallePedido">
        <DeleteParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID_PEDIDO" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="CANTIDAD" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="subTotal" Type="Decimal"></asp:Parameter>
            <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
            <asp:Parameter Name="precio" Type="Decimal"></asp:Parameter>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idPedido" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idDetalle" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="Cantidad" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="subtotal" Type="Decimal"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlUpdateStock" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        UpdateCommandType="StoredProcedure" UpdateCommand="UpdateStock">
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idStock" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlUpdateStockMas" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        UpdateCommandType="StoredProcedure" UpdateCommand="SumarStock">
        <UpdateParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="cantPed" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="idStock" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="contenido-central">
        <div id="encabezado">
            <h1>Modificar pedido</h1>
            <div class="datos-container">
                <asp:Label ID="lblIdPedido" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="lblFecha" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblIdCli" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="Label1" runat="server" Text="Cliente "></asp:Label>
                <asp:Label ID="lblNomCli" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label10" runat="server" Text="Razón social "></asp:Label>
                <asp:Label ID="lblRs" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblAmbito" runat="server" Text="" Visible="false"></asp:Label>
                <asp:TextBox ID="tbDesc" runat="server" Visible="false"></asp:TextBox>
            </div>
        </div>

        <div class="contenido-flex">
            <!-- Grid de productos -->
            <div class="grid-container">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <!--El indice sirve para ubicar los productos en la grilla al cambiar de pagina-->
                        <asp:Label ID="IndiceGrillaProducto" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:GridView ID="GrillaProductos" CssClass="lista" runat="server" AutoGenerateColumns="False" OnRowCommand="GrillaProductos_RowCommand"
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
                        <h3>Detalle</h3>
                        <br />
                        <asp:Label ID="lblNomProd" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="LblPrecio" runat="server"></asp:Label><br />
                        <asp:Label ID="Label4" runat="server" Text="Cantidad"></asp:Label>
                        <br />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Indique la cantidad de productos a vender" Text="*" ControlToValidate="tbCantidad" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                        <asp:TextBox ID="tbCantidad" runat="server" Text="0" TextMode="Number" Enabled="false"></asp:TextBox>
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
                                <asp:GridView ID="GrillaProductoPedido" runat="server" CssClass="lista" AutoGenerateColumns="False" OnRowDeleting="GrillaProductoPedido_RowDeleting" DataKeyNames="IdDetalle,IdProd,ID_STOCK"
                                    OnSelectedIndexChanged="GrillaProductoPedido_SelectedIndexChanged" AllowPaging="True" Width="300px" PageSize="3" OnPageIndexChanging="GrillaProductoPedido_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="IdDetalle" HeaderText="IdDetalle" SortExpression="IdDetalle"></asp:BoundField>
                                        <asp:BoundField DataField="IdProd" HeaderText="IdProd" SortExpression="IdProd"></asp:BoundField>
                                        <asp:BoundField DataField="ID_STOCK" HeaderText="ID_STOCK" ReadOnly="True" InsertVisible="False" SortExpression="ID_STOCK"></asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                        <asp:BoundField DataField="precio" HeaderText="Precio" SortExpression="precio"></asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                                        <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal"></asp:BoundField>
                                        <asp:CommandField ShowDeleteButton="True" />
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:UpdatePanel ID="udpCBViatico" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="CBviatico" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="CBviatico_CheckedChanged" />
                            <asp:Label ID="Label11" CssClass="datosCli" runat="server" Text="Viaticos"></asp:Label>

                            <asp:TextBox ID="TbViatico" runat="server" Enabled="false" onkeypress="return isNumberKey(event)" Text="0"></asp:TextBox>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Coloque 0 si no se sumaran los viaticos" Text="*" ControlToValidate="TbViatico"
                                MinimumValue="0" MaximumValue="2147483647" Type="Integer"></asp:RangeValidator>
                            <br />
                            <asp:Button ID="btnSumar" runat="server" Text="Sumar al precio" OnClick="btnSumar_Click" />
                            <asp:Button ID="btnRestar" runat="server" Text="Restar al precio" OnClick="btnRestar_Click" />
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
                            <asp:Button ID="btnCalcularTotal" runat="server" Enabled="true" Text="Calcular total" OnClick="btnCalcularTotal_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                    <asp:Label CssClass="datosCli" ID="Label5" runat="server"></asp:Label>
                    <asp:Button ID="Button2" runat="server" Text="Modificar pedido" OnClick="Button2_Click" />


                    <!-- Resultado de la operación -->

                    <br />

                    <div class="section">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="label6" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" />
                <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
            </div>

            <div class="section">
                <asp:UpdatePanel ID="updresultCalculos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="labelresultadoCalculos" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>

</asp:Content>
