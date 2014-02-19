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

    public class DiscapacidadCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IDiscapacidadCargoRepository _discapacidadCargoRepository;


        public DiscapacidadCargoController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                IDiscapacidadCargoRepository discapacidadCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _discapacidadCargoRepository = discapacidadCargoRepository;
        }


        #region DISCAPACIDAD

        [HttpPost]
        public virtual JsonResult ListaDiscapacidad(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<DiscapacidadCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_discapacidadCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeDiscapacidadCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoDiscapacidad,
                                item.PuntajeDiscapacidad.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Edit(string id)
        {
            var discapacidadViewModel = inicializarDiscapacidad();
            if (id != "0")
            {
                var discapacidadCargo = _discapacidadCargoRepository.GetSingle(x => x.IdeDiscapacidadCargo == Convert.ToInt32(id));
                discapacidadViewModel.Discapacidad = discapacidadCargo;
            }
            return View(discapacidadViewModel);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Discapacidad")]DiscapacidadCargo discapacidadCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var discapacidadViewModel = inicializarDiscapacidad();
                    discapacidadViewModel.Discapacidad = discapacidadCargo;
                    return View(discapacidadViewModel);
                }
                if (discapacidadCargo.IdeDiscapacidadCargo == 0)
                {
                    discapacidadCargo.EstadoActivo = "A";
                    discapacidadCargo.FechaCreacion = FechaCreacion;
                    discapacidadCargo.UsuarioCreacion = "YO";
                    discapacidadCargo.FechaModificacion = FechaCreacion;
                    discapacidadCargo.Cargo = new Cargo();
                    discapacidadCargo.Cargo.IdeCargo = IdeCargo;

                    _discapacidadCargoRepository.Add(discapacidadCargo);
                    _discapacidadCargoRepository.actualizarPuntaje(discapacidadCargo.PuntajeDiscapacidad,0, IdeCargo);
                }
                else
                {
                    var discapacidadCargoActualizar = _discapacidadCargoRepository.GetSingle(x => x.IdeDiscapacidadCargo == discapacidadCargo.IdeDiscapacidadCargo);
                    int valorEditar = discapacidadCargoActualizar.PuntajeDiscapacidad;
                    discapacidadCargoActualizar.TipoDiscapacidad = discapacidadCargo.TipoDiscapacidad;
                    discapacidadCargoActualizar.PuntajeDiscapacidad = discapacidadCargo.PuntajeDiscapacidad;
                    discapacidadCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                    discapacidadCargoActualizar.FechaModificacion = FechaModificacion;
                    _discapacidadCargoRepository.Update(discapacidadCargoActualizar);
                    _discapacidadCargoRepository.actualizarPuntaje(discapacidadCargo.PuntajeDiscapacidad, valorEditar, IdeCargo);
                }

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

        public DiscapacidadCargoViewModel inicializarDiscapacidad()
        {
            var discapacidadViewModel = new DiscapacidadCargoViewModel();
            discapacidadViewModel.Cargo = new Cargo();
            discapacidadViewModel.Discapacidad = new DiscapacidadCargo();

            discapacidadViewModel.TipoDiscapacidad = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoDiscapacidad));
            discapacidadViewModel.TipoDiscapacidad.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return discapacidadViewModel;
        }

        [HttpPost]
        public ActionResult eliminarDiscapacidad(int ideDiscapacidad)
        {
            ActionResult result = null;
            int IdeCargo = CargoPerfil.IdeCargo;
            var discapacidadCargo = new DiscapacidadCargo();
            discapacidadCargo = _discapacidadCargoRepository.GetSingle(x => x.IdeDiscapacidadCargo == ideDiscapacidad);
            int valorEliminar = discapacidadCargo.PuntajeDiscapacidad;
            _discapacidadCargoRepository.Remove(discapacidadCargo);
            _discapacidadCargoRepository.actualizarPuntaje(0, valorEliminar, IdeCargo);

            return result;
        }


        #endregion

    }
}
