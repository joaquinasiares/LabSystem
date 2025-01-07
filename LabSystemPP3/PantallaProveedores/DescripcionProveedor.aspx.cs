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
    public partial class DescripcionProveedor : System.Web.UI.Page
    {

        string id;
        string desc;
        string Cuit;
        string Nombre;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                CargarPagina();
            }
        }

        private void CargarPagina()
        {
            lblDescNom.Text = "";
            lblDescMostrar.Text = "";
            lblDescCuit.Text = "";
            //ID,idIva,Cuit,Nombre,Razon social,IVA,Ambito
            id = Request.QueryString["ID"];
            Cuit = Request.QueryString["Cuit"];
            Nombre = Request.QueryString["Nombre"];
            desc = Request.QueryString["Descripcion"];

            lblDescNom.Text = "Nombre: <br/>" + Nombre;
            lblDescCuit.Text = "Cuit: <br/>" + Cuit;
            lblDescMostrar.Text = "Descripcion: <br/><br/>" + desc;
        }


    }
}