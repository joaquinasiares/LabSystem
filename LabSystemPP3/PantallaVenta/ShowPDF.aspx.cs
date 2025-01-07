using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.PantallaVenta
{
    public partial class ShowPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ruta= Request.QueryString["url_ubicacion"];
            string fileName = Request.QueryString["nombre"];
           if (!string.IsNullOrEmpty(fileName))
            {
                pdfViewer.Attributes["src"] = ResolveUrl(ruta + fileName);

            }
            Label1.Text = fileName;
        }
    }
}