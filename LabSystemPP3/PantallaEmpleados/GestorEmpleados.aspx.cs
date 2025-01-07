using CapaEntidades;
using CapaNegocios;
using LabSystemPP3.Notificacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaEmpleados
{
    public partial class GestorEmpleados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            if (!IsPostBack)
            {
                CargarEmpleado();
                CargarDDL();
            }
        }


        private void CargarEmpleado()
        {
            try
            {
                GrillaEmpleado.DataSource = SqlEmpleado;
                GrillaEmpleado.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al cargar los empleados: " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        private void CargarDDL() 
        { 
            CheckBoxList1.DataSource= SqltipoEmp;
            CheckBoxList1.DataBind();
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
                    BorrarEmpleado();
                    Session["IdEmpleado"] = "0";
                }
                else { Session["IdEmpleado"] = "0"; }
            }
        }
        private void LimpiarCampos()
        {
            Tbnom.Text = "";
            TbDni.Text = "";
            TbApe.Text = "";
            TbTel.Text = "";
            TbEmail.Text = "";
            foreach (ListItem item in CheckBoxList1.Items)
            {
                item.Selected = false;
            }
        }

        private void busquedaEmpleado()
        {
            try
            {
                SqlBuscarEmpleado.SelectParameters["nom"].DefaultValue = txtBuscar.Text;
                GrillaEmpleado.DataSource = SqlBuscarEmpleado;
                GrillaEmpleado.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                CargarEmpleado();
                string msg = "Ocurrió un error al buscar el empleado " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Equals(""))
            {
                CargarEmpleado();
            }
            else { busquedaEmpleado(); }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void GrillaEmpleado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaEmpleado.PageIndex = e.NewPageIndex;
            if (txtBuscar.Text.Equals(""))
            {
                CargarEmpleado();
            }
            else { busquedaEmpleado(); }
        }

        private void ModificarEmpleado()
        {
           // try
            //{
                SqlEmpleado.UpdateParameters["idEmp"].DefaultValue = lblIdEmp.Text;
                SqlEmpleado.UpdateParameters["dni"].DefaultValue = TbDni.Text;
                SqlEmpleado.UpdateParameters["nombre"].DefaultValue = Tbnom.Text;
                SqlEmpleado.UpdateParameters["apellido"].DefaultValue = TbApe.Text;

                if (TbTel.Text.Equals(""))
                {
                    SqlEmpleado.UpdateParameters["telefono"].DefaultValue = null;
                }
                else
                {
                    SqlEmpleado.UpdateParameters["telefono"].DefaultValue = TbTel.Text;
                }

                if (TbEmail.Text.Equals(""))
                {
                    SqlEmpleado.UpdateParameters["email"].DefaultValue = null;
                }
                else
                {
                    SqlEmpleado.UpdateParameters["email"].DefaultValue = TbEmail.Text;
                }
                SqlEmpleado.Update();
                foreach (ListItem item in CheckBoxList1.Items)
                {
                    SqlAreas.UpdateParameters["idEmp"].DefaultValue = lblIdEmp.Text;
                    SqlAreas.UpdateParameters["idArea"].DefaultValue = item.Value.ToString();
                    if (item.Selected)
                    {
                        SqlAreas.UpdateParameters["visible"].DefaultValue = "1";
                    }
                    else { SqlAreas.UpdateParameters["visible"].DefaultValue = "0"; }
                    SqlAreas.Update();
                }
            /*}
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al modificar el empleado " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }*/
        }

        protected void GrillaEmpleado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                try
                {
                    lblTituloModal.Text = "Modificar empleado";
                    udpTituloModal.Update();
                    LimpiarCampos();
                    BtnAgregar.Visible = false;
                    BtnAgregar.Enabled = false;
                    BtnModificar.Visible = true;
                    BtnModificar.Enabled = true;
                    lblError.Text = "";
                    int index = Convert.ToInt32(e.CommandArgument);
                    lblIdEmp.Text = GrillaEmpleado.DataKeys[index].Values["ID_EMPLEADO_SERVICIO"].ToString();

                    TbDni.Text = GrillaEmpleado.DataKeys[index].Values["DNI_EMP"].ToString();
                    Tbnom.Text = GrillaEmpleado.DataKeys[index].Values["NOMBRE_EMP"].ToString();
                    TbApe.Text = GrillaEmpleado.DataKeys[index].Values["APELLIDO_EMP"].ToString();

                    string tel = GrillaEmpleado.DataKeys[index].Values["Telefono"].ToString();
                    TbTel.Text = string.IsNullOrEmpty(tel) || tel == "&nbsp;" ? string.Empty : tel;

                    string email = GrillaEmpleado.DataKeys[index].Values["Email"].ToString();
                    TbEmail.Text = string.IsNullOrEmpty(email) || email == "&nbsp;" ? string.Empty : email;

                    int idEmpleado = Convert.ToInt32(GrillaEmpleado.DataKeys[index].Values["ID_EMPLEADO_SERVICIO"]);


                    SqlAreas.SelectParameters["idEmp"].DefaultValue = idEmpleado.ToString();

                    var areas = SqlAreas.Select(DataSourceSelectArguments.Empty) as DataView;

                    foreach (ListItem item in CheckBoxList1.Items)
                    {

                        foreach (DataRowView row in areas)
                        {
                            if (item.Value == row["idTipoEmp"].ToString())
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }

                    UpdatePanel2.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al abrir la ventana modificar empleado: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            } else if (e.CommandName == "Areas")
            {
                try {
                    int index = Convert.ToInt32(e.CommandArgument);
                    SqlAreas.SelectParameters["idEmp"].DefaultValue = GrillaEmpleado.DataKeys[index].Values["ID_EMPLEADO_SERVICIO"].ToString();
                    GridViewAreas.DataSource = SqlAreas;
                    GridViewAreas.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModalAreas();", true);
                }
                catch (Exception ex) 
                {
                    string msg = "Ocurrió un error al abrir la ventana Ver areas: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
        }

        //
        private void BorrarEmpleado()
        {
            try
            {

                string idEmpleado = Session["IdEmpleado"].ToString();
                SqlEmpleado.DeleteParameters["idEmpleado"].DefaultValue = idEmpleado;
                SqlEmpleado.Delete();
                CargarEmpleado();
                string mensaje = "Se elimino al empleado con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {

                string mensaje = "Ocurrió un error al eliminar el empleado: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }
        protected void GrillaEmpleado_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Session["IdEmpleado"] = GrillaEmpleado.DataKeys[e.RowIndex].Values["ID_EMPLEADO_SERVICIO"].ToString();
            AlertaBorrar("¿Seguro que quiere borrar al empleado?", NotificationType.warning);
        }



        private void InsertarEmpleado()
        {
            try
            {
                Empleado empleado = new Empleado();
                empleado.dni = int.Parse(TbDni.Text);
                empleado.nombre= Tbnom.Text;
                empleado.apellido = TbApe.Text;
                if (TbTel.Text.Equals(""))
                {
                    empleado.telefono = -1;
                }
                else
                {
                    empleado.telefono = long.Parse(TbTel.Text);
                }

                if (TbEmail.Text.Equals(""))
                {
                    empleado.email = null;
                }
                else
                {
                    empleado.email = TbEmail.Text;
                }

                EmpleadoNegocio empleadoNegocio= new EmpleadoNegocio();

                int idEmpleado = empleadoNegocio.insertar(empleado);

                foreach (ListItem item in CheckBoxList1.Items)
                {
                    SqlAreas.InsertParameters["idEmp"].DefaultValue = idEmpleado.ToString();
                    SqlAreas.InsertParameters["idArea"].DefaultValue = item.Value.ToString();
                    if (item.Selected)
                    {
                        SqlAreas.InsertParameters["visible"].DefaultValue = "1";
                    }
                    else { SqlAreas.InsertParameters["visible"].DefaultValue = "0"; }
                    SqlAreas.Insert();
                }
                GrillaEmpleado.DataSource = SqlEmpleado;
                GrillaEmpleado.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar el empleado" + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }
        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            Page.Validate("empDatos");
            if (Page.IsValid)
            {
                InsertarEmpleado();
                CargarEmpleado();
                LimpiarCampos();
            }
        }

        protected void BtnAbrirModal_Click(object sender, EventArgs e)
        {
            try
            {
                lblTituloModal.Text = "Agregar empleado";
                udpTituloModal.Update();
                BtnAgregar.Visible = true;
                BtnAgregar.Enabled = true;
                BtnModificar.Visible = false;
                BtnModificar.Enabled = false;
                LimpiarCampos();
                lblError.Text = "";
                UpdatePanel2.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al abrir la ventana agregar empleado" + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Page.Validate("empDatos");
            if (Page.IsValid)
            {
                ModificarEmpleado();
                CargarEmpleado();
                LimpiarCampos();
            }
        }

        protected void SqlEmpleado_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                string mensaje = "Ocurrió un error en el registro del empleado";
                MostrarAlerta(mensaje, NotificationType.error);
                e.ExceptionHandled = true;
            }
            else if (e.AffectedRows > 0)
            {

                string mensaje = "Se registro un empleado correctamente";
                MostrarAlerta(mensaje, NotificationType.success);
            }
        }

        protected void SqlEmpleado_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows > 0)
            {
                string mensaje = "Se modifico el empleado correctamente";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            else
            {
                string mensaje = "Ocurrió un error al modificar el empleado";
                MostrarAlerta(mensaje, NotificationType.error);
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
                string mensaje = "Ocurrió un error al cerrar la ventana: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void btnCerrarAreas_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "cerarAreas();", true);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cerrar la ventana: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
    }
}