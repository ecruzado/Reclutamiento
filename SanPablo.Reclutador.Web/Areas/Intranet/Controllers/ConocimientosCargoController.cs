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

    public class ConocimientosCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IConocimientoGeneralCargoRepository _conocimientoCargoRepository;


        public ConocimientosCargoController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                IConocimientoGeneralCargoRepository conocimientoCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _conocimientoCargoRepository = conocimientoCargoRepository;
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


        #region OFIMATICA

        [HttpPost]
        public virtual JsonResult ListaOfimatica(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoConocimientoOfimatica.ToString(),
                                item.TipoNombreOfimatica.ToString(),
                                item.TipoNivelConocimiento.ToString(),
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Ofimatica()
        {
            var ofimaticaViewModel = inicializarOfimatica();
            return View(ofimaticaViewModel);
        }

        [HttpPost]
        public ActionResult Ofimatica([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo conocimientoGeneralCargo)
        {
            if (!ModelState.IsValid)
            {
                var conocimientoCargoViewModel = inicializarOfimatica();
                conocimientoCargoViewModel.Conocimiento = conocimientoGeneralCargo;
                return View("Conocimiento", conocimientoGeneralCargo);
            }
            _conocimientoCargoRepository.Add(conocimientoGeneralCargo);

            return View();

        }

        public PerfilViewModel inicializarOfimatica()
        {
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            cargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoOfimatica));
            cargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TiponombreOfimatica));
            cargoViewModel.DescripcionConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            cargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

        #region IDIOMA

        [HttpPost]
        public virtual JsonResult ListaIdioma(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoIdioma.ToString(),
                                item.TipoConocimientoIdioma.ToString(),
                                item.TipoNivelConocimiento.ToString(),
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Idioma()
        {
            var idiomaViewModel = inicializarIdioma();
            return View(idiomaViewModel);
        }

        [HttpPost]
        public ActionResult Idioma([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo conocimientoCargo)
        {
            if (!ModelState.IsValid)
            {
                var idiomaViewModel = inicializarIdioma();
                idiomaViewModel.Conocimiento = conocimientoCargo;
                return View("Conocimiento", idiomaViewModel);
            }
            _conocimientoCargoRepository.Add(conocimientoCargo);

            return View();

        }

        public PerfilViewModel inicializarIdioma()
        {
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            cargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoIdioma));
            cargoViewModel.DescripcionConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoIdioma));
            cargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            cargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

        #region OTROS CONOCIMIENTOS

        [HttpPost]
        public virtual JsonResult ListaOtrosConocimientos(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoConocimientoGeneral.ToString(),
                                item.TipoNombreConocimientoGeneral.ToString(),
                                item.TipoNivelConocimiento.ToString(),
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult OtrosConocimientos()
        {
            var otrosConocimientodViewModel = inicializarOtrosConocimientos();
            return View(otrosConocimientodViewModel);
        }

        [HttpPost]
        public ActionResult OtrosConocimientos([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo conocimientoCargo)
        {
            if (!ModelState.IsValid)
            {
                var otrosConocimientodViewModel = inicializarOtrosConocimientos();
                otrosConocimientodViewModel.Conocimiento = conocimientoCargo;
                return View("NivelAcademico", otrosConocimientodViewModel);
            }
            _conocimientoCargoRepository.Add(conocimientoCargo);

            return View();

        }

        public PerfilViewModel inicializarOtrosConocimientos()
        {
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            cargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoGral));
            cargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>();
            cargoViewModel.DescripcionConocimiento.Add(new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            cargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

    }
}
