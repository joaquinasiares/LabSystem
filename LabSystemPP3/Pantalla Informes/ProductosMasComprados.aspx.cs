using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabSystemPP3.Pantalla_Informes
{
    public partial class ProductosMasComprados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Llamar al método cargarInforme() para obtener los datos
                string datosInforme = cargarInforme();

                // Inyectar los datos en JavaScript usando ClientScript.RegisterStartupScript
                string script = @"
            google.charts.load('current', { packages: ['corechart'] });
            google.charts.setOnLoadCallback(function() {
                var data = google.visualization.arrayToDataTable(" + datosInforme + @");
                var options = {
                    title: 'Cinco productos más vendidos',
                    width: 900,
                    height: 500,
                    bar: { groupWidth: '95%' },
                    legend: { position: 'none' }
                };
                var chart = new google.visualization.BarChart(document.getElementById('barchart_values'));
                chart.draw(data, options);
            });
        ";

                // Registrar el script en la página
                ClientScript.RegisterStartupScript(this.GetType(), "drawChartScript", script, true);
            }
        }

        protected string cargarInforme()
        {
            // Ajustar encabezado: solo dos columnas (Descripción y Total)
            string stringDatos = "[['Descripcion', 'Total'],";
            SqlDataSource1.DataSourceMode = SqlDataSourceMode.DataReader;

            using (SqlDataReader reader = (SqlDataReader)SqlDataSource1.Select(DataSourceSelectArguments.Empty))
            {
                while (reader.Read())
                {
                    string proveedor = reader["NOMBRE_PROV"].ToString();
                    string nombreProducto = reader["DESCRIPCION"].ToString();
                    string descripcion = $"Proveedor: {proveedor}, Producto: {nombreProducto}";
                    int total = Convert.ToInt32(reader["total"]); // Convertir a número

                    // Construir fila
                    stringDatos += $"['{descripcion}', {total}],";
                }
            }

            // Eliminar la última coma extra y cerrar el arreglo
            if (stringDatos.EndsWith(","))
            {
                stringDatos = stringDatos.TrimEnd(',');
            }
            stringDatos += "]";

            // Debug para verificar los datos generados
            Debug.WriteLine(stringDatos);
            return stringDatos;
        }


    }
}