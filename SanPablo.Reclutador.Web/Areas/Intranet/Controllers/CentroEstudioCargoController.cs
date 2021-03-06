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

    [Authorize]
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

        #region CENTRO ESTUDIOS


        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public virtual JsonResult ListaCentroEstudio(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CentroEstudioCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_centroEstudioCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCentroEstudioCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoCentroEstudio,
                                item.DescripcionNombreCentroEstudio,
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

        [ValidarSesion]
        public ActionResult Edit(string id)
        {
            var centroEstudioViewModel = inicializarCentroEstudio();
            if (id != "0")
            {
                var centroEstudio = _centroEstudioCargoRepository.GetSingle(x => x.IdeCentroEstudioCargo == Convert.ToInt32(id));
                centroEstudioViewModel.CentroEstudio = centroEstudio;
                centroEstudioViewModel = actualizarDatos(centroEstudioViewModel, centroEstudio);
                
            }
            return View(centroEstudioViewModel);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "CentroEstudio")]CentroEstudioCargo centroEstudioCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    //objJsonMessage.Mensaje = "Verifique que haya ingresado los datos obligatorios y que el puntaje sea mayor a cero";
                    //objJsonMessage.Resultado = false;
                    //return Json(objJsonMessage);
                    var centroEstudioViewModel = inicializarCentroEstudio();
                    centroEstudioViewModel.CentroEstudio = centroEstudioCargo;
                    return View(centroEstudioViewModel);
                }
                if (!existe(centroEstudioCargo.TipoCentroEstudio, centroEstudioCargo.TipoNombreCentroEstudio, centroEstudioCargo.IdeCentroEstudioCargo))
                {
                    if (centroEstudioCargo.IdeCentroEstudioCargo == 0)
                    {
                        centroEstudioCargo.EstadoActivo = "A";
                        centroEstudioCargo.FechaCreacion = FechaCreacion;
                        centroEstudioCargo.UsuarioCreacion = "YO";
                        centroEstudioCargo.FechaModificacion = FechaCreacion;
                        centroEstudioCargo.Cargo = new Cargo();
                        centroEstudioCargo.Cargo.IdeCargo = IdeCargo;
                        _centroEstudioCargoRepository.Add(centroEstudioCargo);
                        _centroEstudioCargoRepository.actualizarPuntaje(Convert.ToInt32(centroEstudioCargo.PuntajeCentroEstudios), 0, IdeCargo);
                    }
                    else
                    {
                        var centroEstudioCargoActualizar = _centroEstudioCargoRepository.GetSingle(x => x.IdeCentroEstudioCargo == centroEstudioCargo.IdeCentroEstudioCargo);
                        int valorEditar = Convert.ToInt32(centroEstudioCargoActualizar.PuntajeCentroEstudios);
                        centroEstudioCargoActualizar.TipoCentroEstudio = centroEstudioCargo.TipoCentroEstudio;
                        centroEstudioCargoActualizar.TipoNombreCentroEstudio = centroEstudioCargo.TipoNombreCentroEstudio;
                        centroEstudioCargoActualizar.PuntajeCentroEstudios = centroEstudioCargo.PuntajeCentroEstudios;
                        centroEstudioCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        centroEstudioCargoActualizar.FechaModificacion = FechaModificacion;
                        _centroEstudioCargoRepository.Update(centroEstudioCargoActualizar);
                        _centroEstudioCargoRepository.actualizarPuntaje(Convert.ToInt32(centroEstudioCargo.PuntajeCentroEstudios), valorEditar, IdeCargo);

                    }

                    objJsonMessage.Mensaje = "Agregado Correctamente";
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    objJsonMessage.Mensaje = "No puedes agregar la misma institución mas de una vez";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }
        public CentroEstudioViewModel inicializarCentroEstudio()
        {
            var centroEstudioViewModel = new CentroEstudioViewModel();
            centroEstudioViewModel.Cargo = new Cargo();
            centroEstudioViewModel.CentroEstudio = new CentroEstudioCargo();

            centroEstudioViewModel.TiposInstitucion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoInstitucion));
            centroEstudioViewModel.TiposInstitucion.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            centroEstudioViewModel.Instituciones = new List<DetalleGeneral>();
            centroEstudioViewModel.Instituciones.Add(new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return centroEstudioViewModel;
        }

        [HttpPost]
        public ActionResult eliminarCentroEstudio(int ideCentroEstudio)
        {
            ActionResult result = null;
            int IdeCargo = CargoPerfil.IdeCargo;
            var centroEstudioEliminar = new CentroEstudioCargo();
            centroEstudioEliminar = _centroEstudioCargoRepository.GetSingle(x => x.IdeCentroEstudioCargo == ideCentroEstudio);
            int valorEliminar = Convert.ToInt32(centroEstudioEliminar.PuntajeCentroEstudios);
            _centroEstudioCargoRepository.Remove(centroEstudioEliminar);
            _centroEstudioCargoRepository.actualizarPuntaje(0, valorEliminar, IdeCargo);

            return result;
        }


        [HttpPost]
        public ActionResult listarNombreInstitucion(string tipoInstituto)
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();

            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoInstitucion, tipoInstituto));
            result = Json(listaResultado);
            return result;
        }

        public CentroEstudioViewModel actualizarDatos(CentroEstudioViewModel centroEstudioViewModel, CentroEstudioCargo centroEstudioCargo)
        {
            if (centroEstudioCargo != null)
            {
                var listaResultado = new List<DetalleGeneral>();
                listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoInstitucion, centroEstudioCargo.TipoCentroEstudio));
                centroEstudioViewModel.Instituciones = listaResultado;
            }
            return centroEstudioViewModel;
        }

        public bool existe(string tipInstitucion,string tipNombInstitucion, int ideCentroEstudioCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = 0;
            if (ideCentroEstudioCargo == 0)
            {
                contador = _centroEstudioCargoRepository.CountByExpress(x => x.TipoCentroEstudio == tipInstitucion && x.TipoNombreCentroEstudio ==tipNombInstitucion && x.Cargo.IdeCargo == IdeCargo);
            }
            else
            {
                contador = _centroEstudioCargoRepository.CountByExpress(x => x.TipoCentroEstudio == tipInstitucion&&x.TipoNombreCentroEstudio == tipNombInstitucion && x.Cargo.IdeCargo == IdeCargo && x.IdeCentroEstudioCargo != ideCentroEstudioCargo);
            }

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }

        #endregion

    }
}
