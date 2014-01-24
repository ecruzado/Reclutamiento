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

        public ConocimientoGeneralPostulanteController(IConocimientoGeneralPostulanteRepository conocimientoGeneralPostulanteRepository, IDetalleGeneralRepository detalleGeneralRepository)
        {
            _conocimientoGeneralPostulanteRepository = conocimientoGeneralPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
        }

        public ActionResult Index()
        {
            return View();
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

                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.TipoConocimientoOfimatica,
                                item.TipoNombreOfimatica,
                                item.TipoNivelConocimiento,
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


                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.TipoIdioma,
                                item.TipoConocimientoIdioma,
                                item.TipoNivelConocimiento,
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


                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.TipoConocimientoGeneral,
                                item.TipoNombreConocimientoGeneral,
                                item.TipoNivelConocimiento,
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
                int ideConocimintoEdit = Convert.ToInt32(id);
                var conocimientoResultado = new ConocimientoGeneralPostulante();
                conocimientoResultado = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == ideConocimintoEdit);
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
            conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
            _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
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
                int ideConocimintoEdit = Convert.ToInt32(id);
                var conocimientoResultado = new ConocimientoGeneralPostulante();
                conocimientoResultado = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == ideConocimintoEdit);
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
                if (conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante != 0)
                {
                    _conocimientoGeneralPostulanteRepository.Update(conocimientoGeneralPostulante);
                    return Json(new { msj = true }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
                    _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
                    return Json(new { msj = true }, JsonRequestBehavior.DenyGet);
                }
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
            conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
            _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
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
            switch (tipoConocimiento)
            {
                case "01": //es software
                    listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCGSoftware));
                    break;
                case "02": // es primeros auxilios
                    listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCGPrimerosAux));
                    break;
                case "03": // es Contabilidad
                    listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCGContabilidad));
                    break;
            }
            result = Json(listaResultado);
            return result;
        }
        #endregion
    }
}

