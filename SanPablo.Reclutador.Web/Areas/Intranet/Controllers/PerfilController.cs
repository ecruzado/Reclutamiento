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
    public class PerfilController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ILogSolicitudNuevoCargoRepository _logSolicitudNuevoRepository;
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private IUsuarioRepository _usuarioRepository;
       
        public PerfilController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                ILogSolicitudNuevoCargoRepository logSolicitudNuevoRepository,
                                ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                IUsuarioRepository usuarioRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _logSolicitudNuevoRepository = logSolicitudNuevoRepository;
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _usuarioRepository = usuarioRepository;
        }

        [ValidarSesion]
        public ActionResult Index(string id, string pagina)
        {
            if (pagina != null)
            {
                Session[ConstanteSesion.pagina] = pagina;
            }
            var ideSolicitud = id;
            try
            {
                var solicitud = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == Convert.ToInt32(ideSolicitud));

                var perfilViewModel = inicializarPerfil();
                var usuario = Session[ConstanteSesion.UsuarioDes].ToString();

                if (ideSolicitud != null)
                {
                   DatosCargo datosCargo = _cargoRepository.obtenerDatosCargo(Convert.ToInt32(ideSolicitud),usuario,Convert.ToInt32(Session[ConstanteSesion.Sede]));
                   datosCargo.IdeSolicitud = Convert.ToInt32(ideSolicitud);
                   CargoPerfil = datosCargo;
                   CargoPerfil.Pagina = pagina;
                   CargoPerfil.TipoEtapa = solicitud.TipoEtapa;
                   actualizarAccion(perfilViewModel);
                   
                }
                
                if (CargoPerfil != null)
                {
                    var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                    perfilViewModel.Cargo = cargo;
                    actualizarDatosCargo(perfilViewModel, cargo);
                    actualizarAccion(perfilViewModel);
                    
                }
               

                return View(perfilViewModel);
            }
            catch (Exception)
            {
                //return View(perfilViewModel);
                return View();
            }
            
        }

        public PerfilViewModel inicializarPerfil()
        {
            var pagina = Session[ConstanteSesion.pagina].ToString();
            
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Pagina = pagina;
           
            return cargoViewModel;
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Index([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var cargoViewModel = inicializarGeneral();
            try
            {
                CargoValidator validation = new CargoValidator();
                ValidationResult result = validation.Validate(cargo, "ObjetivoCargo", "FuncionCargo");
                if (!result.IsValid)
                {
                    cargoViewModel.Cargo = cargo;
                    return View(cargoViewModel);
                }

                cargoEditar.UsuarioModificacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                cargoEditar.FechaModificacion = FechaCreacion;
                cargoEditar.ObjetivoCargo = cargo.ObjetivoCargo;
                cargoEditar.FuncionCargo = cargo.FuncionCargo;
                _cargoRepository.Update(cargoEditar);

                return RedirectToAction("../Perfil/General");
            }
            catch (Exception ex)
            {
                cargoViewModel.Cargo = cargo;
                return View(cargoViewModel);
            }
        }

        [ValidarSesion]
        public ActionResult General()
        {
            var perfilViewModel = inicializarGeneral();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                perfilViewModel.Cargo = cargo;
                actualizarDatosCargo(perfilViewModel, cargo);
                actualizarAccion(perfilViewModel);
            }

            return View(perfilViewModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult General([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;

            var cargoViewModel = inicializarGeneral();
            try
            {
                var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo); 
                CargoValidator validation = new CargoValidator();
                ValidationResult resul = validation.Validate(cargo, "PuntajePostulanteInterno", "EdadInicio", "EdadFin",
                                                             "PuntajeEdad", "Sexo", "PuntajeSexo", "TipoRequerimiento", "TipoRangoSalarial", "PuntajeSalario");

                if (!resul.IsValid)
                {
                    cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                    cargoViewModel.Cargo = cargo;
                    actualizarDatosCargo(cargoViewModel, cargo);
                    actualizarAccion(cargoViewModel);
                    
                    return View(cargoViewModel);
                }
                else
                {
                    cargoEditar.UsuarioModificacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                    cargoEditar.FechaModificacion = FechaCreacion;
                    cargoEditar.PuntajeTotalPostulanteInterno = cargo.PuntajeTotalPostulanteInterno;
                    cargoEditar.EdadInicio = cargo.EdadInicio;
                    cargoEditar.EdadFin = cargo.EdadFin;
                    cargoEditar.PuntajeEdad = cargo.PuntajeEdad;
                    cargoEditar.PuntajeTotalEdad = cargo.PuntajeEdad;
                    cargoEditar.Sexo = cargo.Sexo;
                    cargoEditar.PuntajeSexo = cargo.PuntajeSexo;
                    cargoEditar.TipoRequerimiento = cargo.TipoRequerimiento;
                    cargoEditar.TipoRangoSalarial = cargo.TipoRangoSalarial;
                    cargoEditar.IndicadorEdadRanking = cargo.IndicadorEdadRanking;
                    cargoEditar.IndicadorSalarioRanking = cargo.IndicadorSalarioRanking;
                    cargoEditar.IndicadorSexoRanking = cargo.IndicadorSexoRanking;
                    cargoEditar.PuntajeSalario = cargo.PuntajeSalario;
                    _cargoRepository.Update(cargoEditar);
                    return RedirectToAction("../Perfil/Estudio");
                }

                
            }
            catch (Exception ex)
            {
                cargoViewModel.Cargo = cargo;
                return View(cargoViewModel);
            }

        }

        [ValidarSesion]
        public PerfilViewModel inicializarGeneral()
        {
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();

            cargoViewModel.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            cargoViewModel.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            cargoViewModel.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            cargoViewModel.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            cargoViewModel.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return cargoViewModel;
        }

        [ValidarSesion]
        public ActionResult Estudio()
        {

            var estudioCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(estudioCargoViewModel, cargo);
                actualizarAccion(estudioCargoViewModel);
            }
            
            return View(estudioCargoViewModel);
        }

        [ValidarSesion]
        public ActionResult Experiencia()
        {
            var experienciaCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(experienciaCargoViewModel,cargo);
                actualizarAccion(experienciaCargoViewModel);
            }
            return View(experienciaCargoViewModel);
        }

        [ValidarSesion]
        public ActionResult Conocimientos()
        {
            var conocimientosCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(conocimientosCargoViewModel, cargo);
                actualizarAccion(conocimientosCargoViewModel);
            }
            return View(conocimientosCargoViewModel);
        }

        [ValidarSesion]
        public ActionResult Discapacidad()
        {
            var discapacidadCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(discapacidadCargoViewModel,cargo);
                actualizarAccion(discapacidadCargoViewModel);
            }
            return View(discapacidadCargoViewModel);
        }

        [ValidarSesion]
        public ActionResult ConfiguracionPerfil()
        {
            int IdeCargo = CargoPerfil.IdeCargo; 
            var discapacidadCargoViewModel = inicializarDatosConfig(IdeCargo);
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(discapacidadCargoViewModel,cargo);
                actualizarAccion(discapacidadCargoViewModel);
            }
            return View(discapacidadCargoViewModel);
        }
        
        public PerfilViewModel inicializarDatosCargo()
        {
            var discapacidadCargoViewModel = new PerfilViewModel();
            discapacidadCargoViewModel.Cargo = new Cargo();

            return discapacidadCargoViewModel;
        }

        public PerfilViewModel inicializarDatosConfig(int IdeCargo)
        {
            var cargoViewModel = new PerfilViewModel();
            var cargoActual = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);

            int puntajeTotal = Convert.ToInt32(cargoActual.PuntajeTotalCentroEstudio) + Convert.ToInt32(cargoActual.PuntajeTotalConocimientoGeneral) + Convert.ToInt32(cargoActual.PuntajeTotalDiscapacidad)+
                               Convert.ToInt32(cargoActual.PuntajeTotalEdad) + Convert.ToInt32(cargoActual.PuntajeTotalExperiencia) + Convert.ToInt32(cargoActual.PuntajeTotalHorario)+
                               Convert.ToInt32(cargoActual.PuntajeTotalIdioma) + Convert.ToInt32(cargoActual.PuntajeTotalNivelEstudio) + Convert.ToInt32(cargoActual.PuntajeTotalOfimatica) + Convert.ToInt32(cargoActual.PuntajeTotalPostulanteInterno)+
                               Convert.ToInt32(cargoActual.PuntajeTotalSalario) + Convert.ToInt32(cargoActual.PuntajeTotalSexo) + Convert.ToInt32(cargoActual.PuntajeTotalUbigeo);

            cargoViewModel.TotalMaximo = puntajeTotal;
            cargoViewModel.Cargo = cargoActual;
            actualizarAccion(cargoViewModel);
            return cargoViewModel;
        }


        [ValidarSesion]
        [HttpPost]
        public ActionResult ConfiguracionPerfil([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var cargoViewModel = inicializarGeneral();
            
            try
            {
                CargoValidator validation = new CargoValidator();
                ValidationResult result = validation.Validate(cargo, "PuntajeMinimoExamen", "PuntajeMin");

                cargoEditar.UsuarioModificacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                cargoEditar.FechaModificacion = FechaCreacion;
                cargoEditar.PuntajeMinimoExamen = cargo.PuntajeMinimoExamen;
                cargoEditar.PuntajeMinimoGeneral = cargo.PuntajeMinimoGeneral;
                cargoEditar.CantidadPreseleccionados = cargo.CantidadPreseleccionados;

                if (!result.IsValid)
                {
                    cargoViewModel.Cargo = cargoEditar;
                    actualizarDatosCargo(cargoViewModel,cargo);
                    return View(cargoViewModel);
                }
                             
                _cargoRepository.Update(cargoEditar);
                cargoViewModel.Cargo = cargoEditar;
                actualizarDatosCargo(cargoViewModel, cargoEditar);
                actualizarAccion(cargoViewModel);
                return View(cargoViewModel);
            }
            catch (Exception ex)
            {
                cargoViewModel.Cargo = cargo;
                return View(cargoViewModel);
            }

        }

        [ValidarSesion]
        public ActionResult Evaluacion()
        {
            var evaluacionCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(evaluacionCargoViewModel,cargo);
                actualizarAccion(evaluacionCargoViewModel);
            }
            return View(evaluacionCargoViewModel);
        }

        public void actualizarDatosCargo(PerfilViewModel perfilViewModel, Cargo cargo)
        {
            perfilViewModel.Cargo.CodigoCargo = CargoPerfil.CodigoCargo;
            perfilViewModel.Cargo.NombreCargo = CargoPerfil.NombreCargo;
            perfilViewModel.Cargo.DescripcionCargo = CargoPerfil.DescripcionCargo;
            perfilViewModel.Cargo.NumeroPosiciones = CargoPerfil.NumeroPosiciones;
            perfilViewModel.Cargo.IdeCargo = CargoPerfil.IdeCargo;
            perfilViewModel.Area = CargoPerfil.Area;
            perfilViewModel.Dependencia = CargoPerfil.Dependencia;
            perfilViewModel.Departamento = CargoPerfil.Departamento;
            perfilViewModel.IdeSolicitud = CargoPerfil.IdeSolicitud;
            perfilViewModel.Pagina = CargoPerfil.Pagina;

            if (cargo.EstadoActivo == IndicadorActivo.Activo)
            { perfilViewModel.EstadoRegistro = "Activo"; }
            else
            { perfilViewModel.EstadoRegistro = "Inactivo"; }
        }

        [HttpPost]
        public ActionResult aceptarPerfil()
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var enviarMail = new SendMail();
            SedeNivel usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            var cargoEnviar = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
            var solicitud = _solicitudNuevoCargoRepository.GetSingle(x => x.CodigoCargo == cargoEnviar.CodigoCargo);

            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");

            var SedeSession = Session[ConstanteSesion.Sede];

            string SedeDescripcion = "-";
            if (SedeSession != null)
            {
                SedeDescripcion = Session[ConstanteSesion.SedeDes].ToString();
            }
            try
            {

                if ((solicitud.TipoEtapa == Etapa.Aprobacion_Perfil) && (Roles.Encargado_Seleccion == Convert.ToInt32(Session[ConstanteSesion.Rol])))
                {

                    string IndArea = "NO";
                    LogSolicitudNuevoCargo logSolicitud = new LogSolicitudNuevoCargo();
                    
                    logSolicitud.IdeSolicitudNuevoCargo = solicitud.IdeSolicitudNuevoCargo;

                    var logSolResponsable = _solicitudNuevoCargoRepository.responsablePublicacion(solicitud.IdeSolicitudNuevoCargo, solicitud.IdeSede);
                    logSolicitud.RolResponsable = logSolResponsable.RolResponsable;
                    logSolicitud.UsuarioResponsable = logSolResponsable.UsuarioResponsable;

                    logSolicitud.TipoEtapa = Etapa.Aceptado;
                    logSolicitud.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                    logSolicitud.UsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);

                    int ideUsuario = _logSolicitudNuevoRepository.solicitarAprobacion(logSolicitud, solicitud.IdeSede, solicitud.IdeArea, IndArea);

                    if (ideUsuario != -1)
                    {
                        var usuarioResp = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuario);
                        enviarMail.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                        enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();
                        enviarMail.Sede = SedeDescripcion;
                        enviarMail.Area = usuarioSession.AREADES;

                        enviarMail.EnviarCorreo(dir.ToString(), Etapa.Aceptado, usuarioResp.NombreUsuario, "Nuevo Cargo", "", cargoEnviar.NombreCargo, cargoEnviar.CodigoCargo, usuarioResp.Email, "Suceso");

                        objJsonMessage.Mensaje = "Perfil aceptado para su publicación";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "ERROR: no se pudo aceptar la solicitud intente de nuevo";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    objJsonMessage.Mensaje = "ERROR no tiene permisos para la accion o el estado de la solicitud no requiere esta accion";
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

        [HttpPost]
        public ActionResult verificarPerfilCompleto()
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try 
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                if ((cargo.PuntajeMinimoExamen != 0) && (cargo.PuntajeMinimoGeneral != 0) && (cargo.NumeroPosiciones != 0) &&
                    (cargo.PuntajeTotalCentroEstudio != 0) && (cargo.PuntajeTotalConocimientoGeneral != 0) && (cargo.PuntajeTotalEdad != 0) &&
                    (cargo.PuntajeTotalExamen != 0) && (cargo.PuntajeTotalExperiencia != 0) && (cargo.PuntajeTotalNivelEstudio != 0) &&
                    (cargo.PuntajeTotalOfimatica != 0))
                {
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
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


        [HttpPost]
        public ActionResult enviarPerfil()
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var enviarMail = new SendMail();
            SedeNivel usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            
            var cargoEnviar = _cargoRepository.GetSingle(x=>x.IdeCargo == CargoPerfil.IdeCargo);
            
            var solicitud = _solicitudNuevoCargoRepository.GetSingle(x=>x.CodigoCargo==cargoEnviar.CodigoCargo);
            
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");

            var SedeSession = Session[ConstanteSesion.Sede];
            
            string SedeDescripcion = "-";
            if (SedeSession != null)
            {
                SedeDescripcion = Session[ConstanteSesion.SedeDes].ToString();   
            }
            try
            {
                
                if ((solicitud.TipoEtapa == Etapa.Aprobado) && (Roles.Jefe_Corporativo_Seleccion == Convert.ToInt32(Session[ConstanteSesion.Rol])))
                {

                    string IndArea = "NO";

                    LogSolicitudNuevoCargo logSolicitud = new LogSolicitudNuevoCargo();
                    
                    logSolicitud.IdeSolicitudNuevoCargo = solicitud.IdeSolicitudNuevoCargo;
                    
                    var logSolicitudInicial = _logSolicitudNuevoRepository.getFirthValue(x => x.IdeSolicitudNuevoCargo == solicitud.IdeSolicitudNuevoCargo);
                    
                    logSolicitud.RolResponsable = logSolicitudInicial.RolSuceso;
                    logSolicitud.UsuarioResponsable = logSolicitudInicial.UsuarioResponsable;
                    logSolicitud.TipoEtapa = Etapa.Generacion_Perfil;
                    logSolicitud.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                    logSolicitud.UsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);

                    if (logSolicitud.RolResponsable == Roles.Jefe)
                    {
                        IndArea = "SI";
                    }

                    int ideUsuario = _logSolicitudNuevoRepository.solicitarAprobacion(logSolicitud, solicitud.IdeSede, solicitud.IdeArea, IndArea);

                    if (ideUsuario != -1)
                    {
                        var usuarioResp = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuario);
                        enviarMail.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                        enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();
                        enviarMail.Sede = SedeDescripcion;
                        enviarMail.Area = usuarioSession.AREADES;

                        enviarMail.EnviarCorreo(dir.ToString(), Etapa.Generacion_Perfil, usuarioResp.NombreUsuario, "Nuevo Cargo", "", cargoEnviar.NombreCargo, cargoEnviar.CodigoCargo, usuarioResp.Email,"Suceso");

                        objJsonMessage.Mensaje = "Perfil enviado para su aprobación";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "ERROR: no se pudo enviar la solicitud intente de nuevo";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    objJsonMessage.Mensaje = "ERROR no tiene permisos para la accion o el estado de la solicitud no requiere esta accion" ;
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
        public void actualizarAccion(PerfilViewModel perfilViewModel)
        {
            string pagina = Session[ConstanteSesion.pagina].ToString();

            var tipoEtapa = CargoPerfil.TipoEtapa;
            switch (tipoEtapa)
            {
                case Etapa.Aprobado:
                    perfilViewModel.Accion = Accion.Enviar;
                    break;
                case Etapa.Generacion_Perfil:
                    perfilViewModel.Accion = Accion.Aprobar;
                    break;
                case Etapa.Aprobacion_Perfil:
                    perfilViewModel.Accion = Accion.Aceptar;
                    break;
                case Etapa.Aceptado:
                    perfilViewModel.Accion = Accion.Publicar;
                    break;
                case Etapa.Observado:
                    perfilViewModel.Accion = Accion.Enviar;
                    break;
            }

            if (pagina == TipoSolicitud.ConsultaRequerimientos)
            {
                perfilViewModel.Accion = Accion.Consultar;
            }
        }

        

    }
}
