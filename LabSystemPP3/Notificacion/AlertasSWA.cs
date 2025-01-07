using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static LabSystemPP3.Notificacion.Enum;
namespace LabSystemPP3.Notificacion
{


    public static class AlertasSWA
    {
        public static string GenerateAlert(string message, NotificationType notificationType)
        {
            return $"<script language='javascript'>Swal.fire('{notificationType.ToString().ToUpper()}', '{message}', '{notificationType}')</script>";
        }
    }

}