using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Reflection.Emit;
using static LabSystemPP3.Notificacion.Enum;


namespace LabSystemPP3.PantallaVenta
{
    public partial class VerModificarVenta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LblResOpe("");
            if (!IsPostBack)
            {
                CargarDatos();
                llenarVistaFacturas();
            }
        }

        private void CargarDatos()
        {
            //se van agregando los datos enviados desde ListaVentaCliente
            int ver = int.Parse(Request.QueryString["Vista"]);
            lblIdVenta.Text = Request.QueryString["ID_VENTA"];
            lblIdPed.Text = Request.QueryString["ID_PEDIDO"];
            DropDownList1.SelectedValue = Request.QueryString["ID_Pago"];
            ddlEmpleado.SelectedValue = Request.QueryString["empleado"];
            string fechav = Request.QueryString["FECHA_VENTA"];
            DateTime fv;
            DateTime.TryParse(fechav, out fv);
            lblFecha.Text = fv.ToString("dd/MM/yyyy");

            string fechap = Request.QueryString["Fecha_Pago"];
            DateTime fp;
            DateTime.TryParse(fechap, out fp);
            TbFecha.Text = fp.ToString("yyyy-MM-dd");
            TbFecha.Visible = true;
            lblTotal.Text = Request.QueryString["Total"];
            lblEstadoVen.Text = Request.QueryString["id_estado_ven"];
            CbFecha.Checked = true;

            try
            {
                //se seleccionan los datos del pedido
                SqlPedido.SelectParameters["idPedido"].DefaultValue = lblIdPed.Text;
                SqlPedido.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader reader;
                reader = (SqlDataReader)SqlPedido.Select(DataSourceSelectArguments.Empty);
                if (reader.Read())
                {
                    lblIdCli.Text = reader["ID_CLIENTE"].ToString();
                    lblNom.Text = reader["NOM_CLIENTE"].ToString();
                    lblAmbito.Text = reader["priv_pub"].ToString();
                    lblViatico.Text = reader["Viatico"].ToString();
                    lblDesc.Text = reader["DESCRIPCION"].ToString();
                }
                lblEstadoVen.Text = Request.QueryString["id_estado_ven"];
                if (ver == 1)
                {
                    CbFecha.Enabled = false;
                    TbFecha.Enabled = false;
                    btnSubirFacturas.Enabled = false;
                    btnUpdate.Enabled = false;
                    FileUpload1.Enabled = false;
                    GrillaFacturas.Enabled = true;
                    GrillaFacturas.Columns[4].Visible = false;
                    DropDownList1.Enabled = false;
                    btnUpdate.Visible = false;
                    ddlEmpleado.Enabled = false;
                }
                SqlProductos.SelectParameters["ID"].DefaultValue = lblIdPed.Text;
                GrillaProductos.DataBind();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cagar los datos: " + ex.Message;
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

        private void llenarVistaFacturas()
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("ID_FACTURA");
            dt.Columns.Add("url_ubicacion");
            dt.Columns.Add("nombre");

            try
            {
                SqlFactura.SelectParameters["idVenta"].DefaultValue = lblIdVenta.Text;
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
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cagar las facturas de la venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
        protected void CbFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (CbFecha.Checked)
            {
                TbFecha.Visible = true;
            }
            else { TbFecha.Visible = false; }
        }



        protected void btnSubirFacturas_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile && FileUpload1.PostedFile.ContentType == "application/pdf")
            {
                try
                {
                    string Url = Server.MapPath("~/PantallaVenta/Facturas de venta/");
                    string nombre = Path.GetFileName(FileUpload1.FileName);
                    string ruta = Url + nombre;

                    FileUpload1.SaveAs(ruta);
                    GuardarFacturas(nombre, Url);
                    DataTable dt = (DataTable)Session["Facturas"];

                    DataRow dr = dt.NewRow();
                    dr["ID_FACTURA"] = 0;
                    dr["url_ubicacion"] = Url;
                    dr["nombre"] = nombre;
                    dt.Rows.Add(dr);

                    Session["Facturas"] = dt;
                    GrillaFacturas.DataSource = Session["Facturas"];
                    GrillaFacturas.DataBind();
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al seleccionar la factura de la venta: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        private void GuardarFacturas(string nombre, string url)
        {
            try
            {
                SqlFactura.InsertParameters["id_venta"].DefaultValue = lblIdVenta.Text;
                SqlFactura.InsertParameters["ID_CLIENTE"].DefaultValue = lblIdCli.Text;
                SqlFactura.InsertParameters["url_ubicacion"].DefaultValue = url;
                SqlFactura.InsertParameters["nombre"].DefaultValue = nombre;
                int resultado = SqlFactura.Insert();
                if (resultado > 0)
                {
                    string mensaje = "Se registraron las facturas de la venta";
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar las facturas de la venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        protected void GrillaFacturas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = e.RowIndex;

                // Obtener el DataTable de la sesión
                DataTable dt = (DataTable)Session["Facturas"];

                // Verificar si el índice es válido
                if (GrillaFacturas.Rows.Count > 0)
                {
                    if (!GrillaFacturas.DataKeys[index].Value.ToString().Equals("0"))
                    {
                        SqlFactura.DeleteParameters["idVenta"].DefaultValue = lblIdVenta.Text;
                        SqlFactura.DeleteParameters["idFactura"].DefaultValue = GrillaFacturas.DataKeys[index].Value.ToString();
                        SqlFactura.Delete();
                    }
                    string ruta = dt.Rows[index]["url_ubicacion"].ToString() + dt.Rows[index]["nombre"].ToString();
                    string rutaVirtual = ruta;
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
                string mensaje = "Ocurrió un error al borrar la factura de la venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        protected void GrillaFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Select")
            {
                try
                {
                    string[] datos = e.CommandArgument.ToString().Split(',');
                    string nom = datos[0];
                    string url = datos[1];
                    string ruta = ResolveUrl(url + nom);


                    // Genera el script para abrir la ventana
                    string script = "window.open('" + ruta + "', '_blank');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPdf", script, true);
                    Response.Redirect(ruta);
                    Label7.Text = ruta;
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al seleccionar la factura de la venta: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else if (e.CommandName == "VerVenta")
            {
                try
                {
                    // Define la parte de la ruta a partir de la cual deseas extraer ("Facturas de venta")
                    string buscarFacturasDeVenta = @"\Facturas de venta\";

                    // Encuentra el índice de la carpeta "Facturas de venta" en la ruta completa
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GrillaFacturas.Rows[index];
                    int posicion = row.Cells[1].Text.IndexOf(buscarFacturasDeVenta);

                    if (posicion >= 0)
                    {
                        // Extrae la parte de la ruta desde la carpeta "Facturas de venta"
                        string ruta = row.Cells[1].Text.Substring(posicion);

                        // Convierte la ruta en una URL accesible desde la web
                        string rutaWeb = ruta.Replace(@"\", "/");

                        // Añadir "~" para que sea una ruta relativa válida en ASP.NET
                        string nuevaVentana = "~" + rutaWeb + row.Cells[2].Text;

                        // Redirigir a la nueva URL
                        Response.Redirect(nuevaVentana);
                    }

                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al intentar ver la factura de la venta: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        private int CargarFacturas(int idVenta)
        {
            int resultado = 0;
            if (idVenta > 0)
            {
                // Obtener el DataTable de la sesión
                DataTable dt = (DataTable)Session["Facturas"];

                bool facturaNueva = false;
                foreach (DataRow fila in dt.Rows)
                {
                    if (fila["ID_FACTURA"].ToString().Equals("0"))
                    {
                        try
                        {
                            SqlFactura.InsertParameters["id_venta"].DefaultValue = idVenta.ToString();
                            SqlFactura.InsertParameters["ID_CLIENTE"].DefaultValue = lblIdCli.Text;
                            SqlFactura.InsertParameters["url_ubicacion"].DefaultValue = fila["url_ubicacion"].ToString(); ;
                            SqlFactura.InsertParameters["nombre"].DefaultValue = fila["nombre"].ToString();
                            if (resultado == -2)
                            {
                                //si al menos una de las facuras dio error, ya no se toma el valor del insert
                                SqlFactura.Insert();
                            }
                            else
                            {
                                //caso contrario resultado toa lo obtenido el insert
                                resultado = SqlFactura.Insert();
                            }

                            facturaNueva = true;
                        }
                        catch (Exception ex)
                        {
                            StringBuilder mensaje = new StringBuilder();
                            mensaje.Append("Ocurrió un error al registrar la factura de la venta " + fila["nombre"].ToString() + ": " + ex.Message);
                            mensaje.Append(ex.Message);
                            mensaje.Append("<br>");
                            if (System.IO.File.Exists("~/PantallaVenta/Facturas de venta/" + fila["nombre"].ToString()))
                            {
                                System.IO.File.Delete("~/PantallaVenta/Facturas de venta/" + fila["nombre"].ToString());
                            }
                            LblResOpe(mensaje.ToString());
                            //uso -2 para controlar la exepcion del error
                            resultado = -2;

                        }
                    }
                }
                if (facturaNueva == false)
                {
                    //este -1 esta solo en caso de que no hubo ninguna factura nueva agreada, solo se moficaron otros datos
                    resultado = -1;
                }
            }
            return resultado;
        }

        private int ModificarVenta()
        {
            int resultado = 0;
            try
            {
                SqlPedido.UpdateParameters["ID_VENTA"].DefaultValue = lblIdVenta.Text;
                string fechaFormateada = "";
                if (CbFecha.Checked)
                {
                    DateTime fechaP;
                    DateTime.TryParse(TbFecha.Text, out fechaP);
                    fechaFormateada = fechaP.ToString("dd/MM/yyyy");
                    SqlPedido.UpdateParameters["Fecha_Pago"].DefaultValue = DateTime.Parse(fechaFormateada).ToString();
                }
                else
                {
                    fechaFormateada = DateTime.Now.ToString("dd/MM/yyyy");
                    SqlPedido.UpdateParameters["Fecha_Pago"].DefaultValue = DateTime.Parse(fechaFormateada).ToString();
                }
                if (fechaFormateada.Equals(DateTime.Now.ToString("dd/MM/yyyy")))
                {
                    SqlPedido.UpdateParameters["id_estado_ven"].DefaultValue = "2";
                }
                else { SqlPedido.UpdateParameters["id_estado_ven"].DefaultValue = "1"; }
                SqlPedido.UpdateParameters["ID_Pago"].DefaultValue = DropDownList1.SelectedValue;
                SqlPedido.UpdateParameters["idEmp"].DefaultValue = ddlEmpleado.SelectedValue;

                DateTime fechaSeleccionada;
                DateTime.TryParse(TbFecha.Text, out fechaSeleccionada);
                fechaFormateada = fechaSeleccionada.ToString("dd/MM/yyyy");

                DateTime fechaVenta;
                DateTime.TryParse(lblFecha.Text,out fechaVenta);

                if (fechaSeleccionada >= fechaVenta)
                {
                    resultado = SqlPedido.Update();
                }
                else 
                {
                    string mensaje = "La fecha seleccionada no puede ser menor a la fecha en el detalle de venta";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }

            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al modificar la venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
            return resultado;
        }

        protected void btnVenta_Click(object sender, EventArgs e)
        {
            int resultado = ModificarVenta();
            int facturas = CargarFacturas(int.Parse(lblIdVenta.Text));
            //si resultado es !=0 se modificaron los datos de la venta
            if (resultado != 0)
            {
                //si factura es !=0 y !=-2 entonse se cargaron correctamente, o no se agregaron nuevas facturas 
                if (facturas != 0 && facturas != -2)
                {

                    string mensaje = "Se modifico la venta correctamente";
                    MostrarAlerta(mensaje, NotificationType.success);
                }
                else
                {
                    string mensaje = "Se modifico la venta";
                    MostrarAlerta(mensaje, NotificationType.success);
                }
            }
        }

        protected void GrillaFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = (DataTable)Session["Facturas"];
            GrillaFacturas.PageIndex = e.NewPageIndex;
            GrillaFacturas.DataSource = dt;
            GrillaFacturas.DataBind();
            UpdatePanel1.Update();
        }
    }
}