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

    public class CargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private INivelAcademicoCargoRepository _nivelAcademicoCargoRepository;
        private ICentroEstudioCargoRepository _centroEstudioCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICompetenciaCargoRepository _competenciaCargoRepository;
        

        public CargoController(ICargoRepository cargoRepository,
                                INivelAcademicoCargoRepository nivelAcademicoRepository,
                                ICentroEstudioCargoRepository centroEstudiosRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                ICompetenciaCargoRepository competenciaCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _nivelAcademicoCargoRepository = nivelAcademicoRepository;
            _centroEstudioCargoRepository = centroEstudiosRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _competenciaCargoRepository = competenciaCargoRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
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

        #region Competencia

        [HttpPost]
        public virtual JsonResult ListarCompetencias(GridTable grid)
        {
            try
            {
                
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                //DetachedCriteria where = DetachedCriteria.For<CompetenciaCargo>();
                //where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_competenciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCompetenciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionCompetencia,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ViewResult Competencia()
        {
            var cargoViewModel = InicializarCompetencias();
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == 1);
            cargoViewModel.Competencia.Cargo = cargo;
            return View(cargoViewModel);
            
        }

        public CargoViewModel InicializarCompetencias()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Competencia = new CompetenciaCargo();

            cargoViewModel.Competencias = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCompetencia));
            cargoViewModel.Competencias.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return cargoViewModel;
        }

        [HttpPost]
        public ActionResult Competencia([Bind(Prefix = "Competencia")]CompetenciaCargo competenciaCargo)
        {
            if (!ModelState.IsValid)
            {
                var competenciaViewModel = InicializarCompetencias();
                competenciaViewModel.Competencia = competenciaCargo;
                return View("Competencia", competenciaViewModel);
            }
                _competenciaCargoRepository.Add(competenciaCargo);
                return View();
            
        }

        #endregion

        #region OfrecemosCargo

        [HttpPost]
        public virtual JsonResult ListarOfrecemos(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CompetenciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_competenciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCompetenciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionCompetencia,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ViewResult Competencia()
        {
            var cargoViewModel = InicializarCompetencias();
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == 1);
            cargoViewModel.Competencia.Cargo = cargo;
            return View(cargoViewModel);

        }

        public CargoViewModel InicializarCompetencias()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Competencia = new CompetenciaCargo();

            cargoViewModel.Competencias = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCompetencia));
            cargoViewModel.Competencias.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return cargoViewModel;
        }

        [HttpPost]
        public ActionResult Competencia([Bind(Prefix = "Competencia")]CompetenciaCargo competenciaCargo)
        {
            if (!ModelState.IsValid)
            {
                var competenciaViewModel = InicializarCompetencias();
                competenciaViewModel.Competencia = competenciaCargo;
                return View("Competencia", competenciaViewModel);
            }
            _competenciaCargoRepository.Add(competenciaCargo);
            return View();

        }
        #endregion
    }
}
