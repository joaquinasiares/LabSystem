<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paginaOrdenCompra.aspx.cs" Inherits="LabSystemPP3.PantallaOrdendecompra.paginaOrdenCompra" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="ocPdf.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <asp:SqlDataSource ID="SqlOrdenDeCompra" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectOrdenDeCompra">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="IdOc" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDirecciones" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="DireccionesOrdComp">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="IdOc" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlProductos" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectDetalleOrdenComp">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="IdOc" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlEmpleado" runat="server" ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleadoCompra">
        <SelectParameters>
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:SqlDataSource>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Panel ID="PanelVolver" runat="server">
            <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" />
        </asp:Panel>
        <div class="contenido-central">
            <asp:Label ID="LblID" runat="server" Text="" Visible="false"></asp:Label>
            <div id="encabezadoPedido">
                <h1>Orden de Compra</h1>
                <asp:Label ID="Label7" runat="server" Text="Fecha"></asp:Label>
                <asp:Label ID="LblFecha" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="Label24" runat="server" Text="Numero de orden"></asp:Label>
                <asp:Label ID="lblNumOrden" runat="server" Text=""></asp:Label>
            </div>
            <div class="datosOC">
                <!-- Datos del proveedor -->
                <div class="provTecno">
                    <div class="tec">
                        <h2>Proveedor</h2>
                        <div>
                            <asp:Label ID="Label1" runat="server" Text="Empresa"></asp:Label>
                            <asp:Label ID="lblRs" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="Label2" runat="server" Text="Teléfono"></asp:Label>
                            <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="Label3" runat="server" Text="Dirección"></asp:Label>
                            <asp:Label ID="lblDireccion" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="Label4" runat="server" Text="Provincia"></asp:Label>
                            <asp:Label ID="lblProvinciaProveedor" runat="server" Text=""></asp:Label>
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="Label5" runat="server" Text="Ciudad"></asp:Label>
                            <asp:Label ID="lblCiudadProveedor" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="Label6" runat="server" Text="Código Postal"></asp:Label>
                            <asp:Label ID="lblCodPostal" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="tec">
                        <h2>Tecnodiagnóstica</h2>
                        <div>
                            <asp:Label ID="Label9" runat="server" Text="Teléfono"></asp:Label>
                            <asp:Label ID="lblTelefonoTecno" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="Label10" runat="server" Text="Dirección"></asp:Label>
                            <asp:Label ID="lblDireccionTecno" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="Label15" runat="server" Text="Provincia"></asp:Label>
                            <asp:Label ID="lblProvinciaTecno" runat="server" Text=""></asp:Label>
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="Label16" runat="server" Text="Ciudad"></asp:Label>
                            <asp:Label ID="lblCiudadTecno" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="Label17" runat="server" Text="Código Postal"></asp:Label>
                            <asp:Label ID="lblCodPostalTecno" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="entrega">
                    <h2>Dirección de Entrega</h2>
                    <div>
                        <asp:Label ID="Label8" runat="server" Text="Dirección"></asp:Label>
                        <asp:Label ID="lblDireccionEntrega" runat="server" Text=""></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="Label18" runat="server" Text="Provincia"></asp:Label>
                        <asp:Label ID="lblProvinciaEntrega" runat="server" Text=""></asp:Label>
                    </div>
                    <br />
                    <div>
                        <asp:Label ID="Label19" runat="server" Text="Ciudad"></asp:Label>
                        <asp:Label ID="lblCiudadEntrega" runat="server" Text=""></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="Label20" runat="server" Text="Código Postal"></asp:Label>
                        <asp:Label ID="lblCodPostalEntrega" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>

            <div class="medio">



                <div class="grillaPedido">
                    <h3>Pedido</h3>
                    <div class="grid-container">
                        <asp:UpdatePanel ID="UdpProdPedido" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="GrillaProductoPedido" CssClass="lista" runat="server" AutoGenerateColumns="False"
                                    DataKeyNames="TotalGeneral">
                                    <Columns>
                                        <asp:BoundField DataField="ID_DETALLE" HeaderText="ID_DETALLE" SortExpression="ID_DETALLE" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="IdProd" HeaderText="IdProd" SortExpression="IdProd" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="COD_PROD" HeaderText="COD_PROD" SortExpression="COD_PROD"></asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCION" SortExpression="DESCRIPCION"></asp:BoundField>
                                        <asp:BoundField DataField="PrecioUnitario" HeaderText="PrecioUnitario" SortExpression="PrecioUnitario"></asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD" SortExpression="CANTIDAD"></asp:BoundField>
                                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total"></asp:BoundField>
                                        <asp:BoundField DataField="TotalGeneral" HeaderText="TotalGeneral" ReadOnly="True" SortExpression="TotalGeneral" Visible="false"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="inferior">
                <div class="inferiorIzq">

                    <asp:UpdatePanel ID="udpLblTotalProd" UpdateMode="Always" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label12" CssClass="datosCli" runat="server" Text="Total: $ "></asp:Label>
                            <asp:Label ID="lblTotProd" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="udpPrecioViatico" UpdateMode="Always" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label13" CssClass="datosCli" runat="server" Text="Delivery: $ "></asp:Label>
                            <asp:Label ID="LblViatico" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <asp:UpdatePanel ID="UpdPrecioIva" UpdateMode="Always" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label21" CssClass="datosCli" runat="server" Text="Iva: $ "></asp:Label>
                            <asp:Label ID="lblIva" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <asp:UpdatePanel ID="udpLblTotal" UpdateMode="Always" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label14" CssClass="datosCli" runat="server" Text="Sub total: $ "></asp:Label>
                            <asp:Label ID="lblTotal" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel ID="PanelOriginal" runat="server">
                        <asp:Button ID="BtnImprimir" runat="server" Text="Imprimir" OnClick="BtnImprimir_Click" />
                    </asp:Panel>
                </div>
                <div class="inferiorDer">
                    <div>
                        <asp:Label ID="Label22" runat="server" Text="Autorizado por"></asp:Label>
                        <br />
                        <asp:Label ID="lblAutorizacion" runat="server" Text=""></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="Label23" runat="server" Text="Instrucciones"></asp:Label>
                        <br />
                        <asp:Label ID="lblInstrucciones" runat="server" Text=""></asp:Label>
                    </div>
                </div>

            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</body>
</html>
