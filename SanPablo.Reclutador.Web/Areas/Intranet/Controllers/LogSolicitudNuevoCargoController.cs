﻿namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
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

        public LogSolicitudNuevoCargoController(ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                               ILogSolicitudNuevoCargoRepository logSolicitudNuevoCargoRepository)
        {
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _logSolicitudNuevoCargoRepository = logSolicitudNuevoCargoRepository;
        }

        #region LOGSOLICITUD NUEVO CARGO

        public ActionResult Edit(string ideSolicitud)
        {

            var logSolicitudNuevoCargoViewModel = inicializarLogSolicitudNuevoCargo(Convert.ToInt32(ideSolicitud));
            return View(logSolicitudNuevoCargoViewModel);
        }

        [HttpPost]
        public ActionResult Edit(LogSolicitudNuevoCargoViewModel model)
        {
           
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                LogSolicitudNuevoCargoValidator validator = new LogSolicitudNuevoCargoValidator();
                ValidationResult result = validator.Validate(model.LogSolicitudNuevoCargo, "Observacion");
                
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

            return logSolicitudNuevoCargoViewModel;
        }

        public void aprobarRechazarNuevaSolicitud(LogSolicitudNuevoCargoViewModel model)
        {
            SendMail enviar = new SendMail();
            try
            {
                LogSolicitudNuevoCargo logSolicitudNuevo = model.LogSolicitudNuevoCargo;
                LogSolicitudNuevoCargo logSiguiente = new LogSolicitudNuevoCargo();
                int IdeSolicitudNuevoCargo = model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo;
                //recuperar datos del estado actual
                var logSolicitud = _logSolicitudNuevoCargoRepository.getMostRecentValue(x => x.IdeSolicitudNuevoCargo == IdeSolicitudNuevoCargo);
                string RolActual = "";
                if (logSolicitudNuevo.RolResponsable == RolActual)
                {
                    if (logSolicitudNuevo.TipoSuceso == EstadoSolicitud.Pendiente)
                    {
                        //determinar el usuario y el estado de la solicitud
                        logSolicitudNuevo.FechaSuceso = FechaModificacion;
                        logSolicitudNuevo.UsuarioSuceso = UsuarioActual.CodUsuario;
                        
                        if (model.Aprobado)
                        {
                            logSolicitudNuevo.TipoSuceso = EstadoSolicitud.Aprobado;
                            _logSolicitudNuevoCargoRepository.Update(logSolicitudNuevo);
                            enviar.EnviarCorreo(Asunto.Aprobacion, AccionMail.Aprobacion, true, Solicitud.Nuevo);
                        }
                        else
                        {
                            logSolicitudNuevo.TipoSuceso = EstadoSolicitud.Rechazado;
                            _logSolicitudNuevoCargoRepository.Update(logSolicitudNuevo);
                            enviar.EnviarCorreo(Asunto.Rechazo, AccionMail.Rechazo, true, Solicitud.Nuevo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //manejar el error
            }
        }
        
        
        #endregion
    }
}
