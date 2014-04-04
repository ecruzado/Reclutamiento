using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Repository.Interface;
using SanPablo.Reclutador.Web.Areas.Intranet.Models;
using SanPablo.Reclutador.Web.Core;
using SanPablo.Reclutador.Web.Models.JQGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        private IReclutamientoPersonaExamenRepository _reclutamientoPersonaExamenRepository;
        private IReclutamientoPersonaExamenCategoriaRepository _reclutamientoExamenCategoriaRepository;

        public EvaluacionController(IReclutamientoPersonaCriterioRepository reclutamientoPersonaCriterioRepository,
                                    IReclutamientoPersonaAlternativaRepository reclutamientoPersonaAlternativaRepository,
                                    IExamenPorCategoriaRepository examenPorCategoriaRepository,
                                    ICategoriaRepository categoriaRepository,
                                    ICriterioRepository criterioRepository,
                                    IAlternativaRepository alternativaRepository,
                                    IReclutamientoPersonaRepository reclutamientoPersonaRepository,
                                    IUsuarioRepository usuarioRepository,
                                    IReclutamientoPersonaExamenRepository reclutamientoPersonaExamenRepository,
                                    IReclutamientoPersonaExamenCategoriaRepository reclutamientoExamenCategoriaRepository)
        {
            _reclutamientoPersonaCriterioRepository = reclutamientoPersonaCriterioRepository;
            _reclutamientoPersonaAlternativaRepository = reclutamientoPersonaAlternativaRepository;
            _examenPorCategoriaRepository = examenPorCategoriaRepository;
            _categoriaRepository = categoriaRepository;
            _criterioRepository = criterioRepository;
            _alternativaRepository = alternativaRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _usuarioRepository = usuarioRepository;
            _reclutamientoPersonaExamenRepository = reclutamientoPersonaExamenRepository;
            _reclutamientoExamenCategoriaRepository = reclutamientoExamenCategoriaRepository;
        }

        [ValidarSesion]
        [AuthorizeUser]
        public ActionResult Index()
        {
           
            int ideUsuario = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            var usuario = _usuarioRepository.GetSingle(x=>x.IdUsuario == ideUsuario);
            IdeReclutaPersona = 0;
            if(usuario.IdePostulante != 0)
            {
                int ideReclutamientoPersona = _reclutamientoPersonaRepository.getIdeReclutaPersona(usuario.IdePostulante, Convert.ToInt32(Session[ConstanteSesion.Sede]));
                IdeReclutaPersona = ideReclutamientoPersona;
                if (IdeReclutaPersona != 0)
                {
                    _reclutamientoPersonaExamenRepository.obtenerEvaluacionesPostulante(usuario.IdePostulante, ideReclutamientoPersona, usuario.CodUsuario.Substring(0,15));
                    
                    //generar los examenes por categorias
                    int contador = _reclutamientoExamenCategoriaRepository.CountByExpress(x => x.IdeReclutaPersona == IdeReclutaPersona);
                    if (contador == 0)
                    {
                        _reclutamientoExamenCategoriaRepository.obtenerExamenesPorCategoria(IdeReclutaPersona, Session[ConstanteSesion.UsuarioDes].ToString().Substring(0, 15));
                    }
                   
                }
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult ListaExamenesPendientes(GridTable grid)
        {
            DatosExamenPorCategoria examenesPost;
            List<DatosExamenPorCategoria> lista = new List<DatosExamenPorCategoria>();
            try
            {
                if (IdeReclutaPersona != 0)
                {

                    examenesPost = new DatosExamenPorCategoria();

                    int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);

                    lista = _reclutamientoExamenCategoriaRepository.ListarExamenesPorCategoria(IdeReclutaPersona);

                    var generic = GetListar(lista,
                                             grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                    generic.Value.rows = generic.List.Select(item => new Row
                    {
                        id = item.IdeReclutamientoPersonaExamenCategoria.ToString(),
                        cell = new string[]
                            {
                                item.IdeReclutamientoPersonaExamenCategoria ==0?"":item.IdeReclutamientoPersonaExamenCategoria.ToString(),
                                item.IdeExamenCategoria==0?"":item.IdeExamenCategoria.ToString(),
                                item.IdeExamen==0?"":item.IdeExamen.ToString(),
                                item.IdeCategoria==0?"":item.IdeCategoria.ToString(),
                                item.NombreExamen==null?"":item.NombreExamen,
                                item.NombreCategoria==null?"":item.NombreCategoria,
                                item.Tiempo.ToString(),
                                item.Estado == null?"0":item.Estado,
                                item.IdeReclutamientoPersonaExamenCategoria==0?"":item.IdeReclutamientoPersonaExamenCategoria.ToString()
                                
                            }
                    }).ToArray();
                    return Json(generic.Value);
                }
                else 
                {
                    return Json(false);
                }

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
            int idReclutamientoExamenCategoria = Convert.ToInt32(id);
            IdeReclutamientoExamenCategoria = idReclutamientoExamenCategoria;
            int idCategoria = _reclutamientoExamenCategoriaRepository.obtenerIdentificadorCategoria(idReclutamientoExamenCategoria);
            if (idCategoria != 0)
            {
                var categoria = _categoriaRepository.GetSingle(x=>x.IDECATEGORIA == idCategoria);
                string Instrucciones = categoria.INSTRUCCIONES;
                if (Instrucciones != null)
                {
                    Instrucciones = Instrucciones.Replace(".", ". <br /> " /*+ Environment.NewLine*/);
                    categoria.INSTRUCCIONES = Instrucciones;
                }
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

            model.Accion = Accion.Terminar;
            return model;
        }

        //public ActionResult InstruccionesExamen()
        //{
        //    return (ActionResult)this.View("InstruccionesExamen");
        //}

        [ValidarSesion]
        public ActionResult Examen(string id)
        {
            
            var modelExamen = inicializarExamenCategoria();
            int idCategoria = Convert.ToInt32(id);
            ListaCriterioEval = null;
            if (idCategoria != 0)
            {
                ListaCriterioEval = _criterioRepository.ObtenerCriteriosPorCategoria(idCategoria);

                Categoria categoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == idCategoria);
                modelExamen.Categoria = categoria;
                modelExamen.nroPregunta = 1;
                HoraInicioEvaluacion = HoraInicio;
                TiempoEvaluacion = categoria.TIEMPO;
                cargarCriterio(0, modelExamen);
                modelExamen.Accion = Accion.Siguiente;

                var reclutamientoCategoria = _reclutamientoExamenCategoriaRepository.GetSingle(x => x.IdeReclutamientoPersonaExamenCategoria == IdeReclutamientoExamenCategoria);
                reclutamientoCategoria.Estado = EstadoCategoria.Evaluado;
                _reclutamientoExamenCategoriaRepository.Update(reclutamientoCategoria);

            }

            return (ActionResult)this.View("Examen",modelExamen);
        }


        [HttpPost]
        public ActionResult Examen(EvaluarPostulanteViewModel model)
        {

            var modelExamen = inicializarExamen();
            guardarRespuesta(model);
            
            modelExamen.nroPregunta = model.nroPregunta;

            for (int i = model.nroPregunta; i < ListaCriterioEval.criterios.Count; i++)
            {
                if (ListaCriterioEval.criterios[model.nroPregunta].IndRespuesta == Indicador.No)
                {
                        ModelState.Clear();
                        cargarCriterio(modelExamen.nroPregunta, modelExamen);
                        modelExamen.Accion = Accion.Siguiente;
                        break;
                }
            }
            if (modelExamen.nroPregunta == ListaCriterioEval.criterios.Count)
            {
                modelExamen.Accion = Accion.Terminar;
            }

            
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

        public void cargarCriterio(int numeracion, EvaluarPostulanteViewModel modelExamen)
        {
            Criterio criterioSgt = ListaCriterioEval.criterios[Convert.ToInt32(numeracion)];

            modelExamen.SubCategoria.NOMSUBCATEGORIA = criterioSgt.NombreSubCategoria;
            modelExamen.SubCategoria.IDESUBCATEGORIA = criterioSgt.IdeSubCategoria;
            
            modelExamen.Alternativas = new List<Alternativa>(_alternativaRepository.GetBy(x => x.Criterio.IdeCriterio == criterioSgt.IdeCriterio && x.ESTACTIVO == IndicadorActivo.Activo));
            
            modelExamen.totalPreguntas = ListaCriterioEval.criterios.Count();

            modelExamen.nroPregunta = numeracion + 1;

            modelExamen.Categoria.TIEMPO = TiempoEvaluacion;
            modelExamen.Inicio = String.Format("{0:HH:mm:ss}", HoraInicioEvaluacion);
            modelExamen.Fin = String.Format("{0:HH:mm:ss F}", HoraInicioEvaluacion.AddMinutes(TiempoEvaluacion));

            TimeSpan tiempoRestante = (HoraInicioEvaluacion.AddMinutes(TiempoEvaluacion) - DateTime.Now);
           
            modelExamen.segundos = Convert.ToInt32(tiempoRestante.TotalSeconds);

            modelExamen.HoraFin = HoraInicioEvaluacion.AddMinutes(TiempoEvaluacion);

            modelExamen.Criterio = criterioSgt;
        }

        [ValidarSesion]
        public ActionResult MostrarPregunta(int nroPregunta)
        {
            var modelExamen = inicializarExamen();
            ModelState.Clear();
            cargarCriterio(nroPregunta, modelExamen);
            
            modelExamen.Accion = Accion.Siguiente;
            return View("Examen", modelExamen);
        }


        [HttpPost]
        public ActionResult guardarPregunta(EvaluarPostulanteViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            objJson.Resultado = false;
            guardarRespuesta(model);

            int nroPreg = Convert.ToInt32(model.nroPregunta);
            if ((nroPreg != 0)&& (nroPreg <= ListaCriterioEval.criterios.Count))
            {
                if (ListaCriterioEval.criterios[nroPreg - 1].IndRespuesta == Indicador.No)
                {
                    objJson.Resultado = true;
                }
            }
                        
            objJson.Mensaje = "La pregunta ya fue contestada";
            return Json(objJson);
        }


        [HttpPost]
        public ActionResult preguntaAnterior(EvaluarPostulanteViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            objJson.Resultado = false;
            guardarRespuesta(model);
            // pregunta actual
            int nroPregAnterior = -1;
            int nroPregActual = -1;
            for (int i = 0; i <= ListaCriterioEval.criterios.Count -1; i++)
            {
                if (ListaCriterioEval.criterios[i].IdeCriterio == model.Criterio.IdeCriterio)
                {
                    nroPregActual = i;
                    break;
                }
                if (ListaCriterioEval.criterios[i].IndRespuesta == Indicador.No)
                {
                    nroPregAnterior = i;
                }

            }

            if ((nroPregAnterior == nroPregActual)||(nroPregAnterior == -1))
            {
                objJson.Mensaje = "No existe pregunta anterior no contestada";
                objJson.Resultado = false;
            }
            else
            {
                objJson.IdDato = nroPregAnterior;
                objJson.Resultado = true;
            }
            return Json(objJson);
        }

        [HttpPost]
        public ActionResult terminarPrueba(EvaluarPostulanteViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            objJson.Resultado = false;
            try
            {
                guardarRespuesta(model);

                //var reclutamientoCategoria = _reclutamientoExamenCategoriaRepository.GetSingle(x => x.IdeReclutamientoPersonaExamenCategoria == IdeReclutamientoExamenCategoria);
                //reclutamientoCategoria.Estado = EstadoCategoria.Evaluado;
                //_reclutamientoExamenCategoriaRepository.Update(reclutamientoCategoria);
                objJson.Resultado = true;
                return Json(objJson);
            }
            catch (Exception ex)
            {
                objJson.Mensaje = "ERROR: " + ex;
                objJson.Resultado = false;
                return Json(objJson);
            }
        }

        public void guardarRespuesta(EvaluarPostulanteViewModel model)
        {
            if (ListaCriterioEval.criterios[model.nroPregunta - 1].IndRespuesta == Indicador.No)
            {
                string usuario = Session[ConstanteSesion.UsuarioDes].ToString().Substring(0, 15);

                if (model.Alternativa != null)
                {
                    if (_reclutamientoPersonaAlternativaRepository.guardarRespuesta(IdeReclutaPersona, model.Criterio.IdeCriterioPorSubcategoria, model.Alternativa.IdeAlternativa, usuario))
                    {
                        ListaCriterioEval.criterios[model.nroPregunta - 1].IndRespuesta = Indicador.Si;
                    }
                }
            }
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


        public ActionResult PartialExamenCronometro()
        {
            return View();
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
