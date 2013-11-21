using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    public class CriterioController : Controller
    {
        //
        // GET: /Intranet/Criterio/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaCriterio(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[11]
        {
          "100001",
          "100001",
          "¿Cuál es la capital del Perú?",
          "Desempeño",
          "Examen",
          "Texto",
          "Manual",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[11]
        {
          "100002",
          "100002",
          "¿Cómo se llama el Presidente del Perú?",
          "Desempeño",
          "Examen",
          "Texto",
          "Manual",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin"
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[11]
        {
          "100003",
          "100003",
          "",
          "Desempeño",
          "Examen",
          "Imagen",
          "Automática",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin"        
        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaAlternativaxCriterio(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "1",
          "Arequipa",
          "0",
          ""
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "2",
          "Cuzco",
          "0",
          ""
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[]
        {
          "3",
          "Lima",
          "5",
          ""
        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType2_4 = new
            {
                id = 4,
                cell = new string[]
        {
          "4",
          "La Paz",
          "0",
          ""
        }
            };
            list.Add((object)fAnonymousType2_4);
            var fAnonymousType2_5 = new
            {
                id = 5,
                cell = new string[]
        {
          "5",
          "Bogotá",
          "0",
          ""
        }
            };
            list.Add((object)fAnonymousType2_5);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }    
    }
}
