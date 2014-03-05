﻿using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Entity.Validation;
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

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    [Authorize]
    public class SolicitudAmpliacionCargoController : BaseController
    {
        
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solicitudAmpliacionPersonal;
        private ICargoRepository _cargoRepository;
        private IAreaRepository _areaRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IDepartamentoRepository _departamentoRepository;
        

        public SolicitudAmpliacionCargoController(IDetalleGeneralRepository detalleGeneralRepository,
                                                  ISolReqPersonalRepository solicitudAmpliacionPersonal,
                                                  ICargoRepository cargoRepository,
                                                  IAreaRepository areaRepository,
                                                  IDependenciaRepository dependenciaRepository,
                                                  IDepartamentoRepository departamentoRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solicitudAmpliacionPersonal = solicitudAmpliacionPersonal;
            _cargoRepository = cargoRepository;
            _areaRepository = areaRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            
        }
        
        [AuthorizeUser]        
        public ActionResult Index()
        {
            return View();
        }
        [ValidarSesion]
        [AuthorizeUser]
        public ActionResult Edit()
        {
            SolicitudAmpliacionCargoViewModel solicitudModel = inicializarAmpliacionCargo();
                var usuario = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
                SolReqPersonal solicitudAmpliacion = new SolReqPersonal();
                solicitudAmpliacion.IdeArea = usuario.IDEAREA;
                solicitudAmpliacion.IdeSede = usuario.IDESEDE;
                solicitudAmpliacion.IdeDepartamento = usuario.IDEDEPARTAMENTO;
                solicitudAmpliacion.IdeDependencia = usuario.IDEDEPENDENCIA;
                var departamento = _departamentoRepository.GetSingle(x => x.IdeDepartamento == usuario.IDEDEPARTAMENTO);
                var dependencia = _dependenciaRepository.GetSingle(x => x.IdeDependencia == usuario.IDEDEPENDENCIA);
                var area = _areaRepository.GetSingle(x => x.IdeArea == usuario.IDEAREA);
                solicitudAmpliacion.Departamento_des = departamento.NombreDepartamento;
                solicitudAmpliacion.Dependencia_des = dependencia.NombreDependencia;
                solicitudAmpliacion.Area_des = area.NombreArea;
                solicitudModel.SolicitudRequerimiento = solicitudAmpliacion;
            
            return View(solicitudModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "SolReqPersonal")]SolReqPersonal solicitudAmpliacion)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                SolReqPersonalValidator validation = new SolReqPersonalValidator();
                ValidationResult result = validation.Validate(solicitudAmpliacion, "IdeCargo", "NumVacantes", "Observacion", "Motivo");
                
                if (!result.IsValid)
                {
                    var solicitudAmpliacionModel = inicializarAmpliacionCargo();
                    solicitudAmpliacionModel.SolicitudRequerimiento = solicitudAmpliacion;
                    return View(solicitudAmpliacionModel);
                }
                solicitudAmpliacion.EstadoActivo = "A";
                solicitudAmpliacion.FechaCreacion = FechaCreacion;
                solicitudAmpliacion.UsuarioCreacion = "YO";
                ///agregar tip sol 
                solicitudAmpliacion.FechaModificacion = FechaCreacion;
                _solicitudAmpliacionPersonal.Add(solicitudAmpliacion);

                objJsonMessage.Mensaje = "Solicitud enviada correctamente";
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }

        }

        public SolicitudAmpliacionCargoViewModel inicializarAmpliacionCargo()
        {
            SolicitudAmpliacionCargoViewModel model = new SolicitudAmpliacionCargoViewModel();

            model.Accion = Accion.Nuevo;
            model.SolicitudRequerimiento = new SolReqPersonal();
            model.SolicitudRequerimiento = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion);

            model.Cargos = new List<Cargo>(_cargoRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
            model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            model.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            model.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            model.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            model.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });



            return model;
        }


        [ValidarSesion]
        public ActionResult Puesto(string ideSolicitud)
        {
            try
            {

                var perfilAmpliacionViewModel = inicializarPerfil();
                var usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                if (ideSolicitud != null)
                {
                    IdeSolicitudAmpliacion = Convert.ToInt32(ideSolicitud);
                    var cargoAmpliacion = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion);
                    perfilAmpliacionViewModel.SolicitudRequerimiento = cargoAmpliacion;
                }

                return PartialView(perfilAmpliacionViewModel);
            }
            catch (Exception)
            {
                //return View(perfilAmpliacionViewModel);
                return PartialView();
            }

        }


        public SolicitudAmpliacionCargoViewModel inicializarPerfil()
        {
            var ampliacionViewModel = new SolicitudAmpliacionCargoViewModel();
            ampliacionViewModel.SolicitudRequerimiento = new SolReqPersonal();


            return ampliacionViewModel;
        }

        public ActionResult General()
        {
            var perfilAmpliacionViewModel = inicializarGeneral();
            if (CargoPerfil != null)
            {
                var solicitudAmpliacion = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion);
                perfilAmpliacionViewModel.SolicitudRequerimiento = solicitudAmpliacion;
            }

            return PartialView(perfilAmpliacionViewModel);
        }


        public SolicitudAmpliacionCargoViewModel inicializarGeneral()
        {
            
            var cargoViewModel = new SolicitudAmpliacionCargoViewModel();
            cargoViewModel.SolicitudRequerimiento = new SolReqPersonal();

            cargoViewModel.Accion = Accion.Nuevo;

            cargoViewModel.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            cargoViewModel.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            cargoViewModel.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            cargoViewModel.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            cargoViewModel.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return cargoViewModel;
        }

        public ActionResult Estudio()
        {
            var estudioCargoViewModel = inicializarDatosCargo();
            return View(estudioCargoViewModel);
        }

        public ActionResult Experiencia()
        {

            var experienciaCargoViewModel = inicializarDatosCargo();
            return View(experienciaCargoViewModel);
        }

        public ActionResult Conocimientos()
        {
            var conocimientosCargoViewModel = inicializarDatosCargo();
            return View(conocimientosCargoViewModel);
        }

        public ActionResult Discapacidad()
        {
            var discapacidadCargoViewModel = inicializarDatosCargo();
            return View(discapacidadCargoViewModel);
        }

        public ActionResult ConfiguracionPerfil()
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var discapacidadCargoViewModel = inicializarDatosConfig(IdeSolicitudAmpliacion);
            return View(discapacidadCargoViewModel);
        }

        public PerfilAmpliacionViewModel inicializarDatosCargo()
        {
            var discapacidadCargoViewModel = new PerfilAmpliacionViewModel();
            discapacidadCargoViewModel.SolicitudRequerimiento = new SolReqPersonal();
            return discapacidadCargoViewModel;
        }

        public PerfilAmpliacionViewModel inicializarDatosConfig(int IdeSolicitud)
        {
            var solicitudViewModel = new PerfilAmpliacionViewModel();
            var solicitudActual = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitud);
            solicitudViewModel.SolicitudRequerimiento = solicitudActual;
            return solicitudViewModel;
        }

        public ActionResult Evaluacion()
        {
            var evaluacionCargoViewModel = inicializarDatosCargo();
            return View(evaluacionCargoViewModel);
        }

        #region GRILLAS PERFIL AMPLIACION
        //
        //COMPETENCIAS
        //

        [HttpPost]
        public JsonResult ListarCompetencias(GridTable grid)
        {
            List<CompetenciaRequerimiento> lista = new List<CompetenciaRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;
                grid.rows = (grid.rows == 0) ? 10 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaCompetencias(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCompetenciaRequerimiento.ToString(),
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
        //    
        //OFRECEMOS
        //
        [HttpPost]
        public virtual JsonResult ListarOfrecemos(GridTable grid)
        {
            List<OfrecemosRequerimiento> lista = new List<OfrecemosRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaOfrecemos(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeOfrecemosRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.IdeOfrecemosRequerimiento.ToString(),
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
        ////
        ////HORARIO
        ////

        [HttpPost]
        public virtual JsonResult ListaHorario(GridTable grid)
        {
            List<HorarioRequerimiento> lista = new List<HorarioRequerimiento>();

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaHorarios(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeHorarioRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionHorario,
                                item.PuntajeHorario.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }
        ////
        ////UBIGEO
        ////

        [HttpPost]
        public virtual JsonResult ListaUbigeo(GridTable grid)
        {

            List<UbigeoReemplazo> lista = new List<UbigeoReemplazo>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;


                lista = _solicitudAmpliacionPersonal.ListaUbigeos(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeUbigeoReemplazo.ToString(),
                        cell = new string[]
                            {
                                item.Departamento,
                                item.Provincia,
                                item.Distrito,
                                item.PuntajeUbigeo.ToString(),
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
        public virtual JsonResult ListaCentroEstudio(GridTable grid)
        {
            List<CentroEstudioRequerimiento> lista = new List<CentroEstudioRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaCentroEstudio(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCentroEstudioRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoCentroEstudio,
                                item.DescripcionNombreCentroEstudio,
                                item.PuntajeCentroEstudios.ToString(),
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
        public virtual JsonResult ListaNivelAcademico(GridTable grid)
        {
            List<NivelAcademicoRequerimiento> lista = new List<NivelAcademicoRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaNivelAcademico(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoEducacion,
                                item.DescripcionAreaEstudio,
                                item.DescripcionNivelAlcanzado,
                                item.CicloSemestre.ToString(),
                                item.PuntajeNivelEstudio.ToString(),
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
        public virtual JsonResult ListaOfimatica(GridTable grid)
        {
            List<ConocimientoGeneralRequerimiento> lista = new List<ConocimientoGeneralRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaConocimientos(IdeSolicitudAmpliacion, "OFIMATICA");

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoOfimatica,
                                item.DescripcionNombreOfimatica,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
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
        public virtual JsonResult ListaIdioma(GridTable grid)
        {
            List<ConocimientoGeneralRequerimiento> lista = new List<ConocimientoGeneralRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaConocimientos(IdeSolicitudAmpliacion, "IDIOMA");

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionIdioma,
                                item.DescripcionConocimientoIdioma,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
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
        public virtual JsonResult ListaOtrosConocimientos(GridTable grid)
        {
            List<ConocimientoGeneralRequerimiento> lista = new List<ConocimientoGeneralRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaConocimientos(IdeSolicitudAmpliacion, "GENERAL");


                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoGeneral,
                                item.DescripcionNombreConocimientoGeneral,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
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
        public virtual JsonResult ListaExperiencia(GridTable grid)
        {
            List<ExperienciaRequerimiento> lista = new List<ExperienciaRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaExperiencia(IdeSolicitudAmpliacion);


                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString(),
                                item.CantidadMesesExperiencia.ToString(),
                                item.PuntajeExperiencia.ToString(),
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
        public virtual JsonResult ListaDiscapacidad(GridTable grid)
        {
            List<DiscapacidadRequerimiento> lista = new List<DiscapacidadRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaDiscapacidad(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeDiscapacidadRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoDiscapacidad,
                                item.PuntajeDiscapacidad.ToString(),
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
        public virtual JsonResult ListaEvaluaciones(GridTable grid)
        {
            List<EvaluacionRequerimiento> lista = new List<EvaluacionRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solicitudAmpliacionPersonal.ListaEvaluacion(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeEvaluacionRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExamen,
                                item.DescripcionTipoExamen,
                                item.NotaMinimaExamen.ToString(),
                                item.DescripcionAreaResponsable.ToString(),
                                item.PuntajeExamen.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        #endregion


    }
}
