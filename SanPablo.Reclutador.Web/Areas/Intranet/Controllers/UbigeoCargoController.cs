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
    public class UbigeoCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IUbigeoCargoRepository _ubigeoCargoRepository;
        private IUbigeoRepository _ubigeoRepository;


        public UbigeoCargoController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                IUbigeoCargoRepository ubigeoCargoRepository,
                                IUbigeoRepository ubigeoRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _ubigeoCargoRepository = ubigeoCargoRepository;
            _ubigeoRepository = ubigeoRepository;
        }

       
        #region UBIGEO

        [HttpPost]
        public virtual JsonResult ListaUbigeo(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<UbigeoCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_ubigeoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeUbigeo.ToString(),
                        cell = new string[]
                            {
                                item.Departamento,
                                item.Provincia,
                                item.Distrito,
                                item.PuntajeUbigeo.ToString(),
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
            var ubigeoViewModel = inicializarUbigeos();
            if (id != "0")
            {
                ubigeoViewModel.Ubigeo = _ubigeoCargoRepository.GetSingle(x => x.IdeUbigeo == Convert.ToInt32(id)&&x.Cargo.IdeCargo == CargoPerfil.IdeCargo);
                
                mostrarUbigeo(ubigeoViewModel);
            }
            return View(ubigeoViewModel);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Ubigeo")]UbigeoCargo ubigeoCargo)
        {

            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    objJsonMessage.Mensaje = "Ingrese un puntaje mayor a cero";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }

                if (!existe(ubigeoCargo.IdeUbigeo, ubigeoCargo.IdeUbigeoCargo))
                {
                    if (ubigeoCargo.IdeUbigeoCargo == 0)
                    {
                        ubigeoCargo.EstadoActivo = IndicadorActivo.Activo;
                        ubigeoCargo.FechaCreacion = FechaCreacion;
                        ubigeoCargo.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                        ubigeoCargo.FechaModificacion = FechaCreacion;
                        ubigeoCargo.Cargo = new Cargo();
                        ubigeoCargo.Cargo.IdeCargo = IdeCargo;

                        _ubigeoCargoRepository.Add(ubigeoCargo);
                        actualizarPuntaje(Convert.ToInt32(ubigeoCargo.PuntajeUbigeo), 0, IdeCargo);
                    }
                    else
                    {
                        var ubigeoCargoActualizar = _ubigeoCargoRepository.GetSingle(x => x.IdeUbigeoCargo == ubigeoCargo.IdeUbigeoCargo);

                        int puntajeAnterior = Convert.ToInt32(ubigeoCargoActualizar.PuntajeUbigeo);
                        ubigeoCargoActualizar.IdeUbigeo = ubigeoCargo.IdeUbigeo;
                        ubigeoCargoActualizar.PuntajeUbigeo = ubigeoCargo.PuntajeUbigeo;
                        ubigeoCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        ubigeoCargoActualizar.FechaModificacion = FechaModificacion;
                        _ubigeoCargoRepository.Update(ubigeoCargoActualizar);
                        actualizarPuntaje(Convert.ToInt32(ubigeoCargo.PuntajeUbigeo), puntajeAnterior, IdeCargo);
                    }
                    
                    objJsonMessage.Mensaje = "Agregado Correctamente";
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    objJsonMessage.Mensaje = "No puede agregar el ubigeo mas de una vez";
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
        public UbigeoCargoViewModel inicializarUbigeos()
        {
            var ubigeoCargoViewModel = new UbigeoCargoViewModel();
            //cargoViewModel.Cargo = new Cargo();
            ubigeoCargoViewModel.Ubigeo = new UbigeoCargo();

            ubigeoCargoViewModel.Departamentos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == null).OrderBy(x=>x.Nombre));
            ubigeoCargoViewModel.Departamentos.Insert(0, new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            ubigeoCargoViewModel.Provincias = new List<Ubigeo>();
            ubigeoCargoViewModel.Provincias.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            ubigeoCargoViewModel.Distritos = new List<Ubigeo>();
            ubigeoCargoViewModel.Distritos.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            return ubigeoCargoViewModel;
        }

        [HttpPost]
        public ActionResult listarUbigeos(int ideUbigeoPadre)
        {
            ActionResult result = null;

            var listaResultado = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == ideUbigeoPadre).OrderBy(x => x.Nombre));
            result = Json(listaResultado);
            return result;
        }

        [HttpPost]
        public ActionResult eliminarUbigeo(int ideUbigeo)
        {
            ActionResult result = null;

            var ubigeoEliminar = new UbigeoCargo();
            ubigeoEliminar = _ubigeoCargoRepository.GetSingle(x => x.IdeUbigeo == ideUbigeo);
            _ubigeoCargoRepository.Remove(ubigeoEliminar);

            return result;
        }


        public void mostrarUbigeo(UbigeoCargoViewModel model)
        {
            int ubigeo = model.Ubigeo.IdeUbigeo;
            //recuperar distrito
            var distrito = new Ubigeo();
            var provincia = new Ubigeo();
            var departamento = new Ubigeo();

            distrito = _ubigeoRepository.GetSingle(x => x.IdeUbigeo == ubigeo);
            provincia = _ubigeoRepository.GetSingle(x => x.IdeUbigeo == distrito.IdeUbigeoPadre);
            departamento = _ubigeoRepository.GetSingle(x => x.IdeUbigeo == provincia.IdeUbigeoPadre);

            model.Distritos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == provincia.IdeUbigeo));
            model.Provincias = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == departamento.IdeUbigeo));
            model.Departamentos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == null));
            model.Departamentos.Insert(0, departamento);


        }

        public void actualizarPuntaje(int puntaje, int puntajeEliminado, int IdeCargo)
        {
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);

            if (cargo.PuntajeTotalUbigeo < puntaje)
            {
                cargo.PuntajeTotalUbigeo = puntaje;
            }
            if (cargo.PuntajeTotalUbigeo == puntajeEliminado)
            {
                var puntajeMax = _ubigeoCargoRepository.getMaxValue("PuntajeUbigeo", x => x.Cargo == cargo);
                cargo.PuntajeTotalUbigeo = puntajeMax;
            }
            _cargoRepository.Update(cargo);
        }


        public bool existe(int ideUbigeo, int ideUbigeoCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = 0;
            if (ideUbigeoCargo == 0)
            {
                contador = _ubigeoCargoRepository.CountByExpress(x => x.IdeUbigeo == ideUbigeo && x.Cargo.IdeCargo == IdeCargo);
            }
            else
            {
                contador = _ubigeoCargoRepository.CountByExpress(x => x.IdeUbigeo == ideUbigeo && x.Cargo.IdeCargo == IdeCargo && x.IdeUbigeoCargo != ideUbigeoCargo);
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
