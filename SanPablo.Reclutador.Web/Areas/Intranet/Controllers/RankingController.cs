

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
    
    public class RankingController : BaseController
    {
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;


        public RankingController(IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository
                                         
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
           
        }



        /// <summary>
        /// inicializa el ranking
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int id, string tipSol)
        {
            RankingViewModel model;
            
            model = new RankingViewModel();
            model.Solicitud = new SolReqPersonal();


            model.Solicitud.IdeSolReqPersonal = id;
            model.Solicitud.Tipsol = tipSol;
            List<SolReqPersonal> listaSol = _solReqPersonalRepository.GetDatosSol(model.Solicitud);

            if (listaSol!=null && listaSol.Count>0)
            {
                model.Solicitud = (SolReqPersonal)listaSol[0];
            }
            // se incializa
            model.Solicitud.IdeSolReqPersonal = id;
            model.Solicitud.Tipsol = tipSol;
            // consulta que obtiene los datos de la solicitud por id y Tipo de Puesto

            return View("Index",model);
        }

       /// <summary>
       /// Inicializa el popup para ingresa el cv de los postulantes
       /// </summary>
       /// <param name="id"></param>
       /// <param name="idPos"></param>
       /// <param name="tipSol"></param>
       /// <returns></returns>
        public ActionResult inicoPopupCv(int id, string idPos, string tipSol) 
        {
            JsonMessage objJson = new JsonMessage();
            RankingViewModel model = new RankingViewModel();
            model.Solicitud = new SolReqPersonal();
            model.CvPostulanteEx = new CvPostulante();

            model.Solicitud.IdeSolReqPersonal = id;
            model.Solicitud.Tipsol = tipSol;
            model.CvPostulanteEx.IdCvPostulante = Convert.ToInt32(idPos);

            return View("PopupCvPostulante", model);

        
        }
        /// <summary>
        /// guarda los datos del postulante relacionados a una solicitud
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetCvPostulante(RankingViewModel model)
        {
            JsonMessage objJson;
            objJson = new JsonMessage();
            int idPostulante=0;
            int idSolicitud = 0;
            string tipSol = null;

            if (model !=null)
            {
               idPostulante= model.CvPostulanteEx.IdCvPostulante;
               idSolicitud = (int)model.Solicitud.IdeSolReqPersonal;
               tipSol = model.Solicitud.Tipsol;

               if (idPostulante>0)
               {
                   //actualiza
                   
               }
               else
               {
                   //inserta
               }
                
            }

            

            return Json(objJson);

        }
        


    }
}
