using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    public class EvaluacionController : Controller
    {
        //
        // GET: /Intranet/Evaluacion/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaExamenesPendientes(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[5]
        {
          "200001",
          "Proactividad",
          "10 minutos",
          "Evaluado",
          "0"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[5]
        {
          "200001",
          "Comunicación Interpersonal",
          "30 minutos",
          "Pendiente",
          "1"
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[5]
        {
          "200001",
          "Cultura General",
          "20 minutos",
          "Pendiente",
          "2"
        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        public ActionResult InstruccionesExamen()
        {
            return (ActionResult)this.View("InstruccionesExamen");
        }
        public ActionResult Examen()
        {
            return (ActionResult)this.View("Examen");
        }

        public ActionResult Examen1()
        {
            return (ActionResult)this.View("Examen1");
        }

        public ActionResult Examen2()
        {
            return (ActionResult)this.View("Examen2");
        }

        public ActionResult Examen3()
        {
            return (ActionResult)this.View("Examen3");
        }

        public ActionResult Examen4()
        {
            return (ActionResult)this.View("Examen4");
        }

    }
}
