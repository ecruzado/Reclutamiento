using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    public class SolicitudReemplazoCargoController : Controller
    {
        //
        // GET: /Intranet/SolicitudReemplazoCargo/


        /// <summary>
        /// Inicializa la consulta de solicitud de reemplazo de nuevo cargo
        /// </summary>
        /// <returns></returns>
        public ActionResult ListaReemplazo()
        {
            return View("ListaReemplazo");
        }




    }
}
