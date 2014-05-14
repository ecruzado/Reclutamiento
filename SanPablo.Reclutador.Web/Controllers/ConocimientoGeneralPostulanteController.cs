namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity.Validation;
   
    public class ConocimientoGeneralPostulanteController : BaseController
    {
        private IConocimientoGeneralPostulanteRepository _conocimientoGeneralPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IPostulanteRepository _postulanteRepository;

        public ConocimientoGeneralPostulanteController(IConocimientoGeneralPostulanteRepository conocimientoGeneralPostulanteRepository, 
                                                       IDetalleGeneralRepository detalleGeneralRepository,
                                                       IPostulanteRepository postulanteRepository)
        {
            _conocimientoGeneralPostulanteRepository = conocimientoGeneralPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _postulanteRepository = postulanteRepository;
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Index()
        {
            var conocimientoViewModel = InicializarConocimiento();
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            conocimientoViewModel.ConocimientoGeneral.Postulante = postulante;
            return View(conocimientoViewModel);
        }

        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;
                
                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralPostulante>();
                where.Add(Expression.IsNotNull("TipoConocimientoOfimatica"));
                where.Add(Expression.Eq("Postulante.IdePostulante",IdePostulante));

                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.DescripcionConocimientoOfimatica,
                                item.DescripcionNombreOfimatica,
                                item.DescripcionNivelConocimiento,
                                item.Certificacion.ToString()
                                
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        [HttpPost]
        public virtual JsonResult ListarIdiomas(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralPostulante>();
                where.Add(Expression.IsNotNull("TipoIdioma"));
                where.Add(Expression.Eq("Postulante.IdePostulante", IdePostulante));

                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.DescripcionIdioma,
                                item.DescripcionConocimientoIdioma,
                                item.DescripcionNivelConocimiento,
                                item.Certificacion.ToString()
                                
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        [HttpPost]
        public virtual JsonResult ListarOtrosConocimientos(GridTable grid) 
        {
            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralPostulante>();
                where.Add(Expression.IsNotNull("TipoConocimientoGeneral"));
                where.Add(Expression.Eq("Postulante.IdePostulante", IdePostulante));

                var generic = Listar(_conocimientoGeneralPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralPostulante.ToString(),
                        cell = new string[]
                            {
                                item.IdeConocimientoGeneralPostulante.ToString(),
                                item.DescripcionConocimientoGeneral,
                                item.DescripcionNombreConocimientoGeneral==null?"":item.DescripcionNombreConocimientoGeneral.ToUpper(),
                                item.DescripcionNivelConocimiento,
                                item.Certificacion.ToString()
                                
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ViewResult Ofimatica(string id)
        {
            var conocimientoGeneralViewModel = InicializarConocimiento();
            if (id == "0")
            {
                return View(conocimientoGeneralViewModel);
            }
            else
            {
                var conocimientoResultado = new ConocimientoGeneralPostulante();
                conocimientoResultado = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == Convert.ToInt32(id));
                conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoResultado;
                return View(conocimientoGeneralViewModel);
            }
        }


        [HttpPost]
        public ActionResult Ofimatica([Bind(Prefix = "ConocimientoGeneral")]ConocimientoGeneralPostulante conocimientoGeneralPostulante)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                var objetoUsuario = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];
                string usuarioActual = objetoUsuario.CodUsuario.Length <= 15 ? objetoUsuario.CodUsuario : objetoUsuario.CodUsuario.Substring(0, 15);

                ConocimientoGeneralPostulanteValidator validator = new ConocimientoGeneralPostulanteValidator();
                ValidationResult result = validator.Validate(conocimientoGeneralPostulante, "TipoConocimientoOfimatica", "TipoNombreOfimatica", "TipoNivelConocimiento");

                if (!result.IsValid)
                {
                    var conocimientoGeneralViewModel = InicializarConocimiento();
                    conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoGeneralPostulante;
                    return View(conocimientoGeneralViewModel);
                }
                if (!existeOfimatica(conocimientoGeneralPostulante))
                {
                    if (conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante == 0)
                    {
                        conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
                        conocimientoGeneralPostulante.FechaCreacion = FechaCreacion;
                        conocimientoGeneralPostulante.UsuarioCreacion = usuarioActual;
                        var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                        postulante.agregarConocimiento(conocimientoGeneralPostulante);
                        _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        var conocimientoEdit = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante);
                        conocimientoEdit.TipoNombreOfimatica = conocimientoGeneralPostulante.TipoNombreOfimatica;
                        conocimientoEdit.TipoNivelConocimiento = conocimientoGeneralPostulante.TipoNivelConocimiento;
                        conocimientoEdit.TipoConocimientoOfimatica = conocimientoGeneralPostulante.TipoConocimientoOfimatica;
                        conocimientoEdit.IndicadorCertificacion = conocimientoGeneralPostulante.IndicadorCertificacion;
                        conocimientoEdit.FechaModificacion = FechaModificacion;
                        conocimientoEdit.UsuarioModificacion = usuarioActual;
                        _conocimientoGeneralPostulanteRepository.Update(conocimientoEdit);
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    objJsonMessage.Mensaje = "ERROR: Ud. ya ingreso este conocimiento de ofimática, verifique la información a ingresar";
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

        /// <summary>
        /// validar si estiste el conocimiento ofimatica 
        /// </summary>
        /// <param name="ofimatica"></param>
        /// <returns></returns>
        public bool existeOfimatica(ConocimientoGeneralPostulante ofimatica)
        {
            bool respuesta = false;
            int nroOfimatica = 0;

            if (ofimatica.IdeConocimientoGeneralPostulante == 0)
            {
                nroOfimatica = _conocimientoGeneralPostulanteRepository.CountByExpress(x => x.Postulante.IdePostulante == IdePostulante
                                                                                       && x.TipoConocimientoOfimatica == ofimatica.TipoConocimientoOfimatica
                                                                                       && x.TipoNombreOfimatica == ofimatica.TipoNombreOfimatica
                                                                                       && x.TipoNivelConocimiento == ofimatica.TipoNivelConocimiento);
            }
            else
            {
                nroOfimatica = _conocimientoGeneralPostulanteRepository.CountByExpress(x => x.Postulante.IdePostulante == IdePostulante
                                                                                       && x.TipoConocimientoOfimatica == ofimatica.TipoConocimientoOfimatica
                                                                                       && x.TipoNombreOfimatica == ofimatica.TipoNombreOfimatica
                                                                                       && x.TipoNivelConocimiento == ofimatica.TipoNivelConocimiento
                                                                                       && x.IdeConocimientoGeneralPostulante != ofimatica.IdeConocimientoGeneralPostulante);
            }
            if (nroOfimatica > 0)
            {
                respuesta = true;
            }
            else
            {
                respuesta = false;
            }
            

            return respuesta;
 
        }

        /// <summary>
        /// validar si estiste el conocimiento idioma
        /// </summary>
        /// <param name="idioma"></param>
        /// <returns></returns>
        public bool existeIdioma(ConocimientoGeneralPostulante idioma)
        {
            bool respuesta = false;
            int nroIdioma = 0;

            if (idioma.IdeConocimientoGeneralPostulante == 0)
            {
                nroIdioma = _conocimientoGeneralPostulanteRepository.CountByExpress(x => x.Postulante.IdePostulante == IdePostulante
                                                                                       && x.TipoConocimientoIdioma == idioma.TipoConocimientoIdioma
                                                                                       && x.TipoIdioma == idioma.TipoIdioma
                                                                                       && x.TipoNivelConocimiento == idioma.TipoNivelConocimiento);
            }
            else
            {
                nroIdioma = _conocimientoGeneralPostulanteRepository.CountByExpress(x => x.Postulante.IdePostulante == IdePostulante
                                                                                       && x.TipoConocimientoIdioma == idioma.TipoConocimientoIdioma
                                                                                       && x.TipoIdioma == idioma.TipoIdioma
                                                                                       && x.TipoNivelConocimiento == idioma.TipoNivelConocimiento
                                                                                       && x.IdeConocimientoGeneralPostulante != idioma.IdeConocimientoGeneralPostulante);
            }
            if (nroIdioma > 0)
            {
                respuesta = true;
            }
            else
            {
                respuesta = false;
            }


            return respuesta;

        }

        /// <summary>
        /// validar si estiste el conocimiento otro
        /// </summary>
        /// <param name="otroConocimiento"></param>
        /// <returns></returns>
        public bool existeOtro(ConocimientoGeneralPostulante otroConocimiento)
        {
            bool respuesta = false;
            int nroOtro = 0;

            if (otroConocimiento.IdeConocimientoGeneralPostulante == 0)
            {
                nroOtro = _conocimientoGeneralPostulanteRepository.CountByExpress(x => x.Postulante.IdePostulante == IdePostulante
                                                                                       && x.TipoConocimientoGeneral == otroConocimiento.TipoConocimientoGeneral
                                                                                       && x.TipoNombreConocimientoGeneral == otroConocimiento.TipoNombreConocimientoGeneral
                                                                                       && x.NombreConocimientoGeneral == otroConocimiento.NombreConocimientoGeneral
                                                                                       && x.TipoNivelConocimiento == otroConocimiento.TipoNivelConocimiento);
            }
            else
            {
                nroOtro = _conocimientoGeneralPostulanteRepository.CountByExpress(x => x.Postulante.IdePostulante == IdePostulante
                                                                                       && x.TipoConocimientoGeneral == otroConocimiento.TipoConocimientoGeneral
                                                                                       && x.TipoNombreConocimientoGeneral == otroConocimiento.TipoNombreConocimientoGeneral
                                                                                       && x.NombreConocimientoGeneral == otroConocimiento.NombreConocimientoGeneral
                                                                                       && x.TipoNivelConocimiento == otroConocimiento.TipoNivelConocimiento
                                                                                       && x.IdeConocimientoGeneralPostulante != otroConocimiento.IdeConocimientoGeneralPostulante);
            }
            if (nroOtro > 0)
            {
                respuesta = true;
            }
            else
            {
                respuesta = false;
            }


            return respuesta;

        }

        public ConocimientoPostulanteGeneralViewModel InicializarConocimiento()
        {
            var conocimientoPostulanteGeneralViewModel = new ConocimientoPostulanteGeneralViewModel();

            conocimientoPostulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);
            conocimientoPostulanteGeneralViewModel.ConocimientoGeneral = new ConocimientoGeneralPostulante();

            conocimientoPostulanteGeneralViewModel.TiposConocimientoOfimatica = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoOfimatica));
            conocimientoPostulanteGeneralViewModel.TiposConocimientoOfimatica.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            
            conocimientoPostulanteGeneralViewModel.TipoNombresOfimatica = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TiponombreOfimatica));
            conocimientoPostulanteGeneralViewModel.TipoNombresOfimatica.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });


            return conocimientoPostulanteGeneralViewModel;
        }


        [HttpPost]
        public ActionResult eliminarConocimiento(int ideConocimiento)
        {
            ActionResult result = null;

            var conocimientoEliminar = new ConocimientoGeneralPostulante();
            conocimientoEliminar = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == ideConocimiento);
            int antes = _conocimientoGeneralPostulanteRepository.CountBy();
            _conocimientoGeneralPostulanteRepository.Remove(conocimientoEliminar);
            int despues = _conocimientoGeneralPostulanteRepository.CountBy();

            return result;
        }

        #region IDIOMAS

        public ViewResult Idiomas(string id)
        {
            var conocimientoGeneralViewModel = InicializarIdiomas();
            if (id == "0")
            {
                return View(conocimientoGeneralViewModel);
            }
            else
            {
                var conocimientoResultado = new ConocimientoGeneralPostulante();
                conocimientoResultado = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == Convert.ToInt32(id));
                conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoResultado;
                return View(conocimientoGeneralViewModel);
            }
        }


        [HttpPost]
        public ActionResult Idiomas([Bind(Prefix = "ConocimientoGeneral")]ConocimientoGeneralPostulante conocimientoGeneralPostulante)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                var objetoUsuario = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];
                string usuarioActual = objetoUsuario.CodUsuario.Length <= 15 ? objetoUsuario.CodUsuario : objetoUsuario.CodUsuario.Substring(0, 15);

                ConocimientoGeneralPostulanteValidator validator = new ConocimientoGeneralPostulanteValidator();
                ValidationResult result = validator.Validate(conocimientoGeneralPostulante, "TipoIdioma", "TipoConocimientoIdioma", "TipoNivelConocimiento");

                if (!result.IsValid)
                {
                    var conocimientoGeneralViewModel = InicializarIdiomas();
                    conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoGeneralPostulante;
                    return View(conocimientoGeneralViewModel);
                }
                else
                {
                    if (!existeIdioma(conocimientoGeneralPostulante))
                    {
                        if (conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante == 0)
                        {
                            conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
                            conocimientoGeneralPostulante.FechaCreacion = FechaCreacion;
                            conocimientoGeneralPostulante.UsuarioCreacion = usuarioActual;
                            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                            postulante.agregarConocimiento(conocimientoGeneralPostulante);
                            _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
                            objJsonMessage.Resultado = true;
                            return Json(objJsonMessage);

                        }
                        else
                        {
                            var conocimientoEdit = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante);
                            conocimientoEdit.TipoNivelConocimiento = conocimientoGeneralPostulante.TipoNivelConocimiento;
                            conocimientoEdit.TipoIdioma = conocimientoGeneralPostulante.TipoIdioma;
                            conocimientoEdit.TipoConocimientoIdioma = conocimientoGeneralPostulante.TipoConocimientoIdioma;
                            conocimientoEdit.IndicadorCertificacion = conocimientoGeneralPostulante.IndicadorCertificacion;
                            conocimientoEdit.FechaModificacion = FechaModificacion;
                            conocimientoEdit.UsuarioModificacion = usuarioActual;
                            _conocimientoGeneralPostulanteRepository.Update(conocimientoEdit);
                            objJsonMessage.Resultado = true;
                            return Json(objJsonMessage);
                        }
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "ERROR: Ud. ya ingreso este conocimiento de idioma, verifique la información a ingresar";
                        objJsonMessage.Resultado = false;
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


        public ConocimientoPostulanteGeneralViewModel InicializarIdiomas()
        {
            var conocimientoPostulanteGeneralViewModel = new ConocimientoPostulanteGeneralViewModel();
            conocimientoPostulanteGeneralViewModel.ConocimientoGeneral = new ConocimientoGeneralPostulante();

            conocimientoPostulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);

            conocimientoPostulanteGeneralViewModel.TipoIdiomas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoIdioma));
            conocimientoPostulanteGeneralViewModel.TipoIdiomas.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });


            conocimientoPostulanteGeneralViewModel.TipoConocimientoIdiomas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoIdioma));
            conocimientoPostulanteGeneralViewModel.TipoConocimientoIdiomas.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });


            return conocimientoPostulanteGeneralViewModel;
        }
        #endregion


        #region OTROCONOCIMIENTO

        public ViewResult OtroConocimiento(string id)
        {
            var conocimientoGeneralViewModel = InicializarOtroConocimiento();
            if (id == "0")
            {
                return View(conocimientoGeneralViewModel);
            }
            else
            {
                int ideConocimientoEdit = Convert.ToInt32(id);
                var conocimientoResultado = new ConocimientoGeneralPostulante();
                conocimientoResultado = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == ideConocimientoEdit);
                conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoResultado;
                actualizarOtrosConocimientos(conocimientoGeneralViewModel);
                return View(conocimientoGeneralViewModel);
            }
        }


        [HttpPost]
        public ActionResult OtroConocimiento([Bind(Prefix = "ConocimientoGeneral")]ConocimientoGeneralPostulante conocimientoGeneralPostulante)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                var objetoUsuario = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];
                string usuarioActual = objetoUsuario.CodUsuario.Length <= 15 ? objetoUsuario.CodUsuario : objetoUsuario.CodUsuario.Substring(0, 15);

                ConocimientoGeneralPostulanteValidator validator = new ConocimientoGeneralPostulanteValidator();
                ValidationResult result = validator.Validate(conocimientoGeneralPostulante, "TipoConocimientoGeneral", "TipoNombreConocimientoGeneral", "NombreConocimientoGeneral", "TipoNivelConocimiento");

                if (!result.IsValid)
                {
                    var conocimientoGeneralViewModel = InicializarOtroConocimiento();
                    conocimientoGeneralViewModel.ConocimientoGeneral = conocimientoGeneralPostulante;
                    return View(conocimientoGeneralViewModel);
                }
                if (!existeOtro(conocimientoGeneralPostulante))
                {
                    if (conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante == 0)
                    {
                        conocimientoGeneralPostulante.EstadoActivo = IndicadorActivo.Activo;
                        conocimientoGeneralPostulante.FechaCreacion = FechaCreacion;
                        conocimientoGeneralPostulante.UsuarioCreacion = usuarioActual;
                        var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                        postulante.agregarConocimiento(conocimientoGeneralPostulante);
                        _conocimientoGeneralPostulanteRepository.Add(conocimientoGeneralPostulante);
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);

                    }
                    else
                    {
                        var conocimientoEdit = _conocimientoGeneralPostulanteRepository.GetSingle(x => x.IdeConocimientoGeneralPostulante == conocimientoGeneralPostulante.IdeConocimientoGeneralPostulante);
                        conocimientoEdit.TipoNombreConocimientoGeneral = conocimientoGeneralPostulante.TipoNombreConocimientoGeneral;
                        conocimientoEdit.TipoNivelConocimiento = conocimientoGeneralPostulante.TipoNivelConocimiento;
                        conocimientoEdit.TipoConocimientoGeneral = conocimientoGeneralPostulante.TipoConocimientoGeneral;
                        conocimientoEdit.NombreConocimientoGeneral = conocimientoGeneralPostulante.NombreConocimientoGeneral;
                        conocimientoEdit.IndicadorCertificacion = conocimientoGeneralPostulante.IndicadorCertificacion;
                        conocimientoEdit.FechaModificacion = FechaModificacion;
                        conocimientoEdit.UsuarioModificacion = usuarioActual;
                        _conocimientoGeneralPostulanteRepository.Update(conocimientoEdit);
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    objJsonMessage.Mensaje = "ERROR: Ud. ya ingreso este conocimiento de otros conocimientos, verifique la información a ingresar";
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

        public ConocimientoPostulanteGeneralViewModel InicializarOtroConocimiento()
        {
            var conocimientoPostulanteGeneralViewModel = new ConocimientoPostulanteGeneralViewModel();

            conocimientoPostulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);
            conocimientoPostulanteGeneralViewModel.ConocimientoGeneral = new ConocimientoGeneralPostulante();

            conocimientoPostulanteGeneralViewModel.TipoConocimientoGenerales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoGral));
            conocimientoPostulanteGeneralViewModel.TipoConocimientoGenerales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            conocimientoPostulanteGeneralViewModel.TipoNombresConocimientosGrales = new List<DetalleGeneral>();
            conocimientoPostulanteGeneralViewModel.TipoNombresConocimientosGrales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });
            

            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            conocimientoPostulanteGeneralViewModel.TipoNivelesConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });


            return conocimientoPostulanteGeneralViewModel;
        }

        public ActionResult listarNombreConocimiento(string tipoConocimiento) 
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();
            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoConocimientoGral, tipoConocimiento));
            listaResultado.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTRO" });
            result = Json(listaResultado);
            return result;
        }
        public void actualizarOtrosConocimientos(ConocimientoPostulanteGeneralViewModel conocimientoModel)
        {
            var listaResultado = new List<DetalleGeneral>();
            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoConocimientoGral, conocimientoModel.ConocimientoGeneral.TipoConocimientoGeneral.ToString()));
            listaResultado.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTRO" });
            conocimientoModel.TipoNombresConocimientosGrales = listaResultado;

        }
        #endregion
    }
}

