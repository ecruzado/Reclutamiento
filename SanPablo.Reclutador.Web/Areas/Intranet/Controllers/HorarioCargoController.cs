﻿namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
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
    using SanPablo.Reclutador.Entity.Validation;

    [Authorize]
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
            int IdeCargo = CargoPerfil.IdeCargo;
            
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

        [ValidarSesion]
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


        /// <summary>
        /// Ingresa los horarios al perfil cuando es uno nuevo
        /// </summary>
        /// <param name="horarioCargo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Horario")]HorarioCargo horarioCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                HorarioCargoValidator validator = new HorarioCargoValidator();
                ValidationResult result = validator.Validate(horarioCargo, "TipoHorario", "PuntajeHorario");

                if (!ModelState.IsValid)
                {
                    var horariosViewModel = inicializarHorarios();
                    horariosViewModel.Horario = horarioCargo;
                    return View(horariosViewModel);
                    //objJsonMessage.Mensaje = "Ingrese un puntaje mayor a cero";
                    //objJsonMessage.Resultado = false;
                    //return Json(objJsonMessage);

                }


                int contador = _horarioCargoRepository.CountByExpress(x =>  x.Cargo.IdeCargo == IdeCargo && x.PuntajeHorario == horarioCargo.PuntajeHorario);

                if (contador>0)
                {
                    objJsonMessage.Mensaje = "Debe ingresar un puntaje diferente";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }


                if (horarioCargo.IdeHorarioCargo == 0)
                {
                    if (existe(horarioCargo.TipoHorario))
                    {
                        objJsonMessage.Mensaje = "No puede agregar el mismo tipo de horario más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        horarioCargo.EstadoActivo = IndicadorActivo.Activo;
                        horarioCargo.FechaCreacion = FechaCreacion;
                        horarioCargo.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                        horarioCargo.FechaModificacion = FechaCreacion;
                        horarioCargo.Cargo = new Cargo();
                        horarioCargo.Cargo.IdeCargo = IdeCargo;
                        _horarioCargoRepository.Add(horarioCargo);
                        actualizarPuntaje(Convert.ToInt32(horarioCargo.PuntajeHorario), 0, IdeCargo);
                        objJsonMessage.Mensaje = "Agregado Correctamente";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }

                }
                else
                {
                    var horarioCargoActualizar = _horarioCargoRepository.GetSingle(x => x.IdeHorarioCargo == horarioCargo.IdeHorarioCargo);

                    int cont = _horarioCargoRepository.CountByExpress(x => x.TipoHorario == horarioCargo.TipoHorario && x.Cargo.IdeCargo == IdeCargo && x.IdeHorarioCargo != horarioCargo.IdeHorarioCargo);

                    if (cont > 0)
                    {
                        objJsonMessage.Mensaje = "No puede agregar el mismo tipo de horario más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        int puntajeAnterior = Convert.ToInt32(horarioCargoActualizar.PuntajeHorario);
                        horarioCargoActualizar.TipoHorario = horarioCargo.TipoHorario;
                        horarioCargoActualizar.PuntajeHorario = horarioCargo.PuntajeHorario;
                        horarioCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        horarioCargoActualizar.FechaModificacion = FechaModificacion;
                        _horarioCargoRepository.Update(horarioCargoActualizar);

                        actualizarPuntaje(Convert.ToInt32(horarioCargo.PuntajeHorario), puntajeAnterior, IdeCargo);
                        objJsonMessage.Mensaje = "Agregado Correctamente";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    
                }
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
            horarioCargoViewModel.Horarios.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            return horarioCargoViewModel;
        }

        [HttpPost]
        public ActionResult eliminarHorario(int ideHorario)
        {
            ActionResult result = null;
            int IdeCargo = CargoPerfil.IdeCargo;
            var horarioEliminar = new HorarioCargo();
            horarioEliminar = _horarioCargoRepository.GetSingle(x => x.IdeHorarioCargo == ideHorario);
            int puntajeEliminar = Convert.ToInt32(horarioEliminar.PuntajeHorario);
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

        public bool existe(string descripcion)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = _horarioCargoRepository.CountByExpress(x => x.TipoHorario == descripcion && x.Cargo.IdeCargo == IdeCargo);

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }

        #endregion
    }
}
