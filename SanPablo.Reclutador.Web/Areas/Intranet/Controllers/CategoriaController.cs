

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class CategoriaController : BaseController
    {
        private ICategoriaRepository _categoriaRepository;
        private IAlternativaRepository _alternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISubcategoriaRepository _subcategoriaRepository;
        private ICriterioRepository _criterioRepository;
        private ICriterioPorSubcategoriaRepository _criterioPorSubcategoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository, 
            IDetalleGeneralRepository detalleGeneralRepository, 
            IAlternativaRepository alternativaRepository,
            ISubcategoriaRepository subcategoriaRepository,
            ICriterioRepository criterioRepository,
            ICriterioPorSubcategoriaRepository criterioPorSubcategoriaRepository
            )
        {
            _categoriaRepository = categoriaRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _alternativaRepository = alternativaRepository;
            _subcategoriaRepository = subcategoriaRepository;
            _criterioRepository = criterioRepository;
            _criterioPorSubcategoriaRepository = criterioPorSubcategoriaRepository;
        }
        
        
        public ActionResult Index()
        {

            CategoriaViewModel objCategoriaView = new CategoriaViewModel();
            objCategoriaView = InicializarCategoriaIndex();

            return View(objCategoriaView);
        }

        public ActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Edit(CategoriaViewModel model, HttpPostedFileBase image)
        {
            ValidationResult result = null;
            CategoriaValidator validator = new CategoriaValidator();
            CategoriaViewModel categoriaViewModel = new CategoriaViewModel();
            categoriaViewModel.Categoria = new Categoria();
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("DD/MM/YYYY");

            if ("02".Equals(model.Categoria.TIPOEJEMPLO))
            {
                result = validator.Validate(model.Categoria, "DESCCATEGORIA", "NOMCATEGORIA", "TIPCATEGORIA", "INSTRUCCIONES");
                Session["Tipo"] = "I";
            }
            if ("01".Equals(model.Categoria.TIPOEJEMPLO))
            {
                result = validator.Validate(model.Categoria, "DESCCATEGORIA", "NOMCATEGORIA", "TIPCATEGORIA", "TEXTOEJEMPLO", "INSTRUCCIONES");
                Session["Tipo"] = "T";
            }else{
                result = validator.Validate(model.Categoria, "DESCCATEGORIA", "NOMCATEGORIA", "TIPCATEGORIA", "INSTRUCCIONES");
            }
            categoriaViewModel = InicializarCategoriaEdit();
            if (!result.IsValid)
            {
                return View(categoriaViewModel);
            }

            if (model.image != null)
            {
                //string filePath = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(model.image.FileName));
                //model.image.SaveAs(filePath);
                byte[] data;
                using (Stream inputStream = model.image.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }
                model.Categoria.IMAGENEJEMPLO = data;

            }


            if (Accion.Nuevo.Equals(Session["AccionCategoria"]))
            {

                if (model.Categoria.IDECATEGORIA != null && model.Categoria.IDECATEGORIA > 0)
                {
                    var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == model.Categoria.IDECATEGORIA);
                    objCategoria.USRCREACION = "Prueba 02";
                    objCategoria.FECCREACION = Hoy;
                    objCategoria.USRMODIFICA = "Prueba 01";
                    objCategoria.FECMODIFICA = Hoy;
                    objCategoria.TIPCATEGORIA = model.Categoria.TIPCATEGORIA;
                    objCategoria.DESCCATEGORIA = model.Categoria.DESCCATEGORIA;
                    objCategoria.NOMCATEGORIA = model.Categoria.NOMCATEGORIA;
                    objCategoria.INSTRUCCIONES = model.Categoria.INSTRUCCIONES;
                    objCategoria.TIPOEJEMPLO = model.Categoria.TIPOEJEMPLO;
                    if ("01".Equals(model.Categoria.TIPOEJEMPLO))
                    {
                        objCategoria.TEXTOEJEMPLO = model.Categoria.TEXTOEJEMPLO;
                    }
                    else
                    {
                        objCategoria.IMAGENEJEMPLO = model.Categoria.IMAGENEJEMPLO;
                    }
                    _categoriaRepository.Update(objCategoria);
                    
                }
                else
                {
                    model.Categoria.USRCREACION = "Prueba 01";
                    model.Categoria.FECCREACION = Hoy;
                    model.Categoria.USRMODIFICA = "Prueba 01";
                    model.Categoria.FECMODIFICA = Hoy;
                    //model.Categoria.ORDENIMPRESION = "1";
                    model.Categoria.ESTACTIVO = "A";


                    var listaCategoria = _categoriaRepository.All();
                    int maxOrdenImp = 0;
                    if (listaCategoria!=null && listaCategoria.Count>0)
                    {
                        maxOrdenImp = (listaCategoria.Select(d => d.ORDENIMPRESION).Max()) == null ? 0 : (listaCategoria.Select(d => d.ORDENIMPRESION).Max());    
                    }
                    
                    maxOrdenImp = maxOrdenImp + 1;

                    model.Categoria.ORDENIMPRESION = maxOrdenImp;

                    _categoriaRepository.Add(model.Categoria);
                    
                    Session["Tabla1"] = Grilla.Tabla1;
                    Session["Tabla2"] = Grilla.Tabla2;
                }
            }
            
            if (Accion.Editar.Equals(Session["AccionCategoria"]))
            {
                if (model.Categoria.IDECATEGORIA != null && model.Categoria.IDECATEGORIA > 0)
                {
                    var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == model.Categoria.IDECATEGORIA);
                    objCategoria.USRCREACION = "Prueba 02";
                    objCategoria.FECCREACION = Hoy;
                    objCategoria.USRMODIFICA = "Prueba 01";
                    objCategoria.FECMODIFICA = Hoy;
                    objCategoria.TIPCATEGORIA = model.Categoria.TIPCATEGORIA;
                    objCategoria.DESCCATEGORIA = model.Categoria.DESCCATEGORIA;
                    objCategoria.NOMCATEGORIA = model.Categoria.NOMCATEGORIA;
                    objCategoria.INSTRUCCIONES = model.Categoria.INSTRUCCIONES;
                    objCategoria.TIPOEJEMPLO = model.Categoria.TIPOEJEMPLO;
                    objCategoria.ORDENIMPRESION = model.Categoria.ORDENIMPRESION;
                    if ("01".Equals(model.Categoria.TIPOEJEMPLO))
                    {
                        objCategoria.TEXTOEJEMPLO = model.Categoria.TEXTOEJEMPLO;
                    }
                    else
                    {
                        objCategoria.IMAGENEJEMPLO = model.Categoria.IMAGENEJEMPLO;
                    }
                    _categoriaRepository.Update(objCategoria);
                    
                }
            }
            
            
            categoriaViewModel.Categoria.IDECATEGORIA = model.Categoria.IDECATEGORIA;
            categoriaViewModel.Categoria.NOMCATEGORIA = model.Categoria.NOMCATEGORIA;
            categoriaViewModel.Categoria.DESCCATEGORIA = model.Categoria.DESCCATEGORIA;
            categoriaViewModel.Categoria.TIPCATEGORIA = model.Categoria.TIPCATEGORIA;
            categoriaViewModel.Categoria.TIPOEJEMPLO = model.Categoria.TIPOEJEMPLO;
            categoriaViewModel.Categoria.TEXTOEJEMPLO = model.Categoria.TEXTOEJEMPLO;
            categoriaViewModel.Categoria.INSTRUCCIONES = model.Categoria.INSTRUCCIONES;
            categoriaViewModel.image = model.image;

            return View("Edit", categoriaViewModel);

        }


        [HttpPost]
        public ActionResult PopupSubCategoria(CategoriaViewModel model)
        {
            
            SubCategoriaValidator objSubCategoriaValidate = new SubCategoriaValidator();
            ValidationResult result = objSubCategoriaValidate.Validate(model.SubCategoria, "NOMSUBCATEGORIA", "DESCSUBCATEGORIA");

            if (!result.IsValid)
            {
                return View(model);
            }

            CategoriaViewModel objCategoriaModel = new CategoriaViewModel();
            objCategoriaModel.SubCategoria = new SubCategoria();
            objCategoriaModel.SubCategoria.Categoria = new Categoria();
            DateTime Hoy = DateTime.Today;


            if (model.SubCategoria.IDESUBCATEGORIA != null && model.SubCategoria.IDESUBCATEGORIA > 0)
            {
                objCategoriaModel.SubCategoria = _subcategoriaRepository.GetSingle(x => x.IDESUBCATEGORIA == model.SubCategoria.IDESUBCATEGORIA);
                objCategoriaModel.SubCategoria.FECMODIFICACION = Hoy;
                objCategoriaModel.SubCategoria.USRMODIFICACION = "Prueba 02";
                objCategoriaModel.SubCategoria.NOMSUBCATEGORIA = model.SubCategoria.NOMSUBCATEGORIA;
                objCategoriaModel.SubCategoria.DESCSUBCATEGORIA = model.SubCategoria.DESCSUBCATEGORIA;
                objCategoriaModel.SubCategoria.Categoria = model.Categoria;
                objCategoriaModel.SubCategoria.ORDENIMPRESION = model.SubCategoria.ORDENIMPRESION;
                _subcategoriaRepository.Update(objCategoriaModel.SubCategoria);
            }
            else
            {
              
                objCategoriaModel.SubCategoria.FECMODIFICACION = Hoy;
                objCategoriaModel.SubCategoria.FECCREACION = Hoy;
                objCategoriaModel.SubCategoria.USRCREACION = "Prueba01";
                objCategoriaModel.SubCategoria.USRMODIFICACION = "Prueba02";
                objCategoriaModel.SubCategoria.ORDENIMPRESION = 1;
                objCategoriaModel.SubCategoria.ESTACTIVO = "A";
                objCategoriaModel.SubCategoria.NOMSUBCATEGORIA = model.SubCategoria.NOMSUBCATEGORIA;
                objCategoriaModel.SubCategoria.DESCSUBCATEGORIA = model.SubCategoria.DESCSUBCATEGORIA;
                objCategoriaModel.SubCategoria.Categoria = model.Categoria;

                var listaCategoria = _subcategoriaRepository.GetBy(s => s.Categoria.IDECATEGORIA == Convert.ToInt32(model.Categoria.IDECATEGORIA));
                int maxOrdenImp = 0;
                if (listaCategoria != null && listaCategoria.Count>0)
                {
                    maxOrdenImp = (listaCategoria.Select(d => d.ORDENIMPRESION).Max()) == null ? 0 : (listaCategoria.Select(d => d.ORDENIMPRESION).Max());
                }
                maxOrdenImp = maxOrdenImp + 1;

                objCategoriaModel.SubCategoria.ORDENIMPRESION = maxOrdenImp;

                _subcategoriaRepository.Add(objCategoriaModel.SubCategoria);
            }

            return View(objCategoriaModel);
           
        }

        public ViewResult PopupSubCategoria(int id,string idSubCategoria)
        {
            CategoriaViewModel model = new CategoriaViewModel();
            model.Categoria = new Categoria();
            model.SubCategoria = new SubCategoria();
            model.Categoria.IDECATEGORIA = id;
            int dato = Convert.ToInt32(idSubCategoria);
            if (dato == 0)
            {
                 return View(model);
            }
            else
            {
                model.SubCategoria = _subcategoriaRepository.GetSingle(x => x.IDESUBCATEGORIA == Convert.ToInt32(idSubCategoria));
                return View(model);
            }

        }


        [HttpPost]
        public ActionResult ListaSubCategoria(GridTable grid, int idCategoria)
        {

            try
            {
                // int idCriterio = Convert.ToInt32(grid.rules[0].data);
                //DetachedCriteria where = null;

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                //obtiene el valor del criterio
                
                
                // int idCriterio = Convert.ToInt32(grid.rules[0].data);

                DetachedCriteria where = DetachedCriteria.For<SubCategoria>();
                where.Add(Expression.Eq("Categoria.IDECATEGORIA", idCategoria));
                
                var generic = Listar(_subcategoriaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDESUBCATEGORIA.ToString(),
                    cell = new string[]
                            {
                               
                                item.IDESUBCATEGORIA.ToString(),
                                item.ESTACTIVO,
                                item.NOMSUBCATEGORIA,
                                item.DESCSUBCATEGORIA,
                                item.ORDENIMPRESION.ToString(),
                                item.FECCREACION.ToString(),
                                item.USRCREACION,
                                item.FECMODIFICACION.ToString(),
                                item.USRMODIFICACION
                                
                            }
                }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {

                return MensajeError();
            }

        }
        
        [HttpPost]
        public ActionResult ListaCriterioxSub(GridTable grid)
        {
            try
            {
                //IdeSubCategoria
                DetachedCriteria where = null;
                where = DetachedCriteria.For<CriterioPorSubcategoria>();

                if ((grid.rules[0].data!=null))
                {
                   
                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        where.Add(Expression.Eq("SubCategoria.IDESUBCATEGORIA", Convert.ToInt32(grid.rules[0].data)));
                    }

                }
                else
                {
                    where.Add(Expression.Eq("SubCategoria.IDESUBCATEGORIA", 0));
                }

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;


                var generic = Listar(_criterioPorSubcategoriaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDECRITERIOXSUBCATEGORIA.ToString(),
                    cell = new string[]
                            {
                                item.IDECRITERIOXSUBCATEGORIA.ToString(),
                                item.SubCategoria.IDESUBCATEGORIA.ToString(),
                                item.Criterio.IdeCriterio.ToString(),
                                item.Criterio.Pregunta,
                                item.PUNTAMAXIMO.ToString(),
                                item.Criterio.OrdenImpresion.ToString(),
                                item.Criterio.TipoCalificacion.ToString()
                                //item.FECCREACION.ToString(),
                                //item.USRCREACION,
                                //item.FECMODIFICACION.ToString(),
                                //item.USRMODIFICACION
                                
                            }
                }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {

                return MensajeError();
            }

        }

        public ActionResult Nuevo()
        {
            CategoriaViewModel model = new CategoriaViewModel();
            model.Categoria = new Categoria();

            model = InicializarCategoriaEdit();
            //model.Categoria.i = Accion.Nuevo;
            model.IndVisual = Visualicion.NO;
            Session["AccionCategoria"] = Accion.Nuevo;
            Session["Tabla1"] = null;
            Session["Tabla2"] = null;
            Session["Tipo"] = 1;
            /*Session[Grilla.Tabla1] = Grilla.Tabla1;
            Session[Grilla.Tabla1] = Grilla.Tabla1;
            */

            return View("Edit", model);
        }


        private CategoriaViewModel InicializarCategoriaEdit()
        {
            var categoriaViewModel = new CategoriaViewModel();
            categoriaViewModel.Categoria = new Categoria();

            
            categoriaViewModel.TipoCategoria =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCategoria));
            categoriaViewModel.TipoCategoria.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            categoriaViewModel.TipoEjemplo =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.Modo));
            categoriaViewModel.TipoEjemplo.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return categoriaViewModel;
        }

        private CategoriaViewModel InicializarCategoriaIndex()
        {
            var categoriaViewModel = new CategoriaViewModel();
            categoriaViewModel.Categoria = new Categoria();


            categoriaViewModel.TipoCriterio =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCriterio));
            categoriaViewModel.TipoCriterio.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            categoriaViewModel.TipoEstado =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            categoriaViewModel.TipoEstado.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return categoriaViewModel;
        }


        [HttpPost]
        public ActionResult ListCriterioxAlternativa(GridTable grid)
        {
            try
            {
                
                
                DetachedCriteria where = null;
                where = DetachedCriteria.For<Alternativa>();

                if ((grid.rules[0].data != null))
                {

                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        where.Add(Expression.Eq("Criterio.IdeCriterio", Convert.ToInt32(grid.rules[0].data)));
                    }

                }
                else
                {
                    where.Add(Expression.Eq("Criterio.IdeCriterio", 0));
                }


                var generic = Listar(_alternativaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeAlternativa.ToString(),
                        cell = new string[]
                            {
                                
                                item.IdeAlternativa.ToString(),
                                item.NombreAlternativa.ToString(),
                                item.Peso.ToString(),
                                ""
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
        public ActionResult Index(CategoriaViewModel model)
        {

            CategoriaViewModel objCategoriaViewModel = new CategoriaViewModel();
            
            objCategoriaViewModel = InicializarCategoriaIndex();
            objCategoriaViewModel.Criterio = new Criterio();
            objCategoriaViewModel.Categoria = new Categoria();

            objCategoriaViewModel.Categoria.TipoCriterio = model.Categoria.TipoCriterio;
            objCategoriaViewModel.Categoria.ESTACTIVO = model.Categoria.ESTACTIVO;

            return View(objCategoriaViewModel);

        }


        
        [HttpPost]
        public ActionResult ListaPrincipalCategoria(GridTable grid)
        {
            try
            {
                // int idCriterio = Convert.ToInt32(grid.rules[0].data);
                DetachedCriteria where = null;

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)) ||
                    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                   )
                {
                    where = DetachedCriteria.For<Categoria>();

                    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                    {
                        where.Add(Expression.Eq("TIPCATEGORIA", grid.rules[1].data));
                    }
                    if (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data))
                    {
                        where.Add(Expression.Eq("ESTACTIVO", grid.rules[2].data));
                    }
                    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                    {
                        where.Add(Expression.Like("DESCCATEGORIA", '%' + grid.rules[3].data + '%'));
                    }
                  
                }

                var generic = Listar(_categoriaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDECATEGORIA.ToString(),
                    cell = new string[]
                            {
                                "1",
                                item.ESTACTIVO,
                                item.IDECATEGORIA.ToString(),
                                item.NOMCATEGORIA,
                                item.DESCCATEGORIA,
                                item.TIPCATEGORIA,
                                item.FECCREACION.ToString(),
                                item.USRCREACION,
                                item.FECMODIFICA.ToString(),
                                item.USRMODIFICA
                               
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
        /// btnEditarDetalle Edicion del detalle de la categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult btnEditarDetalle(string id)
        {

            CategoriaViewModel model;
            model = new CategoriaViewModel();


            model = InicializarCategoriaEdit();

            var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == Convert.ToInt32(id));

            model.Categoria.IDECATEGORIA = objCategoria.IDECATEGORIA;
            model.Categoria.NOMCATEGORIA = objCategoria.NOMCATEGORIA;
            model.Categoria.DESCCATEGORIA = objCategoria.DESCCATEGORIA;
            model.Categoria.TIPCATEGORIA = objCategoria.TIPCATEGORIA;
            model.Categoria.TIPOEJEMPLO = objCategoria.TIPOEJEMPLO;
            model.Categoria.INSTRUCCIONES = objCategoria.INSTRUCCIONES;
            
            Session["AccionCategoria"] = Accion.Editar;
            Session["Tabla1"] = Grilla.Tabla1;
            Session["Tabla2"] = Grilla.Tabla2;


            if("01".Equals(model.Categoria.TIPOEJEMPLO)){
                Session["Tipo"] = "T";
                model.Categoria.TEXTOEJEMPLO = objCategoria.TEXTOEJEMPLO;
            }else if("02".Equals(model.Categoria.TIPOEJEMPLO)){
              Session["Tipo"]  = "I";
              model.Categoria.IMAGENEJEMPLO = objCategoria.IMAGENEJEMPLO;
            }
            else
            {
                Session["Tipo"] = 1;
            }

            return View("Edit", model);
        }

        /// <summary>
        /// btnConsultarDetalle consulta del detalle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult btnConsultarDetalle(string id)
        {

            CategoriaViewModel model;
            model = new CategoriaViewModel();


            model = InicializarCategoriaEdit();

            var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == Convert.ToInt32(id));

            model.Categoria.IDECATEGORIA = objCategoria.IDECATEGORIA;
            model.Categoria.NOMCATEGORIA = objCategoria.NOMCATEGORIA;
            model.Categoria.DESCCATEGORIA = objCategoria.DESCCATEGORIA;
            model.Categoria.TIPCATEGORIA = objCategoria.TIPCATEGORIA;
            model.Categoria.TIPOEJEMPLO = objCategoria.TIPOEJEMPLO;
            model.Categoria.INSTRUCCIONES = objCategoria.INSTRUCCIONES;
            
            Session["Tabla1"] = Grilla.Tabla1;
            Session["Tabla2"] = Grilla.Tabla2;
            Session["AccionCategoria"] = Accion.Consultar;

            if("01".Equals(model.Categoria.TIPOEJEMPLO)){
                Session["Tipo"] = "T";
                model.Categoria.TEXTOEJEMPLO = objCategoria.TEXTOEJEMPLO;
            }else if("02".Equals(model.Categoria.TIPOEJEMPLO)){
              Session["Tipo"]  = "I";
              model.Categoria.IMAGENEJEMPLO = objCategoria.IMAGENEJEMPLO;
            }
            else
            {
                Session["Tipo"] = 1;
            }

            return View("Edit", model);
        }


        /// <summary>
        /// ActivarDesactivar Activa y desactiva las cartegorias
        /// </summary>
        /// <param name="id"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public ActionResult ActivarDesactivar(string id, string estado)
        {
            var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == Convert.ToInt32(id));

            if (IndicadorActivo.Activo.Equals(estado))
            {
                objCategoria.ESTACTIVO = IndicadorActivo.Inactivo;
            }
            else
            {
                objCategoria.ESTACTIVO = IndicadorActivo.Activo;
            }

            _categoriaRepository.Update(objCategoria);

            CategoriaViewModel model = new CategoriaViewModel();
            model.Categoria = new Categoria();
            model = InicializarCategoriaIndex();

            return View("Index", model); ;
        }

        /// <summary>
        /// EliminarCategoria Elimina la categoria seleccionada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EliminarCategoria(string id)
        {
            CategoriaViewModel model = new CategoriaViewModel();
            model.Categoria = new Categoria();

            model = InicializarCategoriaIndex();
            var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == Convert.ToInt32(id));
            _categoriaRepository.Remove(objCategoria);
            return View("Index", model);

        }

        /// <summary>
        /// EliminarSubCategoria elimina sub categoria
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codSub"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EliminarSubCategoria(string id, string codSub)
        {

            var jsonMessage = new JsonMessage();

           
            var ListaCriterios = _criterioPorSubcategoriaRepository.GetBy(s => s.SubCategoria.IDESUBCATEGORIA == Convert.ToInt32(codSub));

            if (ListaCriterios != null && ListaCriterios.Count>0)
            {
                jsonMessage.Mensaje = "Existen " + ListaCriterios.Count + " criterios asociados a la subcategoria";
                jsonMessage.Resultado = false;
            }
            else
            {
                jsonMessage.Resultado = true;

                var objSubCategoria = _subcategoriaRepository.GetSingle(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(id)
                                                                   && x.IDESUBCATEGORIA == Convert.ToInt32(codSub));
                _subcategoriaRepository.Remove(objSubCategoria);
            }

            return Json(jsonMessage);
        }


        /// <summary>
        /// EliminarCritxSub Elimina los criterios asociados a las subcategorias
        /// </summary>
        /// <param name="id">id Criterio</param>
        /// <param name="codSub">id de subcategoria</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EliminarCritxSub(string id, string codSub)
        {

            var jsonMessage = new JsonMessage();


            var objCriterioxSub = _criterioPorSubcategoriaRepository.GetSingle(s => s.SubCategoria.IDESUBCATEGORIA == Convert.ToInt32(codSub) 
                                                                          && s.Criterio.IdeCriterio == Convert.ToInt32(id));

            jsonMessage.Mensaje = "Se elimino el criterio";
            jsonMessage.Resultado = true;
            _criterioPorSubcategoriaRepository.Remove(objCriterioxSub);
         
            return Json(jsonMessage);
        }


    }
}
