

namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;
    
    public class OportunidadLaboralController : BaseController
    {
        /// <summary>
        /// pagina Inicial de Oportunidades laborales
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            OportunidadLaboralViewModel model = new OportunidadLaboralViewModel();
            model.oportunidadLaboral = new OportunidadLaboral();

            return View("OportunidadLaboral", model);
        }

       




    }
}
