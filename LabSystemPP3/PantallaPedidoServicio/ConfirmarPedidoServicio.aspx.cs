using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;
using static System.Net.WebRequestMethods;

namespace LabSystemPP3.PantallaPedidoServicio
{
    public partial class ConfirmarPedidoServicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            udpEstado("");
            if (!IsPostBack)
            {
                lblIDPedido.Text = Request.QueryString["id"];
                CrearDtInsumos();
                CargarEmpleado();
                CargarProductos();
                CargarDatos();
                CargarInsumos();
                cargarInsumosSeleccionado();
                CargarDtFacturas();
                Session["PrecioServicio"] = 0;
            }
        }

        private void udpEstado(string mensaje)
        {
            lblEstado.Text = mensaje;
            updpEstado.Update();
        }
        private void CargarProductos()
        {
            try
            {
                SqlProductosCliente.SelectParameters["ID"].DefaultValue = lblIDPedido.Text;
                GrillaProductos.DataSource = SqlProductosCliente;
                GrillaProductos.DataBind();
                UpdatePanel3.Update();

            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CargarInsumos()
        {
            try
            {
                SqlInsumos.SelectParameters["idCli"].DefaultValue = lblIdCli.Text;
                GrillaInsumos.DataSource = SqlInsumos;
                GrillaInsumos.DataBind();
                udpInsumos.Update();

                DataView dv = (DataView)SqlInsumos.Select(DataSourceSelectArguments.Empty);

                Session["ListaInsumos"] = dv.ToTable();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CargarDatos()
        {
            try
            {
                this.SqlPedido.SelectParameters["IDPedido"].DefaultValue = lblIDPedido.Text;
                this.SqlPedido.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader datos;

                datos = (SqlDataReader)this.SqlPedido.Select(DataSourceSelectArguments.Empty);
                if (datos.Read())
                {
                    lblIdCli.Text = datos["ID_CLIENTE"].ToString();
                    lblFecha.Text = DateTime.Parse(datos["FECHA_PEDIDO"].ToString()).ToString("dd/MM/yyyy");

                    string fechaSeleccionada = datos["FECHA_SERVICIO"].ToString();

                    // Convertir la fecha a DateTime
                    DateTime fecha;
                    if (DateTime.TryParse(fechaSeleccionada, out fecha))
                    {
                        TbFecha.Text = String.Format("{0:yyyy-MM-dd}", fecha);
                    }
                    else { TbFecha.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Today); }

                    lblNomCli.Text = datos["NOM_CLIENTE"].ToString();
                    lblRs.Text = datos["RAZON_SOCIAL"].ToString();
                    lblTipoServ.Text = datos["ID_TIPO_PEDIDO"].ToString();
                    tbCalle.Text = datos["Calle"].ToString();
                    tbAltura.Text = datos["Altura"].ToString();
                    tbTitulo.Text = datos["TITULO"].ToString();
                    tbDesc.Text = datos["DESCRIPCION"].ToString();

                    TbFecha.Enabled = false;
                    tbCalle.Enabled = false;
                    tbAltura.Enabled = false;
                    tbTitulo.Enabled = false;
                    tbDesc.Enabled = false;
                }
                else
                {
                    string mensaje = "No existe un usuario con dicho nombre";
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error: " + ex.Message;
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


        private void CrearDtInsumos()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("ID_PRODUCTO");
            dt.Columns.Add("DESC");
            dt.Columns.Add("DESCRIPCION");
            dt.Columns.Add("COD_PROD");
            dt.Columns.Add("ID_TIPO");
            dt.Columns.Add("ID_STOCK");
            dt.Columns.Add("CANTIDAD");
            dt.Columns.Add("CANTIDAD_MIN");
            dt.Columns.Add("precio");

            GrillaSeleccionados.DataSource = dt;
            GrillaSeleccionados.DataBind();
            Session["seleccionPI"] = dt;
            UpdatePanel3.Update();
        }


        protected void SqlPedido_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {


            // Verificar que el comando contiene parámetros
            if (e.Command.Parameters.Count > 0)
            {
                int Id = Convert.ToInt32(e.Command.Parameters["@IDServ"].Value);
                DataTable dt = (DataTable)Session["PedidoIns"];

                try
                {
                    foreach (DataRow fila in dt.Rows)
                    {

                        var idProducto = fila["ID_PRODUCTO"];
                        var precio = fila["Precio"];
                        var subtotal = fila["SubTotal"];
                        var cantidad = fila["Cantidad"];
                        var descripcion = fila["DESCRIPCION"];
                        var idStock = fila["IDStock"];

                        SqlInsumos.UpdateParameters["idProd"].DefaultValue = idProducto.ToString();
                        SqlInsumos.UpdateParameters["cantPed"].DefaultValue = cantidad.ToString();
                        SqlInsumos.UpdateParameters["idStock"].DefaultValue = idStock.ToString();
                        SqlInsumos.Update();

                        SqlDetallePedido.InsertParameters["IdServicio"].DefaultValue = Id.ToString();
                        SqlDetallePedido.InsertParameters["idProd"].DefaultValue = idProducto.ToString();
                        SqlDetallePedido.InsertParameters["precio"].DefaultValue = precio.ToString();
                        SqlDetallePedido.InsertParameters["subTotal"].DefaultValue = subtotal.ToString();
                        SqlDetallePedido.InsertParameters["cantidad"].DefaultValue = cantidad.ToString();
                        SqlDetallePedido.InsertParameters["nombre"].DefaultValue = descripcion.ToString();
                        SqlDetallePedido.Insert();

                    }
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al insertar el detalle del pedido, error: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }

                DataTable dtF = (DataTable)Session["Facturas"];

                try
                {
                    foreach (DataRow fila in dtF.Rows)
                    {
                        var url = fila["Url"];
                        var nombre = fila["NombreFactura"];




                        SqlInsumos.InsertParameters["id_servicio"].DefaultValue = Id.ToString();
                        SqlInsumos.InsertParameters["ID_CLIENTE"].DefaultValue = lblIdCli.Text;
                        SqlInsumos.InsertParameters["url_ubicacion"].DefaultValue = url.ToString();
                        SqlInsumos.InsertParameters["nombre"].DefaultValue = nombre.ToString();
                        SqlInsumos.Insert();
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al insertar el detalle del pedido, error: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }

            }
            else
            {
                // Manejo del caso en que no hay parámetros
                string mensaje = "No se recibieron parámetros de salida.";
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void InsertarServicio()
        {
            try
            {
                CalcualarTotal();
                udpLblTotal.Update();
                udpPrecioServ.Update();

                SqlPedido.InsertParameters["idEmp"].DefaultValue = lblIdEmp.Text;
                SqlPedido.InsertParameters["total"].DefaultValue = lblTotal.Text;
                SqlPedido.InsertParameters["idPedido"].DefaultValue = lblIDPedido.Text;
                SqlPedido.Insert();
            }
            catch (Exception e)
            {
                string mensaje = "Ocurrio un error al registrar el servicio, error: " + e.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }


            try
            {

                SqlPedido.UpdateParameters["idPed"].DefaultValue = lblIDPedido.Text;
                SqlPedido.UpdateParameters["idEstado"].DefaultValue = "7";
                SqlPedido.Update();
            }
            catch (Exception e)
            {
                string mensaje = "Ocurrio un error al registrar el servicio, error: " + e.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnAgregarP_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            CalcualarTotal();
            if (lblTotal.Text.Equals("0") || lblIdEmp.Text.Equals(""))
            {
                mensaje = "Ingrese los montos y al empleado correspondiente";
                MostrarAlerta(mensaje, NotificationType.warning);
                ///udpEstado(mensaje);
               

            }
            else
            {

                InsertarServicio();
            }

            if (mensaje.Equals(""))
            {
                Response.Redirect("~/PantallaPedidoServicio/ListadoPedidoServicios.aspx");
            }
        }

        protected void GrillaSeleccionados_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Obtener el índice de la fila que se está eliminando
            // Obtener el DataTable desde la sesión
            DataTable dt = Session["PedidoIns"] as DataTable;
            DataTable dtListadoIns = Session["ListaInsumos"] as DataTable;

            if (dt != null && dtListadoIns!=null)
            {
                try
                {
                    int indice = (GrillaSeleccionados.PageSize * GrillaSeleccionados.PageIndex) + e.RowIndex;
                    // Obtener la ID del producto que se está eliminando
                    string idInsumoEliminar = GrillaSeleccionados.DataKeys[e.RowIndex].Values["ID_PRODUCTO"].ToString();
                    int cantidadDevolver = int.Parse(dt.Rows[indice]["Cantidad"].ToString());

                    // Buscar la fila correspondiente en el DataTable original (ListaInsumos)
                    DataRow filaInsumo = dtListadoIns.AsEnumerable()
                        .FirstOrDefault(row => row["ID_PRODUCTO"].ToString() == idInsumoEliminar);

                    if (filaInsumo != null)
                    {
                        // Actualizar la cantidad en ListaInsumos
                        int cantidadActual = int.Parse(filaInsumo["CANTIDAD"].ToString());
                        filaInsumo["CANTIDAD"] = (cantidadActual + cantidadDevolver).ToString();

                        // Volver a enlazar GrillaInsumos con ListaInsumos
                        GrillaInsumos.DataSource = dtListadoIns;
                        GrillaInsumos.DataBind();
                    }

                    // Calcular el nuevo total
                    decimal subtotalEliminar = decimal.Parse(dt.Rows[indice]["SubTotal"].ToString());//
                    decimal totalActual = decimal.Parse(lblTotInsum.Text);
                    lblTotInsum.Text = (totalActual - subtotalEliminar).ToString();

                    // Eliminar la fila del DataTable de insumos seleccionados
                    dt.Rows[indice].Delete();

                    // Volver a enlazar GrillaSeleccionado con el DataTable actualizado
                    GrillaSeleccionados.DataSource = dt;
                    GrillaSeleccionados.DataBind();

                    // Guardar los DataTables actualizados en la sesión
                    Session["PedidoIns"] = dt;
                    Session["ListaInsumos"] = dtListadoIns;

                    // Actualizar los UpdatePanels
                    udpInsumos.Update();
                    udpLblTotalIns.Update();
                    CalcualarTotal();
                    udpInsSelect.Update();

                }
                catch (Exception ex)
                {

                    string mensaje = "Ocurrió un error al borrar el insumo seleccionado: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }

        }

        protected void btnSelectEqui_Click(object sender, EventArgs e)
        {

        }


        protected void GrillaEmpleado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaEmpleado.PageIndex = e.NewPageIndex;
                CargarEmpleado();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrio un error " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }

        }

        protected void GrillaEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string nombre = GrillaEmpleado.Rows[GrillaEmpleado.SelectedIndex].Cells[4].Text +
                                 " " + GrillaEmpleado.Rows[GrillaEmpleado.SelectedIndex].Cells[5].Text;

                lblEmpleadoSelect.Text = "Empleado seleccionado: " + nombre;
                lblIdEmp.Text = GrillaEmpleado.DataKeys[GrillaEmpleado.SelectedIndex].Values["ID_EMPLEADO_SERVICIO"].ToString();
                udpEmp.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrio un error " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        private void CargarEmpleado()
        {
            try
            {
                GrillaEmpleado.DataSource = SqlEmpleado;
                GrillaEmpleado.DataBind();
                udpEmp.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrio cargar a los empleados " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }
        private void busquedaEmpleado()
        {
            try
            {
                SqlBuscarEmpleado.SelectParameters["nom"].DefaultValue = txtBuscarEmp.Text;
                GrillaEmpleado.DataSource = SqlBuscarEmpleado;
                GrillaEmpleado.DataBind();
                udpEmp.Update();
            }
            catch (Exception ex)
            {
                CargarEmpleado();
                string msg = "Ocurrio un error al buscar el empleado " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }

        }

        protected void btnBuscarEmp_Click(object sender, EventArgs e)
        {
            if (txtBuscarEmp.Text.Equals(""))
            {
                CargarEmpleado();
            }
            else { busquedaEmpleado(); }
        }

        protected void cbConsumoInt_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbConsumoInt.Checked)
                {
                    GrillaInsumos.Enabled = true;
                    GrillaInsumos.Visible = true;
                    GrillaSeleccionados.Enabled = true;
                    GrillaSeleccionados.Visible = true;
                    btnSelectIns.Enabled = true;
                    btnSelectIns.Visible = true;
                    numberCantidad.Enabled = true;
                    numberCantidad.Visible = true;
                    Label8.Visible = true;
                    updAgregarIns.Update();
                    udpInsSelect.Update();
                    udpInsumos.Update();
                }
                else
                {
                    GrillaInsumos.Enabled = false;
                    GrillaInsumos.Visible = false;
                    GrillaSeleccionados.Enabled = false;
                    GrillaSeleccionados.Visible = false;
                    btnSelectIns.Enabled = false;
                    btnSelectIns.Visible = false;
                    numberCantidad.Enabled = false;
                    numberCantidad.Visible = false;
                    numberCantidad.Text = "1";
                    Label8.Visible = false;
                    updAgregarIns.Update();
                    udpInsSelect.Update();
                    udpInsumos.Update();
                }
            }
            catch (Exception ex)
            {

                string mensaje = "Ocurrió un error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void GrillaInsumos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                try
                {

                    IndiceGrillaInsumos.Text = index.ToString();
                    btnSelectIns.Enabled = true;
                    numberCantidad.Enabled = true;
                    updAgregarIns.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al seleccionar el insumo: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }


        private void cargarInsumosSeleccionado()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID_PRODUCTO");
            dt.Columns.Add("IDStock");
            dt.Columns.Add("COD_PROD");
            dt.Columns.Add("DESCRIPCION");
            dt.Columns.Add("Precio");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("SubTotal");
            Session["PedidoIns"] = dt;
        }

        private void agregarInsumos()
        {
            try
            {
                int indiceRelativo = int.Parse(IndiceGrillaInsumos.Text); // Índice relativo en la página actual
                int indiceGlobal = (GrillaInsumos.PageIndex * GrillaInsumos.PageSize) + indiceRelativo; // Índice global en el DataTable


                DataTable dtListadoIns = (DataTable)Session["ListaInsumos"];
             
                int totalProducto = int.Parse(dtListadoIns.Rows[indiceGlobal]["CANTIDAD"].ToString());
                int totalAPedir = int.Parse(numberCantidad.Text);

                if (!numberCantidad.Text.Equals("0") && totalAPedir <= totalProducto && totalAPedir > 0)
                {

                    bool nuevoProducto = true;
                    Decimal total = 0;
                    string id = this.GrillaInsumos.DataKeys[indiceRelativo].Values["ID_PRODUCTO"].ToString();
                    string precio = this.GrillaInsumos.Rows[indiceRelativo].Cells[8].Text;

                    for (int i = 0; i < ((DataTable)Session["PedidoIns"]).Rows.Count; i++)
                    {
                        DataTable dt = (DataTable)Session["PedidoIns"];
                        if (dt.Rows[i]["ID_PRODUCTO"].ToString().Equals(id))
                        {
                            int cantidadVieja = int.Parse(dt.Rows[i]["Cantidad"].ToString());
                            int cantidadNueva = totalAPedir;
                            int resultado = cantidadVieja + cantidadNueva;

                            
                            dt.Rows[i]["Cantidad"] = resultado.ToString();

                            Decimal precioProducto = Decimal.Parse(precio);
                            Decimal subtotalNuvo = precioProducto * resultado;
                            dt.Rows[i]["SubTotal"] = subtotalNuvo.ToString();

                            Session["PedidoIns"] = dt;

                            GrillaSeleccionados.DataSource = dt;
                            int paginaActual = GrillaSeleccionados.PageIndex;
                            GrillaSeleccionados.PageIndex = paginaActual;
                            GrillaSeleccionados.DataBind();

                            nuevoProducto = false;
                            break;
                        }
                    }

                    if (nuevoProducto)
                    {
                        string nombre = this.GrillaInsumos.Rows[indiceRelativo].Cells[2].Text;
                        string cantidad = numberCantidad.Text;
                        string stock = this.GrillaInsumos.DataKeys[indiceRelativo].Values["ID_STOCK"].ToString();
                        string cod = this.GrillaInsumos.Rows[indiceRelativo].Cells[3].Text;
                        precio = this.GrillaInsumos.Rows[indiceRelativo].Cells[8].Text;


                        Decimal DPrecio = Decimal.Parse(precio);
                        int ICantidad = int.Parse(cantidad);

                        Decimal subtotal = DPrecio * ICantidad;


                        // Crear una tabla para almacenar los datos
                        DataTable dt = (DataTable)Session["PedidoIns"];

                        //Agregar una fila con los datos de las Label

                        DataRow dr = dt.NewRow();
                        dr["ID_PRODUCTO"] = id;
                        dr["IDStock"] = stock;
                        dr["COD_PROD"] = cod;
                        dr["DESCRIPCION"] = nombre;
                        dr["Precio"] = DPrecio;
                        dr["Cantidad"] = cantidad;
                        dr["SubTotal"] = subtotal;

                        dt.Rows.Add(dr);
                        Session["PedidoIns"] = dt;

                        // Asignar el DataTable al GridView
                        GrillaSeleccionados.DataSource = dt;
                        int paginaActual = GrillaSeleccionados.PageIndex;
                        GrillaSeleccionados.PageIndex = paginaActual;
                        GrillaSeleccionados.DataBind();

                    }

                    // Actualizar la cantidad en el DataTable con índice global
                    int reduccion = totalProducto - totalAPedir;
                    this.GrillaInsumos.Rows[this.GrillaInsumos.SelectedIndex].Cells[6].Text = reduccion.ToString();
                    dtListadoIns.Rows[indiceGlobal]["CANTIDAD"] = reduccion.ToString();
                    GrillaInsumos.DataSource = dtListadoIns;
                    GrillaInsumos.DataBind();

                    DataTable dti = (DataTable)Session["PedidoIns"];

                    foreach (DataRow fila in dti.Rows)
                    {
                        total += Decimal.Parse(fila["SubTotal"].ToString());
                    }

                    lblTotInsum.Text = total.ToString();
                    udpLblTotalIns.Update();

                    numberCantidad.Text = "1";
                    udpInsumos.Update();
                    numberCantidad.Enabled = false;
                    btnSelectIns.Enabled = false;
                    udpInsSelect.Update();
                    updAgregarIns.Update();
                    udpLblTotal.Update();
                }
                else if (totalAPedir > totalProducto)
                {
                    string mensaje = "La cantidad de " + totalAPedir + " insumos seleccionados, supera a los " + totalProducto + " disponibles";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
                else if (totalAPedir <= 0)
                {
                    string mensaje = "La cantidad de productos seleccionados, debe ser mayor a 0 ";
                    numberCantidad.Text = "1";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error al seleccionar un insumo " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnSelectIns_Click(object sender, EventArgs e)
        {
            agregarInsumos();
        }

        protected void GrillaInsumos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrillaInsumos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dti = (DataTable)Session["ListaInsumos"];
                GrillaInsumos.PageIndex = e.NewPageIndex;
                GrillaInsumos.DataSource = dti;
                GrillaInsumos.DataBind();
                udpInsumos.Update();

            }
            catch (Exception ex)
            {
                string msg = "Ocurrio un error " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        protected void btnPrecioServicio_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TbPrecio.Text.Equals("0"))
                {
                    Decimal Total = Decimal.Parse(TbPrecio.Text);
                    lblPrecio.Text = Total.ToString();
                    Session["ViaticoPedido"] = TbPrecio.Text;

                }
                else
                {
                    lblPrecio.Text = "0";
                    
                }
                CalcualarTotal();
                udpPrecioServ.Update();
                //udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al sumar el precio: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnRestar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TbPrecio.Text.Equals("") || !TbPrecio.Text.Equals("0"))
                {
                    Decimal total = 0;
                    if (!lblPrecio.Text.Equals("0"))
                    {
                        total = Decimal.Parse(lblPrecio.Text);
                    }
                    Decimal precio = Decimal.Parse(TbPrecio.Text);
                    if (precio <= total)
                    {
                        Decimal Total = Decimal.Parse(lblPrecio.Text) - Decimal.Parse(TbPrecio.Text);
                        lblPrecio.Text = Total.ToString();
                        TbPrecio.Text = "0";
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
                udpPrecioServ.Update();
                udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrio un error " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        private void CargarDtFacturas()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Url");
            dt.Columns.Add("NombreFactura");
            dt.Columns.Add("Ver");
            Session["Facturas"] = dt;
        }

        protected void btnSubirFacturas_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile && FileUpload1.PostedFile.ContentType == "application/pdf")
            {
                try
                {
                    //string Url = Server.MapPath("~/Venta/Facturas de venta/");
                    string Url = Server.MapPath("~/FacturasServicios/");
                    string nombre = Path.GetFileName(FileUpload1.FileName);
                    string ruta = Url + nombre;

                    FileUpload1.SaveAs(ruta);

                    DataTable dt = (DataTable)Session["Facturas"];

                    DataRow dr = dt.NewRow();
                    dr["Url"] = Url;
                    dr["NombreFactura"] = nombre;
                    dr["Ver"] = "~/FacturasServicios/" + nombre;
                    dt.Rows.Add(dr);

                    Session["Facturas"] = dt;
                    GrillaFacturas.DataSource = Session["Facturas"];
                    GrillaFacturas.DataBind();
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrio un error " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
        }

        protected void GrillaFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    string ruta = e.CommandArgument.ToString();

                    // Abrir el archivo en una nueva pestaña
                    string script = "window.open('" + ResolveUrl(ruta) + "', '_blank');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPdf", script, true);
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrio un error " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
        }

        protected void GrillaFacturas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;

            try
            {
                // Obtener el DataTable de la sesión
                DataTable dt = (DataTable)Session["Facturas"];

                // Verificar si el índice es válido
                if (GrillaFacturas.Rows.Count > 0)
                {

                    string rutaVirtual = dt.Rows[index]["Ver"].ToString();
                    string rutaFisica = Server.MapPath(rutaVirtual);
                    if (System.IO.File.Exists(rutaFisica))
                    {
                        System.IO.File.Delete(rutaFisica);
                    }
                    // Eliminar la fila
                    dt.Rows[index].Delete();

                    // Actualizar la sesión
                    Session["Facturas"] = dt;

                    // Reenlazar el GridView con los datos actualizados
                    GrillaFacturas.DataSource = Session["Facturas"];
                    GrillaFacturas.DataBind();

                    // Actualizar el UpdatePanel
                    UpdatePanel1.Update();

                }
            }
            catch (Exception ex)
            {
                string msg = "Ocurrio un error " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        private void CalcualarTotal()
        {

            try
            {
                Session["PrecioServicio"] = 0;
                Decimal total = Decimal.Parse(lblTotal.Text);
                Decimal sesionServicio = Decimal.Parse(Session["PrecioServicio"].ToString());
                Decimal servicio = Decimal.Parse(lblPrecio.Text);
                if (servicio != sesionServicio)
                {
                    total = total - sesionServicio;
                }

                Decimal insumos = Decimal.Parse(lblTotInsum.Text);
                Decimal resultado = insumos + servicio;
                lblTotal.Text = resultado.ToString();
                udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al calcular el total: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }


            /*Decimal insumos = Decimal.Parse(lblTotInsum.Text);
            Decimal servicio = Decimal.Parse(lblPrecio.Text);
            Decimal resultado = insumos + servicio;
            lblTotal.Text = resultado.ToString();
            udpLblTotal.Update();*/
        }
        protected void btnCalcularTotal_Click(object sender, EventArgs e)
        {
            CalcualarTotal();
        }

        protected void GrillaSeleccionados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaSeleccionados.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)Session["PedidoIns"];
                GrillaSeleccionados.DataSource = dt;
                GrillaSeleccionados.DataBind();
                udpInsSelect.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrio un error " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }
    }
}