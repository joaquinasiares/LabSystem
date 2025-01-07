using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaPedidoServivio
{
    public partial class NuevoPedidoServicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            udpEstado("");
            if (!IsPostBack)
            {
                lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblIdCli.Text = Request.QueryString["id"];
                lblNomCli.Text = Request.QueryString["nombre"];
                lblRs.Text = Request.QueryString["rs"];
                lblTipoServ.Text = "3";
                CrearDtProd();
                CargarProductos();
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
                SqlProductosCliente.SelectParameters["idCli"].DefaultValue = lblIdCli.Text;
                GrillaProductos.DataSource = SqlProductosCliente;
                GrillaProductos.DataBind();
                UpdatePanel3.Update();
            }catch (Exception ex) 
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

        private void CrearDtProd()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID_PRODUCTO");
            dt.Columns.Add("COD_PROD");
            dt.Columns.Add("DESCRIPCION");
            dt.Columns.Add("Cantidad");

            GrillaSeleccionados.DataSource = dt;
            GrillaSeleccionados.DataBind();
            Session["seleccionPS"] = dt;
            UpdatePanel3.Update();
        }

        protected void GrillaProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            numberCantidad.Enabled = true;
            btnSelectEqui.Enabled = true;
            updAgregarEq.Update();
        }


        private void InsertarPedido()
        {
            try
            {
                SqlPedido.InsertParameters["idCli"].DefaultValue = lblIdCli.Text;
                SqlPedido.InsertParameters["idtipo"].DefaultValue = lblTipoServ.Text;
                SqlPedido.InsertParameters["calle"].DefaultValue = tbCalle.Text;
                SqlPedido.InsertParameters["altura"].DefaultValue = tbAltura.Text;
                SqlPedido.InsertParameters["titulo"].DefaultValue = tbTitulo.Text;
                SqlPedido.InsertParameters["desc"].DefaultValue = tbDesc.Text;
                SqlPedido.InsertParameters["fechaPed"].DefaultValue = lblFecha.Text;
                string fechaFormateada = "";
                DateTime fechaSeleccionada;
                DateTime.TryParse(TbFecha.Text, out fechaSeleccionada);
                fechaFormateada = fechaSeleccionada.ToString("dd/MM/yyyy");
                SqlPedido.InsertParameters["fechaSer"].DefaultValue = DateTime.Parse(fechaFormateada).ToString();
                SqlPedido.InsertParameters["idVenta"].DefaultValue = null;

                DateTime hoy = DateTime.Now;;
                if (fechaSeleccionada >= hoy)
                {
                    SqlPedido.Insert();
                }
                else
                {
                    string mensaje = "La fecha no puede ser menor a hoy";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
            }
            catch (Exception e)
            {
                string mensaje = "Ocurrio un error al insertar el pedido, error: " + e.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }


        protected void SqlPedido_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            // Verificar que el comando contiene parámetros
            if (e.Command.Parameters.Count > 0)
            {
                int Id = Convert.ToInt32(e.Command.Parameters["@IDPedido"].Value);
                DataTable dt = (DataTable)Session["seleccionPS"];

                try
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        var idProducto = fila["ID_PRODUCTO"];
                        var codigoProducto = fila["COD_PROD"];
                        var descripcion = fila["DESCRIPCION"];
                        var cantidad = fila["Cantidad"];

                        SqlDetallePedido.InsertParameters["IdPed"].DefaultValue = Id.ToString();
                        SqlDetallePedido.InsertParameters["cantidad"].DefaultValue = cantidad.ToString();
                        SqlDetallePedido.InsertParameters["idProd"].DefaultValue = idProducto.ToString();
                        SqlDetallePedido.InsertParameters["nombre"].DefaultValue = descripcion.ToString();
                        SqlDetallePedido.InsertParameters["codigo"].DefaultValue = codigoProducto.ToString();
                        SqlDetallePedido.Insert();
                    }
                    string mensaje = "Se inserto el pedido con exito";
                    MostrarAlerta(mensaje, NotificationType.success);
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
                udpEstado(mensaje);
            }
        }

        protected void btnAgregarP_Click(object sender, EventArgs e)
        {
            if (GrillaSeleccionados.Rows.Count > 0)
            {
                if (tbAltura.Text.Equals("") || tbCalle.Text.Equals("")
                    || tbTitulo.Text.Equals("") || tbDesc.Text.Equals(""))
                {
                    string mensaje = "Complete los campos vacios";//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
                else 
                {
                    InsertarPedido();
                }
                
            }
            else
            {
                string mensaje = "seleccione un producto";
                MostrarAlerta(mensaje, NotificationType.warning);
            }
        }

        protected void GrillaSeleccionados_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Obtener el DataTable desde la sesión
            DataTable dt = (DataTable)Session["seleccionPS"];

            if (dt != null)
            {
                // Obtener el índice de la fila que se está eliminando
                int index = e.RowIndex;
                dt.Rows[index].Delete();
                GrillaSeleccionados.DataSource = dt;
                GrillaSeleccionados.DataBind();
                Session["seleccionPS"] = dt;

                UpdatePanel1.Update();
            }
        }

        protected void btnSelectEqui_Click(object sender, EventArgs e)
        {
            try
            {
                if (!numberCantidad.Text.Equals("0") && !numberCantidad.Text.Equals(""))
                {
                    DataTable dt = (DataTable)Session["seleccionPS"];
                    bool nuevoProducto = true;
                    string id = this.GrillaProductos.DataKeys[GrillaProductos.SelectedIndex].Values["ID_PRODUCTO"].ToString();

                    for (int i = 0; i < GrillaSeleccionados.Rows.Count; i++)
                    {
                        if (GrillaSeleccionados.DataKeys[i].Values["ID_PRODUCTO"].ToString().Equals(id))
                        {
                            int cantAntes = int.Parse(GrillaSeleccionados.Rows[i].Cells[3].Text);
                            int total = cantAntes + int.Parse(numberCantidad.Text);

                            int cantiEquiposDelCLient = int.Parse(GrillaProductos.Rows[GrillaProductos.SelectedIndex].Cells[3].Text);

                            //si se intentan agregar mas equipos de los que tiene el cliente, "total" se vuelve el maximo de equipos que
                            //tiene el cliente
                            if (total > cantiEquiposDelCLient)
                            {
                                total = cantiEquiposDelCLient;
                            }
                            dt.Rows[i]["Cantidad"] = total.ToString();

                            nuevoProducto = false;
                            break;

                        }
                    }

                    if (nuevoProducto)
                    {

                        int total = int.Parse(numberCantidad.Text);

                        int cantiEquiposDelCLient = int.Parse(GrillaProductos.Rows[GrillaProductos.SelectedIndex].Cells[3].Text);

                        //si se intentan agregar mas equipos de los que tiene el cliente, "total" se vuelve el maximo de equipos que
                        //tiene el cliente
                        if (total > cantiEquiposDelCLient)
                        {
                            total = cantiEquiposDelCLient;
                        }

                        dt.Rows.Add(
                                GrillaProductos.DataKeys[GrillaProductos.SelectedIndex].Values["ID_PRODUCTO"].ToString(),
                                GrillaProductos.Rows[GrillaProductos.SelectedIndex].Cells[1].Text,
                                GrillaProductos.Rows[GrillaProductos.SelectedIndex].Cells[2].Text,
                                total
                            );
                    }

                    GrillaSeleccionados.DataSource = dt;
                    GrillaSeleccionados.DataBind();
                    Session["seleccionPS"] = dt;
                    UpdatePanel1.Update();
                    numberCantidad.Text = "1";
                    numberCantidad.Enabled = false;
                    btnSelectEqui.Enabled = false;
                    updAgregarEq.Update();
                }
                else
                {
                    numberCantidad.Text = "1";
                    string mensaje = "La cantidad minima de equipos por cliente debe de ser al menos de 1";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error al agregar un equipo, error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.warning);
            }
        }

        protected void GrillaProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaProductos.PageIndex = e.NewPageIndex;
                GrillaProductos.DataSource = SqlProductosCliente;
                GrillaProductos.DataBind();
                UpdatePanel3.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar de página en la tabla productos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.warning);
            }
        }
    }
}