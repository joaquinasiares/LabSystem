using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaProveedores
{
    public partial class GestorProveedores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            LblResOpe("");
            ModalLblResOpe("");

            if (!IsPostBack)
            {
                Session["ProveedorSeleccionado"] = false;
                CargarProveedores();
            }
        }

        private void CargarProveedores()
        {
            /*ProveedorNegocio prn = new ProveedorNegocio();
            List<Proveedor> lista = prn.listarProveedores();

            DataTable dt = crearDT(lista);

            foreach (Proveedor proveedor in lista)
            {
                dt.Rows.Add(proveedor.getId(),
                proveedor.getDesc(),
                proveedor.getcuit(),
                proveedor.getnombre());
            }
            Grilla.Columns[0].Visible = true;
            Grilla.Columns[1].Visible = true;
            Grilla.DataSource = dt;
            Grilla.DataBind();
            Grilla.Columns[0].Visible = false;
            Grilla.Columns[1].Visible = false;*/
            try
            {
                Grilla.DataSourceID = null;
                Grilla.DataSource = SqlDataSource1;
                Grilla.DataBind();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los proveedores: " + ex.Message;
                LblResOpe(mensaje);
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
        protected void GrillaProveedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                /*/ Obtener el índice de la fila seleccionada
                int index = Convert.ToInt32(e.CommandArgument);

                // Marcar la fila seleccionada
                GrillaProveedores.SelectedIndex = index;

                // Opcional: Guardar el ID del proveedor seleccionado en un Label u otro control
                GridViewRow selectedRow = GrillaProveedores.Rows[index];
                string proveedorID = selectedRow.Cells[0].Text; // Asume que la columna 0 tiene el ID
                lblProvID.Text = proveedorID;
           */
            }
        }


        //metodo para manejar la alerta de borrar
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
                    BorrarProveedor();
                    Session["idProv"] = "0";
                }
                else { Session["idProv"] = "0"; }
            }
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
                    lblTituloModal.Text = "Modificar Proveedor";
                    udpTituloModal.Update();
                    limpiarCampos();
                    BtnAgregar.Visible = false;
                    BtnAgregar.Enabled = false;
                    BtnModificar.Visible = true;
                    BtnModificar.Enabled = true;
                    int index = Convert.ToInt32(e.CommandArgument);
                    string idProv = Grilla.DataKeys[index].Values["ID_PROV"].ToString();
                    lblprov.Text = idProv;
                    Session["ProveedorSeleccionado"] = false;

                    txtCuit.Text = Grilla.Rows[index].Cells[1].Text;
                    txtNombre.Text = Grilla.Rows[index].Cells[2].Text;
                    txtDesc.Text = Grilla.Rows[index].Cells[3].Text;
                    txtTel.Text = Grilla.Rows[index].Cells[4].Text;
                    string email = Grilla.Rows[index].Cells[5].Text.Trim();
                    txtEmail.Text = string.IsNullOrEmpty(email) || email == "&nbsp;" ? string.Empty : email;
                    RequiredFieldValidator1.IsValid = true;
                    RequiredFieldValidator2.IsValid = true;
                    UpdatePanel2.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);

                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al mostrar la ventana modificar proveedor: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }

            if (e.CommandName == "Select")
            {

                try
                {

                    int index = Convert.ToInt32(e.CommandArgument);

                    GridViewRow fila = Grilla.Rows[index];

                    int idProveedor = int.Parse(Grilla.DataKeys[index].Values["ID_PROV"].ToString());
                    string nombreProveedor = fila.Cells[2].Text;

                    Session["ProveedorSeleccionado"] = true;
                    Session["idProveedor"] = idProveedor;
                    Session["nombreProveedor"] = nombreProveedor;
                }

                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al seleccinar al proveedor: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                Session["idProveedor"] = 0;
                Session["nombreProveedor"] = "";
            }
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow fila = Grilla.Rows[index];
                    ProveedorNegocio proveedorNegocio = new ProveedorNegocio();
                    if (!fila.Cells[0].Text.Equals("-10"))
                    {
                        Session["ProveedorSeleccionado"] = false;
                        int id = int.Parse(Grilla.DataKeys[index].Values["ID_PROV"].ToString());
                        proveedorNegocio.ProveedorDelete(id);
                        Grilla.DataSource = null;
                        Grilla.DataBind();
                        CargarProveedores();
                    }
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al intentar borrar al proveedor: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        private DataTable crearDT(List<Proveedor> li)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("Cuit");
            dt.Columns.Add("Nombre");
            if (li.Count() == 0)
            {
                dt.Rows.Add(-10, "", "", "");
            }
            return dt;
        }

        private void limpiarCampos()
        {
            txtCuit.Text = "";
            txtDesc.Text = "";
            txtNombre.Text = "";
            txtTel.Text = "";
            txtEmail.Text = "";
            lblprov.Text = "";
        }

        private void crearProveedor()
        {
            if (RequiredFieldValidator1.IsValid && RequiredFieldValidator2.IsValid)
            {
                try
                {
                    Proveedor proveedor = new Proveedor();
                    proveedor.setnombre(txtNombre.Text);
                    proveedor.setDesc(txtDesc.Text);
                    proveedor.setcuit(long.Parse(txtCuit.Text));
                    ProveedorNegocio proveedorNegocio = new ProveedorNegocio();
                    int idProv = proveedorNegocio.insertar(proveedor);
                    SqlDataSource1.InsertParameters["idProv"].DefaultValue = idProv.ToString();
                    SqlDataSource1.InsertParameters["Tel"].DefaultValue = txtTel.Text;
                    SqlDataSource1.InsertParameters["Email"].DefaultValue = txtEmail.Text;
                    SqlDataSource1.Insert();
                    Grilla.DataSourceID = null;
                    Grilla.DataSource = SqlDataSource1;
                    Grilla.DataBind();
                    UpdatePanel1.Update();
                    limpiarCampos();
                    string mensaje = "Se registro el proveedor con éxito";
                    MostrarAlerta(mensaje, NotificationType.success);
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al registrar al proveedor: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            crearProveedor();
        }


        private void BuscarProveedor()
        {
            ProveedorNegocio prn = new ProveedorNegocio();
            List<Proveedor> lista = prn.buscarProvedores(txtBuscar.Text);

            DataTable dt = crearDT(lista);

            foreach (Proveedor proveedor in lista)
            {

                dt.Rows.Add(proveedor.getId(),
                proveedor.getDesc(),
                proveedor.getcuit(),
                proveedor.getnombre());
            }
            Grilla.Columns[0].Visible = true;
            Grilla.Columns[1].Visible = true;
            Grilla.DataSource = dt;
            Grilla.DataBind();
            Grilla.Columns[0].Visible = false;
            Grilla.Columns[1].Visible = false;

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
                    string mensaje = "Ocurrió un error al buscar a los proveedores: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
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
                    string mensaje = "Ocurrió un error al buscar a los proveedores: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);

                }
            }
        }

        private void ModificarProveedor()
        {
            if (!String.IsNullOrEmpty(lblprov.Text))
            {
                try
                {
                    ProveedorNegocio provN = new ProveedorNegocio();
                    Proveedor prov = new Proveedor();
                    prov.setId(int.Parse(lblprov.Text));
                    prov.setnombre(txtNombre.Text);
                    prov.setcuit(long.Parse(txtCuit.Text));
                    prov.setDesc(txtDesc.Text);
                    provN.ProveedorUpdate(prov);

                    SqlDataSource1.UpdateParameters["idProv"].DefaultValue = lblprov.Text;
                    SqlDataSource1.UpdateParameters["Tel"].DefaultValue = txtTel.Text;
                    SqlDataSource1.UpdateParameters["Email"].DefaultValue = txtEmail.Text;
                    SqlDataSource1.Update();
                    string mensaje = "se ah modificado el proveedor";
                    MostrarAlerta(mensaje, NotificationType.success);
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al modificar el proveedor: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
                Grilla.DataSourceID = null;
                Grilla.DataSource = SqlDataSource1;
                Grilla.DataBind();
                UpdatePanel1.Update();

                limpiarCampos();


            }
            else { ModalLblResOpe("Seleccione un nuevo proveedor para modificar"); }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            ModificarProveedor();
        }

        protected void BtnModal_Click(object sender, EventArgs e)
        {
            try
            {
                lblTituloModal.Text = "Agregar Proveedor";
                limpiarCampos();
                RequiredFieldValidator1.IsValid = true;
                RequiredFieldValidator2.IsValid = true;
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
                string mensaje = "Ocurrió un error al abrir la ventana agregar/modificar proveedor: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
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
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al cambiar de página la tabla proveedores: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                try
                {
                    SqlDataSource2.SelectParameters["Nombre"].DefaultValue = txtBuscar.Text;
                    Grilla.DataSource = SqlDataSource2;
                    Grilla.DataBind();
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al cambiar de página la tabla proveedores: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        private void BorrarProveedor()
        {
            try
            {
                SqlDataSource1.DeleteParameters["id"].DefaultValue = Session["idProv"].ToString();
                SqlDataSource1.Delete();
                Grilla.DataSourceID = null;
                Grilla.DataSource = SqlDataSource1;
                Grilla.DataBind();
                UpdatePanel1.Update();
                MostrarAlerta("Se borro al proveedor con éxito", NotificationType.success);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al borrar al proveedores: " + ex.Message;
                updpLblResOpe.Update();
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void Grilla_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Session["idProv"] = Grilla.DataKeys[e.RowIndex].Values["ID_PROV"].ToString();
            AlertaBorrar("¿Seguro que quiere borrar al proveedor?", NotificationType.warning);
        }
        protected void BtnCerrarModal_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "cerar();", true);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cerrar la ventana: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnNuevaOrdComp_Click(object sender, EventArgs e)
        {
            try
            {
                bool seleccion=(bool)Session["ProveedorSeleccionado"];

                if (seleccion)
                {

                    int id = int.Parse(Session["idProveedor"].ToString());
                    string Rs = Session["nombreProveedor"].ToString();

                    string url = $"~/PantallaOrdendecompra/NuevaOrdenDeCompra.aspx?id={id}&rs={Rs}";
                    Response.Redirect(url);
                }
                else
                {
                    string mensaje = "Seleccione un proveedor";
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al intentar realizar una orden de compra: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
    }
}