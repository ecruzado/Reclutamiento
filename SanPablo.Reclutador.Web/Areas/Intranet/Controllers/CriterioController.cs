namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using FluentValidation;
    using FluentValidation.Results;
    using Newtonsoft.Json;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [Authorize]
    public class CriterioController : BaseController
    {
        private ICriterioRepository _criterioRepository;
        private IAlternativaRepository _alternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICriterioPorSubcategoriaRepository _criterioPorSubcategoriaRepository;
        private ICategoriaRepository _categoriaRepository;

        public CriterioController(ICriterioRepository criterioRepository, 
            IDetalleGeneralRepository detalleGeneralRepository, 
            IAlternativaRepository alternativaRepository,
            ICriterioPorSubcategoriaRepository criterioPorSubcategoriaRepository,
            ICategoriaRepository categoriaRepository
            )
        {
            _criterioRepository = criterioRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _alternativaRepository = alternativaRepository;
            _criterioPorSubcategoriaRepository = criterioPorSubcategoriaRepository;
            _categoriaRepository = categoriaRepository;
        }


        /// <summary>
        /// Inicializa la pantalla inicial
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult Index()
        {
            var criterioViewModel = InicializarCriteriosIndex();
           

            //accesos a los botones

            int idRol = (Session[ConstanteSesion.Rol] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Rol]));

            if (Roles.Administrador_Sistema.Equals(idRol))
            {
                criterioViewModel.btnActivarDesactivar = Visualicion.SI;
                criterioViewModel.btnBuscar = Visualicion.SI;
                criterioViewModel.btnConsultar = Visualicion.SI;
                criterioViewModel.btnEditar = Visualicion.SI;
                criterioViewModel.btnEliminar = Visualicion.SI;
                criterioViewModel.btnLimpiar = Visualicion.SI;
                criterioViewModel.btnNuevo = Visualicion.SI;
                
            }
            else if (Roles.Jefe_Corporativo_Seleccion.Equals(idRol))
            {
                criterioViewModel.btnActivarDesactivar = Visualicion.SI;
                criterioViewModel.btnBuscar = Visualicion.SI;
                criterioViewModel.btnConsultar = Visualicion.SI;
                criterioViewModel.btnEditar = Visualicion.SI;
                criterioViewModel.btnEliminar = Visualicion.SI;
                criterioViewModel.btnLimpiar = Visualicion.SI;
                criterioViewModel.btnNuevo = Visualicion.SI;
                
            }
            else
            {
                criterioViewModel.btnActivarDesactivar = Visualicion.NO;
                criterioViewModel.btnBuscar = Visualicion.NO;
                criterioViewModel.btnConsultar = Visualicion.NO;
                criterioViewModel.btnEditar = Visualicion.NO;
                criterioViewModel.btnEliminar = Visualicion.NO;
                criterioViewModel.btnLimpiar = Visualicion.NO;
                criterioViewModel.btnNuevo = Visualicion.NO;
            }

            return View(criterioViewModel);

        }

        /// <summary>
        /// obtiene la lista de criterios
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaCriterio(GridTable grid)
        {
            try
            {
                DetachedCriteria where = null;

                int idSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));

                where = DetachedCriteria.For<Criterio>();

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data) ) ||
                    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data !="0")
                   )
                {
                   
                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        where.Add(Expression.Eq("TipoCriterio", grid.rules[0].data));
                    }
                    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                    {
                        where.Add(Expression.Eq("TipoMedicion", grid.rules[1].data));
                    }
                    if (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data !="0")
                    {
                        where.Add(Expression.Like("Pregunta", '%'+grid.rules[2].data+'%'));
                    }
                    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data !="0")
                    {
                        where.Add(Expression.Eq("IndicadorActivo", grid.rules[3].data));
                    }


                }

                //obtiene por la Sede
                if (idSede>0) 
                {
                    where.Add(Expression.Eq("IdeSede",idSede));
                }
                
                
                var generic = Listar(_criterioRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                    {
                        id = item.IdeCriterio.ToString(),
                        cell = new string[]
                            {
                                "1",
                                item.IndicadorActivo,
                                item.IndicadorActivo,
                                (item.Pregunta==null?"":item.Pregunta),
                                item.TipoMedicionDes,
                                item.TipoMedicion,
                                item.TipoCriterio,
                                item.TipoCriterioDes,
                                //item.TipoCalificacion,
                                //item.TipoCalificacionDes,
                                item.TipoModo,
                                item.TipoModoDes,
                                item.FechaCreacion == DateTime.MinValue? "": item.FechaCreacion.ToString("dd/MM/yyyy"),
                                item.UsuarioCreacion,
                                item.FechaModificacion == DateTime.MinValue? "": item.FechaModificacion.ToString("dd/MM/yyyy"),
                                item.UsuarioModificacion
                   
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

        /// <summary>
        /// PopupCriterio: obtiene los datos del Popup y los guarda en la DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="imagenAlternativa"></param>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult PopupCriterio(CriterioViewModel model)
        {
            var criterioViewModel = new CriterioViewModel();
            criterioViewModel.Alternativa = new Alternativa();
            criterioViewModel.Alternativa.Criterio = new Criterio();

            DateTime Hoy = DateTime.Today;

            AlternativaValidator validator = new AlternativaValidator();
            ValidationResult result = validator.Validate(model.Alternativa, "NombreAlternativa", "Peso");

            if (!result.IsValid)
            {
                return View(criterioViewModel);
            }

            if (model.imagen2!=null)
            {
                string filePath = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(model.imagen2));
                Stream s = System.IO.File.OpenRead(filePath);
                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, (int)s.Length);
                int len = (int)s.Length;
                s.Dispose();
                s.Close();

                model.Alternativa.Image = buffer;    
            }
            
            if (model.Alternativa.IdeAlternativa != 0 && model.Alternativa.IdeAlternativa != null)
            {

                var alter = _alternativaRepository.GetSingle(x => x.IdeAlternativa == model.Alternativa.IdeAlternativa);
                alter.NombreAlternativa = model.Alternativa.NombreAlternativa.Trim();
                alter.Peso = model.Alternativa.Peso;
                model.Alternativa.FechaModificacion = Hoy;
                model.Alternativa.UsuarioModificacion = UsuarioActual.NombreUsuario;
                if (model.Alternativa.Image!=null)
                {
                    alter.Image = model.Alternativa.Image;     
                }
               
                _alternativaRepository.Update(alter);

            }
            else
            {
                model.Alternativa.FechaCreacion = Hoy;
                model.Alternativa.UsuarioCreacion = UsuarioActual.NombreUsuario;
                string nombreAlter = model.Alternativa.NombreAlternativa.Trim();
                model.Alternativa.NombreAlternativa = nombreAlter;


                _alternativaRepository.Add(model.Alternativa);
            }

            criterioViewModel.Alternativa.Criterio.IdeCriterio = model.Alternativa.Criterio.IdeCriterio;
            criterioViewModel.Alternativa.IdeAlternativa = model.Alternativa.IdeAlternativa;
            criterioViewModel.Alternativa.NombreAlternativa = model.Alternativa.NombreAlternativa;
            criterioViewModel.Alternativa.Peso = model.Alternativa.Peso;

            return View(criterioViewModel);
        }

        [ValidarSesion]
        public ActionResult Edit(string ideCriterio)
        {

            var criterioViewModel = InicializarCriteriosEdit();
            return View(criterioViewModel);
        }

        [ValidarSesion]
        public ActionResult BuscarCriterios()
        {
            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosIndex();
            
            return View("Index", model);
        }

        [ValidarSesion]
        public ActionResult Nuevo()
        {
            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosEdit();
            model.Criterio.IndPagina = Accion.Nuevo;
            model.IndVisual = Visualicion.NO;

            Session[ConstanteSesion.Criterio] = null;

            model.Criterio.EstadoDes = "Activo";


            Session["TipoModo"] = 1;
            
            return View("Edit", model);
        }

        //[ValidarSesion(TipoDevolucionError = TipoDevolucionError.Json, NombresValidar = new string[] { ConstanteSesion.Usuario })]
        [ValidarSesion]
        public ActionResult Edicion(string id)
        {
            Session[ConstanteSesion.Criterio] = null;
            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosEdit();
            
            var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == Convert.ToInt32(id));

            if (objCriterio!=null)
            {

                if (IndicadorActivo.Activo.Equals(objCriterio.IndicadorActivo))
                {
                    model.Criterio.EstadoDes = "Activo";
                }
                else
                {
                    model.Criterio.EstadoDes = "Inactivo";
                }
                
            }
            
            
            model.Criterio.IdeCriterio = objCriterio.IdeCriterio;
           // model.Criterio.TipoCalificacion = objCriterio.TipoCalificacion;
            model.Criterio.TipoCriterio = objCriterio.TipoCriterio;
            model.Criterio.TipoMedicion = objCriterio.TipoMedicion;
            model.Criterio.TipoModo = objCriterio.TipoModo;
            model.Criterio.IdeCriterio = objCriterio.IdeCriterio;
            model.Criterio.Pregunta = objCriterio.Pregunta;
            model.Criterio.IndPagina = Accion.Actualizar.ToString();
            model.Criterio.IMAGENCRIT = objCriterio.IMAGENCRIT;
            model.Criterio.rutaImagen = objCriterio.rutaImagen;
            model.IndVisual = Visualicion.SI;

            var objAlternativa = _alternativaRepository.GetSingle(x => x.IdeAlternativa == Convert.ToInt32(id));
            model.Alternativa = objAlternativa;

            if ("01".Equals(model.Criterio.TipoModo))
            {
                Session["TipoModo"] = "T";
            }
            else
            {
                Session["TipoModo"] = "I";
               
            }

            Session[ConstanteSesion.Criterio] = model.Criterio;

            return View("Edit", model);
        }

        [ValidarSesion]
        public ActionResult ActivarDesactivar(string id,string estado)
        {
            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();
            model = InicializarCriteriosIndex();

            var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == Convert.ToInt32(id));
            
            if (IndicadorActivo.Activo.Equals(estado))
            {
                objCriterio.IndicadorActivo = IndicadorActivo.Inactivo;
            }
            else
            {
                objCriterio.IndicadorActivo = IndicadorActivo.Activo;
            }
            _criterioRepository.Update(objCriterio);

            return View("Index", model); ;
        }


        /// <summary>
        /// EliminarCriterio : Elimina el criterio
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult EliminarCriterio(string id)
        {
            CriterioViewModel model = new CriterioViewModel();
            JsonMessage objJsonMessage = new JsonMessage();
            model.Criterio = new Criterio();

            model = InicializarCriteriosIndex();


            var objCriterioxSubCategoria = _criterioPorSubcategoriaRepository.GetBy(x => x.Criterio.IdeCriterio == Convert.ToInt32(id));
            if (objCriterioxSubCategoria != null && objCriterioxSubCategoria.Count>0)
            {
                objJsonMessage.Mensaje = "El criterio esta asociado a una Subcategoria";
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            else
            {
                var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == Convert.ToInt32(id));
                _criterioRepository.Remove(objCriterio);
                objJsonMessage.Mensaje = "Se eliminó el criterio correctamente";
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
                
            }


        }

        [ValidarSesion]
        public ActionResult ConsultaCriterios(string id)
        {

            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosEdit();

            var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == Convert.ToInt32(id));


            if (objCriterio != null)
            {

                if (IndicadorActivo.Activo.Equals(objCriterio.IndicadorActivo))
                {
                    model.Criterio.EstadoDes = "Activo";
                }
                else
                {
                    model.Criterio.EstadoDes = "Inactivo";
                }

            }
            
            model.Criterio.IdeCriterio = objCriterio.IdeCriterio;
            //model.Criterio.TipoCalificacion = objCriterio.TipoCalificacion;
            model.Criterio.TipoCriterio = objCriterio.TipoCriterio;
            model.Criterio.TipoMedicion = objCriterio.TipoMedicion;
            model.Criterio.TipoModo = objCriterio.TipoModo;
            model.Criterio.IdeCriterio = objCriterio.IdeCriterio;
            model.Criterio.Pregunta = objCriterio.Pregunta;
            model.Criterio.IndPagina = Accion.Consultar;
            model.Criterio.rutaImagen = objCriterio.rutaImagen;
            model.Criterio.IMAGENCRIT = objCriterio.IMAGENCRIT;
            model.IndVisual = Visualicion.SI;
            var objAlternativa = _alternativaRepository.GetSingle(x => x.IdeAlternativa == Convert.ToInt32(id));
            model.Alternativa = objAlternativa;

            if ("01".Equals(model.Criterio.TipoModo))
            {
                Session["TipoModo"] = "T";
            }
            else
            {
                Session["TipoModo"] = "I";

            }

            return View("Edit", model);

        }
        /// <summary>
        /// Inicializa el los datos del Popup Criterio para mostrar las alternativas
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codCriterio"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ViewResult PopupCriterio(int id, int codCriterio)
        {

            int IdCriterio = codCriterio;
            CriterioViewModel modelo = new CriterioViewModel();
            modelo.Alternativa = new Alternativa();
            modelo.Alternativa.Criterio = new Criterio();
            if (id == 0)
            {
                
                modelo.Alternativa.Criterio.IdeCriterio = IdCriterio;
                return View(modelo);
            }
            else 
            {
                
                modelo.Alternativa.Criterio.IdeCriterio = IdCriterio;
                modelo.Alternativa.IdeAlternativa = id;
                modelo.Alternativa = _alternativaRepository.GetSingle(x => x.IdeAlternativa == id);

                return View(modelo);

            }
            
        }

        
        [HttpPost]
        public ActionResult ListaAlternativaxCriterio(GridTable grid, int idCriterio)
        {
            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;
                
               

                DetachedCriteria where = DetachedCriteria.For<Alternativa>();


                where.Add(Expression.Eq("Criterio.IdeCriterio", idCriterio));


                var generic = Listar(_alternativaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeAlternativa.ToString(),
                        cell = new string[]
                            {
                                
                                item.IdeAlternativa.ToString(),
                                item.NombreAlternativa==null?"":item.NombreAlternativa.ToString(),
                                item.Peso.ToString(),
                                item.IdeAlternativa.ToString(),
                                item.Criterio.TipoModo
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


        

        /// <summary>
        /// Guarda los datos del criterio
        /// </summary>
        /// <param name="model"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit(CriterioViewModel model)
        {

            try
            {


                    Criterio ObjCriterioConsulta = new Criterio();
                    var crit = Session[ConstanteSesion.Criterio];
                    if (crit!=null)
                    {
                        ObjCriterioConsulta = (Criterio)Session[ConstanteSesion.Criterio];
                        model.Criterio.TipoModo = ObjCriterioConsulta.TipoModo;
                    }
                
                
                    var criterioViewModel = InicializarCriteriosEdit();
                    JsonMessage objJsonMessage = new JsonMessage();

                    string fullPath = null;
                   

                    if ("02".Equals(model.Criterio.TipoModo))
                    {
                        if (model.Criterio.rutaImagen == null)
                        {
                            objJsonMessage.Resultado = false;
                            objJsonMessage.Mensaje = "Ingrese una imagen";
                            return Json(objJsonMessage);
                        }
                    }
          
           
                    model.Criterio.IndicadorActivo = IndicadorActivo.Activo;


                    if (!string.IsNullOrEmpty(model.NombreTemporalArchivo))
                    {
                
                        string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                        string directoryPath = "Archivos\\Imagenes\\";
                        fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, model.NombreTemporalArchivo));

                        using (Stream s = System.IO.File.OpenRead(fullPath))
                        {
                            byte[] buffer = new byte[s.Length];
                            s.Read(buffer, 0, (int)s.Length);
                            int len = (int)s.Length;
                            s.Close();
                            model.Criterio.IMAGENCRIT = buffer;
                            model.Criterio.rutaImagen = model.Criterio.rutaImagen;
                        }
                    }


                    int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));


                    if (Accion.Nuevo.Equals(model.Criterio.IndPagina))
                    {
                        model.Criterio.FechaCreacion = DateTime.Now;
                        model.Criterio.UsuarioCreacion = UsuarioActual.NombreUsuario;
                        //se agrega la sede
                        model.Criterio.IdeSede = IdSede;
                        model.Criterio.OrdenImpresion = 0;
                        if (model.Criterio.Pregunta!=null)
                        {
                            string pregunta = model.Criterio.Pregunta.Trim();
                            model.Criterio.Pregunta = pregunta;
                        }
                        


                        _criterioRepository.Add(model.Criterio);
                        objJsonMessage.Accion = "N";
                        objJsonMessage.Resultado = true;
                        objJsonMessage.Mensaje = "Se registro el criterio correctamente";
                        objJsonMessage.IdDato = model.Criterio.IdeCriterio;
                
                    }
                    else
                    {
                
                        var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == model.Criterio.IdeCriterio);
                        objCriterio.TipoCriterio = model.Criterio.TipoCriterio;
                        objCriterio.TipoMedicion = model.Criterio.TipoMedicion;
                        //objCriterio.TipoModo = model.Criterio.TipoModo;
                       // objCriterio.TipoCalificacion = model.Criterio.TipoCalificacion;
                        objCriterio.Pregunta = model.Criterio.Pregunta;
                        objCriterio.FechaModificacion = DateTime.Now;
                        objCriterio.UsuarioModificacion = UsuarioActual.NombreUsuario;

                        if ("02".Equals(objCriterio.TipoModo))
                        {
                            if ( model.Criterio.IMAGENCRIT!=null)
                            {
                                objCriterio.IMAGENCRIT = model.Criterio.IMAGENCRIT;
                                objCriterio.rutaImagen = model.Criterio.rutaImagen;     
                            }
                   
                        }
                
                        _criterioRepository.Update(objCriterio);
                        objJsonMessage.Accion = "S";
                        objJsonMessage.Resultado = true;
                        objJsonMessage.Mensaje = "Se actualizó el criterio correctamente";
                        objJsonMessage.IdDato = objCriterio.IdeCriterio;
                    }

                    criterioViewModel.IndVisual = Visualicion.SI;
                    criterioViewModel.Criterio.TipoMedicion = model.Criterio.TipoMedicion;
                    criterioViewModel.Criterio.TipoCriterio = model.Criterio.TipoCriterio;
                    //criterioViewModel.Criterio.TipoModo = model.Criterio.TipoModo;
                    if("01".Equals(model.Criterio.TipoModo))
                    {
                        Session["TipoModo"] = "T";
                    }
                    else
	                {
                        Session["TipoModo"] = "I";
                        criterioViewModel.image = model.image;
                        criterioViewModel.Criterio.rutaImagen = model.Criterio.rutaImagen;
	                }
                    criterioViewModel.Criterio.Pregunta = model.Criterio.Pregunta;
                   // criterioViewModel.Criterio.TipoCalificacion = model.Criterio.TipoCalificacion;
                    criterioViewModel.Criterio.IdeCriterio = model.Criterio.IdeCriterio;
                    if (fullPath != null)
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    //return RedirectToAction("Edicion", "Criterio", new { id = model.Criterio.IdeCriterio });
                    return Json(objJsonMessage);
                    }
            
            catch (Exception)
            {

                return MensajeError();
            }

        }

        [ValidarSesion]
        [HttpPost]
        public ActionResult Index(CriterioViewModel model)
        {
            var criterioViewModel = InicializarCriteriosIndex();

            criterioViewModel.Criterio = model.Criterio;
            
            criterioViewModel.Criterio.TipoMedicion = model.Criterio.TipoMedicion;
            criterioViewModel.Criterio.TipoCriterio = model.Criterio.TipoCriterio;
            criterioViewModel.Criterio.TipoModo = model.Criterio.TipoModo;
            criterioViewModel.Criterio.Pregunta = model.Criterio.Pregunta;
          


            return View(criterioViewModel);

        }

        [HttpPost]
        public ActionResult Eliminar(string ideAlternativa, string codigoCriterio)
        {

            var alter = _alternativaRepository.GetSingle(x => x.IdeAlternativa == Convert.ToInt32(ideAlternativa));
            
            _alternativaRepository.Remove(alter);
            return null;            
        }

        public CriterioViewModel InicializarCriteriosIndex()
        {
            var criterioViewModel = new CriterioViewModel();
            criterioViewModel.Criterio = new Criterio();

            criterioViewModel.TipoCriterio =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCriterio));
            criterioViewModel.TipoCriterio.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            criterioViewModel.Medicion =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.Medicion));
            criterioViewModel.Medicion.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            criterioViewModel.Estado =
               new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            criterioViewModel.Estado.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            return criterioViewModel;
        }

        private CriterioViewModel InicializarCriteriosEdit()
        {
            var criterioViewModel = new CriterioViewModel();
            criterioViewModel.Criterio = new Criterio();

            criterioViewModel.TipoCriterio =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCriterio));
            criterioViewModel.TipoCriterio.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            criterioViewModel.Medicion =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.Medicion));
            criterioViewModel.Medicion.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            criterioViewModel.TipoModo =
               new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.Modo));
            criterioViewModel.TipoModo.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            //criterioViewModel.TipoCalificacion =
            //    new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCalificacion));
            //criterioViewModel.TipoCalificacion.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            return criterioViewModel;
        }

       

        // lista de criterios del popup
        [HttpPost]
        public ActionResult ListaCriterioxSubCategoria(GridTable grid)
        {
            try
            {
                
                Criterio objCriterio = new Criterio();
                int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));
                List<Criterio> listaCriterios = new List<Criterio>();

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)) ||
                    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                   )
                {

                if (grid.rules[0].data==null || grid.rules[0].data=="0")
                {
                    objCriterio.TipoCriterio = "";
                }
                else
                {
                    objCriterio.TipoCriterio = grid.rules[0].data;
                }
                //objCriterio.TipoCriterio = (grid.rules[0].data == null ? "" : grid.rules[0].data);

                if (grid.rules[1].data == null || grid.rules[1].data == "0")
                {
                    objCriterio.TipoMedicion = "";
                }
                else
                {
                    objCriterio.TipoMedicion = grid.rules[1].data;
                }

                
                if (grid.rules[2].data == null)
                {
                    objCriterio.Pregunta = "";
                }
                else
                {
                    objCriterio.Pregunta = grid.rules[2].data;
                }


                if (grid.rules[3].data == null || grid.rules[3].data == "0")
                {
                    objCriterio.IndicadorActivo = "";
                }
                else
                {
                    objCriterio.IndicadorActivo = grid.rules[3].data;
                }

                }

                objCriterio.IdeSede = IdSede;


                listaCriterios = _criterioRepository.ObtenerCriterios(objCriterio);

                var generic = GetListar(listaCriterios,
                                        grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

              
                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeCriterio.ToString(),
                    cell = new string[]
                            {
                               
                               "1",
                                item.IndicadorActivo,
                                item.IndicadorActivo,
                                item.Pregunta==null?"":item.Pregunta,
                                item.TipoMedicionDes,
                                item.TipoMedicion,
                                item.TipoCriterio,
                                item.TipoCriterioDes,
                                //item.TipoCalificacion,
                                //item.TipoCalificacionDes,
                                item.TipoModo,
                                item.TipoModoDes
                               
                            }
                }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
               
                return MensajeError();
            }
        }

        [ValidarSesion]
        [HttpPost]
        public ActionResult PopupListaCriterio(CriterioViewModel model)
        {
            CriterioViewModel objCriterioModel = new CriterioViewModel();
            objCriterioModel = InicializarCriteriosIndex();

            objCriterioModel.Criterio.TipoCriterio = model.Criterio.TipoCriterio;
            objCriterioModel.Criterio.TipoModo = model.Criterio.TipoModo;
            objCriterioModel.Criterio.TipoMedicion = model.Criterio.TipoMedicion;
            objCriterioModel.Criterio.Pregunta = model.Criterio.Pregunta;

            return View(objCriterioModel);
        }

        /// <summary>
        /// Inicializa el popup de la lista de criterios
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idSubCategoria"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult PopupListaCriterio(int id, string idSubCategoria)
        {
            try
            {
                CriterioViewModel objCriterioModel = new CriterioViewModel();
                objCriterioModel = InicializarCriteriosIndex();
                objCriterioModel.CriterioPorSubcategoria = new CriterioPorSubcategoria();
                objCriterioModel.CriterioPorSubcategoria.IDESUBCATEGORIA = Convert.ToInt32(idSubCategoria);
                objCriterioModel.Criterio = new Criterio();

                var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == id);
                objCriterioModel.Criterio.TipoCriterio = objCategoria.TIPCATEGORIA;

                return View(objCriterioModel);
                
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                MensajeError();
                return null;
            }
           

        }

        [HttpPost]
        public ActionResult GetListaCriterio(List<int> test, string subCategoria)
        {
            int codSubCategoria = Convert.ToInt32(subCategoria);
            int codCriterio=0;
            DateTime Hoy = DateTime.Today;
            int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));
            int maxOrdenImp = 0;

            CriterioPorSubcategoria objCriterioxSubCategoria;

           

            if (test!=null && test.Count>0)
            {
                for (int i = 0; i < test.Count; i++)
                {

                    objCriterioxSubCategoria = new CriterioPorSubcategoria();
                    objCriterioxSubCategoria.SubCategoria = new SubCategoria();
                    objCriterioxSubCategoria.Criterio = new Criterio();

                    codCriterio = test[i];
                    
                    var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == codCriterio);


                   var criterioxSubCategoria = _criterioPorSubcategoriaRepository.GetSingle(s => s.SubCategoria.IDESUBCATEGORIA == codSubCategoria && 
                                                                 s.Criterio.IdeCriterio == codCriterio);


                   if (criterioxSubCategoria !=null && criterioxSubCategoria.Criterio.IdeCriterio > 0)
                   {
                       continue;
                   }
                   else
                   {

                       var listaCriterios = _criterioPorSubcategoriaRepository.GetBy(x => x.SubCategoria.IDESUBCATEGORIA == codSubCategoria);
                       if (listaCriterios!=null && listaCriterios.Count>0)
                       {
                           maxOrdenImp = (listaCriterios.Select(d => d.PRIORIDAD).Max()) == null ? 0 : (listaCriterios.Select(d => d.PRIORIDAD).Max());
                       }
                       
                       maxOrdenImp = maxOrdenImp + 1;

                       objCriterioxSubCategoria.PRIORIDAD = maxOrdenImp;


                       objCriterioxSubCategoria.SubCategoria.IDESUBCATEGORIA = codSubCategoria;

                       objCriterioxSubCategoria.Criterio.IdeCriterio = codCriterio;
                       objCriterioxSubCategoria.PUNTAMAXIMO = 0;
                       objCriterioxSubCategoria.ESTREGISTRO = IndicadorActivo.Activo;
                       objCriterioxSubCategoria.USRCREACION = UsuarioActual.NombreUsuario;
                       objCriterioxSubCategoria.USRMODIFICA = UsuarioActual.NombreUsuario;
                       objCriterioxSubCategoria.FECCREACION = Hoy;
                       objCriterioxSubCategoria.FECMODIFICA = Hoy;
                       objCriterioxSubCategoria.IdeSede = IdSede;

                       _criterioPorSubcategoriaRepository.Add(objCriterioxSubCategoria);


                   }

                }
            }

            return null;
        }


        /// <summary>
        /// GetImage Muestra la Imagen en el criterio
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetImage(int id)
        {
            var firstOrDefault = _criterioRepository.GetSingle(c => c.IdeCriterio == id);
            if (firstOrDefault!=null)
            {
                
            
            if (firstOrDefault.IMAGENCRIT != null)
            {
                byte[] image = firstOrDefault.IMAGENCRIT;
                return File(image, "image/jpg");
            }
            else
            {
                return null;
            }
            }
            else
            {
                return null;
            
            }
        }




        /// <summary>
        /// Subida de imagen a la carpeta temporal
        /// </summary>
        /// <param name="file"></param>
        /// <param name="forms"></param>
        /// <returns></returns>
        [HttpPost]
        public string Upload(HttpPostedFileBase file, FormCollection forms)
        {
            var jsonResponse = new JsonResponse { Success = false };

            try
            {
                string[] extensiones = forms.Get("ext").Split(';');

                string extensionArchivo = Path.GetExtension(file.FileName);

                if (extensiones.Contains(extensionArchivo.ToLower()))
                {
                    var content = new byte[file.ContentLength];
                    file.InputStream.Read(content, 0, file.ContentLength);

                    var indexOfLastDot = file.FileName.LastIndexOf('.');
                    var extension = file.FileName.Substring(indexOfLastDot + 1, file.FileName.Length - indexOfLastDot - 1);
                    var name = file.FileName.Substring(0, indexOfLastDot);

                    string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                    string directoryPath = ConfigurationManager.AppSettings["ImageFilePath"];
                    string nombreTemporalArchivo = Guid.NewGuid().ToString();
                    string fullPath = Path.Combine(applicationPath, string.Format("{0}{1}{2}", directoryPath, nombreTemporalArchivo, extensionArchivo));

                    System.IO.File.WriteAllBytes(fullPath, content);



                    jsonResponse.Data = new
                    {
                        NombreArchivo = file.FileName,
                        NombreTemporalArchivo = string.Format("{0}{1}", nombreTemporalArchivo, extensionArchivo)
                    };
                    jsonResponse.Success = true;

                }
                else
                {
                    jsonResponse.Success = false;
                    jsonResponse.Message = "0";

                }
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                jsonResponse.Message = "Ocurrio un error, por favor intente de nuevo o más tarde.";
            }

            return JsonConvert.SerializeObject(jsonResponse);
        }

        /// <summary>
        /// obtiene sub Imagen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ObtenerSubImagen(int id)
        {
            var firstOrDefault = _alternativaRepository.GetSingle(c => c.IdeAlternativa == id);
            
                if (firstOrDefault!=null && firstOrDefault.Image != null)
                {
                    byte[] image = firstOrDefault.Image;
                    return File(image, "image/jpg");
                }
                else
                {
                    return null;
                }     
            
        }

    }
}
