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
                                item.IdeCategoria==0?"":item.IdeCategoria.ToString()
                                
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

                //cargar en sesion los valores a usar
                Categoria categoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == idCategoria);
                modelExamen.Categoria = categoria;
                Numeracion = 0;
                HoraInicioEvaluacion = HoraInicio;
                TiempoEvaluacion = categoria.TIEMPO;
                cargarCriterio(Numeracion, modelExamen);
                modelExamen.Accion = Accion.Siguiente;

            }

            return (ActionResult)this.View("Examen",modelExamen);
        }


        //[OutputCache(Duration = 50, VaryByParam = "idCriterio")]
        [HttpPost]
        public ActionResult Examen(EvaluarPostulanteViewModel model)
        {
            var modelExamen = inicializarExamen();
            string usuario = Session[ConstanteSesion.UsuarioDes].ToString().Substring(0,15);
            if (model.Alternativa != null)
            {
                if (_reclutamientoPersonaAlternativaRepository.guardarRespuesta(IdeReclutaPersona, model.Criterio.IdeCriterioPorSubcategoria, model.Alternativa.IdeAlternativa, usuario))
                {
                    ListaCriterioEval.criterios[Numeracion].IndRespuesta = Indicador.Si;
                }
            }

            Numeracion = Numeracion + 1;
            if (Numeracion < ListaCriterioEval.criterios.Count())
            {
                cargarCriterio(Numeracion, modelExamen);
                modelExamen.Accion = Accion.Siguiente;
            }
            else
            {
                modelExamen.Accion = Accion.Close;
               // return JavaScript("close();");
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
            modelExamen.Fin = String.Format("{0:HH:mm:ss}", HoraInicioEvaluacion.AddMinutes(TiempoEvaluacion));

            modelExamen.Criterio = criterioSgt;
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

        public void calcularTime()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Thread.Sleep(500);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

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
