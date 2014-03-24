
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
        public ActionResult Index(string id, string idRecluPost)
        {
            var modelEvaluaciones = inicializarEvaluacionesPreseleccionados();
            int idePostulante = Convert.ToInt32(id);
            int ideReclutamientoPersona = Convert.ToInt32(idRecluPost);

            var reclutaPersona = _reclutamientoPersonaRepository.GetSingle(x => x.IdeReclutaPersona == ideReclutamientoPersona);
            
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == reclutaPersona.IdePostulante);
            
            modelEvaluaciones.Solicitud.IdeSolReqPersonal = reclutaPersona.IdeSol;
            modelEvaluaciones.Solicitud.Tipsol = reclutaPersona.TipSol;
            modelEvaluaciones.PostulantePreSel = postulante;


            List<SolReqPersonal> listaSolicitud = _solReqPersonalRepository.GetDatosSol(modelEvaluaciones.Solicitud);

            if (listaSolicitud != null && listaSolicitud.Count > 0)
            {
                modelEvaluaciones.Solicitud = (SolReqPersonal)listaSolicitud[0];
            }

            return View("EvaluacionesPreSeleccionados", modelEvaluaciones);
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
        public ActionResult ListEvaluacionesPost(GridTable grid)
        {

            List<ReclutamientoPersonaExamen> lista = new List<ReclutamientoPersonaExamen>();
            try
            {

                var ideRecluPersona = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
                var idePostulante = (grid.rules[1].data == null ? 0 : Convert.ToInt32(grid.rules[1].data));

                lista = _reclutamientoPersonaExamenRepository.obtenerEvaluacionesPostulante(ideRecluPersona,idePostulante );

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = (item.IdeReclutamientoPersonaExamen.ToString()),
                    cell = new string[]
                            {
                                item.IdeReclutamientoPersonaExamen==0?"":item.IdeReclutamientoPersonaExamen.ToString(),
                                item.IdeReclutamientoPersona==0?"":item.IdeReclutamientoPersona.ToString(),
                                item.IdeEvaluacion==0?"":item.IdeEvaluacion.ToString(),
                                item.DescripcionExamen==null?"":item.DescripcionExamen,
                                item.TipoExamen==null?"":item.TipoExamen,
                                item.FechaEvaluacion==null?"":item.FechaEvaluacion.ToString(),
                                item.HoraEvaluacion==null?"":item.HoraEvaluacion.ToString(),
                                item.IdeRolResponsable==null?"":item.IdeRolResponsable.ToString(),
                                item.ResponsableDescripcion==null?"":item.ResponsableDescripcion,
                                item.TipoEstadoEvaluacion==null?"":item.TipoEstadoEvaluacion,
                                item.EstadoEvaluacion==null?"":item.EstadoEvaluacion,
                                item.NotaFinal==0?"":item.NotaFinal.ToString(),
                                item.ComentarioResultado==null?"":item.ComentarioResultado
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
