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

    [Authorize]
    public class PerfilController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
       
        public PerfilController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
        }

        public ActionResult Index(string ideSolicitud)
        {
            //try
            //{

                (Session["CargoIde"]) = 1;
                int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
                var perfilViewModel = inicializarPerfil();
                if (IdeCargo != 0)
                {
                    var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
                    perfilViewModel.Cargo = cargo;
                }
            //}
            //catch (Exception)
            //{
 
            //}
            return View(perfilViewModel);
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
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var cargoViewModel = inicializarGeneral();
            try
            {
                if (!ModelState.IsValid)
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
            (Session["CargoIde"]) = 1;
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            var perfilViewModel = inicializarGeneral();
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
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);  
            var cargoViewModel = inicializarGeneral();
            try
            {
                if (!ModelState.IsValid)
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
        
        [HttpPost]
        public ActionResult ListaCargo(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[11]
        {
          "200001",
          "200001",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[11]
        {
          "200002",
          "200002",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[11]
        {
          "200003",
          "200003",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType2_4 = new
            {
                id = 4,
                cell = new string[11]
        {
          "200004",
          "200004",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_4);
            var fAnonymousType2_5 = new
            {
                id = 5,
                cell = new string[11]
        {
          "200005",
          "200005",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_5);
            var fAnonymousType2_6 = new
            {
                id = 6,
                cell = new string[11]
        {
          "200006",
          "200006",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_6);
            var fAnonymousType2_7 = new
            {
                id = 7,
                cell = new string[11]
        {
          "200007",
          "200007",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_7);
            var fAnonymousType2_8 = new
            {
                id = 8,
                cell = new string[11]
        {
          "200008",
          "200008",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_8);
            var fAnonymousType2_9 = new
            {
                id = 9,
                cell = new string[11]
        {
          "200009",
          "200009",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_9);
            var fAnonymousType2_10 = new
            {
                id = 10,
                cell = new string[11]
        {
          "200010",
          "200010",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_10);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        public ActionResult Estudio()
        {
            (Session["CargoIde"]) = 1;
            var estudioCargoViewModel = inicializarDatosCargo();
            return View(estudioCargoViewModel);
        }
        
        public ActionResult Experiencia()
        {
            (Session["CargoIde"]) = 1;
            var experienciaCargoViewModel = inicializarDatosCargo();
            return View(experienciaCargoViewModel);
        }

        public ActionResult Conocimientos()
        {
            (Session["CargoIde"]) = 1;
            var conocimientosCargoViewModel = inicializarDatosCargo();
            return View(conocimientosCargoViewModel);
        }

        public ActionResult Discapacidad()
        {
            (Session["CargoIde"]) = 1;
            var discapacidadCargoViewModel = inicializarDatosCargo();
            return View(discapacidadCargoViewModel);
        }

        public ActionResult ConfiguracionPerfil()
        {
            (Session["CargoIde"]) = 1;
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]); 
            var discapacidadCargoViewModel = inicializarDatosConfig(IdeCargo);
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
            var discapacidadCargoViewModel = new PerfilViewModel();
            var cargoActual = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            discapacidadCargoViewModel.Cargo = cargoActual;
            return discapacidadCargoViewModel;
        }
        [HttpPost]
        public ActionResult ConfiguracionPerfil([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var cargoViewModel = inicializarGeneral();
            try
            {
                if (!ModelState.IsValid)
                {
                    cargoViewModel.Cargo = cargo;
                    return View(cargoViewModel);
                }

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
                _cargoRepository.Update(cargoEditar);

                
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
            (Session["CargoIde"]) = 1;
            var evaluacionCargoViewModel = inicializarDatosCargo();
            return View(evaluacionCargoViewModel);
        }
        
    }
}
