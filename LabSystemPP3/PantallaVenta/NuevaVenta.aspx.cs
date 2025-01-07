using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaEntidades;
using CapaNegocios;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.Venta
{
    public partial class NuevaVenta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LblResOpe("");
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            try
            {
                lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblIdPed.Text = Request.QueryString["IdPedido"];
                lblIdCli.Text = Request.QueryString["IdCliente"];
                lblNom.Text = Request.QueryString["Cliente"];
                lblAmbito.Text = Request.QueryString["Ambito"];
                lblViatico.Text = Request.QueryString["Viatico"];
                lblTotal.Text = Request.QueryString["Total"];
                lblDesc.Text = Request.QueryString["Descripcion"];

                SqlProductos.SelectParameters["ID"].DefaultValue = lblIdPed.Text;
                GrillaProductos.DataBind();
                DataTable dt = new DataTable();
                dt.Columns.Add("Url");
                dt.Columns.Add("NombreFactura");
                dt.Columns.Add("Ver");
                Session["Facturas"] = dt;
            }catch (Exception ex) 
            { 
                string mensaje= "Ocurrió un error al cargar los datos del pedido de venta: "+ex.Message;
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

                    DataTable dt = (DataTable)Session["Facturas"];

                    DataRow dr = dt.NewRow();
                    dr["Url"] = Url;
                    dr["NombreFactura"] = nombre;
                    dr["Ver"] = "~/PantallaVenta/Facturas de venta/" + nombre;
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



        protected void GrillaFacturas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;

            // Obtener el DataTable de la sesión
            DataTable dt = (DataTable)Session["Facturas"];

            // Verificar si el índice es válido
            if (GrillaFacturas.Rows.Count > 0)
            {
                try
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
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al eliminar la factura de la venta: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
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
                    string mensaje = "Ocurrió un error al ver la factura de la venta: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        private int CargarFacturas(int idVenta)
        {
            int resultado = 0;
            if (idVenta > 0)
            {
                DataTable dt = (DataTable)Session["Facturas"];
                foreach (DataRow fila in dt.Rows)
                {
                    try
                    {
                        SqlFactura.InsertParameters["id_venta"].DefaultValue = idVenta.ToString();
                        SqlFactura.InsertParameters["ID_CLIENTE"].DefaultValue = lblIdCli.Text;
                        SqlFactura.InsertParameters["url_ubicacion"].DefaultValue = fila["Url"].ToString();
                        SqlFactura.InsertParameters["nombre"].DefaultValue = fila["NombreFactura"].ToString();
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
                    }
                    catch (Exception ex)
                    {
                        StringBuilder mensaje = new StringBuilder();
                        mensaje.Append("Ocurrió un error al registrar la factura de la venta " + fila["NombreFactura"].ToString() + ": " + ex.Message);
                        mensaje.Append(ex.Message);
                        mensaje.Append("<br>");
                        LblResOpe(mensaje.ToString());
                        if (File.Exists("~/PantallaVenta/Facturas de venta/" + fila["NombreFactura"].ToString()))
                        {
                            File.Delete("~/PantallaVenta/Facturas de venta/" + fila["NombreFactura"].ToString());
                        }
                        //uso -2 para controlar la exepcion del error
                        resultado = -2;
                    }
                }
            }
            return resultado;
        }

        private void CargarVenta()
        {
            try
            {
                SqlVenta.InsertParameters["ID_CLIENTE"].DefaultValue = lblIdCli.Text;
                SqlVenta.InsertParameters["ID_PEDIDO"].DefaultValue = lblIdPed.Text;

                SqlVenta.InsertParameters["FECHA_VENTA"].DefaultValue = DateTime.Parse(lblFecha.Text).ToString();
                string fechaFormateada = "";
                if (CbFecha.Checked)
                {
                    DateTime fechaP;
                    DateTime.TryParse(TbFecha.Text, out fechaP);
                    fechaFormateada = fechaP.ToString("dd/MM/yyyy");
                    SqlVenta.InsertParameters["Fecha_Pago"].DefaultValue = DateTime.Parse(fechaFormateada).ToString();
                }
                else
                {
                    fechaFormateada = DateTime.Now.ToString("dd/MM/yyyy");
                    SqlVenta.InsertParameters["Fecha_Pago"].DefaultValue = DateTime.Parse(fechaFormateada).ToString();

                }
                if (fechaFormateada.Equals(DateTime.Now.ToString("dd/MM/yyyy")))
                {
                    SqlVenta.InsertParameters["id_estado_ven"].DefaultValue = "2";
                }
                else
                {
                    SqlVenta.InsertParameters["id_estado_ven"].DefaultValue = "1";

                }
                SqlVenta.InsertParameters["fechaConfirmPago"].DefaultValue = DateTime.Now.ToString();
                SqlVenta.InsertParameters["ID_Pago"].DefaultValue = DropDownList1.SelectedValue;
                SqlVenta.InsertParameters["total"].DefaultValue = lblTotal.Text;
                SqlVenta.InsertParameters["idemp"].DefaultValue = ddlEmpleado.SelectedValue;
                SqlVenta.Insert();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar la venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void SqlVenta_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            try
            {
                if (e.AffectedRows > 0)
                {
                    int Id = Convert.ToInt32(e.Command.Parameters["ID_VENTA"].Value);
                    int facturas = CargarFacturas(Id);
                    if (Id > 0)
                    {
                        //si factura es !=0 y !=-2 entonse se cargaron correctamente, o no se agregaron nuevas facturas 
                        if (facturas != 0 && facturas != -2)
                        {
                            string mensaje = "Se registro la venta correctamente";
                            MostrarAlerta(mensaje, NotificationType.success);
                        }
                        else
                        {
                            lblResultadoOperacion.Text += "<br>" + "Se registro la venta";
                            updpLblResOpe.Update();
                        }

                    }


                    if (!CbFecha.Checked)
                    {
                        cargarMPrevntivo(Id);
                        SqlPedidosDelCliente.UpdateParameters["idPed"].DefaultValue = lblIdPed.Text;
                        SqlPedidosDelCliente.UpdateParameters["idEstado"].DefaultValue = "5";
                        SqlPedidosDelCliente.Update();
                    }
                    else
                    {
                        if (DateTime.TryParse(TbFecha.Text, out DateTime fechaIngresada))
                        {
                            DateTime fechaHoy = DateTime.Today;
                            if (fechaIngresada == fechaHoy)
                            {
                                cargarMPrevntivo(Id);
                                SqlPedidosDelCliente.UpdateParameters["idPed"].DefaultValue = lblIdPed.Text;
                                SqlPedidosDelCliente.UpdateParameters["idEstado"].DefaultValue = "5";
                                SqlPedidosDelCliente.Update();
                            }
                            else 
                            {
                                SqlPedidosDelCliente.UpdateParameters["idPed"].DefaultValue = lblIdPed.Text;
                                SqlPedidosDelCliente.UpdateParameters["idEstado"].DefaultValue = "4";
                                SqlPedidosDelCliente.Update();
                            }
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar la venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void cargarMPrevntivo(int id)
        {
            bool equipo = false;
            for (int i = 0; i < GrillaProductos.Rows.Count; i++)
            {
                if (GrillaProductos.DataKeys[i].Values["ID_TIPO"].ToString().Equals("2"))
                {
                    equipo = true;
                    break;
                }
            }

            if (equipo)
            {
                try
                {
                    SqlPedidoServicio.InsertParameters["idCli"].DefaultValue = lblIdCli.Text;
                    SqlPedidoServicio.InsertParameters["idtipo"].DefaultValue = "4";
                    SqlPedidoServicio.InsertParameters["calle"].DefaultValue = null;
                    SqlPedidoServicio.InsertParameters["altura"].DefaultValue = null;
                    SqlPedidoServicio.InsertParameters["titulo"].DefaultValue = "Venta " + lblFecha.Text;
                    SqlPedidoServicio.InsertParameters["desc"].DefaultValue = null;
                    SqlPedidoServicio.InsertParameters["fechaPed"].DefaultValue = lblFecha.Text;
                    DateTime fechaHoy = DateTime.Now;
                    DateTime fechaCon90Dias = fechaHoy.AddDays(90);
                    SqlPedidoServicio.InsertParameters["fechaSer"].DefaultValue = fechaCon90Dias.ToString();
                    SqlPedidoServicio.InsertParameters["idVenta"].DefaultValue = id.ToString();
                    SqlPedidoServicio.Insert();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al registrar el servicio preventivo: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void btnVenta_Click(object sender, EventArgs e)
        {
            CargarVenta();
        }

        protected void SqlPedidoServicio_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows > 0)
            {
                try
                {
                    int Id = Convert.ToInt32(e.Command.Parameters["@IDPedido"].Value);
                    SqlPedidoServicio.SelectParameters["idPedidoVent"].DefaultValue = lblIdPed.Text;
                    SqlPedidoServicio.DataSourceMode = SqlDataSourceMode.DataReader;
                    SqlDataReader reader;
                    reader = (SqlDataReader)SqlPedidoServicio.Select(DataSourceSelectArguments.Empty);
                    while (reader.Read())
                    {

                        SqlDetallePedidoServicio.InsertParameters["IdPed"].DefaultValue = Id.ToString();
                        SqlDetallePedidoServicio.InsertParameters["cantidad"].DefaultValue = reader["CANTIDAD"].ToString();
                        SqlDetallePedidoServicio.InsertParameters["idProd"].DefaultValue = reader["IdProd"].ToString();
                        SqlDetallePedidoServicio.InsertParameters["nombre"].DefaultValue = reader["nombreProd"].ToString();
                        SqlDetallePedidoServicio.InsertParameters["codigo"].DefaultValue = reader["COD_PROD"].ToString();
                        SqlDetallePedidoServicio.Insert();
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al registrar el detalle del servicio preventivo: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }
    }
}