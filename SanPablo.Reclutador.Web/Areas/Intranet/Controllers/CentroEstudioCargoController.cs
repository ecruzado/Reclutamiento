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

    public class CentroEstudioCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private ICentroEstudioCargoRepository _centroEstudioCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public CentroEstudioCargoController(ICargoRepository cargoRepository,
                                ICentroEstudioCargoRepository centroEstudiosRepository,
                                IDetalleGeneralRepository detalleGeneralRepository)
        {
            _cargoRepository = cargoRepository;
            _centroEstudioCargoRepository = centroEstudiosRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            
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

        #region CENTRO ESTUDIOS

        [HttpPost]
        public virtual JsonResult ListaCentroEstudio(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CentroEstudioCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_centroEstudioCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCentroEstudioCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoCentroEstudio,
                                item.TipoNombreCentroEstudio,
                                item.PuntajeCentroEstudios.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult CentroEstudio()
        {
            var centroEstudioViewModel = inicializarCentroEstudio();
            return View(centroEstudioViewModel);
        }

        [HttpPost]
        public ActionResult CentroEstudio([Bind(Prefix = "CentroEstudio")]CentroEstudioCargo centroEstudioCargo)
        {
            if (!ModelState.IsValid)
            {
                var centroEstudioViewModel = inicializarCentroEstudio();
                centroEstudioViewModel.CentroEstudio = centroEstudioCargo;
                return View("CentroEstudio", centroEstudioViewModel);
            }
            _centroEstudioCargoRepository.Add(centroEstudioCargo);

            return View();

        }
        public PerfilViewModel inicializarCentroEstudio()
        {
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.CentroEstudio = new CentroEstudioCargo();

            cargoViewModel.TiposInstitucion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoInstitucion));
            cargoViewModel.TiposInstitucion.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.Instituciones = new List<DetalleGeneral>();
            cargoViewModel.Instituciones.Add(new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

    }
}
