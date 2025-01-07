using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.PantallaProductos
{
    public partial class ModificarProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                cargarDDLS();
                CargarDatos();
            }
        }

        private void cargarDDLS()
        {

            DDLTipo.Items.Add(new ListItem("Insumo", 1.ToString()));
            DDLTipo.Items.Add(new ListItem("Equipo", 2.ToString()));
            ProveedorNegocio prov = new ProveedorNegocio();
            List<Proveedor> listaprov = prov.listarProveedores();

            foreach (Proveedor proveedor in listaprov)
            {
                string idProv = proveedor.getId().ToString();
                DDLProv.Items.Add(new ListItem(proveedor.getnombre(), idProv));
            }

        }

        private void CargarDatos() 
        {
            //ID={0}&IDProv={1}&IDTipo={2}&Nombre={3}&Codigo={4}&Lote={5}&Proveedor={6}&Tipo={7}&Fecha de Ingreso={8}&Fecha de Vencimiento={9}&Estado={10}&Precio de compra={11}"
            
            DDLTipo.SelectedValue= Request.QueryString["IDTipo"];
            DDLProv.SelectedValue= Request.QueryString["IDProv"];

            TbNom.Text = Request.QueryString["Nombre"];
            TbCod.Text = Request.QueryString["Codigo"];
            TbLote.Text = Request.QueryString["Lote"];
            string fecha = Request.QueryString["Fecha de Vencimiento"];
            Calendar1.SelectedDate = DateTime.Parse(fecha);
            TbPreciCom.Text = Request.QueryString["Precio de compra"];
            TbCantidad.Text = Request.QueryString["Cantidad"];
            TbPreVenPub.Text = Request.QueryString["PreVenPub"];
            TbPreVenPriv.Text = Request.QueryString["PreVenPriv"];
        }
        
        private void Modificar() 
        { 
            Proveedor proveedor = new Proveedor();
            proveedor.setId(int.Parse(DDLProv.SelectedValue));

            TipoProducto tipoProducto = new TipoProducto();
            tipoProducto.setIdTipo(int.Parse(DDLTipo.SelectedValue));

            Stock stock = new Stock();
            stock.setIdStock(int.Parse(Request.QueryString["IDStock"]));
            stock.setCantidad(int.Parse(TbCantidad.Text));

            Producto producto = new Producto();
            producto.setIdProd(int.Parse(Request.QueryString["productoID"]));
            producto.setProveedor(proveedor);
            producto.setTipo(tipoProducto);
            producto.setCodProd(TbCod.Text);
            producto.setDescProd(TbNom.Text);
            DateTime vto = Calendar1.SelectedDate.Date;
            producto.setFechaVTO(vto);
            producto.setPrecioCompra(Decimal.Parse(TbPreciCom.Text));
            producto.setLote(TbLote.Text);
            producto.setStock(stock);
            producto.setPrecioVentaPub(Decimal.Parse(TbPreVenPub.Text));
            producto.setPrecioVentaPriv(Decimal.Parse(TbPreVenPriv.Text));

            ProductoNegocio productoNegocio = new ProductoNegocio();
            productoNegocio.Modificar(producto);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Modificar();
        }
    }
}