using CapaEntidades;
using CapaNegocios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaProductos
{
    public partial class GestorProductos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            LblResOpe("");
            ModalLblResOpe("");
            if (!IsPostBack)
            {
                CargarProductos();
            }
        }

        protected void Grilla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                BtnInsert.Visible = false;
                BtnInsert.Enabled = false;
                BtnUpdate.Visible = true;
                BtnUpdate.Enabled = true;
                DDLTipo.Enabled = false;
                TbFecha.Enabled= false;
                UpdatePanel5.Update();

                int index = Convert.ToInt32(e.CommandArgument);
                try
                {
                    lblIdProd.Text = Grilla.DataKeys[index].Values["ID_PRODUCTO"].ToString();
                    DataTable dt = crearDT();
                    dt.Rows.Add(
                        Grilla.DataKeys[index].Values["ID_PROV"].ToString(),
                        Grilla.Rows[index].Cells[6].Text
                        );
                    GrillaProveedoreInUp.DataSource = dt;
                    GrillaProveedoreInUp.DataBind();
                    UpdatePanel12.Update();
                    lblIDProvInUp.Text = GrillaProveedoreInUp.DataKeys[0].Values["ID_PROV"].ToString();
                    DDLTipo.SelectedValue = Grilla.DataKeys[index].Values["ID_TIPO"].ToString();
                    TbCod.Text = Grilla.Rows[index].Cells[2].Text;
                    TbLote.Text = Grilla.Rows[index].Cells[3].Text;

                    TbNom.Text = Grilla.Rows[index].Cells[4].Text;

                    string fechaSeleccionada = Grilla.Rows[index].Cells[8].Text;

                    // Convertir la fecha a DateTime
                    DateTime fecha;
                    if (DateTime.TryParse(fechaSeleccionada, out fecha))
                    {
                        TbFecha.Text = String.Format("{0:yyyy-MM-dd}", fecha);


                    }
                    UpdatePanel1.Update();
                    TbPreciCom.Text = Grilla.DataKeys[index].Values["PRECIO_COMPRA"].ToString();
                    tbPreVentPub.Text = Grilla.DataKeys[index].Values["PrecioVentaPub"].ToString();
                    tbPreVentPriv.Text = Grilla.DataKeys[index].Values["PrecioVentaPriv"].ToString();
                    TbCantidad.Text = Grilla.Rows[index].Cells[13].Text;
                    tbCantidadMin.Text = Grilla.Rows[index].Cells[14].Text;

                    UpdatePanel3.Update();
                    UpdatePanel4.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al abrir la ventana modificar: " + ex.Message;
                    MostrarAlerta(mensaje,NotificationType.error);
                }
            }
            else if (e.CommandName == "Borrar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow fila = Grilla.Rows[index];
                ProductoNegocio prdn = new ProductoNegocio();
                if (!fila.Cells[2].Text.Equals("-10"))
                {
                    int id = int.Parse(fila.Cells[2].Text);
                    prdn.eliminarproducto(id);
                    Grilla.DataSource = null;
                    Grilla.DataBind();
                    CargarProductos();
                }
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
                    __doPostBack('UpdatePanel2', 'Borrar'); // Enviar al servidor
                }} else if (result.isDenied) {{
                    __doPostBack('UpdatePanel2', 'Deny'); // Enviar al servidor
                }}
                }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "borrarAlerta", script, true);
        }

        //metodo para confirmar el borrado
        private void ConfirmacionDeOpciones()
        {
            string target = Request.Form["__EVENTTARGET"];
            string argument = Request.Form["__EVENTARGUMENT"];

            if (target == "UpdatePanel2") // Asegúrate de que coincida con el UpdatePanel
            {
                if (argument == "Borrar")
                {
                    BorrarProducto();
                    Session["idProd"] = "0";
                }
                else { Session["idProd"] = "0"; }
            }
        }


        private void CargarProductos()
        {

            try
            {
                Grilla.DataSourceID = null;
                Grilla.DataSource = SqlProductos;
                Grilla.DataBind();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los productos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private DataTable crearDT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID_PROV");
            dt.Columns.Add("NOMBRE_PROV");
            return dt;
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
        private bool ComprobarCampos()
        {
            LblResultado.Text = "";
            bool resultado = true;
            if (lblIDProvInUp.Text.Equals(""))
            {
                LblResultado.Text += "Selecciona un proveedor <br />";
                resultado = false;
            }
            if (TbCod.Text.Equals(""))
            {
                LblResultado.Text += "LLena el campo Codigo <br />";
                resultado = false;
            }
            if (TbNom.Text.Equals(""))
            {
                LblResultado.Text += "LLena el campo Nombre <br />";
                resultado = false;
            }
            if (TbPreciCom.Text.Equals(""))
            {
                LblResultado.Text += "LLena el campo Precio de compra <br />";
                resultado = false;
            }
            if (TbCantidad.Text.Equals(""))
            {
                LblResultado.Text += "LLena el campo Cantidad <br />";
                resultado = false;
            }
            if (tbCantidadMin.Text.Equals(""))
            {
                LblResultado.Text += "LLena el campo Cantidad minima <br />";
                resultado = false;
            }
            if (tbPreVentPub.Text.Equals(""))
            {
                LblResultado.Text += "LLena el campo Publico <br />";
                resultado = false;
            }
            if (tbPreVentPriv.Text.Equals(""))
            {
                LblResultado.Text += "LLena el campo Privado <br />";
                resultado = false;
            }
            return resultado;
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            if (ComprobarCampos())
            {
                InsertarProducto();
                CargarProductos();
            }
            //else { LblResultado.Text = lblIDProvInUp.Text; }

        }

        private void LimpiarCampos()
        {
            try
            {
                GrillaProveedoreInUp.DataSource = SqlInsertarProductoProveedor;
                GrillaProveedoreInUp.DataBind();
                UpdatePanel12.Update();
                DDLTipo.SelectedIndex = 0;
                TbCod.Text = "";
                TbNom.Text = "";
                TbPreciCom.Text = "";
                TbLote.Text = "";
                TbCantidad.Text = "";
                tbCantidadMin.Text = "";
                tbPreVentPub.Text = "";
                tbPreVentPriv.Text = "";
                TbFecha.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Today);
                LblfechaV.Text = "";
                LblResultado.Text = "";
                UpdatePanel1.Update();
                UpdatePanel3.Update();
                UpdatePanel4.Update();
            }
            catch (Exception e) 
            { 
                string mensaje = "Ocurrió un error al limpiar los campos: " + e.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
        private void InsertarProducto()
        {


            /* 
             Producto producto = new Producto();

             Proveedor prov = new Proveedor();
             prov.setId(int.Parse(DDLProv.SelectedValue));

             TipoProducto tipo = new TipoProducto();
             tipo.setIdTipo(int.Parse(DDLTipo.SelectedValue));

             Stock stock = new Stock();
             stock.setCantidad(int.Parse(TbCantidad.Text));

             producto.setProveedor(prov);
             producto.setTipo(tipo);
             producto.setCodProd(TbCod.Text);
             producto.setDescProd(TbNom.Text);
             producto.setFechaIngreso(DateTime.Now.Date);
             producto.setFechaVTO(vto);
             producto.setPrecioCompra(decimal.Parse(TbPreciCom.Text));
             producto.setLote(TbLote.Text);
             producto.setStock(stock);
             producto.setPrecioVentaPub(Decimal.Parse(tbPreVentPub.Text));
             producto.setPrecioVentaPriv(Decimal.Parse(tbPreVentPriv.Text));

             ProductoNegocio prn = new ProductoNegocio();
             prn.insertar(producto);*/


            try
            {
                SqlProductos.InsertParameters["idProv"].DefaultValue = lblIDProvInUp.Text;
                SqlProductos.InsertParameters["idTipo"].DefaultValue = DDLTipo.SelectedValue;



                if (DDLTipo.SelectedValue.Equals("2"))
                {
                    SqlProductos.InsertParameters["fechaVto"].DefaultValue = null;
                    SqlProductos.InsertParameters["idEstado"].DefaultValue = "1";
                }
                else
                {
                    string fechaFormateada = "";
                    DateTime fechaSeleccionada;
                    DateTime.TryParse(TbFecha.Text, out fechaSeleccionada);
                    fechaFormateada = fechaSeleccionada.ToString("dd/MM/yyyy");
                    SqlProductos.InsertParameters["fechaVto"].DefaultValue = DateTime.Parse(fechaFormateada).ToString();
                    DateTime mesQueViene = fechaSeleccionada.AddMonths(1);
                    if (fechaSeleccionada.Year == mesQueViene.Year && fechaSeleccionada.Month == fechaSeleccionada.Month)
                    {
                        SqlProductos.InsertParameters["idEstado"].DefaultValue = "3";
                    }
                    else { SqlProductos.InsertParameters["idEstado"].DefaultValue = "2"; }


                }

                SqlProductos.InsertParameters["codigo"].DefaultValue = TbCod.Text;
                SqlProductos.InsertParameters["descripcion"].DefaultValue = TbNom.Text;
                SqlProductos.InsertParameters["fechaIngreso"].DefaultValue = DateTime.Today.Date.ToString();

                SqlProductos.InsertParameters["precioCompra"].DefaultValue = TbPreciCom.Text;
                SqlProductos.InsertParameters["lote"].DefaultValue = TbLote.Text;
                SqlProductos.InsertParameters["cantidad"].DefaultValue = TbCantidad.Text;
                SqlProductos.InsertParameters["cantidadMin"].DefaultValue = tbCantidadMin.Text;
                SqlProductos.InsertParameters["precioVentaPub"].DefaultValue = tbPreVentPub.Text;
                SqlProductos.InsertParameters["precioVentaPriv"].DefaultValue = tbPreVentPriv.Text;

                DateTime hoy = DateTime.Today;
                DateTime fechaSelecc;
                DateTime.TryParse(TbFecha.Text, out fechaSelecc);
                if (fechaSelecc >= hoy || DDLTipo.SelectedValue.Equals("2"))
                {

                    SqlProductos.Insert();
                    LblfechaV.Text = "";
                }
                else
                {
                    string mensaje = "La fecha no puede ser menor a hoy";
                    ModalLblResOpe(mensaje);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar el producto: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
            UpdatePanel1.Update();
            UpdatePanel5.Update();
        }

        protected void VencimientoCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
        }
        protected void SqlProductos_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows > 0 && e.Exception == null)
            {
                LimpiarCampos();
                string mensaje = "El producto se registró correctamente";
                MostrarAlerta(mensaje, NotificationType.success);
            }
            else
            {
                MostrarAlerta("Ocurrió un error al registrar el producto", NotificationType.error);
            }
        }

        private void BuscarProducto()
        {

            if (RbBuscDesc.Checked)
            {
                try
                {
                    SqlBuscarproductoNom.SelectParameters["nom"].DefaultValue = txtBuscar.Text;
                    Grilla.DataSource = SqlBuscarproductoNom;
                    Grilla.DataBind();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al buscar el producto por su nombre: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }

            if (RbBuscCod.Checked)
            {
                try
                {
                    SqlBuscarproductoCod.SelectParameters["cod"].DefaultValue = txtBuscar.Text;
                    Grilla.DataSource = SqlBuscarproductoCod;
                    Grilla.DataBind();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al buscar el producto por su codigo: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Equals(""))
            {
                CargarProductos();
            }
            else
            {
                BuscarProducto();
            }
        }


        /*protected void BtnBorrar_Click(object sender, EventArgs e)
        {
            GridViewRow fila = Grilla.SelectedRow;
            ProductoNegocio prdn = new ProductoNegocio();
            if (!fila.Cells[0].Text.Equals("-10"))
            {
                int id = int.Parse(fila.Cells[0].Text);
                prdn.eliminarproducto(id);
                Grilla.DataSource = null;
                Grilla.DataBind();
                CargarProductos();
            }
        }*/

        protected void BtnVentanaModal_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos();
                BtnInsert.Visible = true;
                BtnInsert.Enabled = true;
                BtnUpdate.Visible = false;
                BtnUpdate.Enabled = false;
                DDLTipo.Enabled = true;
                TbFecha.Enabled = true;
                UpdatePanel2.Update();
                BtnInsert.Visible = true;
                BtnInsert.Enabled = true;
                BtnUpdate.Visible = false;
                BtnUpdate.Enabled = false;
                UpdatePanel5.Update();
                LimpiarCampos();
                GrillaProveedoreInUp.DataSourceID = null;
                GrillaProveedoreInUp.DataSource = SqlInsertarProductoProveedor;
                GrillaProveedoreInUp.DataBind();
                UpdatePanel12.Update();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al abrir la ventana agregar/modificar producto: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void DDLTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLTipo.SelectedValue.Equals("2"))
            {
                TbFecha.Enabled=false;
                UpdatePanel1.Update();

            }
            else
            {
                TbFecha.Enabled = true;
                UpdatePanel1.Update();
            }
        }

        protected void Grilla_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grilla.PageIndex = e.NewPageIndex;
            if (txtBuscar.Text.Equals(""))
            {
                Grilla.DataSource = SqlProductos;
                Grilla.DataBind();
            }
            else
            {
                BuscarProducto();
            }
        }

        private void BorrarProducto() 
        {
            try
            {
                string idProducto = Session["idProd"].ToString();
                SqlProductos.DeleteParameters["id"].DefaultValue = idProducto;
                SqlProductos.Delete();
                Grilla.DataSourceID = null;
                Grilla.DataSource = SqlProductos;
                Grilla.DataBind();
                UpdatePanel2.Update();
                MostrarAlerta("Se borro el producto con éxito",NotificationType.success);//esto no se esta mostrando!!!!
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al borrar el producto: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void Grilla_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Session["idProd"]= Grilla.DataKeys[e.RowIndex].Values["ID_PRODUCTO"].ToString();
            AlertaBorrar("¿Seguro que quiere borrar el producto?", NotificationType.warning);
        }

        protected void BtnVentanaModalPrecio_Click(object sender, EventArgs e)
        {
            limpiarCamposPorcentaje();
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModalPrecio", "mostrarModalPrecio();", true);
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al abrir la ventana precios: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        private void limpiarCamposPorcentaje()
        {
            try
            {
                tbBuscarProv.Text = "";
                lblPrecioResultado.Text = "";
                lblProvID.Text = "";
                rbAumento.Checked = true;
                TbPorcentaje.Text = "";
                GrillaProveedores.DataSourceID = null;
                GrillaProveedores.DataSource = null;
                GrillaProveedores.DataBind();
                UpdatePanel9.Update();
                UpdatePanel7.Update();
                UpdatePanel10.Update();
                UpdatePanel11.Update();
            }catch(Exception ex) 
            {
                string mensaje = "Ocurrió un error al limpiar los campos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void BuscarProveedor()
        {
            if (!tbBuscarProv.Text.Equals(""))
            {
                try
                {
                    SqlBuscarProv.SelectParameters["nombre"].DefaultValue = tbBuscarProv.Text;
                    GrillaProveedores.DataSourceID = null;
                    GrillaProveedores.DataSource = SqlBuscarProv;
                    GrillaProveedores.DataBind();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al buscar al proveedor: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                GrillaProveedores.DataSourceID = null;
                GrillaProveedores.DataSource = null;
                GrillaProveedores.DataBind();
            }
            UpdatePanel7.Update();
        }
        protected void btnBuscarProv_Click(object sender, EventArgs e)
        {
            BuscarProveedor();
        }

        protected void GrillaProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProvID.Text = GrillaProveedores.DataKeys[GrillaProveedores.SelectedIndex].Values["ID_PROV"].ToString();
        }

        private void modificarPorcentaje()
        {
            if (!lblProvID.Text.Equals("") && !TbPorcentaje.Text.Equals(""))
            {
                try
                {
                    SqlBuscarProv.UpdateParameters["idProv"].DefaultValue = lblProvID.Text;
                    SqlBuscarProv.UpdateParameters["precio"].DefaultValue = TbPorcentaje.Text;
                    if (rbAumento.Checked) { SqlBuscarProv.UpdateParameters["sumOResta"].DefaultValue = "1"; }
                    else { SqlBuscarProv.UpdateParameters["sumOResta"].DefaultValue = "2"; }
                    int resultado = SqlBuscarProv.Update();
                    CargarProductos();
                    if (resultado == -1)
                    {
                        limpiarCamposPorcentaje();
                        string mensaje = "Se modificaron los precios segun el procentaje";
                        MostrarAlerta(mensaje, NotificationType.success);
                    }
                    else 
                    { 
                        string mensaje = "Error al modificar el precio";
                        MostrarAlerta(mensaje, NotificationType.error);
                    }
                    UpdatePanel2.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al modificar el precio: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
            else
            {
                string mensaje = "Seleccione un proveedor e ingrese el porcentaje";
                MostrarAlerta(mensaje, NotificationType.warning);
            }

        }

        protected void btnPorcentaje_Click(object sender, EventArgs e)
        {
            modificarPorcentaje();
        }

        protected void GrillaProveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaProveedores.PageIndex = e.NewPageIndex;
            BuscarProveedor();
        }

        protected void GrillaProveedoreInUp_SelectedIndexChanged(object sender, EventArgs e)
        {

            lblIDProvInUp.Text = GrillaProveedoreInUp.DataKeys[GrillaProveedoreInUp.SelectedIndex].Values["ID_PROV"].ToString();
            UpdatePanel12.Update();
        }

        protected void GrillaProveedoreInUp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaProveedoreInUp.PageIndex = e.NewPageIndex;
                GrillaProveedoreInUp.DataSource = SqlInsertarProductoProveedor;
                GrillaProveedoreInUp.DataBind();
                UpdatePanel12.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cambiar la pagina de la tabla proveedor: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void ModificarProducto()
        {
            try
            {
                SqlProductos.UpdateParameters["id"].DefaultValue = lblIdProd.Text;
                SqlProductos.UpdateParameters["idProv"].DefaultValue = lblIDProvInUp.Text;
                SqlProductos.UpdateParameters["codigo"].DefaultValue = TbCod.Text;
                SqlProductos.UpdateParameters["descripcion"].DefaultValue = TbNom.Text;
                SqlProductos.UpdateParameters["precioCompra"].DefaultValue = TbPreciCom.Text;
                SqlProductos.UpdateParameters["lote"].DefaultValue = TbLote.Text;
                SqlProductos.UpdateParameters["cantidad"].DefaultValue = TbCantidad.Text;
                SqlProductos.UpdateParameters["cantidadMin"].DefaultValue = tbCantidadMin.Text;
                SqlProductos.UpdateParameters["preVentPub"].DefaultValue = tbPreVentPub.Text;
                SqlProductos.UpdateParameters["preVentPriv"].DefaultValue = tbPreVentPriv.Text;
                int resultado = SqlProductos.Update();
                if (resultado == -1)
                {
                    CargarProductos();
                    UpdatePanel2.Update();
                    string mensaje = "Se modifico el producto";
                    MostrarAlerta(mensaje, NotificationType.success);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al modificar el producto: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (ComprobarCampos())
            {
                ModificarProducto();
            }
        }
        protected void BtnCerrarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "cerar();", true);
        }
        protected void GrillaProveedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {

                int index = Convert.ToInt32(e.CommandArgument);
                lblProvID.Text = GrillaProveedores.DataKeys[index].Values["ID_PROV"].ToString();
            }
        }

        protected void BtnCerrarModalAgregar_Click(object sender, EventArgs e)
        {

        }
    }
}