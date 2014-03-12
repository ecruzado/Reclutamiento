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

    public class LogSolicitudAmpliacionCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ISolReqPersonalRepository _solicitudAmpliacionCargoRepository;
        private ILogSolicitudRequerimientoRepository _logSolicitudAmpliacionRepository;
        private IUsuarioRepository _usuarioRepository;
        private ICargoRepository _cargoRepository;

        public LogSolicitudAmpliacionCargoController(ISolReqPersonalRepository solicitudAmpliacionCargoRepository,
                                                     ILogSolicitudRequerimientoRepository logSolicitudAmpliacionRepository,
                                                     IUsuarioRepository usuarioRepository,
                                                     ICargoRepository cargoRepository)
        {
            _solicitudAmpliacionCargoRepository = solicitudAmpliacionCargoRepository;
            _logSolicitudAmpliacionRepository = logSolicitudAmpliacionRepository;
            _usuarioRepository = usuarioRepository;
            _cargoRepository = cargoRepository;
        }

        #region LOGSOLICITUD AMPLIACION CARGO
        [ValidarSesion]
        public ActionResult Edit(string ideSolicitud)
        {
            var logSolicitudAmpliacionCargoViewModel = inicializarLogSolicitudAmpliacionCargo(Convert.ToInt32(ideSolicitud));
            return View(logSolicitudAmpliacionCargoViewModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit(LogSolicitudAmpliacionCargoViewModel model)
        {
           
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                LogSolicitudRequerimientoValidator validator = new LogSolicitudRequerimientoValidator();
                ValidationResult result = validator.Validate(model.LogSolicitudAmpliacion, "Observacion");
                int RolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                int ideSolicitud = Convert.ToInt32(model.SolicitudRequerimiento.IdeSolReqPersonal);
                LogSolReqPersonal estadoSolicitud = _logSolicitudAmpliacionRepository.estadoSolicitud(ideSolicitud);
                if (model.LogSolicitudAmpliacion.TipEtapa != "-1")
                {
                    if (RolSession == Convert.ToInt32(estadoSolicitud.RolResponsable))
                    {
                        if (!result.IsValid)
                        {
                            objJsonMessage.Mensaje = "ERROR: Aprobación/Rechazo no enviado, intente de nuevo";
                            objJsonMessage.Resultado = false;
                            return Json(objJsonMessage);
                        }
                        aprobarRechazarSolicitud(model);

                        objJsonMessage.Mensaje = "Enviado Correctamente";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    objJsonMessage.Mensaje = "Usuario sin permiso para esta accion";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
                else
                {
                    objJsonMessage.Mensaje = "La solicitud no tiene pendiente ninguna Aprobación";
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

        /// <summary>
        /// iniciar el log con datos necesarios
        /// </summary>
        /// <param name="ideSolicitud"></param>
        /// <returns></returns>
        public LogSolicitudAmpliacionCargoViewModel inicializarLogSolicitudAmpliacionCargo(int ideSolicitud)
        {
            var logSolicitudAmpCargoViewModel = new LogSolicitudAmpliacionCargoViewModel();
            logSolicitudAmpCargoViewModel.SolicitudRequerimiento = new SolReqPersonal();

            logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion = new LogSolReqPersonal();
            var solicitud = _solicitudAmpliacionCargoRepository.GetSingle(x => x.IdeSolReqPersonal == ideSolicitud);
            logSolicitudAmpCargoViewModel.SolicitudRequerimiento = solicitud;

            logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.Observacion = "";

            var estadoSolicitud = _solicitudAmpliacionCargoRepository.GetSingle(x => x.IdeSolReqPersonal == ideSolicitud);

            logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.UsrSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);

            SolReqPersonal solicitudAmpliacion = _solicitudAmpliacionCargoRepository.GetSingle(x => x.IdeSolReqPersonal == ideSolicitud);
            switch (estadoSolicitud.TipEtapa)
            {
                case Etapa.Pendiente:
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.TipEtapa = Etapa.Validado;
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.RolResponsable = Roles.Gerente_General_Adjunto;
                    break;

                case Etapa.Validado:
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.TipEtapa = Etapa.Aprobado;
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.RolResponsable = Roles.Encargado_Seleccion;
                    break;

                case Etapa.Aprobado:
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.TipEtapa = Etapa.Aceptado;
                    if (solicitud.TipoRequerimiento == TipoRequerimientos.jefe_corporativo_administrativo)
                    {
                        logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.RolResponsable = Roles.Encargado_Seleccion;
                    }
                    else
                    {
                        logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.RolResponsable = Roles.Analista_Seleccion;
                    }
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.UsResponsable = _solicitudAmpliacionCargoRepository.responsablePublicacion(solicitudAmpliacion.IdeCargo, solicitudAmpliacion.IdeSede);
                    break;

                default:
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.TipEtapa = "-1";
                    break;
             }

            return logSolicitudAmpCargoViewModel;
        }

        public void aprobarRechazarSolicitud(LogSolicitudAmpliacionCargoViewModel model)
        {
            SendMail enviar = new SendMail();
           
            SedeNivel usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            
            enviar.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
            enviar.Rol = Session[ConstanteSesion.RolDes].ToString();
            enviar.Sede = Session[ConstanteSesion.SedeDes].ToString();
            enviar.Area = usuarioSession.AREADES;
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            
            int ideUsuarioResp;
            
            string indicadorArea="NO";
            
            try
            {
                
                if (model.Aprobado)
                {
                    model.LogSolicitudAmpliacion.Observacion = "";
                }
                else
                {
                    model.LogSolicitudAmpliacion.TipEtapa = Etapa.Rechazado;

                    var solicitudInicial = _logSolicitudAmpliacionRepository.getFirthValue(x => x.IdeSolReqPersonal == model.SolicitudRequerimiento.IdeSolReqPersonal);

                    model.LogSolicitudAmpliacion.RolResponsable = solicitudInicial.RolSuceso;

                    if (solicitudInicial.RolSuceso == Roles.Jefe)
                    {
                        indicadorArea = "SI";
                    }
                }
                LogSolReqPersonal logSolicitudAmpliacion = model.LogSolicitudAmpliacion;
                int IdeSolicitud = model.LogSolicitudAmpliacion.IdeSolReqPersonal;

                ideUsuarioResp = _logSolicitudAmpliacionRepository.solicitarAprobacion(model.LogSolicitudAmpliacion, (Int32)model.SolicitudRequerimiento.IdeSolReqPersonal, model.SolicitudRequerimiento.IdeSede, model.SolicitudRequerimiento.IdeArea, indicadorArea);

                if (ideUsuarioResp != 0)
                {
                    Usuario usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuarioResp);
                    var SedeDesc = Session[ConstanteSesion.SedeDes];
                    string SedeDescripcion = "";
                    if (SedeDesc != null)
                    {
                        SedeDescripcion = SedeDesc.ToString();
                    }
                    SolReqPersonal cargo = _solicitudAmpliacionCargoRepository.GetSingle(x=>x.IdeSolReqPersonal == model.SolicitudRequerimiento.IdeSolReqPersonal);
                    enviar.EnviarCorreo(dir.ToString(), model.LogSolicitudAmpliacion.TipEtapa, usuario.DscNombres, TipoSolicitud.Ampliacion, model.LogSolicitudAmpliacion.Observacion, cargo.nombreCargo, cargo.CodSolReqPersonal, usuario.Email, "UN SUCESSO");
                }

            }
            catch (Exception ex)
            {
                //manejar el error
            }
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult AprobacionRechazoPerfil(LogSolicitudAmpliacionCargoViewModel model)
        {

            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                LogSolicitudRequerimientoValidator validator = new LogSolicitudRequerimientoValidator();
                ValidationResult result = validator.Validate(model.LogSolicitudAmpliacion, "Observacion");
                var RolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                //obtener el ultimo pendiente
                var estadoSolicitud = _logSolicitudAmpliacionRepository.GetSingle(x=>x.IdeLogSolReqPersonal==model.SolicitudRequerimiento.IdeSolReqPersonal);
                if(estadoSolicitud.RolResponsable == RolSession)
                {
                    if (!result.IsValid)
                    {
                        objJsonMessage.Mensaje = "ERROR: No se ha enviado la aprobacion intente de nuevo";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    aprobarRechazarSolicitud(model);

                    objJsonMessage.Mensaje = "Enviado Correctamente";
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);

                }
                objJsonMessage.Mensaje = "Usuario sin permiso para esta accion";
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);

            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }

        }
        #endregion
    }
}
