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

    public class NivelAcademicoCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private INivelAcademicoCargoRepository _nivelAcademicoCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;



        public NivelAcademicoCargoController(ICargoRepository cargoRepository,
                                INivelAcademicoCargoRepository nivelAcademicoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository)
        {
            _cargoRepository = cargoRepository;
            _nivelAcademicoCargoRepository = nivelAcademicoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
        }

        #region NIVEL ACADEMICO

        [HttpPost]
        public virtual JsonResult ListaNivelAcademico(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<NivelAcademicoCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_nivelAcademicoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoEducacion.ToString(),
                                item.TipoAreaEstudio.ToString(),
                                item.TipoNivelAlcanzado.ToString(),
                                item.PuntajeNivelEstudio.ToString(),
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
            var nivelAcademicoViewModel = inicializarNivelAcademico();
            if (id != "0")
            {
                var nivelAcademico = _nivelAcademicoCargoRepository.GetSingle(x => x.IdeNivelAcademicoCargo == Convert.ToInt32(id));
                nivelAcademicoViewModel.NivelAcademico = nivelAcademico;
            }
            return View(nivelAcademicoViewModel);

        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "NivelAcademico")]NivelAcademicoCargo nivelAcademicoCargo)
        { 
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var nivelAcademicoViewModel = inicializarNivelAcademico();
                    nivelAcademicoViewModel.NivelAcademico = nivelAcademicoCargo;
                    return View("NivelAcademico", nivelAcademicoViewModel);
                }
                if (nivelAcademicoCargo.IdeNivelAcademicoCargo == 0)
                {
                    nivelAcademicoCargo.EstadoActivo = "A";
                    nivelAcademicoCargo.FechaCreacion = FechaCreacion;
                    nivelAcademicoCargo.UsuarioCreacion = "YO";
                    nivelAcademicoCargo.FechaModificacion = FechaCreacion;
                    nivelAcademicoCargo.Cargo = new Cargo();
                    nivelAcademicoCargo.Cargo.IdeCargo = IdeCargo;

                    _nivelAcademicoCargoRepository.Add(nivelAcademicoCargo);
                }
                else
                {
                    var nivelAcedemicoCargoActualizar = _nivelAcademicoCargoRepository.GetSingle(x => x.IdeNivelAcademicoCargo == nivelAcademicoCargo.IdeNivelAcademicoCargo);
                    nivelAcedemicoCargoActualizar.TipoEducacion = nivelAcademicoCargo.TipoEducacion;
                    nivelAcedemicoCargoActualizar.TipoAreaEstudio = nivelAcademicoCargo.TipoAreaEstudio;
                    nivelAcedemicoCargoActualizar.TipoNivelAlcanzado = nivelAcademicoCargo.TipoNivelAlcanzado;
                    nivelAcedemicoCargoActualizar.PuntajeNivelEstudio = nivelAcademicoCargo.PuntajeNivelEstudio;
                    nivelAcedemicoCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                    nivelAcedemicoCargoActualizar.FechaModificacion = FechaModificacion;
                    _nivelAcademicoCargoRepository.Update(nivelAcedemicoCargoActualizar);
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

        public NivelAcademicoViewModel inicializarNivelAcademico()
        {
            var nivelAcademicoViewModel = new NivelAcademicoViewModel();
            nivelAcademicoViewModel.Cargo = new Cargo();
            nivelAcademicoViewModel.NivelAcademico = new NivelAcademicoCargo();

            nivelAcademicoViewModel.TiposEducacion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEducacion));
            nivelAcademicoViewModel.TiposEducacion.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            nivelAcademicoViewModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoArea));
            nivelAcademicoViewModel.AreasEstudio.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            nivelAcademicoViewModel.NivelesAlcanzados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.NivelAlcanzado));
            nivelAcademicoViewModel.NivelesAlcanzados.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return nivelAcademicoViewModel;
        }

        [HttpPost]
        public ActionResult eliminarNivelAcademico(int ideNivelAcademico)
        {
            ActionResult result = null;

            var nivelAcademicoEliminar = new NivelAcademicoCargo();
            nivelAcademicoEliminar = _nivelAcademicoCargoRepository.GetSingle(x => x.IdeNivelAcademicoCargo == ideNivelAcademico);
            _nivelAcademicoCargoRepository.Remove(nivelAcademicoEliminar);

            return result;
        }

        #endregion

    }
}
