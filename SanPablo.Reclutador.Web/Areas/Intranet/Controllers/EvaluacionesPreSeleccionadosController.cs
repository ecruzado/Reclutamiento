
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
    
    public class EvaluacionesPreSeleccionadosController : BaseController
    {
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICvPostulanteRepository _cvPostulanteRepository;
        private IReclutamientoPersonaRepository _reclutamientoPersonaRepository;
        private IPostulanteRepository _postulanteRepository;
        private IReclutamientoPersonaExamenRepository _reclutamientoPersonaExamenRepository;



        public EvaluacionesPreSeleccionadosController(IDetalleGeneralRepository detalleGeneralRepository,
                                                      ISolReqPersonalRepository solReqPersonalRepository,
                                                      ICvPostulanteRepository cvPostulanteRepository,
                                                      IReclutamientoPersonaRepository reclutamientoPersonaRepository,
                                                      IPostulanteRepository postulanteRepository,
                                                      IReclutamientoPersonaExamenRepository reclutamientoPersonaExamenRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _cvPostulanteRepository = cvPostulanteRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _postulanteRepository = postulanteRepository;
            _reclutamientoPersonaExamenRepository = reclutamientoPersonaExamenRepository;
           
        }



        /// <summary>
        /// inicializa las evaluaciones
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Index()
        {
            var modelEvaluaciones = inicializarEvaluacionesPreseleccionados();
            return View("EvaluacionesPreSeleccionados");
        }

        /// <summary>
        /// Inicializar el model de la evaluacion de preseleccionados
        /// </summary>
        /// <returns></returns>
        public EvaluacionesPreSeleccionadosViewModel inicializarEvaluacionesPreseleccionados()
        {
            EvaluacionesPreSeleccionadosViewModel model = new EvaluacionesPreSeleccionadosViewModel();
            model.PostulantePreSel = new Postulante();
            model.Solicitud = new SolReqPersonal();

            return model;
        }


        [HttpPost]
        public ActionResult ListPostulantesPre(GridTable grid)
         {

             ReclutamientoPersona reclutamientoPersona;
             List<ReclutamientoPersona> lista = new List<ReclutamientoPersona>();
             try
             {

                 reclutamientoPersona = new ReclutamientoPersona();

                 reclutamientoPersona.IdeSol = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
                 reclutamientoPersona.TipSol = (grid.rules[1].data == null ? "" : Convert.ToString(grid.rules[1].data));
               

                 if ("0".Equals(reclutamientoPersona.EstPostulante))
                 {
                     reclutamientoPersona.EstPostulante = "";
                 }

                 lista = _postulanteRepository.GetPostulantesPreseleccionado(reclutamientoPersona);

                 var generic = GetListar(lista,
                                          grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                 generic.Value.rows = generic.List.Select(item => new Row
                 {
                     id = (item.IdeReclutaPersona.ToString()),
                     cell = new string[]
                            {
                                item.IdeReclutaPersona==null?"":item.IdeReclutaPersona.ToString(),
                                item.IdePostulante==null?"":item.IdePostulante.ToString(),
                                item.IdeSol==null?"":item.IdeSol.ToString(),
                                item.IdSede==null?"":item.IdSede.ToString(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.Apellidos==null?"":item.Apellidos,
                                item.Nombres==null?"":item.Nombres,
                                item.FonoFijo==null?"":item.FonoFijo,
                                item.FonoMovil==null?"":item.FonoMovil,
                                item.IndicadorContactado==null?"":item.IndicadorContactado.ToString(),
                                item.IdePostulante==null?"":item.IdePostulante.ToString(),
                                item.EstPostulante==null?"":item.EstPostulante.ToString(),
                                item.DesEstadoPostulante==null?"":item.DesEstadoPostulante,
                                item.EvalPostulante==null?"":item.EvalPostulante,
                                item.PtoTotal==null?"":item.PtoTotal.ToString()
                                
                            }
                 }).ToArray();

                 return Json(generic.Value);

             }
             catch (Exception ex)
             {

                 return MensajeError();
             }
         }



    }
}
