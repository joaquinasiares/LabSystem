using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                UtilizarCookie();
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



        private void BuscarUsuario() 
        {
            SqlBuscarUsuario.SelectParameters["usuario"].DefaultValue = usuario.Text.Trim();
            SqlBuscarUsuario.SelectParameters["contrasenia"].DefaultValue = password.Text.Trim();


            DataView dv = (DataView)SqlBuscarUsuario.Select(DataSourceSelectArguments.Empty);

            if (dv != null && dv.Count > 0)
            {
                if (recordar.Checked) 
                {
                    CrearCookies();
                }

                Session["NombreUsuario"] = dv[0]["USUARIO"].ToString();
                Session["ClaveUsuario"] = dv[0]["CLAVE"].ToString();
                Session["IdTipoUsuario"] = dv[0]["idTipoUsu"].ToString();
                Session["DescTipoUsuario"] = dv[0]["descTipoUsu"].ToString();
                string url = "~/Default.aspx";
                Response.Redirect(url);
            }
            else
            {

                MostrarAlerta("Usuario o contraseña incorrectos.", NotificationType.error);
            }
        }

        protected void SqlBuscarUsuario_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            // Verifica si hay filas devueltas
            if (e.AffectedRows > 0)
            {
                // Usuario encontrado
                string url = "~/Default.aspx";
                Response.Redirect(url);
            }
            else
            {
                // Usuario no encontrado
                MostrarAlerta("Usuario o contraseña incorrectos.",NotificationType.error);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (usuario.Text.Equals("") || password.Text.Equals(""))
            {
                MostrarAlerta("Ingrese el usuario o contraseña.", NotificationType.error);
            }
            else
            {
                //if (IsReCaptchValid())
                //{
                    //string messageToDB = txtMensaje.Value;
                    BuscarUsuario();
                /*}
                else
                {
                    MostrarAlerta("Validación de Captcha incorrecta.", NotificationType.error);
                }*/
            }
        }

        private void CrearCookies() 
        {
            HttpCookie Usuario = new HttpCookie("usuario",usuario.Text);
            Usuario.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(Usuario);
            
        }

        private void UtilizarCookie()
        {
            if (Request.Cookies["usuario"] != null)
            {
                usuario.Text = Request.Cookies["usuario"].Value;
            }
        }

        //Método para validar Captcha
        public bool IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }
    }
}