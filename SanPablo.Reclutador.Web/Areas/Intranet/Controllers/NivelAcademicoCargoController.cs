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

    [Authorize]
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
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<NivelAcademicoCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_nivelAcademicoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoEducacion,
                                item.DescripcionAreaEstudio,
                                item.DescripcionNivelAlcanzado,
                                item.CicloSemestre.ToString(),
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
            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    objJsonMessage.Mensaje = "Verifique que haya ingresado los datos obligatorios y que el puntaje sea mayor a cero";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
               
                else
                {
                    if (nivelAcademicoCargo.IdeNivelAcademicoCargo == 0)
                    {
                        if (existe(nivelAcademicoCargo.TipoEducacion))
                        {
                            objJsonMessage.Mensaje = "El tipo de Educación ya existe, no puede agregar otro igual";
                            objJsonMessage.Resultado = false;
                            return Json(objJsonMessage);
                        }
                        else
                        {
                            nivelAcademicoCargo.EstadoActivo = "A";
                            nivelAcademicoCargo.FechaCreacion = FechaCreacion;
                            nivelAcademicoCargo.UsuarioCreacion = "YO";
                            nivelAcademicoCargo.FechaModificacion = FechaCreacion;
                            nivelAcademicoCargo.Cargo = new Cargo();
                            nivelAcademicoCargo.Cargo.IdeCargo = IdeCargo;
                            _nivelAcademicoCargoRepository.Add(nivelAcademicoCargo);
                            _nivelAcademicoCargoRepository.actualizarPuntaje(Convert.ToInt32(nivelAcademicoCargo.PuntajeNivelEstudio), 0, IdeCargo);

                            objJsonMessage.Mensaje = "Agregado Correctamente";
                            objJsonMessage.Resultado = true;
                            return Json(objJsonMessage);
                        }
                    }
                    else
                    {
                         int contador = _nivelAcademicoCargoRepository.CountByExpress(x => x.TipoEducacion == nivelAcademicoCargo.TipoEducacion && x.Cargo.IdeCargo == IdeCargo && x.IdeNivelAcademicoCargo != nivelAcademicoCargo.IdeNivelAcademicoCargo);

                         if (contador > 0)
                         {
                             objJsonMessage.Mensaje = "El tipo de Educación ya existe, no puede agregar otro igual";
                             objJsonMessage.Resultado = false;
                             return Json(objJsonMessage);
                         }
                         else
                         {
                             var nivelAcedemicoCargoActualizar = _nivelAcademicoCargoRepository.GetSingle(x => x.IdeNivelAcademicoCargo == nivelAcademicoCargo.IdeNivelAcademicoCargo);
                             int valorAnterior = Convert.ToInt32(nivelAcedemicoCargoActualizar.PuntajeNivelEstudio);
                             nivelAcedemicoCargoActualizar.TipoEducacion = nivelAcademicoCargo.TipoEducacion;
                             nivelAcedemicoCargoActualizar.TipoAreaEstudio = nivelAcademicoCargo.TipoAreaEstudio;
                             nivelAcedemicoCargoActualizar.TipoNivelAlcanzado = nivelAcademicoCargo.TipoNivelAlcanzado;
                             nivelAcedemicoCargoActualizar.CicloSemestre = nivelAcademicoCargo.CicloSemestre;
                             nivelAcedemicoCargoActualizar.PuntajeNivelEstudio = nivelAcademicoCargo.PuntajeNivelEstudio;
                             nivelAcedemicoCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                             nivelAcedemicoCargoActualizar.FechaModificacion = FechaModificacion;
                             _nivelAcademicoCargoRepository.Update(nivelAcedemicoCargoActualizar);
                             _nivelAcademicoCargoRepository.actualizarPuntaje(Convert.ToInt32(nivelAcademicoCargo.PuntajeNivelEstudio), valorAnterior, IdeCargo);

                             objJsonMessage.Mensaje = "Editado Correctamente";
                             objJsonMessage.Resultado = true;
                             return Json(objJsonMessage);
                         }
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

        public NivelAcademicoViewModel inicializarNivelAcademico()
        {
            var nivelAcademicoViewModel = new NivelAcademicoViewModel();
            nivelAcademicoViewModel.Cargo = new Cargo();
            nivelAcademicoViewModel.NivelAcademico = new NivelAcademicoCargo();

            nivelAcademicoViewModel.TiposEducacion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEducacion));
            nivelAcademicoViewModel.TiposEducacion.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            nivelAcademicoViewModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoArea));
            nivelAcademicoViewModel.AreasEstudio.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            nivelAcademicoViewModel.NivelesAlcanzados = new List<DetalleGeneral>();
            nivelAcademicoViewModel.NivelesAlcanzados.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return nivelAcademicoViewModel;
        }

        /// <summary>
        /// Eliminar Item
        /// </summary>
        /// <param name="ideNivelAcademico"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult eliminarNivelAcademico(int ideNivelAcademico)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            ActionResult result = null;
            var nivelAcademicoEliminar = new NivelAcademicoCargo();
            nivelAcademicoEliminar = _nivelAcademicoCargoRepository.GetSingle(x => x.IdeNivelAcademicoCargo == ideNivelAcademico);
            int puntajeEliminar = Convert.ToInt32(nivelAcademicoEliminar.PuntajeNivelEstudio);
            _nivelAcademicoCargoRepository.Remove(nivelAcademicoEliminar);
            _nivelAcademicoCargoRepository.actualizarPuntaje(0,puntajeEliminar,IdeCargo);
            return result;
        }

        /// <summary>
        /// Validar q el tipo de Educacion no sea duplicado
        /// </summary>
        /// <param name="tipoEducacion"></param>
        /// <returns></returns>
        public bool existe(string tipoEducacion)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = _nivelAcademicoCargoRepository.CountByExpress(x => x.TipoEducacion == tipoEducacion && x.Cargo.IdeCargo == IdeCargo);

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }

        [HttpPost]
        public ActionResult listarNivelAlcanzado(string tipoEducacion)
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();

            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoEducacion, tipoEducacion));
            listaResultado.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTRO" });

            result = Json(listaResultado);
            return result;
        }

        #endregion

    }
}
