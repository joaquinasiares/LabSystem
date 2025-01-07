using CapaEntidades;
using CapaNegocios;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.PantallaOrdendecompra
{
    public partial class paginaOrdenCompra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Detecta si la página se está generando para PDF
            if (Request.QueryString["pdf"] == "true")
            {
                // Oculta elementos innecesarios
                PanelOriginal.Visible = false; // Panel con contenido regular
                PanelVolver.Visible = false;
            }
            else
            {
                // Mostrar la página normal
                PanelOriginal.Visible = true;
                PanelVolver.Visible = true;
            }
            if (!IsPostBack)
            {
                
                LblID.Text= Request.QueryString["Id"];
                CargarDatos();
            }
        }

        private DataTable CargarEmpleados()
        {
            DataTable dt = new DataTable();

            // Asignar el SqlDataSource al DataTable
            SqlDataAdapter da = new SqlDataAdapter(SqlEmpleado.SelectCommand, SqlEmpleado.ConnectionString);
            da.Fill(dt);

            return dt;
        }
        private async void CargarDatos()
        {
            try
            {
                string idOc = Request.QueryString["Id"];
                lblNumOrden.Text = idOc;
                this.SqlOrdenDeCompra.SelectParameters["IdOc"].DefaultValue = idOc;
                this.SqlOrdenDeCompra.DataSourceMode = SqlDataSourceMode.DataReader;
                SqlDataReader datos;
                datos = (SqlDataReader)this.SqlOrdenDeCompra.Select(DataSourceSelectArguments.Empty);
                if (datos.Read())
                {

                    lblRs.Text = datos["NOMBRE_PROV"].ToString();
                    LblFecha.Text = Convert.ToDateTime(datos["FECHA"]).ToString("dd/MM/yyyy");
                    lblTotal.Text = datos["Total"].ToString();
                    if (datos["Delivery"] != null)
                    {
                        LblViatico.Text = datos["Delivery"].ToString();
                    }
                    if (datos["PrecioIva"] != null)
                    {
                        lblIva.Text = datos["PrecioIva"].ToString();
                    }

                    DataTable dt = CargarEmpleados();

                    foreach (DataRow row in dt.Rows)
                    {
                        // Compara el ID del empleado con el valor 'Autorizacion'
                        if (row["ID_EMPLEADO_SERVICIO"].ToString() == datos["Autorizacion"].ToString())
                        {
                            lblAutorizacion.Text = row["empleado"].ToString();
                        }
                    }

                    if (datos["instrucciones"] != null)
                    {
                        lblInstrucciones.Text = datos["instrucciones"].ToString();
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
                    int tipo = int.Parse(datosDir["tipo"].ToString());
                    if (tipo==1)
                    {
                        lblTelefonoTecno.Text = datosDir["telefono"].ToString();
                        lblDireccionTecno.Text = datosDir["direccion"].ToString();
                        lblProvinciaTecno.Text = datosDir["provincia"].ToString();
                        await SeleccionarProvincia(datosDir["provincia"].ToString(), tipo);
                        await CargarCiudadesEntrega(datosDir["provincia"].ToString(), datosDir["ciudad"].ToString(), tipo);
                        lblCodPostalTecno.Text = datosDir["codigoPostal"].ToString();
                    }

                    if (tipo==2)
                    {
                        lblTelefono.Text = datosDir["telefono"].ToString();
                        lblDireccion.Text = datosDir["direccion"].ToString();
                        await SeleccionarProvincia(datosDir["provincia"].ToString(), tipo);
                        await CargarCiudadesEntrega(datosDir["provincia"].ToString(), datosDir["ciudad"].ToString(), tipo);
                        lblCodPostal.Text = datosDir["codigoPostal"].ToString();
                    }

                    if (tipo==3)
                    {
                        lblDireccionEntrega.Text = datosDir["direccion"].ToString();
                        await SeleccionarProvincia(datosDir["provincia"].ToString(), tipo);
                        await CargarCiudadesEntrega(datosDir["provincia"].ToString(),datosDir["ciudad"].ToString(), tipo);
                        lblCodPostalEntrega.Text = datosDir["codigoPostal"].ToString();
                    }
                }

                this.SqlProductos.SelectParameters["IdOc"].DefaultValue = idOc;
                GrillaProductoPedido.DataSource = this.SqlProductos;
                GrillaProductoPedido.DataBind();
                lblTotProd.Text = this.GrillaProductoPedido.DataKeys[0].Values["TotalGeneral"].ToString();
            }catch(Exception ex) 
            {
                MostrarAlerta("a: "+ex.Message,NotificationType.error);
            }
        }

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

        private void Imprimir()
        {
            string idOc = Request.QueryString["Id"];
            if (string.IsNullOrEmpty(idOc))
            {
                MostrarAlerta("No se encontró el ID para generar el PDF.", NotificationType.error);
                return;
            }

            // Captura la URL actual y agrega el parámetro pdf=true e Id=...
            string url = Request.Url.GetLeftPart(UriPartial.Path) + $"?pdf=true&Id={idOc}";

            // Configurar SelectPdf
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MinPageLoadTime = 5;

            // Generar el PDF
            PdfDocument pdfDocument = converter.ConvertUrl(url);

            // Descargar el PDF
            using (MemoryStream ms = new MemoryStream())
            {
                pdfDocument.Save(ms);
                pdfDocument.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Reporte.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }

        private async Task CargarCiudadesEntrega(string idProvincia, string idDepartamento, int tipo)
        {
            string apiUrl = $"https://apis.datos.gob.ar/georef/api/departamentos?provincia={idProvincia}&campos=id,nombre&max=100";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializa el JSON
                    dynamic resultado = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("nombre");
                    dt.Columns.Add("id");

                    // Llenar DataTable con departamentos
                    foreach (var depto in resultado.departamentos)
                    {
                        dt.Rows.Add(depto.nombre.ToString(), depto.id.ToString());
                    }

                    // Buscar departamento por ID
                    DataRow[] filas = dt.Select($"id = '{idDepartamento}'");

                    if (filas.Length > 0)
                    {
                        string nombreDepto = filas[0]["nombre"].ToString();

                        // Actualizar el texto según el tipo
                        switch (tipo)
                        {
                            case 1:
                                lblCiudadTecno.Text = nombreDepto;
                                break;
                            case 2:
                                lblCiudadProveedor.Text = nombreDepto;
                                break;
                            case 3:
                                lblCiudadEntrega.Text = nombreDepto;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores
                string mensaje = "Error al cargar los departamentos para la entrega: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }

        private async Task SeleccionarProvincia(string idProvincia, int tipo)
        {
            string apiUrl = $"https://apis.datos.gob.ar/georef/api/provincias?id={idProvincia}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializar JSON
                    dynamic resultado = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                    // Accede al primer elemento del array de provincias
                    var provincia = resultado.provincias[0];

                    // Recupera el nombre o el id de la provincia según sea necesario
                    string nombreProvincia = provincia.nombre.ToString();
                    string idProvinciaEncontrada = provincia.id.ToString();

                    switch (tipo)
                    {
                        case 1:
                            lblProvinciaTecno.Text = nombreProvincia;
                            break;
                        case 2:
                            lblProvinciaProveedor.Text = nombreProvincia;
                            break;
                        case 3:
                            lblProvinciaEntrega.Text = nombreProvincia;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores
                string mensaje = "Error al cargar la provincia: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
            }
        }



        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            Imprimir();

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/PantallaProveedores/GestorProveedores.aspx");
        }
    }
}
