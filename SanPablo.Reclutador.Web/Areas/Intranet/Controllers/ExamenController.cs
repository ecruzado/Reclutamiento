using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    public class ExamenController : Controller
    {
        //
        // GET: /Intranet/Examen/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaExamen(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "250001",
          "250001",
          "Exa01",
          "Examen 01",
          "Evaluación",
          "20",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "250002",
          "250002",
          "Exa02",
          "Examen 02",
          "Entrevista",
          "30",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

    }
}
