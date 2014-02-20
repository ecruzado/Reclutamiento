using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SanPablo.Reclutador.Web.Models;
using SanPablo.Reclutador.Web.Models.JQGrid;

namespace SanPablo.Reclutador.Web.Controllers
{
    public class SeguridadController : Controller
    {
        //
        // GET: /Seguridad/

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Logon(SeguridadViewModel model)
        {
            JsonMessage ObjJsonMessage = new JsonMessage();

            ObjJsonMessage.Resultado = true;


            return Json(ObjJsonMessage);
        }

    }
}
