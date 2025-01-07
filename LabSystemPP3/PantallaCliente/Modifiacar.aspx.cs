using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.PantallaCliente
{
    public partial class Modifiacr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                cargarAmbito();
                CargarComboIva();
                CargarPagina();
            }
        }

        private void cargarAmbito()
        {
            listaAmbito.Items.Add(new ListItem("publico", 0.ToString()));
            listaAmbito.Items.Add(new ListItem("privado", 1.ToString()));
        }
        private void CargarComboIva()
        {
            CondicionIvaNegocio ivaNegocio = new CondicionIvaNegocio();
            List<CondicionIva> lista = ivaNegocio.listarIva();
            foreach (CondicionIva iva in lista)
            {
                listaIva.Items.Add(new ListItem(iva.getDescIva(), iva.getIdIva().ToString()));
            }

        }

        private void CargarPagina()
        {
            listaAmbito.Items.Clear();
            listaIva.Items.Clear();
            //ID,idIva,Cuit,Nombre,Razon social,IVA,Ambito
            string id = Request.QueryString["ID"];
            string idIva = Request.QueryString["idIva"];
            string Cuit = Request.QueryString["Cuit"];
            string Nombre = Request.QueryString["Nombre"];
            string rs = Request.QueryString["Razon social"];
            string IVA = Request.QueryString["IVA"];
            string ambito = Request.QueryString["Ambito"];
            int amb = 0;
            if (ambito.Equals("Privado")) { amb = 1; }

            listaAmbito.Items.Add(new ListItem("publico", 0.ToString()));
            listaAmbito.Items.Add(new ListItem("privado", 1.ToString()));

            CondicionIvaNegocio ivaNegocio = new CondicionIvaNegocio();
            List<CondicionIva> lista = ivaNegocio.listarIva();
            foreach (CondicionIva iva in lista)
            {
                listaIva.Items.Add(new ListItem(iva.getDescIva(), iva.getIdIva().ToString()));
            }
            tbNom.Text = Nombre;
            tbCuit.Text = Cuit;
            tbRS.Text = rs;
            int idIVA = int.Parse(idIva) - 1;
            listaIva.SelectedIndex = int.Parse(idIva) - 1;
            listaAmbito.SelectedIndex = amb;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Cliente cli = new Cliente();
            cli.setId(int.Parse(tbId.Text));
            cli.setcuit((long)double.Parse(tbCuit.Text));
            cli.setidIva(int.Parse(listaIva.SelectedIndex.ToString()));
            cli.setnombre(tbNom.Text);
            cli.setrazonSocial(tbRS.Text);
            cli.setprivPubid(int.Parse(listaAmbito.SelectedIndex.ToString()));
            clienteNegocio cn = new clienteNegocio();
            cn.ClienteUpdate(cli);
        }
    }
}