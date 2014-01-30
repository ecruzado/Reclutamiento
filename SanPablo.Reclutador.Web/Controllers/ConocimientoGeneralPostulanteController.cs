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
   
    public class ConocimientoGeneralPostulanteController : BaseController
    {
        private IConocimientoGeneralPostulanteRepository _conocimientoGeneralPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IPostulanteRepository _postulanteRepository;

        public ConocimientoGeneralPostulanteController(IConocimientoGeneralPostulanteRepository conocimientoGeneralPostulanteRepository, 
                                                       IDetalleGeneralRepository detalleGeneralRepository,
                                                       IPostulanteRepository postulanteRepository)
        {
            _conocimientoGeneralPostulanteRepository = conocimientoGeneralPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _postulanteRepository = postulanteRepository;
        }

        public ActionResult Index()
        {
            var conocimientoViewModel = InicializarConocimiento();
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            conocimientoViewModel.ConocimientoGeneral.Postulante = postulante;
            return View(conocimientoViewModel);
        }

        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;
                
                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralPostulante>();
                where.Add(Expression.IsNotNull("TipoConocimientoOfimatica"));
                where.Add(Expression.Eq("Postulante.IdePostulante",IdePostulante));

                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.DescripcionConocimientoOfimatica,
                                item.DescripcionNombreOfimatica,
                                item.DescripcionNivelConocimiento,
                                item.Certificacion.ToString()
                                
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        [HttpPost]
        public virtual JsonResult ListarIdiomas(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralPostulante>();
                where.Add(Expression.IsNotNull("TipoIdioma"));
                where.Add(Expression.Eq("Postulante.IdePostulante", IdePostulante));

                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.DescripcionIdioma,
                                item.DescripcionConocimientoIdioma,
                                item.DescripcionNivelConocimiento,
                                item.Certificacion.ToString()
                                
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        [HttpPost]
        public virtual JsonResult ListarOtrosConocimientos(GridTable grid) 
        {
            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralPostulante>();
                where.Add(Expression.IsNotNull("TipoConocimientoGeneral"));
                where.Add(Expression.Eq("Postulante.IdePostulante", IdePostulante));

                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.DescripcionConocimientoGeneral,
                                item.DescripcionNombreConocimientoGeneral,
                                item.DescripcionNivelConocimiento,
                                item.Certificacion.ToString()
                                
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        public ViewResult Ofimatica(string id)
        {
            var conocimientoGeneralViewModel = InicializarConocimiento();
            if (id == "0")
            {
                return View(conocimientoGeneralViewModel);
            }
            else
            {
                var conocimientoResultado = new ConocimientoGeneralPostulante();
                conocimientoResultado = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == Convert.ToInt32(id));
                conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoResultado;
                return View(conocimientoGeneralViewModel);
            }
        }


        [HttpPost]
        public JsonResult Ofimatica([Bind(Prefix = "ConocimientoGeneral")]ConocimientoGeneralPostulante conocimientoGeneralPostulante)
        {


            if (!ModelState.IsValid)
            {
                return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
            }
            if (conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante == 0)
            {
                conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
                var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                postulante.agregarConocimiento(conocimientoGeneralPostulante);
                _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
            }
            else
            {
                var conocimientoEdit = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante);
                conocimientoEdit.TipoNombreOfimatica = conocimientoGeneralPostulante.TipoNombreOfimatica;
                conocimientoEdit.TipoNivelConocimiento = conocimientoGeneralPostulante.TipoNivelConocimiento;
                conocimientoEdit.TipoConocimientoOfimatica = conocimientoGeneralPostulante.TipoConocimientoOfimatica;
                conocimientoEdit.IndicadorCertificacion = conocimientoGeneralPostulante.IndicadorCertificacion;
                _conocimientoGeneralPostulanteRepository.Update(conocimientoEdit);
            }
            return Json(new { msj = true }, JsonRequestBehavior.DenyGet);

        }

        public ConocimientoPostulanteGeneralViewModel InicializarConocimiento()
        {
            var conocimientoPostulanteGeneralViewModel = new ConocimientoPostulanteGeneralViewModel();
            conocimientoPostulanteGeneralViewModel.ConocimientoGeneral = new ConocimientoGeneralPostulante();

            conocimientoPostulanteGeneralViewModel.TiposConocimientoOfimatica = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoOfimatica));
            conocimientoPostulanteGeneralViewModel.TiposConocimientoOfimatica.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            
            conocimientoPostulanteGeneralViewModel.TipoNombresOfimatica = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TiponombreOfimatica));
            conocimientoPostulanteGeneralViewModel.TipoNombresOfimatica.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return conocimientoPostulanteGeneralViewModel;
        }


        [HttpPost]
        public ActionResult eliminarConocimiento(int ideConocimiento)
        {
            ActionResult result = null;

            var conocimientoEliminar = new ConocimientoGeneralPostulante();
            conocimientoEliminar = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == ideConocimiento);
            int antes = _conocimientoGeneralPostulanteRepository.CountBy();
            _conocimientoGeneralPostulanteRepository.Remove(conocimientoEliminar);
            int despues = _conocimientoGeneralPostulanteRepository.CountBy();

            return result;
        }

        #region IDIOMAS

        public ViewResult Idiomas(string id)
        {
            var conocimientoGeneralViewModel = InicializarIdiomas();
            if (id == "0")
            {
                return View(conocimientoGeneralViewModel);
            }
            else
            {
                var conocimientoResultado = new ConocimientoGeneralPostulante();
                conocimientoResultado = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == Convert.ToInt32(id));
                conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoResultado;
                return View(conocimientoGeneralViewModel);
            }
        }


        [HttpPost]
        public JsonResult Idiomas([Bind(Prefix = "ConocimientoGeneral")]ConocimientoGeneralPostulante conocimientoGeneralPostulante)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                if (conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante == 0)
                {
                    conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
                    var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                    postulante.agregarConocimiento(conocimientoGeneralPostulante);
                    _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
                   
                }
                else
                {
                    var conocimientoEdit = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante);
                    conocimientoEdit.TipoNivelConocimiento = conocimientoGeneralPostulante.TipoNivelConocimiento;
                    conocimientoEdit.TipoIdioma = conocimientoGeneralPostulante.TipoIdioma;
                    conocimientoEdit.TipoConocimientoIdioma = conocimientoGeneralPostulante.TipoConocimientoIdioma;
                    conocimientoEdit.IndicadorCertificacion = conocimientoGeneralPostulante.IndicadorCertificacion;

                    _conocimientoGeneralPostulanteRepository.Update(conocimientoEdit);
                }
                return Json(new { msj = true }, JsonRequestBehavior.DenyGet);
            }

        }


        public ConocimientoPostulanteGeneralViewModel InicializarIdiomas()
        {
            var conocimientoPostulanteGeneralViewModel = new ConocimientoPostulanteGeneralViewModel();
            conocimientoPostulanteGeneralViewModel.ConocimientoGeneral = new ConocimientoGeneralPostulante();

            conocimientoPostulanteGeneralViewModel.TipoIdiomas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoIdioma));
            conocimientoPostulanteGeneralViewModel.TipoIdiomas.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            conocimientoPostulanteGeneralViewModel.TipoConocimientoIdiomas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoIdioma));
            conocimientoPostulanteGeneralViewModel.TipoConocimientoIdiomas.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return conocimientoPostulanteGeneralViewModel;
        }
        #endregion


        #region OTROCONOCIMIENTO

        public ViewResult OtroConocimiento(string id)
        {
            var conocimientoGeneralViewModel = InicializarOtroConocimiento();
            if (id == "0")
            {
                return View(conocimientoGeneralViewModel);
            }
            else
            {
                int ideConocimientoEdit = Convert.ToInt32(id);
                var conocimientoResultado = new ConocimientoGeneralPostulante();
                conocimientoResultado = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == ideConocimientoEdit);
                conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoResultado;
                actualizarOtrosConocimientos(conocimientoGeneralViewModel);
                return View(conocimientoGeneralViewModel);
            }
        }


        [HttpPost]
        public JsonResult OtroConocimiento([Bind(Prefix = "ConocimientoGeneral")]ConocimientoGeneralPostulante conocimientoGeneralPostulante)
        {


            if (!ModelState.IsValid)
            {
                return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
            }
            if (conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante == 0)
            {
                conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
                var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                postulante.agregarConocimiento(conocimientoGeneralPostulante);
                conocimientoGeneralPostulante.Postulante.IndicadorRegistroCompleto = "D";
                _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
            }
            else
            {
                var conocimientoEdit = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante);
                conocimientoEdit.TipoNombreConocimientoGeneral = conocimientoGeneralPostulante.TipoNombreConocimientoGeneral;
                conocimientoEdit.TipoNivelConocimiento = conocimientoGeneralPostulante.TipoNivelConocimiento;
                conocimientoEdit.TipoConocimientoGeneral = conocimientoGeneralPostulante.TipoConocimientoGeneral;
                conocimientoEdit.NombreConocimientoGeneral = conocimientoGeneralPostulante.NombreConocimientoGeneral;
                conocimientoEdit.IndicadorCertificacion = conocimientoGeneralPostulante.IndicadorCertificacion;

                _conocimientoGeneralPostulanteRepository.Update(conocimientoEdit);
            }
            return Json(new { msj = true }, JsonRequestBehavior.DenyGet);

        }

        public ConocimientoPostulanteGeneralViewModel InicializarOtroConocimiento()
        {
            var conocimientoPostulanteGeneralViewModel = new ConocimientoPostulanteGeneralViewModel();
            conocimientoPostulanteGeneralViewModel.ConocimientoGeneral = new ConocimientoGeneralPostulante();

            conocimientoPostulanteGeneralViewModel.TipoConocimientoGenerales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoGral));
            conocimientoPostulanteGeneralViewModel.TipoConocimientoGenerales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            conocimientoPostulanteGeneralViewModel.TipoNombresConocimientosGrales = new List<DetalleGeneral>();
            conocimientoPostulanteGeneralViewModel.TipoNombresConocimientosGrales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return conocimientoPostulanteGeneralViewModel;
        }

        public ActionResult listarNombreConocimiento(string tipoConocimiento) 
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();
            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoConocimientoGral, tipoConocimiento));
            result = Json(listaResultado);
            return result;
        }
        public void actualizarOtrosConocimientos(ConocimientoPostulanteGeneralViewModel conocimientoModel)
        {
            var listaResultado = new List<DetalleGeneral>();
            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoConocimientoGral, conocimientoModel.ConocimientoGeneral.TipoConocimientoGeneral.ToString()));
            conocimientoModel.TipoNombresConocimientosGrales = listaResultado;

        }
        #endregion
    }
}

