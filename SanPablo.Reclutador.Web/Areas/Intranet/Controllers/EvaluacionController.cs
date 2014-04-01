using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Repository.Interface;
using SanPablo.Reclutador.Web.Areas.Intranet.Models;
using SanPablo.Reclutador.Web.Core;
using SanPablo.Reclutador.Web.Models.JQGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    [Authorize]
    public class EvaluacionController : BaseController
    {
        private IReclutamientoPersonaCriterioRepository _reclutamientoPersonaCriterioRepository;
        private IReclutamientoPersonaAlternativaRepository _reclutamientoPersonaAlternativaRepository;
        private IExamenPorCategoriaRepository _examenPorCategoriaRepository;
        private ICategoriaRepository _categoriaRepository;
        private ICriterioRepository _criterioRepository;
        private IAlternativaRepository _alternativaRepository;
        private IReclutamientoPersonaRepository _reclutamientoPersonaRepository;
        private IUsuarioRepository _usuarioRepository;

        public EvaluacionController(IReclutamientoPersonaCriterioRepository reclutamientoPersonaCriterioRepository,
                                    IReclutamientoPersonaAlternativaRepository reclutamientoPersonaAlternativaRepository,
                                    IExamenPorCategoriaRepository examenPorCategoriaRepository,
                                    ICategoriaRepository categoriaRepository,
                                    ICriterioRepository criterioRepository,
                                    IAlternativaRepository alternativaRepository,
                                    IReclutamientoPersonaRepository reclutamientoPersonaRepository,
                                    IUsuarioRepository usuarioRepository)
        {
            _reclutamientoPersonaCriterioRepository = reclutamientoPersonaCriterioRepository;
            _reclutamientoPersonaAlternativaRepository = reclutamientoPersonaAlternativaRepository;
            _examenPorCategoriaRepository = examenPorCategoriaRepository;
            _categoriaRepository = categoriaRepository;
            _criterioRepository = criterioRepository;
            _alternativaRepository = alternativaRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _usuarioRepository = usuarioRepository;
        }

        [ValidarSesion]
        [AuthorizeUser]
        public ActionResult Index()
        {
            ReclutamientoPersona reclutamiento = new ReclutamientoPersona();
            int ideUsuario = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            var usuario = _usuarioRepository.GetSingle(x=>x.IdUsuario == ideUsuario);

            if(usuario.IdePostulante != null)
            {
                reclutamiento = _reclutamientoPersonaRepository.GetSingle(x => x.IdePostulante == usuario.IdePostulante && x.IdSede == Convert.ToInt32(Session[ConstanteSesion.Sede]) && 
                                                                         (x.EstPostulante == PostulanteEstado.PRESELECCIONADO_AUTOMATICO)||(x.EstPostulante==PostulanteEstado.PRESELECCIONADO_MANUAL));

                IdeReclutaPersona = reclutamiento.IdeReclutaPersona;
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult ListaExamenesPendientes(GridTable grid)
        {
            ExamenPorCategoria examenesPost;
            List<ExamenPorCategoria> lista = new List<ExamenPorCategoria>();
            try
            {

                examenesPost = new ExamenPorCategoria();

                //int idPostulante = 57;
                //int idSolicitud = 106;
                //string tipoSolicitud = TipoSolicitud.Remplazo;
                int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);

                lista = _examenPorCategoriaRepository.ListarExamenesPorCategoria(IdeReclutaPersona,idSede);

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeExamenxCategoria.ToString(),
                    cell = new string[]
                            {
                               
                                item.IdeExamenxCategoria==0?"":item.IdeExamenxCategoria.ToString(),
                                item.Examen.IdeExamen==0?"":item.Examen.IdeExamen.ToString(),
                                item.Categoria.IDECATEGORIA==0?"":item.Categoria.IDECATEGORIA.ToString(),
                                item.Examen.NomExamen==null?"":item.Examen.NomExamen,
                                //item.Examen.DescExamen==null?"":item.Examen.DescExamen,
                                //item.Examen.TipExamen==null?"":item.Examen.TipExamen,
                                //item.Examen.TipExamenDes==null?"":item.Examen.TipExamenDes,
                                //item.Categoria.ORDENIMPRESION.ToString(),
                                item.Categoria.NOMCATEGORIA==null?"":item.Categoria.NOMCATEGORIA,
                                //item.Categoria.DESCCATEGORIA==null?"":item.Categoria.DESCCATEGORIA,
                                //item.Categoria.TIPCATEGORIA==null?"":item.Categoria.TIPCATEGORIA,
                                //item.Categoria.INSTRUCCIONES==null?"":item.Categoria.INSTRUCCIONES,
                                //item.Categoria.TIPOEJEMPLO==null?"":item.Categoria.TIPOEJEMPLO,
                                item.Categoria.TIEMPO.ToString(),
                                "1",
                                item.Categoria.IDECATEGORIA==0?"":item.Categoria.IDECATEGORIA.ToString()
                                
                            }
                }).ToArray();

                return Json(generic.Value);

            }
            catch (Exception ex)
            {

                return MensajeError();
            }



            #region lista
            //    List<object> list = new List<object>();
        //    var fAnonymousType2_1 = new
        //    {
        //        id = 1,
        //        cell = new string[5]
        //{
        //  "200001",
        //  "Proactividad",
        //  "10 minutos",
        //  "Evaluado",
        //  "0"
        //}
        //    };
        //    list.Add((object)fAnonymousType2_1);
        //    var fAnonymousType2_2 = new
        //    {
        //        id = 2,
        //        cell = new string[5]
        //{
        //  "200001",
        //  "Comunicación Interpersonal",
        //  "30 minutos",
        //  "Pendiente",
        //  "1"
        //}
        //    };
        //    list.Add((object)fAnonymousType2_2);
        //    var fAnonymousType2_3 = new
        //    {
        //        id = 3,
        //        cell = new string[5]
        //{
        //  "200001",
        //  "Cultura General",
        //  "20 minutos",
        //  "Pendiente",
        //  "2"
        //}
        //    };
        //    list.Add((object)fAnonymousType2_3);
        //    var fAnonymousType3 = new
        //    {
        //        rows = list
        //    };
            //    return (ActionResult)this.Json((object)fAnonymousType3);
            #endregion
        }

        [ValidarSesion]
        public ActionResult Instrucciones(string id)
        {
            var  modelEvaluacion = inicializarExamen();
            int idCategoria = Convert.ToInt32(id);
            if (idCategoria != 0)
            {
                var categoria = _categoriaRepository.GetSingle(x=>x.IDECATEGORIA == idCategoria);
                string Instrucciones = categoria.INSTRUCCIONES;
                Instrucciones = Instrucciones.Replace(".", "." + Environment.NewLine);

                modelEvaluacion.Categoria = categoria;

                     
            }
            return View("Instrucciones", modelEvaluacion);
        }


        public EvaluarPostulanteViewModel inicializarExamen()
        {
            EvaluarPostulanteViewModel model = new EvaluarPostulanteViewModel();
            model.Categoria = new Categoria();
            model.SubCategoria = new SubCategoria();
            model.Criterio = new Criterio();
            model.Alternativa = new Alternativa();
            model.Alternativas = new List<Alternativa>();

            return model;
        }

        #region Evaluacion Postulante
        public void recuperarPregunta(int idCriterio)
        {

        }
        #endregion




        public ActionResult InstruccionesExamen()
        {
            return (ActionResult)this.View("InstruccionesExamen");
        }

        [ValidarSesion]
        public ActionResult Examen(string id)
        {
            
            var modelExamen = inicializarExamenCategoria();
            int idCategoria = Convert.ToInt32(id);
            ListaCriterioEval = null;
            if (idCategoria != 0)
            {
                ListaCriterioEval = _criterioRepository.ObtenerCriteriosPorCategoria(idCategoria);

                var criterio = ListaCriterioEval.criterios[0];
                modelExamen.SubCategoria.NOMSUBCATEGORIA = criterio.NombreSubCategoria;
                modelExamen.SubCategoria.IDESUBCATEGORIA = criterio.IdeSubCategoria;
                modelExamen.Criterio = criterio;
                modelExamen.nroPregunta = criterio.numeracion;
                modelExamen.totalPreguntas = ListaCriterioEval.criterios.Count();
                modelExamen.Alternativas = new List<Alternativa>(_alternativaRepository.GetBy(x => x.Criterio.IdeCriterio == criterio.IdeCriterio && x.ESTACTIVO == IndicadorActivo.Activo));

                //calcular la hora de inicio
                Categoria categoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == idCategoria);
                modelExamen.Categoria = categoria;

                HoraInicioEvaluacion = HoraInicio;
                TiempoEvaluacion = categoria.TIEMPO;
                modelExamen.Inicio = String.Format("{0:HH:mm:ss}", HoraInicioEvaluacion);
                modelExamen.Fin = String.Format("{0:HH:mm:ss}", HoraInicio.AddMinutes(TiempoEvaluacion));

                
            }

            return (ActionResult)this.View("Examen",modelExamen);
        }


        //[OutputCache(Duration = 50, VaryByParam = "idCriterio")]
        [HttpPost]
        public ActionResult Examen(EvaluarPostulanteViewModel model)
        {
            var modelExamen = inicializarExamen();

            if (_reclutamientoPersonaAlternativaRepository.guardarRespuesta(IdeReclutaPersona, model.Criterio.IdeCriterioPorSubcategoria, model.Alternativa.IdeAlternativa, Session[ConstanteSesion.UsuarioDes].ToString()))
            {
                ListaCriterioEval.criterios[model.Criterio.numeracion - 1].IndRespuesta = Indicador.Si;
            }
            else
            {
                ListaCriterioEval.criterios[model.Criterio.numeracion - 1].IndRespuesta = Indicador.No;
            }
            

            Criterio criterioSgt = ListaCriterioEval.criterios[Convert.ToInt32(model.Criterio.numeracion)];
            modelExamen.SubCategoria.NOMSUBCATEGORIA = criterioSgt.NombreSubCategoria;
            modelExamen.SubCategoria.IDESUBCATEGORIA = criterioSgt.IdeSubCategoria;
            modelExamen.Criterio = null;
            //modelExamen.Criterio = criterioSgt;
            //modelExamen.nroPregunta = criterioSgt.numeracion;
            modelExamen.Alternativas = new List<Alternativa>(_alternativaRepository.GetBy(x => x.Criterio.IdeCriterio == criterioSgt.IdeCriterio && x.ESTACTIVO == IndicadorActivo.Activo));
            modelExamen.totalPreguntas = ListaCriterioEval.criterios.Count();
            modelExamen.Categoria.TIEMPO = TiempoEvaluacion;
            modelExamen.Inicio = String.Format("{0:HH:mm:ss}", HoraInicioEvaluacion);
            modelExamen.Fin = String.Format("{0:HH:mm:ss}", HoraInicioEvaluacion.AddMinutes(TiempoEvaluacion));

            var criterio = model.Criterio;

            modelExamen.Criterio.numeracion = criterioSgt.numeracion;
            return View("Examen", modelExamen);
        }

        public EvaluarPostulanteViewModel inicializarExamenCategoria()
        {
            EvaluarPostulanteViewModel model = new EvaluarPostulanteViewModel();
            model.Categoria = new Categoria();
            model.SubCategoria = new SubCategoria();
            model.Criterio = new Criterio();
            model.Alternativas = new List<Alternativa>();

            return model;
        }

        public ActionResult GetImage(int id)
        {
            var firstOrDefault = _categoriaRepository.GetSingle(c => c.IDECATEGORIA == id);
            if (firstOrDefault.IMAGENEJEMPLO != null)
            {
                byte[] image = firstOrDefault.IMAGENEJEMPLO;
                return File(image, "image/jpg");
            }
            else
            {
                return null;
            }
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
