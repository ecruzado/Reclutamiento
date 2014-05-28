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
        private ICargoRepository _cargoRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;

        public LogSolicitudNuevoCargoController(ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                               ILogSolicitudNuevoCargoRepository logSolicitudNuevoCargoRepository,
                                                IUsuarioRepository usuarioRepository,
            ISolReqPersonalRepository solReqPersonalRepository,
            ICargoRepository cargoRepository
            )
        {
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _logSolicitudNuevoCargoRepository = logSolicitudNuevoCargoRepository;
            _usuarioRepository = usuarioRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _cargoRepository = cargoRepository;
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

            if ((solicitud.TipoEtapa == Etapa.Generacion_Perfil) || (solicitud.TipoEtapa == Etapa.Observado))
            {
                logSolicitudNuevoCargoViewModel.rechazadoObservado = "Observar";
            }
            else
            {
                logSolicitudNuevoCargoViewModel.rechazadoObservado = "Rechazar";
            }

            logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.UsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            logSolicitudNuevoCargoViewModel.LogSolicitudNuevoCargo.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);

           

            return logSolicitudNuevoCargoViewModel;
        }


        //Realiza la aprobacion de la solictu de nuevo cargo
        [ValidarSesion]
        public string aprobarRechazarNuevaSolicitud(LogSolicitudNuevoCargoViewModel model)
        {
            int ideUsuarioResp;
            LogSolicitudNuevoCargo logSolicitud = model.LogSolicitudNuevoCargo;
            string AccionAprobacion;
            string AccionRechazo;
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
            List<String>  listSends = null;
            List<String> listCopys = null;

            if (model.indPantalla==1)
            {
                AccionAprobacion = AccionEnvioEmail.AprobarPerfil;
                AccionRechazo = AccionEnvioEmail.ObservarPerfil;
            }
            else
            {
                AccionAprobacion = AccionEnvioEmail.AprobarSolicitud;
                AccionRechazo = AccionEnvioEmail.RechazarSolicitud;
            }

            logSolicitud.UsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            logSolicitud.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            logSolicitud.IdeSolicitudNuevoCargo = model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo;

           
            var solicitud = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo);
            
            //obtiene los correos
            System.Collections.ArrayList lista = new System.Collections.ArrayList();

            if (logSolicitud.Aprobado)
	        {
                lista = listaEmail(Convert.ToInt32(solicitud.IdeSolicitudNuevoCargo), idRol, AccionAprobacion, idSede, TipoSolicitud.Nuevo);
	        }else
	        {
                lista = listaEmail(Convert.ToInt32(solicitud.IdeSolicitudNuevoCargo), idRol, AccionRechazo, idSede, TipoSolicitud.Nuevo);
	        }
            
            listSends = new List<String>();
            listSends = (List<String>)lista[0];

            listCopys = new List<String>();
            listCopys = (List<String>)lista[1];
            //

            string indArea = "NO";
            try
            {
               
                var estadoSolicitud = solicitud.TipoEtapa;
                string rol ="";
                switch (estadoSolicitud)
                {
                    case Etapa.Pendiente:
                        logSolicitud.TipoEtapa = Etapa.Validado;
                        logSolicitud.RolResponsable = Roles.Gerente_General_Adjunto;
                        logSolicitud.UsuarioResponsable = 0;
                        rol = "Gerente General Adjunto";
                        break;

                    case Etapa.Validado:
                        logSolicitud.TipoEtapa = Etapa.Aprobado;
                        logSolicitud.RolResponsable = Roles.Jefe_Corporativo_Seleccion;
                        logSolicitud.UsuarioResponsable = 0;
                        rol = "Jefe Corporativo de Selección";
                        break;

                    case Etapa.Generacion_Perfil:
                        if (logSolicitud.Aprobado == true)
                        {
                            logSolicitud.TipoEtapa = Etapa.Aprobacion_Perfil;
                            logSolicitud.RolResponsable = Roles.Encargado_Seleccion;
                            logSolicitud.UsuarioResponsable = 0;
                            rol="Encargado de Selección";
                            break;
                        }
                        else
                        {
                            logSolicitud.TipoEtapa = Etapa.Observado;
                            logSolicitud.RolResponsable = Roles.Jefe_Corporativo_Seleccion;
                            logSolicitud.UsuarioResponsable = 0;
                            rol = "Jefe Corporativo de Selección";
                            break;
                        }
                    case Etapa.Aprobacion_Perfil:
                        logSolicitud.TipoEtapa = Etapa.Aceptado;
                        var logSolResponsable = _solicitudNuevoCargoRepository.responsablePublicacion(solicitud.IdeSolicitudNuevoCargo, solicitud.IdeSede);
                        logSolicitud.RolResponsable = logSolResponsable.RolResponsable;
                        logSolicitud.UsuarioResponsable = logSolResponsable.UsuarioResponsable;
                        break;

                    //case Etapa.Observado:
                    //    logSolicitud.TipoEtapa = Etapa.Aprobacion_Perfil;
                    //    logSolicitud.RolResponsable = Roles.Encargado_Seleccion;
                    //    break;
                }
                if (logSolicitud.Aprobado)
                {
                    logSolicitud.Observacion = "";
                }
                else
                {
                    if (logSolicitud.TipoEtapa != Etapa.Observado)
                    {
                        logSolicitud.TipoEtapa = Etapa.Rechazado;

                        if (logSolicitud.Observacion == null)
                        {
                            logSolicitud.Observacion = "";
                        }

                        //Regresa la solicitud al solicitante en caso de rechazo
                        //var logSolicitudInicial = _logSolicitudNuevoCargoRepository.getFirthValue(x => x.IdeSolicitudNuevoCargo == solicitud.IdeSolicitudNuevoCargo);

                        //logSolicitud.RolResponsable = logSolicitudInicial.RolSuceso;
                        //logSolicitud.UsuarioResponsable = logSolicitudInicial.UsuarioResponsable;

                        //if ((logSolicitud.RolResponsable == Roles.Jefe) || (logSolicitud.RolResponsable == Roles.Gerente))
                        //{
                        //    indArea = "SI";
                        //}
                        indArea = "NO";
                       
                    }
                }

                var solicitudNuevo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == model.SolicitudNuevoCargo.IdeSolicitudNuevoCargo);

                ideUsuarioResp = _logSolicitudNuevoCargoRepository.solicitarAprobacion(logSolicitud, solicitudNuevo.IdeSede, solicitudNuevo.IdeArea, indArea);

                if (ideUsuarioResp != -1)
                {
                    if (!logSolicitud.Aprobado)
                    {
                        var logSolicitudInicial = _logSolicitudNuevoCargoRepository.getFirthValue(x => x.IdeSolicitudNuevoCargo == solicitud.IdeSolicitudNuevoCargo);
                        ideUsuarioResp = logSolicitudInicial.UsuarioResponsable;
                    }
                    
                    Usuario usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuarioResp);

                    if (enviarCorreoAll(model.LogSolicitudNuevoCargo, usuario, solicitud,listSends,listCopys))
                    {
                        
                        string menj = "El proceso de envío se realizó exitosamente";
                               menj += Environment.NewLine;
                               menj += "Solicitud derivada a " + rol +" "+usuario.DscNombres+" "+usuario.DscApePaterno;
                            return menj;
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

                    //indica que la pantalla es de perfil
                    model.indPantalla = 1;

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


        //realiza el envio de email
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
                enviar.EnviarCorreo(dir.ToString(), logSolicitud.TipoEtapa, usuario.DscNombres,"Nuevo Cargo", logSolicitud.Observacion, solicitudNuevo.NombreCargo, solicitudNuevo.CodigoCargo, usuario.Email, "Suceso");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Envia el correo a todos 
        /// </summary>
        /// <param name="logSolicitud"></param>
        /// <param name="usuario"></param>
        /// <param name="solicitudNuevo"></param>
        /// <param name="Sends"></param>
        /// <param name="Copys"></param>
        /// <returns></returns>
        public bool enviarCorreoAll(LogSolicitudNuevoCargo logSolicitud, Usuario usuario, SolicitudNuevoCargo solicitudNuevo,List<String> Sends,List<String> Copys)
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
                enviar.EnviarCorreoVarios(dir.ToString(), logSolicitud.TipoEtapa, usuario.DscNombres, "Nuevo Cargo", logSolicitud.Observacion, solicitudNuevo.NombreCargo, solicitudNuevo.CodigoCargo, Sends, "Suceso",Copys);
                return true;
            }
            catch (Exception)
            {
                return false;
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
