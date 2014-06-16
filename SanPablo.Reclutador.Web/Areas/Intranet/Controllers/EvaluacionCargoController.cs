namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using SanPablo.Reclutador.Entity.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;


    [Authorize]
    public class EvaluacionCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IEvaluacionCargoRepository _evaluacionCargoRepository;
        private IExamenRepository _examenRepository;
        private IAreaRepository _areaRepository;

        public EvaluacionCargoController(ICargoRepository cargoRepository,
                                      IDetalleGeneralRepository detalleGeneralRepository,
                                      IEvaluacionCargoRepository evaluacionCargoRepository,
                                      IExamenRepository examenRepository,
                                      IAreaRepository areaRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _evaluacionCargoRepository = evaluacionCargoRepository;
            _examenRepository = examenRepository;
            _areaRepository = areaRepository;
        }

        #region EVALUACION CARGO

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public virtual JsonResult ListaEvaluaciones(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<EvaluacionCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_evaluacionCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeEvaluacionCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExamen,
                                item.DescripcionTipoExamen,
                                item.NotaMinimaExamen.ToString(),
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
            var evaluacionViewModel = inicializarEvaluacion();
            if (id != "0")
            {
                var evaluacionCargo = _evaluacionCargoRepository.GetSingle(x => x.IdeEvaluacionCargo == Convert.ToInt32(id));
                evaluacionViewModel.Evaluacion = evaluacionCargo;
                evaluacionViewModel.descExamen = evaluacionCargo.DescripcionTipoExamen;
            }
            return View(evaluacionViewModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Evaluacion")]EvaluacionCargo evaluacionCargo)
        {
            EvaluacionCargoValidator validator = new EvaluacionCargoValidator();
            ValidationResult result = validator.Validate(evaluacionCargo, "TipoExamen", "NotaMinimaExamen");

            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!result.IsValid)
                {
                    //if (!validarPtjeCero(evaluacionCargo))
                    //{
                        var evaluacionViewModel = inicializarEvaluacion();
                        evaluacionViewModel.Evaluacion = evaluacionCargo;
                        return View(evaluacionViewModel);
                    //}
                }
                if (evaluacionCargo.IdeEvaluacionCargo == 0)
                {

                    if (existeEvaluacion(evaluacionCargo.Examen.IdeExamen))
                    {
                        objJsonMessage.Mensaje = "No puede agregar evaluación duplicada";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {

                        evaluacionCargo.EstadoActivo = IndicadorActivo.Activo;
                        evaluacionCargo.FechaCreacion = FechaCreacion;
                        evaluacionCargo.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                        evaluacionCargo.FechaModificacion = FechaCreacion;
                        evaluacionCargo.Cargo = new Cargo();
                        evaluacionCargo.Cargo.IdeCargo = IdeCargo;

                        _evaluacionCargoRepository.Add(evaluacionCargo);
                        _evaluacionCargoRepository.actualizarPuntaje(evaluacionCargo.PuntajeExamen, 0, IdeCargo);

                        objJsonMessage.Mensaje = "Agregado Correctamente";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    var evaluacionCargoActualizar = _evaluacionCargoRepository.GetSingle(x => x.IdeEvaluacionCargo == evaluacionCargo.IdeEvaluacionCargo);

                    int contador = _evaluacionCargoRepository.CountByExpress(x => x.Examen.IdeExamen == evaluacionCargo.IdeExamen
                                                                               && x.Cargo.IdeCargo == CargoPerfil.IdeCargo && x.IdeEvaluacionCargo !=  evaluacionCargo.IdeEvaluacionCargo);

                    if (contador > 0)
                    {
                        objJsonMessage.Mensaje = "No puede insertar el mismo exámen más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        int valorEditar = evaluacionCargoActualizar.PuntajeExamen;

                        var examen = _examenRepository.GetSingle(x => x.IdeExamen == evaluacionCargo.IdeExamen);
                        evaluacionCargoActualizar.Examen = examen;

                        evaluacionCargoActualizar.TipoExamen = evaluacionCargo.TipoExamen;
                        evaluacionCargoActualizar.TipoAreaResponsable = evaluacionCargo.TipoAreaResponsable;
                        evaluacionCargoActualizar.PuntajeExamen = evaluacionCargo.PuntajeExamen;
                        evaluacionCargoActualizar.NotaMinimaExamen = evaluacionCargo.NotaMinimaExamen;
                        evaluacionCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        evaluacionCargoActualizar.FechaModificacion = FechaModificacion;
                        _evaluacionCargoRepository.Update(evaluacionCargoActualizar);
                        _evaluacionCargoRepository.actualizarPuntaje(evaluacionCargo.PuntajeExamen, valorEditar, IdeCargo);

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
        public EvaluacionCargoViewModel inicializarEvaluacion()
        {
            var evaluacionCargoViewModel = new EvaluacionCargoViewModel();
            evaluacionCargoViewModel.Cargo = new Cargo();
            evaluacionCargoViewModel.Evaluacion = new EvaluacionCargo();

            int ideSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);

            evaluacionCargoViewModel.Examenes = new List<Examen>(_examenRepository.GetBy(x=>x.EstActivo == IndicadorActivo.Activo && x.IdeSede == ideSede).OrderBy(X=>X.DescExamen));
            evaluacionCargoViewModel.Examenes.Insert(0, new Examen { IdeExamen = 0, DescExamen = "Seleccionar" });

            return evaluacionCargoViewModel;
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult eliminarEvaluacion(int ideEvaluacion)
        {
            ActionResult result = null;
            int IdeCargo = CargoPerfil.IdeCargo;
            var evaluacionEliminar = new EvaluacionCargo();
            evaluacionEliminar = _evaluacionCargoRepository.GetSingle(x => x.IdeEvaluacionCargo == ideEvaluacion);
            int valorEliminar = evaluacionEliminar.PuntajeExamen;
            _evaluacionCargoRepository.Remove(evaluacionEliminar);
            _evaluacionCargoRepository.actualizarPuntaje(0, valorEliminar, IdeCargo);

            return result;
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult getTipoExamen(int ideExamen)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var evaluacionBuscar = new Examen();
            if (ideExamen == 0)
            {
                objJsonMessage.Resultado = false;
            }
            else
            {
                evaluacionBuscar = _examenRepository.GetSingle(x => x.IdeExamen == ideExamen);
                objJsonMessage.Mensaje = evaluacionBuscar.TipExamenDes;
                objJsonMessage.Accion = evaluacionBuscar.TipExamen;
                objJsonMessage.Resultado = true;
            }
            return Json(objJsonMessage);

            
        }


        public bool existeEvaluacion(int ideExamen)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = _evaluacionCargoRepository.CountByExpress(x => x.Examen.IdeExamen == ideExamen && x.Cargo.IdeCargo == IdeCargo);

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }

        public bool validarPtjeCero(EvaluacionCargo evaluacionCargo)
        {
            bool result = false;
            if (evaluacionCargo.NotaMinimaExamen == 0)
            {
                if (evaluacionCargo.Examen.IdeExamen != 0)
                {
                    result = true;
                }
            }
            return result;
        }

        #endregion
    }
}
