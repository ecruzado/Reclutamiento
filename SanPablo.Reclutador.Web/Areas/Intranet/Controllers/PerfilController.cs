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

        public ActionResult Index()
        {
            (Session["CargoIde"]) = 1;
            var perfilViewModel = inicializarPerfil();
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


        public ActionResult General()
        {
            (Session["CargoIde"]) = 1;
            var perfilViewModel = inicializarGeneral();
            return View(perfilViewModel);
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
            var estudioCargoViewModel = inicializarEstudio();
            return View(estudioCargoViewModel);
        }

        public  PerfilViewModel inicializarEstudio()
        {
            var estudioCargoViewModel = new PerfilViewModel();
            estudioCargoViewModel.Cargo = new Cargo();
            //estudioCargoViewModel.NivelAcademico = new NivelAcademicoCargo();
            //estudioCargoViewModel.CentroEstudio = new CentroEstudioCargo();

            return estudioCargoViewModel;
        }

        public ActionResult Experiencia()
        {
            (Session["CargoIde"]) = 1;
            var experienciaCargoViewModel = inicializarExperiencia();
            return View(experienciaCargoViewModel);
        }

        public PerfilViewModel inicializarExperiencia()
        {
            var experienciaCargoViewModel = new PerfilViewModel();
            experienciaCargoViewModel.Cargo = new Cargo();
            //estudioCargoViewModel.NivelAcademico = new NivelAcademicoCargo();
            //estudioCargoViewModel.CentroEstudio = new CentroEstudioCargo();

            return experienciaCargoViewModel;
        }

        public ActionResult Conocimientos()
        {
            (Session["CargoIde"]) = 1;
            var conocimientosCargoViewModel = inicializarConocimientos();
            return View(conocimientosCargoViewModel);
        }

        public PerfilViewModel inicializarConocimientos()
        {
            var experienciaCargoViewModel = new PerfilViewModel();
            experienciaCargoViewModel.Cargo = new Cargo();
            //estudioCargoViewModel.NivelAcademico = new NivelAcademicoCargo();
            //estudioCargoViewModel.CentroEstudio = new CentroEstudioCargo();

            return experienciaCargoViewModel;
        }

    }
}
