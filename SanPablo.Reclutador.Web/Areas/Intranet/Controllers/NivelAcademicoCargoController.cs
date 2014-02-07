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

    public class NivelAcademicoCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private INivelAcademicoCargoRepository _nivelAcademicoCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;



        public NivelAcademicoCargoController(ICargoRepository cargoRepository,
                                INivelAcademicoCargoRepository nivelAcademicoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository)
        {
            _cargoRepository = cargoRepository;
            _nivelAcademicoCargoRepository = nivelAcademicoRepository;
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

        #region NIVEL ACADEMICO

        [HttpPost]
        public virtual JsonResult ListaNivelAcademico(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<NivelAcademicoCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_nivelAcademicoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoEducacion.ToString(),
                                item.TipoAreaEstudio.ToString(),
                                item.TipoNivelAlcanzado.ToString(),
                                item.PuntajeNivelEstudio.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult NivelAcademico()
        {
            var nivelAcademicoViewModel = inicializarNivelAcademico();
            return View(nivelAcademicoViewModel);
        }

        [HttpPost]
        public ActionResult NivelAcademico([Bind(Prefix = "NivelAcademico")]NivelAcademicoCargo nivelAcademicoCargo)
        {
            if (!ModelState.IsValid)
            {
                var nivelAcademicoViewModel = inicializarNivelAcademico();
                nivelAcademicoViewModel.NivelAcademico = nivelAcademicoCargo;
                return View("NivelAcademico", nivelAcademicoViewModel);
            }
            _nivelAcademicoCargoRepository.Add(nivelAcademicoCargo);

            return View();

        }

        public PerfilViewModel inicializarNivelAcademico()
        {
            var cargoViewModel = new PerfilViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.NivelAcademico = new NivelAcademicoCargo();

            cargoViewModel.TiposEducacion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEducacion));
            cargoViewModel.TiposEducacion.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoArea));
            cargoViewModel.AreasEstudio.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.NivelesAlcanzados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.NivelAlcanzado));
            cargoViewModel.NivelesAlcanzados.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

    }
}
