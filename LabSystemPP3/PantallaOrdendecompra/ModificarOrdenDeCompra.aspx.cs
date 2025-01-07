using CapaEntidades;
using CapaNegocios;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaOrdendecompra
{
    public partial class ModificarOrdenDeCompra : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            ConfirmacionDeOpciones();
            if (!IsPostBack)
            {
                Session["OCID"] = Request.QueryString["Id"];
                lblidOC.Text = Request.QueryString["Id"];
                CargarProductos();
                RegisterAsyncTask(new PageAsyncTask(CargarProvincias));
                UtilizarCookie();
                Session["DeliveryOC"] = 0;
                Session["ivaOc"] = 0;
                CrearDtProd();
                lblRs.Text = Request.QueryString["rs"];
                CrearDtBorrar();
                CargarDatos();

            }
        }


        private async void CargarDatos()
        {
            try
            {
                string idOc = lblidOC.Text;
                this.SqlOrdenDeCompra.SelectParameters["IdOc"].DefaultValue = idOc;
                this.SqlOrdenDeCompra.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader datos;
                datos = (SqlDataReader)this.SqlOrdenDeCompra.Select(DataSourceSelectArguments.Empty);
                if (datos.Read())
                {

                    lblRs.Text = datos["NOMBRE_PROV"].ToString();
                    lblTotal.Text = datos["Total"].ToString();
                    if (datos["Delivery"] != null)
                    {
                        LblViatico.Text = datos["Delivery"].ToString();
                        TbViatico.Text = datos["Delivery"].ToString();
                    }
                    if (datos["PrecioIva"] != null)
                    {
                        lblIva.Text = datos["PrecioIva"].ToString();
                        TbIva.Text = datos["Iva"].ToString();
                    }
                    if (datos["Autorizacion"] != null)
                    {
                        ddlAutorizacion.SelectedValue= datos["Autorizacion"].ToString();
                    }
                    if (datos["Autorizacion"] != null)
                    {
                        tbInstrucciones.Text = datos["instrucciones"].ToString();
                    }

                }
                else
                {
                    MostrarAlerta("No se enconto la orden de compra", NotificationType.error);
                }

                this.SqlDirecciones.SelectParameters["IdOc"].DefaultValue = idOc;
                this.SqlDirecciones.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader datosDir;
                datosDir = (SqlDataReader)this.SqlDirecciones.Select(DataSourceSelectArguments.Empty);
                while (datosDir.Read())
                {

                    if (datosDir["tipo"].ToString().Equals("1"))
                    {
                        TbTelefonoTecnoDiag.Text = datosDir["telefono"].ToString();
                        TbDireccionTecnoDiag.Text = datosDir["direccion"].ToString();
                        DropDownListProvincasTecnoDiag.SelectedValue = datosDir["provincia"].ToString();
                        udpProvinciaTecnoDiag.Update();
                        tbCodPostalTecnoDiag.Text = datosDir["codigoPostal"].ToString();
                    }

                    if (datosDir["tipo"].ToString().Equals("2"))
                    {
                        TbTelefonoProv.Text = datosDir["telefono"].ToString();
                        TbDireccionProv.Text = datosDir["direccion"].ToString();
                        DropDownListProvinciasProveedor.SelectedValue = datosDir["provincia"].ToString();
                        udpProvinciaProv.Update();
                        //await SeleccionarProvincias(datosDir["provincia"].ToString());
                        tbCodPostalProv.Text = datosDir["codigoPostal"].ToString();
                    }

                    if (datosDir["tipo"].ToString().Equals("3"))
                    {
                        TbDireccionEntrega.Text = datosDir["direccion"].ToString();
                        DropDownListEntrgaProvincia.SelectedValue = datosDir["provincia"].ToString();
                        UdpEntrega.Update();
                        //await SeleccionarProvincias(datosDir["provincia"].ToString(), 3);
                        tbCodPostalEntrega.Text = datosDir["codigoPostal"].ToString();
                    }
                }
                lblTotProd.Text = this.GrillaProductoPedido.DataKeys[0].Values["TotalGeneral"].ToString();
            }
            catch (Exception ex)
            {
                MostrarAlerta("a: " + ex.Message, NotificationType.error);
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

        private void CargarProductos()
        {
            try
            {
                string id = Request.QueryString["IdProv"];
                SqlProductos.SelectParameters["idProv"].DefaultValue = id;
                GrillaProductos.DataSource = SqlProductos;
                GrillaProductos.DataBind();
                UpdatePanelProductos.Update();

                DataView dv = (DataView)SqlProductos.Select(DataSourceSelectArguments.Empty);

                Session["ListaProductos"] = dv.ToTable();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los productos del proveedor: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CargarProductosNom()
        {
            try
            {
                string id = Request.QueryString["IdProv"];
                string nom = txtBuscar.Text;
                SqlBuscarproductoNom.SelectParameters["idProv"].DefaultValue = id;
                SqlBuscarproductoNom.SelectParameters["nom"].DefaultValue = nom;
                GrillaProductos.DataSource = SqlBuscarproductoNom;
                GrillaProductos.DataBind();
                UpdatePanelProductos.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los productos por su nombre: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CargarProductosCod()
        {
            try
            {
                string id = Request.QueryString["IdProv"];
                string nom = txtBuscar.Text;
                SqlBuscarproductoCod.SelectParameters["idProv"].DefaultValue = id;
                SqlBuscarproductoCod.SelectParameters["cod"].DefaultValue = nom;
                GrillaProductos.DataSource = SqlBuscarproductoCod;
                GrillaProductos.DataBind();
                UpdatePanelProductos.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar los productos por su codigo: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CrearDtProd()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID_DETALLE");
            dt.Columns.Add("IdProd");
            dt.Columns.Add("COD_PROD");
            dt.Columns.Add("DESCRIPCION");
            dt.Columns.Add("PrecioUnitario");
            dt.Columns.Add("CANTIDAD");
            dt.Columns.Add("Total");
            dt.Columns.Add("TotalGeneral");


            this.SqlDetalleOrdCom.SelectParameters["IdOc"].DefaultValue = Request.QueryString["Id"];
            this.SqlDetalleOrdCom.DataSourceMode = SqlDataSourceMode.DataReader;
            SqlDataReader reader;
            reader = (SqlDataReader)this.SqlDetalleOrdCom.Select(DataSourceSelectArguments.Empty);

            while (reader.Read())
            {
                DataRow row = dt.NewRow();
                row["ID_DETALLE"] = reader["ID_DETALLE"];
                row["IdProd"] = reader["IdProd"];
                row["COD_PROD"] = reader["COD_PROD"];
                row["DESCRIPCION"] = reader["DESCRIPCION"];
                row["PrecioUnitario"] = reader["PrecioUnitario"];
                row["CANTIDAD"] = reader["CANTIDAD"];
                row["Total"] = reader["Total"];
                row["TotalGeneral"] = reader["TotalGeneral"];

                dt.Rows.Add(row);
            }

            GrillaProductoPedido.DataSource = dt;
            GrillaProductoPedido.DataBind();
            Session["ProdSelec"] = dt;
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Equals(""))
            {
                CargarProductos();
            }
            else if (RbBuscCod.Checked)
            {
                CargarProductosCod();
            }
            else { CargarProductosNom(); }
        }

        protected void GrillaProducto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrillaProductos.PageIndex = e.NewPageIndex;
            if (txtBuscar.Text.Equals(""))
            {
                CargarProductos();
            }
            else if (RbBuscCod.Checked)
            {
                CargarProductosCod();
            }
            else { CargarProductosNom(); }
        }


        private void agregarProducto()
        {
            try
            {
                int indiceRelativo = int.Parse(IndiceGrillaProducto.Text); // Índice relativo en la página actual
                int indiceGlobal = (GrillaProductos.PageIndex * GrillaProductos.PageSize) + indiceRelativo; // Índice global en el DataTable

                DataTable dtListadoProd = (DataTable)Session["ListaProductos"];
                int totalAPedir = int.Parse(TbCantidad.Text);

                if (!TbCantidad.Text.Equals("0") && totalAPedir > 0)
                {
                    bool nuevoProducto = true;
                    Decimal total = 0;
                    string id = GrillaProductos.DataKeys[indiceRelativo].Values["ID_PRODUCTO"].ToString();
                    string precio = GrillaProductos.Rows[indiceRelativo].Cells[3].Text;
                    string codigo = GrillaProductos.Rows[indiceRelativo].Cells[1].Text;

                    for (int i = 0; i < ((DataTable)Session["ProdSelec"]).Rows.Count; i++)
                    {
                        DataTable dt = (DataTable)Session["ProdSelec"];
                        if (dt.Rows[i]["IdProd"].ToString().Equals(id))
                        {
                            int cantidadVieja = int.Parse(dt.Rows[i]["Cantidad"].ToString());
                            int cantidadNueva = totalAPedir;
                            int resultado = cantidadVieja + cantidadNueva;


                            dt.Rows[i]["Cantidad"] = resultado.ToString();

                            Decimal precioProducto = Decimal.Parse(precio);
                            Decimal totalNuevo = precioProducto * resultado;
                            dt.Rows[i]["Total"] = totalNuevo.ToString();

                            Session["ProdSelec"] = dt;

                            GrillaProductoPedido.DataSource = dt;
                            GrillaProductoPedido.Columns[0].Visible = true;
                            GrillaProductoPedido.Columns[1].Visible = true;
                            int paginaActual = GrillaProductoPedido.PageIndex;
                            GrillaProductoPedido.PageIndex = paginaActual;
                            GrillaProductoPedido.DataBind();
                            GrillaProductoPedido.Columns[0].Visible = false;
                            GrillaProductoPedido.Columns[1].Visible = false;

                            nuevoProducto = false;
                            break;
                        }
                    }

                    if (nuevoProducto)
                    {
                        string nombre = GrillaProductos.Rows[indiceRelativo].Cells[2].Text;
                        string cantidad = TbCantidad.Text;

                        Decimal DPrecio = Decimal.Parse(precio);
                        int ICantidad = int.Parse(cantidad);

                        Decimal totalProd = DPrecio * ICantidad;

                        DataTable dt = (DataTable)Session["ProdSelec"];

                        DataRow dr = dt.NewRow();

                        dr["ID_DETALLE"] = 0;
                        dr["IdProd"] = id;
                        dr["COD_PROD"] = codigo;
                        dr["DESCRIPCION"] = nombre;
                        dr["PrecioUnitario"] = DPrecio;
                        dr["CANTIDAD"] = cantidad;
                        dr["Total"] = totalProd;
                        dr["TotalGeneral"] = lblTotProd.Text;

                        dt.Rows.Add(dr);
                        Session["ProdSelec"] = dt;

                        GrillaProductoPedido.DataSource = dt;
                        GrillaProductoPedido.Columns[0].Visible = true;
                        GrillaProductoPedido.Columns[1].Visible = true;
                        int paginaActual = GrillaProductoPedido.PageIndex;
                        GrillaProductoPedido.PageIndex = paginaActual;
                        GrillaProductoPedido.DataBind();
                        GrillaProductoPedido.Columns[0].Visible = false;
                        GrillaProductoPedido.Columns[1].Visible = false;
                    }



                    DataTable dtp = (DataTable)Session["ProdSelec"];

                    foreach (DataRow fila in dtp.Rows)
                    {
                        total += Decimal.Parse(fila["Total"].ToString());
                    }
                    lblTotProd.Text = total.ToString();

                    TbCantidad.Text = "0";
                    UdpProdPedido.Update();
                    TbCantidad.Enabled = false;
                    BtnSelectCant.Enabled = false;
                    btnCalcularTotal.Enabled = true;
                    UpdatePanel1.Update();
                    udpLblTotalProd.Update();
                    CBviatico.Enabled = true;
                    udpCBViatico.Update();
                    lblProdNom.Text = "";
                    UpdatePanel2.Update();
                    UpdatePanel4.Update();
                    UpdatePanelProductos.Update();
                    cbIva.Enabled = true;
                    UdpIva.Update();

                }
                else if (totalAPedir <= 0)
                {
                    string mensaje = "La cantidad de productos seleccionados, debe ser mayor a 0 ";
                    TbCantidad.Text = "0";
                    MostrarAlerta(mensaje, NotificationType.warning);
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al seleccionar el producto: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected void BtnSelectCant_Click(object sender, EventArgs e)
        {
            if (!lblIdProd.Text.Equals(""))
            {
                agregarProducto();
            }
            else
            {
                string mensaje = "Seleccione un producto";
                MostrarAlerta(mensaje, NotificationType.warning);
            }
        }


        protected void btnSumar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TbViatico.Text.Equals("0"))
                {

                    Decimal Total = Decimal.Parse(TbViatico.Text);
                    LblViatico.Text = Total.ToString();
                    Session["DeliveryOC"] = TbViatico.Text;

                }
                else
                {
                    LblViatico.Text = "0";
                }
                udpPrecioViatico.Update();
                //udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al sumar el precio: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }



        protected void btnRestar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TbViatico.Text.Equals("") || !TbViatico.Text.Equals("0"))
                {
                    Decimal total = 0;
                    if (!LblViatico.Text.Equals("0"))
                    {
                        total = Decimal.Parse(LblViatico.Text);
                    }
                    Decimal precio = Decimal.Parse(TbViatico.Text);
                    if (precio <= total)
                    {
                        Decimal Total = Decimal.Parse(LblViatico.Text) - Decimal.Parse(TbViatico.Text);
                        LblViatico.Text = Total.ToString();
                        TbViatico.Text = "0";
                    }
                    else
                    {
                        string msg = "El precio a disminuir no puede ser mayor que el total";
                        MostrarAlerta(msg, NotificationType.warning);
                    }
                }
                else
                {
                    string msg = "El precio del servicio esta vacio";
                    MostrarAlerta(msg, NotificationType.warning);
                }
                udpPrecioViatico.Update();
                udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al restar el precio: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CalcualarTotal()
        {
            try
            {
                Session["DeliveryOC"] = 0;
                Session["ivaOc"] = 0;
                Decimal total = Decimal.Parse(lblTotal.Text);
                Decimal sesionDeliveri = Decimal.Parse(Session["DeliveryOC"].ToString());
                Decimal delivery = Decimal.Parse(LblViatico.Text);
                if (delivery != sesionDeliveri)
                {
                    total = total - sesionDeliveri;
                }

                Decimal sesioniva = Decimal.Parse(Session["ivaOc"].ToString());
                Decimal iva = Decimal.Parse(lblIva.Text);

                if (iva != sesioniva)
                {
                    total = total - iva;
                }

                Decimal productos = Decimal.Parse(lblTotProd.Text);
                Decimal resultado = productos + delivery + iva;
                lblTotal.Text = resultado.ToString();
                udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al calcular el total: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }
        protected void btnCalcularTotal_Click(object sender, EventArgs e)
        {
            CalcualarTotal();
        }

        protected void GrillaProducto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                try
                {

                    DataTable dt = (DataTable)Session["ListaProductos"];
                    IndiceGrillaProducto.Text = index.ToString();
                    lblIdProd.Text = this.GrillaProductos.DataKeys[index].Values["ID_PRODUCTO"].ToString();
                    lblProdNom.Text = GrillaProductos.Rows[index].Cells[2].Text;
                    UpdatePanel4.Update();
                    TbCantidad.Enabled = true;
                    BtnSelectCant.Enabled = true;
                    UpdatePanel1.Update();
                    CBviatico.Enabled = true;
                    udpCBViatico.Update();
                }
                catch (Exception ex)
                {
                    string mensaje = "Ocurrió un error al seleccionar el productos: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        protected void CBviatico_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (CBviatico.Checked)
                {
                    TbViatico.Enabled = true;
                    btnSumar.Enabled = true;
                    // btnRestar.Enabled = true;
                }
                else
                {
                    TbViatico.Enabled = false;
                    btnSumar.Enabled = false;
                    //btnRestar.Enabled = false;
                    Decimal Total = Decimal.Parse(lblTotal.Text) - Decimal.Parse(LblViatico.Text);
                    lblTotal.Text = Total.ToString();
                    LblViatico.Text = "0";
                    TbViatico.Text = "0";
                    udpPrecioViatico.Update();
                    CalcualarTotal();
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al seleccionar el viatico: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        private async Task CargarProvincias()
        {
            string apiUrl = "https://apis.datos.gob.ar/georef/api/provincias?campos=id,nombre";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Procesa el JSON (por ejemplo, con System.Text.Json o Newtonsoft.Json)
                    dynamic provincias = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);


                    // Llena un DropDownList con las provincias
                    DropDownListProvinciasProveedor.DataSource = provincias.provincias;
                    DropDownListProvinciasProveedor.DataTextField = "nombre";
                    DropDownListProvinciasProveedor.DataValueField = "id";
                    DropDownListProvinciasProveedor.DataBind();

                    await CargarCiudadesProveedor(DropDownListProvinciasProveedor.SelectedValue);

                    DropDownListProvincasTecnoDiag.DataSource = provincias.provincias;
                    DropDownListProvincasTecnoDiag.DataTextField = "nombre";
                    DropDownListProvincasTecnoDiag.DataValueField = "id";
                    DropDownListProvincasTecnoDiag.DataBind();

                    await CargarCiudadesTecnoDiag(DropDownListProvincasTecnoDiag.SelectedValue);

                    DropDownListEntrgaProvincia.DataSource = provincias.provincias;
                    DropDownListEntrgaProvincia.DataTextField = "nombre";
                    DropDownListEntrgaProvincia.DataValueField = "id";
                    DropDownListEntrgaProvincia.DataBind();

                    await CargarCiudadesEntrega(DropDownListEntrgaProvincia.SelectedValue);

                }
            }
            catch (Exception ex)
            {
                // Manejar errores
                string mensaje = "Error al cargar provincias: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }



        protected async void DropDownListProvincias_SelectedIndexChanged(object sender, EventArgs e)
        {
            await CargarCiudadesProveedor(DropDownListProvinciasProveedor.SelectedValue);
            udpProvinciaProv.Update();
        }

        private async Task CargarCiudadesProveedor(string id)
        {
            string apiUrl = "https://apis.datos.gob.ar/georef/api/departamentos?provincia=" + id + "&campos=id,nombre&max=100";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();


                    dynamic departamentos = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                    // Llena un DropDownList con las provincias
                    DropDownListDepartamentosProveedor.DataSource = departamentos.departamentos;
                    DropDownListDepartamentosProveedor.DataTextField = "nombre";
                    DropDownListDepartamentosProveedor.DataValueField = "id";
                    DropDownListDepartamentosProveedor.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores
                string mensaje = "Error al cargar los departamentos del proveedor: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }

        private async Task CargarCiudadesTecnoDiag(string id)
        {
            string apiUrl = "https://apis.datos.gob.ar/georef/api/departamentos?provincia=" + id + "&campos=id,nombre&max=100";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Procesa el JSON (por ejemplo, con System.Text.Json o Newtonsoft.Json)
                    dynamic departamentos = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                    // Llena un DropDownList con las provincias
                    DropDownListDepartamentosTecnoDiag.DataSource = departamentos.departamentos;
                    DropDownListDepartamentosTecnoDiag.DataTextField = "nombre";
                    DropDownListDepartamentosTecnoDiag.DataValueField = "id";
                    DropDownListDepartamentosTecnoDiag.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores
                string mensaje = "Error al cargar los departamentos de tecnodiagnostica: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }

        }

        private async Task CargarCiudadesEntrega(string id)
        {
            string apiUrl = "https://apis.datos.gob.ar/georef/api/departamentos?provincia=" + id + "&campos=id,nombre&max=100";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Procesa el JSON (por ejemplo, con System.Text.Json o Newtonsoft.Json)
                    dynamic departamentos = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                    // Llena un DropDownList con las provincias
                    DropDownListEntrgaCiudad.DataSource = departamentos.departamentos;
                    DropDownListEntrgaCiudad.DataTextField = "nombre";
                    DropDownListEntrgaCiudad.DataValueField = "id";
                    DropDownListEntrgaCiudad.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores
                string mensaje = "Error al cargar los departamentos para la entrega: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        protected async void DropDownListProvincasTecnoDiag_SelectedIndexChanged(object sender, EventArgs e)
        {
            await CargarCiudadesTecnoDiag(DropDownListProvincasTecnoDiag.SelectedValue);
            udpProvinciaTecnoDiag.Update();
        }

        protected async void DropDownListEntrgaProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            await CargarCiudadesEntrega(DropDownListEntrgaProvincia.SelectedValue);
            UdpEntrega.Update();
        }

        protected void GrillaProductoPedido_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrillaProductoPedido.PageIndex = e.NewPageIndex;
                DataTable dtp = (DataTable)Session["ProdSelec"];
                GrillaProductoPedido.DataSource = dtp;
                GrillaProductoPedido.DataBind();
                UdpProdPedido.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void CrearDtBorrar()
        {
            DataTable dtBorrar = new DataTable();
            dtBorrar.Columns.Add("idDetalle");

            Session["OcDetalleABorrar"] = dtBorrar;
        }

        protected void GrillaProductoPedido_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Obtener los DataTables desde la sesión
            DataTable dtPedidos = Session["ProdSelec"] as DataTable;
            DataTable dtListadoProd = Session["ListaProductos"] as DataTable;



            if (dtPedidos != null && dtListadoProd != null)
            {
                try
                {
                    int indice = (GrillaProductoPedido.PageSize * GrillaProductoPedido.PageIndex) + e.RowIndex;
                    // Calcular el nuevo total

                    decimal subtotalEliminar = decimal.Parse(dtPedidos.Rows[indice]["Total"].ToString());
                    decimal totalActual = decimal.Parse(lblTotProd.Text);
                    lblTotProd.Text = (totalActual - subtotalEliminar).ToString();

                    if (!dtPedidos.Rows[indice]["ID_DETALLE"].ToString().Equals("0"))
                    {
                        DataTable borrar = (DataTable)Session["OcDetalleABorrar"];
                        borrar.Rows.Add(dtPedidos.Rows[indice]["ID_DETALLE"].ToString());
                        Session["OcDetalleABorrar"] = borrar;
                    }
                    // Eliminar la fila del DataTable de Pedidos
                    Console.WriteLine(dtPedidos.Rows[indice]["DESCRIPCION"]);
                    dtPedidos.Rows[indice].Delete();

                    // Volver a enlazar GrillaProductoPedido con el DataTable actualizado
                    GrillaProductoPedido.DataSource = dtPedidos;
                    GrillaProductoPedido.DataBind();

                    // Guardar los DataTables actualizados en la sesión
                    Session["Pedidos"] = dtPedidos;
                    Session["ListaProductos"] = dtListadoProd;

                    // Actualizar los UpdatePanels
                    udpLblTotalProd.Update();
                    CalcualarTotal();
                    UpdatePanelProductos.Update();
                    UdpProdPedido.Update();
                }
                catch (Exception ex)
                {

                    string mensaje = "Ocurrió un error al borrar el producto seleccionado: " + ex.Message;
                    MostrarAlerta(mensaje, NotificationType.error);
                }
            }
        }

        private void BorrarDetallesOC()
        {
            try
            {
                DataTable borrar = (DataTable)Session["OcDetalleABorrar"];
                foreach (DataRow dr in borrar.Rows)
                {
                    SqlDetalleOrdCom.DeleteParameters["idDetalle"].DefaultValue = dr["idDetalle"].ToString();
                    SqlDetalleOrdCom.Delete();
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al borrar el producto de la base de datos: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        protected void cbIva_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIva.Checked)
            {
                btnIva.Enabled = true;
                TbIva.Enabled = true;
            }
            else
            {
                btnIva.Enabled = false;
                TbIva.Enabled = false;
                lblIva.Text = "0";
                TbIva.Text = "0";
                UdpIva.Update();
                UpdPrecioIva.Update();
            }
        }

        protected void btnIva_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TbIva.Text.Equals("0"))
                {

                    Decimal total = Decimal.Parse(lblTotProd.Text);
                    Decimal porcentaje = Decimal.Parse(TbIva.Text) / 100;

                    Decimal montoIVA = total * porcentaje;

                    lblIva.Text = montoIVA.ToString();
                    Session["ivaOc"] = montoIVA;
                    UdpIva.Update();
                    UpdPrecioIva.Update();

                   // MostrarAlerta("tbIva= " + TbIva.Text + " total= " + total + " porcentaje= " + porcentaje + " monto iva= " + montoIVA, NotificationType.info);
                }
                else
                {
                    lblIva.Text = "0";
                    UpdPrecioIva.Update();
                }
                //udpLblTotal.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al sumar el precio: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private void modificarOrdenDeCompra()
        {
            try
            {

                OrdendeCompraNegocio ocn = new OrdendeCompraNegocio();

                OrdendeCompra orden = new OrdendeCompra();
                SqlOrdenDeCompra.UpdateParameters["idOc"].DefaultValue = Request.QueryString["Id"];
                SqlOrdenDeCompra.UpdateParameters["total"].DefaultValue = lblTotal.Text;
                if (LblViatico.Text.Equals(""))
                {
                    SqlOrdenDeCompra.UpdateParameters["delivery"].DefaultValue = null;
                }
                else
                {
                    SqlOrdenDeCompra.UpdateParameters["delivery"].DefaultValue = LblViatico.Text;
                }
                if (lblIva.Text.Equals(""))
                {
                    SqlOrdenDeCompra.UpdateParameters["iva"].DefaultValue = null;
                    SqlOrdenDeCompra.UpdateParameters["precioIva"].DefaultValue = null;
                }
                else
                {
                    SqlOrdenDeCompra.UpdateParameters["iva"].DefaultValue = TbIva.Text;
                    SqlOrdenDeCompra.UpdateParameters["precioIva"].DefaultValue = lblIva.Text;
                }
                SqlOrdenDeCompra.UpdateParameters["autorizacion"].DefaultValue = ddlAutorizacion.SelectedValue;
                SqlOrdenDeCompra.UpdateParameters["instrucciones"].DefaultValue = tbInstrucciones.Text;
                SqlOrdenDeCompra.Update();

            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar la orden de compra: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);

            }

        }

        private void UpdateDirecciones()
        {
            try
            {

                SqlDirecciones.UpdateParameters["idOrdeCompra"].DefaultValue = Request.QueryString["Id"];
                SqlDirecciones.UpdateParameters["telefono"].DefaultValue = TbTelefonoTecnoDiag.Text;
                SqlDirecciones.UpdateParameters["direccion"].DefaultValue = TbDireccionTecnoDiag.Text;
                SqlDirecciones.UpdateParameters["provincia"].DefaultValue = DropDownListProvincasTecnoDiag.SelectedValue.ToString();
                SqlDirecciones.UpdateParameters["ciudad"].DefaultValue = DropDownListDepartamentosTecnoDiag.SelectedValue.ToString();
                SqlDirecciones.UpdateParameters["codigoPostal"].DefaultValue = tbCodPostalTecnoDiag.Text;
                SqlDirecciones.UpdateParameters["tipo"].DefaultValue = "1";
                SqlDirecciones.Update();

                SqlDirecciones.UpdateParameters["idOrdeCompra"].DefaultValue = Request.QueryString["Id"];
                SqlDirecciones.UpdateParameters["telefono"].DefaultValue = TbTelefonoProv.Text;
                SqlDirecciones.UpdateParameters["direccion"].DefaultValue = TbDireccionProv.Text;
                SqlDirecciones.UpdateParameters["provincia"].DefaultValue = DropDownListProvinciasProveedor.SelectedValue.ToString();
                SqlDirecciones.UpdateParameters["ciudad"].DefaultValue = DropDownListDepartamentosProveedor.SelectedValue.ToString();
                SqlDirecciones.UpdateParameters["codigoPostal"].DefaultValue = tbCodPostalProv.Text;
                SqlDirecciones.UpdateParameters["tipo"].DefaultValue = "2";
                SqlDirecciones.Update();

                SqlDirecciones.UpdateParameters["idOrdeCompra"].DefaultValue = Request.QueryString["Id"];
                SqlDirecciones.UpdateParameters["direccion"].DefaultValue = TbDireccionEntrega.Text;
                SqlDirecciones.UpdateParameters["provincia"].DefaultValue = DropDownListEntrgaProvincia.SelectedValue.ToString();
                SqlDirecciones.UpdateParameters["ciudad"].DefaultValue = DropDownListEntrgaCiudad.SelectedValue.ToString();
                SqlDirecciones.UpdateParameters["codigoPostal"].DefaultValue = tbCodPostalEntrega.Text;
                SqlDirecciones.UpdateParameters["tipo"].DefaultValue = "3";
                SqlDirecciones.Update();
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar las direcciones de la orden de compra: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }


        private bool ValidarCampos()
        {
            bool validado = true;
            if (tbCodPostalProv.Text.Equals("") ||
                tbCodPostalTecnoDiag.Text.Equals("") ||
                TbDireccionProv.Text.Equals("") ||
                TbDireccionTecnoDiag.Text.Equals("") ||
                TbTelefonoProv.Text.Equals("") ||
                TbTelefonoTecnoDiag.Text.Equals(""))
            {
                validado = false;
            }
            return validado;
        }

        protected void BtnOrdenCom_Click(object sender, EventArgs e)
        {
            CalcualarTotal();
            Decimal total = Decimal.Parse(lblTotal.Text);
            DataTable dtPedidos = (DataTable)Session["ProdSelec"];
            if (total > 0 && dtPedidos.Rows.Count > 0)
            {
                if (ValidarCampos())
                {
                    try
                    {
                        int idOrd = int.Parse(Request.QueryString["Id"]);
                        modificarOrdenDeCompra();
                        BorrarDetallesOC();
                        insertarDetalleOrdenCom(idOrd);
                        UpdateDirecciones();
                        

                        Response.Redirect($"~/PantallaOrdendecompra/paginaOrdenCompra.aspx?Id={idOrd}");
                    }
                    catch (Exception ex)
                    {
                        MostrarAlerta("Ocurrió un error al registra la orden de compra " + ex.Message, NotificationType.error);
                    }
                }
                else
                {
                    MostrarAlerta("Faltan campos del proveedor o de tecnodiagnostica.", NotificationType.warning);
                }

            }
            else
            {
                MostrarAlerta("No hay productos seleccionados.", NotificationType.warning);
            }
        }

        private void InsertarCompra(int id)
        {
            SqlProductos.InsertParameters["idOrdComp"].DefaultValue = id.ToString();
            SqlProductos.InsertParameters["idProv"].DefaultValue = Request.QueryString["IdProv"];
            SqlProductos.InsertParameters["fechaSolicitud"].DefaultValue = DateTime.Now.ToString();
            SqlProductos.InsertParameters["idEstado"].DefaultValue = "1";
            SqlProductos.Insert();
        }

        private void insertarDetalleOrdenCom(int Id)
        {
            try
            {
                if (Id > 0)
                {
                    DataTable dtPedidos = (DataTable)Session["ProdSelec"];

                    if (dtPedidos == null || dtPedidos.Rows.Count == 0)
                    {
                        MostrarAlerta("No hay productos seleccionados para insertar.", NotificationType.warning);
                        Debug.WriteLine("La sesión 'ProdSelec' está vacía o no contiene filas.");
                        return;
                    }

                    Debug.WriteLine($"ID de la orden de compra: {Id}");
                    Debug.WriteLine($"Número de filas en ProdSelec: {dtPedidos.Rows.Count}");

                    foreach (DataRow fila in dtPedidos.Rows)
                    {
                        try
                        {

                            // Depuración del contenido de la fila
                            string idProducto = fila["IdProd"]?.ToString();
                            string cantidad = fila["CANTIDAD"]?.ToString();
                            string precioCompra = fila["PrecioUnitario"]?.ToString();
                            string total = fila["Total"]?.ToString();
                            int idDetalle = int.Parse(fila["ID_DETALLE"]?.ToString());

                            Debug.WriteLine($"Procesando fila: IdProd={idProducto}, Cantidad={cantidad}, PrecioUnitario={precioCompra}, Total={total}");

                            // Validar los valores antes de usarlos
                            if (string.IsNullOrEmpty(idProducto) || string.IsNullOrEmpty(cantidad) ||
                                string.IsNullOrEmpty(precioCompra) || string.IsNullOrEmpty(total))
                            {
                                Debug.WriteLine("Fila con valores nulos o vacíos. Saltando inserción.");
                                continue;
                            }

                            if (idDetalle == 0)
                            {
                                SqlDetalleOrdCom.InsertParameters["ID_ORD_COMP"].DefaultValue = Id.ToString();
                                SqlDetalleOrdCom.InsertParameters["ID_PROV"].DefaultValue = Request.QueryString["IdProv"];
                                SqlDetalleOrdCom.InsertParameters["IdProd"].DefaultValue = fila["IdProd"].ToString();
                                SqlDetalleOrdCom.InsertParameters["CANTIDAD"].DefaultValue = fila["CANTIDAD"].ToString() ;
                                SqlDetalleOrdCom.InsertParameters["PrecioUnitario"].DefaultValue = fila["PrecioUnitario"].ToString();
                                SqlDetalleOrdCom.InsertParameters["Total"].DefaultValue = fila["Total"].ToString();

                                SqlDetalleOrdCom.Insert(); // Ejecutar la inserción
                                Debug.WriteLine($"Fila insertada correctamente: IdProd={idProducto}");
                            }
                            else 
                            {

                                SqlDetalleOrdCom.UpdateParameters["idDetalle"].DefaultValue = fila["ID_DETALLE"].ToString();
                                SqlDetalleOrdCom.UpdateParameters["CANTIDAD"].DefaultValue = fila["CANTIDAD"].ToString(); ;
                                SqlDetalleOrdCom.UpdateParameters["Total"].DefaultValue = fila["Total"].ToString();

                                SqlDetalleOrdCom.Update(); // Ejecutar la inserción
                                Debug.WriteLine($"Fila insertada correctamente: IdProd={idProducto}");
                            }
                        }
                        catch (Exception exFila)
                        {
                            Debug.WriteLine($"Error al insertar la fila: {exFila.Message}");
                            MostrarAlerta($"Error al insertar el producto {fila["IdProd"]}: {exFila.Message}", NotificationType.error);
                        }
                    }

                    MostrarAlerta("Todos los productos procesados correctamente.", NotificationType.success);
                }
                else
                {
                    MostrarAlerta("El ID de la orden de compra no es válido.", NotificationType.error);
                    Debug.WriteLine("El ID proporcionado es inválido.");
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al registrar el detalle de la orden de compra: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
                Debug.WriteLine($"Error en el método insertarDetalleOrdenCom: {ex.Message}");
            }
        }






        private void AlertaCookies(string mensaje, NotificationType tipo)
        {

            string script = $@"
                Swal.fire({{
                title: '{mensaje}',
                icon: '{tipo.ToString().ToLower()}',
                showDenyButton: true,
                confirmButtonText: 'Aceptar',
                denyButtonText: `Cancelar`,
                confirmButtonColor: '#305d6',  // Color del botón de confirmar
                denyButtonColor: '#d33'         // Color del botón de negar (Deny)
                }}).then((result) => {{
                if (result.isConfirmed) {{
                    __doPostBack('UpdCookeDatos', 'Aceptar'); // Enviar al servidor
                }} else if (result.isDenied) {{
                                        // Enviar al servidor
                }}
                }});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertaCookies", script, true);
        }

        private void ConfirmacionDeOpciones()
        {
            string target = Request.Form["__EVENTTARGET"];
            string argument = Request.Form["__EVENTARGUMENT"];

            if (target == "UpdCookeDatos") // Asegúrate de que coincida con el UpdatePanel
            {
                if (argument == "Aceptar")
                {
                    HttpCookie telefono = new HttpCookie("telefono", TbTelefonoTecnoDiag.Text);
                    telefono.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(telefono);
                    HttpCookie direccion = new HttpCookie("direccion", TbDireccionTecnoDiag.Text);
                    direccion.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(direccion);
                    HttpCookie provincia = new HttpCookie("provincia", DropDownListProvincasTecnoDiag.SelectedValue);
                    provincia.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(provincia);
                    HttpCookie ciudad = new HttpCookie("ciudad", DropDownListDepartamentosTecnoDiag.SelectedValue);
                    ciudad.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(ciudad);
                    HttpCookie codigoPostal = new HttpCookie("codigoPostal", tbCodPostalTecnoDiag.Text);
                    codigoPostal.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(codigoPostal);

                }
            }
        }

        private void UtilizarCookie()
        {
            if (Request.Cookies["telefono"] != null)
            {
                TbTelefonoTecnoDiag.Text = Request.Cookies["telefono"].Value;
                TbDireccionTecnoDiag.Text = Request.Cookies["direccion"].Value;
                DropDownListProvincasTecnoDiag.SelectedValue = Request.Cookies["provincia"].Value;
                DropDownListDepartamentosTecnoDiag.SelectedValue = Request.Cookies["ciudad"].Value;
                tbCodPostalTecnoDiag.Text = Request.Cookies["codigoPostal"].Value;
            }
        }

        protected void GuardarDatos_Click(object sender, EventArgs e)
        {
            if (!TbTelefonoTecnoDiag.Text.Equals("") && !TbDireccionTecnoDiag.Text.Equals("") && !tbCodPostalTecnoDiag.Text.Equals(""))
            {
                string mensaje = "¿Desea guardar los datos de tecnodiagnostica en el navegado, para que estos se carguen automaticamente?";
                AlertaCookies(mensaje, NotificationType.info);
            }
            else
            {
                MostrarAlerta("Complete los campos de tecnodiagnostica", NotificationType.warning);
            }

        }
    }
}