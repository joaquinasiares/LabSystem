using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaPedido
{
    public partial class ListadoPedidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            LblResOpe("");
            if (!IsPostBack)
            {
                string mensaje = Request.QueryString["mensaje"];
                if (!string.IsNullOrEmpty(mensaje))
                {
                    MostrarAlerta(mensaje, NotificationType.success);
                }
                CargarPedidos();
                CargarClientes();
                Session["BusquedaEstado"] = false;
            }

        }

        private void CargarClientes()
        {
            try
            {
                GrillaClientes.DataSource = SqlCliente;
                GrillaClientes.DataBind();
                UpdatePanel2.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los clientes: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
        private void CargarPedidos()
        {
            /* PedidoNegocio ped = new PedidoNegocio();
             List<Pedido> lista = ped.selectPedidoAll();


             DataTable dt = crearDT(lista);

             foreach (Pedido pedido in lista)
             {
                 DateTime fecha = pedido.getFecha().Date;
                 string fechaPedido = fecha.ToString("dd/MM/yyyy");
                 string ambito;
                 if (pedido.getCliente().getprivPuba() == 0)
                 {
                     ambito = "Publico";
                 }
                 else { ambito = "Privado"; }

                 dt.Rows.Add(
                     pedido.getId(),
                     pedido.getCliente().getId(),
                     fechaPedido,
                     pedido.getDesc(),
                     pedido.getCliente().getnombre(),
                     ambito,
                     pedido.getNomEstado(),
                     pedido.getViatico(),
                     pedido.getTotal()
                     );

             }
             GrillaPedidos.Columns[1].Visible = true;
             GrillaPedidos.DataSource = dt;
             GrillaPedidos.DataBind();
             GrillaPedidos.Columns[1].Visible = false;*/
            try
            {
                GrillaPedidos.DataSourceID = null;
                GrillaPedidos.DataSource = SqlPedido;
                GrillaPedidos.DataBind();
                UpdatePanel1.Update();
                Session["BusquedaEstado"] = false;
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los clientes: " + ex.Message;
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
                    BorrarPedido();
                }
                else if (argument == "Confirmar")
                { ConfirmarPedido(); }
                else if (argument == "Cancelar")
                { CancelarPedido(); }
                Session["Indice"] = 0;
            }
        }






        private void LblResOpe(string mensaje)
        {
            lblResultadoOperacion.Text = mensaje;
            updpLblResOpe.Update();
        }

        private DataTable crearDT(List<Pedido> li)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IdPedido");
            dt.Columns.Add("IdCliente");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("Cliente");
            dt.Columns.Add("Ambito");
            dt.Columns.Add("Estado");
            dt.Columns.Add("Viatico");
            dt.Columns.Add("Total");
            if (li.Count == 0)
            {
                dt.Rows.Add(-10, "", "", "", "", "", "", "");
            }


            return dt;
        }
        /*
            apartir de aca estan los metodos para: Confirmar, cancelar y borrar
            debajo de estos metodos se encuentra el metodo rowcommand el cual los activa
            la razon de separarlos de los metodos vender y modificar, es que los tres
            metodos anteriores tiene una pregunta para confirmar la accion respectiva
        */

        //metodo Confirmar

        private void ConfirmarPedido()
        {
            try
            {
                int indice = int.Parse(Session["Indice"].ToString());
                SqlPedidosDelCliente.UpdateParameters["idPed"].DefaultValue = GrillaPedidos.DataKeys[indice].Values["ID_PEDIDO"].ToString();
                SqlPedidosDelCliente.UpdateParameters["idEstado"].DefaultValue = "8";
                SqlPedidosDelCliente.Update();

                GrillaPedidos.DataSource = null;
                GrillaPedidos.DataBind();
                CargarPedidos();
                UpdatePanel1.Update();
                Response.Redirect("~/PantallaPedido/ListadoPedidos.aspx");
                string mensaje = "Se confirmo el pedido con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al confirmar el pedido: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        //metodo Cancelar

        private void CancelarPedido()
        {
            try
            {
                int indice = int.Parse(Session["Indice"].ToString());
                SqlDetallePedido.SelectParameters["ID"].DefaultValue = GrillaPedidos.DataKeys[indice].Values["ID_PEDIDO"].ToString();
                SqlDetallePedido.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader reader;
                reader = (SqlDataReader)SqlDetallePedido.Select(DataSourceSelectArguments.Empty);
                while (reader.Read())
                {
                    SqlDetallePedido.UpdateParameters["idProd"].DefaultValue = reader["IdProd"].ToString();
                    SqlDetallePedido.UpdateParameters["cantPed"].DefaultValue = reader["CANTIDAD"].ToString();
                    SqlDetallePedido.UpdateParameters["idStock"].DefaultValue = reader["ID_STOCK"].ToString();
                    SqlDetallePedido.Update();
                }

                SqlPedidosDelCliente.UpdateParameters["idPed"].DefaultValue = GrillaPedidos.DataKeys[indice].Values["ID_PEDIDO"].ToString();
                SqlPedidosDelCliente.UpdateParameters["idEstado"].DefaultValue = "3";
                SqlPedidosDelCliente.Update();
                GrillaPedidos.DataSource = null;
                GrillaPedidos.DataBind();
                CargarPedidos();
                UpdatePanel1.Update();
                Response.Redirect("~/PantallaPedido/ListadoPedidos.aspx");
                string mensaje = "Se cancelo el pedido con exito";
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
                int indice = int.Parse(Session["Indice"].ToString());
                //string id = Session["IdPedido"].ToString();
                SqlPedidosDelCliente.DeleteParameters["idPed"].DefaultValue = GrillaPedidos.DataKeys[indice].Values["ID_PEDIDO"].ToString();

                SqlPedidosDelCliente.Delete();
                GrillaPedidos.DataSource = null;
                GrillaPedidos.DataBind();
                CargarPedidos();
                string mensaje = "Se borro el pedido con exito";
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
            if (e.CommandName == "Confirmar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = GrillaPedidos.Rows[index];
                string estado = GrillaPedidos.DataKeys[index].Values["DESCRIPCION_ESTADO_PEDIDO"].ToString();

                if (estado.Equals("Pendiente"))
                {
                    Session["Indice"] = index;
                    string mensaje = "¿Está seguro de querer confirmar el pedido " + GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString() + "?";
                    AlertaConfirmar(mensaje, NotificationType.warning);
                }

            }
            if (e.CommandName == "Cancel")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = GrillaPedidos.Rows[index];
                string estado = GrillaPedidos.DataKeys[index].Values["DESCRIPCION_ESTADO_PEDIDO"].ToString();

                if (estado.Equals("Pendiente"))
                {
                    Session["Indice"] = index;
                    string mensaje = "¿Está seguro de querer cancelar el pedido " +
                        GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString() + "?";
                    AlertaCancelar(mensaje, NotificationType.warning);
                }

            }
            if (e.CommandName == "Vender")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = GrillaPedidos.Rows[index];
                string estado = GrillaPedidos.DataKeys[index].Values["DESCRIPCION_ESTADO_PEDIDO"].ToString();

                if (estado.Equals("Confirmado"))
                {
                    try
                    {

                        Response.Redirect($"~/PantallaVenta/NuevaVenta.aspx?" +
                        $"IdPedido={GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString()}" +
                        $"&IdCliente={GrillaPedidos.DataKeys[index].Values["ID_CLIENTE"].ToString()}" +
                        $"&Fecha={GrillaPedidos.DataKeys[index].Values["FECHA_PEDIDO"].ToString()}" +
                        $"&Cliente={GrillaPedidos.DataKeys[index].Values["NOM_CLIENTE"].ToString()}" +
                        $"&Rs={GrillaPedidos.DataKeys[index].Values["RAZON_SOCIAL"].ToString()}" +
                        $"&Ambito={GrillaPedidos.DataKeys[index].Values["priv_pub"].ToString()}" +
                        $"&Viatico={GrillaPedidos.DataKeys[index].Values["Viatico"].ToString()}" +
                        $"&Total={GrillaPedidos.DataKeys[index].Values["TOTAL"].ToString()}" +
                        $"&Descripcion={GrillaPedidos.DataKeys[index].Values["ID_CLIENTE"].ToString()}");
                    }
                    catch (Exception ex)
                    {
                        string mensaje = "Ocurrió un error al abrir la ventana de venta: " + ex.Message;
                        MostrarAlerta(mensaje, NotificationType.error);
                    }
                }

            }
            if (e.CommandName == "Delete")
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = GrillaPedidos.Rows[index];
                string estado = GrillaPedidos.DataKeys[index].Values["DESCRIPCION_ESTADO_PEDIDO"].ToString();

                if (estado.Equals("Cancelado"))
                {
                    Session["Indice"] = index;
                    string mensaje = "¿Está seguro de querer borrar el pedido" + GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString() + "?";
                    AlertaBorrar(mensaje, NotificationType.warning);
                }
            }
            if (e.CommandName == "Modificar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = GrillaPedidos.Rows[index];
                string estado = GrillaPedidos.DataKeys[index].Values["DESCRIPCION_ESTADO_PEDIDO"].ToString();

                if (estado.Equals("Pendiente"))
                {
                    try
                    {
                        Response.Redirect($"~/PantallaPedido/PedidoVentaModificar.aspx?" +
                        $"IdPedido={GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString()}" +
                        $"&IdCliente={GrillaPedidos.DataKeys[index].Values["ID_CLIENTE"].ToString()}" +
                        $"&Fecha={GrillaPedidos.DataKeys[index].Values["FECHA_PEDIDO"].ToString()}" +
                        $"&Cliente={GrillaPedidos.DataKeys[index].Values["NOM_CLIENTE"].ToString()}" +
                        $"&Rs={GrillaPedidos.DataKeys[index].Values["RAZON_SOCIAL"].ToString()}" +
                        $"&Ambito={GrillaPedidos.DataKeys[index].Values["priv_pub"].ToString()}" +
                        $"&Viatico={GrillaPedidos.DataKeys[index].Values["Viatico"].ToString()}" +
                        $"&Total={GrillaPedidos.DataKeys[index].Values["TOTAL"].ToString()}" +
                        $"&Descripcion={GrillaPedidos.DataKeys[index].Values["ID_CLIENTE"].ToString()}");
                    }
                    catch (Exception ex)
                    {
                        string mensaje = "Ocurrió un error al abrir la ventana de modificacion: " + ex.Message;
                        MostrarAlerta(mensaje, NotificationType.error);
                    }
                }

            }

            if (e.CommandName == "Ver")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = GrillaPedidos.Rows[index];


                try
                {
                    Response.Redirect($"~/PantallaPedido/VerPedido.aspx?" +
                        $"IdPedido={GrillaPedidos.DataKeys[index].Values["ID_PEDIDO"].ToString()}" +
                        $"&IdCliente={GrillaPedidos.DataKeys[index].Values["ID_CLIENTE"].ToString()}" +
                        $"&Fecha={GrillaPedidos.DataKeys[index].Values["FECHA_PEDIDO"].ToString()}" +
                        $"&Cliente={GrillaPedidos.DataKeys[index].Values["NOM_CLIENTE"].ToString()}" +
                        $"&Rs={GrillaPedidos.DataKeys[index].Values["RAZON_SOCIAL"].ToString()}" +
                        $"&Ambito={GrillaPedidos.DataKeys[index].Values["priv_pub"].ToString()}" +
                        $"&Viatico={GrillaPedidos.DataKeys[index].Values["Viatico"].ToString()}" +
                        $"&Total={GrillaPedidos.DataKeys[index].Values["TOTAL"].ToString()}" +
                        $"&Descripcion={GrillaPedidos.DataKeys[index].Values["ID_CLIENTE"].ToString()}");
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al abrir la ventana de modificacion: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }

            }
        }

        //eliminar el pedido

        protected void GrillaPedidos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }


        //por alguna razon sin esto no anda el cancelar, aunque no lo uso a este metodo
        protected void GrillaPedidos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Salir del modo de edición
            GrillaPedidos.EditIndex = -1;

            // Recargar los datos en el GridView
            CargarPedidos();

            // Actualizar el UpdatePanel si es necesario
            UpdatePanel1.Update();
        }



        protected void GrillaPedidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaPedidos.PageIndex = e.NewPageIndex;

            if (lblId.Text.Equals(""))
            {
                bool busquedaEsatdo = (bool)Session["BusquedaEstado"];
                if (busquedaEsatdo)
                {
                    CargarPedidosPorEstado();
                }
                else
                {
                    if (tbBuscaPedido.Text.Equals(""))
                    {
                        CargarPedidos();
                    }
                    else
                    {
                        CargarPedidoID();
                    }

                    UpdatePanel1.Update();
                }
            }
            else
            {
                try
                {
                    SqlPedidosDelCliente.SelectParameters["idCli"].DefaultValue =
                            this.GrillaClientes.DataKeys[GrillaClientes.SelectedIndex].Values["ID_CLIENTE"].ToString();
                    GrillaPedidos.DataSourceID = null;
                    GrillaPedidos.DataSource = SqlPedidosDelCliente;
                    GrillaPedidos.DataBind();
                    UpdatePanel1.Update();
                    Session["BusquedaEstado"] = false;
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al cambiar de página en la tabla pedidos: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }

        }

        protected void GrillaClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string idCliente = this.GrillaClientes.DataKeys[GrillaClientes.SelectedIndex].Values["ID_CLIENTE"].ToString();

                lblId.Text = idCliente;
                lblAmb.Text = this.GrillaPedidos.DataKeys[GrillaClientes.SelectedIndex].Values["priv_pub"].ToString();
                lblNom.Text = this.GrillaPedidos.DataKeys[GrillaClientes.SelectedIndex].Values["NOM_CLIENTE"].ToString();
                lblRs.Text = this.GrillaPedidos.DataKeys[GrillaClientes.SelectedIndex].Values["RAZON_SOCIAL"].ToString();
                UPPCli.Update();

                SqlPedidosDelCliente.SelectParameters["idCli"].DefaultValue = idCliente.ToString();
                GrillaPedidos.DataSourceID = "SqlPedidosDelCliente";
                GrillaPedidos.AllowPaging = false;
                GrillaPedidos.DataBind();
                GrillaPedidos.AllowPaging = true;
                UpdatePanel1.Update();
                Session["BusquedaEstado"] = false;
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al seleccionar un cliente: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!txtBuscar.Text.Equals(""))
            {
                try
                {
                    // Cambiar el parámetro de búsqueda antes de enlazar la nueva fuente de datos
                    SqlDataSource1.SelectParameters["Nombre"].DefaultValue = txtBuscar.Text;

                    // Desvincular cualquier DataSource previamente asignado
                    GrillaClientes.DataSourceID = null;
                    GrillaClientes.DataSource = SqlDataSource1;  // Asignar directamente el SqlDataSource
                    GrillaClientes.DataBind();  // Enlazar los datos
                    UpdatePanel2.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al buscar un cliente: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                lblId.Text = "";
                // Desvincular cualquier DataSource previamente asignado
                GrillaClientes.DataSourceID = null;
                GrillaClientes.DataSource = null;  // Asignar directamente el SqlDataSource
                GrillaClientes.DataBind();  // Enlazar los datos
                CargarClientes();
                UpdatePanel2.Update();
                CargarPedidos();

                UpdatePanel1.Update();
            }
        }

        protected void BtnNuevoPedido_Click(object sender, EventArgs e)
        {
            if (!lblId.Text.Equals(""))
            {
                try
                {
                    Response.Redirect($"GestorPedidoVenta.aspx?NOM_CLIENTE={lblNom.Text}&priv_pub={lblAmb.Text}&ID_CLIENTE={lblId.Text}" +
                                            $"&Rs={lblRs.Text}");
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al realizar un nuevo pedido: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                string mensaje = "Seleccione un cliente";
                MostrarAlerta(mensaje, NotificationType.info);
            }

        }

        protected void GrillaClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaClientes.PageIndex = e.NewPageIndex;
            if (txtBuscar.Text.Equals(""))
            {
                CargarClientes();
            }
            else
            {
                try
                {
                    GrillaClientes.DataSource = SqlDataSource1;
                    GrillaClientes.DataBind();  // Enlazar los datos
                    UpdatePanel2.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al cambiar de página en la tabla clientes: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        private void CargarPedidoID()
        {
            try
            {
                SqlBuscarPedidoId.SelectParameters["idPed"].DefaultValue = tbBuscaPedido.Text;

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
            Session["BusquedaEstado"] = false;
        }

        protected void btnBuscarPedido_Click(object sender, EventArgs e)
        {
            if (tbBuscaPedido.Text.Equals(""))
            {
                CargarPedidos();
            }
            else
            {
                lblId.Text = "";
                CargarPedidoID();
            }
            Session["BusquedaEstado"] = false;
        }

        private void CargarPedidosPorEstado()
        {
            try
            {
                string estado = DropDownList1.SelectedItem.Text;
                SqlBuscarPorEstadoDePedidos.SelectParameters["Estado"].DefaultValue = estado;

                GrillaPedidos.DataSourceID = null;
                GrillaPedidos.DataSource = SqlBuscarPorEstadoDePedidos;
                GrillaPedidos.DataBind();
                UpdatePanel1.Update();
                lblId.Text = "";
                UPPCli.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar el pedido de venta por su estado: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnBuscarPorEstado_Click(object sender, EventArgs e)
        {
            CargarPedidosPorEstado();
            Session["BusquedaEstado"] = true;
        }

        protected void BtnLimp_Click(object sender, EventArgs e)
        {
            CargarPedidos();
        }

        protected void GrillaClientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                try { 

                    string idCliente = GrillaClientes.DataKeys[index].Values["ID_CLIENTE"].ToString();
                    string privPub = GrillaClientes.DataKeys[index].Values["priv_pub"].ToString();
                    string nomCliente = GrillaClientes.DataKeys[index].Values["NOM_CLIENTE"].ToString();
                    string razonSocial = GrillaClientes.DataKeys[index].Values["RAZON_SOCIAL"].ToString();

                    lblId.Text = idCliente;
                    lblAmb.Text = privPub;
                    lblNom.Text = nomCliente;
                    lblRs.Text = razonSocial;
                    UPPCli.Update();

                    SqlPedidosDelCliente.SelectParameters["idCli"].DefaultValue = idCliente;
                    GrillaPedidos.DataSourceID = null;
                    GrillaPedidos.DataSource = SqlPedidosDelCliente;
                    GrillaPedidos.DataBind();
                    UpdatePanel1.Update();

                    Session["BusquedaEstado"] = false;
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al seleccionar un cliente: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

    }
}