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
            int IdeCargo = CargoPerfil.IdeCargo;
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
                                item.Puntaje.ToString(),
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
        public ViewResult Edit()
        {
            var cargoViewModel = InicializarCompetencias();
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
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
            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            CompetenciaCargoValidator validator = new CompetenciaCargoValidator();
            ValidationResult resultValidator = validator.Validate(competenciaCargo, "TipoCompetencia", "Puntaje");
            bool result = validarPuntajeCero(competenciaCargo);
            try
            {
                if (!resultValidator.IsValid)
                {
                    if (!result)
                    {
                        objJsonMessage.Mensaje = "Verifique los datos ingresados";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                if (existe(competenciaCargo.TipoCompetencia))
                {
                    objJsonMessage.Mensaje = "La competencia seleccionada ya fue ingresada";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
                else
                {
                    competenciaCargo.EstadoActivo = IndicadorActivo.Activo;
                    competenciaCargo.FechaCreacion = FechaCreacion;
                    competenciaCargo.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                    competenciaCargo.FechaModificacion = FechaCreacion;
                    competenciaCargo.Cargo = new Cargo();
                    competenciaCargo.Cargo.IdeCargo = IdeCargo;

                    _competenciaCargoRepository.Add(competenciaCargo);

                    objJsonMessage.Mensaje = "Agregado Correctamente";
                    objJsonMessage.Resultado = true;
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

        public bool existe(string descripcion)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = _competenciaCargoRepository.CountByExpress(x => x.TipoCompetencia == descripcion && x.Cargo.IdeCargo == IdeCargo);

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }

        public bool validarPuntajeCero(CompetenciaCargo competencia)
        {
            bool result = false;
            if ((Convert.ToInt32(competencia.Puntaje) == 0) && (competencia.TipoCompetencia !="00"))
            {
                result = true;
            }
            return result;
        }

    }
}
