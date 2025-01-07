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
    public partial class PromedioVentasMensualesClientes : System.Web.UI.Page
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

        protected string ObtenerDatos()
        {
            // Columnas de datos
            DataTable datos = new DataTable();
            datos.Columns.Add(new DataColumn("Task", typeof(string)));
            datos.Columns.Add(new DataColumn("Hours per day", typeof(int))); // Cambié a 'int' ya que los datos son numéricos

            // Datos de las columnas
            datos.Rows.Add(new Object[] { "Work", 11 });
            datos.Rows.Add(new Object[] { "Eat", 2 });
            datos.Rows.Add(new Object[] { "Commute", 2 });
            datos.Rows.Add(new Object[] { "Watch tv", 2 });
            datos.Rows.Add(new Object[] { "Sleep", 7 });

            string stringDatos = "[['Task', 'Hours per day'],";

            foreach (DataRow row in datos.Rows)
            {
                stringDatos += "['" + row[0] + "'," + row[1] + "],"; // Corrección en la concatenación
            }

            // Eliminar la última coma extra y cerrar la lista
            stringDatos = stringDatos.TrimEnd(',') + "]";
            Debug.WriteLine(stringDatos);
            return stringDatos;
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
                        // Convertir DataRowView a DataRow
                        DataRow row = rowView.Row;

                        // Obtener los valores
                        string razonSocial = row["RAZON_SOCIAL"].ToString();
                        int cantidad = Convert.ToInt32(row["PorcentajeVentas"]);
                        datos.Rows.Add(new Object[] { razonSocial, cantidad });
                    }
                    foreach (DataRow row in datos.Rows)
                    {
                        stringDatos += "['" + row[0] + "'," + row[1] + "],"; // Corrección en la concatenación
                    }

                    // Eliminar la última coma extra y cerrar la lista
                    stringDatos = stringDatos.TrimEnd(',') + "]";

                    Debug.WriteLine(stringDatos);
                    return stringDatos;
                }
                else
                {
                    MostrarAlerta("No se registraron ventas en esa fecha.", NotificationType.error);
                    return stringDatos;
                }
            }catch (Exception ex) 
            {
                string mensaje = "Ocurrió un error al cargar el informe: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
                return mensaje;
            }
        }

    }
}