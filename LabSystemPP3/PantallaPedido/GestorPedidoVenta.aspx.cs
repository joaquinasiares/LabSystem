using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static LabSystemPP3.Notificacion.Enum;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace LabSystemPP3.PantallaPedido
{
    public partial class GestorPedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            udpEstado("");
            LblResOpe("");
            if (!IsPostBack)
            {
                lblNomCli.Text = Request.QueryString["NOM_CLIENTE"];
                lblAmbito.Text = Request.QueryString["priv_pub"];
                lblIdCli.Text = Request.QueryString["ID_CLIENTE"];
                lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblRs.Text = Request.QueryString["Rs"];
                cargarProductos();
                CrearDtProd();
                Session["BusquedaProdVent"] = false;
                Session["ViaticoPedido"] = 0;
            }
        }

        private void cargarProductos()
        {
            try
            {
                string ambito = "";
                if (lblAmbito.Text.Equals("Publico"))
                {
                    ambito = "1";
                }
                else { ambito = "0"; }

                SqldsProductosYPedido.SelectParameters["ambito"].DefaultValue = ambito;
                GrillaProductos.DataSource = SqldsProductosYPedido;
                GrillaProductos.DataBind();
                UpdatePanel1.Update();

                DataView dv = (DataView)SqldsProductosYPedido.Select(DataSourceSelectArguments.Empty);

                Session["ListaProductos"] = dv.ToTable();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los productos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        //metod para mostrar alertas generales
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


        private void LblResOpe(string mensaje)
        {
            lblResultadoOperacion.Text = mensaje;
            updpLblResOpe.Update();
        }



        protected void GrillaProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                try
                {

                    DataTable dt = (DataTable)Session["ListaProductos"];
                    IndiceGrillaProducto.Text = index.ToString();
                    lblNomProd.Text = this.GrillaProductos.Rows[index].Cells[8].Text;
                    bool buscar = (bool)Session["BusquedaProdVent"];
                    if (buscar)
                    {
                        Session["IdProdBusc"] = GrillaProductos.DataKeys[index].Values["ID_PRODUCTO"].ToString();
                    }
                    else { Session["IdProdBusc"] = 0; }
                    //LblPrecio.Text = this.GrillaProductos.DataKeys[this.GrillaProductos.SelectedIndex].Values["ID_PRODUCTO"].ToString();
                    //LblPrecio.Text = this.GrillaProductos.Rows[this.GrillaProductos.SelectedIndex].Cells[16].Text;
                    tbCantidad.Enabled = true;
                    Button1.Enabled = true;
                    UpdatePanel2.Update();
                    CBviatico.Enabled = true;
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al seleccionar el productos: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void GrillaProductos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void CBviatico_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (CBviatico.Checked)
                {
                    TbViatico.Enabled = true;
                    btnSumar.Enabled = true;
                    // btnRestar.Enabled = true;
                }
                else
                {
                    TbViatico.Enabled = false;
                    btnSumar.Enabled = false;
                    //btnRestar.Enabled = false;
                    Decimal Total = Decimal.Parse(lblTotal.Text) - Decimal.Parse(LblViatico.Text);
                    lblTotal.Text = Total.ToString();
                    LblViatico.Text = "0";
                    TbViatico.Text = "0";
                    udpPrecioViatico.Update();
                    CalcualarTotal();
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al seleccionar el viatico: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        /*
        protected void CBviatico_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al seleccionar el viatico: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }*/


        private void CrearDtProd()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("IDStock");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Precio");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("SubTotal");

            GrillaProductoPedido.Columns[0].Visible = true;
            GrillaProductoPedido.Columns[1].Visible = true;
            GrillaProductoPedido.DataSource = dt;
            GrillaProductoPedido.DataBind();
            GrillaProductoPedido.Columns[0].Visible = false;
            GrillaProductoPedido.Columns[1].Visible = false;
            Session["Pedidos"] = dt;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int indiceRelativo = int.Parse(IndiceGrillaProducto.Text); // Índice relativo en la página actual
                int indiceGlobal = (GrillaProductos.PageIndex * GrillaProductos.PageSize) + indiceRelativo; // Índice global en el DataTable
                bool buscar = (bool)Session["BusquedaProdVent"];
                DataTable dtListadoProd;
                if (buscar)
                {
                    dtListadoProd = (DataTable)Session["ResutadoBusquedaProd"];
                }
                else { dtListadoProd = (DataTable)Session["ListaProductos"]; }
                
                int totalProducto = int.Parse(dtListadoProd.Rows[indiceGlobal]["CANTIDAD"].ToString());
                int totalAPedir = int.Parse(tbCantidad.Text);

                if (!lblNomProd.Text.Equals("") && !tbCantidad.Text.Equals("0") && totalAPedir <= totalProducto && totalAPedir > 0)
                {
                    bool nuevoProducto = true;
                    Decimal total = 0;
                    string id = GrillaProductos.DataKeys[indiceRelativo].Values["ID_PRODUCTO"].ToString();
                    string precio = GrillaProductos.Rows[indiceRelativo].Cells[16].Text;

                    for (int i = 0; i < ((DataTable)Session["Pedidos"]).Rows.Count; i++)
                    {
                        DataTable dt = (DataTable)Session["Pedidos"];
                        if (dt.Rows[i]["ID"].ToString().Equals(id))
                        {
                            int cantidadVieja = int.Parse(dt.Rows[i]["Cantidad"].ToString());
                            int cantidadNueva = totalAPedir;
                            int resultado = cantidadVieja + cantidadNueva;

                            
                            dt.Rows[i]["Cantidad"] = resultado.ToString();

                            Decimal precioProducto = Decimal.Parse(precio);
                            Decimal subtotalNuevo = precioProducto * resultado;
                            dt.Rows[i]["SubTotal"] = subtotalNuevo.ToString();

                            Session["Pedidos"] = dt;

                            GrillaProductoPedido.DataSource = dt;
                            GrillaProductoPedido.Columns[0].Visible = true;
                            GrillaProductoPedido.Columns[1].Visible = true;
                            int paginaActual = GrillaProductoPedido.PageIndex;
                            GrillaProductoPedido.PageIndex = paginaActual;
                            GrillaProductoPedido.DataBind();
                            GrillaProductoPedido.Columns[0].Visible = false;
                            GrillaProductoPedido.Columns[1].Visible = false;

                            nuevoProducto = false;
                            break;
                        }
                    }

                    if (nuevoProducto)
                    {
                        string nombre = GrillaProductos.Rows[indiceRelativo].Cells[8].Text;
                        string cantidad = tbCantidad.Text;
                        string stock = GrillaProductos.DataKeys[indiceRelativo].Values["ID_STOCK"].ToString();

                        Decimal DPrecio = Decimal.Parse(precio);
                        int ICantidad = int.Parse(cantidad);

                        Decimal subtotal = DPrecio * ICantidad;

                        DataTable dt = (DataTable)Session["Pedidos"];

                        DataRow dr = dt.NewRow();
                        dr["ID"] = id;
                        dr["IDStock"] = stock;
                        dr["Nombre"] = nombre;
                        dr["Precio"] = DPrecio;
                        dr["Cantidad"] = cantidad;
                        dr["SubTotal"] = subtotal;

                        dt.Rows.Add(dr);
                        Session["Pedidos"] = dt;

                        GrillaProductoPedido.DataSource = dt;
                        GrillaProductoPedido.Columns[0].Visible = true;
                        GrillaProductoPedido.Columns[1].Visible = true;
                        int paginaActual = GrillaProductoPedido.PageIndex;
                        GrillaProductoPedido.PageIndex = paginaActual;
                        GrillaProductoPedido.DataBind();
                        GrillaProductoPedido.Columns[0].Visible = false;
                        GrillaProductoPedido.Columns[1].Visible = false;
                    }

                    // Actualizar la cantidad en el DataTable con índice global
                    int reduccion = totalProducto - totalAPedir;
                    dtListadoProd.Rows[indiceGlobal]["CANTIDAD"] = reduccion.ToString();

                   
                    if (buscar)
                    {
                        DataTable dtList = (DataTable)Session["ListaProductos"];
                        int idbuscar = int.Parse(Session["IdProdBusc"].ToString());

                        string filtro = $"ID_PRODUCTO = {idbuscar}";
                        DataRow[] filas = dtList.Select(filtro);

                        // Verificar si se encontraron filas
                        if (filas.Length > 0)
                        {

                            foreach (DataRow fila in filas)
                            {
                                fila["CANTIDAD"] = reduccion.ToString();
                            }
                        }

                        GrillaProductos.DataSource = dtList;
                    }
                    else
                    {
                        GrillaProductos.DataSource = dtListadoProd;
                    }
                    GrillaProductos.DataBind();
                    DataTable dtp = (DataTable)Session["Pedidos"];

                    foreach (DataRow fila in dtp.Rows)
                    {
                        total += Decimal.Parse(fila["SubTotal"].ToString());
                    }
                    lblTotProd.Text = total.ToString();

                    tbCantidad.Text = "0";
                    UpdatePanel3.Update();
                    lblNomProd.Text = string.Empty;
                    tbCantidad.Text = "0";
                    tbCantidad.Enabled = false;
                    Button1.Enabled = false;
                    btnCalcularTotal.Enabled = true;
                    BtnInsertpedido.Enabled = true;
                    udpInsertarPedido.Update();
                    UpdatePanel1.Update();
                    udpLblTotalProd.Update();
                    CBviatico.Enabled = true;
                    udpCBViatico.Update();
                    UpdatePanel4.Update();

                }
                else if (totalAPedir > totalProducto)
                {
                    string mensaje = "La cantidad de " + totalAPedir + " productos seleccionados, supera a los " + totalProducto + " disponibles";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
                else if (totalAPedir <= 0) 
                {
                    string mensaje = "La cantidad de productos seleccionados, debe ser mayor a 0 ";
                    tbCantidad.Text = "0";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
                Session["BusquedaProdVent"] = false;
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al seleccionar un producto: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }

        protected void BtnInsertpedido_Click(object sender, EventArgs e)
        {
            if (GrillaProductoPedido.Rows.Count > 0)
            {
                AgregarPedido();
                //Session.Remove("Pedidos");
                GrillaProductoPedido.DataSource = null;
                GrillaProductoPedido.DataSourceID = null;
                GrillaProductoPedido.DataBind();
            }
        }

        private void InsertarDetalle(string idPed, string idProd, string nombre, string precio, string cantidad, string subtotal)
        {
            try
            {
                SqlDetallePedido.InsertParameters["ID_PEDIDO"].DefaultValue = idPed;
                SqlDetallePedido.InsertParameters["idProd"].DefaultValue = idProd;
                SqlDetallePedido.InsertParameters["nombre"].DefaultValue = nombre;
                SqlDetallePedido.InsertParameters["precio"].DefaultValue = precio;
                SqlDetallePedido.InsertParameters["CANTIDAD"].DefaultValue = cantidad;
                SqlDetallePedido.InsertParameters["subTotal"].DefaultValue = subtotal;
                SqlDetallePedido.Insert();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar el detalle del pedido: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        private void AgregarPedido()
        {
            udpLblTotal.Update();
            udpPrecioViatico.Update();
            CalcualarTotal();
            try
            {
                SqldsProductosYPedido.InsertParameters["ID_CLIENTE"].DefaultValue = lblIdCli.Text;
                SqldsProductosYPedido.InsertParameters["ID_ESTADO_PEDIDO"].DefaultValue = "1";
                SqldsProductosYPedido.InsertParameters["FECHA_PEDIDO"].DefaultValue = DateTime.Now.Date.ToString();
                SqldsProductosYPedido.InsertParameters["DESCRIPCION"].DefaultValue = tbDesc.Text;
                SqldsProductosYPedido.InsertParameters["total"].DefaultValue = lblTotal.Text;
                SqldsProductosYPedido.InsertParameters["Viatico"].DefaultValue = LblViatico.Text;
                SqldsProductosYPedido.Insert();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar el pedido pedidos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }


        protected void SqldsProductosYPedido_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {

            DataTable dt = (DataTable)Session["Pedidos"];
            int Id = Convert.ToInt32(e.Command.Parameters["@ID_PEDIDO"].Value);
            if (Id != 0)
            {
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string idPed = Id.ToString();
                        string idProd = row["ID"].ToString();
                        string nom = row["Nombre"].ToString();
                        string precio = row["Precio"].ToString();
                        string cantidad = row["Cantidad"].ToString();
                        string subtotal = row["SubTotal"].ToString();

                        if (!string.IsNullOrEmpty(idProd) && !string.IsNullOrEmpty(precio) && !string.IsNullOrEmpty(cantidad) && !string.IsNullOrEmpty(subtotal))
                        {

                            InsertarDetalle(idPed, idProd, nom, precio, cantidad, subtotal);

                            string idStock = row["IDStock"].ToString();

                            SqlStock.DeleteParameters["idProd"].DefaultValue = idProd;
                            SqlStock.DeleteParameters["cantPed"].DefaultValue = cantidad;
                            SqlStock.DeleteParameters["idStock"].DefaultValue = idStock;
                            SqlStock.Delete();
                            //ActualizarStock(idProd, idStock, cantidad);
                        }
                    }
                    string mensaje = "Se registro el pedido exitosamente";
                    MostrarAlerta(mensaje, NotificationType.success);

                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al modificar el stock: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            try
            {
                LblViatico.Text = "0";
                lblTotProd.Text = "0";
                lblTotal.Text = "0";
                CBviatico.Enabled = false;
                TbViatico.Enabled = false;
                btnSumar.Enabled = false;
                //btnRestar.Enabled = false;
                btnCalcularTotal.Enabled = false;
                BtnInsertpedido.Enabled = false;
                udpInsertarPedido.Update();
                UpdatePanel4.Update();
                udpPrecioViatico.Update();
                udpLblTotalProd.Update();
                udpLblTotal.Update();
                udpCBViatico.Update();
                cargarProductos();
                GrillaProductoPedido.DataSource = null; 
                GrillaProductoPedido.DataBind();
                UpdatePanel3.Update();
                CrearDtProd();
            }
            catch (Exception ex) 
            {
                string mensaje = "Ocurrió un error al limpiar los campos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void BtnViatico_Click(object sender, EventArgs e)
        {
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(tbCantidad.Text) > 0 || !tbCantidad.Enabled && int.Parse(tbCantidad.Text) == 0)
            {
                args.IsValid = true;
            }
            else { args.IsValid = false; }
        }


        protected void GrillaProductoPedido_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Obtener los DataTables desde la sesión
            DataTable dtPedidos = Session["Pedidos"] as DataTable;
            DataTable dtListadoProd = Session["ListaProductos"] as DataTable;

            

            if (dtPedidos != null && dtListadoProd != null)
            {
                try
                {
                    int indice = (GrillaProductoPedido.PageSize * GrillaProductoPedido.PageIndex) + e.RowIndex;
                    // Obtener la ID del producto que se está eliminando
                    string idProductoEliminar = dtPedidos.Rows[indice]["ID"].ToString();
                    int cantidadDevolver = int.Parse(dtPedidos.Rows[indice]["Cantidad"].ToString());

                    // Buscar la fila correspondiente en el DataTable original (ListaProductos)
                    DataRow filaProducto = dtListadoProd.AsEnumerable()
                        .FirstOrDefault(row => row["ID_PRODUCTO"].ToString() == idProductoEliminar);

                    if (filaProducto != null)
                    {
                        // Actualizar la cantidad en ListaProductos
                        int cantidadActual = int.Parse(filaProducto["CANTIDAD"].ToString());
                        filaProducto["CANTIDAD"] = (cantidadActual + cantidadDevolver).ToString();

                        // Volver a enlazar GrillaProductos con ListaProductos
                        GrillaProductos.DataSource = dtListadoProd;
                        GrillaProductos.DataBind();
                    }

                    // Calcular el nuevo total

                    decimal subtotalEliminar = decimal.Parse(dtPedidos.Rows[indice]["SubTotal"].ToString());
                    decimal totalActual = decimal.Parse(lblTotProd.Text);
                    lblTotProd.Text = (totalActual - subtotalEliminar).ToString();

                    // Eliminar la fila del DataTable de Pedidos
                    Console.WriteLine(dtPedidos.Rows[indice]["Nombre"]);
                    dtPedidos.Rows[indice].Delete();

                    // Volver a enlazar GrillaProductoPedido con el DataTable actualizado
                    GrillaProductoPedido.DataSource = dtPedidos;
                    GrillaProductoPedido.DataBind();

                    // Guardar los DataTables actualizados en la sesión
                    Session["Pedidos"] = dtPedidos;
                    Session["ListaProductos"] = dtListadoProd;

                    // Actualizar los UpdatePanels
                    udpLblTotalProd.Update();
                    CalcualarTotal();
                    UpdatePanel2.Update();
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {

                    string mensaje = "Ocurrió un error al borrar el producto seleccionado: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }


        protected void GrillaProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)Session["ListaProductos"];
                GrillaProductos.PageIndex = e.NewPageIndex;
                GrillaProductos.DataSource = dt;
                GrillaProductos.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar la página de la tabla productos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void GrillaProductoPedido_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaProductoPedido.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)Session["Pedidos"];
                GrillaProductoPedido.DataSource = dt;
                GrillaProductoPedido.DataBind();
                UpdatePanel3.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar la página de la tabla productos pedidos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnSumar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TbViatico.Text.Equals("0"))
                {
                    Decimal Total = Decimal.Parse(TbViatico.Text);
                    LblViatico.Text = Total.ToString();
                    Session["ViaticoPedido"] = TbViatico.Text;

                }
                else
                {
                    LblViatico.Text = "0";
                }
                CalcualarTotal();
                udpPrecioViatico.Update();
                //udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al sumar el precio: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        /*protected void btnSumar_Click(object sender, EventArgs e)
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
                string mensaje = "Ocurrió un error al sumar el precio: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }*/



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
                        MostrarAlerta(msg, NotificationType.warning);
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
                string mensaje = "Ocurrió un error al restar el precio: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void udpEstado(string mensaje)
        {
            labelresultadoCalculos.Text = mensaje;
            updresultCalculos.Update();
        }

        private void CalcualarTotal()
        {
            try
            {
                Session["ViaticoPedido"] = 0;
                Decimal total = Decimal.Parse(lblTotal.Text);
                Decimal sesionViatico= Decimal.Parse(Session["ViaticoPedido"].ToString());
                Decimal viatico = Decimal.Parse(LblViatico.Text);
                if (viatico != sesionViatico)
                {
                    total = total - sesionViatico;
                }

                Decimal productos = Decimal.Parse(lblTotProd.Text);
                Decimal resultado = productos + viatico;
                lblTotal.Text = resultado.ToString();
                udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al calcular el total: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        /*private void CalcualarTotal()
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
                string mensaje = "Ocurrió un error al calcular el total: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }*/
        protected void btnCalcularTotal_Click(object sender, EventArgs e)
        {
            CalcualarTotal();
        }


        private void Busqueda(string campo) 
        {
            try
            {
                DataTable dtOriginal = (DataTable)Session["ListaProductos"];

                // Filtrar el DataTable
                string filtro = $"{campo} LIKE '%{txtBuscar.Text}%'";
                DataRow[] filasFiltradas = dtOriginal.Select(filtro);
                if (filasFiltradas.Count() > 0)
                {

                    DataTable dtFiltrado = dtOriginal.Clone();
                    foreach (DataRow fila in filasFiltradas)
                    {
                        dtFiltrado.ImportRow(fila);
                    }


                    Session["ResutadoBusquedaProd"] = dtFiltrado;

                    GrillaProductos.DataSource = dtFiltrado;
                    GrillaProductos.DataBind();
                    UpdatePanel1.Update();
                }
                else 
                {
                    if (campo.Equals("DESCRIPCION"))
                    {
                        campo = "nombre";
                    }
                    else { campo = "codigo"; }

                    string mensaje = $"No se encontro un producto con un {campo} con el valor: {txtBuscar.Text}";
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            catch (Exception ex)
            {

                if (campo.Equals("DESCRIPCION"))
                {
                    campo = "nombre";
                }
                else { campo = "codigo"; }
                string mensaje = $"Ocurrió un error al buscar el producto por su {campo}: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void BuscarProducto()
        {
            if (RbBuscDesc.Checked)
            {
              Busqueda("DESCRIPCION");
            }

            if (RbBuscCod.Checked)
            {
                Busqueda("COD_PROD");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Equals(""))
            {
                Session["BusquedaProdVent"] = false;
                DataTable dt = (DataTable)Session["ListaProductos"];
                GrillaProductos.DataSource = dt;
                GrillaProductos.DataBind();
                UpdatePanel1.Update();
            }
            else
            {
                Session["BusquedaProdVent"] = true;
                BuscarProducto();
            }
        }

    }
}