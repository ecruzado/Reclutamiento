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
    using SanPablo.Reclutador.Entity.Validation;

    [Authorize]
    public class ExperienciaCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IExperienciaCargoRepository _experienciaCargoRepository;

        public ExperienciaCargoController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                IExperienciaCargoRepository experienciaCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _experienciaCargoRepository = experienciaCargoRepository;
        }

        
        #region EXPERIENCIA


        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public virtual JsonResult ListaExperiencia(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ExperienciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_experienciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString(),
                                item.CantidadMesesExperiencia.ToString(),
                                item.PuntajeExperiencia.ToString(),
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
            var experienciaViewModel = inicializarExperiencia();
            if (id != "0")
            {
                var experiencia = _experienciaCargoRepository.GetSingle(x => x.IdeExperienciaCargo == Convert.ToInt32(id));
                experienciaViewModel.Experiencia = experiencia;
            }
            return View(experienciaViewModel);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Experiencia")]ExperienciaCargo experienciaCargo)
        {

            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                ExperienciaCargoValidator validator = new ExperienciaCargoValidator();
                ValidationResult resultValidator = validator.Validate(experienciaCargo, "TipoExperiencia", "PuntajeExperiencia");
                bool result = validarExperiencia(experienciaCargo);

                if ((!resultValidator.IsValid)||(!result))
                {
                    if (!result)
                    {
                        objJsonMessage.Mensaje = "Verifique la cantidad de años, meses y puntaje sean correctos";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "Verifique que haya llenado los datos obligatorios";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                if (experienciaCargo.IdeExperienciaCargo == 0)
                {
                    if (existe(experienciaCargo.TipoExperiencia))
                    {
                        objJsonMessage.Mensaje = "No puede agregar el mismo tipo de experiencia más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        experienciaCargo.EstadoActivo = IndicadorActivo.Activo;
                        experienciaCargo.FechaCreacion = FechaCreacion;
                        experienciaCargo.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                        experienciaCargo.FechaModificacion = FechaCreacion;
                        experienciaCargo.Cargo = new Cargo();
                        experienciaCargo.Cargo.IdeCargo = IdeCargo;

                        _experienciaCargoRepository.Add(experienciaCargo);
                        _experienciaCargoRepository.actualizarPuntaje(Convert.ToInt32(experienciaCargo.PuntajeExperiencia), 0, IdeCargo);

                        objJsonMessage.Mensaje = "Agregado Correctamente";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    var experienciaCargoActualizar = _experienciaCargoRepository.GetSingle(x => x.IdeExperienciaCargo == experienciaCargo.IdeExperienciaCargo);

                    int contador = _experienciaCargoRepository.CountByExpress(x => x.TipoExperiencia == experienciaCargo.TipoExperiencia && x.Cargo.IdeCargo == IdeCargo && x.IdeExperienciaCargo != experienciaCargo.IdeExperienciaCargo);

                    if (contador > 0)
                    {
                        objJsonMessage.Mensaje = "No puede agregar el mismo tipo de experiencia más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {

                        int valorEditar = Convert.ToInt32(experienciaCargoActualizar.PuntajeExperiencia);
                        experienciaCargoActualizar.TipoExperiencia = experienciaCargo.TipoExperiencia;
                        experienciaCargoActualizar.CantidadAnhosExperiencia = experienciaCargo.CantidadAnhosExperiencia;
                        experienciaCargoActualizar.CantidadMesesExperiencia = experienciaCargo.CantidadMesesExperiencia;
                        experienciaCargoActualizar.PuntajeExperiencia = experienciaCargo.PuntajeExperiencia;
                        experienciaCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        experienciaCargoActualizar.FechaModificacion = FechaModificacion;
                        _experienciaCargoRepository.Update(experienciaCargoActualizar);
                        _experienciaCargoRepository.actualizarPuntaje(Convert.ToInt32(experienciaCargo.PuntajeExperiencia), valorEditar, IdeCargo);

                        objJsonMessage.Mensaje = "Editado Correctamente";
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

        public ExperienciaCargoViewModel inicializarExperiencia()
        {
            var experienciaViewModel = new ExperienciaCargoViewModel();
            experienciaViewModel.Cargo = new Cargo();
            experienciaViewModel.Experiencia = new ExperienciaCargo();

            experienciaViewModel.TiposCargo = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCargo));
            experienciaViewModel.TiposCargo.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return experienciaViewModel;
        }

        [HttpPost]
        public ActionResult eliminarExperiencia(int ideExperiencia)
        {
            ActionResult result = null;
            int IdeCargo = CargoPerfil.IdeCargo;
            var experienciaEliminar = new ExperienciaCargo();
            experienciaEliminar = _experienciaCargoRepository.GetSingle(x => x.IdeExperienciaCargo == ideExperiencia);
            int valorEliminar = Convert.ToInt32(experienciaEliminar.PuntajeExperiencia);
            _experienciaCargoRepository.Remove(experienciaEliminar);
            _experienciaCargoRepository.actualizarPuntaje(0, valorEliminar, IdeCargo);

            return result;
        }

        public bool validarExperiencia(ExperienciaCargo experienciaCargo)
        {
            if ((experienciaCargo.CantidadAnhosExperiencia >= 0) && (experienciaCargo.CantidadAnhosExperiencia <= 70) &&
                (experienciaCargo.CantidadMesesExperiencia >= 0) && (experienciaCargo.CantidadMesesExperiencia <= 12) &&
                (experienciaCargo.PuntajeExperiencia >= 0) && (experienciaCargo.CantidadMesesExperiencia <= 20))
            {
                return true;
            }
            else { return false; }
        }

        public bool existe(string tipoExperiencia)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = _experienciaCargoRepository.CountByExpress(x => x.TipoExperiencia == tipoExperiencia && x.Cargo.IdeCargo == IdeCargo);

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }

        #endregion

    }
}
