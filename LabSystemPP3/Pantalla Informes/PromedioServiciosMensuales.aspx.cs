using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.Pantalla_Informes
{
    public partial class PromedioServiciosMensuales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected string cargarInforme()
        {
            string stringDatos = "[['Razon social', 'Cantidad'],";

            DataTable datos = new DataTable();
            datos.Columns.Add(new DataColumn("Razon social", typeof(string)));
            datos.Columns.Add(new DataColumn("Porcentaje de ventas", typeof(int)));

            try
            {
                // Establecer los parámetros para la consulta
                SqlCargarInforme.SelectParameters["mes"].DefaultValue = ddlMeses.SelectedValue;
                SqlCargarInforme.SelectParameters["anio"].DefaultValue = TbAño.Text;

                // Ejecutar la consulta y obtener el DataView
                DataView dv = (DataView)SqlCargarInforme.Select(DataSourceSelectArguments.Empty);

                if (dv != null && dv.Count > 0)
                {
                    // Iterar sobre cada DataRowView
                    foreach (DataRowView rowView in dv)
                    {
                        DataRow row = rowView.Row;

                        string razonSocial = row["RAZON_SOCIAL"].ToString();
                        int cantidad = Convert.ToInt32(row["PorcentajeVentas"]);
                        datos.Rows.Add(new Object[] { razonSocial, cantidad });
                    }

                    // Construir la cadena JSON
                    foreach (DataRow row in datos.Rows)
                    {
                        stringDatos += "['" + row[0] + "', " + row[1] + "],";
                    }

                    // Eliminar la última coma extra y cerrar el arreglo
                    stringDatos = stringDatos.TrimEnd(',') + "]";
                }
                else
                {
                    MostrarAlerta("No se registraron servicios en esa fecha.", NotificationType.error);
                    stringDatos += "]";
                }

                Debug.WriteLine(stringDatos); // Verificar el JSON generado en la consola
                return stringDatos;
            }catch (Exception ex) 
            {
                string mensaje = "Ocurrió un error al cargar el informe: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
                return mensaje;
            }
        }
    }
}