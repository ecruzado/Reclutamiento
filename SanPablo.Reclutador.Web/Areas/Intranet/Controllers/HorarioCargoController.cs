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

    public class HorarioCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IHorarioCargoRepository _horarioCargoRepository;

        public HorarioCargoController(ICargoRepository cargoRepository,
                                      IDetalleGeneralRepository detalleGeneralRepository,
                                      IHorarioCargoRepository horarioCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _horarioCargoRepository = horarioCargoRepository;
        }

        #region HORARIOS

        [HttpPost]
        public virtual JsonResult ListaHorario(GridTable grid)
        {
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<HorarioCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_horarioCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeHorarioCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionHorario,
                                item.PuntajeHorario.ToString(),
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
            var horariosViewModel = inicializarHorarios();
            if (id != "0")
            {
                var horarioCargo = _horarioCargoRepository.GetSingle(x => x.IdeHorarioCargo == Convert.ToInt32(id));
                horariosViewModel.Horario = horarioCargo;
            }
            return View(horariosViewModel);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Horario")]HorarioCargo horarioCargo)
        {
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var horarioViewModel = inicializarHorarios();
                    horarioViewModel.Horario = horarioCargo;
                    return View("Horario", horarioViewModel);
                }
                if (horarioCargo.IdeHorarioCargo == 0)
                {
                    horarioCargo.EstadoActivo = "A";
                    horarioCargo.FechaCreacion = FechaCreacion;
                    horarioCargo.UsuarioCreacion = "YO";
                    horarioCargo.FechaModificacion = FechaCreacion;
                    horarioCargo.Cargo = new Cargo();
                    horarioCargo.Cargo.IdeCargo = IdeCargo;
                    _horarioCargoRepository.Add(horarioCargo);
                    

                }
                else
                {
                    var horarioCargoActualizar = _horarioCargoRepository.GetSingle(x => x.IdeHorarioCargo == horarioCargo.IdeHorarioCargo);
                    horarioCargoActualizar.TipoHorario = horarioCargo.TipoHorario;
                    horarioCargoActualizar.PuntajeHorario = horarioCargo.PuntajeHorario;
                    horarioCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                    horarioCargoActualizar.FechaModificacion = FechaModificacion;
                    _horarioCargoRepository.Update(horarioCargoActualizar);
                    
                }

                actualizarPuntaje(horarioCargo.PuntajeHorario,0,IdeCargo);

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
        public HorarioCargoViewModel inicializarHorarios()
        {
            var horarioCargoViewModel = new HorarioCargoViewModel();
            horarioCargoViewModel.Cargo = new Cargo();
            horarioCargoViewModel.Horario = new HorarioCargo();

            horarioCargoViewModel.Horarios = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));

            return horarioCargoViewModel;
        }

        [HttpPost]
        public ActionResult eliminarHorario(int ideHorario)
        {
            ActionResult result = null;
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            var horarioEliminar = new HorarioCargo();
            horarioEliminar = _horarioCargoRepository.GetSingle(x => x.IdeHorarioCargo == ideHorario);
            int puntajeEliminar = horarioEliminar.PuntajeHorario;
            _horarioCargoRepository.Remove(horarioEliminar);
            actualizarPuntaje(0, puntajeEliminar, IdeCargo);
            
            return result;
        }
        public void actualizarPuntaje(int puntaje, int puntajeEliminado,int IdeCargo)
        {
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);

            if (cargo.PuntajeTotalHorario < puntaje)
            {
                cargo.PuntajeTotalHorario = puntaje;
            }
            if (cargo.PuntajeTotalHorario == puntajeEliminado)
            {
                var puntajeMax = _horarioCargoRepository.getMaxValue("PuntajeHorario", x => x.Cargo == cargo);
                cargo.PuntajeTotalHorario = puntajeMax;
            }
            _cargoRepository.Update(cargo);
        }

        
        #endregion
    }
}
