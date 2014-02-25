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

    public class LogSolicitudNuevoCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private ILogSolicitudNuevoCargoRepository _logSolicitudNuevoCargoRepository;
        private IUsuarioRepository _usuarioRepository;

        public LogSolicitudNuevoCargoController(ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                               ILogSolicitudNuevoCargoRepository logSolicitudNuevoCargoRepository,
                                                IUsuarioRepository usuarioRepository)
        {
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _logSolicitudNuevoCargoRepository = logSolicitudNuevoCargoRepository;
            _usuarioRepository = usuarioRepository;
        }

        #region LOGSOLICITUD NUEVO CARGO
        [ValidarSesion]
        public ActionResult Edit(string ideSolicitud)
        {
            var logSolicitudNuevoCargoViewModel = inicializarLogSolicitudNuevoCargo(Convert.ToInt32(ideSolicitud));
            return View(logSolicitudNuevoCargoViewModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit(LogSolicitudNuevoCargoViewModel model)
        {
           
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                LogSolicitudNuevoCargoValidator validator = new LogSolicitudNuevoCargoValidator();
                ValidationResult result = validator.Validate(model.LogSolicitudNuevoCargo, "Observacion");
                int RolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                var estadoSolicitud = _logSolicitudNuevoCargoRepository.estadoSolicitud(model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo);
                if (RolSession == Convert.ToInt32(estadoSolicitud.RolResponsable) )
                {
                    if (!result.IsValid)
                    {
                        objJsonMessage.Mensaje = "ERROR: Aprobación/Rechazo no enviado, intente de nuevo";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    aprobarRechazarNuevaSolicitud(model);

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
        public LogSolicitudNuevoCargoViewModel inicializarLogSolicitudNuevoCargo(int ideSolicitud)
        {
            var logSolicitudNuevoCargoViewModel = new LogSolicitudNuevoCargoViewModel();
            logSolicitudNuevoCargoViewModel.SolicitudNuevoCargo = new SolicitudNuevoCargo();
            
            logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo = new LogSolicitudNuevoCargo();
            logSolicitudNuevoCargoViewModel.SolicitudNuevoCargo.IdeSolicitudNuevoCargo = ideSolicitud;

            logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.Observacion = "";

            var estadoSolicitud = _logSolicitudNuevoCargoRepository.estadoSolicitud(ideSolicitud);
            if (estadoSolicitud.TipoSuceso == SucesoSolicitud.Pendiente)
            {
                switch (estadoSolicitud.TipoEtapa)
                {
                    case EtapasSolicitud.PendienteAprobacionGerenteArea :
                        logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.TipoEtapa = EtapasSolicitud.PendienteAprobacionGerenteGralAdj;
                        break;

                    case EtapasSolicitud.PendienteAprobacionGerenteGralAdj:
                        logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.TipoEtapa = EtapasSolicitud.PendienteElaboracionPerfil;
                        break;

                    case EtapasSolicitud.PendienteElaboracionPerfil:
                        logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.TipoEtapa = EtapasSolicitud.PendienteAprobacionPerfilJefeArea;
                        break;

                    case EtapasSolicitud.PendienteAprobacionPerfilJefeArea:
                        logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.TipoEtapa = EtapasSolicitud.PendienteAprobacionPerfilEncargSeleccion;
                        break;

                    case EtapasSolicitud.PendienteAprobacionPerfilEncargSeleccion:
                        logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.TipoEtapa = EtapasSolicitud.PendientePublicacion;
                        break;
                }
            }

            return logSolicitudNuevoCargoViewModel;
        }

        public void aprobarRechazarNuevaSolicitud(LogSolicitudNuevoCargoViewModel model)
        {
            SendMail enviar = new SendMail();
            enviar.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
            enviar.Rol = Session[ConstanteSesion.RolDes].ToString();
            enviar.Sede = Session[ConstanteSesion.SedeDes].ToString();
            enviar.Area = "AREA 1";
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            int ideUsuarioResp;
            try
            {
                
                if (model.Aprobado)
                {
                    model.LogSolicitudNuevoCargo.TipoSuceso = SucesoSolicitud.Aprobado;
                    model.LogSolicitudNuevoCargo.Observacion = "";
                }
                else
                {
                    model.LogSolicitudNuevoCargo.TipoSuceso = SucesoSolicitud.Rechazado;
                   
                }
                LogSolicitudNuevoCargo logSolicitudNuevo = model.LogSolicitudNuevoCargo;
                int IdeSolicitudNuevoCargo = model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo;
                ideUsuarioResp = _logSolicitudNuevoCargoRepository.solicitarAprobacion(Convert.ToInt32(Session[ConstanteSesion.Sede]), model.SolicitudNuevoCargo.IdeArea, model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo, Convert.ToInt32(Session[ConstanteSesion.Usuario]), Convert.ToInt32(Session[ConstanteSesion.Rol]),
                                                                                       model.LogSolicitudNuevoCargo.Observacion, model.LogSolicitudNuevoCargo.TipoSuceso, model.LogSolicitudNuevoCargo.TipoEtapa);

                if (ideUsuarioResp != 0)
                {
                    Usuario usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuarioResp);
                    var SedeDesc = Session[ConstanteSesion.SedeDes];
                    string SedeDescripcion = "";
                    if (SedeDesc != null)
                    {
                        SedeDescripcion = SedeDesc.ToString();
                    }

                    enviar.EnviarCorreo(dir.ToString(), model.LogSolicitudNuevoCargo.TipoEtapa, SedeDescripcion, usuario.DscNombres, "NUEVO CARGO", model.LogSolicitudNuevoCargo.Observacion, model.SolicitudNuevoCargo.NombreCargo, model.SolicitudNuevoCargo.CodigoCargo, usuario.Email,model.LogSolicitudNuevoCargo.TipoSuceso);
                }

            }
            catch (Exception ex)
            {
                //manejar el error
            }
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult AprobacionRechazoPerfil(LogSolicitudNuevoCargoViewModel model)
        {

            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                LogSolicitudNuevoCargoValidator validator = new LogSolicitudNuevoCargoValidator();
                ValidationResult result = validator.Validate(model.LogSolicitudNuevoCargo, "Observacion");
                var RolSession = Convert.ToString(Session[ConstanteSesion.Rol]);
                var estadoSolicitud = _logSolicitudNuevoCargoRepository.estadoSolicitud(model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo);
                if(estadoSolicitud.RolResponsable == RolSession)
                {
                    if (!result.IsValid)
                    {
                        objJsonMessage.Mensaje = "ERROR: No se ha enviado la aprobacion intente de nuevo";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    aprobarRechazarNuevaSolicitud(model);

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
