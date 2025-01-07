using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.PantallaProveedores
{
    public partial class Modificar : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPagina();
            }
        }

        private void CargarPagina()
        {
            TbNom.Text = "";
            TbCuit.Text = "";
            TbDesc.Text = "";

            TbId.Text = Request.QueryString["ID"];
            TbNom.Text= Request.QueryString["Nombre"];
            TbCuit.Text= Request.QueryString["Cuit"];
            TbDesc.Text= Request.QueryString["Descripcion"];
        }

        private void Actualizar() 
        { 

            Proveedor proveedor = new Proveedor();
            proveedor.setId(int.Parse(TbId.Text));
            proveedor.setcuit((long)double.Parse(TbCuit.Text));
            proveedor.setnombre(TbNom.Text);
            proveedor.setDesc(TbDesc.Text);

            ProveedorNegocio proveedorNegocio = new ProveedorNegocio();
            proveedorNegocio.ProveedorUpdate(proveedor);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Actualizar();
        }
    }
}