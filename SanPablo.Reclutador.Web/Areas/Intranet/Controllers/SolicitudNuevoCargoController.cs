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
    public class SolicitudNuevoCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IAreaRepository _areaRepository;
        private ILogSolicitudNuevoCargoRepository _logSolicitudNuevoCargoRepository;
        private IListaSolicitudNuevoCargoVistaRepository _listaSolicitudRepository;
        private IUsuarioRepository _usuarioRepository;
        private IRolRepository _rolRepository;

      
        public SolicitudNuevoCargoController(ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                             IDetalleGeneralRepository detalleGeneralRepository,
                                             IDependenciaRepository dependenciaRepository,
                                             IDepartamentoRepository departamentoRepository,
                                             IAreaRepository areaRepository,
                                             IListaSolicitudNuevoCargoVistaRepository listaSolicitudRepository,
                                             ILogSolicitudNuevoCargoRepository logSolicitudNuevoCargoRepository,
                                             IUsuarioRepository usuarioRepository,
                                             IRolRepository rolRepository)
        {
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _areaRepository = areaRepository;
            _listaSolicitudRepository = listaSolicitudRepository;
            _usuarioRepository = usuarioRepository;
            _logSolicitudNuevoCargoRepository = logSolicitudNuevoCargoRepository;
            _rolRepository = rolRepository;
        }


        [HttpPost]
        public virtual JsonResult ListaSolicitudes(GridTable grid)
        {
            SolicitudNuevoCargo solicitudNuevo;
            List<SolicitudNuevoCargo> lista = new List<SolicitudNuevoCargo>();
            try
            {

                solicitudNuevo = new SolicitudNuevoCargo();

                solicitudNuevo.IdeSolicitudNuevoCargo = (grid.rules[1].data == null ? 0 : Convert.ToInt32(grid.rules[1].data));
                solicitudNuevo.IdeDependencia = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));
                solicitudNuevo.IdeArea = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                solicitudNuevo.TipoResponsable = (grid.rules[4].data == null ? 0 : Convert.ToInt32(grid.rules[4].data));

                if (grid.rules[5].data != null && grid.rules[6].data != null)
                {
                    solicitudNuevo.FechaBusquedaInicio = Convert.ToDateTime(grid.rules[5].data);
                    solicitudNuevo.FechaBusquedaFin = Convert.ToDateTime(grid.rules[6].data);
                }

                solicitudNuevo.IdeDepartamento = (grid.rules[7].data == null ? 0 : Convert.ToInt32(grid.rules[7].data));
                solicitudNuevo.TipoEtapa = (grid.rules[8].data == null ? "" : grid.rules[8].data);
                solicitudNuevo.TipoEstado = (grid.rules[9].data == null ? "" : grid.rules[9].data);

                solicitudNuevo.RolResponsableActual = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                solicitudNuevo.IdeUsuarioResponsable = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                solicitudNuevo.IdeSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);

                lista = _solicitudNuevoCargoRepository.GetListaSolicitudNuevo(solicitudNuevo);



                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeSolicitudNuevoCargo.ToString(),
                    cell = new string[]
                            {
                               
                                "1",
                                item.TipoEstado==null?"":item.TipoEstado,
                                item.IdeSolicitudNuevoCargo==null?"":item.IdeSolicitudNuevoCargo.ToString(),
                                item.CodigoCargo==null?"":item.CodigoCargo.ToString().ToUpper(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.NombreCargo==null?"":item.NombreCargo.ToUpper(),
                                item.IdeDependencia==null?"":item.IdeDependencia.ToString(),
                                item.DependenciaDescripcion==null?"":item.DependenciaDescripcion,
                                item.IdeDepartamento==null?"":item.IdeDepartamento.ToString(),
                                item.DepartamentoDescripcion==null?"":item.DepartamentoDescripcion,
                                item.IdeArea==null?"":item.IdeArea.ToString(),
                                item.AreaDescripcion==null?"":item.AreaDescripcion,
                                item.NumeroPosiciones==null?"":item.NumeroPosiciones.ToString(),
                                item.Postulantes==null?"":item.Postulantes.ToString(),
                                item.PreSeleccionados==null?"":item.PreSeleccionados.ToString(),
                                item.Evaluados==null?"":item.Evaluados.ToString(),
                                item.Seleccionados==null?"":item.Seleccionados.ToString(),
                                item.FechaCreacion==null?"":item.FechaCreacion.ToString(),
                                item.FechaExpiracion==null?"":item.FechaExpiracion.ToString(),
                               
                                item.IdRolSuceso==null?"":item.IdRolSuceso.ToString(),
                                item.RolSuceso==null?"":item.RolSuceso,
                                item.NombreResponsable==null?"":item.NombreResponsable,
                                
                                item.IndicadoPublicado==null?"":item.IndicadoPublicado,
                                item.TipoEtapa==null?"":item.TipoEtapa,
                                item.IdeUsuarioResponsable ==null?"":item.IdeUsuarioResponsable.ToString(),
                               
                            }
                }).ToArray();

                return Json(generic.Value);

            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }
        [ValidarSesion]
        [AuthorizeUser]
        public ActionResult Index()
        {
            var solicitudnuevoViewModel = inicializarNuevaSolicitud();
            return View(solicitudnuevoViewModel);
        }


        [HttpPost]
        public ActionResult CambiarEstado(string id, string codEstado)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            SolicitudNuevoCargo solicitudNuevoCargo = new SolicitudNuevoCargo();
            try
            {
                solicitudNuevoCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == Convert.ToInt32(id));
                
                if (IndicadorActivo.Activo.Equals(codEstado))
                {
                    solicitudNuevoCargo.EstadoActivo = IndicadorActivo.Inactivo;
                    objJsonMessage.Mensaje = "La solicitud fue desactivada";
                }
                else
                {
                    solicitudNuevoCargo.EstadoActivo = IndicadorActivo.Activo;
                    objJsonMessage.Mensaje = "La solicitud fue activada";
                }
                _solicitudNuevoCargoRepository.Update(solicitudNuevoCargo);
                objJsonMessage.Resultado = true;
            }
            catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "ERROR: Ocurrio un error al cambiar el estado";
            }

            return Json(objJsonMessage);
        }

        [ValidarSesion]
        public ActionResult Edit(string id , string pagina)
        {
            var solicitudNuevoCargoViewModel = inicializarSolicitudNuevoCargo(pagina);
            if (id != "0")
            {
                var solNuevoCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == Convert.ToInt32(id));
                solicitudNuevoCargoViewModel.SolicitudNuevoCargo = solNuevoCargo;
                actualizarDatosAreas(solicitudNuevoCargoViewModel, solNuevoCargo.IdeArea);
                if (solNuevoCargo.EstadoActivo == IndicadorActivo.Activo)
                {
                    solicitudNuevoCargoViewModel.Estado = "Activo";
                }
                else
                { solicitudNuevoCargoViewModel.Estado = "Inactivo"; }
                solicitudNuevoCargoViewModel.Accion = Accion.Aprobar;
            }
            else
            {
                solicitudNuevoCargoViewModel.Accion = Accion.Enviar;
            }
            return View(solicitudNuevoCargoViewModel);
        }

        public SolicitudNuevoCargoViewModel inicializarNuevaSolicitud()
        {
            var solicitudNuevoViewModel = new SolicitudNuevoCargoViewModel();
            
            solicitudNuevoViewModel.SolicitudNuevoCargo = new SolicitudNuevoCargo();

            solicitudNuevoViewModel.Cargos = new List<SolicitudNuevoCargo>(_solicitudNuevoCargoRepository.ListarCargos(
                                             Convert.ToInt32(Session[ConstanteSesion.Sede]),Convert.ToInt32(Session[ConstanteSesion.Rol]),Convert.ToInt32(Session[ConstanteSesion.Usuario])));
            solicitudNuevoViewModel.Cargos.Insert(0, new SolicitudNuevoCargo { IdeSolicitudNuevoCargo = 0,NombreCargo="Seleccionar"});

            solicitudNuevoViewModel.Estados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoRegistro));
            solicitudNuevoViewModel.Estados.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            solicitudNuevoViewModel.Etapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapa));
            solicitudNuevoViewModel.Etapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            solicitudNuevoViewModel.Responsables = new List<Rol>(_rolRepository.GetBy(x => x.FlgEstado == IndicadorActivo.Activo));
            solicitudNuevoViewModel.Responsables.Insert(0, new Rol { IdRol = 0, DscRol = "Seleccionar" });

            solicitudNuevoViewModel.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
            solicitudNuevoViewModel.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            solicitudNuevoViewModel.Departamentos = new List<Departamento>();
            solicitudNuevoViewModel.Departamentos.Insert(0, new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            solicitudNuevoViewModel.Areas = new List<Area>();
            solicitudNuevoViewModel.Areas.Insert(0, new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            return solicitudNuevoViewModel;
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit(SolicitudNuevoCargoViewModel model)
        {

            SolicitudNuevoCargo nuevaSolicitudCargo = model.SolicitudNuevoCargo;
            var enviarMail = new SendMail();
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            JsonMessage objJsonMessage = new JsonMessage();
            int ideUsuarioResp =-1;
            try
            {
                SolicitudNuevoCargoValidator solicitudValidator = new SolicitudNuevoCargoValidator();
                ValidationResult result = solicitudValidator.Validate(nuevaSolicitudCargo, "IdeArea", "CodigoCargo", "NombreCargo", "DescripcionCargo", "NumeroPosiciones", "TipoRangoSalarial", "DescripcionEstudios", "DescripcionFunciones", "DescripcionCompetencias", "DescripcionObservaciones");

                if (!result.IsValid)
                {
                    var nuevoCargoViewModel = inicializarSolicitudNuevoCargo(model.Pagina);
                    nuevoCargoViewModel.SolicitudNuevoCargo = nuevaSolicitudCargo;
                    return View(nuevoCargoViewModel);
                }
                if (nuevaSolicitudCargo.IdeSolicitudNuevoCargo == 0)
                {

                    int Sede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
                    int UsuarioSession = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    
                    nuevaSolicitudCargo.IdeSede = Sede;
                    nuevaSolicitudCargo.EstadoActivo = IndicadorActivo.Activo;
                    nuevaSolicitudCargo.FechaCreacion = FechaCreacion;
                    nuevaSolicitudCargo.UsuarioCreacion = Session[ConstanteSesion.UsuarioDes].ToString();

                    var RolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);

                    LogSolicitudNuevoCargo logSolicitud = new LogSolicitudNuevoCargo();
                    logSolicitud.UsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    logSolicitud.RolSuceso = RolSession;

                    string indArea = "NO";
                    
                    
                    switch (RolSession)
                    {
                        case Roles.Jefe:
                            logSolicitud.RolResponsable = Roles.Gerente;
                            logSolicitud.TipoEtapa = Etapa.Pendiente;
                            indArea = "SI";
                            break;

                        case Roles.Gerente:
                            logSolicitud.RolResponsable = Roles.Gerente_General_Adjunto;
                            logSolicitud.TipoEtapa = Etapa.Validado;
                            break;

                        case Roles.Gerente_General_Adjunto:
                            logSolicitud.RolResponsable = Roles.Jefe_Corporativo_Seleccion;
                            logSolicitud.TipoEtapa = Etapa.Aprobado;
                            break;

                    }

                    if ((logSolicitud.RolResponsable != null) && (logSolicitud.RolResponsable != 0))
                    {

                        ideUsuarioResp = _solicitudNuevoCargoRepository.insertarSolicitudNuevo(nuevaSolicitudCargo, logSolicitud, indArea);
                    }
                   // ideUsuarioResp = _logSolicitudNuevoCargoRepository.solicitarAprobacion(nuevaSolicitudCargo, UsuarioSession, Convert.ToInt32(Session[ConstanteSesion.Rol]), "", SucesoSolicitud.Pendiente, EtapasSolicitud.PendienteAprobacionGerenteArea);
                   
                    if (ideUsuarioResp != -1)
                    {
                        SedeNivel datosSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
                        enviarMail.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                        enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();
                        enviarMail.Sede = datosSession.SEDEDES;
                        enviarMail.Area = datosSession.AREADES;

                        Usuario usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuarioResp);
                        var SedeDesc = Session[ConstanteSesion.SedeDes];

                        enviarMail.EnviarCorreo(dir.ToString(), logSolicitud.TipoEtapa, usuario.DscNombres, "Nuevo Cargo", null, nuevaSolicitudCargo.NombreCargo, nuevaSolicitudCargo.CodigoCargo, usuario.Email, "suceso");

                        objJsonMessage.Mensaje = "Solicitud enviada exitosamente";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "ERROR: no se puede enviar enviar la solicitud, revise sus permisos o intente nuevamente";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    objJsonMessage.Mensaje = "No puede enviar la solicitud nuevamente";
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
        public SolicitudNuevoCargoViewModel inicializarSolicitudNuevoCargo(string pagina)
        {
            var solicitudCargoViewModel = new SolicitudNuevoCargoViewModel();
            solicitudCargoViewModel.SolicitudNuevoCargo = new SolicitudNuevoCargo();

            solicitudCargoViewModel.RangosSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            solicitudCargoViewModel.RangosSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            solicitudCargoViewModel.Pagina = pagina;

            var usuarioSede = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            if (usuarioSede != null)
            {
                var rolUsuario = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                if ((rolUsuario == Roles.Jefe) || (rolUsuario == Roles.Gerente))
                {
                    solicitudCargoViewModel.Dependencias = new List<Dependencia>();
                    solicitudCargoViewModel.Dependencias.Add(_dependenciaRepository.GetSingle(x => x.IdeDependencia == usuarioSede.IDEDEPENDENCIA));
                    
                    solicitudCargoViewModel.Departamentos = new List<Departamento>();
                    solicitudCargoViewModel.Departamentos.Add(_departamentoRepository.GetSingle(x => x.IdeDepartamento == usuarioSede.IDEDEPARTAMENTO));

                    solicitudCargoViewModel.Areas = new List<Area>();
                    solicitudCargoViewModel.Areas.Add(_areaRepository.GetSingle(x => x.IdeArea == usuarioSede.IDEAREA));
                }
                else
                {
                    solicitudCargoViewModel.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
                    solicitudCargoViewModel.Dependencias.Insert(0,new Dependencia{IdeDependencia = 0,NombreDependencia = "Seleccionar"});
                    
                    solicitudCargoViewModel.Departamentos = new List<Departamento>();
                    solicitudCargoViewModel.Departamentos.Insert(0, new Departamento {IdeDepartamento = 0, NombreDepartamento = "Seleccionar"});

                    solicitudCargoViewModel.Areas = new List<Area>();
                    solicitudCargoViewModel.Areas.Insert(0, new Area { IdeArea = 0, NombreArea = "Seleccionar" });
                }
            }
            return solicitudCargoViewModel;
        }

        [HttpPost]
        public ActionResult listaDepartamentos(int ideDependencia)
        {
            ActionResult result = null;

            var listaResultado = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == ideDependencia));
            result = Json(listaResultado);
            return result;
        }

        public ActionResult listaAreas(int ideDepartamento)
        {
            ActionResult result = null;

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));
            result = Json(listaResultado);
            return result;
        }
        public void actualizarDatosAreas(SolicitudNuevoCargoViewModel model, int ideArea)
        {
            List<string> datosArea = _solicitudNuevoCargoRepository.obtenerDatosArea(ideArea);

            model.Areas.Add(new Area { IdeArea = Convert.ToInt32(datosArea[0]), NombreArea = datosArea[1] });
            model.Departamentos.Add(new Departamento { IdeDepartamento = Convert.ToInt32(datosArea[2]), NombreDepartamento = datosArea[3] });
            model.Dependencias.Add(new Dependencia { IdeDependencia = Convert.ToInt32(datosArea[4]), NombreDependencia = datosArea[5] });
        }

        [HttpPost]
        public ActionResult recuperarCodigoSolicitud(string codigo)
        {
            
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {

                var solicitudCargo = new SolicitudNuevoCargo();
                int ideSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
                solicitudCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.CodigoCargo == codigo && x.IdeSede == ideSede);
                objJsonMessage.Mensaje = solicitudCargo.IdeSolicitudNuevoCargo.ToString();
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

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult EstadoSolicitud(string ideSolicitud)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                LogSolicitudNuevoCargo estado = _logSolicitudNuevoCargoRepository.estadoSolicitud(Convert.ToInt32(ideSolicitud));

                if ((Etapa.Pendiente != estado.TipoEtapa) && (Etapa.Validado != estado.TipoEtapa))
                {
                    if (estado.RolResponsable == Convert.ToInt32(Session[ConstanteSesion.Rol]))
                    {
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "No tiene pendiente ninguna Acción";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                else  
                {
                    objJsonMessage.Mensaje = "ERROR: La solicitud no tiene las aprobaciones necesarias";
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

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult verificarCodigo(string codCodigo)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                int ideSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
                objJsonMessage.Resultado =  _solicitudNuevoCargoRepository.verificarCodCodigo(codCodigo, ideSede);
                objJsonMessage.Mensaje = "Codigo de cargo ya existe";
                return Json(objJsonMessage);
            }
            catch(Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }
        
       
    }
}
