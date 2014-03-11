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
                    string resultado = aprobarRechazarNuevaSolicitud(model);

                    objJsonMessage.Mensaje = resultado;
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
            var solicitud = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == ideSolicitud);
            logSolicitudNuevoCargoViewModel.SolicitudNuevoCargo = solicitud;

            logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.Observacion = "";

            logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.UsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);

           

            return logSolicitudNuevoCargoViewModel;
        }

        public string aprobarRechazarNuevaSolicitud(LogSolicitudNuevoCargoViewModel model)
        {
            int ideUsuarioResp;
            LogSolicitudNuevoCargo logSolicitud = model.LogSolicitudNuevoCargo;

            logSolicitud.UsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            logSolicitud.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            logSolicitud.IdeSolicitudNuevoCargo = model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo;

            var solicitud = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo);
            
            string indArea = "NO";
            try
            {
               
                var estadoSolicitud = solicitud.TipoEtapa;
                switch (estadoSolicitud)
                {
                    case Etapa.Pendiente:
                        logSolicitud.TipoEtapa = Etapa.Validado;
                        logSolicitud.RolResponsable = Roles.Gerente_General_Adjunto;
                        break;

                    case Etapa.Validado:
                        logSolicitud.TipoEtapa = Etapa.Aprobado;
                        logSolicitud.RolResponsable = Roles.Jefe_Corporativo_Seleccion;
                        break;

                    case Etapa.Generacion_Perfil:
                        logSolicitud.TipoEtapa = Etapa.Aprobacion_Perfil;
                        logSolicitud.RolResponsable = Roles.Analista_Seleccion;//identificar destiatario
                        break;

                    case Etapa.Observado:
                        logSolicitud.TipoEtapa = Etapa.Aprobacion_Perfil;
                        logSolicitud.RolResponsable = Roles.Jefe_Corporativo_Seleccion;
                        break;

                }
                if (model.Aprobado)
                {
                    logSolicitud.Observacion = "";
                }
                else
                {
                    logSolicitud.TipoEtapa = Etapa.Rechazado;
                    logSolicitud.RolResponsable = Roles.Jefe;
                    indArea = "SI";
                }


                var solicitudNuevo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo);

                ideUsuarioResp = _logSolicitudNuevoCargoRepository.solicitarAprobacion(logSolicitud, solicitudNuevo.IdeSede, solicitudNuevo.IdeArea, indArea);

                if (ideUsuarioResp != -1)
                {
                    Usuario usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuarioResp);

                    if (enviarCorreo(model.LogSolicitudNuevoCargo, usuario, model.SolicitudNuevoCargo))
                    {
                        return "Se envio la Aprobación correctamente";
                    }
                    else
                    {
                        return "ERROR: No se pudo enviar el correo al responsable";
                    }
                }
                else
                {
                    return "ERROR: No se pudo realizar la aprobacion intente de nuevo";
                }

            }
            catch (Exception ex)
            {
                return "ERROR"+ex;
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
                var RolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                var estadoSolicitud = _logSolicitudNuevoCargoRepository.estadoSolicitud(model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo);
                if(estadoSolicitud.RolResponsable == RolSession)
                {
                    if (!result.IsValid)
                    {
                        objJsonMessage.Mensaje = "ERROR: verifique los datos ingresados";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    string resultado = aprobarRechazarNuevaSolicitud(model);

                    objJsonMessage.Mensaje = resultado;
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

        public bool enviarCorreo(LogSolicitudNuevoCargo logSolicitud, Usuario usuario, SolicitudNuevoCargo solicitudNuevo)
        {
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            SedeNivel usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            SendMail enviar = new SendMail();
            enviar.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
            enviar.Rol = Session[ConstanteSesion.RolDes].ToString();
            enviar.Sede = usuarioSession.SEDEDES;
            enviar.Area = usuarioSession.AREADES;
            try
            {
                enviar.EnviarCorreo(dir.ToString(), logSolicitud.TipoEtapa, usuario.DscNombres, "NUEVO CARGO", logSolicitud.Observacion, solicitudNuevo.NombreCargo, solicitudNuevo.CodigoCargo, usuario.Email, "Suceso");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
