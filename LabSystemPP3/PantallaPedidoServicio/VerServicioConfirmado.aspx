﻿<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerServicioConfirmado.aspx.cs" Inherits="LabSystemPP3.PantallaPedidoServicio.VerServicioConfirmado" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="nuevoPedidoServicio.css" rel="stylesheet" type="text/css" />
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:SqlDataSource ID="SqlProductosCliente" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="SelectDetallePedidoServicio">
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
                </SelectParameters>

            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlPedido" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>"
                ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="SelectPedidoServicioID"
                InsertCommandType="StoredProcedure" InsertCommand="InsertServicio"
                OnInserted="SqlPedido_Inserted"
                UpdateCommandType="StoredProcedure" UpdateCommand="UpdateEstadoServicio">
                <InsertParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idEmp" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="total" Type="Decimal"></asp:Parameter>
                    <asp:Parameter Name="idPedido" Type="Int32"></asp:Parameter>
                    <asp:Parameter Direction="Output" Name="IDServ" Type="Int32"></asp:Parameter>
                </InsertParameters>
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="IDPedido" Type="Int32"></asp:Parameter>
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idPed" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idEstado" Type="Int32"></asp:Parameter>
                </UpdateParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDetallePedido" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                InsertCommandType="StoredProcedure" InsertCommand="InsertServicioDetalle"
                SelectCommandType="StoredProcedure" SelectCommand="SelectDetallePedidoServicio">
                <InsertParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="IdServicio" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="precio" Type="Decimal"></asp:Parameter>
                    <asp:Parameter Name="subTotal" Type="Decimal"></asp:Parameter>
                    <asp:Parameter Name="cantidad" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
                </InsertParameters>

                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="ID" Type="Int32"></asp:Parameter>
                </SelectParameters>
            </asp:SqlDataSource>

            <!--voy a usar este sql para guardar los datos del servicio-->
            <asp:SqlDataSource ID="SqlEmpleado" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="SelectEmpleadoAll">
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlBuscarEmpleado" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="SelectVistaServicio">
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idPedido" Type="Int32"></asp:Parameter>
                </SelectParameters>

            </asp:SqlDataSource>

            <!--voy a usar este sql para guardar las rutas de las facturas-->

            <asp:SqlDataSource ID="SqlInsumos" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="SelectServicioDetalleConsInt"
                InsertCommandType="StoredProcedure" InsertCommand="InsertFacturaServicio"
                UpdateCommandType="StoredProcedure" UpdateCommand="RestarStock">
                <InsertParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="id_servicio" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="ID_CLIENTE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="url_ubicacion" Type="String"></asp:Parameter>
                    <asp:Parameter Name="nombre" Type="String"></asp:Parameter>
                </InsertParameters>

                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idServicio" Type="Int32"></asp:Parameter>
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idProd" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="cantPed" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="idStock" Type="Int32"></asp:Parameter>
                </UpdateParameters>
            </asp:SqlDataSource>


            <asp:SqlDataSource ID="SqlFactura" runat="server"
                ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
                SelectCommandType="StoredProcedure" SelectCommand="SelectFacturaServicio">
                <SelectParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                    <asp:Parameter Name="Idserv" Type="Int32"></asp:Parameter>
                    </SelectParameters>
            </asp:SqlDataSource>
            <div id="conteiner">
                <div class="encabezado">
                    <h1>Nuevo Producto</h1>
                </div>
            <div class="datosCliente">
                <asp:Label CssClass="datosCli" ID="lblIdservicio" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblIDPedido" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblIdCli" runat="server" Text="" Visible="false"></asp:Label>
                
                <asp:Label CssClass="datosCli" ID="lblTipoServ" runat="server" Text="" Visible="false"></asp:Label>
                 <asp:Label ID="Labeldatosclie" CssClass="datosCli1" runat="server" Text="Datos del cliente"></asp:Label><br />
                <asp:Label CssClass="datosCli1" ID="Label7" runat="server" Text="Fecha del pedido: "></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblFecha" runat="server" Text=""></asp:Label>
                
                <asp:Label CssClass="datosCli1" ID="Label1" runat="server" Text="Nombre: "></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblNomCli" runat="server" Text=""></asp:Label>
                
                <asp:Label CssClass="datosCli1" ID="Label2" runat="server" Text="Razon social: "></asp:Label>
                <asp:Label CssClass="datosCli" ID="lblRs" runat="server" Text=""></asp:Label>
                
            </div>

            <div id="direccion">
                <asp:Label CssClass="datosCli1" ID="Label3" runat="server" Text="Calle: "></asp:Label>
                <asp:TextBox ID="tbCalle" runat="server"></asp:TextBox>
               
                <asp:Label CssClass="datosCli1" ID="Label4" runat="server" Text="Altura: "></asp:Label>
                <asp:TextBox ID="tbAltura" TextMode="Number" runat="server"></asp:TextBox>
                
                <asp:Label CssClass="datosCli1" ID="Label5" runat="server" Text="Titulo: "></asp:Label>
                <asp:TextBox ID="tbTitulo" runat="server"></asp:TextBox>
                
                <asp:Label CssClass="datosCli1" ID="Label6" runat="server" Text="Descripcion: "></asp:Label>
                <asp:TextBox ID="tbDesc" runat="server" TextMode="MultiLine"></asp:TextBox>
               
            </div>
            <div class="grid-container">
                 <h3>Productos del cliente</h3>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GrillaProductos" CssClass="lista" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="5" DataKeyNames="ID_DETALLE_PEDIDO,IdProd">
                            <Columns>
                                <asp:BoundField DataField="ID_DETALLE_PEDIDO" HeaderText="ID_DETALLE_PEDIDO" ReadOnly="True" InsertVisible="False"
                                    SortExpression="ID_DETALLE_PEDIDO" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="IdProd" HeaderText="IdProd" SortExpression="IdProd" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="codigo" HeaderText="Codigo" SortExpression="codigo"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Producto" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                            </Columns>
                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div id="fechaServicio">
            <h3>Fecha del servicio</h3>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="TbFecha" runat="server" TextMode="Date"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>


            <div id="empleado">
            <h3>Empleado</h3>
            <asp:UpdatePanel ID="udpBuscEmp" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="search-bar">
                        <asp:TextBox CssClass="Inp" ID="txtBuscarEmp" runat="server" onkeypress="Buscar(event)" placeholder="Ingrese Nombre"></asp:TextBox>
                        <asp:Button CssClass="search-button" ID="btnBuscarEmp" runat="server" Text="Buscar" OnClick="btnBuscarEmp_Click" />

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
            <div class="grid-container">
                <asp:UpdatePanel ID="udpEmp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GrillaEmpleado" CssClass="lista" runat="server" OnSelectedIndexChanged="GrillaEmpleado_SelectedIndexChanged"
                            AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="5" OnPageIndexChanging="GrillaEmpleado_PageIndexChanging"
                            DataKeyNames="ID_EMPLEADO_SERVICIO,ID_CONTACTO">
                            <Columns>

                                <asp:BoundField DataField="ID_EMPLEADO_SERVICIO" HeaderText="IeEmpleado" ReadOnly="True" InsertVisible="False"
                                    SortExpression="ID_EMPLEADO_SERVICIO" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="DNI_EMP" HeaderText="Dni" SortExpression="DNI_EMP"></asp:BoundField>
                                <asp:BoundField DataField="NOMBRE_EMP" HeaderText="Nombre" SortExpression="NOMBRE_EMP"></asp:BoundField>
                                <asp:BoundField DataField="APELLIDO_EMP" HeaderText="Apellido" SortExpression="APELLIDO_EMP"></asp:BoundField>
                                <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono"></asp:BoundField>
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email"></asp:BoundField>
                                <asp:BoundField DataField="ID_CONTACTO" HeaderText="ID_CONTACTO" ReadOnly="True" InsertVisible="False" SortExpression="ID_CONTACTO"
                                    Visible="false"></asp:BoundField>
                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                            ToolTip="Seleccionar" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                            <RowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                        <asp:Label CssClass="datosCli" ID="lblEmpleadoSelect" runat="server" Text="Empleado seleccionado: "></asp:Label>
                        <asp:Label CssClass="datosCli" ID="lblNomEmp" runat="server" Text=""></asp:Label>
                        <asp:Label CssClass="datosCli" ID="lblApeEmp" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblIdEmp" runat="server" Text="" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div id="facturas">

            <h3>Facturas</h3>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <br />
            <asp:Button ID="btnSubirFacturas" runat="server" Text="Agregar facturas" OnClick="btnSubirFacturas_Click" />
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GrillaFacturas" runat="server" AutoGenerateColumns="False" AllowPaging="true" 
                        DataKeyNames="ID_FACTURA,url_ubicacion"
                        OnRowCommand="GrillaFacturas_RowCommand"
                        OnRowDeleting="GrillaFacturas_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ID_FACTURA" HeaderText="ID" ReadOnly="True" SortExpression="ID_FACTURA"
                                Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="url_ubicacion" HeaderText="Url" ReadOnly="True" SortExpression="url_ubicacion"></asp:BoundField>
                            <asp:BoundField DataField="nombre" HeaderText="Nombre" ReadOnly="True" SortExpression="Nombre"></asp:BoundField>
                            <asp:ButtonField CommandName="VerVenta" Text="Ver pdf" ButtonType="Button"></asp:ButtonField>
                            
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
          <div id="consumoInterno">
            <h3>Consumo interno</h3>
            <asp:UpdatePanel ID="udpCbConsuInt" runat="server">
                <ContentTemplate>
                    <asp:Label CssClass="datosCli" ID="Label9" runat="server" Text="Consumo interno"></asp:Label>
                    <asp:CheckBox ID="cbConsumoInt" runat="server" AutoPostBack="true" OnCheckedChanged="cbConsumoInt_CheckedChanged" />
                </ContentTemplate>
            </asp:UpdatePanel>

            <!--grilla de insumos para consumo interno-->




         <br />

            <asp:UpdatePanel ID="udpInsumos" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <!-- Contenedor de la grilla con scroll -->
                    <div class="grid-container">
                        <asp:GridView ID="GrillaInsumos" CssClass="lista" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="5" OnPageIndexChanging="GrillaInsumos_PageIndexChanging"
                            Visible="false" Enabled="false" OnSelectedIndexChanged="GrillaInsumos_SelectedIndexChanged"
                            DataKeyNames="ID_PRODUCTO,ID_TIPO,ID_STOCK">
                            <Columns>
                                <asp:BoundField DataField="ID_PRODUCTO" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False"
                                    SortExpression="ID_PRODUCTO" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="DESC" HeaderText="Tipo" SortExpression="DESC"></asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="COD_PROD" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="ID_TIPO" HeaderText="ID_TIPO" SortExpression="ID_TIPO"
                                    Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="ID_STOCK" HeaderText="ID_STOCK" ReadOnly="True" InsertVisible="False"
                                    Visible="false" SortExpression="ID_STOCK"></asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD"></asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD_MIN" HeaderText="Cantidad minima" SortExpression="CANTIDAD_MIN"></asp:BoundField>
                                <asp:BoundField DataField="precio" HeaderText="precio" ReadOnly="True" SortExpression="precio"></asp:BoundField>

                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelect" runat="server" ImageUrl="~/iconos/Seleccionar.png"
                                            ToolTip="Seleccionar" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                            <RowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="updAgregarIns" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:RadioButton ID="RbAmb2" runat="server" Text="Publico" ForeColor="Black" GroupName="Ambito"
                        Enabled="false" Visible="false" />
                    <br />
                    <asp:RadioButton ID="RbAmb1" runat="server" Text="Privado" ForeColor="Black" Checked="true" GroupName="Ambito"
                        Enabled="false" Visible="false" />
                    <br />
                    <asp:Label CssClass="datosCli" ID="Label8" runat="server" Text="Cantidad: "></asp:Label>
                    <asp:TextBox ID="numberCantidad" runat="server" TextMode="Number" Enabled="false" Visible="false" Text="1" min="1"></asp:TextBox>
                    <br />
                    <asp:Button ID="btnSelectIns" runat="server" Text="Agregar" Enabled="false" Visible="false" OnClick="btnSelectIns_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
          </div>
            <!--grilla de insumos Que fueron seleccionados consumo interno-->
            <div class="grid-container">
                <asp:UpdatePanel ID="udpInsSelect" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:GridView ID="GrillaSeleccionados" CssClass="lista" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="5" DataKeyNames="idProd" OnRowDeleting="GrillaSeleccionados_RowDeleting"
                            Enabled="false">
                            <Columns>
                                <asp:BoundField DataField="idProd" HeaderText="ID_PRODUCTO" ReadOnly="True" InsertVisible="False"
                                    Visible="false" SortExpression="ID_PRODUCTO"></asp:BoundField>
                                <asp:BoundField DataField="COD_PROD" HeaderText="Codigo" SortExpression="COD_PROD"></asp:BoundField>
                                <asp:BoundField DataField="nomProd" HeaderText="Nombre" SortExpression="DESCRIPCION"></asp:BoundField>
                                <asp:BoundField DataField="precio" HeaderText="Precio" SortExpression="Precio"></asp:BoundField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad"></asp:BoundField>
                                <asp:BoundField DataField="subTotal" HeaderText="Sub total" SortExpression="SubTotal"></asp:BoundField>

                                <asp:TemplateField HeaderText="Quitar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgdelete" runat="server" ImageUrl="~/iconos/borrarPV.png"
                                            ToolTip="Quitar" CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <SelectedRowStyle BackColor="#8ed3e8"></SelectedRowStyle>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div id="descPedido">
            <asp:UpdatePanel ID="udpPrecioServ" UpdateMode="Conditional" runat="server">
                <ContentTemplate>

                    <asp:Label ID="Label11" CssClass="datosCli" runat="server" Text="Servicio: $ "></asp:Label>
                    <asp:TextBox ID="TbPrecio" runat="server" onkeypress="return isNumberKey(event)" Text="0"></asp:TextBox>
                    <br />
                    <asp:Button ID="btnPrecioServicio" runat="server" Text="Sumar al precio" OnClick="btnPrecioServicio_Click" />
                    <asp:Button ID="btnRestar" runat="server" Text="Restar al precio" OnClick="btnRestar_Click" />
                    <br />
                    <asp:Label ID="Label13" CssClass="datosCli" runat="server" Text="Total servicio: $ "></asp:Label>
                    <asp:Label ID="lblPrecio" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="udpLblTotalIns" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Label ID="Label12" CssClass="datosCli" runat="server" Text="Total insumos: $ "></asp:Label>
                    <asp:Label ID="lblTotInsum" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="udpLblTotal" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Label ID="Label10" CssClass="datosCli" runat="server" Text="Total: $ "></asp:Label>
                    <asp:Label ID="lblTotal" CssClass="datosCli" runat="server" Text="0"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>


            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnCalcularTotal" runat="server" Text="Calculartotal" OnClick="btnCalcularTotal_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
            <div>
                <asp:Button ID="btnAgregarP" runat="server" Text="Registrar servicio"  OnClick="btnAgregarP_Click" />      
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
