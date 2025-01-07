<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerPedido.aspx.cs" Inherits="LabSystemPP3.PantallaPedido.VerPedido" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="gestorpedidoventa.css" rel="stylesheet" type="text/css" />

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

    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="contenido-central">
        <div id="encabezado">
            <h1>Ver pedido</h1>
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
                                <div class="grid-container-pedido">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="GrillaProductoPedido" runat="server" CssClass="lista" AutoGenerateColumns="False" DataKeyNames="IdDetalle,IdProd,ID_STOCK"
                                    AllowPaging="True" Width="300px" PageSize="3" OnPageIndexChanging="GrillaProductoPedido_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="IdDetalle" HeaderText="IdDetalle" SortExpression="IdDetalle"></asp:BoundField>
                                        <asp:BoundField DataField="IdProd" HeaderText="IdProd" SortExpression="IdProd"></asp:BoundField>
                                        <asp:BoundField DataField="ID_STOCK" HeaderText="ID_STOCK" ReadOnly="True" InsertVisible="False" SortExpression="ID_STOCK"></asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                        <asp:BoundField DataField="precio" HeaderText="Precio" SortExpression="precio"></asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                                        <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

            <!-- Descripción del pedido -->
            <div id="descPedido">
                <!-- Más controles de la descripción aquí -->
                <div class="section">
                    <asp:UpdatePanel ID="updpLblResOpe" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblResultadoOperacion" runat="server" Text=""></asp:Label>
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

                </div>

            </div>
        </div>

    </div>

</asp:Content>

