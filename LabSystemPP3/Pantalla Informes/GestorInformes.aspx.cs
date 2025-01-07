using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.Pantalla_Informes
{
    public partial class Gestor_Informes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void imgBtn1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pantalla Informes/TopCincoProductos.aspx");
        }

        protected void btnTop3Servicio_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pantalla Informes/TresEquiposReparacion.aspx");
        }

        protected void BtnPromedoVentasCli_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pantalla Informes/PromedioVentasMensualesClientes.aspx");
        }

        protected void btnServiciosMens_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pantalla Informes/PromedioServiciosMensuales.aspx");
        }

        protected void BtnMasComprados_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pantalla Informes/ProductosMasComprados.aspx");
        }
    }
}