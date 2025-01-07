using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3
{
    public partial class ALTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarComboIva();
                cargarAmbito();
                cargarTablaContacto();
            }
        }
        private void crearCliente() { 
            Cliente cliente = new Cliente();
            cliente.setnombre(txtNombre.Text);
            cliente.setrazonSocial(txtRazonSocial.Text);
            cliente.setcuit(long.Parse(txtCuit.Text));
            cliente.setidIva(int.Parse(listaIva.SelectedValue));
            cliente.setprivPubid(int.Parse(listaAmbito.SelectedValue));
            clienteNegocio clienteNegocio = new clienteNegocio();
            int id =clienteNegocio.insertar(cliente);
            cargarContacto(id);
        }

        private void cargarTablaContacto() 
        {
            if (Session["Contacto"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Contacto");
                dt.Columns.Add("Descripcion");
                Session["Contacto"] = dt;
            }
        }
        private void cargarContacto(int id) 
        {
            if (id > 0) 
            {
                if (GrillaContacto.Rows.Count>0) 
                {
                    foreach (GridViewRow row in GrillaContacto.Rows) 
                    {
                        SqlContacto.InsertParameters["ID_CLIENTE"].DefaultValue = id.ToString();
                        SqlContacto.InsertParameters["Contacto"].DefaultValue = row.Cells[0].Text;
                        SqlContacto.InsertParameters["Descripcion"].DefaultValue = row.Cells[1].Text;
                        SqlContacto.Insert();
                    }
                }
            }
        } 
        
        private void cargarAmbito() {
            listaAmbito.Items.Add(new ListItem("Privado", 0.ToString()));
            listaAmbito.Items.Add(new ListItem("Publico", 1.ToString()));
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
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (RequiredFieldValidator1.IsValid &&
                RequiredFieldValidator2.IsValid &&
                RequiredFieldValidator3.IsValid||!TbContacto.Text.Equals(""))
            {
                if (TbContacto.Text.Equals(""))
                {
                    crearCliente();
                    txtCuit.Text = "";
                    txtNombre.Text = "";
                    txtRazonSocial.Text = "";

                    Session["Contacto"] = null;
                    GrillaContacto.DataSource = null;
                    GrillaContacto.DataBind();
                    UpdatePanel1.Update();
                    lblResultado.Text = "Se Agrego un cliente";
                }
                else { lblResultado.Text = "Hay un contacto para agregar"; }
            }
            else {
                lblResultado.Text = "LLene los campos con *"; 
            }
        }

        private void AgregarContacto() 
        {
            string Contacto = TbContacto.Text;
            if (!string.IsNullOrEmpty(Contacto))
            {
                string Descripcion = TbDesCont.Text;

                DataTable dt = (DataTable)Session["Contacto"];

                DataRow dr = dt.NewRow();
                dr["Contacto"] = Contacto;
                dr["Descripcion"] = Descripcion;
                dt.Rows.Add(dr);

                Session["Contacto"] = dt;
                GrillaContacto.DataSource = dt;
                GrillaContacto.DataBind();
                UpdatePanel1.Update();
                TbContacto.Text = "";
                TbDesCont.Text = "";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AgregarContacto();
        }
    }
}