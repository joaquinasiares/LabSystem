using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.PantallaCliente
{
    public partial class WebForm2 : System.Web.UI.Page
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
                string idVa = iva.getIdIva().ToString();
                listaIva.Items.Add(new ListItem(iva.getDescIva(), idVa));
            }
            txtNombre.Text = Nombre;
            txtCuit.Text = Cuit;
            txtRazonSocial.Text = rs;
            int idIVA = int.Parse(idIva)-1;
            lblID.Text = id;
            lblIdIVA.Text = (idIVA + 1).ToString();
            listaIva.SelectedIndex = int.Parse(idIva) - 1;
            listaAmbito.SelectedIndex = amb;
        }

        private void Cargarcliente() 
        {
                int idIVA = 0;
                if (listaIva.SelectedValue.Equals("Responsable Inscripto"))
                {
                    idIVA = 1;
                }
                else if (listaIva.SelectedValue.Equals("Monotributista"))
                {
                    idIVA = 2;
                }
                else { idIVA = 3; }
                Cliente cli = new Cliente();
                cli.setId(int.Parse(lblID.Text));
                int iva=listaIva.SelectedIndex+1;
                cli.setcuit((long)double.Parse(txtCuit.Text));
                cli.setidIva(iva);
                cli.setnombre(txtNombre.Text);
                cli.setrazonSocial(txtRazonSocial.Text);
                cli.setprivPubid(int.Parse(listaAmbito.SelectedIndex.ToString()));
                clienteNegocio cn = new clienteNegocio();
                cn.ClienteUpdate(cli);
                Label1.Text = listaIva.SelectedIndex.ToString()+" iva con suma "+iva;
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Cargarcliente();

        }
    }
}