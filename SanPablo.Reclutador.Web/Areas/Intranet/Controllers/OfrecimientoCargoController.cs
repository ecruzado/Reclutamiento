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

    public class OfrecimientoCargoController : BaseController
    {
        private ICargoRepository _cargoRepository;
        private IOfrecemosCargoRepository _ofrecemosCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public OfrecimientoCargoController(ICargoRepository cargoRepository,
                                           IDetalleGeneralRepository detalleGeneralRepository,
                                           IOfrecemosCargoRepository ofrecemosCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _ofrecemosCargoRepository = ofrecemosCargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
                        
        }


        #region OFRECEMOS CARGO

        [HttpPost]
        public virtual JsonResult ListarOfrecemos(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<OfrecemosCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_ofrecemosCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeOfrecemosCargo.ToString(),
                        cell = new string[]
                            {
                                item.IdeOfrecemosCargo.ToString(),
                                item.DescripcionOfrecimiento,
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
            var cargoViewModel = InicializarOfrecimientos();
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            cargoViewModel.Ofrecimiento.Cargo = cargo;
            return View(cargoViewModel);

        }

        public OfrecimientoViewModel InicializarOfrecimientos()
        {
            var OfrecimientoViewModel = new OfrecimientoViewModel();
            OfrecimientoViewModel.Ofrecimiento = new OfrecemosCargo();

            OfrecimientoViewModel.Ofrecimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoOfrecimiento));
            OfrecimientoViewModel.Ofrecimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return OfrecimientoViewModel;
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Ofrecimiento")]OfrecemosCargo ofrecemosCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var OfrecemosViewModel = InicializarOfrecimientos();
                    OfrecemosViewModel.Ofrecimiento = ofrecemosCargo;
                    return View("Ofrecemos", OfrecemosViewModel);
                }
                ofrecemosCargo.EstadoActivo = "A";
                ofrecemosCargo.FechaCreacion = FechaCreacion;
                ofrecemosCargo.UsuarioCreacion = "YO";
                ofrecemosCargo.FechaModificacion = FechaCreacion;
                ofrecemosCargo.Cargo = new Cargo();
                ofrecemosCargo.Cargo.IdeCargo = IdeCargo;

                //cargo.agregarCompetencia(competenciaCargo);
                _ofrecemosCargoRepository.Add(ofrecemosCargo);

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
        public ActionResult eliminarOfrecimiento(int ideOfrecimiento)
        {
            ActionResult result = null;

            var ofrecimientoEliminar = new OfrecemosCargo();
            ofrecimientoEliminar = _ofrecemosCargoRepository.GetSingle(x => x.IdeOfrecemosCargo == ideOfrecimiento);
            _ofrecemosCargoRepository.Remove(ofrecimientoEliminar);

            return result;
        }

        #endregion

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

    }
}
