using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.Pantalla_Informes
{
    public partial class TresEquiposReparacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //MostrarGrafico();
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

        protected string cargarInforme()
        {
            try
            {
                string stringDatos = "[['Nombre', 'Promedio'],";
                SqlDataSource1.DataSourceMode = SqlDataSourceMode.DataReader;

                using (SqlDataReader reader = (SqlDataReader)SqlDataSource1.Select(DataSourceSelectArguments.Empty))
                {
                    while (reader.Read())
                    {
                        string nombre = reader["DESCRIPCION"].ToString();
                        int promedio = Convert.ToInt32(reader["total"]); // Asegurar que es un número

                        // Construir la fila
                        stringDatos += $"['{nombre}', {promedio}],";
                    }
                }

                // Eliminar la última coma extra y cerrar la lista
                if (stringDatos.EndsWith(","))
                {
                    stringDatos = stringDatos.TrimEnd(',');
                }
                stringDatos += "]";


                Debug.WriteLine(stringDatos); // Verifica el resultado en la consola de depuración
                return stringDatos;
            }
            catch (Exception ex)
            {
                string mensaje = "Ocurrió un error al cargar el informe: " + ex.Message;
                MostrarAlerta(mensaje, NotificationType.error);
                return mensaje;
            }
        }

    }
}