using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaVenta
{


    public partial class ListaVentasCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            //LblResOpe("");
            if (!IsPostBack)
            {
                CargarVentas();
                CargarClientes();
            }
        }

        private void CargarVentas()
        {
            try
            {
                SqlDeudas.Update();
                GrillaVentas.DataSourceID = null;
                GrillaVentas.DataSource = SqlVentasTotales;
                GrillaVentas.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar las ventas: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
            Session["idCliVenta"] = "0";

        }

        private void CargarClientes()
        {
            try
            {
                GrillaClientes.DataSourceID = null;
                GrillaClientes.DataSource = SqlCliente;
                GrillaClientes.DataBind();
                UpdatePanel2.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los clientes: " + ex.Message;
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
                confirmButtonColor: '#305d6',  // Color del botón de confirmar
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

        //metodo para manejar la alerta de cancelar
        private void AlertaCancelar(string mensaje, NotificationType tipo)
        {

            string script = $@"
                Swal.fire({{
                title: '{mensaje}',
                icon: '{tipo.ToString().ToLower()}',
                showDenyButton: true,
                confirmButtonText: 'Aceptar',
                denyButtonText: `Cancelar`,
                confirmButtonColor: '#3085d6',  // Color del botón de confirmar
                denyButtonColor: '#d33'         // Color del botón de negar (Deny)
                }}).then((result) => {{
                if (result.isConfirmed) {{
                    __doPostBack('UpdatePanel1', 'Cancelar'); // Enviar al servidor
                }} else if (result.isDenied) {{
                    __doPostBack('UpdatePanel1', 'Deny'); // Enviar al servidor
                }}
                }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cancelarAlerta", script, true);
        }

        //metodo para manejar la alerta de confirmar
        private void AlertaConfirmar(string mensaje, NotificationType tipo)
        {

            string script = $@"
                Swal.fire({{
                title: '{mensaje}',
                icon: '{tipo.ToString().ToLower()}',
                showDenyButton: true,
                confirmButtonText: 'Confirmar',
                denyButtonText: `Cancelar`,
                confirmButtonColor: '#3085d6',  // Color del botón de confirmar
                denyButtonColor: '#d33'         // Color del botón de negar (Deny)
                }}).then((result) => {{
                if (result.isConfirmed) {{
                    __doPostBack('UpdatePanel1', 'Confirmar'); // Enviar al servidor
                }} else if (result.isDenied) {{
                    __doPostBack('UpdatePanel1', 'Deny'); // Enviar al servidor
                }}
                }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmarAlerta", script, true);
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
                    BorrarVenta();
                }
                else if (argument == "Confirmar")
                { ConfirmarVenta(); }
                else if (argument == "Cancelar")
                { CancelarVenta(); }
                Session["IDVen"] = 0;
                Session["Indice"] = 0;
            }
        }

        /*private void LblResOpe(string mensaje)
        {
            lblResultadoOperacion.Text = mensaje;
            updpLblResOpe.Update();
        }*/


        /*
    apartir de aca estan los metodos para: Confirmar, cancelar y borrar
    debajo de estos metodos se encuentra el metodo rowcommand el cual los activa
    la razon de separarlos de los metodos vender y modificar, es que los tres
    metodos anteriores tiene una pregunta para confirmar la accion respectiva
*/

        //metodo Confirmar

        private void ConfirmarVenta()
        {
            string idVenta = Session["IDVen"].ToString();
            int index=int.Parse(Session["Indice"].ToString());
            try
            {
                
                SqlOperacionesV.UpdateParameters["idVenta"].DefaultValue = idVenta;
                SqlOperacionesV.UpdateParameters["idEstado"].DefaultValue = "2";
                SqlOperacionesV.Update();
                CargarVentas();
                UpdatePanel1.Update();
                SqlPedidosDelCliente.UpdateParameters["idPed"].DefaultValue = GrillaVentas.DataKeys[index].Values["ID_PEDIDO"].ToString();
                SqlPedidosDelCliente.UpdateParameters["idEstado"].DefaultValue = "5";
                SqlPedidosDelCliente.Update();
                string mensaje = "Se abono la venta con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al abonar la venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
            try
            {
                SqlOperacionesV.SelectParameters["idVenta"].DefaultValue = idVenta;
                SqlOperacionesV.SelectParameters["idPedido"].DefaultValue = GrillaVentas.DataKeys[index].Values["ID_PEDIDO"].ToString();

                DataView dataView = (DataView)SqlOperacionesV.Select(DataSourceSelectArguments.Empty);

                if (dataView.Count > 0)
                {
                    int resultado = Convert.ToInt32(dataView[0][0]);

                    if (resultado == 1)
                    {
                        SqlPedidoServicio.InsertParameters["idCli"].DefaultValue = GrillaVentas.DataKeys[index].Values["ID_CLIENTE"].ToString();
                        SqlPedidoServicio.InsertParameters["idtipo"].DefaultValue = "4";
                        SqlPedidoServicio.InsertParameters["calle"].DefaultValue = null;
                        SqlPedidoServicio.InsertParameters["altura"].DefaultValue = null;
                        SqlPedidoServicio.InsertParameters["titulo"].DefaultValue = "Venta " + GrillaVentas.DataKeys[index].Values["Fecha_Confirmacion"].ToString();
                        SqlPedidoServicio.InsertParameters["desc"].DefaultValue = null;
                        SqlPedidoServicio.InsertParameters["fechaPed"].DefaultValue = GrillaVentas.DataKeys[index].Values["Fecha_Confirmacion"].ToString();
                        DateTime fecha = DateTime.Parse(SqlPedidoServicio.InsertParameters["fechaPed"].DefaultValue = GrillaVentas.DataKeys[index].Values["Fecha_Confirmacion"].ToString());
                        fecha.AddDays(90);
                        SqlPedidoServicio.InsertParameters["fechaSer"].DefaultValue = fecha.ToString();
                        SqlPedidoServicio.InsertParameters["idVenta"].DefaultValue = idVenta;
                        lblIdPedido.Text = GrillaVentas.DataKeys[index].Values["ID_PEDIDO"].ToString();
                        SqlPedidoServicio.Insert();
                    }
                }

            }
            catch (Exception ex)
            {
                string mensaje= "Ocurrió un error al cargar el servicio preventivo: " + ex;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        //metodo Cancelar

        private void CancelarVenta()
        {
            try
            {
                int index = int.Parse(Session["Indice"].ToString());
                string idVenta = Session["IDVen"].ToString();
                SqlOperacionesV.UpdateParameters["idVenta"].DefaultValue = idVenta;
                SqlOperacionesV.UpdateParameters["idEstado"].DefaultValue = "3";
                SqlOperacionesV.Update();
                CargarVentas();
                UpdatePanel1.Update();
                SqlPedidosDelCliente.UpdateParameters["idPed"].DefaultValue = GrillaVentas.DataKeys[index].Values["ID_PEDIDO"].ToString();
                SqlPedidosDelCliente.UpdateParameters["idEstado"].DefaultValue = "9";
                SqlPedidosDelCliente.Update();
                string mensaje = "Se cancelo la venta con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cancelar la venta: " + ex.Message;
                MostrarAlerta (mensaje, NotificationType.error);
            }
        }

        //borrar pedido

        private void BorrarVenta()
        {
            try
            {
                int index = int.Parse(Session["Indice"].ToString());
                string idVenta = Session["IDVen"].ToString();
                string idPedido = GrillaVentas.DataKeys[index].Values["ID_PEDIDO"].ToString();
                SqlOperacionesV.DeleteParameters["idVenta"].DefaultValue = idVenta;
                SqlOperacionesV.Delete();
                SqlPedidosDelCliente.DeleteParameters["idPed"].DefaultValue= idPedido;
                SqlPedidosDelCliente.Delete();
                CargarVentas();
                UpdatePanel1.Update();
                string mensaje = "Se borro la venta con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al intentar borrar la venta: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        protected void GrillaVentas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int ver = 0;
            // int index = 0;
            string idPedido = "";
            string idVenta = "";
            string idPago = "";
            string fechaVenta = "";
            string fechaPago = "";
            string fechaConfirmacionPago = "";
            string total = "";
            string estadoVenta = "";
            string empleado = "";
            if (e.CommandName == "VerVenta" || e.CommandName == "Modificar"
                || e.CommandName == "Borrar" || e.CommandName == "Abonar" || e.CommandName == "Cancelar")
            {
                ver = 0;

                GridViewRow row = GrillaVentas.Rows[index];
                idPedido = GrillaVentas.DataKeys[index].Values["ID_PEDIDO"].ToString();
                idVenta = GrillaVentas.DataKeys[index].Values["ID_VENTA"].ToString();
                idPago = GrillaVentas.DataKeys[index].Values["ID_Pago"].ToString();
                fechaVenta = GrillaVentas.DataKeys[index].Values["FECHA_VENTA"].ToString();
                fechaPago = GrillaVentas.DataKeys[index].Values["Fecha_Pago"].ToString();
                fechaConfirmacionPago = GrillaVentas.DataKeys[index].Values["Fecha_Confirmacion"].ToString();
                total = GrillaVentas.DataKeys[index].Values["Total"].ToString();
                estadoVenta = GrillaVentas.DataKeys[index].Values["id_estado_ven"].ToString();
                empleado= GrillaVentas.DataKeys[index].Values["idEmpleado"].ToString();
            }

            if (e.CommandName == "VerVenta")
            {
                ver = 1;
                // se redirecciona a la página vermodificarventa, dependiendo de si quiere ver o modificar los datos
                try
                {
                    Response.Redirect($"~/PantallaVenta/VerModificarVenta.aspx?ID_PEDIDO={idPedido}&ID_Pago={idPago}&FECHA_VENTA={fechaVenta}" +
                        $"&Fecha_Pago={fechaPago}&Total={total}&id_estado_ven={estadoVenta}&ID_VENTA={idVenta}&Vista={ver}&empleado={empleado}");
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al intentar ver la venta: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else if (e.CommandName == "Modificar")
            {
                if (estadoVenta.Equals("4") || estadoVenta.Equals("1"))
                {
                    try
                    {
                        Response.Redirect($"~/PantallaVenta/VerModificarVenta.aspx?ID_PEDIDO={idPedido}&ID_Pago={idPago}&FECHA_VENTA={fechaVenta}" +
                                        $"&Fecha_Pago={fechaPago}&Total={total}&id_estado_ven={estadoVenta}&ID_VENTA={idVenta}&Vista={ver}");
                    }
                    catch (Exception ex)
                    {
                        string mensaje = "Ocurrió un error al intentar modificar la venta: " + ex.Message;
                        MostrarAlerta(mensaje, NotificationType.error);
                    }
                }
            }
            else if (e.CommandName == "Borrar")
            {
                if (estadoVenta.Equals("3"))
                {
                    Session["IDVen"] = idVenta;
                    Session["Indice"] = index;
                    string mensaje = "¿Está seguro de querer borrar la venta?";
                    AlertaBorrar(mensaje, NotificationType.warning);
                }
            }
            else if (e.CommandName == "Abonar")
            {
                if (estadoVenta.Equals("1") || estadoVenta.Equals("4"))
                {
                    Session["IDVen"] = idVenta;
                    Session["Indice"] = index;
                    string mensaje = "¿Está seguro de querer abonar la venta?";
                    AlertaConfirmar(mensaje, NotificationType.warning);
                }
            }
            else if (e.CommandName == "Cancelar")
            {
                if (estadoVenta.Equals("1"))
                {
                    Session["IDVen"] = idVenta;
                    Session["Indice"] = index;
                    string mensaje = "¿Está seguro de querer cancelar la venta?";
                    AlertaCancelar(mensaje, NotificationType.warning);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!txtBuscar.Text.Equals(""))
            {
                try
                {
                    // Cambiar el parámetro de búsqueda antes de enlazar la nueva fuente de datos
                    SqlBuscarCliente.SelectParameters["Nombre"].DefaultValue = txtBuscar.Text;

                    // Desvincular cualquier DataSource previamente asignado
                    GrillaClientes.DataSourceID = null;
                    GrillaClientes.DataSource = SqlBuscarCliente;  // Asignar directamente el SqlDataSource
                    GrillaClientes.DataBind();  // Enlazar los datos
                    UpdatePanel2.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al buscar al cliente: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                // Desvincular cualquier DataSource previamente asignado
                CargarClientes();
                Session["idCliVenta"] = "0";
                gvDeuda.Enabled = false;
                gvDeuda.Visible = false;
                btnVerdeudas.Enabled = false;
                btnVerdeudas.Visible = false;
                UpdatePanel2.Update();
                CargarVentas();
                UpdatePanel1.Update();
                UpdatePanel4.Update();
            }
        }

        protected void GrillaClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                btnVerdeudas.Enabled = true;
                btnVerdeudas.Visible = true;
                lblId.Text = this.GrillaClientes.DataKeys[GrillaClientes.SelectedIndex].Values["ID_CLIENTE"].ToString();
                Session["idCliVenta"] = this.GrillaClientes.DataKeys[GrillaClientes.SelectedIndex].Values["ID_CLIENTE"].ToString();

                SqlVentas.SelectParameters["idCli"].DefaultValue = this.GrillaClientes.DataKeys[GrillaClientes.SelectedIndex].Values["ID_CLIENTE"].ToString();

                GrillaVentas.DataSourceID = "SqlVentas";
                GrillaVentas.AllowPaging = false;
                GrillaVentas.DataBind();
                GrillaVentas.AllowPaging = true;
                UpdatePanel1.Update();
            } catch (Exception ex) { throw; }
            
        }


        protected void GrillaVentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            /*try
            {
                // Cambia la página
                GrillaVentas.PageIndex = e.NewPageIndex;
                GrillaVentas.DataSourceID = null;
                GrillaVentas.DataSource = null;
                // Selecciona la fuente de datos correcta dependiendo de la condición
                if (Session["idCliVenta"].Equals("0"))
                {
                    // Carga ventas de acuerdo con el método CargarVentas
                    CargarVentas(); // Asegúrate de que este método establezca correctamente la fuente de datos y haga el DataBind()
                }
                else
                {
                    // Asigna SqlVentas como la fuente de datos y hace DataBind
                    GrillaVentas.DataSource = SqlVentas;
                    GrillaVentas.DataBind(); // Asegúrate de que el parámetro esté bien configurado

                }

                // Actualiza el UpdatePanel
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar de página la tabla de ventas: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }*/

            GrillaVentas.PageIndex = e.NewPageIndex;
            if (lblId.Text.Equals(""))
            {
                if (tbBuscaPedido.Text.Equals(""))
                {
                    CargarVentas();//////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else
                {
                    CargarVentasID();
                }

                UpdatePanel1.Update();
            }
            else
            {
                try
                {
                    SqlVentas.SelectParameters["idCli"].DefaultValue = this.GrillaClientes.DataKeys[GrillaClientes.SelectedIndex].Values["ID_CLIENTE"].ToString();

                    GrillaVentas.DataSourceID = "SqlVentas";
                    GrillaVentas.AllowPaging = false;
                    GrillaVentas.DataBind();
                    GrillaVentas.AllowPaging = true;
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al cambiar de página en la tabla pedidos: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void GrillaClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaClientes.PageIndex = e.NewPageIndex;
            try
            {
                if (!txtBuscar.Text.Equals(""))
                {

                    GrillaClientes.DataSource = SqlBuscarCliente;
                    GrillaClientes.DataBind();
                    UpdatePanel2.Update();
                }
                else { CargarClientes(); }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar de página la tabla de clientes: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnVerdeudas_Click(object sender, EventArgs e)
        {
            try
            {
                SqlDeudas.SelectParameters["idCli"].DefaultValue = Session["idCliVenta"].ToString();
                gvDeuda.DataSourceID = null;
                gvDeuda.DataSource = SqlDeudas;
                gvDeuda.DataBind();
                gvDeuda.Visible = true;
                gvDeuda.Enabled = true;
                UpdatePanel4.Update();
                if (gvDeuda.Rows.Count == 0)
                {
                    string mensaje = "El cliente no cuenta con deudas";
                    MostrarAlerta(mensaje, NotificationType.error);
                }
                else 
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModalDeudas();", true);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al ver las deudas del cliente: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void gvDeuda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDeuda.PageIndex = e.NewPageIndex;
                gvDeuda.DataSource = SqlDeudas;
                gvDeuda.DataBind();
                UpdatePanel4.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar de página la tabla de deudas: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void SqlPedidoServicio_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows > 0)
            {
                try
                {
                    int Id = Convert.ToInt32(e.Command.Parameters["@IDPedido"].Value);
                    SqlPedidoServicio.SelectParameters["idPedidoVent"].DefaultValue = lblIdPedido.Text;
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

        private void CargarVentasID()
        {
            try
            {
                SqlVentasId.SelectParameters["idVen"].DefaultValue = tbBuscaPedido.Text;

                GrillaVentas.DataSourceID = null;
                GrillaVentas.DataSource = SqlVentasId;
                GrillaVentas.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar la venta por su id: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CargarVentasIDPedido()
        {
            try
            {
                SqlPedidosDelCliente.SelectParameters["Idped"].DefaultValue = tbBuscaPedido.Text;

                GrillaVentas.DataSourceID = null;
                GrillaVentas.DataSource = SqlPedidosDelCliente;
                GrillaVentas.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar la venta por su pedido: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnBuscarPedido_Click(object sender, EventArgs e)
        {
            if (tbBuscaPedido.Text.Equals(""))
            {
                CargarVentas();
            }
            else
            {
                lblId.Text = "";
                if (RbBuscIdVenta.Checked)
                {
                    CargarVentasID();
                }
                else { CargarVentasIDPedido(); }
                
                
            }
        }

        protected void btnCerrarDudas_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "cerarDeudas();", true);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cerrar la ventana: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
    }
}