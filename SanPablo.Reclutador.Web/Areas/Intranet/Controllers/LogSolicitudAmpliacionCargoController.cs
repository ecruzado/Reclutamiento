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
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IRolRepository _rolRepository;



        public LogSolicitudAmpliacionCargoController(ISolReqPersonalRepository solicitudAmpliacionCargoRepository,
                                                     ILogSolicitudRequerimientoRepository logSolicitudAmpliacionRepository,
                                                     IUsuarioRepository usuarioRepository,
                                                     ICargoRepository cargoRepository,
                                                     ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                                     ISolReqPersonalRepository solReqPersonalRepository,
                                                     IRolRepository rolRepository
            
            )
        {
            _solicitudAmpliacionCargoRepository = solicitudAmpliacionCargoRepository;
            _logSolicitudAmpliacionRepository = logSolicitudAmpliacionRepository;
            _usuarioRepository = usuarioRepository;
            _cargoRepository = cargoRepository;
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _rolRepository = rolRepository;
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
                var msjFinal = "";
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

                        if (aprobarRechazarSolicitud(model)) 
                        {
                            if (model.Aprobado)
                            {
                                var rol = _rolRepository.GetSingle(x=>x.IdRol == model.LogSolicitudAmpliacion.RolResponsable);
                                int idUsuarioResp = Convert.ToInt32(Session[ConstanteSesion.IdUsuario]);
                                msjFinal = "El proceso de envío se realizó exitosamente.";
                                msjFinal += "Derivado a " + rol.DscRol + " ";

                                if(idUsuarioResp != 0)
                                {
                                    var usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == idUsuarioResp);
                                    msjFinal += usuario.DscNombres +" "+usuario.DscApeMaterno;
                                }
                               
                            }
                            else
                            {
                                msjFinal = "Proceso registrado exitosamente. ";
                            }
                            objJsonMessage.Mensaje = msjFinal;
                            objJsonMessage.Resultado = true;
                            return Json(objJsonMessage);
                        }
                        else
                        {
                            objJsonMessage.Mensaje = "ERROR: Ocurrio un error al intentar enviar la Aprobación/Rechazo, intente de nuevo";
                            objJsonMessage.Resultado = false;
                            return Json(objJsonMessage);
                        }
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

                    var solicitudResp =  _solicitudAmpliacionCargoRepository.responsablePublicacion(ideSolicitud, solicitudAmpliacion.IdeSede);
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.UsResponsable = solicitudResp.UsResponsable;
                    break;

                default:
                    logSolicitudAmpCargoViewModel.LogSolicitudAmpliacion.TipEtapa = "-1";
                    break;
             }

            return logSolicitudAmpCargoViewModel;
        }


        /// <summary>
        /// Aprueba o rechaza solicitud de ampliacion
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool aprobarRechazarSolicitud(LogSolicitudAmpliacionCargoViewModel model)
        {
            SendMail enviar = new SendMail();
           
            SedeNivel usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            
            List<String> listSends=null;
            List<String> listCopys = null;

            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);

            var objUsuario = (Usuario)Session[ConstanteSesion.ObjUsuario];

            if (objUsuario != null)
            {
                enviar.Usuario = objUsuario.DscNombres + " " + objUsuario.DscApePaterno + " " + objUsuario.DscApeMaterno;
            }

            //enviar.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
            enviar.Rol = Session[ConstanteSesion.RolDes].ToString();
            enviar.Sede = Session[ConstanteSesion.SedeDes].ToString();
            enviar.Area = usuarioSession.AREADES;
            
            
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            
            int ideUsuarioResp;

            string msj = "";
            
            string indicadorArea="NO";
            System.Collections.ArrayList lista = new System.Collections.ArrayList();
            try
            {
                if (model.Aprobado)
                {
                    lista = listaEmail(Convert.ToInt32(model.SolicitudRequerimiento.IdeSolReqPersonal), idRol, AccionEnvioEmail.AprobarSolicitud, idSede, TipoSolicitud.Ampliacion);
                }
                else
                {
                    lista = listaEmail(Convert.ToInt32(model.SolicitudRequerimiento.IdeSolReqPersonal), idRol, AccionEnvioEmail.RechazarSolicitud, idSede, TipoSolicitud.Ampliacion);
                }
                
                
                listSends = new List<String>();
                listSends = (List<String>)lista[0];

                listCopys = new List<String>();
                listCopys = (List<String>)lista[1];



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
                Session[ConstanteSesion.IdUsuario] = ideUsuarioResp;
                if (ideUsuarioResp != -1)
                {
                    Usuario usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuarioResp);
                    var SedeDesc = Session[ConstanteSesion.SedeDes];
                    string SedeDescripcion = "";
                    if (SedeDesc != null)
                    {
                        SedeDescripcion = SedeDesc.ToString();
                    }
                    SolReqPersonal cargo = _solicitudAmpliacionCargoRepository.GetSingle(x => x.IdeSolReqPersonal == model.SolicitudRequerimiento.IdeSolReqPersonal);


                    enviar.EnviarCorreoVarios(dir.ToString(), model.LogSolicitudAmpliacion.TipEtapa, usuario.DscNombres, TipoRequerimientoEmail.Ampliacion, model.LogSolicitudAmpliacion.Observacion, cargo.nombreCargo, ""+model.SolicitudRequerimiento.IdeSolReqPersonal, listSends, "UN SUCESSO", listCopys);

                    return true;
                }
                else
                {
                    //msj = "ERROR: No se pudo completar la acción intente de nuevo";
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        #endregion
    }
}
