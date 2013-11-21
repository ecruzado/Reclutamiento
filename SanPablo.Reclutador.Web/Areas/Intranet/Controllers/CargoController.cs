using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    public class CargoController : Controller
    {
        //
        // GET: /Intranet/Cargo/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaCargo(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[11]
        {
          "200001",
          "200001",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[11]
        {
          "200002",
          "200002",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[11]
        {
          "200003",
          "200003",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType2_4 = new
            {
                id = 4,
                cell = new string[11]
        {
          "200004",
          "200004",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_4);
            var fAnonymousType2_5 = new
            {
                id = 5,
                cell = new string[11]
        {
          "200005",
          "200005",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_5);
            var fAnonymousType2_6 = new
            {
                id = 6,
                cell = new string[11]
        {
          "200006",
          "200006",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_6);
            var fAnonymousType2_7 = new
            {
                id = 7,
                cell = new string[11]
        {
          "200007",
          "200007",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_7);
            var fAnonymousType2_8 = new
            {
                id = 8,
                cell = new string[11]
        {
          "200008",
          "200008",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_8);
            var fAnonymousType2_9 = new
            {
                id = 9,
                cell = new string[11]
        {
          "200009",
          "200009",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_9);
            var fAnonymousType2_10 = new
            {
                id = 10,
                cell = new string[11]
        {
          "200010",
          "200010",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_10);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }
    }
}
