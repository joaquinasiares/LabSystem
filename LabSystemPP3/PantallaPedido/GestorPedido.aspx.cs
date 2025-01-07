using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.PantallaPedido
{
    public partial class GestorPedido1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cargarDDLS();
            CargarClientes();
        }

        private void cargarDDLS()
        {

            PedidoNegocio pedidoNegocio = new PedidoNegocio();
            List<PedidoTipo> listatipo = pedidoNegocio.listartipos();

            foreach (PedidoTipo tipo in listatipo)
            {
                string idtipo = tipo.getId().ToString();
                DDLPedidos.Items.Add(new ListItem(tipo.getDescripcion(), idtipo));
            }

        }

        private void CargarClientes()
        {
            clienteNegocio cn = new clienteNegocio();
            List<Cliente> lista = cn.listarClientes();


            string ambito = "";
            DataTable dt = crearDT(lista);

            foreach (Cliente cliente in lista)
            {
                if (cliente.getprivPuba() == 0)
                {
                    ambito = "Publico";
                }
                else
                {
                    ambito = "Privado";
                }
                dt.Rows.Add(cliente.getId(),
                cliente.getcuit(),
                cliente.getnombre(),
                cliente.getrazonSociak(),
                cliente.getDescIva(),
                ambito);
            }
            Grilla.Columns[0].Visible = true;
            Grilla.DataSource = dt;
            Grilla.DataBind();
            Grilla.Columns[0].Visible = false;
        }

        protected void Grilla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = Grilla.Rows[index];
                lblId.Text = fila.Cells[0].Text;
                lblNom.Text = fila.Cells[2].Text;
                lblAmb.Text = fila.Cells[5].Text;
                UPPCli.Update();
            }
        }

        private DataTable crearDT(List<Cliente> li)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Cuit");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Razon social");
            dt.Columns.Add("IVA");
            dt.Columns.Add("Ambito");
            if (li.Count() == 0)
            {
                dt.Rows.Add("", "", "", "", "", "");
            }
            return dt;
        }

        private void BuscarCliente()
        {
            clienteNegocio cn = new clienteNegocio();
            List<Cliente> lista = cn.buscarClientes(txtBuscar.Text);
            string ambito = "";
            DataTable dt = crearDT(lista);

            foreach (Cliente cliente in lista)
            {
                if (cliente.getprivPuba() == 0)
                {
                    ambito = "Publico";
                }
                else
                {
                    ambito = "Privado";
                }
                dt.Rows.Add(cliente.getId(),
                cliente.getcuit(),
                cliente.getnombre(),
                cliente.getrazonSociak(),
                cliente.getDescIva(),
                ambito);
            }
            Grilla.Columns[0].Visible = true;
            Grilla.DataSource = dt;
            Grilla.DataBind();
            Grilla.Columns[0].Visible = false;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Equals(""))
            {
                CargarClientes();
            }
            else
            {
                BuscarCliente();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SelecTipoPedido();
        }

        private void SelecTipoPedido() 
        {
            if (!lblNom.Text.Equals("")) {
                int seleccion = int.Parse(DDLPedidos.SelectedValue);
                string nom = lblNom.Text;
                string id = lblId.Text;
                string ambito = lblAmb.Text;
                if (seleccion == 1)
                {
                    Response.Redirect("GestorPedidoVenta.aspx?Nombre=" + nom + "&Ambito=" + ambito + "&ID=" + id);
                }
            }
        }
    }
}