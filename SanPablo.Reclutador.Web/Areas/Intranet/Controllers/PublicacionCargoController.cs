namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;

    [Authorize]
    public class PublicacionCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IAreaRepository _areaRepository;
        private INivelAcademicoCargoRepository _nivelAcademicoCargoRepository;
        private ICompetenciaCargoRepository _competenciaCargoRepository;
        private IOfrecemosCargoRepository _ofrecemosCargoRepository;
        private IConocimientoGeneralCargoRepository _conocimientoCargoRepository;
        private IExperienciaCargoRepository _experienciaCargoRepository;
        private ICargoRepository _cargoRepository;
        private ILogSolicitudNuevoCargoRepository _logSolicitudNuevoCargoRepository;


        public PublicacionCargoController(ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                             IDetalleGeneralRepository detalleGeneralRepository,
                                             IAreaRepository areaRepository,
                                             INivelAcademicoCargoRepository nivelAcademicoCargoRepository,
                                             ICompetenciaCargoRepository competenciaCargoRepository,
                                             IOfrecemosCargoRepository ofrecemosCargoRepository,
                                             IConocimientoGeneralCargoRepository conocimientoCargoRepository,
                                             IExperienciaCargoRepository experienciaCargoRepository,
                                             ICargoRepository cargoRespository,
                                             ILogSolicitudNuevoCargoRepository logSolicitudNuevoCargoRepository)
        {
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _areaRepository = areaRepository;
            _nivelAcademicoCargoRepository = nivelAcademicoCargoRepository;
            _competenciaCargoRepository = competenciaCargoRepository;
            _ofrecemosCargoRepository = ofrecemosCargoRepository;
            _conocimientoCargoRepository = conocimientoCargoRepository;
            _experienciaCargoRepository = experienciaCargoRepository;
            _cargoRepository = cargoRespository;
            _logSolicitudNuevoCargoRepository = logSolicitudNuevoCargoRepository;
        }


        [HttpPost]
        public virtual JsonResult Estudios(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<NivelAcademicoCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_nivelAcademicoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionAreaEstudio,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult Conocimientos(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoGeneral,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult Experiencia(GridTable grid)
        {
            int Idecargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ExperienciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", Idecargo));

                var generic = Listar(_experienciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult Funciones(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<SolicitudNuevoCargo>();
                //where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_solicitudNuevoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeSolicitudNuevoCargo.ToString(),
                        cell = new string[]
                            {
                                item.EstadoActivo,
                                item.EstadoActivo,
                                item.CodigoCargo,
                                item.NombreCargo,
                                item.IdeArea.ToString(),
                                item.IdeArea.ToString(),
                                item.IdeArea.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.FechaCreacion.ToString(),
                                item.FechaExpiracion.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult Competencias(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CompetenciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_competenciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCompetenciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionCompetencia,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult Ofrecemos(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<OfrecemosCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_ofrecemosCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeOfrecemosCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionOfrecimiento,
                                
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }


        [AuthorizeUser]
        public ActionResult Edit()
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var Cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var publicacionViewModel = inicializarPublicacion();
            if (Cargo != null)
            {
                publicacionViewModel.SolicitudCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.CodigoCargo == Cargo.CodigoCargo);
                
            }
            return View(publicacionViewModel);
        }

        public PublicacionViewModel inicializarPublicacion()
        {
            var publicacionNuevoViewModel = new PublicacionViewModel();

            publicacionNuevoViewModel.Cargo = new Cargo();
            publicacionNuevoViewModel.SolicitudCargo = new SolicitudNuevoCargo();

            return publicacionNuevoViewModel;
        }


        //[HttpPost]
        //public ActionResult Edit([Bind(Prefix = "SolicitudNuevoCargo")]SolicitudNuevoCargo nuevaSolicitudCargo)
        //{
        //    var enviarMail = new SendMail();
        //    //int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
        //    var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
        //    JsonMessage objJsonMessage = new JsonMessage();
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            var nuevoCargoViewModel = inicializarSolicitudNuevoCargo();
        //            nuevoCargoViewModel.SolicitudNuevoCargo = nuevaSolicitudCargo;
        //            return View(nuevoCargoViewModel);
        //        }
           
        //        objJsonMessage.Mensaje = "Agregado Correctamente";
        //        objJsonMessage.Resultado = true;
        //        return Json(objJsonMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        objJsonMessage.Mensaje = "ERROR:" + ex.Message;
        //        objJsonMessage.Resultado = false;
        //        return Json(objJsonMessage);
        //    }

        //}
       

       
    }
}
