using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Messaging;
using CapaEntidades;
using System.Reflection;
using static LabSystemPP3.Notificacion.Enum;
using System.Web.Services.Description;

namespace LabSystemPP3.PantallaPedido
{
    public partial class PedidoVentaModificar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            udpEstado("");
            LblResOpe("");
            if (!IsPostBack)
            {
                CargarDatos();
                cargarProductos();
                CrearDetalleProd();
                CrearTablaParaBorrar();
            }
        }

        private void CargarDatos()
        {
            try
            {
                lblIdPedido.Text = Request.QueryString["IdPedido"];
                lblNomCli.Text = Request.QueryString["Cliente"];
                lblAmbito.Text = Request.QueryString["Ambito"];
                lblIdCli.Text = Request.QueryString["IdCliente"];
                lblFecha.Text = Request.QueryString["Fecha"];
                lblRs.Text = Request.QueryString["Rs"];
                string viatico = Request.QueryString["Viatico"];
                lblTotal.Text = Request.QueryString["Total"];
                if (viatico.Equals("0"))
                {
                    CBviatico.Checked = false;
                    TbViatico.Text = "0";
                    TbViatico.Enabled = false;
                }
                else
                {
                    CBviatico.Checked = true;
                    TbViatico.Text = viatico;
                    TbViatico.Enabled = true;
                    LblViatico.Text = viatico;
                    Decimal ValorDelViatico = Decimal.Parse(viatico);
                    Decimal totalDeTodo = Decimal.Parse(lblTotal.Text);
                    Decimal resta = totalDeTodo - ValorDelViatico;
                    lblTotProd.Text = resta.ToString();
                }

                lblTotal.Text = Request.QueryString["Total"];
                tbDesc.Text = Request.QueryString["Descripcion"];
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los datos: " + ex.Message;
                MostrarAlerta(mensaje,NotificationType.error);
            }
        }

        private void MostrarAlerta(string mensaje, NotificationType tipo)
        {
            string script = $@"
                Swal.fire({{
                title: '{mensaje}',
                icon: '{tipo.ToString().ToLower()}',
                confirmButtonText: 'OK',
                confirmButtonColor: '#28a745'
                }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarAlerta", script, true);
        }


        private void udpEstado(string mensaje)
        {
            labelresultadoCalculos.Text = mensaje;
            updresultCalculos.Update();
        }

        private void LblResOpe(string mensaje)
        {
            lblResultadoOperacion.Text = mensaje;
            updpLblResOpe.Update();
        }

        //se cargan los datos de los productos para la venta en el session GrillaProductos 
        private void cargarProductos()
        {
            string ambito = "";
            if (lblAmbito.Text.Equals("Publico"))
            {
                ambito = "1";
            }
            else { ambito = "0"; }

            try
            {

                DataTable dtProd = new DataTable();
                dtProd.Columns.Add("ID_PRODUCTO");
                dtProd.Columns.Add("ID_PROV");
                dtProd.Columns.Add("NOMBRE_PROV");
                dtProd.Columns.Add("PRECIO_COMPRA");
                dtProd.Columns.Add("IdEstado");
                dtProd.Columns.Add("ID_TIPO");
                dtProd.Columns.Add("ID_STOCK");
                dtProd.Columns.Add("COD_PROD");
                dtProd.Columns.Add("DESCRIPCION");
                dtProd.Columns.Add("DESC");
                dtProd.Columns.Add("FECHA_INGRESO");
                dtProd.Columns.Add("FECHA_VTO");
                dtProd.Columns.Add("LOTE");
                dtProd.Columns.Add("DescEstado");
                dtProd.Columns.Add("CANTIDAD");
                dtProd.Columns.Add("CANTIDAD_MIN");
                dtProd.Columns.Add("precio");

                SqldsProductosYPedido.SelectParameters["ambito"].DefaultValue = ambito;
                this.SqldsProductosYPedido.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader reader;
                reader = (SqlDataReader)this.SqldsProductosYPedido.Select(DataSourceSelectArguments.Empty);

                while (reader.Read())
                {
                    DataRow row = dtProd.NewRow();
                    row["ID_PRODUCTO"] = reader["ID_PRODUCTO"];
                    row["ID_PROV"] = reader["ID_PROV"];
                    row["NOMBRE_PROV"] = reader["NOMBRE_PROV"];
                    row["PRECIO_COMPRA"] = reader["PRECIO_COMPRA"];
                    row["IdEstado"] = reader["IdEstado"];
                    row["ID_TIPO"] = reader["ID_TIPO"];
                    row["ID_STOCK"] = reader["ID_STOCK"];
                    row["COD_PROD"] = reader["COD_PROD"];
                    row["DESCRIPCION"] = reader["DESCRIPCION"];
                    row["DESC"] = reader["DESC"];
                    // Manejo de FECHA_INGRESO
                    if (reader["FECHA_INGRESO"] != DBNull.Value &&
                        DateTime.TryParse(reader["FECHA_INGRESO"].ToString(), out DateTime fechaIngreso))
                    {
                        row["FECHA_INGRESO"] = fechaIngreso.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        row["FECHA_INGRESO"] = string.Empty; // O el valor por defecto que desees
                    }

                    // Manejo de FECHA_VTO
                    if (reader["FECHA_VTO"] != DBNull.Value &&
                        DateTime.TryParse(reader["FECHA_VTO"].ToString(), out DateTime fechaVto))
                    {
                        row["FECHA_VTO"] = fechaVto.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        row["FECHA_VTO"] = string.Empty; // O el valor por defecto que desees
                    }
                    row["LOTE"] = reader["LOTE"];
                    row["DescEstado"] = reader["DescEstado"];
                    row["CANTIDAD"] = reader["CANTIDAD"];
                    row["CANTIDAD_MIN"] = reader["CANTIDAD_MIN"];
                    row["precio"] = reader["precio"];
                    dtProd.Rows.Add(row);
                }

                Session["GrillaProductos"] = dtProd;


                GrillaProductos.DataSource = dtProd;
                GrillaProductos.DataBind();
                UpdatePanel1.Update();
                lblNomCli.Text = Request.QueryString["Cliente"];
                string viatico = Request.QueryString["Viatico"];
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los productos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void CBviatico_CheckedChanged(object sender, EventArgs e)
        {
            if (CBviatico.Checked)
            {
                TbViatico.Enabled = true;
                btnSumar.Enabled = true;
                btnRestar.Enabled = true;
            }
            else
            {
                TbViatico.Enabled = false;
                btnSumar.Enabled = false;
                btnRestar.Enabled = false;
                Decimal Total = Decimal.Parse(lblTotal.Text) - Decimal.Parse(LblViatico.Text);
                lblTotal.Text = Total.ToString();
                LblViatico.Text = "0";
                TbViatico.Text = "0";
                CalcualarTotal();
            }
        }

        //se cargan los datos de los detalles del pedido del cliente en el session Pedidos
        private void CrearDetalleProd()
        {
            try
            {
                SqlDetalle.SelectParameters["ID"].DefaultValue = lblIdPedido.Text;
                SqlDetalle.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader reader;

                reader = (SqlDataReader)SqlDetalle.Select(DataSourceSelectArguments.Empty);
                DataTable dt = new DataTable();
                dt.Columns.Add("IdDetalle");
                dt.Columns.Add("IdProd");
                dt.Columns.Add("ID_STOCK");
                dt.Columns.Add("DESCRIPCION");
                dt.Columns.Add("precio");
                dt.Columns.Add("CANTIDAD");
                dt.Columns.Add("SubTotal");

                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["IdDetalle"] = reader["ID_DETALLE_PEDIDO"];
                    dr["IdProd"] = reader["IdProd"];
                    dr["ID_Stock"] = reader["ID_STOCK"];
                    dr["DESCRIPCION"] = reader["DESCRIPCION"];
                    dr["precio"] = reader["precio"];
                    dr["CANTIDAD"] = reader["CANTIDAD"];
                    dr["SubTotal"] = reader["SubTotal"];

                    dt.Rows.Add(dr);

                }
                Session["Pedidos"] = dt;


                GrillaProductoPedido.DataBind();
                GrillaProductoPedido.Columns[0].Visible = true;
                GrillaProductoPedido.Columns[1].Visible = true;
                GrillaProductoPedido.Columns[2].Visible = true;
                GrillaProductoPedido.DataSource = dt;
                GrillaProductoPedido.DataBind();
                GrillaProductoPedido.Columns[0].Visible = false;
                GrillaProductoPedido.Columns[1].Visible = false;
                GrillaProductoPedido.Columns[2].Visible = false;
                GrillaProductoPedido.AllowPaging = true;
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los productos a vender: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        //se crea un session que va a guardar los detalles que se van a eliminar cuando se modifique el pedido

        private void CrearTablaParaBorrar()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("IdDetalle");
                dt.Columns.Add("IdProd");
                dt.Columns.Add("ID_STOCK");
                dt.Columns.Add("CANTIDAD");

                Session["TablaParaBorrar"] = dt;
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        protected void GrillaProductoPedido_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrillaProductoPedido_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Recupera los DataTables desde la sesión
            DataTable dtPedidos = Session["Pedidos"] as DataTable;
            DataTable dtListadoProd = Session["GrillaProductos"] as DataTable;

            if (dtPedidos != null && dtListadoProd != null)
            {
                try
                {
                    // Obtén el IdProd del producto a eliminar desde los DataKeys
                    string idProductoEliminar = GrillaProductoPedido.DataKeys[e.RowIndex]["IdProd"].ToString();

                    // Busca la fila correspondiente en la tabla de pedidos
                    DataRow filaPedido = dtPedidos.AsEnumerable()
                        .FirstOrDefault(row => row["IdProd"].ToString() == idProductoEliminar);

                    if (filaPedido != null)
                    {
                        // Recupera la cantidad y subtotal del producto a eliminar
                        int cantidadDevolver = int.Parse(filaPedido["CANTIDAD"].ToString());
                        decimal subtotalEliminar = decimal.Parse(filaPedido["SubTotal"].ToString());

                        // Busca la fila correspondiente en la tabla de productos (dtListadoProd)
                        DataRow filaProducto = dtListadoProd.AsEnumerable()
                            .FirstOrDefault(row => row["ID_PRODUCTO"].ToString() == idProductoEliminar);

                        if (filaProducto != null)
                        {
                            // Actualiza la cantidad en la tabla de productos
                            int cantidadActual = int.Parse(filaProducto["CANTIDAD"].ToString());
                            filaProducto["CANTIDAD"] = cantidadActual + cantidadDevolver;

                            // Actualiza la GridView de productos (si aplica)
                            GrillaProductos.DataSource = dtListadoProd;
                            GrillaProductos.DataBind();
                        }

                        // Actualiza el total de la venta
                        decimal totalActual = decimal.Parse(lblTotProd.Text);
                        lblTotProd.Text = (totalActual - subtotalEliminar).ToString();

                        // Elimina la fila del DataTable de pedidos
                        dtPedidos.Rows.Remove(filaPedido);

                        // Vuelve a enlazar la tabla de pedidos con la GridView
                        GrillaProductoPedido.DataSource = dtPedidos;
                        GrillaProductoPedido.DataBind();

                        // Guarda los DataTables actualizados en la sesión
                        Session["Pedidos"] = dtPedidos;
                        Session["GrillaProductos"] = dtListadoProd;

                        // Actualiza los UpdatePanels
                        udpLblTotalProd.Update();
                        CalcualarTotal();
                        UpdatePanel1.Update();
                        UpdatePanel2.Update();
                    }
                    else
                    {
                        MostrarAlerta("No se encontró el producto a eliminar en la lista de pedidos.", NotificationType.error);
                    }
                }
                catch (Exception ex)
                {
                    // Maneja cualquier error durante el proceso
                    string mensaje = "Ocurrió un error al borrar el producto seleccionado: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                MostrarAlerta("No se encontraron las tablas necesarias en la sesión.", NotificationType.error);
            }
        }

        protected void GrillaProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                try
                {

                    DataTable dtp = (DataTable)Session["GrillaProductos"];
                    IndiceGrillaProducto.Text = index.ToString();
                    lblNomProd.Text = this.GrillaProductos.Rows[index].Cells[8].Text;
                    tbCantidad.Enabled = true;
                    Button1.Enabled = true;
                    UpdatePanel2.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al seleccionar el productos: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void BtnViatico_Click(object sender, EventArgs e)
        {
            try
            {
                if (!LblViatico.Text.Equals(TbViatico.Text))
                {
                    if (TbViatico.Text.Equals("0"))
                    {
                        Decimal Total = Decimal.Parse(lblTotal.Text) - Decimal.Parse(LblViatico.Text);
                        lblTotal.Text = Total.ToString();
                        LblViatico.Text = "0";
                    }
                    else
                    {
                        Decimal Total = Decimal.Parse(lblTotal.Text) + Decimal.Parse(TbViatico.Text);
                        lblTotal.Text = Total.ToString();
                        LblViatico.Text = TbViatico.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al seleccionar el viatico: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int indiceRelativo = int.Parse(IndiceGrillaProducto.Text);
                int indiceGlobal = (GrillaProductos.PageIndex * GrillaProductos.PageSize) + indiceRelativo;

                DataTable dtp = (DataTable)Session["GrillaProductos"];
                if (indiceGlobal < 0 || indiceGlobal >= dtp.Rows.Count)
                {
                    MostrarAlerta("El índice calculado está fuera de rango.", NotificationType.error);
                    return;
                }

                int totalProducto = int.Parse(dtp.Rows[indiceGlobal]["CANTIDAD"].ToString());
                int totalAPedir = int.Parse(tbCantidad.Text);

                if (totalAPedir > totalProducto || totalAPedir <= 0)
                {
                    MostrarAlerta("Cantidad inválida seleccionada.", NotificationType.warning);
                    return;
                }

                string id = GrillaProductos.DataKeys[indiceRelativo].Values["ID_PRODUCTO"].ToString();
                string precio = dtp.Rows[indiceGlobal]["precio"].ToString();
                string stock = GrillaProductos.DataKeys[indiceRelativo].Values["ID_STOCK"].ToString();
                string nombre = dtp.Rows[indiceGlobal]["DESCRIPCION"].ToString();

                DataTable dtPedidos = (DataTable)Session["Pedidos"];
                DataRow productoExistente = dtPedidos.AsEnumerable().FirstOrDefault(row => row["IdProd"].ToString() == id);

                if (productoExistente != null)
                {
                    int cantidadVieja = int.Parse(productoExistente["CANTIDAD"].ToString());
                    productoExistente["CANTIDAD"] = cantidadVieja + totalAPedir;
                    productoExistente["SubTotal"] = (decimal.Parse(precio) * (cantidadVieja + totalAPedir)).ToString();
                }
                else
                {
                    DataRow dr = dtPedidos.NewRow();
                    dr["IdDetalle"] = 0;
                    dr["IdProd"] = id;
                    dr["ID_STOCK"] = stock;
                    dr["DESCRIPCION"] = nombre;
                    dr["precio"] = decimal.Parse(precio);
                    dr["CANTIDAD"] = totalAPedir;
                    dr["SubTotal"] = (decimal.Parse(precio) * totalAPedir).ToString();

                    dtPedidos.Rows.Add(dr);
                }

                Session["Pedidos"] = dtPedidos;

                // Actualizar la cantidad disponible en el stock
                dtp.Rows[indiceGlobal]["CANTIDAD"] = (totalProducto - totalAPedir).ToString();

                // Actualizar GridViews
                GrillaProductos.DataSource = dtp;
                GrillaProductos.DataBind();

                GrillaProductoPedido.DataSource = dtPedidos;
                GrillaProductoPedido.DataBind();

                // Actualizar el total
                lblTotProd.Text = dtPedidos.AsEnumerable().Sum(row => decimal.Parse(row["SubTotal"].ToString())).ToString("N2");

                tbCantidad.Text = "0";
                lblNomProd.Text = string.Empty;
                tbCantidad.Enabled = false;
                Button1.Enabled = false;

                UpdatePanel1.Update();
                UpdatePanel2.Update();
                UpdatePanel3.Update();
                udpLblTotalProd.Update();
            }
            catch (Exception ex)
            {
                MostrarAlerta("Ocurrió un error al agregar el producto: " + ex.Message, NotificationType.error);
            }
        }


        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(tbCantidad.Text) > 0 || !tbCantidad.Enabled && int.Parse(tbCantidad.Text) == 0)
            {
                args.IsValid = true;
            }
            else { args.IsValid = false; }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {

                //se borran los detalles elminados
                DataTable tpb = (DataTable)Session["TablaParaBorrar"];
                for (int i = 0; i < tpb.Rows.Count; i++)
                {
                    SqlUpdateStockMas.UpdateParameters["idProd"].DefaultValue = tpb.Rows[i][1].ToString();
                    SqlUpdateStockMas.UpdateParameters["cantPed"].DefaultValue = tpb.Rows[i][3].ToString();
                    SqlUpdateStockMas.UpdateParameters["idStock"].DefaultValue = tpb.Rows[i][2].ToString();
                    SqlUpdateStockMas.Update();

                    SqlDetalle.DeleteParameters["ID"].DefaultValue = tpb.Rows[i][0].ToString();
                    SqlDetalle.Delete();
                }

                DataTable dtp = (DataTable)Session["GrillaProductos"];
                SqldsProductosYPedido.UpdateParameters["idPed"].DefaultValue = lblIdPedido.Text;
                SqldsProductosYPedido.UpdateParameters["desc"].DefaultValue = tbDesc.Text;
                SqldsProductosYPedido.UpdateParameters["total"].DefaultValue = lblTotal.Text;
                SqldsProductosYPedido.UpdateParameters["viatico"].DefaultValue = LblViatico.Text;
                int resultado = SqldsProductosYPedido.Update();



                    //se cargan los detalles nuevos
                    foreach (GridViewRow fila in GrillaProductoPedido.Rows)
                {
                    if (fila.Cells[0].Text.Equals("0"))
                    {
                        SqlDetalle.InsertParameters["ID_PEDIDO"].DefaultValue = lblIdPedido.Text;
                        SqlDetalle.InsertParameters["idProd"].DefaultValue = GrillaProductoPedido.DataKeys[fila.RowIndex].Values["IdProd"].ToString();
                        SqlDetalle.InsertParameters["nombre"].DefaultValue = fila.Cells[3].Text;
                        SqlDetalle.InsertParameters["precio"].DefaultValue = fila.Cells[4].Text;
                        SqlDetalle.InsertParameters["CANTIDAD"].DefaultValue = fila.Cells[5].Text;
                        SqlDetalle.InsertParameters["subTotal"].DefaultValue = fila.Cells[6].Text;
                        SqlDetalle.Insert();

                    }
                    else
                    {
                        SqlDetalle.UpdateParameters["idPedido"].DefaultValue = lblIdPedido.Text;
                        SqlDetalle.UpdateParameters["idDetalle"].DefaultValue = GrillaProductoPedido.DataKeys[fila.RowIndex].Values["IdDetalle"].ToString();
                        SqlDetalle.UpdateParameters["Cantidad"].DefaultValue = fila.Cells[5].Text;
                        SqlDetalle.UpdateParameters["subtotal"].DefaultValue = fila.Cells[6].Text;
                        SqlDetalle.Update();


                    }

                    for (int i = 0; i < dtp.Rows.Count; i++)
                    {
                        if (GrillaProductoPedido.DataKeys[fila.RowIndex].Values["IdProd"].ToString().Equals(dtp.Rows[i][0].ToString()))
                        {
                            SqlUpdateStock.UpdateParameters["idProd"].DefaultValue = GrillaProductoPedido.DataKeys[fila.RowIndex].Values["IdProd"].ToString();
                            SqlUpdateStock.UpdateParameters["cantPed"].DefaultValue = dtp.Rows[i][14].ToString();
                            SqlUpdateStock.UpdateParameters["idStock"].DefaultValue = GrillaProductoPedido.DataKeys[fila.RowIndex].Values["ID_STOCK"].ToString();
                            SqlUpdateStock.Update();
                            break;
                        }
                    }


                }
                if (resultado != 0)
                {
                    Session.Remove("Pedidos");
                    Session.Remove("GrillaProductos");
                    string mensaje = "Se modifico el pedido exitosamente";
                    Response.Redirect($"~/PantallaPedido/ListadoPedidos.aspx?mensaje={mensaje}");
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al modificar el pedido de venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void GrillaProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaProductos.PageIndex = e.NewPageIndex;
                DataTable dtp = (DataTable)Session["GrillaProductos"];
                GrillaProductos.DataSource = dtp;
                GrillaProductos.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar de página en la tabla productos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void GrillaProductoPedido_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaProductoPedido.PageIndex = e.NewPageIndex;

                // Recupera los datos de la sesión y los vuelve a enlazar
                DataTable dtPedidos = Session["Pedidos"] as DataTable;
                if (dtPedidos != null)
                {
                    GrillaProductoPedido.DataSource = dtPedidos;
                    GrillaProductoPedido.DataBind();
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar de página en la tabla productos seleccionados: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }



        protected void btnSumar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TbViatico.Text.Equals("0"))
                {

                    Decimal Total = Decimal.Parse(LblViatico.Text) + Decimal.Parse(TbViatico.Text);
                    LblViatico.Text = Total.ToString();
                    TbViatico.Text = "0";


                }
                udpPrecioViatico.Update();
                udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al sumar el precio: " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }



        protected void btnRestar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TbViatico.Text.Equals("") || !TbViatico.Text.Equals("0"))
                {
                    Decimal total = 0;
                    if (!LblViatico.Text.Equals("0"))
                    {
                        total = Decimal.Parse(LblViatico.Text);
                    }
                    Decimal precio = Decimal.Parse(TbViatico.Text);
                    if (precio <= total)
                    {
                        Decimal Total = Decimal.Parse(LblViatico.Text) - Decimal.Parse(TbViatico.Text);
                        LblViatico.Text = Total.ToString();
                        TbViatico.Text = "0";
                    }
                    else
                    {
                        string msg = "El precio a disminuir no puede ser mayor que el total";
                        MostrarAlerta(msg, NotificationType.error);
                    }
                }
                else
                {
                    string msg = "El precio del servicio esta vacio";
                    MostrarAlerta(msg, NotificationType.warning);
                }
                udpPrecioViatico.Update();
                udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al restar el precio" + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        private void CalcualarTotal()
        {
            try
            {
                Decimal insumos = Decimal.Parse(lblTotProd.Text);
                Decimal servicio = Decimal.Parse(LblViatico.Text);
                Decimal resultado = insumos + servicio;
                lblTotal.Text = resultado.ToString();
                udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al calcular el total del precio" + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }
        protected void btnCalcularTotal_Click(object sender, EventArgs e)
        {
            CalcualarTotal();
        }


    }
}