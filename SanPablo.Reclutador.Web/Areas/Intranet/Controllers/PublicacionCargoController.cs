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
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IUsuarioRepository _usuarioRepository;


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
                                             ILogSolicitudNuevoCargoRepository logSolicitudNuevoCargoRepository,
                                             ISolReqPersonalRepository solReqPersonalRepository,
            IUsuarioRepository usuarioRepository
            )
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
            _solReqPersonalRepository = solReqPersonalRepository;
            _usuarioRepository = usuarioRepository;
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
            
            List<ConocimientoGeneralCargo> lista = new List<ConocimientoGeneralCargo>();

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _conocimientoCargoRepository.listarConocimientosPublicacion(IdeCargo);

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeConocimientoGeneralCargo.ToString(),
                    cell = new string[]
                            {
                                item.DescripcionConocimientoGeneral==null?"":item.DescripcionConocimientoGeneral.ToString(),
                                item.NombreConocimientoGeneral==null?"":item.NombreConocimientoGeneral.ToString(),
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

               


                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

               
                where.Add(Expression.Eq("Cargo.IdeCargo", Idecargo));
                

                var generic = Listar(_experienciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia==null?"":item.DescripcionExperiencia,
                                (item.CantidadAnhosExperiencia==0?"":item.CantidadAnhosExperiencia.ToString() + " AÑO(S) ") +
                                (item.CantidadMesesExperiencia==0?"":item.CantidadMesesExperiencia + " MES(ES)"),
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
        public ActionResult Edit(string id, string pagina)
        {
            SolicitudNuevoCargo solicitud = new SolicitudNuevoCargo();
            if ((id != "0") && (id != null))
            {
                solicitud = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == Convert.ToInt32(id));
                CargoPerfil = new DatosCargo();
                CargoPerfil.IdeCargo = Convert.ToInt32(solicitud.IdeCargo);
            }
            
            int IdeCargo = CargoPerfil.IdeCargo;
            var Cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var publicacionViewModel = inicializarPublicacion(Cargo);
            publicacionViewModel.pagina = pagina;
            publicacionViewModel.editarFechaExpiracion = Indicador.Si;
            publicacionViewModel.editarFechaPublicacion = Indicador.Si;
            publicacionViewModel.editarObservaciones = Indicador.Si;

            if (Cargo != null)
            {
                publicacionViewModel.Cargo = Cargo;
                publicacionViewModel.SolicitudCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeCargo == Cargo.IdeCargo);
            }

            if (pagina == TipoSolicitud.ConsultaRequerimientos)
            {
                publicacionViewModel.btnActualizar = Visualicion.SI;
                publicacionViewModel.btnPublicar = Visualicion.NO;
            }
            else
            {
                publicacionViewModel.btnActualizar = Visualicion.NO;
                publicacionViewModel.btnPublicar = Visualicion.SI;
                //si las solicitud ya fue publicada

                var solictudPublica = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == CargoPerfil.IdeSolicitud);
                if (solictudPublica != null)
                {
                    if (solictudPublica.FechaPublicacion != null)
                    {
                        publicacionViewModel.editarFechaExpiracion = Indicador.Si;
                        publicacionViewModel.editarFechaPublicacion = Indicador.No;
                        publicacionViewModel.editarObservaciones = Indicador.No;
                    }
                }
            }

            //visualizar competencias
            var contadorCompetencias = _competenciaCargoRepository.CountByExpress(x=>x.Cargo.IdeCargo == IdeCargo);

            if (contadorCompetencias >0)
            {
                publicacionViewModel.visualizarCompetencias = Indicador.Si;
            }
            //visualizar ofrecemos

            var contadorOfrecemos = _ofrecemosCargoRepository.CountByExpress(x=>x.Cargo.IdeCargo == IdeCargo);
            if(contadorOfrecemos >0)
            {
                publicacionViewModel.visualizarOfrecemos = Indicador.Si;
            }
           
            return View(publicacionViewModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "SolicitudCargo")]SolicitudNuevoCargo solicitudNuevoCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var Cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var publicacionViewModel = inicializarPublicacion(Cargo);
            List<String> listSends=null;
            List<String> listCopys = null;
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);


            JsonMessage objJsonMessage = new JsonMessage();
            LogSolicitudNuevoCargo logSolicitud = new LogSolicitudNuevoCargo();
            var enviarMail = new SendMail();
            try
            {
                SolicitudNuevoCargoValidator validation = new SolicitudNuevoCargoValidator();
                ValidationResult result = validation.Validate(solicitudNuevoCargo, "ObservacionPublicacion", "FechaPublicacion", "FechaExpiracion");
               

                if (!result.IsValid)
                {
                    publicacionViewModel.SolicitudCargo = solicitudNuevoCargo;
                    objJsonMessage.Mensaje = "ERROR: Verifique los datos ingresados";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
                
                var estadoSolicitud = _logSolicitudNuevoCargoRepository.estadoSolicitud(solicitudNuevoCargo.IdeSolicitudNuevoCargo);
                
                int rolActual = Convert.ToInt32(Session[ConstanteSesion.Rol]);

                
                if ((estadoSolicitud.TipoEtapa == Etapa.Aceptado)&&(Convert.ToInt32(estadoSolicitud.RolResponsable) == rolActual))
                {

                    var solicitudNuevoCargoEditar = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == solicitudNuevoCargo.IdeSolicitudNuevoCargo);
                    solicitudNuevoCargoEditar.UsuarioModificacion = Session[ConstanteSesion.Usuario].ToString();
                    solicitudNuevoCargoEditar.FechaModificacion = FechaModificacion;
                    solicitudNuevoCargoEditar.RangoSalarioPublicar = solicitudNuevoCargo.RangoSalarioPublicar;
                    solicitudNuevoCargoEditar.FechaPublicacion = solicitudNuevoCargo.FechaPublicacion;
                    solicitudNuevoCargoEditar.FechaExpiracion = solicitudNuevoCargo.FechaExpiracion;
                    solicitudNuevoCargoEditar.ObservacionPublicacion = solicitudNuevoCargo.ObservacionPublicacion;
                    solicitudNuevoCargoEditar.TipoEtapa = Etapa.Publicado;

                    logSolicitud.IdeSolicitudNuevoCargo = solicitudNuevoCargo.IdeSolicitudNuevoCargo;
                    logSolicitud.TipoEtapa = Etapa.Publicado;
                    logSolicitud.RolSuceso = rolActual;
                    logSolicitud.UsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    logSolicitud.RolResponsable = rolActual;
                    logSolicitud.UsuarioResponsable = Convert.ToInt32(Session[ConstanteSesion.Usuario]);


                    System.Collections.ArrayList lista = listaEmail(Convert.ToInt32(solicitudNuevoCargoEditar.IdeSolicitudNuevoCargo), idRol, AccionEnvioEmail.Publicar, idSede, TipoSolicitud.Nuevo);
                    listSends = new List<String>();
                    listSends = (List<String>)lista[0];

                    listCopys = new List<String>();
                    listCopys = (List<String>)lista[1];

                    logSolicitud.Observacion = "";

                    _logSolicitudNuevoCargoRepository.solicitarAprobacion(logSolicitud, solicitudNuevoCargoEditar.IdeSede, solicitudNuevoCargoEditar.IdeArea, "NO");


                    ReclutamientoPersona objRecluta = new ReclutamientoPersona();

                    objRecluta.IdeSol =solicitudNuevoCargoEditar.IdeSolicitudNuevoCargo;
                    objRecluta.TipSol = TipoSolicitud.Nuevo;

                    var horario = _horarioCargoRepository.getMaxPuntValue(x => x.Cargo.IdeCargo == solicitudNuevoCargoEditar.IdeCargo);
                    objRecluta.TipPuesto = horario.TipoHorario;
                    objRecluta.IdSede = solicitudNuevoCargoEditar.IdeSede;
                    objRecluta.IdeCargo = Convert.ToInt32(solicitudNuevoCargoEditar.IdeCargo);
                    //Se asigna postulantes potenciales si hay antes de publicar una nueva solicitud
                    _solReqPersonalRepository.verificaPotenciales(objRecluta);
					
                    _solicitudNuevoCargoRepository.Update(solicitudNuevoCargoEditar);

                    var objUsuario = (Usuario)Session[ConstanteSesion.ObjUsuario];

                    bool indEnvio =  enviarCorreoAll(logSolicitud, objUsuario, solicitudNuevoCargoEditar, listSends, listCopys);

                    objJsonMessage.Mensaje = "Publicado Correctamente";
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    if (estadoSolicitud.TipoEtapa == Etapa.Publicado)
                    {
                        var solicitudNuevoCargoEditar = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == solicitudNuevoCargo.IdeSolicitudNuevoCargo);
                        solicitudNuevoCargoEditar.UsuarioModificacion = Session[ConstanteSesion.Usuario].ToString();
                        solicitudNuevoCargoEditar.FechaModificacion = FechaModificacion;
                        solicitudNuevoCargoEditar.FechaExpiracion = solicitudNuevoCargo.FechaExpiracion;

                        _solicitudNuevoCargoRepository.Update(solicitudNuevoCargoEditar);

                        objJsonMessage.Mensaje = "Fecha de expiración actualizada correctamente";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        publicacionViewModel.SolicitudCargo = solicitudNuevoCargo;
                        objJsonMessage.Mensaje = "ERROR: La solicitud no esta pendiente de publicacion";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR: "+ex;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }


        public bool enviarCorreoAll(LogSolicitudNuevoCargo logSolicitud, Usuario usuario, SolicitudNuevoCargo solicitudNuevo, List<String> Sends, List<String> Copys)
        {
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            SedeNivel usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            SendMail enviar = new SendMail();
            //enviar.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();

            var objUsuario = (Usuario)Session[ConstanteSesion.ObjUsuario];

            if (objUsuario != null)
            {
                enviar.Usuario = objUsuario.DscNombres + " " + objUsuario.DscApePaterno + " " + objUsuario.DscApeMaterno;
            }

            enviar.Rol = Session[ConstanteSesion.RolDes].ToString();
            enviar.Sede = usuarioSession.SEDEDES;
            enviar.Area = usuarioSession.AREADES;
            try
            {
                enviar.EnviarCorreoVarios(dir.ToString(), logSolicitud.TipoEtapa, usuario.DscNombres, "Nuevo Cargo", logSolicitud.Observacion, solicitudNuevo.NombreCargo, ""+solicitudNuevo.IdeSolicitudNuevoCargo, Sends, "Suceso", Copys);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult ActualizarFechaExpiracion(string fechaExpiracion, string Observacion, string fechaInicio)
        { 
            JsonMessage objJsonMessage = new JsonMessage();
            fechaExpiracion = String.Format("{0:dd/MM/yyyy}", fechaExpiracion);
            DateTime fecha = Convert.ToDateTime(fechaExpiracion);
            fechaInicio = String.Format("{0:dd/MM/yyyy}", fechaInicio);
            DateTime fechaPublicacion = Convert.ToDateTime(fechaInicio);
            try
            {
                if (CargoPerfil != null)
                {
                    var solicitud = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                    solicitud.FechaExpiracion = fecha;
                    solicitud.FechaPublicacion = fechaPublicacion;
                    solicitud.ObservacionPublicacion = Observacion;
                    _solicitudNuevoCargoRepository.Update(solicitud);

                    objJsonMessage.Mensaje = "Solicitud actualizada correctamente";
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    objJsonMessage.Mensaje = "ERROR: no se pudo actualizar la fecha de expiración";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
 
                }
                    
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

            publicacionNuevoViewModel.visualizarCompetencias = Indicador.No;
            publicacionNuevoViewModel.visualizarDiscapacidad = Indicador.No;
            publicacionNuevoViewModel.visualizarOfrecemos = Indicador.No;

            var area = _areaRepository.GetSingle(x => x.IdeArea == cargo.IdeArea);
            if (area != null)
            {
                publicacionNuevoViewModel.Area = area.NombreArea;
            }
            else
            {
                publicacionNuevoViewModel.Area = "Error. verificar datos";
            }

            publicacionNuevoViewModel.Sede = Session[ConstanteSesion.SedeDes].ToString();

            var horario = _horarioCargoRepository.getMaxPuntValue(x => x.Cargo.IdeCargo == cargo.IdeCargo);
            publicacionNuevoViewModel.TipoHorario = horario.DescripcionHorario;

            string tipoRangoSalarial = cargo.TipoRangoSalarial;
            publicacionNuevoViewModel.RangoSalario = _detalleGeneralRepository.GetByTableDescription(TipoTabla.TipoSalario, tipoRangoSalarial);

            return publicacionNuevoViewModel;
        }


        /// <summary>
        /// obtiene la lista de Emails
        /// </summary>
        /// <param name="idSol">id de la solicitud</param>
        /// <param name="idRolSuceso">id del rol de la persona logueada</param>
        /// <param name="btnAccion">codigo de la accion del boton</param>
        /// <param name="idSede">id de la sede de la solicitud</param>
        /// <param name="TipoSol">tipo de solicitud</param>
        /// <returns></returns>
        public System.Collections.ArrayList listaEmail(int idSol, int idRolSuceso, string btnAccion, int idSede, string TipoSol)
        {
            EmailSol objEmailSol;
            List<EmailSol> listaRolxEmail;
            //List<EmailSol> listaEmialSend;
            //List<EmailSol> listaEmialCopy;

            List<String> listaSend;
            List<String> listaCopy;
            SolReqPersonal objSolReqPersonal;
            System.Collections.ArrayList ListaEmailEnvio = new System.Collections.ArrayList();


            objEmailSol = new EmailSol();
            listaRolxEmail = new List<EmailSol>();

            objEmailSol.IdSol = idSol;
            objEmailSol.IdRolSuceso = idRolSuceso;
            objEmailSol.TipSol = TipoSol;
            objEmailSol.AccionBoton = btnAccion;
            objEmailSol.idSede = idSede;

            //obtiene los roles de para  el envio de correo
            listaRolxEmail = _solReqPersonalRepository.GetRolxEmial(objEmailSol);
            listaSend = new List<String>();
            listaCopy = new List<String>();
            Boolean ind = false;

            string tipoReq = null;
            if (listaRolxEmail != null)
            {
                if (listaRolxEmail.Count > 0)
                {
                    foreach (EmailSol item in listaRolxEmail)
                    {
                        //obtiene la lista de send
                        ind = false;

                        if (item.RolSend != null)
                        {

                            if (item.RolSend.Equals("**"))
                            {
                                if (TipoSolicitud.Nuevo.Equals(TipoSol))
                                {
                                    var objSolNuevo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == idSol && x.EstadoActivo == IndicadorActivo.Activo);
                                    var idCargo = objSolNuevo.IdeCargo;

                                    var objCargo = _cargoRepository.GetSingle(x => x.IdeCargo == idCargo && x.EstadoActivo == IndicadorActivo.Activo);

                                    tipoReq = objCargo.TipoRequerimiento;
                                }
                                else
                                {
                                    var objSolReq = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == idSol && x.EstadoActivo == IndicadorActivo.Activo);
                                    if (objSolReq != null)
                                    {
                                        tipoReq = objSolReq.TipoRequerimiento;
                                    }


                                }

                                if (tipoReq != null)
                                {
                                    objSolReqPersonal = new SolReqPersonal();
                                    objSolReqPersonal = _solReqPersonalRepository.GetResponsable("U", idSede, tipoReq);
                                    var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == objSolReqPersonal.idUsuarioResp && x.FlgEstado == IndicadorActivo.Activo);

                                    ind = listaSend.Contains(objUsuario.Email);
                                    if (!ind)
                                    {
                                        listaSend.Add(objUsuario.Email);
                                    }
                                }

                            }
                            else
                            {

                                ind = listaSend.Contains(item.RolSend);
                                if (!ind)
                                {
                                    listaSend.Add(item.RolSend);
                                }

                            }



                        }

                        ind = false;
                        ind = listaCopy.Contains(item.RolCopy1);
                        if (!ind)
                        {
                            if (item.RolCopy1 != null && item.RolCopy1 != "")
                            {
                                listaCopy.Add(item.RolCopy1);
                            }
                        }

                        ind = false;
                        ind = listaCopy.Contains(item.RolCopy2);

                        if (!ind)
                        {
                            if (item.RolCopy2 != null && item.RolCopy2 != "")
                            {
                                listaCopy.Add(item.RolCopy2);
                            }
                        }

                        ind = false;
                        ind = listaCopy.Contains(item.RolCopy3);

                        if (!ind)
                        {
                            if (item.RolCopy3 != null && item.RolCopy3 != "")
                            {
                                listaCopy.Add(item.RolCopy3);
                            }
                        }



                        // obtiene la lista para las copias




                    }
                }
            }

            ListaEmailEnvio.Add(listaSend);
            ListaEmailEnvio.Add(listaCopy);
            return ListaEmailEnvio;

        }

    }
}
