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

    public class CompetenciaCargoController : BaseController
    {
        private ICargoRepository _cargoRepository;
        private ICompetenciaCargoRepository _competenciaCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public CompetenciaCargoController(ICargoRepository cargoRepository,
                                          ICompetenciaCargoRepository competenciaCargoRepository,
                                          IDetalleGeneralRepository detalleGeneralRepository)
                                      
        {
            _cargoRepository = cargoRepository;
            _competenciaCargoRepository = competenciaCargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;

        }

        #region COMPETENCIA

        [HttpPost]
        public JsonResult ListarCompetencias(GridTable grid)
        {
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CompetenciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

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

        public ViewResult Edit()
        {
            var cargoViewModel = InicializarCompetencias();
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == 1);
            cargoViewModel.Competencia.Cargo = cargo;
            return View(cargoViewModel);
        }

        public CompetenciaCargoViewModel InicializarCompetencias()
        {
            var competenciaCargoViewModel = new CompetenciaCargoViewModel();
            competenciaCargoViewModel.Competencia = new CompetenciaCargo();

            competenciaCargoViewModel.Competencias = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCompetencia));
            competenciaCargoViewModel.Competencias.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            return competenciaCargoViewModel;
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Competencia")]CompetenciaCargo competenciaCargo)
        {
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var competenciaViewModel = InicializarCompetencias();
                    competenciaViewModel.Competencia = competenciaCargo;
                    return View("Competencia", competenciaViewModel);
                }
                //var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
                competenciaCargo.EstadoActivo = "A";
                competenciaCargo.FechaCreacion = FechaCreacion;
                competenciaCargo.UsuarioCreacion = "YO";
                competenciaCargo.FechaModificacion = FechaCreacion;
                competenciaCargo.Cargo = new Cargo();
                competenciaCargo.Cargo.IdeCargo = IdeCargo;

                //cargo.agregarCompetencia(competenciaCargo);
                _competenciaCargoRepository.Add(competenciaCargo);

                objJsonMessage.Mensaje = "Agregado Correctamente";
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }


        [HttpPost]
        public ActionResult eliminarCompetencia(int ideCompetencia)
        {
            ActionResult result = null;

            var competenciaEliminar = new CompetenciaCargo();
            competenciaEliminar = _competenciaCargoRepository.GetSingle(x => x.IdeCompetenciaCargo == ideCompetencia);
            _competenciaCargoRepository.Remove(competenciaEliminar);

            return result;
        }

        #endregion

        public PerfilViewModel inicializarPerfil()
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

    }
}
