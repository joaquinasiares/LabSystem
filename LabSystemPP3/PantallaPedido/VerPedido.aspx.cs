using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaPedido
{
    public partial class VerPedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LblResOpe("");
            if (!IsPostBack)
            {
                CargarDatos();
                CrearDetalleProd();
                CrearTablaParaBorrar();
            }
        }

        private void CargarDatos()
        {
            try
            {
                lblIdPedido.Text = Request.QueryString["IdPedido"];
                lblNomCli.Text = Request.QueryString["Cliente"];
                lblAmbito.Text = Request.QueryString["Ambito"];
                lblIdCli.Text = Request.QueryString["IdCliente"];
                lblFecha.Text = Request.QueryString["Fecha"];
                lblRs.Text = Request.QueryString["Rs"];
                string viatico = Request.QueryString["Viatico"];
                lblTotal.Text = Request.QueryString["Total"];

                LblViatico.Text = viatico;
                Decimal ValorDelViatico = Decimal.Parse(viatico);
                Decimal totalDeTodo = Decimal.Parse(lblTotal.Text);
                Decimal resta = totalDeTodo - ValorDelViatico;
                lblTotProd.Text = resta.ToString();


                lblTotal.Text = Request.QueryString["Total"];
                tbDesc.Text = Request.QueryString["Descripcion"];
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los datos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

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




        //se cargan los datos de los detalles del pedido del cliente en el session Pedidos
        private void CrearDetalleProd()
        {
            try
            {
                SqlDetalle.SelectParameters["ID"].DefaultValue = lblIdPedido.Text;
                SqlDetalle.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader reader;

                reader = (SqlDataReader)SqlDetalle.Select(DataSourceSelectArguments.Empty);
                DataTable dt = new DataTable();
                dt.Columns.Add("IdDetalle");
                dt.Columns.Add("IdProd");
                dt.Columns.Add("ID_STOCK");
                dt.Columns.Add("DESCRIPCION");
                dt.Columns.Add("precio");
                dt.Columns.Add("CANTIDAD");
                dt.Columns.Add("SubTotal");

                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["IdDetalle"] = reader["ID_DETALLE_PEDIDO"];
                    dr["IdProd"] = reader["IdProd"];
                    dr["ID_Stock"] = reader["ID_STOCK"];
                    dr["DESCRIPCION"] = reader["DESCRIPCION"];
                    dr["precio"] = reader["precio"];
                    dr["CANTIDAD"] = reader["CANTIDAD"];
                    dr["SubTotal"] = reader["SubTotal"];

                    dt.Rows.Add(dr);

                }
                Session["Pedidos"] = dt;


                GrillaProductoPedido.DataBind();
                GrillaProductoPedido.Columns[0].Visible = true;
                GrillaProductoPedido.Columns[1].Visible = true;
                GrillaProductoPedido.Columns[2].Visible = true;
                GrillaProductoPedido.DataSource = dt;
                GrillaProductoPedido.DataBind();
                GrillaProductoPedido.Columns[0].Visible = false;
                GrillaProductoPedido.Columns[1].Visible = false;
                GrillaProductoPedido.Columns[2].Visible = false;
                GrillaProductoPedido.AllowPaging = true;
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los productos a vender: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        //se crea un session que va a guardar los detalles que se van a eliminar cuando se modifique el pedido

        private void CrearTablaParaBorrar()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("IdDetalle");
                dt.Columns.Add("IdProd");
                dt.Columns.Add("ID_STOCK");
                dt.Columns.Add("CANTIDAD");

                Session["TablaParaBorrar"] = dt;
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }









        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }



        protected void GrillaProductoPedido_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaProductoPedido.PageIndex = e.NewPageIndex;

                // Recupera los datos de la sesión y los vuelve a enlazar
                DataTable dtPedidos = Session["Pedidos"] as DataTable;
                if (dtPedidos != null)
                {
                    GrillaProductoPedido.DataSource = dtPedidos;
                    GrillaProductoPedido.DataBind();
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar de página en la tabla productos seleccionados: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

    }
}