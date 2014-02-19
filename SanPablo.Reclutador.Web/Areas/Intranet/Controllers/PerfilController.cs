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

    public class PerfilController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ILogSolicitudNuevoCargoRepository _logSolicitudNuevoRepository;
       
        public PerfilController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                ILogSolicitudNuevoCargoRepository logSolicitudNuevoRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _logSolicitudNuevoRepository = logSolicitudNuevoRepository;
        }

        public ActionResult Index(string ideSolicitud)
        {
            try
            {
                var perfilViewModel = inicializarPerfil();
                if (ideSolicitud != null)
                {
                    DatosCargo datosCargo = _cargoRepository.obtenerDatosCargo(Convert.ToInt32(ideSolicitud));
                    CargoPerfil = datosCargo;
                }
                actualizarDatosCargo(perfilViewModel);

                int IdeCargo = CargoPerfil.IdeCargo;
                if (IdeCargo != 0)
                {

                    var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
                    perfilViewModel.Cargo = cargo;
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
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();
            
            //cargoViewModel.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            //cargoViewModel.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            
            //cargoViewModel.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            //cargoViewModel.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            //cargoViewModel.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            //cargoViewModel.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return cargoViewModel;
        }

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

                cargoEditar.UsuarioModificacion = "USUA";
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

        public ActionResult General()
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var perfilViewModel = inicializarGeneral();
            actualizarDatosCargo(perfilViewModel);
            if (IdeCargo != 0)
            {
                var cargo = _cargoRepository.GetSingle(x=>x.IdeCargo == IdeCargo);
                perfilViewModel.Cargo = cargo;
            }
            
            return View(perfilViewModel);
        }
       
        [HttpPost]
        public ActionResult General([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);  
            var cargoViewModel = inicializarGeneral();
            try
            {
                CargoValidator validation = new CargoValidator();
                ValidationResult resul = validation.Validate(cargo, "PuntajePostulanteInterno", "EdadInicio", "EdadFin",
                                                             "PuntajeEdad", "Sexo", "PuntajeSexo", "TipoRequerimiento", "TipoRangoSalarial", "PuntajeSalario");

                if (!resul.IsValid)
                {
                    cargoViewModel.Cargo = cargo;
                    return View(cargoViewModel);
                }
                cargoEditar.UsuarioModificacion = "USUA";
                cargoEditar.FechaModificacion = FechaCreacion;
                cargoEditar.PuntajePostulanteInterno = cargo.PuntajePostulanteInterno;
                cargoEditar.EdadInicio = cargo.EdadInicio;
                cargoEditar.EdadFin = cargo.EdadFin;
                cargoEditar.PuntajeEdad = cargo.PuntajeEdad;
                cargoEditar.Sexo = cargo.Sexo;
                cargoEditar.PuntajeSexo = cargo.PuntajeSexo;
                cargoEditar.TipoRequerimiento = cargo.TipoRequerimiento;
                cargoEditar.TipoRangoSalarial = cargo.TipoRangoSalarial;
                cargoEditar.PuntajeSalario = cargo.PuntajeSalario;
                _cargoRepository.Update(cargoEditar);

                return RedirectToAction("../Perfil/Estudio");
            }
            catch (Exception ex)
            {
                cargoViewModel.Cargo = cargo;
                return View(cargoViewModel);
            }

        }

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
        
        public ActionResult Estudio()
        {
            var estudioCargoViewModel = inicializarDatosCargo();
            actualizarDatosCargo(estudioCargoViewModel);
            return View(estudioCargoViewModel);
        }
        
        public ActionResult Experiencia()
        {
            var experienciaCargoViewModel = inicializarDatosCargo();
            actualizarDatosCargo(experienciaCargoViewModel);
            return View(experienciaCargoViewModel);
        }

        public ActionResult Conocimientos()
        {
            var conocimientosCargoViewModel = inicializarDatosCargo();
            actualizarDatosCargo(conocimientosCargoViewModel);
            return View(conocimientosCargoViewModel);
        }

        public ActionResult Discapacidad()
        {
            var discapacidadCargoViewModel = inicializarDatosCargo();
            actualizarDatosCargo(discapacidadCargoViewModel);
            return View(discapacidadCargoViewModel);
        }

        public ActionResult ConfiguracionPerfil()
        {
            int IdeCargo = CargoPerfil.IdeCargo; 
            var discapacidadCargoViewModel = inicializarDatosConfig(IdeCargo);
            actualizarDatosCargo(discapacidadCargoViewModel);
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
            cargoViewModel.Cargo = cargoActual;
            return cargoViewModel;
        }

        [HttpPost]
        public ActionResult ConfiguracionPerfil([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var cargoViewModel = inicializarGeneral();
            
            try
            {
                CargoValidator validation = new CargoValidator();
                ValidationResult result = validation.Validate(cargo, "PuntajeMinimoPostulanteInterno", "PuntajeMinimoEdad", "PuntajeMinimoSexo", "PuntajeMinimoSalario",
                                                              "PuntajeMinimoNivelEstudio", "PuntajeMinimoCentroEstudio", "PuntajeMinimoExperiencia", "PuntajeMinimoOfimatica", "PuntajeMinimoIdioma", "PuntajeMinimoConocimientoGeneral",
                                                              "PuntajeMinimoDiscapacidad", "PuntajeMinimoHorario", "PuntajeMinimoUbigeo", "PuntajeMinimoExamen");

                cargoEditar.UsuarioModificacion = "USUA";
                cargoEditar.FechaModificacion = FechaCreacion;
                cargoEditar.PuntajeMinimoPostulanteInterno = cargo.PuntajeMinimoPostulanteInterno;
                cargoEditar.PuntajeMinimoEdad = cargo.PuntajeMinimoEdad;
                cargoEditar.PuntajeMinimoSexo = cargo.PuntajeMinimoSexo;
                cargoEditar.PuntajeMinimoSalario = cargo.PuntajeMinimoSalario;
                cargoEditar.PuntajeMinimoNivelEstudio = cargo.PuntajeMinimoNivelEstudio;
                cargoEditar.PuntajeMinimoCentroEstudio = cargo.PuntajeMinimoCentroEstudio;
                cargoEditar.PuntajeMinimoExperiencia = cargo.PuntajeMinimoExperiencia;
                cargoEditar.PuntajeMinimoOfimatica = cargo.PuntajeMinimoOfimatica;
                cargoEditar.PuntajeMinimoIdioma = cargo.PuntajeMinimoIdioma;
                cargoEditar.PuntajeMinimoConocimientoGeneral = cargo.PuntajeMinimoConocimientoGeneral;
                cargoEditar.PuntajeMinimoDiscapacidad = cargo.PuntajeMinimoDiscapacidad;
                cargoEditar.PuntajeMinimoHorario = cargo.PuntajeMinimoHorario;
                cargoEditar.PuntajeMinimoUbigeo = cargo.PuntajeMinimoUbigeo;
                cargoEditar.PuntajeMinimoExamen = cargo.PuntajeMinimoExamen;

                if (!result.IsValid)
                {
                    cargoViewModel.Cargo = cargoEditar;
                    actualizarDatosCargo(cargoViewModel);
                    return View(cargoViewModel);
                }
                             
                _cargoRepository.Update(cargoEditar);
                cargoViewModel.Cargo = cargoEditar;
                actualizarDatosCargo(cargoViewModel);
                
                return View(cargoViewModel);
            }
            catch (Exception ex)
            {
                cargoViewModel.Cargo = cargo;
                return View(cargoViewModel);
            }

        }

        public ActionResult Evaluacion()
        {
            var evaluacionCargoViewModel = inicializarDatosCargo();
            actualizarDatosCargo(evaluacionCargoViewModel);
            return View(evaluacionCargoViewModel);
        }

        public void actualizarDatosCargo(PerfilViewModel perfilViewModel)
        {
            perfilViewModel.Cargo.CodigoCargo = CargoPerfil.CodigoCargo;
            perfilViewModel.Cargo.NombreCargo = CargoPerfil.NombreCargo;
            perfilViewModel.Cargo.DescripcionCargo = CargoPerfil.DescripcionCargo;
            perfilViewModel.Cargo.NumeroPosiciones = CargoPerfil.NumeroPosiciones;
            perfilViewModel.Cargo.IdeCargo = CargoPerfil.IdeCargo;
            perfilViewModel.Area = CargoPerfil.Area;
            perfilViewModel.Dependencia = CargoPerfil.Dependencia;
            perfilViewModel.Departamento = CargoPerfil.Departamento;
        }

        public void enviarPerfil()
        {
            var enviarMail = new SendMail();
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargoEnviar = _cargoRepository.GetSingle(x=>x.IdeCargo == IdeCargo);
           
            enviarMail.EnviarCorreo(Asunto.Solicitado, AccionMail.Solicitado, true, Solicitud.Nuevo);
           
        }

    }
}
