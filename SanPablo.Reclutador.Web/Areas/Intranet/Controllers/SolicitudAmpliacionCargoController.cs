using SanPablo.Reclutador.Entity;
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
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private IUsuarioRepository _usuarioRepository;
        

        public SolicitudAmpliacionCargoController(IDetalleGeneralRepository detalleGeneralRepository,
                                                  ISolReqPersonalRepository solicitudAmpliacionPersonal,
                                                  ICargoRepository cargoRepository,
                                                  IAreaRepository areaRepository,
                                                  IDependenciaRepository dependenciaRepository,
                                                  IDepartamentoRepository departamentoRepository,
                                                  IUsuarioRolSedeRepository usuarioRolSedeRepository,
                                                  IUsuarioRepository usuarioRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solicitudAmpliacionPersonal = solicitudAmpliacionPersonal;
            _cargoRepository = cargoRepository;
            _areaRepository = areaRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _usuarioRepository = usuarioRepository;
        }
        
        
        [ValidarSesion]
        public ActionResult Edit(string id, string pagina)
        {
            SolicitudAmpliacionCargoViewModel solicitudModel = inicializarAmpliacionCargo( pagina);
            
            var usuario = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            
            SolReqPersonal solicitudAmpliacion = new SolReqPersonal();

            IdeSolicitudAmpliacion = Convert.ToInt32(id);

            if (IdeSolicitudAmpliacion != 0)
            {
                var solicitud = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion && x.TipoSolicitud == TipoSolicitud.Ampliacion);
                solicitudModel.SolicitudRequerimiento = solicitud;

                var departamento = _departamentoRepository.GetSingle(x => x.IdeDepartamento == solicitud.IdeDepartamento);
                var area = _areaRepository.GetSingle(x => x.IdeArea == solicitud.IdeArea);

                solicitudModel.Areas.Add(area);
                solicitudModel.Departamentos.Add(departamento);
                
                var rolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                
                if ((rolSession == Roles.Gerente) || (rolSession == Roles.Gerente_General_Adjunto)|| (solicitud.TipEtapa== Etapa.Aprobado))
                {
                    solicitudModel.Accion = Accion.Aprobar;
                }
                if (solicitud.TipEtapa == Etapa.Aceptado)
                {
                    solicitudModel.Accion = Accion.Publicar;
                }
            }
            else
            {
                solicitudModel.Accion = Accion.Enviar;
                var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
                solicitudAmpliacion.Departamento_des = usuarioSession.DEPARTAMENTODES;
                solicitudAmpliacion.Dependencia_des = usuario.DEPENDENCIADES;
                solicitudAmpliacion.Area_des = usuario.AREADES;
                solicitudAmpliacion.IdeSede = usuario.IDESEDE;
                solicitudAmpliacion.IdeDependencia = usuario.IDEDEPENDENCIA;
                solicitudAmpliacion.IdeDepartamento = usuario.IDEDEPARTAMENTO;
                solicitudModel.SolicitudRequerimiento = solicitudAmpliacion;
            }
            return View(solicitudModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit(SolicitudAmpliacionCargoViewModel model)
        {

            SolReqPersonal solicitudAmpliacion = model.SolicitudRequerimiento;
            JsonMessage objJsonMessage = new JsonMessage();
            
            try
            {
                SolReqPersonalValidator validation = new SolReqPersonalValidator();
                ValidationResult result = validation.Validate(solicitudAmpliacion, "IdeCargo", "NumVacantes", "Observacion", "Motivo");
                
                if (!result.IsValid)
                {
                    var solicitudAmpliacionModel = inicializarAmpliacionCargo(model.Pagina);
                    solicitudAmpliacionModel.SolicitudRequerimiento = solicitudAmpliacion;
                    return View(solicitudAmpliacionModel);
                }
               
                solicitudAmpliacion.TipoSolicitud = TipoSolicitud.Ampliacion; 
                solicitudAmpliacion.FechaModificacion = FechaCreacion;
                Cargo cargoSol = _cargoRepository.GetSingle(x=>x.IdeCargo == solicitudAmpliacion.IdeCargo);
                var rolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                var rolResponsable = 0;
                var etapaInicio = "";
                /// 
                ///enviar dependiendo el usuario que registra la solicitud
                /// 
                switch (rolSuceso)
                {
                    case Roles.Jefe:
                        rolResponsable = Roles.Gerente;
                        etapaInicio = Etapa.Pendiente;
                        break;

                    case Roles.Gerente:
                        rolResponsable = Roles.Gerente_General_Adjunto;
                        etapaInicio = Etapa.Validado;
                        break;

                    case Roles.Gerente_General_Adjunto:
                        rolResponsable = Roles.Encargado_Seleccion;
                        etapaInicio = Etapa.Aprobado;
                        break;
                }

                if ((rolResponsable != 0) && (etapaInicio != ""))
                {
                    var idUsuarioResponsable = _solicitudAmpliacionPersonal.insertarSolicitudAmpliacion(solicitudAmpliacion, Convert.ToInt32(Session[ConstanteSesion.Usuario]), rolSuceso, etapaInicio, rolResponsable, "SI");
                    if (idUsuarioResponsable != -1)
                    {
                        var usuarioResponsable = _usuarioRepository.GetSingle(x => x.IdUsuario == idUsuarioResponsable);

                        bool flag = EnviarCorreo(usuarioResponsable, rolResponsable.ToString(), etapaInicio, "", cargoSol.NombreCargo, cargoSol.CodigoCargo);
                        string msj = "";
                        if (!flag)
                        {
                            msj = "Falló envio de correo";
                        }
                        objJsonMessage.Mensaje = "Solicitud enviada correctamente " + msj;
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "Solicitud no enviada, intente de nuevo ";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    
                }
                else
                {
                    objJsonMessage.Mensaje = "No puede realizar esta accion, revise sus permisos";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }

        }

        public SolicitudAmpliacionCargoViewModel inicializarAmpliacionCargo(string pagina)
        {
            SolicitudAmpliacionCargoViewModel model = new SolicitudAmpliacionCargoViewModel();

            model.SolicitudRequerimiento = new SolReqPersonal();

            model.Pagina = pagina;

            model.Cargos = new List<Cargo>(_cargoRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
            model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            model.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            model.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            model.TipoPuestos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.TipoPuestos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            model.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            model.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            model.Departamentos = new List<Departamento>();
            model.Areas = new List<Area>();
            if (Convert.ToInt32(Session[ConstanteSesion.Rol]) == Roles.Gerente_General_Adjunto)
            {
                model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
                model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccione" });
                
                model.Departamentos.Insert(0, new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccione" });
                
                model.Areas.Insert(0, new Area { IdeArea = 0, NombreArea = "Seleccione" });
            }
            else
            {
                var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
                model.Dependencias = new List<Dependencia>();
                model.Dependencias.Add(new Dependencia { IdeDependencia = usuarioSession.IDEDEPENDENCIA, NombreDependencia = usuarioSession.DEPENDENCIADES });

                model.Departamentos = new List<Departamento>();
                model.Departamentos.Add(new Departamento { IdeDepartamento = usuarioSession.IDEDEPARTAMENTO, NombreDepartamento = usuarioSession.DEPARTAMENTODES });

                model.Areas = new List<Area>();
                model.Areas.Add(new Area { IdeArea = usuarioSession.IDEAREA, NombreArea = usuarioSession.AREADES });
            }
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
                    var cargoAmpliacion = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion && x.TipoSolicitud == TipoSolicitud.Ampliacion);
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

        public bool EnviarCorreo(Usuario usuarioDestinatario, string rolResponsable, string etapa, string observacion, string cargoDescripcion, string codCargo)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            try
            {
                SendMail enviarMail = new SendMail();
                enviarMail.Area = usuarioSession.AREADES;
                enviarMail.Sede = usuarioSession.SEDEDES;
                enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();
                enviarMail.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();

                enviarMail.EnviarCorreo(dir,etapa, rolResponsable,"Ampliación de cargo", observacion, cargoDescripcion, codCargo, usuarioDestinatario.Email, "suceso");
                
               return true;
            }
            catch (Exception Ex)
            {
                return false;
                
            }

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


        #region GRILLAS PERFIL AMPLIACION
        ///
        ///COMPETENCIAS
        ///

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
        ///
        ///HORARIO
        ///

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
        /// <summary>
        ///UBIGEO
        /// </summary>

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

        #region lista ampliacion

        /// <summary>
        /// inicializa la busqueda de lista de reemplazo
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult Index()
        {
            SolicitudAmpliacionCargoViewModel model;
            try
            {
                model = new SolicitudAmpliacionCargoViewModel();


                var sede = Session[ConstanteSesion.Sede];
                if (sede != null)
                {
                    model = InicializarListaReemplazo(Convert.ToInt32(sede));
                    model.SolicitudRequerimiento = new SolReqPersonal();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View("Index", model);
        }

        /// <summary>
        /// lista de departamentos
        /// </summary>
        /// <param name="ideDependencia"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaDepartamentos(int ideDependencia)
        {
            ActionResult result = null;
            Dependencia objDepencia = new Dependencia();

            var listaResultado = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == ideDependencia));

            foreach (Departamento item in listaResultado)
            {
                item.Dependencia = null;
            }
            
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// lista de areas
        /// </summary>
        /// <param name="ideDepartamento"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaAreas(int ideDepartamento)
        {
            ActionResult result = null;

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));

            foreach (Area item in listaResultado)
            {
                item.Departamento = null;
            }
            
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// iniciliza la pantalla de busqueda de reemplazo
        /// </summary>
        /// <param name="idSel"></param>
        /// <returns></returns>
        public SolicitudAmpliacionCargoViewModel InicializarListaReemplazo(int idSel)
        {
            var model = new SolicitudAmpliacionCargoViewModel();

            model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                         && x.IdeSede == idSel));
            model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            model.Departamentos = new List<Departamento>();
            model.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            model.Areas = new List<Area>();
            model.Areas.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            model.Cargos = new List<Cargo>(_solicitudAmpliacionPersonal.GetTipCargo(0));
            model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.Roles = new List<Rol>(_usuarioRolSedeRepository.GetListaRol(0));
            model.Roles.Insert(0, new Rol { IdRol = 0, CodRol = "Seleccionar" });

            model.Etapas =new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapaSolicitud));
            model.Etapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.Estados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            model.Estados.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.Puestos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.Puestos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return model;
        }


        /// <summary>
        /// Lista de busqueda de reemplazo
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListBusquedaAmpliacion(GridTable grid)
        {

            SolReqPersonal solicitudRequerimiento;
            List<SolReqPersonal> lista = new List<SolReqPersonal>();
            try
            {

                    solicitudRequerimiento = new SolReqPersonal();

                    solicitudRequerimiento.IdeCargo = (grid.rules[1].data == null ? 0 : Convert.ToInt32(grid.rules[1].data));
                    solicitudRequerimiento.IdeDependencia = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));
                    solicitudRequerimiento.IdeArea = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                    solicitudRequerimiento.TipResponsable = (grid.rules[4].data == null ? "" : grid.rules[4].data);

                    if (grid.rules[5].data != null && grid.rules[6].data != null)
                    {
                        solicitudRequerimiento.FechaInicioBus = Convert.ToDateTime(grid.rules[5].data);
                        solicitudRequerimiento.FechaFinBus = Convert.ToDateTime(grid.rules[6].data);
                    }

                    solicitudRequerimiento.IdeDepartamento = (grid.rules[7].data == null ? 0 : Convert.ToInt32(grid.rules[7].data));
                    solicitudRequerimiento.TipEtapa = (grid.rules[8].data == null ? "" : grid.rules[8].data);
                    solicitudRequerimiento.TipEstado = (grid.rules[9].data == null ? "" : grid.rules[9].data);

                    solicitudRequerimiento.Tipsol = TipoSolicitud.Ampliacion;

                    var IdRolResp = Session[ConstanteSesion.Rol];
                    solicitudRequerimiento.IdRolResp = Convert.ToInt32(IdRolResp);
                    
                    var idUsuarioResp = Session[ConstanteSesion.Usuario];
                    solicitudRequerimiento.idUsuarioResp = Convert.ToInt32(idUsuarioResp);

                    var idSede = Session[ConstanteSesion.Sede];
                    solicitudRequerimiento.IdeSede = Convert.ToInt32(idSede);

                    lista = _solicitudAmpliacionPersonal.GetListaSolReqPersonal(solicitudRequerimiento);
                //}


                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeSolReqPersonal.ToString(),
                    cell = new string[]
                            {
                               
                                "1",
                                item.TipEstado==null?"":item.TipEstado,
                                item.IdeSolReqPersonal==null?"":item.IdeSolReqPersonal.ToString(),
                                item.CodSolReqPersonal==null?"":item.CodSolReqPersonal.ToString(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.DesCargo==null?"":item.DesCargo,
                                item.IdeDependencia==null?"":item.IdeDependencia.ToString(),
                                item.Dependencia_des==null?"":item.Dependencia_des,
                                item.IdeDepartamento==null?"":item.IdeDepartamento.ToString(),
                                item.Departamento_des==null?"":item.Departamento_des,
                                item.IdeArea==null?"":item.IdeArea.ToString(),
                                item.Area_des==null?"":item.Area_des,
                                item.NumVacantes==null?"":item.NumVacantes.ToString(),
                                item.CantPostulante==null?"":item.CantPostulante.ToString(),
                                item.CantPreSelec==null?"":item.CantPreSelec.ToString(),
                                item.CantEvaluados==null?"":item.CantEvaluados.ToString(),
                                item.CantSeleccionados==null?"":item.CantSeleccionados.ToString(),
                                item.Feccreacion==null?"":String.Format("{0:dd/MM/yyyy}", item.Feccreacion),
                                item.FecExpiracacion==null?"":String.Format("{0:dd/MM/yyyy}", item.FecExpiracacion),
                               
                                item.idRolSuceso==null?"":item.idRolSuceso.ToString(),
                                item.DesRolSuceso==null?"":item.DesRolSuceso,
                                item.NomPersonReemplazo==null?"":item.NomPersonReemplazo,
                                
                                item.FlagPublicado==null?"":item.FlagPublicado,
                                item.TipEtapa==null?"":item.TipEtapa,
                                item.Tipsol==null?"":item.Tipsol,
                                item.Des_etapa==null?"":item.Des_etapa
                               
                            }
                }).ToArray();

                return Json(generic.Value);

            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }


        /// <summary>
        /// view de publicacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Publica(string id , string pagina)
        {
            SolicitudRempCargoViewModel model;
            model = new SolicitudRempCargoViewModel();
            model.SolReqPersonal = new SolReqPersonal();
            
            var ObjSol = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(id));

            if (ObjSol != null)
            {
                model.SolReqPersonal.nombreCargo = ObjSol.nombreCargo;
                model.SolReqPersonal.DesCargo = ObjSol.DesCargo;
                model.SolReqPersonal.IdeSolReqPersonal = ObjSol.IdeSolReqPersonal;

                var objArea = _areaRepository.GetSingle(x => x.Departamento.IdeDepartamento == ObjSol.IdeDepartamento
                                                        && x.IdeArea == ObjSol.IdeArea);

                model.SolReqPersonal.Area_des = objArea.NombreArea;

                int TipoPuesto = Convert.ToInt32(TipoCampo.TipoSalario);

                var ObjDetalleGeneral = _detalleGeneralRepository.GetSingle(x => x.IdeGeneral == TipoPuesto
                                                                            && x.Valor == ObjSol.TipPuesto);

                model.SolReqPersonal.TipPuestoDes = ObjDetalleGeneral.Descripcion == null ? "" : ObjDetalleGeneral.Descripcion;

                model.SolReqPersonal.NumVacantes = ObjSol.NumVacantes;
                model.SolReqPersonal.FuncionesCargo = ObjSol.FuncionesCargo;

                model.listaRangoSalarial =
                    new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
                model.listaRangoSalarial.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

                model.SolReqPersonal.TipoRangoSalario = ObjSol.TipoRangoSalario == null ? "" : ObjSol.TipoRangoSalario;

            }

            if (pagina == TipoSolicitud.ConsultaRequerimientos)
            {
                model.btnActualizar = Visualicion.SI;
                model.btnPublicar = Visualicion.NO;
            }
            else
            {
                model.btnPublicar = Visualicion.SI;
                model.btnActualizar = Visualicion.NO;
            }

            model.Pagina = pagina;

            return View("Publicacion", model);
        }


        /// <summary>
        /// Realiza la publicacion de la solicitud de reemplazo de personal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult PublicaSolReqPersonal(SolicitudRempCargoViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            var verSalario = model.verSalario;
            string IndVerSalario;

            if (verSalario)
            {
                IndVerSalario = "S";
            }
            else
            {
                IndVerSalario = "N";
            }


            if (model != null)
            {

                var objSol = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(model.SolReqPersonal.IdeSolReqPersonal));
                if (objSol != null)
                {

                    var objCargo = _cargoRepository.GetSingle(x => x.IdeCargo == objSol.IdeCargo);

                    objSol.FecPublicacion = model.SolReqPersonal.FecPublicacion;
                    objSol.FechaModificacion = FechaSistema;
                    objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;
                    objSol.FecExpiracacion = model.SolReqPersonal.FecExpiracacion;
                    objSol.TipEtapa = Etapa.Publicado;
                    objSol.IndicadorSalario = IndVerSalario;
                    objSol.ObservacionPublica = model.SolReqPersonal.ObservacionPublica;

                    _solicitudAmpliacionPersonal.Update(objSol);

                    model.LogSolReqPersonal = new LogSolReqPersonal();
                    model.LogSolReqPersonal.IdeSolReqPersonal = (int)objSol.IdeSolReqPersonal;
                    model.LogSolReqPersonal.UsrSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    model.LogSolReqPersonal.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                    string desRol = Convert.ToString(Session[ConstanteSesion.RolDes]);
                    model.LogSolReqPersonal.FecSuceso = FechaSistema;
                    model.LogSolReqPersonal.TipEtapa = Etapa.Publicado;

                    _solicitudAmpliacionPersonal.ActualizaLogSolReq(model.LogSolReqPersonal);

                    var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == model.LogSolReqPersonal.UsrSuceso);

                    bool flag = EnviarCorreo(objUsuario, desRol, Etapa.Publicado, "", objCargo.NombreCargo, objCargo.CodigoCargo);

                    objJson.Resultado = true;
                    objJson.Mensaje = "Se publico la Solicitud";
                }
            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede realizar la publicación de la solicitud";
            }



            return Json(objJson);
        }


        #endregion


    }
}
