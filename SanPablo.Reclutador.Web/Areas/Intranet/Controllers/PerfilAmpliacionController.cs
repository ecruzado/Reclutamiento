namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
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

    [Authorize]
    public class PerfilAmpliacionController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ILogSolicitudNuevoCargoRepository _logSolicitudNuevoRepository;
        private ISolReqPersonalRepository _solicitudAmpliacionRepository;
        private IUsuarioRepository _usuarioRepository;

        public PerfilAmpliacionController(IDetalleGeneralRepository detalleGeneralRepository,
                                          ILogSolicitudNuevoCargoRepository logSolicitudNuevoRepository,
                                          ISolReqPersonalRepository solicitudAmpliacionRepository,
                                          IUsuarioRepository usuarioRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _logSolicitudNuevoRepository = logSolicitudNuevoRepository;
            _solicitudAmpliacionRepository = solicitudAmpliacionRepository;
            _usuarioRepository = usuarioRepository;
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
                    var cargoAmpliacion = _solicitudAmpliacionRepository.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion);
                    perfilAmpliacionViewModel.SolicitudRequerimiento = cargoAmpliacion;
                }
                
                return View(perfilAmpliacionViewModel);
            }
            catch (Exception)
            {
                //return View(perfilAmpliacionViewModel);
                return View();
            }
            
        }

        
        public PerfilAmpliacionViewModel inicializarPerfil()
        {
            var cargoViewModel = new PerfilAmpliacionViewModel();
            cargoViewModel.SolicitudRequerimiento = new SolReqPersonal();
            

            return cargoViewModel;
        }

        public ActionResult General()
        {
            var perfilAmpliacionViewModel = inicializarGeneral();
            if (CargoPerfil != null)
            {
                var solicitudAmpliacion = _solicitudAmpliacionRepository.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion);
                perfilAmpliacionViewModel.SolicitudRequerimiento = solicitudAmpliacion;
            }
            
            return View(perfilAmpliacionViewModel);
        }
     

        public PerfilAmpliacionViewModel inicializarGeneral()
        {
            var cargoViewModel = new PerfilAmpliacionViewModel();
            cargoViewModel.SolicitudRequerimiento = new SolReqPersonal();

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
            //if (CargoPerfil != null)
            //{
            //    var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
            //    actualizarDatosCargo(estudioCargoViewModel, cargo);
            //}
            return View(estudioCargoViewModel);
        }
        
        public ActionResult Experiencia()
        {

            var experienciaCargoViewModel = inicializarDatosCargo();
            //if (CargoPerfil != null)
            //{
            //    var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
            //    actualizarDatosCargo(experienciaCargoViewModel,cargo);
            //}
            return View(experienciaCargoViewModel);
        }

        public ActionResult Conocimientos()
        {
            var conocimientosCargoViewModel = inicializarDatosCargo();
            //if (CargoPerfil != null)
            //{
            //    var cargo = _solicitudAmpliacionRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
            //   // actualizarDatosCargo(conocimientosCargoViewModel, cargo);
            //}
            return View(conocimientosCargoViewModel);
        }

        public ActionResult Discapacidad()
        {
            var discapacidadCargoViewModel = inicializarDatosCargo();
            //if (CargoPerfil != null)
            //{
            //    var cargo = _solicitudAmpliacionRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
            //    //actualizarDatosCargo(discapacidadCargoViewModel,cargo);
            //}
            return View(discapacidadCargoViewModel);
        }

        public ActionResult ConfiguracionPerfil()
        {
            int IdeCargo = CargoPerfil.IdeCargo; 
            var discapacidadCargoViewModel = inicializarDatosConfig(IdeSolicitudAmpliacion);
            //if (CargoPerfil != null)
            //{
            //    var cargo = _solicitudAmpliacionRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
            //    //actualizarDatosCargo(discapacidadCargoViewModel,cargo);
            //}
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
            var solicitudActual = _solicitudAmpliacionRepository.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitud);
            solicitudViewModel.SolicitudRequerimiento = solicitudActual;
            return solicitudViewModel;
        }

        

        public ActionResult Evaluacion()
        {
            var evaluacionCargoViewModel = inicializarDatosCargo();
            //if (CargoPerfil != null)
            //{
            //    var cargo = _solicitudAmpliacionRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
            //    //actualizarDatosCargo(evaluacionCargoViewModel,cargo);
            //}
            return View(evaluacionCargoViewModel);
        }

       
        //[HttpPost]
        //public ActionResult enviarPerfil()
        //{
        //    JsonMessage objJsonMessage = new JsonMessage();
        //    var enviarMail = new SendMail();
        //    int IdeCargo = CargoPerfil.IdeCargo;
        //    var cargoEnviar = _cargoRepository.GetSingle(x=>x.IdeCargo == IdeCargo);
        //    var solicitud = _solicitudNuevoCargo.GetSingle(x=>x.CodigoCargo==cargoEnviar.CodigoCargo);
        //    var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
        //    var SedeSession = Session[ConstanteSesion.Sede];
        //    int Area = 1;
        //    string SedeDescripcion = "-";
        //    if (SedeSession != null)
        //    {
        //        SedeDescripcion = Session[ConstanteSesion.SedeDes].ToString();   
        //    }
        //    try
        //    {
        //        LogSolicitudNuevoCargo estadoSolicitud = _logSolicitudNuevoRepository.estadoSolicitud(solicitud.IdeSolicitudNuevoCargo);
        //        if ((estadoSolicitud.TipoEtapa == EtapasSolicitud.PendienteElaboracionPerfil) && (estadoSolicitud.TipoSuceso == SucesoSolicitud.Pendiente) && (estadoSolicitud.RolResponsable == Convert.ToString(Session[ConstanteSesion.Rol])))
        //        {

        //            int ideUsuario = _logSolicitudNuevoRepository.solicitarAprobacion(solicitud, Convert.ToInt32(Session[ConstanteSesion.Usuario]), Convert.ToInt32(Session[ConstanteSesion.Rol]), "", SucesoSolicitud.Aprobado, EtapasSolicitud.PendienteAprobacionPerfilJefeArea);
        //            if (ideUsuario != 0)
        //            {
        //                var usuarioResp = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuario);
        //                enviarMail.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
        //                enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();
        //                enviarMail.Sede = SedeDescripcion;
        //                enviarMail.Area = "AREA1";
        //                enviarMail.EnviarCorreo(dir.ToString(), EtapasSolicitud.PendienteAprobacionPerfilJefeArea, SedeDescripcion,"RESPONSABLE", "Nuevo Cargo", "", cargoEnviar.NombreCargo, cargoEnviar.CodigoCargo, usuarioResp.Email, SucesoSolicitud.Pendiente);

        //                objJsonMessage.Mensaje = "Perfil enviado para su aprobación";
        //                objJsonMessage.Resultado = true;
        //                return Json(objJsonMessage);
        //            }
        //            else
        //            {
        //                objJsonMessage.Mensaje = "ERROR: no se pudo enviar la solicitud intente de nuevo";
        //                objJsonMessage.Resultado = false;
        //                return Json(objJsonMessage);
        //            }
        //        }
        //        else
        //        {
        //            objJsonMessage.Mensaje = "ERROR no tiene permisos para la accion o el estado de la solicitud no requiere esta accion" ;
        //            objJsonMessage.Resultado = false;
        //            return Json(objJsonMessage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objJsonMessage.Mensaje = "ERROR:" + ex.Message;
        //        objJsonMessage.Resultado = false;
        //        return Json(objJsonMessage);
        //    }
        //}

        

    }
}
