using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Data.SqlClient;
using System.Diagnostics;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.Pantalla_Informes
{
    public partial class TopCincoProductos : System.Web.UI.Page
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
            try
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
            }catch (Exception ex) 
            {
                string mensaje = "Ocurrió un error al cargar el informe: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
                return mensaje;
            }
        }

    }
}