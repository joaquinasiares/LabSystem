using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.PantallaOrdendecompra
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { 
                cargarDDLS(); 
                CargarProveedores();
                CargarProductos(0);
            }
        }

        private void cargarDDLS()
        {
            ProveedorNegocio prov = new ProveedorNegocio();
            List<Proveedor> listaprov = prov.listarProveedores();

            foreach (Proveedor proveedor in listaprov)
            {
                string idProv = proveedor.getId().ToString();
                DDlProv.Items.Add(new ListItem(proveedor.getnombre(), idProv));
            }
        }

        private void CargarProveedores()
        {
            ProveedorNegocio prn = new ProveedorNegocio();
            List<Proveedor> lista = prn.listarProveedores();

            DataTable dt = crearDTProv(lista);

            foreach (Proveedor proveedor in lista)
            {
                dt.Rows.Add(proveedor.getId(),

                proveedor.getnombre());
            }
            GrillaProv.Columns[0].Visible = true;
            GrillaProv.DataSource = dt;
            GrillaProv.DataBind();
            GrillaProv.Columns[0].Visible = false;
        }

        private DataTable crearDTProv(List<Proveedor> li)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Nombre");
            if (li.Count() == 0)
            {
                dt.Rows.Add(-10, "");
            }
            return dt;
        }

        private void BuscarProveedor()
        {
            ProveedorNegocio prn = new ProveedorNegocio();
            List<Proveedor> lista = prn.buscarProvedores(TbBucProveedor.Text);

            DataTable dt = crearDTProv(lista);

            foreach (Proveedor proveedor in lista)
            {
                dt.Rows.Add(proveedor.getId(),

                proveedor.getnombre());
            }
            GrillaProv.Columns[0].Visible = true;
            GrillaProv.DataSource = dt;
            GrillaProv.DataBind();
            GrillaProv.Columns[0].Visible = false;

        }

        protected void BtnBuscarProv_Click(object sender, EventArgs e)
        {
            if (TbBucProveedor.Text.Equals(""))
            {
                CargarProveedores();
            }
            else
            {
                BuscarProveedor();
            }
        }
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                string id = e.CommandArgument.ToString();


                foreach (GridViewRow row in GrillaProv.Rows)
                {

                    if (row.Cells[0].Text == id)
                    {
                        
                        string name = row.Cells[1].Text;
                        string idprov= row.Cells[0].Text;

                        lblProbID.Text = idprov;
                        lblprovNom.Text = name;
                        CargarProductos(int.Parse(lblProbID.Text));
                        break;
                    }
                }
            }
        }

        private DataTable crearDTProd(List<Producto> li)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Nombre");
            if (li.Count() == 0)
            {
                dt.Rows.Add(-10, "");
            }
            return dt;
        }

        private void CargarProductos(int id)
        {
            ProductoNegocio prn = new ProductoNegocio();
            List<Producto> lista = prn.BuscarProductosProv(id);


            DataTable dt = crearDTProd(lista);

            foreach (Producto producto in lista)
            {
                dt.Rows.Add(
                    producto.getIdProd(),
                    producto.getDescProd());
            }
            GrillaProducto.Columns[0].Visible = true;
            GrillaProducto.DataSource = dt;
            GrillaProducto.DataBind();
            GrillaProducto.Columns[0].Visible = false;
        }

        protected void gvItemsProd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                string id = e.CommandArgument.ToString();


                foreach (GridViewRow row in GrillaProducto.Rows)
                {

                    if (row.Cells[0].Text == id)
                    {

                        string name = row.Cells[1].Text;
                        string idprod = row.Cells[0].Text;

                        lblIdProd.Text = idprod;
                        lblProdNom.Text = name;

                        UpdatePanel4.Update();
                        break;
                    }
                }
            }
        }

        protected void menos_Click(object sender, EventArgs e)
        {
            int cant = int.Parse(TbCantidad.Text);
            Button btn = (Button)sender;
            if (btn.Text.Equals("-"))
            {
                if (cant != 0)
                {
                    cant--;
                    TbCantidad.Text = cant.ToString();
                }
            }
            else 
            {
                cant++;
                TbCantidad.Text = cant.ToString();
            }
        }
    }
}