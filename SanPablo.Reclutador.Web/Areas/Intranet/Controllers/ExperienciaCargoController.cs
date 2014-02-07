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

    public class ExperienciaCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IExperienciaCargoRepository _experienciaCargoRepository;

        public ExperienciaCargoController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                IExperienciaCargoRepository experienciaCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _experienciaCargoRepository = experienciaCargoRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            Session["CargoIde"] = 1;
            var cargoViewModel = inicializarCargo();
            return View(cargoViewModel);
        }

        public PerfilViewModel inicializarCargo()
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






        #region EXPERIENCIA

        [HttpPost]
        public virtual JsonResult ListaExperiencia(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ExperienciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_experienciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoExperiencia.ToString(),
                                item.CantidadAnhosExperiencia.ToString(),
                                item.CantidadMesesExperiencia.ToString(),
                                item.PuntajeExperiencia.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Experiencia()
        {
            var experienciaViewModel = inicializarExperiencia();
            return View(experienciaViewModel);
        }

        [HttpPost]
        public ActionResult Idioma([Bind(Prefix = "Experiencia")]ExperienciaCargo experienciaCargo)
        {
            if (!ModelState.IsValid)
            {
                var experienciaViewModel = inicializarExperiencia();
                experienciaViewModel.Experiencia = experienciaCargo;
                return View("Experiencia", experienciaViewModel);
            }
            _experienciaCargoRepository.Add(experienciaCargo);

            return View();

        }

        public PerfilViewModel inicializarExperiencia()
        {
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Experiencia = new ExperienciaCargo();

            cargoViewModel.TipoCargos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCargo));
            cargoViewModel.TipoCargos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

    }
}
