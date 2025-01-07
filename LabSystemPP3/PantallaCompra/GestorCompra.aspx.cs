using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;
using static System.Net.WebRequestMethods;

namespace LabSystemPP3.PantallaCompra
{
    public partial class GestorCompra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            if (!IsPostBack) 
            {
                CargarCompras();
                AjustarPaginacion();
            }
        }

        private void CargarCompras() 
        {
            try
            {
                Grilla.DataSource = SqlCompra;
                Grilla.DataBind();
                UpdatePanel1.Update();
            }catch(Exception ex) 
            {
                string mensaje = "Ocurrió un error al cargar las compras " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void AjustarPaginacion()
        {
            // Realiza una consulta para contar los registros
            SqlDataSource sqlDataSource = SqlCompra;
            DataView dataView = (DataView)sqlDataSource.Select(DataSourceSelectArguments.Empty);
            int totalRegistros = dataView.Count;

            // Define el tamaño máximo antes de habilitar paginación
            int tamañoMaximo = 10;

            if (totalRegistros > tamañoMaximo)
            {
                Grilla.AllowPaging = true;
                Grilla.PageSize = tamañoMaximo; // Configura el tamaño deseado
            }
            else
            {
                Grilla.AllowPaging = false; // Desactiva la paginación si no supera el tamaño
            }

            // Vuelve a enlazar los datos para reflejar los cambios
            Grilla.DataBind();
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
                Session["IndiceCompra"] = 0;
            }
        }


        private void ConfirmarPedido()
        {
            try
            {
                int indice = int.Parse(Session["IndiceCompra"].ToString());

                DataTable dt = new DataTable();
                dt.Columns.Add("ID_DETALLE");
                dt.Columns.Add("IdProd");
                dt.Columns.Add("COD_PROD");
                dt.Columns.Add("DESCRIPCION");
                dt.Columns.Add("PrecioUnitario");
                dt.Columns.Add("CANTIDAD");
                dt.Columns.Add("Total");
                dt.Columns.Add("TotalGeneral");


                this.SqlDetalleOrdCom.SelectParameters["IdOc"].DefaultValue = Grilla.DataKeys[indice].Values["ID_ORD_COMP"].ToString();
                this.SqlDetalleOrdCom.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader reader;
                reader = (SqlDataReader)this.SqlDetalleOrdCom.Select(DataSourceSelectArguments.Empty);

                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ID_DETALLE"] = reader["ID_DETALLE"];
                    row["IdProd"] = reader["IdProd"];
                    row["COD_PROD"] = reader["COD_PROD"];
                    row["DESCRIPCION"] = reader["DESCRIPCION"];
                    row["PrecioUnitario"] = reader["PrecioUnitario"];
                    row["CANTIDAD"] = reader["CANTIDAD"];
                    row["Total"] = reader["Total"];
                    row["TotalGeneral"] = reader["TotalGeneral"];

                    dt.Rows.Add(row);
                }
                MostrarAlerta(dt.Rows.Count.ToString(), NotificationType.success);
                foreach (DataRow fila in dt.Rows)
                {
                    SqlDetalleOrdCom.UpdateParameters["idProd"].DefaultValue = fila["IdProd"].ToString();
                    SqlDetalleOrdCom.UpdateParameters["cantidad"].DefaultValue = fila["CANTIDAD"].ToString();
                    SqlDetalleOrdCom.Update();
                }

                SqlCompra.UpdateParameters["idCompra"].DefaultValue = Grilla.DataKeys[indice].Values["ID_COMPRA"].ToString();
                SqlCompra.UpdateParameters["estado"].DefaultValue = "3";
                SqlCompra.UpdateParameters["idEmp"].DefaultValue = ddlEmpleado.SelectedValue;
                SqlCompra.UpdateParameters["fecha"].DefaultValue =DateTime.Now.ToString();
                SqlCompra.Update();

                UpdatePanel1.Update();
                Response.Redirect("~/PantallaCompra/GestorCompra.aspx");
                string mensaje = "Se confirmo la compra con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al confirmar la compra: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        //metodo Cancelar

        private void CancelarPedido()
        {
            try
            {
                int indice = int.Parse(Session["IndiceCompra"].ToString());

                SqlCompra.UpdateParameters["idCompra"].DefaultValue = Grilla.DataKeys[indice].Values["ID_COMPRA"].ToString() ;
                SqlCompra.UpdateParameters["estado"].DefaultValue = "2";
                SqlCompra.Update();

                UpdatePanel1.Update();
                Response.Redirect("~/PantallaCompra/GestorCompra.aspx");
                string mensaje = "Se cancelo la compra con exito";
                MostrarAlerta(mensaje, NotificationType.success);

            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cancelar la compra: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        //borrar pedido

        private void BorrarPedido()
        {
            try
            {
                int indice = int.Parse(Session["IndiceCompra"].ToString());

                SqlCompra.DeleteParameters["idCompra"].DefaultValue = Grilla.DataKeys[indice].Values["ID_COMPRA"].ToString();
                SqlCompra.Delete();

                UpdatePanel1.Update();
                Response.Redirect("~/PantallaCompra/GestorCompra.aspx");
                string mensaje = "Se cancelo el pedido con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al borrar la compra: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        
        protected void Grilla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Ver")
            {
                int id = int.Parse(this.Grilla.DataKeys[index].Values["ID_ORD_COMP"].ToString());
                try
                {
                    
                    Response.Redirect($"~/PantallaOrdendecompra/paginaOrdenCompra.aspx?Id={id}");
                }catch (Exception ex) 
                {
                    string mensaje = "Ocurrió un error al ver la orden de compra "+ id + " "+ ex.Message;
                    MostrarAlerta(mensaje,NotificationType.error);
                }
            }
            if (e.CommandName == "Modificar")
            {
                int idEstado = int.Parse(this.Grilla.DataKeys[index].Values["IdEstadoCom"].ToString());
                if (idEstado==1) 
                {
                    int id = int.Parse(this.Grilla.DataKeys[index].Values["ID_ORD_COMP"].ToString());
                    try
                    {
                        
                        int idProv= int.Parse(this.Grilla.DataKeys[index].Values["ID_PROV"].ToString());
                        Response.Redirect($"~/PantallaOrdendecompra/ModificarOrdenDeCompra.aspx?Id={id}&IdProv={idProv}");
                    }
                    catch (Exception ex)
                    {
                        string mensaje = "Ocurrió un error al abrir la ventana modificar orden de compra " + id + " " + ex.Message;
                        MostrarAlerta(mensaje, NotificationType.error);
                    }
                }
            }
            if (e.CommandName == "Cancelar")
            {
                int id = int.Parse(this.Grilla.DataKeys[index].Values["ID_ORD_COMP"].ToString());
                try
                {
                    
                    int estado = int.Parse(Grilla.DataKeys[index].Values["IdEstadoCom"].ToString());

                    if (estado == 1)
                    {
                        Session["IndiceCompra"] = index;
                        string mensaje = "¿Está seguro de querer cancelar la orden de compra " + id + "?";
                        AlertaCancelar(mensaje, NotificationType.warning);
                    }

                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al cancelar la orden de compra " + id + " " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            if (e.CommandName == "Borrar")
            {
                int id = int.Parse(this.Grilla.DataKeys[index].Values["ID_ORD_COMP"].ToString());
                try
                {
                    
                    int estado = int.Parse(Grilla.DataKeys[index].Values["IdEstadoCom"].ToString());

                    if (estado == 2)
                    {
                        Session["IndiceCompra"] = index;
                        string mensaje = "¿Está seguro de querer borrar la orden de compra " + id + "?";
                        AlertaBorrar(mensaje, NotificationType.warning);
                    }

                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al borrar la orden de compra " + id + " " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            if (e.CommandName == "Confirmar")
            {
                int id = int.Parse(this.Grilla.DataKeys[index].Values["ID_ORD_COMP"].ToString());
                try
                {
                    
                    int estado = int.Parse(Grilla.DataKeys[index].Values["IdEstadoCom"].ToString());

                    if (estado == 1)
                    {
                        lblNumOrd.Text = id.ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
                        Session["IndiceCompra"] = index;
                        //string mensaje = "¿Está seguro de querer confirmar la orden de compra " + id + "?";
                        //AlertaConfirmar(mensaje, NotificationType.warning);
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al confirmar la orden de compra " + id + " " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void Grilla_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Grilla.PageIndex = e.NewPageIndex;
                if (txtBuscar.Text.Equals(""))
                {
                    CargarCompras();
                }
                else { CargarComprasId(); }
                
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar la página de la tabla compras: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CargarComprasId() 
        {
            try
            {
                // Cambiar el parámetro de búsqueda antes de enlazar la nueva fuente de datos
                SqlBuscarCompraId.SelectParameters["idOrdcom"].DefaultValue = txtBuscar.Text;

                // Desvincular cualquier DataSource previamente asignado
                Grilla.DataSourceID = null;
                Grilla.DataSource = SqlBuscarCompraId;  // Asignar directamente el SqlDataSource
                Grilla.DataBind();  // Enlazar los datos
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar las compras " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Equals(""))
            {
                CargarCompras();
            }
            else
            {
                try
                {
                    CargarComprasId();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al buscar un cliente: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void btnConfirmarCom_Click(object sender, EventArgs e)
        {
            string mensaje = "¿Está seguro de querer confirmar la orden de compra"+lblNumOrd.Text+"?";
            AlertaConfirmar(mensaje, NotificationType.warning);
        }

        protected void BtnCerrarModal_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "cerar();", true);
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al cerrar la ventana";
                MostrarAlerta(msg, NotificationType.error);
            }
        }
    }
}