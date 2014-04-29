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
    public class ConocimientosCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IConocimientoGeneralCargoRepository _conocimientoCargoRepository;


        public ConocimientosCargoController(ICargoRepository cargoRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                IConocimientoGeneralCargoRepository conocimientoCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _conocimientoCargoRepository = conocimientoCargoRepository;
        }

         #region OFIMATICA

        [HttpPost]
        public virtual JsonResult ListaOfimatica(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.IsNotNull("TipoConocimientoOfimatica"));
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoOfimatica,
                                item.DescripcionNombreOfimatica,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Ofimatica(string id)
        {
            var ofimaticaViewModel = inicializarOfimatica();
            if (id != "0")
            {
                var ofimatica = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == Convert.ToInt32(id));
                ofimaticaViewModel.Conocimiento = ofimatica;
            }
            return View(ofimaticaViewModel);
        }

        [HttpPost]
        public ActionResult Ofimatica([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo conocimientoGeneralCargo)
        {

            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var conocimientoCargoViewModel = inicializarOfimatica();
                    conocimientoCargoViewModel.Conocimiento = conocimientoGeneralCargo;
                    return View(conocimientoCargoViewModel);
                }
                if (conocimientoGeneralCargo.IdeConocimientoGeneralCargo == 0)
                {
                    if (existeOfimatica(conocimientoGeneralCargo.TipoNombreOfimatica))
                    {
                        objJsonMessage.Mensaje = "No puede insertar la misma descirpción más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        conocimientoGeneralCargo.EstadoActivo = IndicadorActivo.Activo;
                        conocimientoGeneralCargo.FechaCreacion = FechaCreacion;
                        conocimientoGeneralCargo.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                        conocimientoGeneralCargo.FechaModificacion = FechaCreacion;
                        conocimientoGeneralCargo.Cargo = new Cargo();
                        conocimientoGeneralCargo.Cargo.IdeCargo = IdeCargo;

                        _conocimientoCargoRepository.Add(conocimientoGeneralCargo);
                        _conocimientoCargoRepository.actualizarPuntaje(conocimientoGeneralCargo.PuntajeConocimiento, 0, IdeCargo, "Ofimatica");

                        objJsonMessage.Mensaje = "Agregado Correctamente";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);

                    }
                }
                else
                {
                    var ofimaticaCargoActualizar = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == conocimientoGeneralCargo.IdeConocimientoGeneralCargo);

                    int contador = _conocimientoCargoRepository.CountByExpress(x => x.TipoNombreOfimatica == conocimientoGeneralCargo.TipoNombreOfimatica
                                                                               && x.Cargo.IdeCargo == CargoPerfil.IdeCargo && x.IdeConocimientoGeneralCargo != conocimientoGeneralCargo.IdeConocimientoGeneralCargo);

                    if (contador > 0)
                    {
                        objJsonMessage.Mensaje = "No puede insertar la misma descripción más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        int valorEliminar = ofimaticaCargoActualizar.PuntajeConocimiento;
                        ofimaticaCargoActualizar.TipoConocimientoOfimatica = conocimientoGeneralCargo.TipoConocimientoOfimatica;
                        ofimaticaCargoActualizar.TipoNombreOfimatica = conocimientoGeneralCargo.TipoNombreOfimatica;
                        ofimaticaCargoActualizar.TipoNivelConocimiento = conocimientoGeneralCargo.TipoNivelConocimiento;
                        ofimaticaCargoActualizar.PuntajeConocimiento = conocimientoGeneralCargo.PuntajeConocimiento;
                        ofimaticaCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        ofimaticaCargoActualizar.FechaModificacion = FechaModificacion;
                        _conocimientoCargoRepository.Update(ofimaticaCargoActualizar);
                        _conocimientoCargoRepository.actualizarPuntaje(conocimientoGeneralCargo.PuntajeConocimiento, valorEliminar, IdeCargo, "Ofimatica");

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

        public ConocimientoCargoViewModel inicializarOfimatica()
        {
            var conocimientoCargoViewModel = new ConocimientoCargoViewModel();
            conocimientoCargoViewModel.Cargo = new Cargo();
            conocimientoCargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            conocimientoCargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoOfimatica));
            conocimientoCargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            conocimientoCargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TiponombreOfimatica));
            conocimientoCargoViewModel.DescripcionConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            conocimientoCargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoCargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            return conocimientoCargoViewModel;
        }

        [HttpPost]
        public ActionResult eliminarOfimatica(int ideOfimatica)
        {
            ActionResult result = null;
            int IdeCargo = CargoPerfil.IdeCargo;
            var ofimaticaEliminar = new ConocimientoGeneralCargo();
            ofimaticaEliminar = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == ideOfimatica);
            int valorEliminar = ofimaticaEliminar.PuntajeConocimiento;
            _conocimientoCargoRepository.Remove(ofimaticaEliminar);
            _conocimientoCargoRepository.actualizarPuntaje(0, valorEliminar, IdeCargo, "Ofimatica");

            return result;
        }

        #endregion

        #region IDIOMA

        [HttpPost]
        public virtual JsonResult ListaIdioma(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));
                where.Add(Expression.IsNotNull("TipoIdioma"));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionIdioma,
                                item.DescripcionConocimientoIdioma,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Idioma(string id)
        {
            var idiomaViewModel = inicializarIdioma();
            if (id != "0")
            {
                var idioma = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == Convert.ToInt32(id));
                idiomaViewModel.Conocimiento = idioma;
            }
            return View(idiomaViewModel);
        }

        [HttpPost]
        public ActionResult Idioma([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo idiomaCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var idiomaCargoViewModel = inicializarIdioma();
                    idiomaCargoViewModel.Conocimiento = idiomaCargo;
                    return View(idiomaCargoViewModel);
                }
                if (idiomaCargo.IdeConocimientoGeneralCargo == 0)
                {
                    if (existeIdioma(idiomaCargo.TipoIdioma))
                    {
                        objJsonMessage.Mensaje = "No puede agregar el mismo idioma más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        idiomaCargo.EstadoActivo = IndicadorActivo.Activo;
                        idiomaCargo.FechaCreacion = FechaCreacion;
                        idiomaCargo.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                        idiomaCargo.FechaModificacion = FechaCreacion;
                        idiomaCargo.Cargo = new Cargo();
                        idiomaCargo.Cargo.IdeCargo = IdeCargo;

                        _conocimientoCargoRepository.Add(idiomaCargo);
                        _conocimientoCargoRepository.actualizarPuntaje(idiomaCargo.PuntajeConocimiento, 0, IdeCargo, "Idioma");

                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    var idiomaCargoActualizar = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == idiomaCargo.IdeConocimientoGeneralCargo);

                    int contador = _conocimientoCargoRepository.CountByExpress(x => x.TipoIdioma == idiomaCargo.TipoIdioma && x.Cargo.IdeCargo == CargoPerfil.IdeCargo
                                                                               && x.IdeConocimientoGeneralCargo != idiomaCargo.IdeConocimientoGeneralCargo);

                    if (contador > 0)
                    {
                        objJsonMessage.Mensaje = "No puede agregar el mismo idioma más de una vez";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        int valorEliminar = idiomaCargoActualizar.PuntajeConocimiento;
                        idiomaCargoActualizar.TipoConocimientoIdioma = idiomaCargo.TipoConocimientoIdioma;
                        idiomaCargoActualizar.TipoIdioma = idiomaCargo.TipoIdioma;
                        idiomaCargoActualizar.TipoNivelConocimiento = idiomaCargo.TipoNivelConocimiento;
                        idiomaCargoActualizar.PuntajeConocimiento = idiomaCargo.PuntajeConocimiento;
                        idiomaCargoActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        idiomaCargoActualizar.FechaModificacion = FechaModificacion;
                        _conocimientoCargoRepository.Update(idiomaCargo);
                        _conocimientoCargoRepository.actualizarPuntaje(idiomaCargo.PuntajeConocimiento, valorEliminar, IdeCargo, "Idioma");

                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }

                }

                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }

        public ConocimientoCargoViewModel inicializarIdioma()
        {
            var conocimientoCargoViewModel = new ConocimientoCargoViewModel();
            conocimientoCargoViewModel.Cargo = new Cargo();
            conocimientoCargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            conocimientoCargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoIdioma));
            conocimientoCargoViewModel.DescripcionConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            conocimientoCargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoIdioma));
            conocimientoCargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            conocimientoCargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoCargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            return conocimientoCargoViewModel;
        }

        [HttpPost]
        public ActionResult eliminarIdioma(int ideIdioma)
        {
            ActionResult result = null;
            int IdeCargo = CargoPerfil.IdeCargo;
            var idiomaEliminar = new ConocimientoGeneralCargo();
            idiomaEliminar = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == ideIdioma);
            int valorEliminar = idiomaEliminar.PuntajeConocimiento;
            _conocimientoCargoRepository.Remove(idiomaEliminar);
            _conocimientoCargoRepository.actualizarPuntaje(0, valorEliminar, IdeCargo, "Idioma");

            return result;
        }

        #endregion

        #region OTROS CONOCIMIENTOS

        [HttpPost]
        public virtual JsonResult ListaOtrosConocimientos(GridTable grid)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.IsNotNull("TipoConocimientoGeneral"));
                where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));


                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoGeneral,
                                item.DescripcionNombreConocimientoGeneral,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult OtrosConocimientos(string id)
        {
            var otrosConocimientosViewModel = inicializarOtrosConocimientos();
            if (id != "0")
            {
                var otrosConocimientos = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == Convert.ToInt32(id));
                //actualizar la descripcion de conocimiento
                otrosConocimientosViewModel.DescripcionConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoConocimientoGral, otrosConocimientos.TipoConocimientoGeneral.ToString()));
                otrosConocimientosViewModel.Conocimiento = otrosConocimientos;
            }
            return View(otrosConocimientosViewModel);
        }

        [HttpPost]
        public ActionResult OtrosConocimientos([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo conocimientoCargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var otrosConocimientosViewModel = inicializarOtrosConocimientos();
                    otrosConocimientosViewModel.Conocimiento = conocimientoCargo;
                    return View(otrosConocimientosViewModel);
                }
                if (conocimientoCargo.IdeConocimientoGeneralCargo == 0)
                {
                    if (existeGenerales(conocimientoCargo.TipoNombreConocimientoGeneral))
                    {
                        objJsonMessage.Mensaje = "No puede agregar, conocimientos duplicados";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        conocimientoCargo.EstadoActivo = IndicadorActivo.Activo;
                        conocimientoCargo.FechaCreacion = FechaCreacion;
                        conocimientoCargo.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                        conocimientoCargo.FechaModificacion = FechaCreacion;
                        conocimientoCargo.Cargo = new Cargo();
                        conocimientoCargo.Cargo.IdeCargo = IdeCargo;

                        _conocimientoCargoRepository.Add(conocimientoCargo);
                        _conocimientoCargoRepository.actualizarPuntaje(conocimientoCargo.PuntajeConocimiento, 0, IdeCargo, "Otros");
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);

                    }
                }
                else
                {
                    var otrosConocimientosActualizar = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == conocimientoCargo.IdeConocimientoGeneralCargo);

                    int contador = _conocimientoCargoRepository.CountByExpress(x => x.TipoNombreConocimientoGeneral == conocimientoCargo.TipoNombreConocimientoGeneral
                                                                                && x.Cargo.IdeCargo == CargoPerfil.IdeCargo && x.IdeConocimientoGeneralCargo != conocimientoCargo.IdeConocimientoGeneralCargo);

                    if (contador > 0)
                    {
                        objJsonMessage.Mensaje = "No puede agregar, conocimientos duplicados";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        int valorEliminar = otrosConocimientosActualizar.PuntajeConocimiento;
                        otrosConocimientosActualizar.TipoConocimientoGeneral = conocimientoCargo.TipoConocimientoGeneral;
                        otrosConocimientosActualizar.TipoNombreConocimientoGeneral = conocimientoCargo.TipoNombreConocimientoGeneral;
                        otrosConocimientosActualizar.TipoNivelConocimiento = conocimientoCargo.TipoNivelConocimiento;
                        otrosConocimientosActualizar.PuntajeConocimiento = conocimientoCargo.PuntajeConocimiento;
                        otrosConocimientosActualizar.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        otrosConocimientosActualizar.FechaModificacion = FechaModificacion;
                        _conocimientoCargoRepository.Update(otrosConocimientosActualizar);
                        _conocimientoCargoRepository.actualizarPuntaje(conocimientoCargo.PuntajeConocimiento, valorEliminar, IdeCargo, "Otros");

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

        public ConocimientoCargoViewModel inicializarOtrosConocimientos()
        {
            var conocimientoCargoViewModel = new ConocimientoCargoViewModel();
            conocimientoCargoViewModel.Cargo = new Cargo();
            conocimientoCargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            conocimientoCargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoGral));
            conocimientoCargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            conocimientoCargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>();
            conocimientoCargoViewModel.DescripcionConocimiento.Add(new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            conocimientoCargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoCargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONAR" });

            return conocimientoCargoViewModel;
        }

        [HttpPost]
        public ActionResult eliminarOtrosConocimientos(int ideOtrosConocimientos)
        {
            ActionResult result = null;
            int IdeCargo = CargoPerfil.IdeCargo;
            var otrosConocimientosEliminar = new ConocimientoGeneralCargo();
            otrosConocimientosEliminar = _conocimientoCargoRepository.GetSingle(x => x.IdeConocimientoGeneralCargo == ideOtrosConocimientos);
            int valorEliminar = otrosConocimientosEliminar.PuntajeConocimiento;
            _conocimientoCargoRepository.Remove(otrosConocimientosEliminar);
            _conocimientoCargoRepository.actualizarPuntaje(0, valorEliminar, IdeCargo, "Otros");

            return result;
        }

        public ActionResult listarNombreConocimiento(string tipoConocimiento)
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();
            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoConocimientoGral, tipoConocimiento));
            result = Json(listaResultado);
            return result;
        }

        public void actualizarOtrosConocimientos(ConocimientoCargoViewModel conocimientoModel, ConocimientoGeneralCargo conocimiento)
        {
            var listaResultado = new List<DetalleGeneral>();
            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoConocimientoGral, conocimiento.TipoConocimientoGeneral.ToString()));
            conocimientoModel.TipoNombresConocimientosGrales = listaResultado;

        }

        public bool existeOfimatica(string descripcion)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = _conocimientoCargoRepository.CountByExpress(x => x.TipoNombreOfimatica == descripcion && x.Cargo.IdeCargo == IdeCargo);

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }

        public bool existeIdioma(string descripcion)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = _conocimientoCargoRepository.CountByExpress(x => x.TipoIdioma == descripcion && x.Cargo.IdeCargo == IdeCargo);

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }

        public bool existeGenerales(string descripcion)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            bool result = false;
            int contador = _conocimientoCargoRepository.CountByExpress(x => x.TipoNombreConocimientoGeneral == descripcion && x.Cargo.IdeCargo == IdeCargo);

            if (contador > 0)
            {
                result = true;
            }

            return result;
        }
        #endregion

    }
}
