using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaUsuarios
{
    public partial class GestorUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            if (!IsPostBack)
            {
                CargarUsuario();
            }

        }

        private void CargarUsuario()
        {
            try
            {
                GrillaUsuarios.DataSource = SqlUsuarios;
                GrillaUsuarios.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string msg = "Ocurrió un error al cargar los usuarios: " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
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
                    BorrarUsuario();
                    Session["IdUsuBorrar"] = "0";
                }
                else { Session["IdUsuBorrar"] = "0"; }
            }
        }

        protected void GrillaUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                try
                {
                    lblTituloModal.Text = "Modificar usuario";
                    udpTituloModal.Update();
                    LimpiarCampos();
                    BtnAgregar.Visible = false;
                    BtnAgregar.Enabled = false;
                    BtnModificar.Visible = true;
                    BtnModificar.Enabled = true;
                    //lblError.Text = "";
                    int index = Convert.ToInt32(e.CommandArgument);
                    lblIdUsuario.Text = GrillaUsuarios.DataKeys[index].Values["ID_USUARIO"].ToString();

                    TbUsuario.Text = GrillaUsuarios.Rows[index].Cells[1].Text;
                    TbCLave.Text = GrillaUsuarios.Rows[index].Cells[2].Text;


                    UpdatePanel2.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
                }
                catch (Exception ex)
                {
                    string msg = "Ocurrió un error al abrir la ventana modificar empleado: " + ex.Message;
                    MostrarAlerta(msg, NotificationType.error);
                }
            }
        }

        private void LimpiarCampos()
        {
            TbUsuario.Text = "";
            TbCLave.Text = "";
            lblIdUsuario.Text = "";
        }

        protected void GrillaUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaUsuarios.PageIndex = e.NewPageIndex;
            CargarUsuario();
        }

        private void ModificarUsuario()
        {
            try 
            {
                SqlUsuarios.UpdateParameters["idUsuario"].DefaultValue = lblIdUsuario.Text;
                SqlUsuarios.UpdateParameters["usuario"].DefaultValue = TbUsuario.Text;
                SqlUsuarios.UpdateParameters["clave"].DefaultValue = TbCLave.Text;
                SqlUsuarios.UpdateParameters["idTipo"].DefaultValue = ddlTipoUsuario.SelectedValue;
                SqlUsuarios.Update();
            }
            catch(Exception ex) 
            {
                string msg = "Ocurrió un error al modificar el usuario " + ex.Message;
                MostrarAlerta(msg, NotificationType.error);
            }
        }

        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Page.Validate("empDatos");
            if (Page.IsValid)
            {
                ModificarUsuario();
                CargarUsuario();
                LimpiarCampos();
            }
            else 
            {
                string mensaje = "Complete los campos vacios";
                MostrarAlerta(mensaje,NotificationType.warning);
            }
        }

        protected void BtnAbrirModal_Click(object sender, EventArgs e)
        {
            try
            {
                lblTituloModal.Text = "Agregar Usuario";
                udpTituloModal.Update();
                BtnAgregar.Visible = true;
                BtnAgregar.Enabled = true;
                BtnModificar.Visible = false;
                BtnModificar.Enabled = false;
                LimpiarCampos();
                //lblError.Text = "";
                UpdatePanel2.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al abrir la ventana agregar Usuario" + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void InsertarUsuario()
        {
            try
            {
                SqlUsuarios.InsertParameters["usuario"].DefaultValue = TbUsuario.Text;
                SqlUsuarios.InsertParameters["clave"].DefaultValue = TbCLave.Text;
                SqlUsuarios.InsertParameters["idTipo"].DefaultValue = ddlTipoUsuario.SelectedValue;

                SqlUsuarios.Insert();
                GrillaUsuarios.DataSource = SqlUsuarios;
                GrillaUsuarios.DataBind();
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar el usuario" + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            Page.Validate("empDatos");
            if (Page.IsValid)
            {
                InsertarUsuario();
                CargarUsuario();
                LimpiarCampos();
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


        private void BorrarUsuario()
        {
            try
            {

                string idUsu = Session["IdUsuBorrar"].ToString();
                SqlUsuarios.DeleteParameters["idUsuario"].DefaultValue = idUsu;
                SqlUsuarios.Delete();
                CargarUsuario();
                string mensaje = "Se elimino al usuario con exito";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            catch (Exception ex)
            {

                string mensaje = "Ocurrió un error al eliminar el usuario: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }

        protected void GrillaUsuarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Session["IdUsuBorrar"] = GrillaUsuarios.DataKeys[e.RowIndex].Values["ID_USUARIO"].ToString();
            AlertaBorrar("¿Seguro que quiere borrar al Usuario?", NotificationType.warning);
        }

        protected void SqlUsuarios_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                string mensaje = "Ocurrió un error en el registro del usuario";
                MostrarAlerta(mensaje, NotificationType.error);
                e.ExceptionHandled = true;
            }
            else if (e.AffectedRows > 0)
            {
                string mensaje = "Se registro el usuario correctamente";
                MostrarAlerta(mensaje, NotificationType.success);
            }
        }

        protected void SqlUsuarios_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows > 0)
            {
                string mensaje = "Se modifico el usuario correctamente";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            else
            {
                string mensaje = "Ocurrió un error al modificar el usuario";
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
    }
}