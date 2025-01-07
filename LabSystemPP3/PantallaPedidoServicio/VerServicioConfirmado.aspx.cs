using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;
using System.Web.Services.Description;

namespace LabSystemPP3.PantallaPedidoServicio
{
    public partial class VerServicioConfirmado : System.Web.UI.Page
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

                CargarDtFacturas();

                CargarDatosServicio();
                cargarInsumosSeleccionado();
                llenarVistaFacturas();
            }
        }


        private void CargarDatosServicio()
        {


            try
            {
                SqlBuscarEmpleado.SelectParameters["idPedido"].DefaultValue = lblIDPedido.Text;
                this.SqlBuscarEmpleado.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader datos;

                datos = (SqlDataReader)this.SqlBuscarEmpleado.Select(DataSourceSelectArguments.Empty);
                if (datos.Read())
                {
                    lblIdservicio.Text = datos["IdServicio"].ToString();
                    lblIdEmp.Text = datos["idEmpleado"].ToString();
                    lblNomEmp.Text = datos["NOMBRE_EMP"].ToString();
                    lblTotal.Text = datos["total"].ToString();

                    udpLblTotal.Update();
                    TbFecha.Enabled = false;
                    tbCalle.Enabled = false;
                    tbAltura.Enabled = false;
                    tbTitulo.Enabled = false;
                    tbDesc.Enabled = false;
                    GrillaEmpleado.Enabled = false;
                    GrillaSeleccionados.Enabled = false;
                    txtBuscarEmp.Enabled = false;
                    btnAgregarP.Enabled = false;
                    btnRestar.Enabled = false;
                    btnCalcularTotal.Enabled = false;
                    btnPrecioServicio.Enabled = false;
                    numberCantidad.Enabled = false;
                    cbConsumoInt.Enabled = false;
                    TbPrecio.Enabled = false;
                    btnSelectIns.Enabled = false;
                    GrillaFacturas.Enabled = true;
                    GrillaFacturas.Visible = true;
                    GrillaSeleccionados.Visible = true;
                    udpEmp.Update();
                }
                else
                {
                    string mensaje = "No existe un usuario con dicho nombre";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error: " + ex.Message;
                MostrarAlerta(mensaje,NotificationType.error);
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
            /*
            try
            {
                SqlInsumos.SelectParameters["idCli"].DefaultValue = lblIdCli.Text;
                GrillaInsumos.DataSource = SqlInsumos;
                GrillaInsumos.DataBind();
                udpInsumos.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error: " + ex.Message;
                udpEstado(mensaje);
            }*/
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
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
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


        private void llenarVistaFacturas()
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("ID_FACTURA");
            dt.Columns.Add("url_ubicacion");
            dt.Columns.Add("nombre");

            SqlFactura.SelectParameters["Idserv"].DefaultValue = lblIdservicio.Text;
            SqlFactura.DataSourceMode = SqlDataSourceMode.DataReader;
            SqlDataReader datos;
            datos = (SqlDataReader)SqlFactura.Select(DataSourceSelectArguments.Empty);
            while (datos.Read())
            {
                DataRow dr = dt.NewRow();
                dr["ID_FACTURA"] = datos["ID_FACTURA"].ToString();
                dr["url_ubicacion"] = datos["url_ubicacion"].ToString();
                dr["nombre"] = datos["nombre"].ToString();
                dt.Rows.Add(dr);
            }
            Session["Facturas"] = dt;
            GrillaFacturas.DataSource = dt;
            GrillaFacturas.DataBind();
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
                MostrarAlerta(mensaje, NotificationType.warning);
            }
        }

        private void InsertarServicio()
        {
            try
            {
                udpLblTotal.Update();
                udpPrecioServ.Update();
                CalcualarTotal();
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
                SqlPedido.UpdateParameters["idEstado"].DefaultValue = "2";
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
            if (!lblTotal.Text.Equals("0") || !lblIdEmp.Text.Equals(""))
            {
                InsertarServicio();
            }
            else
            {
                string mensaje = "Ingrese los montos y al empleado correspondiente";
                udpEstado(mensaje);
                MostrarAlerta(mensaje, NotificationType.warning);
            }

            if (lblEstado.Text.Equals(""))
            {
                Response.Redirect("~/PantallaPedidoServicio/ListadoPedidoServicios.aspx");
            }
        }

        protected void GrillaSeleccionados_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Obtener el índice de la fila que se está eliminando
                int index = e.RowIndex;
                // Obtener el DataTable desde la sesión
                DataTable dt = Session["PedidoIns"] as DataTable;

                if (dt != null)
                {




                    for (int i = 0; i < GrillaInsumos.Rows.Count; i++)
                    {
                        int idIns = int.Parse(GrillaInsumos.DataKeys[i].Values["ID_PRODUCTO"].ToString());
                        int idSelct = int.Parse(GrillaSeleccionados.DataKeys[index].Values["ID_PRODUCTO"].ToString());
                        if (idIns == idSelct)
                        {
                            Decimal Total = Decimal.Parse(lblTotal.Text) - Decimal.Parse(GrillaSeleccionados.Rows[index].Cells[6].Text);
                            lblTotal.Text = Total.ToString();
                            string CantiGrilla = GrillaInsumos.Rows[i].Cells[6].Text;
                            string cantiSelec = GrillaSeleccionados.Rows[index].Cells[5].Text;
                            int suma = int.Parse(CantiGrilla) + int.Parse(cantiSelec);
                            this.GrillaInsumos.Rows[i].Cells[6].Text = suma.ToString();

                            // Eliminar la fila del DataTable
                            dt.Rows[index].Delete();

                            // Volver a enlazar el GridView con el DataTable actualizado
                            GrillaSeleccionados.DataSource = dt;
                            GrillaSeleccionados.DataBind();

                            // Guardar el DataTable actualizado en la sesión
                            Session["PedidoIns"] = dt;
                            udpInsumos.Update();
                            udpLblTotal.Update();
                            break;
                        }
                    }
                    udpInsumos.Update();
                    udpLblTotal.Update();
                }
            }catch(Exception ex) 
            {
                string mensaje = "Ocurrio un error:" + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
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
                lblEmpleadoSelect.Text = "Empleado seleccionado: " + GrillaEmpleado.Rows[GrillaEmpleado.SelectedIndex].Cells[2].Text;
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
        /* private void busquedaEmpleado()
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
                 udpEstado(msg);
             }

         }*/

        protected void btnBuscarEmp_Click(object sender, EventArgs e)
        {
            /* if (txtBuscarEmp.Text.Equals(""))
             {
                 CargarEmpleado();
             }
             else { busquedaEmpleado(); }*/
        }

        protected void cbConsumoInt_CheckedChanged(object sender, EventArgs e)
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
                RbAmb1.Enabled = true;
                RbAmb1.Visible = true;
                RbAmb2.Enabled = true;
                RbAmb2.Visible = true;
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
                RbAmb1.Enabled = false;
                RbAmb1.Visible = false;
                RbAmb1.Checked = true;
                RbAmb2.Enabled = false;
                RbAmb2.Visible = false;
                Label8.Visible = false;
                updAgregarIns.Update();
                udpInsSelect.Update();
                udpInsumos.Update();
            }
        }


        private void cargarInsumosSeleccionado()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idProd");
            dt.Columns.Add("COD_PROD");
            dt.Columns.Add("nomProd");
            dt.Columns.Add("precio");
            dt.Columns.Add("cantidad");
            dt.Columns.Add("subTotal");

            try
            {
                // Establecer el parámetro y obtener los datos del SqlDataSource como DataView
                SqlInsumos.SelectParameters["idServicio"].DefaultValue = lblIdservicio.Text;
                DataView dv = (DataView)SqlInsumos.Select(DataSourceSelectArguments.Empty);

                // Llenar el DataTable con los datos obtenidos
                foreach (DataRowView drv in dv)
                {
                    DataRow row = dt.NewRow();
                    row["idProd"] = drv["idProd"];
                    row["COD_PROD"] = drv["COD_PROD"];
                    row["nomProd"] = drv["nomProd"];
                    row["precio"] = drv["precio"];
                    row["cantidad"] = drv["cantidad"];
                    row["subTotal"] = drv["subTotal"];

                    dt.Rows.Add(row);
                }

                // Enlazar el DataTable al GridView y guardar en Session
                GrillaSeleccionados.DataSource = dt;
                GrillaSeleccionados.DataBind();
                udpInsSelect.Update();
                UpdatePanel1.Update();
                Decimal subtotal = 0;
                if (GrillaSeleccionados.Rows.Count > 0)
                {
                    for (int i = 0; i < GrillaSeleccionados.Rows.Count; i++)
                    {
                        Decimal subtotInsum = Decimal.Parse(GrillaSeleccionados.Rows[i].Cells[5].Text);
                        subtotal += subtotInsum;
                    }
                }
                if (subtotal != 0)
                {
                    lblTotInsum.Text = subtotal.ToString();
                    udpInsumos.Update();

                    Decimal resultado = Decimal.Parse(lblTotal.Text) - subtotal;
                    lblPrecio.Text = resultado.ToString();
                    udpPrecioServ.Update();
                }
            }catch(Exception ex) 
            {
                string mensaje= "Ocurrió un error al cargar los insumos del servicio: "+ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void agregarInsumos()
        {
            /*try
            {
                int totalProducto = int.Parse(this.GrillaInsumos.Rows[this.GrillaInsumos.SelectedIndex].Cells[6].Text);
                int totalAPedir = int.Parse(numberCantidad.Text);
                if (!numberCantidad.Text.Equals("0") && totalAPedir <= totalProducto && totalAPedir > 0)
                {

                    bool nuevoProducto = true;
                    Decimal total = 0;
                    string id = this.GrillaInsumos.DataKeys[GrillaInsumos.SelectedIndex].Values["ID_PRODUCTO"].ToString();
                    string precio = this.GrillaInsumos.Rows[this.GrillaInsumos.SelectedIndex].Cells[8].Text;

                    for (int i = 0; i < GrillaSeleccionados.Rows.Count; i++)
                    {
                        if (GrillaSeleccionados.DataKeys[i].Values["ID_PRODUCTO"].ToString().Equals(id))
                        {
                            int cantidadVieja = int.Parse(GrillaSeleccionados.Rows[i].Cells[5].Text);
                            int cantidadNueva = totalAPedir;
                            int resultado = cantidadVieja + cantidadNueva;
                            DataTable dt = (DataTable)Session["PedidoIns"];
                            dt.Rows[i]["Cantidad"] = resultado.ToString();
                            Decimal precioProducto = Decimal.Parse(precio);
                            Decimal subtotalNuvo = precioProducto * resultado;
                            dt.Rows[i]["SubTotal"] = subtotalNuvo.ToString();

                            Session["PedidoIns"] = dt;

                            GrillaSeleccionados.DataSource = dt;
                            GrillaSeleccionados.DataBind();

                            nuevoProducto = false;
                            break;
                        }
                    }

                    if (nuevoProducto)
                    {
                        string nombre = this.GrillaInsumos.Rows[this.GrillaInsumos.SelectedIndex].Cells[2].Text;
                        string cantidad = numberCantidad.Text;
                        string stock = this.GrillaInsumos.DataKeys[GrillaInsumos.SelectedIndex].Values["ID_STOCK"].ToString();
                        string cod = this.GrillaInsumos.Rows[this.GrillaInsumos.SelectedIndex].Cells[3].Text;
                        precio = this.GrillaInsumos.Rows[this.GrillaInsumos.SelectedIndex].Cells[8].Text;


                        Decimal DPrecio = Decimal.Parse(precio);
                        int ICantidad = int.Parse(cantidad);

                        Decimal subtotal = DPrecio * ICantidad;


                        // Crear una tabla para almacenar los datos
                        DataTable dt = (DataTable)Session["PedidoIns"];

                        //Agregar una fila con los datos de las Label
                        dt.Rows.Add(
                            id,
                            stock,
                            cod,
                            nombre,
                            DPrecio,
                            cantidad,
                            subtotal
                            );

                        Session["PedidoIns"] = dt;

                        // Asignar el DataTable al GridView
                        GrillaSeleccionados.DataSource = dt;
                        GrillaSeleccionados.DataBind();

                    }

                    string CantiGrilla = GrillaInsumos.Rows[this.GrillaInsumos.SelectedIndex].Cells[6].Text;
                    int reduccion = int.Parse(CantiGrilla) - int.Parse(numberCantidad.Text);
                    this.GrillaInsumos.Rows[this.GrillaInsumos.SelectedIndex].Cells[6].Text = reduccion.ToString();

                    foreach (GridViewRow fila in GrillaSeleccionados.Rows)
                    {
                        total += Decimal.Parse(fila.Cells[6].Text);
                    }

                    lblTotInsum.Text = total.ToString();
                    udpLblTotalIns.Update();

                    numberCantidad.Text = "1";
                    udpInsumos.Update();
                    numberCantidad.Enabled = false;
                    btnSelectIns.Enabled = false;
                    RbAmb1.Enabled = false;
                    RbAmb2.Enabled = false;
                    udpInsSelect.Update();
                    updAgregarIns.Update();
                    udpLblTotal.Update();
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error al seleccionar un insumo " + ex.Message;
                udpEstado(mensaje);
            }*/
        }

        protected void btnSelectIns_Click(object sender, EventArgs e)
        {
            agregarInsumos();
        }

        protected void GrillaInsumos_SelectedIndexChanged(object sender, EventArgs e)
        {/*
            btnSelectIns.Enabled = true;
            numberCantidad.Enabled = true;
            RbAmb1.Enabled = true;
            RbAmb2.Enabled = true;
            updAgregarIns.Update();*/
        }

        protected void GrillaInsumos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaInsumos.PageIndex = e.NewPageIndex;
                CargarInsumos();
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

                    Decimal Total = Decimal.Parse(lblPrecio.Text) + Decimal.Parse(TbPrecio.Text);
                    lblPrecio.Text = Total.ToString();
                    TbPrecio.Text = "0";


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
                        MostrarAlerta(msg, NotificationType.error);
                    }
                }
                else
                {
                    string msg = "El precio del servicio esta vacio";
                    MostrarAlerta(msg, NotificationType.error);
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
            /*if (FileUpload1.HasFile && FileUpload1.PostedFile.ContentType == "application/pdf")
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
                    udpEstado(msg);
                }
            }*/
        }

        protected void GrillaFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerVenta")
            {
                try
                {
                    // Define la parte de la ruta a partir de la cual deseas extraer ("Temp")
                    string buscarTemp = @"\FacturasServicios\";

                    // Encuentra el índice de la carpeta "Temp" en la ruta completa
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GrillaFacturas.Rows[index];
                    int posicion = row.Cells[1].Text.IndexOf(buscarTemp);

                    if (posicion >= 0)
                    {
                        // Extrae la parte de la ruta desde la carpeta "Temp"
                        string ruta = row.Cells[1].Text.Substring(posicion);

                        // Convierte la ruta en una URL accesible desde la web
                        string rutaWeb = ruta.Replace(@"\", "/");

                        // Añadir "~" para que sea una ruta relativa válida en ASP.NET
                        string nuevaVentana = "~" + rutaWeb + row.Cells[2].Text;


                        Response.Redirect(nuevaVentana);


                    }
                }catch (Exception ex) 
                {
                    string mensaje= "Ocurrió un error al intentar ver las facturas: "+ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
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
                    if (File.Exists(rutaFisica))
                    {
                        File.Delete(rutaFisica);
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
            Decimal insumos = Decimal.Parse(lblTotInsum.Text);
            Decimal servicio = Decimal.Parse(lblPrecio.Text);
            Decimal resultado = insumos + servicio;
            lblTotal.Text = resultado.ToString();
            udpLblTotal.Update();
        }
        protected void btnCalcularTotal_Click(object sender, EventArgs e)
        {
            CalcualarTotal();
        }
    }

}