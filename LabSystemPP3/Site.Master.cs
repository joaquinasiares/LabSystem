using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["NombreUsuario"] == null)
            {
                Response.Redirect("~/Login/Login.aspx");
            }
            else
            {
                FiltarContenido();
            }
        }

        private void FiltarContenido() 
        {
            int idTipo = int.Parse(Session["IdTipoUsuario"].ToString());
            if (idTipo==3) 
            {
                HyperLink6.Visible = false;
                HyperLink6.Enabled = false;
                HyperLink2.Visible = false;
                HyperLink2.Enabled = false;
                HyperLink10.Visible = false;
                HyperLink10.Enabled = false;
                HyperLink9.Enabled = false;
                HyperLink9.Visible = false;
                HyperLink3.Visible = false;
                HyperLink3.Enabled = false;
                HyperLink8.Visible = false;
                HyperLink8.Enabled = false;
            }

            if (idTipo==2) 
            {
                HyperLink1.Visible=false; 
                HyperLink1.Enabled=false;
                HyperLink4.Visible=false;
                HyperLink4.Enabled=false;
                HyperLink8.Visible = false;
                HyperLink8.Enabled = false;
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login/Login.aspx");
        }
    }
}