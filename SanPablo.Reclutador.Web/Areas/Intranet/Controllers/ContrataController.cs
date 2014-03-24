

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using FluentValidation;
    using FluentValidation.Results;
    using Newtonsoft.Json;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    
    
    public class ContrataController : Controller
    {

        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICvPostulanteRepository _cvPostulanteRepository;
        private IReclutamientoPersonaRepository _reclutamientoPersonaRepository;
        private IPostulanteRepository _postulanteRepository;



        public ContrataController(IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository,
                                         ICvPostulanteRepository cvPostulanteRepository,
                                        IReclutamientoPersonaRepository reclutamientoPersonaRepository,
            IPostulanteRepository postulanteRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _cvPostulanteRepository = cvPostulanteRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _postulanteRepository = postulanteRepository;
           
        }
        
        /// <summary>
        /// obtiene los postulantes seleccionados
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipSol"></param>
        /// <returns></returns>
        public ActionResult Index(int id, string tipSol)
        {
            
           RankingViewModel model = new RankingViewModel();

           model = new RankingViewModel();
           model.Solicitud = new SolReqPersonal();
           model.ReclutaPersonal = new ReclutamientoPersona();


           model.Solicitud.IdeSolReqPersonal = id;
           model.Solicitud.Tipsol = tipSol;
           List<SolReqPersonal> listaSol = _solReqPersonalRepository.GetDatosSol(model.Solicitud);

           if (listaSol != null && listaSol.Count > 0)
           {
               model.Solicitud = (SolReqPersonal)listaSol[0];
           }
           // se incializa
           model.Solicitud.IdeSolReqPersonal = id;
           model.Solicitud.Tipsol = tipSol;

           return View("Index", model);
        }





    }
}
