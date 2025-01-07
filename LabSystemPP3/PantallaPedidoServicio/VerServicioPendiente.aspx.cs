using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaPedidoServicio
{
    public partial class VerServicioPendiente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            udpEstado("");
            if (!IsPostBack)
            {
                lblIDPedido.Text = Request.QueryString["id"];
                lblVerMod.Text = Request.QueryString["VerModificar"];

                CargarDatos();

                CrearDtProd();
                CargarProductos();
                CargarEquipos();
            }
        }

        private void udpEstado(string mensaje)
        {
            lblEstado.Text = mensaje;
            updpEstado.Update();
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

        //metodo para manejar la alerta de borrar
        private void AlertaBorrar(string mensaje, NotificationType tipo)
        {

            string script = $@"
                Swal.fire({{
                title: '{mensaje}',
                icon: '{tipo.ToString().ToLower()}',
                showDenyButton: true,
                confirmButtonText: 'Borrar',
                denyButtonText: `Cancelar`,
                confirmButtonColor: '#3085d6',  // Color del botón de confirmar
                denyButtonColor: '#d33'         // Color del botón de negar (Deny)
                }}).then((result) => {{
                if (result.isConfirmed) {{
                    __doPostBack('UpdatePanel1', 'Borrar'); // Enviar al servidor
                }} else if (result.isDenied) {{
                    __doPostBack('UpdatePanel1', 'Deny'); // Enviar al servidor
                }}
                }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "borrarAlerta", script, true);
        }

        //metodo para confirmar el borrado
        private void ConfirmacionDeOpciones()
        {
            string target = Request.Form["__EVENTTARGET"];
            string argument = Request.Form["__EVENTARGUMENT"];

            if (target == "UpdatePanel1") // Asegúrate de que coincida con el UpdatePanel
            {
                if (argument == "Borrar")
                {
                    //BorrarProveedor();
                    Session["idProv"] = "0";
                }
                else { Session["idProv"] = "0"; }
            }
        }

        private void CargarEquipos()
        {
            SqlProductosCliente.SelectParameters["idCli"].DefaultValue = lblIdCli.Text;
            UpdatePanel3.Update();
        }
        private void CrearDtProd()
        {
            try
            {
                //data table para listar los equipos
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_DETALLE_PEDIDO");
                dt.Columns.Add("IdProd");
                dt.Columns.Add("codigo");
                dt.Columns.Add("DESCRIPCION");
                dt.Columns.Add("CANTIDAD");

                GrillaSeleccionados.DataSource = dt;
                GrillaSeleccionados.DataBind();
                Session["seleccionPS"] = dt;
                UpdatePanel3.Update();

                DataTable dtBorrar = new DataTable();
                dtBorrar.Columns.Add("ID");
                Session["dtBorrar"] = dtBorrar;
            }catch(Exception ex) 
            { 
                string mensaje= "Ocurrió un error: "+ex.Message;
                MostrarAlerta(mensaje,NotificationType.error);
            }
        }
        private void CargarProductos()
        {
            try
            {
                DataTable dt = (DataTable)Session["seleccionPS"];
                SqlPedidodetalle.SelectParameters["ID"].DefaultValue = lblIDPedido.Text;

                DataView dv = (DataView)SqlPedidodetalle.Select(DataSourceSelectArguments.Empty);


                foreach (DataRowView drv in dv)
                {
                    DataRow row = dt.NewRow();
                    row["ID_DETALLE_PEDIDO"] = drv["ID_DETALLE_PEDIDO"];
                    row["IdProd"] = drv["IdProd"];
                    row["codigo"] = drv["codigo"];
                    row["DESCRIPCION"] = drv["DESCRIPCION"];
                    row["CANTIDAD"] = drv["CANTIDAD"];
                    dt.Rows.Add(row);
                }

                GrillaSeleccionados.DataSource = dt;
                GrillaSeleccionados.DataBind();
                Session["seleccionPS"] = dt;

                UpdatePanel1.Update();
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

                    if (lblVerMod.Text.Equals("Ver"))
                    {
                        lblTitulo.Text = "Ver pedido de servicio";
                        TbFecha.Enabled = false;
                        tbCalle.Enabled = false;
                        tbAltura.Enabled = false;
                        tbTitulo.Enabled = false;
                        tbDesc.Enabled = false;
                        btnAgregarP.Enabled = false;
                        btnAgregarP.Visible = false;
                        GrillaProductos.Enabled = false;
                        GrillaProductos.Visible = false;
                        GrillaSeleccionados.Enabled = false;
                        Label8.Visible = false;
                        numberCantidad.Visible = false;
                        numberCantidad.Enabled = false;
                        btnSelectEqui.Enabled = false;
                        btnSelectEqui.Visible = false;
                        //lblproductoDelCLiente.Visible = false;
                    }
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


        private void ModificarPedido()
        {
            try
            {



                SqlPedido.UpdateParameters["idPedido"].DefaultValue = lblIDPedido.Text;
                SqlPedido.UpdateParameters["calle"].DefaultValue = tbCalle.Text;
                SqlPedido.UpdateParameters["altura"].DefaultValue = tbAltura.Text;
                SqlPedido.UpdateParameters["titulo"].DefaultValue = tbTitulo.Text;
                SqlPedido.UpdateParameters["desc"].DefaultValue = tbDesc.Text;
                string fechaFormateada = "";
                DateTime fechaSeleccionada;
                DateTime.TryParse(TbFecha.Text, out fechaSeleccionada);
                fechaFormateada = fechaSeleccionada.ToString("dd/MM/yyyy");
                SqlPedido.UpdateParameters["fechaSer"].DefaultValue = DateTime.Parse(fechaFormateada).ToString();

                SqlPedido.Update();

                DataTable dtBorrar = (DataTable)Session["dtBorrar"];
                foreach (DataRow fila in dtBorrar.Rows)
                {
                    if (!fila["ID"].ToString().Equals("0")) 
                    {
                        SqlPedidodetalle.DeleteParameters["idDetalle"].DefaultValue = fila["ID"].ToString();
                        SqlPedidodetalle.Delete();
                    }

                }


                DataTable dt = (DataTable)Session["seleccionPS"];
                foreach (DataRow fila in dt.Rows)
                {
                    int idDetalle = int.Parse(fila["ID_DETALLE_PEDIDO"].ToString());
                    var idProducto = fila["IdProd"];
                    var codigoProducto = fila["codigo"];
                    var descripcion = fila["DESCRIPCION"];
                    var cantidad = fila["Cantidad"];
                    if (idDetalle == 0)
                    {

                        SqlPedidodetalle.InsertParameters["IdPed"].DefaultValue = lblIDPedido.Text;
                        SqlPedidodetalle.InsertParameters["cantidad"].DefaultValue = cantidad.ToString();
                        SqlPedidodetalle.InsertParameters["idProd"].DefaultValue = idProducto.ToString();
                        SqlPedidodetalle.InsertParameters["nombre"].DefaultValue = descripcion.ToString();
                        SqlPedidodetalle.InsertParameters["codigo"].DefaultValue = codigoProducto.ToString();
                        SqlPedidodetalle.Insert();
                    }
                    else
                    {

                        SqlPedidodetalle.UpdateParameters["idPedido"].DefaultValue = lblIDPedido.Text;
                        SqlPedidodetalle.UpdateParameters["idDetalle"].DefaultValue = idDetalle.ToString();
                        SqlPedidodetalle.UpdateParameters["Cantidad"].DefaultValue = cantidad.ToString();
                        SqlPedidodetalle.Update();
                    }
                }

                
                string mensaje = "Se modifico el pedido con exito";
                Response.Redirect($"~/PantallaPedidoServicio/ListadoPedidoServicios.aspx?mensaje={Server.UrlEncode(mensaje)}");
                MostrarAlerta(mensaje, NotificationType.error);
            }
            catch (Exception e)
            {
                string mensaje = "Ocurrio un error al modificar el pedido, error: " + e.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }

        protected void GrillaProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            numberCantidad.Enabled = true;
            btnSelectEqui.Enabled = true;
            updAgregarEq.Update();
        }

        protected void GrillaSeleccionados_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;
            try
            {
                // Obtener el DataTable desde la sesión
                DataTable dt = (DataTable)Session["seleccionPS"];
                DataTable dtBorrar = (DataTable)Session["dtBorrar"];

                if (dt != null)
                {
                    string idequipoEliminar = GrillaSeleccionados.DataKeys[e.RowIndex]["IdProd"].ToString();
                    DataRow filaABorrar= dt.AsEnumerable().FirstOrDefault(fila => fila["IdProd"].ToString() == idequipoEliminar);
                    if (filaABorrar != null)
                    {
                        DataRow dr = dtBorrar.NewRow();
                        string id= dt.Rows[index]["ID_DETALLE_PEDIDO"].ToString();
                        if (id.IsNullOrWhiteSpace())
                        {
                            dr["ID"] = "0";
                        }
                        else 
                        {
                            dr["ID"] = id;
                        }
                        dtBorrar.Rows.Add(dr);
                        Session["dtBorrar"] = dtBorrar;
                        dt.Rows.Remove(filaABorrar);

                        GrillaSeleccionados.DataSource = dt;
                        GrillaSeleccionados.DataBind();
                        Session["seleccionPS"] = dt;

                        UpdatePanel1.Update();
                    }
                    else 
                    {
                        string mensaje = "No se encontró el equipo a eliminar en la lista de equipos seleccionado.";
                        MostrarAlerta(mensaje, NotificationType.error);
                    }

                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error al eliminar el detalle, error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnAgregarP_Click(object sender, EventArgs e)
        {
            ModificarPedido();
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow fila = dt.Rows[i];
                        if (fila["IdProd"].ToString().Equals(id))
                        {
                            int cantAntes = int.Parse(fila["CANTIDAD"].ToString());
                            int total = cantAntes + int.Parse(numberCantidad.Text);

                            int cantiEquiposDelCLient = int.Parse(GrillaProductos.Rows[GrillaProductos.SelectedIndex].Cells[3].Text);

                            //si se intentan agregar mas equipos de los que tiene el cliente, "total" se vuelve el maximo de equipos que
                            //tiene el cliente
                            if (total > cantiEquiposDelCLient)
                            {
                                total = cantiEquiposDelCLient;
                            }
                            dt.Rows[i]["CANTIDAD"] = total.ToString();

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
                                "0",
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
                    string mensaje = "La cantidad minima de equipos por cliente debe de ser al menos de 1";
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error al agregar un equipo, error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void GrillaSeleccionados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaSeleccionados.PageIndex = e.NewPageIndex;
            DataTable dt = (DataTable)Session["seleccionPS"];
            GrillaSeleccionados.DataSource = dt;
            GrillaSeleccionados.DataBind();
            UpdatePanel1.Update();
        }

        protected void GrillaProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaProductos.PageIndex = e.NewPageIndex;
            CargarEquipos();
        }
    }
}