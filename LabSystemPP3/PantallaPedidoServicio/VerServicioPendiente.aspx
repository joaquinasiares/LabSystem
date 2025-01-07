<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerServicioPendiente.aspx.cs" Inherits="LabSystemPP3.PantallaPedidoServicio.VerServicioPendiente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="nuevoPedidoServicio.css" rel="stylesheet" type="text/css" />
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <asp:SqlDataSource ID="SqlProductosCliente" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="selectEquiposCliente">
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idCli" Type="Int32"></asp:Parameter>
                </SelectParameters>

            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlPedido" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>"
                ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="SelectPedidoServicioID"
                UpdateCommandType="StoredProcedure" UpdateCommand="UpdatePedidoServicio">
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="IDPedido" Type="Int32"></asp:Parameter>
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idPedido" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="calle" Type="String"></asp:Parameter>
                    <asp:Parameter Name="altura" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="titulo" Type="String"></asp:Parameter>
                    <asp:Parameter Name="desc" Type="String"></asp:Parameter>
                    <asp:Parameter Name="fechaSer" Type="DateTime"></asp:Parameter>
                </UpdateParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlPedidodetalle" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>"
                ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="SelectDetallePedidoServicio"
                DeleteCommandType="StoredProcedure" DeleteCommand="DeleteDetallePedidoServicio"
                InsertCommandType="StoredProcedure" InsertCommand="InsertDetallePedidoServicio"
                UpdateCommandType="StoredProcedure" UpdateCommand="UpdateDetallePedidoServicio">
                <InsertParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="IdPed" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="cantidad" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
                    <asp:Parameter Name="codigo" Type="String"></asp:Parameter>
                </InsertParameters>
                <DeleteParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idDetalle" Type="Int32"></asp:Parameter>
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idPedido" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idDetalle" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="Cantidad" Type="Int32"></asp:Parameter>
                </UpdateParameters>
            </asp:SqlDataSource>

           <div ID="conteiner">
               <div class="encabezado">
                    <h1><asp:Label CssClass="datosCli" ID="lblTitulo" runat="server" Text=""></asp:Label></h1>
                </div>
            <div class="datosCliente">
               
                <asp:Label CssClass="datosCli" ID="lblVerMod" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblIDPedido" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label CssClass="datosCli" ID="Label9" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblIdCli" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblTipoServ" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="Labeldatosclie" CssClass="datosCli1" runat="server" Text="Datos del cliente"></asp:Label><br />
                <asp:Label CssClass="datosCli" ID="Label7" runat="server" Text="Fecha: "></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblFecha" runat="server" Text=""></asp:Label>
                
                <asp:Label CssClass="datosCli" ID="Label1" runat="server" Text="Nombre: "></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblNomCli" runat="server" Text=""></asp:Label>
               
                <asp:Label CssClass="datosCli" ID="Label2" runat="server" Text="Razon social: "></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblRs" runat="server" Text=""></asp:Label>
                
            </div>

            <div ID="direccion">>
                <asp:Label CssClass="datosCli" ID="Label3" runat="server" Text="Calle: "></asp:Label>
                <asp:TextBox ID="tbCalle" runat="server"></asp:TextBox>
               
                <asp:Label CssClass="datosCli" ID="Label4" runat="server" Text="Altura: "></asp:Label>
                <asp:TextBox ID="tbAltura" TextMode="Number" runat="server"></asp:TextBox>
               
                <asp:Label CssClass="datosCli" ID="Label5" runat="server" Text="Titulo: "></asp:Label>
                <asp:TextBox ID="tbTitulo" runat="server"></asp:TextBox>
               
                <asp:Label CssClass="datosCli" ID="Label6" runat="server" Text="Descripcion: "></asp:Label>
                <asp:TextBox ID="tbDesc" runat="server" TextMode="MultiLine"></asp:TextBox>
               
            </div>
            <div class="grid-container">
                <h3>Productos del cliente</h3>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GrillaProductos" CssClass="lista" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="3" DataKeyNames="ID_PRODUCTO" OnPageIndexChanging="GrillaProductos_PageIndexChanging"
                            OnSelectedIndexChanged="GrillaProductos_SelectedIndexChanged" DataSourceID="SqlProductosCliente">
                            <Columns>
                                <asp:BoundField DataField="ID_PRODUCTO" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False" 
                                    SortExpression="ID_PRODUCTO" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad"></asp:BoundField>
                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                            ToolTip="Seleccionar" CommandName="Select"  CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div id="ProductosServicio">
            <h3>Productos para servicio</h3>
                <asp:UpdatePanel ID="updAgregarEq" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label CssClass="datosCli" ID="Label8" runat="server" Text="Cantidad: "></asp:Label>
                        <asp:TextBox ID="numberCantidad" runat="server" TextMode="Number" Enabled="false" Text="1" min="1"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnSelectEqui" runat="server" Text="Agregar" Enabled="false" OnClick="btnSelectEqui_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div class="grid-container">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:GridView ID="GrillaSeleccionados" CssClass="lista" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="2" DataKeyNames="IdProd" OnPageIndexChanging="GrillaSeleccionados_PageIndexChanging"
                            OnRowDeleting="GrillaSeleccionados_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="ID_DETALLE_PEDIDO" HeaderText="ID_DETALLE_PEDIDO" ReadOnly="True" InsertVisible="False"
                                    SortExpression="ID_DETALLE_PEDIDO" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="IdProd" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False"
                                    Visible="false" SortExpression="ID_PRODUCTO"></asp:BoundField>
                                <asp:BoundField DataField="codigo" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="cantidad"></asp:BoundField>

                                <asp:TemplateField HeaderText="Quitar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgdelete" runat="server" ImageUrl="~/iconos/borrarPV.png"
                                            ToolTip="Quitar" CommandName="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div id="panel2">
                <h3>Fecha del servicio</h3>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="TbFecha" runat="server" TextMode="Date"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Button ID="btnAgregarP" runat="server" Text="Modificar pedido" OnClick="btnAgregarP_Click" />
            </div>
            <!--la clase de css esta para que se mustre el label nomas-->
            <asp:UpdatePanel ID="updpEstado" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Label CssClass="datosCli" ID="lblEstado" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
               </div>
        </div>
</asp:Content>