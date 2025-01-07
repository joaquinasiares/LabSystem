using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaPedidoServivio
{
    public partial class ListadoPedidoServicios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            updpLblErrorCli("");
            if (!IsPostBack)
            {
                lblIdCli.Text = "";
                UPPCli.Update();
                CargarClientes();
                CargarPedidos();
            }
        }
        private void updpLblErrorCli(string mensaje)
        {
            lblErrorCliente.Text = mensaje;
            updpLblErrorCliente.Update();
        }

        private void CargarPedidos()
        {
            try
            {
                GrillaPedidos.DataSource = SqlPedido;
                GrillaPedidos.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los pedidos de servicio: " + ex.Message;
                updpLblErrorCli(mensaje);
            }
        }
        private void CargarClientes()
        {
            try
            {
                GrillaClientes.DataSource = SqlCliente;
                GrillaClientes.DataBind();
                UpdatePanel3.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error al mostrar a los clientes: " + ex.Message;
                updpLblErrorCli(mensaje);
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

        //metodo para confirmar el borrado
        private void ConfirmacionDeOpciones()
        {
            string target = Request.Form["__EVENTTARGET"];
            string argument = Request.Form["__EVENTARGUMENT"];

            if (target == "UpdatePanel1") // Asegúrate de que coincida con el UpdatePanel
            {
                if (argument == "Borrar")
                {
                    BorrarPedido();
                }
                else if (argument == "Cancelar")
                { CancelarPedido(); }
                Session["IdPedidoS"] = 0;
            }
        }

        private void BuscarCliente()
        {
            if (txtBuscar.Text.Equals(""))
            {
                try
                {
                    GrillaClientes.DataSource = SqlCliente;
                    GrillaClientes.DataBind();
                    UpdatePanel3.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrio un error al mostrar a los clientes: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                try
                {
                    SqlBuscarCliente.SelectParameters["Nombre"].DefaultValue = txtBuscar.Text;
                    GrillaClientes.DataSource = SqlBuscarCliente;
                    GrillaClientes.DataBind();
                    UpdatePanel3.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrio un error al buescar al cliente: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarCliente();
        }

        protected void GrillaClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblIdCli.Text = GrillaClientes.DataKeys[GrillaClientes.SelectedIndex].Values["ID_CLIENTE"].ToString();
            lblNom.Text = GrillaClientes.Rows[GrillaClientes.SelectedIndex].Cells[2].Text;
            lblRs.Text = GrillaClientes.Rows[GrillaClientes.SelectedIndex].Cells[3].Text;
            UPPCli.Update();
            BtnNuevoPedido.Enabled = true;
            udpNuevoServ.Update();


            SqlPedidosDelCliente.SelectParameters["idCli"].DefaultValue = lblIdCli.Text;
            GrillaPedidos.DataSourceID = "SqlPedidosDelCliente";
            GrillaPedidos.AllowPaging = false;
            GrillaPedidos.DataBind();
            GrillaPedidos.AllowPaging = true;
            UpdatePanel1.Update();
        }

        private void NuevoPedidoServicio()
        {
            try
            {

                Response.Redirect($"NuevoPedidoServicio.aspx?id={lblIdCli.Text}&nombre={lblNom.Text}&rs={lblRs.Text}");
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrio un error al intentar realizar un nuevo pedido " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }

        protected void BtnNuevoPedido_Click(object sender, EventArgs e)
        {
            if (lblIdCli.Text.Equals("0"))
            {
                string mensaje = "Seleccione un cliente";
                MostrarAlerta(mensaje, NotificationType.warning);
                Console.WriteLine( mensaje );
            }
            else
            {
                NuevoPedidoServicio();
            }
        }


        /*
           apartir de aca estan los metodos para: cancelar y borrar
           debajo de estos metodos se encuentra el metodo rowcommand el cual los activa
           la razon de separarlos de los metodos confirmar y modificar y ver, es que los dos
           metodos anteriores (cancelar y borrar) tiene una pregunta para confirmar la accion respectiva
       */

        //metodo Cancelar

        private void CancelarPedido()
        {
            try
            {
                string id = Session["IdPedidoS"].ToString();
                SqlPedido.UpdateParameters["idPed"].DefaultValue = id;
                SqlPedido.UpdateParameters["idEstado"].DefaultValue = "3";
                SqlPedido.Update();
                GrillaPedidos.DataBind();
                UpdatePanel1.Update();

                string mensaje = "Se cancelo el pedido de servicio con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cancelar el pedido: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        //borrar pedido

        private void BorrarPedido()
        {
            try
            {
                string id = Session["IdPedidoS"].ToString();
                SqlPedido.DeleteParameters["idPed"].DefaultValue = id;
                SqlPedido.Delete();
                GrillaPedidos.DataBind();
                UpdatePanel1.Update();
                string mensaje = "Se borro el pedido de servicio con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al borrar el pedido: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }



        protected void GrillaPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Confirmar")
            {
                try
                {
                    string id = GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString();
                    int estado = int.Parse(GrillaPedidos.DataKeys[index].Values["ID_ESTADO_PEDIDO"].ToString());
                    if (estado == 6)
                    {
                        Response.Redirect($"~/PantallaPedidoServicio/ConfirmarPedidoServicio.aspx?id={id}");
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al abrir la ventana de confirmar: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            if (e.CommandName == "Ver")
            {
                try
                {
                    string id = GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString();
                    int estado = int.Parse(GrillaPedidos.DataKeys[index].Values["ID_ESTADO_PEDIDO"].ToString());

                    if (estado == 6)
                    {
                        Response.Redirect($"~/PantallaPedidoServicio/VerServicioPendiente.aspx?id={id}&VerModificar={"Ver"}");
                    }
                    else if (estado == 7) { Response.Redirect($"~/PantallaPedidoServicio/VerServicioConfirmado.aspx?id={id}"); }
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al abrir la ventana de ver pedido: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }

            }
            if (e.CommandName == "Modificar")
            {
                try
                {
                    string id = GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString();
                    int estado = int.Parse(GrillaPedidos.DataKeys[index].Values["ID_ESTADO_PEDIDO"].ToString());

                    if (estado == 6)
                    {
                        Response.Redirect($"~/PantallaPedidoServicio/VerServicioPendiente.aspx?id={id}&VerModificar={"Modificar"}");
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al abrir la ventana de modificar: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            if (e.CommandName == "Cancel")
            {
                string id = GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString();
                int estado = int.Parse(GrillaPedidos.DataKeys[index].Values["ID_ESTADO_PEDIDO"].ToString());

                if (estado == 6)
                {
                    Session["IdPedidoS"] = id;
                    string mensaje = "¿Está seguro de querer cancelar el pedido "+id+"?";
                    AlertaCancelar(mensaje, NotificationType.warning);
                }

            }
            if (e.CommandName == "del")
            {
                string id = GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString();
                int estado = int.Parse(GrillaPedidos.DataKeys[index].Values["ID_ESTADO_PEDIDO"].ToString());

                if (estado == 3)
                {
                    Session["IdPedidoS"] = id;
                    string mensaje = "¿Está seguro de querer borrar el pedido "+id+"?";
                    AlertaBorrar(mensaje, NotificationType.warning);
                }

            }

        }

        protected void GrillaClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaClientes.PageIndex = e.NewPageIndex;
            if (txtBuscar.Text.Equals(""))
            {
                CargarClientes();
            }
            else { BuscarCliente(); }
        }

        protected void GrillaClientes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        private void CargarPedidoID()
        {
            try
            {
                SqlBuscarPedidoId.SelectParameters["IDPedido"].DefaultValue = tbBuscaPedido.Text;

                GrillaPedidos.DataSourceID = null;
                GrillaPedidos.DataSource = SqlBuscarPedidoId;
                GrillaPedidos.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar el pedido de venta por su codigo: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void GrillaPedidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaPedidos.PageIndex = e.NewPageIndex;
            if (lblIdCli.Text.Equals(""))
            {
                if (tbBuscaPedido.Text.Equals(""))
                {
                    CargarPedidos();//////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else
                {
                    CargarPedidoID();
                }

                UpdatePanel1.Update();
            }
            else
            {
                try
                {
                    SqlPedidosDelCliente.SelectParameters["idCli"].DefaultValue =
                            this.GrillaClientes.DataKeys[GrillaClientes.SelectedIndex].Values["ID_CLIENTE"].ToString();
                    GrillaPedidos.DataSourceID = "SqlPedidosDelCliente";
                    GrillaPedidos.DataBind();
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al cambiar de página en la tabla pedidos: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

            protected void btnBuscarPedido_Click(object sender, EventArgs e)
        {
            if (tbBuscaPedido.Text.Equals(""))
            {
                CargarPedidos();
            }
            else
            {
                lblIdCli.Text = "";
                UPPCli.Update();
                CargarPedidoID();
            }
        }
    }
}