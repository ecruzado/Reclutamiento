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
    using SanPablo.Reclutador.Entity.Validation;

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
        private IHorarioCargoRepository _horarioCargoRepository;


        public PublicacionCargoController(ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                             IDetalleGeneralRepository detalleGeneralRepository,
                                             IAreaRepository areaRepository,
                                             INivelAcademicoCargoRepository nivelAcademicoCargoRepository,
                                             ICompetenciaCargoRepository competenciaCargoRepository,
                                             IOfrecemosCargoRepository ofrecemosCargoRepository,
                                             IConocimientoGeneralCargoRepository conocimientoCargoRepository,
                                             IExperienciaCargoRepository experienciaCargoRepository,
                                             ICargoRepository cargoRespository,
                                             IHorarioCargoRepository horarioCargoRepository,   
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
            _horarioCargoRepository = horarioCargoRepository;
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
                DetachedCriteria where = null;
                where = DetachedCriteria.For<ExperienciaCargo>();

                //ProjectionList lista = Projections.ProjectionList();
                //lista.Add(Projections.Property("TipoExperiencia"), "TipoExperiencia");
               
                //where.SetProjection(Projections.Distinct(lista));
                //where.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<ExperienciaCargo>());


                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                //DetachedCriteria where = DetachedCriteria.For<ExperienciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", Idecargo));
                

                var generic = Listar(_experienciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString() + " AÑO(S)",
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

        [ValidarSesion]
        [AuthorizeUser]
        public ActionResult Edit()
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var Cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var publicacionViewModel = inicializarPublicacion(Cargo);
            if (Cargo != null)
            {
                publicacionViewModel.Cargo = Cargo;
                publicacionViewModel.SolicitudCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.CodigoCargo == Cargo.CodigoCargo);
                
            }
            return View(publicacionViewModel);
        }
        
       
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "SolicitudCargo")]SolicitudNuevoCargo solicitudNuevoCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var Cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var publicacionViewModel = inicializarPublicacion(Cargo);
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                SolicitudNuevoCargoValidator validation = new SolicitudNuevoCargoValidator();
                ValidationResult result = validation.Validate(solicitudNuevoCargo, "DescripcionObservaciones", "FechaPublicacion", "FechaExpiracion");
                if (!result.IsValid)
                {
                    publicacionViewModel.SolicitudCargo = solicitudNuevoCargo;
                    objJsonMessage.Mensaje = "ERROR: Verifique los datos ingresados";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
                var solicitudNuevoCargoEditar = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == solicitudNuevoCargo.IdeSolicitudNuevoCargo);
                solicitudNuevoCargoEditar.UsuarioModificacion = Session[ConstanteSesion.Usuario].ToString();
                solicitudNuevoCargoEditar.FechaModificacion = FechaModificacion;
                solicitudNuevoCargoEditar.RangoSalarioPublicar = solicitudNuevoCargo.RangoSalarioPublicar;
                solicitudNuevoCargoEditar.FechaPublicacion = solicitudNuevoCargo.FechaPublicacion;
                solicitudNuevoCargoEditar.FechaExpiracion = solicitudNuevoCargo.FechaExpiracion;
                _solicitudNuevoCargoRepository.Update(solicitudNuevoCargoEditar);

                objJsonMessage.Mensaje = "Publicado Correctamente";
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR: "+ex;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }

        public PublicacionViewModel inicializarPublicacion(Cargo cargo)
        {
            
            var publicacionNuevoViewModel = new PublicacionViewModel();

            publicacionNuevoViewModel.Cargo = new Cargo();
            publicacionNuevoViewModel.SolicitudCargo = new SolicitudNuevoCargo();

            var horario = _horarioCargoRepository.getMaxPuntValue(x => x.Cargo.IdeCargo == cargo.IdeCargo);
            publicacionNuevoViewModel.TipoHorario = horario.DescripcionHorario;

            string tipoRangoSalarial = cargo.TipoRangoSalarial;

            publicacionNuevoViewModel.RangoSalario = _detalleGeneralRepository.GetByTableDescription(TipoTabla.TipoSalario, tipoRangoSalarial);

            
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
