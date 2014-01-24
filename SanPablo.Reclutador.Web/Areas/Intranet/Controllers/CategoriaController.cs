

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


        public CategoriaController(ICategoriaRepository categoriaRepository, 
            IDetalleGeneralRepository detalleGeneralRepository, 
            IAlternativaRepository alternativaRepository,
            ISubcategoriaRepository subcategoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _alternativaRepository = alternativaRepository;
            _subcategoriaRepository = subcategoriaRepository;
        }
        
        
        public ActionResult Index()
        {
            return View();
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
                    model.Categoria.ORDENIMPRESION = "1";
                    model.Categoria.ESTACTIVO = "A";
                    _categoriaRepository.Add(model.Categoria);
                    //Session.Remove("AccionCategoria");
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
            //[Bind(Prefix = "SubCategoria")]SubCategoria

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
        public ActionResult ListaCategoria(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "200001",
          "Cat01",
          "Categoría 01",
          "Evaluación",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "200002",
          "Cat02",
          "Categoría 02",
          "Entrevista",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[]
        {
          "200003",
          "Cat03",
          "Categoría 03",
          "Entrevista",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
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
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }

           

        //    List<object> list = new List<object>();
        //    var fAnonymousType2_1 = new
        //    {
        //        id = 1,
        //        cell = new string[]
        //{
        //  "200001",
        //  "Activo",
        //  "SubCat01",
        //  "SubCategoría 01",
        //  "5",
        //  "01/01/2013",
        //  "Admin",
        //  "10/10/2013",
        //  "Admin"
          
        //}
        //    };
        //    list.Add((object)fAnonymousType2_1);
        //    var fAnonymousType2_2 = new
        //    {
        //        id = 2,
        //        cell = new string[]
        //{
        //  "200002",
        //  "Activo",
        //  "SubCat02",
        //  "SubCategoría 02",
        //  "4",
        //  "01/01/2013",
        //  "Admin",
        //  "10/10/2013",
        //  "Admin"
          
        //}
        //    };
        //    list.Add((object)fAnonymousType2_2);
        //    var fAnonymousType3 = new
        //    {
        //        rows = list
        //    };
        //    return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaCriterio(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "100001",
          "¿Cuál es la capital del Perú?",
          "10",
          "1",
          "Automática"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "100002",
          "¿Cómo se llama el Presidente del Perú?",
          "10",
          "1",
          "Automática"
        }
            };

            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
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

    
    }
}
