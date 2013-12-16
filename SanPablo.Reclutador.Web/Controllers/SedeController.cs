using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Repository;
using SanPablo.Reclutador.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Controllers
{
    public class SedeController : Controller
    {
        private ISedeRepository _sedeRepository;

        public SedeController(ISedeRepository sedeRepository)
        {
            _sedeRepository = sedeRepository;
        }

        public ActionResult Index()
        {
            var lista = _sedeRepository.All();
            return View();
        }

        [HttpGet]
        public ActionResult Crear() 
        {
            var sede = new Sede();
            return View("Edit",sede);
        }

        [HttpPost]
        public ActionResult Crear(Sede sede)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return View("Edit", sede);
            }

            TempData["notice"] = "Person successfully created";
            return View("Edit", sede);
        }

        [HttpGet]
        public ActionResult Editar(string codigo)
        {
            var sede = _sedeRepository.GetSingle(x => x.CodigoSede == codigo);
            return View("Edit", sede);
        }

    }
}
