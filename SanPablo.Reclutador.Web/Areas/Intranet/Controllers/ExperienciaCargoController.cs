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

        [HttpPost]
        public virtual JsonResult ListaExperiencia(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ExperienciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_experienciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoExperiencia.ToString(),
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
            
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var experienciaViewModel = inicializarExperiencia();
                    experienciaViewModel.Experiencia = experienciaCargo;
                    return View(experienciaViewModel);
                }
                if (experienciaCargo.IdeExperienciaCargo == 0)
                {
                    experienciaCargo.EstadoActivo = "A";
                    experienciaCargo.FechaCreacion = FechaCreacion;
                    experienciaCargo.UsuarioCreacion = "YO";
                    experienciaCargo.FechaModificacion = FechaCreacion;
                    experienciaCargo.Cargo = new Cargo();
                    experienciaCargo.Cargo.IdeCargo = IdeCargo;

                    _experienciaCargoRepository.Add(experienciaCargo);
                }
                else
                {
                    var experienciaCargoActualizar = _experienciaCargoRepository.GetSingle(x => x.IdeExperienciaCargo == experienciaCargo.IdeExperienciaCargo);
                    experienciaCargoActualizar.TipoExperiencia = experienciaCargo.TipoExperiencia;
                    experienciaCargoActualizar.CantidadAnhosExperiencia = experienciaCargo.CantidadAnhosExperiencia;
                    experienciaCargoActualizar.CantidadMesesExperiencia = experienciaCargo.CantidadMesesExperiencia;
                    experienciaCargoActualizar.PuntajeExperiencia = experienciaCargo.PuntajeExperiencia;
                    experienciaCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                    experienciaCargoActualizar.FechaModificacion = FechaModificacion;
                    _experienciaCargoRepository.Update(experienciaCargoActualizar);
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

            var experienciaEliminar = new ExperienciaCargo();
            experienciaEliminar = _experienciaCargoRepository.GetSingle(x => x.IdeExperienciaCargo == ideExperiencia);
            _experienciaCargoRepository.Remove(experienciaEliminar);

            return result;
        }

        #endregion

    }
}
