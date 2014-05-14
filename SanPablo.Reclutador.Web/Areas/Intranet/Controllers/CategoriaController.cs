

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
    using System.Configuration;
    using Newtonsoft.Json;

    [Authorize]
    public class CategoriaController : BaseController
    {
        private ICategoriaRepository _categoriaRepository;
        private IAlternativaRepository _alternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISubcategoriaRepository _subcategoriaRepository;
        private ICriterioRepository _criterioRepository;
        private ICriterioPorSubcategoriaRepository _criterioPorSubcategoriaRepository;
        private IExamenRepository _examenRepository;
        private IExamenPorCategoriaRepository _examenPorCategoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository, 
            IDetalleGeneralRepository detalleGeneralRepository, 
            IAlternativaRepository alternativaRepository,
            ISubcategoriaRepository subcategoriaRepository,
            ICriterioRepository criterioRepository,
            ICriterioPorSubcategoriaRepository criterioPorSubcategoriaRepository,
            IExamenRepository examenRepository,
            IExamenPorCategoriaRepository examenPorCategoriaRepository
            )
        {
            _categoriaRepository = categoriaRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _alternativaRepository = alternativaRepository;
            _subcategoriaRepository = subcategoriaRepository;
            _criterioRepository = criterioRepository;
            _criterioPorSubcategoriaRepository = criterioPorSubcategoriaRepository;
            _examenRepository = examenRepository;
            _examenPorCategoriaRepository = examenPorCategoriaRepository;
        }

        [AuthorizeUser]
        public ActionResult Index()
        {

            CategoriaViewModel objCategoriaView = new CategoriaViewModel();
            objCategoriaView = InicializarCategoriaIndex();


            //botonera

            int idRol = (Session[ConstanteSesion.Rol] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Rol]));

            if (Roles.Administrador_Sistema.Equals(idRol))
            {
                objCategoriaView.btnActivarDesactivar = Visualicion.SI;
                objCategoriaView.btnBuscar = Visualicion.SI;
                objCategoriaView.btnConsultar = Visualicion.SI;
                objCategoriaView.btnEditar = Visualicion.SI;
                objCategoriaView.btnEliminar = Visualicion.SI;
                objCategoriaView.btnLimpiar = Visualicion.SI;
                objCategoriaView.btnNuevo = Visualicion.SI;

            }
            else if (Roles.Jefe_Corporativo_Seleccion.Equals(idRol))
            {
                objCategoriaView.btnActivarDesactivar = Visualicion.SI;
                objCategoriaView.btnBuscar = Visualicion.SI;
                objCategoriaView.btnConsultar = Visualicion.SI;
                objCategoriaView.btnEditar = Visualicion.SI;
                objCategoriaView.btnEliminar = Visualicion.SI;
                objCategoriaView.btnLimpiar = Visualicion.SI;
                objCategoriaView.btnNuevo = Visualicion.SI;

            }
            else
            {
                objCategoriaView.btnActivarDesactivar = Visualicion.NO;
                objCategoriaView.btnBuscar = Visualicion.NO;
                objCategoriaView.btnConsultar = Visualicion.NO;
                objCategoriaView.btnEditar = Visualicion.NO;
                objCategoriaView.btnEliminar = Visualicion.NO;
                objCategoriaView.btnLimpiar = Visualicion.NO;
                objCategoriaView.btnNuevo = Visualicion.NO;
            }


            return View(objCategoriaView);
        }

        public ActionResult Edit()
        {
            return View();
        }



        /// <summary>
        /// Edicion de categoria update y Insert
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult Edit(CategoriaViewModel model)
        {

            JsonMessage objJson = new JsonMessage();
            ValidationResult result = null;
            CategoriaValidator validator = new CategoriaValidator();
            CategoriaViewModel categoriaViewModel = new CategoriaViewModel();
            categoriaViewModel.Categoria = new Categoria();
            JsonMessage objJsonMessage = new JsonMessage();
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("DD/MM/YYYY");
            string mensaje = "";
            Boolean resultado = false;

            //try
            //{

                    int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));

                    //result = validator.Validate(model.Categoria, "DESCCATEGORIA", "NOMCATEGORIA", "TIPCATEGORIA", "INSTRUCCIONES");
                    if ("02".Equals(model.Categoria.TIPOEJEMPLO))
                    {
                        Session["Tipo"] = "I";
                    }
                    if ("01".Equals(model.Categoria.TIPOEJEMPLO))
                    {
                        Session["Tipo"] = "T";
                    }

                    categoriaViewModel = InicializarCategoriaEdit();
                    //if (!result.IsValid)
                    //{
                    //    return View(categoriaViewModel);
                    //}

           
                    string fullPath = null;
                    if (!string.IsNullOrEmpty(model.NombreTempImgCategoria))
                    {
                        string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                        string directoryPath = "Archivos\\Imagenes\\";
                        fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, model.NombreTempImgCategoria));

                        using (Stream s = System.IO.File.OpenRead(fullPath))
                        {
                            byte[] buffer = new byte[s.Length];
                            s.Read(buffer, 0, (int)s.Length);
                            int len = (int)s.Length;
                            s.Close();
                            model.Categoria.IMAGENEJEMPLO = buffer;
                        }
                    }


                    if (Accion.Nuevo.Equals(Session["AccionCategoria"]))
                    {

                        if (model.Categoria.IDECATEGORIA != null && model.Categoria.IDECATEGORIA > 0)
                        {
                            var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == model.Categoria.IDECATEGORIA);
                            objCategoria.USRCREACION = UsuarioActual.NombreUsuario;
                            objCategoria.FECCREACION = Hoy;
                            objCategoria.USRMODIFICA = UsuarioActual.NombreUsuario;
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
                            else if (("02".Equals(model.Categoria.TIPOEJEMPLO)))
                            {
                                if (model.NombreTempImgCategoria != null)
                                {
                                    objCategoria.IMAGENEJEMPLO = model.Categoria.IMAGENEJEMPLO;
                                    objCategoria.NOMBREIMAGEN = model.Categoria.NOMBREIMAGEN;
                                }
                        
                            }
                            objCategoria.IdeSede = IdSede;

                            _categoriaRepository.Update(objCategoria);

                            mensaje = "Se actualizó la categoría correctamente";
                            resultado = true;
                    
                        }
                        else
                        {
                            model.Categoria.USRCREACION = UsuarioActual.NombreUsuario;
                            model.Categoria.FECCREACION = Hoy;
                            model.Categoria.USRMODIFICA = UsuarioActual.NombreUsuario;
                            model.Categoria.FECMODIFICA = Hoy;
                            //model.Categoria.ORDENIMPRESION = "1";
                            model.Categoria.ESTACTIVO = IndicadorActivo.Activo;


                            var listaCategoria = _categoriaRepository.All();
                            int maxOrdenImp = 0;
                            if (listaCategoria!=null && listaCategoria.Count>0)
                            {
                                maxOrdenImp = (listaCategoria.Select(d => d.ORDENIMPRESION).Max()) == null ? 0 : (listaCategoria.Select(d => d.ORDENIMPRESION).Max());    
                            }
                    
                            maxOrdenImp = maxOrdenImp + 1;

                            model.Categoria.ORDENIMPRESION = maxOrdenImp;
                            model.Categoria.IdeSede = IdSede;
                            _categoriaRepository.Add(model.Categoria);
                    
                            Session["Tabla1"] = Grilla.Tabla1;
                            Session["Tabla2"] = Grilla.Tabla2;

                            mensaje = "Se registro la categoría correctamente";
                            resultado = true;
                        }
                    }
            
                    if (Accion.Editar.Equals(Session["AccionCategoria"]))
                    {
                        if (model.Categoria.IDECATEGORIA != null && model.Categoria.IDECATEGORIA > 0)
                        {
                            var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == model.Categoria.IDECATEGORIA);
                            objCategoria.USRCREACION = UsuarioActual.NombreUsuario;
                            objCategoria.FECCREACION = Hoy;
                            objCategoria.USRMODIFICA = UsuarioActual.NombreUsuario;
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
                            else if ("02".Equals(model.Categoria.TIPOEJEMPLO))
                            {
                                if (model.NombreTempImgCategoria!=null)
                                {
                                    objCategoria.IMAGENEJEMPLO = model.Categoria.IMAGENEJEMPLO;
                                    objCategoria.NOMBREIMAGEN = model.Categoria.NOMBREIMAGEN;    
                                }
                        
                            }
                            objCategoria.IdeSede = IdSede;
                            _categoriaRepository.Update(objCategoria);

                            mensaje = "Se actulizo la categoría correctamente";
                            resultado = true;
                        }
                    }
            
            
                    categoriaViewModel.Categoria.IDECATEGORIA = model.Categoria.IDECATEGORIA;
                    categoriaViewModel.Categoria.NOMCATEGORIA = model.Categoria.NOMCATEGORIA;
                    categoriaViewModel.Categoria.DESCCATEGORIA = model.Categoria.DESCCATEGORIA;
                    categoriaViewModel.Categoria.TIPCATEGORIA = model.Categoria.TIPCATEGORIA;
                    categoriaViewModel.Categoria.TIPOEJEMPLO = model.Categoria.TIPOEJEMPLO;
                    categoriaViewModel.Categoria.TEXTOEJEMPLO = model.Categoria.TEXTOEJEMPLO;
                    categoriaViewModel.Categoria.INSTRUCCIONES = model.Categoria.INSTRUCCIONES;
                    categoriaViewModel.Categoria.NOMBREIMAGEN = model.Categoria.NOMBREIMAGEN;
                    categoriaViewModel.image = model.image;

                    if (fullPath != null)
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    var objLista = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == categoriaViewModel.Categoria.IDECATEGORIA);
                    int contTiempo = 0;
                    if (objLista != null)
                    {
                        foreach (SubCategoria item in objLista)
                        {
                            contTiempo = contTiempo + item.TIEMPO;
                        }
                    }
            
                    categoriaViewModel.Categoria.TIEMPO = contTiempo;

                    //Se actualiza el tiempo
                    var obj = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == model.Categoria.IDECATEGORIA);
                    obj.TIEMPO = categoriaViewModel.Categoria.TIEMPO;
                    _categoriaRepository.Update(obj);

                    //return RedirectToAction("btnEditarDetalle", "Categoria", new { id = categoriaViewModel.Categoria.IDECATEGORIA });

                    objJson.Mensaje = mensaje;
                    objJson.Resultado = resultado;
                    objJson.IdDato = categoriaViewModel.Categoria.IDECATEGORIA;

                    return Json(objJson);

            //}
            //catch (Exception)
            //{

            //    MensajeError();
            //}

        }


        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult PopupSubCategoria(CategoriaViewModel model)
        {
            
            SubCategoriaValidator objSubCategoriaValidate = new SubCategoriaValidator();
            //ValidationResult result = objSubCategoriaValidate.Validate(model.SubCategoria, "NOMSUBCATEGORIA", "DESCSUBCATEGORIA","TIEMPO");
            JsonMessage objJsonMessge = new JsonMessage();


            int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));

            //if (!result.IsValid)
            //{
            //    return View(model);
            //}

            CategoriaViewModel objCategoriaModel = new CategoriaViewModel();
            objCategoriaModel.SubCategoria = new SubCategoria();
            objCategoriaModel.SubCategoria.Categoria = new Categoria();
            DateTime Hoy = DateTime.Today;


            if (model.SubCategoria.IDESUBCATEGORIA != null && model.SubCategoria.IDESUBCATEGORIA > 0)
            {
                objCategoriaModel.SubCategoria = _subcategoriaRepository.GetSingle(x => x.IDESUBCATEGORIA == model.SubCategoria.IDESUBCATEGORIA);
                objCategoriaModel.SubCategoria.FECMODIFICACION = Hoy;
                objCategoriaModel.SubCategoria.USRMODIFICACION = UsuarioActual.NombreUsuario;
                objCategoriaModel.SubCategoria.NOMSUBCATEGORIA = model.SubCategoria.NOMSUBCATEGORIA;
                objCategoriaModel.SubCategoria.DESCSUBCATEGORIA = model.SubCategoria.DESCSUBCATEGORIA;
                objCategoriaModel.SubCategoria.Categoria = model.Categoria;
                objCategoriaModel.SubCategoria.ORDENIMPRESION = model.SubCategoria.ORDENIMPRESION;
                objCategoriaModel.SubCategoria.TIEMPO = model.SubCategoria.TIEMPO;

                _subcategoriaRepository.Update(objCategoriaModel.SubCategoria);
            }
            else
            {
              
                objCategoriaModel.SubCategoria.FECMODIFICACION = Hoy;
                objCategoriaModel.SubCategoria.FECCREACION = Hoy;
                objCategoriaModel.SubCategoria.USRCREACION = UsuarioActual.NombreUsuario;
                objCategoriaModel.SubCategoria.USRMODIFICACION = UsuarioActual.NombreUsuario;
                objCategoriaModel.SubCategoria.ORDENIMPRESION = 1;
                objCategoriaModel.SubCategoria.ESTACTIVO = IndicadorActivo.Activo;
                objCategoriaModel.SubCategoria.NOMSUBCATEGORIA = model.SubCategoria.NOMSUBCATEGORIA;
                objCategoriaModel.SubCategoria.DESCSUBCATEGORIA = model.SubCategoria.DESCSUBCATEGORIA;
                objCategoriaModel.SubCategoria.Categoria = model.Categoria;
                objCategoriaModel.SubCategoria.TIEMPO = model.SubCategoria.TIEMPO;

                var listaCategoria = _subcategoriaRepository.GetBy(s => s.Categoria.IDECATEGORIA == Convert.ToInt32(model.Categoria.IDECATEGORIA));
                int maxOrdenImp = 0;
                if (listaCategoria != null && listaCategoria.Count>0)
                {
                    maxOrdenImp = (listaCategoria.Select(d => d.ORDENIMPRESION).Max()) == null ? 0 : (listaCategoria.Select(d => d.ORDENIMPRESION).Max());
                }
                maxOrdenImp = maxOrdenImp + 1;

                objCategoriaModel.SubCategoria.ORDENIMPRESION = maxOrdenImp;
                objCategoriaModel.SubCategoria.IdeSede = IdSede;

                _subcategoriaRepository.Add(objCategoriaModel.SubCategoria);
            }

            
            var objLista = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == objCategoriaModel.SubCategoria.Categoria.IDECATEGORIA);
            int contTiempo = 0;
            if (objLista!=null)
            {
                foreach (SubCategoria item in objLista)
                {
                    contTiempo = contTiempo + item.TIEMPO;
                }
            }

            objJsonMessge.IdDato = contTiempo;

            var obj = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == objCategoriaModel.SubCategoria.Categoria.IDECATEGORIA);
            obj.TIEMPO = contTiempo;

            _categoriaRepository.Update(obj);

            return Json(objJsonMessge);
           
        }
        
        [ValidarSesion]
        public ActionResult PopupSubCategoria(int id,string idSubCategoria)
        {

            try
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
            catch (Exception)
            {

                return MensajeError();
            }

        }


        [HttpPost]
        public ActionResult ListaSubCategoria(GridTable grid, int idCategoria)
        {

            try
            {
                

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;
                int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));
                

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
                                item.ESTACTIVO==null?"":item.ESTACTIVO,
                                item.NOMSUBCATEGORIA==null?"":item.NOMSUBCATEGORIA,
                                item.DESCSUBCATEGORIA==null?"":item.DESCSUBCATEGORIA,
                                item.ORDENIMPRESION==null?"":item.ORDENIMPRESION.ToString(),
                                item.TIEMPO==null?"":item.TIEMPO.ToString(),
                                item.FECCREACION==null?"":String.Format("{0:dd/MM/yyyy}", item.FECCREACION),
                                item.USRCREACION==null?"":item.USRCREACION.ToString(),
                                item.FECMODIFICACION==null?"":String.Format("{0:dd/MM/yyyy}", item.FECMODIFICACION),
                                item.USRMODIFICACION==null?"":item.USRMODIFICACION
                                
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

                int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));

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
                                item.SubCategoria.IDESUBCATEGORIA==null?"":item.SubCategoria.IDESUBCATEGORIA.ToString(),
                                item.Criterio.IdeCriterio==null?"":item.Criterio.IdeCriterio.ToString(),
                                item.Criterio.Pregunta== null?"":item.Criterio.Pregunta,
                                item.PUNTAJECAL== null? "":item.PUNTAJECAL.ToString(),
                                item.PRIORIDAD==null?"":item.PRIORIDAD.ToString()
                               
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
        public ActionResult Nuevo()
        {
            CategoriaViewModel model = new CategoriaViewModel();
            model.Categoria = new Categoria();

            model = InicializarCategoriaEdit();
            model.Categoria = new Categoria();
            model.Categoria.DesEstado = "Activo";
              



            model.IndVisual = Visualicion.NO;
            Session["AccionCategoria"] = Accion.Nuevo;
            Session["Tabla1"] = null;
            Session["Tabla2"] = null;
            Session["Tipo"] = 1;
           

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
                int IdSede = (Session[ConstanteSesion.Sede]==null?0:Convert.ToInt32(Session[ConstanteSesion.Sede]));
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
                                item.NombreAlternativa==null?"":item.NombreAlternativa.ToString(),
                                item.IdeAlternativa.ToString(),
                                item.Peso.ToString(),
                                item.Criterio.TipoModo
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
        [ValidarSesion]
        public ActionResult Index(CategoriaViewModel model)
        {
            try
            {

                CategoriaViewModel objCategoriaViewModel = new CategoriaViewModel();
            
                objCategoriaViewModel = InicializarCategoriaIndex();
                objCategoriaViewModel.Criterio = new Criterio();
                objCategoriaViewModel.Categoria = new Categoria();

                objCategoriaViewModel.Categoria.TipoCriterio = model.Categoria.TipoCriterio;
                objCategoriaViewModel.Categoria.ESTACTIVO = model.Categoria.ESTACTIVO;

                return View(objCategoriaViewModel);
            }
            catch (Exception)
            {

                return MensajeError();
            }
        }


        /// <summary>
        /// Lista principal de categoria
        /// </summary>
        /// <param name="grid">los rules</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaPrincipalCategoria(GridTable grid)
        {
            try
            {
                // int idCriterio = Convert.ToInt32(grid.rules[0].data);
                DetachedCriteria where = null;
                int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));
                where = DetachedCriteria.For<Categoria>();

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)) ||
                    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                   )
                {
                    

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

                        string desc = grid.rules[3].data;

                        if (desc!=null)
                        {
                            where.Add(Expression.Like("DESCCATEGORIA", '%' + desc.Trim() + '%'));
                        }
                        
                       
                    }
                  
                }

                where.Add(Expression.Eq("IdeSede", IdSede));

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
                                item.TIPCATEGORIADES,
                                String.Format("{0:dd/MM/yyyy}", item.FECCREACION),
                                item.USRCREACION,
                                String.Format("{0:dd/MM/yyyy}", item.FECMODIFICA),
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
        [ValidarSesion]
        public ActionResult btnEditarDetalle(string id)
        {

            try
            {

                CategoriaViewModel model;
                model = new CategoriaViewModel();
           

                model = InicializarCategoriaEdit();
                model.Categoria = new Categoria();
                var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == Convert.ToInt32(id));

                if (objCategoria!=null)
                {
                    if (IndicadorActivo.Activo.Equals(objCategoria.ESTACTIVO))
                    {
                        model.Categoria.DesEstado = "Activo";
                    }
                    else
                    {
                        model.Categoria.DesEstado = "Inactivo";
                    }
                }


                model.Categoria.IDECATEGORIA = objCategoria.IDECATEGORIA;
                model.Categoria.NOMCATEGORIA = objCategoria.NOMCATEGORIA;
                model.Categoria.DESCCATEGORIA = objCategoria.DESCCATEGORIA;
                model.Categoria.TIPCATEGORIA = objCategoria.TIPCATEGORIA;
                model.Categoria.TIPOEJEMPLO = objCategoria.TIPOEJEMPLO;
                model.Categoria.INSTRUCCIONES = objCategoria.INSTRUCCIONES;
                model.Categoria.NOMBREIMAGEN = objCategoria.NOMBREIMAGEN;

                var objLista = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == model.Categoria.IDECATEGORIA);
                int contTiempo = 0;
                if (objLista != null)
                {
                    foreach (SubCategoria item in objLista)
                    {
                        contTiempo = contTiempo + item.TIEMPO;
                    }
                }

                model.Categoria.TIEMPO = contTiempo;
            
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
            catch (Exception)
            {

                return MensajeError();
            }
        }

        /// <summary>
        /// btnConsultarDetalle consulta del detalle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult btnConsultarDetalle(string id)
        {

            try
            {

                CategoriaViewModel model;
                model = new CategoriaViewModel();


                model = InicializarCategoriaEdit();

                var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == Convert.ToInt32(id));


                if (objCategoria != null)
                {
                    if (IndicadorActivo.Activo.Equals(objCategoria.ESTACTIVO))
                    {
                        model.Categoria.DesEstado = "Activo";
                    }
                    else
                    {
                        model.Categoria.DesEstado = "Inactivo";
                    }
                }

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
                  model.Categoria.NOMBREIMAGEN = objCategoria.NOMBREIMAGEN;
                }
                else
                {
                    Session["Tipo"] = 1;
                }

                var objLista = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == model.Categoria.IDECATEGORIA);
                int contTiempo = 0;
                if (objLista != null)
                {
                    foreach (SubCategoria item in objLista)
                    {
                        contTiempo = contTiempo + item.TIEMPO;
                    }
                }

                model.Categoria.TIEMPO = contTiempo;

                return View("Edit", model);
            }
            catch (Exception)
            {

                return MensajeError();
            }
            
        }


        /// <summary>
        /// ActivarDesactivar Activa y desactiva las cartegorias
        /// </summary>
        /// <param name="id"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult ActivarDesactivar(string id, string estado)
        {

            try
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

                return View("Index", model);
            }
            catch (Exception)
            {

                return MensajeError();
            }

        }



        /// <summary>
        /// valida si no hay dependencias
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult ValidaCategoria(int id) 
        {

            try
            {
                JsonMessage jsonMensaje = new JsonMessage();
                string mensaje = "";
                
                string nombExamen = "";
                int contExamen = 0;
                var ListaExemxCategoria = _examenPorCategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(id));
                if (ListaExemxCategoria!=null)
                {
                    if (ListaExemxCategoria.Count>0)
                    {
                        contExamen = contExamen + 1;
                        foreach (ExamenPorCategoria item in ListaExemxCategoria)
                        {

                            var ObjExamen = _examenRepository.GetSingle(x => x.IdeExamen == item.Examen.IdeExamen);
                            
                            if (contExamen==1)
                            {
                                nombExamen = nombExamen + ObjExamen.NomExamen;
                            }
                            else
                            {
                                nombExamen = nombExamen + ", " + ObjExamen.NomExamen;
                            }
                    
                        }

                        if (contExamen == 1)
                        {
                            mensaje = "No se puede eliminar la categoría debido a que se encuentra relacionada con el examen: " + nombExamen;
                        }
                        else
                        {
                            mensaje = "No se puede eliminar la categoría debido a que se encuentra relacionada con los examenes: " + nombExamen;
                        }
                       

                        jsonMensaje.Resultado = false;
                        jsonMensaje.Mensaje = mensaje;

                        return Json(jsonMensaje);
                    }    
                }

                var listaSubcategoria = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(id));

                if (listaSubcategoria!=null)
                {
                    if (listaSubcategoria.Count>0)
                    {

                        if (listaSubcategoria.Count==1)
                        {
                            mensaje = "No se puede eliminar la categoría debido a que contiene: " + listaSubcategoria.Count + " Subcategoría";
                        }
                        else
                        {
                            mensaje = "No se puede eliminar la categoría debido a que contiene: " + listaSubcategoria.Count + " subcategorías";
                        }
                       

                        jsonMensaje.Resultado = false;
                        jsonMensaje.Mensaje = mensaje;

                        return Json(jsonMensaje);

                    }    
                }

                jsonMensaje.Resultado = true;

                return Json(jsonMensaje);

            }
            catch (Exception)
            {

                return MensajeError();
            }

        }






        /// <summary>
        /// EliminarCategoria Elimina la categoria seleccionada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult EliminarCategoria(string id)
        {
            try
            {
                CategoriaViewModel model = new CategoriaViewModel();
                model.Categoria = new Categoria();

                model = InicializarCategoriaIndex();

                ExamenPorCategoria objExamenxCategoria;

                var ListaExemxCategoria = _examenPorCategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(id));
                if (ListaExemxCategoria != null)
                {
                    foreach (ExamenPorCategoria item in ListaExemxCategoria)
                    {
                        objExamenxCategoria = new ExamenPorCategoria();
                        objExamenxCategoria = item;
                        _examenPorCategoriaRepository.Remove(objExamenxCategoria);
                    }
                }


                var objCategoria = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == Convert.ToInt32(id));
                if (objCategoria!=null)
                {
                    _categoriaRepository.Remove(objCategoria);
                }

                SubCategoria objSub;
                CriterioPorSubcategoria objCritSub;
                var objSubcategoria = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(id));
                if (objSubcategoria!=null)
                {
                    foreach (SubCategoria item in objSubcategoria)
                    {
                        objSub = new SubCategoria();
                        objSub = item;
                        _subcategoriaRepository.Remove(objSub);


                        var ListCritSub = _criterioPorSubcategoriaRepository.GetBy(x => x.SubCategoria.IDESUBCATEGORIA == item.IDESUBCATEGORIA);
                        if (ListCritSub != null)
                        {
                            foreach (CriterioPorSubcategoria item2 in ListCritSub)
                            {
                                objCritSub = new CriterioPorSubcategoria();
                                objCritSub = item2;
                                _criterioPorSubcategoriaRepository.Remove(objCritSub);
                            }
                        }
                    }

                }

                return View("Index", model);
            }
            catch (Exception)
            {

                return MensajeError();
            }
        }

        /// <summary>
        /// EliminarSubCategoria elimina sub categoria
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codSub"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult EliminarSubCategoria(string id, string codSub)
        {

            try
            {
                    var jsonMessage = new JsonMessage();
                    int i = 1; 
          
                    var ListaCriterios = _criterioPorSubcategoriaRepository.GetBy(s => s.SubCategoria.IDESUBCATEGORIA == Convert.ToInt32(codSub));

                    if (ListaCriterios != null && ListaCriterios.Count>0)
                    {
                        jsonMessage.Mensaje = "No se puede eliminar el registro debido a que tiene criterios asociados";
                        jsonMessage.Resultado = false;
                    }
                    else
                    {
                        jsonMessage.Resultado = true;

                        var objSubCategoria = _subcategoriaRepository.GetSingle(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(id)
                                                                           && x.IDESUBCATEGORIA == Convert.ToInt32(codSub));
                        _subcategoriaRepository.Remove(objSubCategoria);


                        var listaActualizar = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(id)).OrderBy(x => x.ORDENIMPRESION);


                        foreach (SubCategoria item in listaActualizar)
                        {
                            item.ORDENIMPRESION = i++;
                            _subcategoriaRepository.Update(item);
                        }


                    }

                    //CALCULA EL TIEMPO
                    var objLista = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(id));
                    int contTiempo = 0;
                    if (objLista != null)
                    {
                        foreach (SubCategoria item in objLista)
                        {
                            contTiempo = contTiempo + item.TIEMPO;
                        }
                    }

                    jsonMessage.IdDato = contTiempo;

                    var obj = _categoriaRepository.GetSingle(x => x.IDECATEGORIA == Convert.ToInt32(id));
                    obj.TIEMPO = contTiempo;

                    _categoriaRepository.Update(obj);



                    return Json(jsonMessage);
            }
            catch (Exception)
            {

                return MensajeError();
            }

        }


        /// <summary>
        /// EliminarCritxSub Elimina los criterios asociados a las subcategorias
        /// </summary>
        /// <param name="id">id Criterio</param>
        /// <param name="codSub">id de subcategoria</param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult EliminarCritxSub(string id, string codSub)
        {
            try
            {

                var jsonMessage = new JsonMessage();
                int i = 1;

           
                var objCriterioxSub = _criterioPorSubcategoriaRepository.GetSingle(s => s.SubCategoria.IDESUBCATEGORIA == Convert.ToInt32(codSub) 
                                                                              && s.Criterio.IdeCriterio == Convert.ToInt32(id));

                _criterioPorSubcategoriaRepository.Remove(objCriterioxSub);
            
                var lista = _criterioPorSubcategoriaRepository.GetBy(x => x.SubCategoria.IDESUBCATEGORIA == Convert.ToInt32(codSub)).OrderBy(x => x.PRIORIDAD);

                if (lista != null)
                {
                    foreach (CriterioPorSubcategoria item in lista)
                    {
                    
                        var objeto = _criterioPorSubcategoriaRepository.GetSingle(s => s.SubCategoria.IDESUBCATEGORIA == item.SubCategoria.IDESUBCATEGORIA
                                                                              && s.Criterio.IdeCriterio == item.Criterio.IdeCriterio);

                        objeto.PRIORIDAD = i++;

                        _criterioPorSubcategoriaRepository.Update(objeto);
                    }
                }
            
                jsonMessage.Mensaje = "Se elimino el criterio";
                jsonMessage.Resultado = true;
           
         
                return Json(jsonMessage);
            }
            catch (Exception)
            {

                return MensajeError();
            }

        }

        /// <summary>
        /// IniciaPopupCategoria Inicializa los datos del Popup de busqueda de categorias
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult IniciaPopupCategoria(int id)
        {

            CategoriaViewModel objCategoria = new CategoriaViewModel();
            objCategoria.Categoria = new Categoria();
            objCategoria = InicializarCategoriaIndex();
            objCategoria.Examen = new Examen();


            if (id != null && id > 0)
            {
                objCategoria.Examen.IdeExamen = id;
                var objExamen = _examenRepository.GetSingle(x => x.IdeExamen == objCategoria.Examen.IdeExamen);
                objCategoria.Categoria.TipoCriterio = objExamen.TipExamen;
                
            }

            return View("PopupCategoria", objCategoria);
           
        }



        /// <summary>
        /// ListaPopupCategoria busqueda de categorias por popup
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaPopupCategoria(GridTable grid)
        {
            try
            {
               
                Categoria objCategoria;
                List<Categoria> listaCategorias = new List<Categoria>();

                objCategoria = new Categoria();

                int IdSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));
                
                if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                {
                    objCategoria.TIPCATEGORIA = (grid.rules[1].data == null ? "" : grid.rules[1].data);
                }
                    
                if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                {
                    objCategoria.DESCCATEGORIA = (grid.rules[3].data == null ? "" : grid.rules[3].data).Trim();
                }

                objCategoria.IdeSede = (IdSede==null?0:IdSede);
                
                listaCategorias = _categoriaRepository.ObtenerCategorias(objCategoria);

                var generic = GetListar(listaCategorias,
                                            grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);


                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDECATEGORIA.ToString(),
                    cell = new string[]
                            {
                                "1",
                                item.ESTACTIVO,
                                item.IDECATEGORIA.ToString(),
                                item.NOMCATEGORIA==null?"":item.NOMCATEGORIA,
                                item.DESCCATEGORIA==null?"":item.DESCCATEGORIA,
                                item.TIPCATEGORIA,
                                item.TIPCATEGORIADES
                               
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
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult OrdenarSubCategorias(int ideCategoria, int ideSubcategoria, string accion)
        {

            var jsonMessage = new JsonMessage();
            var listaActualizar = _subcategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == ideCategoria).OrderBy(x=>x.ORDENIMPRESION);
            
            var subCategoriaActual = listaActualizar.Single(x => x.IDESUBCATEGORIA == ideSubcategoria);
            var ordenImpresionActual = subCategoriaActual.ORDENIMPRESION;
            
            int ordenImpresionContiguo = 0;
            if (accion == "Subir")
                ordenImpresionContiguo = ordenImpresionActual - 1;
            else 
                ordenImpresionContiguo = ordenImpresionActual + 1;


            try 
	        {	        
		        var subCategoriaContiguo = listaActualizar.Single(x => x.ORDENIMPRESION == ordenImpresionContiguo);

                subCategoriaContiguo.ORDENIMPRESION = ordenImpresionActual;
                subCategoriaActual.ORDENIMPRESION = ordenImpresionContiguo;

                _subcategoriaRepository.Update(subCategoriaActual);
                _subcategoriaRepository.Update(subCategoriaContiguo);
                jsonMessage.Resultado = true;
                return Json(jsonMessage);
            }
	        catch (Exception ex)
	        {
                return MensajeError();
	        }
            
        }

       
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult OrdenarCriterios(int ideCategoria, int ideSubcategoria, int ideCriterio,string accion)
        {

            var jsonMessage = new JsonMessage();
            var listaActualizar = _criterioPorSubcategoriaRepository.GetBy(x => x.SubCategoria.IDESUBCATEGORIA == ideSubcategoria).OrderBy(x => x.PRIORIDAD);

            var critxCategoriaActual = listaActualizar.Single(x => x.Criterio.IdeCriterio == ideCriterio);
            var ordenImpresionActual = critxCategoriaActual.PRIORIDAD;
            
            int ordenImpresionContiguo = 0;
            if (accion == "Subir")
                ordenImpresionContiguo = ordenImpresionActual - 1;
            else 
                ordenImpresionContiguo = ordenImpresionActual + 1;


            try 
	        {	        
		        var critxCategoriaContiguo = listaActualizar.Single(x => x.PRIORIDAD == ordenImpresionContiguo);

                critxCategoriaContiguo.PRIORIDAD = ordenImpresionActual;
                critxCategoriaActual.PRIORIDAD = ordenImpresionContiguo;

                _criterioPorSubcategoriaRepository.Update(critxCategoriaActual);
                _criterioPorSubcategoriaRepository.Update(critxCategoriaContiguo);

                jsonMessage.Resultado = true;
                return Json(jsonMessage);
	        }
	        catch (Exception ex)
	        {

                return MensajeError();
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
        /// GetImage Muestra la Imagen en el criterio
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetImage(int id)
        {
            var firstOrDefault = _categoriaRepository.GetSingle(c => c.IDECATEGORIA == id);
            if (firstOrDefault.IMAGENEJEMPLO != null)
            {
                byte[] image = firstOrDefault.IMAGENEJEMPLO;
                return File(image, "image/jpg");
            }
            else
            {
                return null;
            }
        }

    }
}
