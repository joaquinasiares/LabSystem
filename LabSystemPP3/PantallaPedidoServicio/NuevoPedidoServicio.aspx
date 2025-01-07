<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevoPedidoServicio.aspx.cs" Inherits="LabSystemPP3.PantallaPedidoServivio.NuevoPedidoServicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <link href="nuevoPedidoServicio.css" rel="stylesheet" type="text/css" />
 
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
                InsertCommandType="StoredProcedure"
                InsertCommand="InsertPedidoServicio"
                OnInserted="SqlPedido_Inserted">
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
                    <asp:Parameter Name="IDPedido" Type="Int32" Direction="Output" />
                </InsertParameters>

            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDetallePedido" runat="server"
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


            <div ID="conteiner">
                <div class="encabezado">
                        <h1>Nuevo Pedido</h1>
                </div>
                    <div class="datosCliente">
                        <asp:Label ID="Label9" CssClass="datosCli1" runat="server" Text="Datos del cliente"></asp:Label><br />
                        <asp:Label CssClass="datosCli" ID="lblIdCli" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label CssClass="datosCli" ID="lblTipoServ" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label CssClass="datosCli1" ID="Label7" runat="server" Text="Fecha:  "></asp:Label>
                        <asp:Label CssClass="datosCli" ID="lblFecha" runat="server" Text=""></asp:Label>
                      
                        <asp:Label CssClass="datosCli1" ID="Label1" runat="server" Text="Nombre:  "></asp:Label>
                        <asp:Label CssClass="datosCli" ID="lblNomCli" runat="server" Text=""></asp:Label>
                       
                        <asp:Label CssClass="datosCli1" ID="Label2" runat="server" Text="Razon social:  "></asp:Label>
                        <asp:Label CssClass="datosCli" ID="lblRs" runat="server" Text=""></asp:Label>
                        <br />
                </div>
                <div ID="direccion">
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
                            AllowPaging="true" PageSize="5" DataKeyNames="ID_PRODUCTO" OnPageIndexChanging="GrillaProductos_PageIndexChanging"
                            OnSelectedIndexChanged="GrillaProductos_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="ID_PRODUCTO" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False"
                                    Visible="false" SortExpression="ID_PRODUCTO"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad"></asp:BoundField>
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
                            AllowPaging="true" PageSize="5" DataKeyNames="ID_PRODUCTO" OnRowDeleting="GrillaSeleccionados_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="ID_PRODUCTO" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False"
                                    Visible="false" SortExpression="ID_PRODUCTO"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="cantidad"></asp:BoundField>

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
                <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnAgregarP" runat="server" Text="Agregar pedido" OnClick="btnAgregarP_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </div>
            <!--la clase de css esta para que se mustre el label nomas-->
            <asp:UpdatePanel ID="updpEstado" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Label CssClass="datosCli" ID="lblEstado" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

</asp:Content>