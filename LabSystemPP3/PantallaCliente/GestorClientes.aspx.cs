using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaCliente
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            LblResOpe("");
            ModalLblResOpe("");
            if (!IsPostBack)
            {
                CargarClientes();
                CargarDatos();
            }
        }

        private void CargarClientes()
        {
            try
            {
                Grilla.DataSource = SqlDataSource1;
                Grilla.DataBind();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al cargar los clientes: " + ex.Message;
                LblResOpe(msg);
            }
        }


        //metod para mostrar alertas generales
        private void MostrarAlerta(string mensaje, NotificationType tipo)
        {
            string script = $@"
                Swal.fire({{
                title: '{mensaje}',
                icon: '{tipo.ToString().ToLower()}',
                confirmButtonText: 'OK',
                confirmButtonColor: '#28a745'
                }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarAlerta", script, true);
        }

        private void LblResOpe(string mensaje)
        {
            lblResultadoOperacion.Text = mensaje;
            updpLblResOpe.Update();
        }

        private void ModalLblResOpe(string mensaje)
        {
            ModallblResultadoOperacion.Text = mensaje;
            ModalupdpLblResOpe.Update();
        }

        protected void Grilla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                try
                {
                    lblTituloModal.Text = "Modificar Cliente";
                    udpTituloModal.Update();
                    lblCrear.Text = "";
                    limpiarCampos();
                    BtnAgregar.Visible = false;
                    BtnAgregar.Enabled = false;
                    BtnModificar.Visible = true;
                    BtnModificar.Enabled = true;

                    int index = Convert.ToInt32(e.CommandArgument);

                    string idCliente = Grilla.DataKeys[index].Values["ID_CLIENTE"].ToString();
                    lblIdCli.Text = idCliente;
                    int ambito = 0;
                    if (Grilla.Rows[index].Cells[4].Text.ToLower().Trim().Equals("publico")) { ambito = 1; }
                    listaAmbito.SelectedIndex = ambito;

                    int iva = 0;
                    if (Grilla.Rows[index].Cells[3].Text.ToLower().Trim().Equals("monotributista")) { iva = 1; }
                    else if (Grilla.Rows[index].Cells[3].Text.ToLower().Trim().Equals("exento")) { iva = 2; }
                    listaIva.SelectedIndex = iva;


                    txtCuit.Text = Grilla.Rows[index].Cells[1].Text;
                    txtNombre.Text = Grilla.Rows[index].Cells[2].Text;
                    txtRazonSocial.Text = Grilla.Rows[index].Cells[3].Text;
                    TxtTelefono.Text = Grilla.Rows[index].Cells[6].Text;
                    string email = Grilla.Rows[index].Cells[7].Text.Trim();
                    TxtEmail.Text = string.IsNullOrEmpty(email) || email == "&nbsp;" ? string.Empty : email;
                    RequiredFieldValidator1.IsValid = true;
                    RequiredFieldValidator2.IsValid = true;
                    UpdatePanel2.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al abrir la ventana modificar cliente: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }

            }
            else if (e.CommandName == "Borrar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = Grilla.Rows[index];
                clienteNegocio clienteNegocio = new clienteNegocio();
                if (!fila.Cells[0].Text.Equals("-10"))
                {
                    int id = int.Parse(fila.Cells[0].Text);
                    clienteNegocio.ClienteDelete(id);
                    Grilla.DataSource = null;
                    Grilla.DataBind();
                    //CargarClientes();
                }
            }
        }

        //metodo para manejar la alerta de borrado
        private void AlertaBorrar(string mensaje, NotificationType tipo)
        {

            string script = $@"
                Swal.fire({{
                title: '{mensaje}',
                icon: '{tipo.ToString().ToLower()}',
                showDenyButton: true,
                confirmButtonText: 'Borrar',
                denyButtonText: `Cancelar`,
                confirmButtonColor: '#3085d6',  // Color del botón de confirmar
                denyButtonColor: '#d33'         // Color del botón de negar (Deny)
                }}).then((result) => {{
                if (result.isConfirmed) {{
                    __doPostBack('UpdatePanel1', 'Borrar'); // Enviar al servidor
                }} else if (result.isDenied) {{
                    __doPostBack('UpdatePanel1', 'Deny'); // Enviar al servidor
                }}
                }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "borrarAlerta", script, true);
        }

        //metodo para confirmar el borrado
        private void ConfirmacionDeOpciones()
        {
            string target = Request.Form["__EVENTTARGET"];
            string argument = Request.Form["__EVENTARGUMENT"];

            if (target == "UpdatePanel1") // Asegúrate de que coincida con el UpdatePanel
            {
                if (argument == "Borrar")
                {
                    BorrarCliente();
                    Session["IdCliente"] = "0";
                }
                else { Session["IdCliente"] = "0"; }
            }
        }

        private void BorrarCliente()
        {
            try
            {
                SqlDataSource1.DeleteParameters["id"].DefaultValue = Session["IdCliente"].ToString();
                SqlDataSource1.Delete();
                Grilla.DataSourceID = null;
                Grilla.DataSource = SqlDataSource1;
                Grilla.DataBind();
                UpdatePanel1.Update();
                string mensaje = "Se elimino al cliente con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al borrar el cliente: " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }


        private DataTable crearDT(List<Cliente> li)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("idIva");
            dt.Columns.Add("Cuit");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Razon social");
            dt.Columns.Add("IVA");
            dt.Columns.Add("Ambito");
            if (li.Count() == 0)
            {
                dt.Rows.Add(-10, "", "", "", "", "", "");
            }
            return dt;
        }

        public void CargarDatos()
        {
            try
            {
                listaAmbito.Items.Add(new ListItem("publico", 0.ToString()));
                listaAmbito.Items.Add(new ListItem("privado", 1.ToString()));

                CondicionIvaNegocio ivaNegocio = new CondicionIvaNegocio();
                List<CondicionIva> lista = ivaNegocio.listarIva();
                foreach (CondicionIva iva in lista)
                {
                    string idVa = iva.getIdIva().ToString();
                    listaIva.Items.Add(new ListItem(iva.getDescIva(), idVa));
                }
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al cargar el ámbito del cliente o su relación frente al IVA: " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

            GridViewRow fila = Grilla.SelectedRow;

            listaAmbito.Items.Add(new ListItem("publico", 0.ToString()));
            listaAmbito.Items.Add(new ListItem("privado", 1.ToString()));

            CondicionIvaNegocio ivaNegocio = new CondicionIvaNegocio();
            List<CondicionIva> lista = ivaNegocio.listarIva();
            foreach (CondicionIva iva in lista)
            {
                listaIva.Items.Add(new ListItem(iva.getDescIva(), iva.getIdIva().ToString()));
            }
        }

        private void crearCliente()
        {
            if (!String.IsNullOrEmpty(txtCuit.Text) && !String.IsNullOrEmpty(txtNombre.Text))
            {
                try
                {
                    lblCrear.Text = "";
                    Cliente cliente = new Cliente();
                    cliente.setnombre(txtNombre.Text);
                    cliente.setrazonSocial(txtRazonSocial.Text);
                    cliente.setcuit(long.Parse(txtCuit.Text));
                    cliente.setidIva(int.Parse(listaIva.SelectedValue));
                    cliente.setprivPubid(int.Parse(listaAmbito.SelectedValue));
                    clienteNegocio clienteNegocio = new clienteNegocio();
                    int idCliente = clienteNegocio.insertar(cliente);

                    SqlDataSourceContacto.InsertParameters["idCli"].DefaultValue = idCliente.ToString();
                    SqlDataSourceContacto.InsertParameters["Tel"].DefaultValue = TxtTelefono.Text;
                    SqlDataSourceContacto.InsertParameters["Email"].DefaultValue = TxtEmail.Text;
                    SqlDataSourceContacto.Insert();
                    Grilla.DataSourceID = null;
                    Grilla.DataSource = SqlDataSource1;
                    Grilla.DataBind();
                    UpdatePanel1.Update();

                    MostrarAlerta("Se ha agregado un nuevo cliente", NotificationType.success);

                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al registrar el cliente: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }

            }
            else
            {
                string msg = "Complete todos los campos";
                MostrarAlerta(msg, NotificationType.warning);
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            crearCliente();
            //CargarClientes();
        }


        private void BuscarCliente()
        { /*
            clienteNegocio cn = new clienteNegocio();
            List<Cliente> lista = cn.buscarClientes(txtBuscar.Text);
            string ambito = "";
            DataTable dt = crearDT(lista);

            foreach (Cliente cliente in lista)
            {
                if (cliente.getprivPuba() == 0)
                {
                    ambito = "Publico";
                }
                else
                {
                    ambito = "Privado";
                }
                dt.Rows.Add(cliente.getId(),
                cliente.getIdIva(),
                cliente.getcuit(),
                cliente.getnombre(),
                cliente.getrazonSociak(),
                cliente.getDescIva(),
                ambito);
            }
            Grilla.DataSource = null;
            Grilla.DataSourceID = null;
            Grilla.Columns[0].Visible = true;
            Grilla.Columns[1].Visible = true;
            Grilla.DataSource = dt;
            Grilla.DataBind();
            Grilla.Columns[0].Visible = false;
            Grilla.Columns[1].Visible = false;*/

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Equals(""))
            {
                try
                {
                    // Desvincular cualquier DataSource previamente asignado
                    Grilla.DataSourceID = null;
                    Grilla.DataSource = SqlDataSource1;  // Asignar directamente el SqlDataSource
                    Grilla.DataBind();  // Enlazar los datos
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al cargar los clientes: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
            else
            {
                try
                {
                    // Cambiar el parámetro de búsqueda antes de enlazar la nueva fuente de datos
                    SqlDataSource2.SelectParameters["Nombre"].DefaultValue = txtBuscar.Text;

                    // Desvincular cualquier DataSource previamente asignado
                    Grilla.DataSourceID = null;
                    Grilla.DataSource = SqlDataSource2;  // Asignar directamente el SqlDataSource
                    Grilla.DataBind();  // Enlazar los datos
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al buscar el cliente: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
        }

        protected void Grilla_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grilla.PageIndex = e.NewPageIndex;
            if (txtBuscar.Text.Equals(""))
            {
                try
                {
                    Grilla.DataSource = SqlDataSource1;
                    Grilla.DataBind();
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al cambiar la pagina de la tabla cliente: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
            else
            {
                try
                {
                    SqlDataSource2.SelectParameters["Nombre"].DefaultValue = txtBuscar.Text;
                    Grilla.DataSource = SqlDataSource2;
                    Grilla.DataBind();
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al cambiar la pagina de la tabla cliente: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("AltaCliente.aspx");
        }

        protected void Grilla_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Session["IdCliente"] = Grilla.DataKeys[e.RowIndex].Values["ID_CLIENTE"].ToString();
            AlertaBorrar("¿Seguro que quiere borrar al cliente?", NotificationType.warning);
        }

        protected void imgUpdate_Click(object sender, ImageClickEventArgs e)
        {

        }

        private void limpiarCampos()
        {
            try
            {
                txtCuit.Text = "";
                txtNombre.Text = "";
                txtRazonSocial.Text = "";
                TxtEmail.Text = "";
                TxtTelefono.Text = "";
                listaAmbito.SelectedIndex = 0;
                listaIva.SelectedIndex = 0;
                lblIdCli.Text = "";
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al limpiar los campos: " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        private void modificarCliente()
        {
            if (!String.IsNullOrEmpty(lblIdCli.Text))
            {

                try
                {
                    Cliente cliente = new Cliente();
                    cliente.setId(int.Parse(lblIdCli.Text));
                    cliente.setnombre(txtNombre.Text);
                    cliente.setrazonSocial(txtRazonSocial.Text);
                    cliente.setcuit(long.Parse(txtCuit.Text));
                    cliente.setidIva(int.Parse(listaIva.SelectedValue));
                    cliente.setprivPubid(int.Parse(listaAmbito.SelectedValue));
                    clienteNegocio clienteNegocio = new clienteNegocio();
                    clienteNegocio.ClienteUpdate(cliente);

                    SqlDataSourceContacto.UpdateParameters["idCli"].DefaultValue = lblIdCli.Text;
                    SqlDataSourceContacto.UpdateParameters["Tel"].DefaultValue = TxtTelefono.Text;
                    SqlDataSourceContacto.UpdateParameters["Email"].DefaultValue = TxtEmail.Text;
                    SqlDataSourceContacto.Update();
                    Grilla.DataSourceID = null;
                    Grilla.DataSource = SqlDataSource1;
                    Grilla.DataBind();
                    UpdatePanel1.Update();

                    MostrarAlerta("Se ha modificado el cliente", NotificationType.success);
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al modificar el cliente: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
            else
            {
                string msg = "Seleccione un nuevo cliente para modificar";
                ModalLblResOpe(msg);
            }
        }

        protected void BtnModificar_Click(object sender, EventArgs e)
        {

            modificarCliente();
        }

        protected void BtnModal_Click(object sender, EventArgs e)
        {
            try
            {
                lblTituloModal.Text = "Agregar Cliente";
                limpiarCampos();
                RequiredFieldValidator1.IsValid = true;
                RequiredFieldValidator2.IsValid = true;
                lblCrear.Text = "";
                BtnAgregar.Visible = true;
                BtnAgregar.Enabled = true;
                BtnModificar.Visible = false;
                BtnModificar.Enabled = false;
                udpTituloModal.Update();
                UpdatePanel2.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al abrir la ventana Agregar cliente";
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        protected void BtnCerrarModal_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "cerar();", true);
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al cerrar la ventana";
                MostrarAlerta(msg, NotificationType.error);
            }
        }
    }
}