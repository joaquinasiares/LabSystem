using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static LabSystemPP3.Notificacion.Enum;

namespace LabSystemPP3.Controllers
{
    public class AlertControlle : Controller
    {
        public class AlertController : BaseController
        {
            [HttpGet]
            public ActionResult ShowAlert(string mensje, NotificationType notificacion)
            {
                Alert(mensje, notificacion);
                return View();
            }
        }
    }
}