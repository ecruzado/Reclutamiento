﻿namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
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

    public class CriterioController : BaseController
    {
        private ICriterioRepository _criterioRepository;
        private IAlternativaRepository _alternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICriterioPorSubcategoriaRepository _criterioPorSubcategoriaRepository;

        public CriterioController(ICriterioRepository criterioRepository, 
            IDetalleGeneralRepository detalleGeneralRepository, 
            IAlternativaRepository alternativaRepository,
            ICriterioPorSubcategoriaRepository criterioPorSubcategoriaRepository
            )
        {
            _criterioRepository = criterioRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _alternativaRepository = alternativaRepository;
            _criterioPorSubcategoriaRepository = criterioPorSubcategoriaRepository;
        }
        
        public ActionResult Index()
        {
            var criterioViewModel = InicializarCriteriosIndex();
            return View(criterioViewModel);
        }

        [HttpPost]
        public ActionResult ListaCriterio(GridTable grid)
        {
            try
            {
                DetachedCriteria where = null;
                
                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data) ) ||
                    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data !="0")
                   )
                {
                    where = DetachedCriteria.For<Criterio>();

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
                                item.Pregunta,
                                item.TipoMedicion,
                                item.TipoCriterio,
                                item.TipoModo,
                                item.TipoCalificacion,
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

        [HttpPost]
        public ActionResult PopupCriterio(CriterioViewModel model, HttpPostedFileBase ImagenAlternativa)
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

            /*HttpPostedFileBase imagen = Request.Files[model.Alternativa.RutaDeImagen];


            if (imagen != null)
            {
                //string filePath = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(model.image.FileName));
                //model.image.SaveAs(filePath);
                byte[] data;

                using (Stream inputStream = imagen.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }
                model.Alternativa.Image = data;

            }
            */
            

            if (model.Alternativa.IdeAlternativa != 0 && model.Alternativa.IdeAlternativa != null)
            {

                var alter = _alternativaRepository.GetSingle(x => x.IdeAlternativa == model.Alternativa.IdeAlternativa);
                alter.NombreAlternativa = model.Alternativa.NombreAlternativa;
                alter.Peso = model.Alternativa.Peso;

                model.Alternativa.FechaMod = Hoy;
                model.Alternativa.UsrMod = "Prueba 02";
                

                _alternativaRepository.Update(alter);


            }
            else
            {
                model.Alternativa.FechaCreacion = Hoy;
                model.Alternativa.UsrCreacion = "Prueba 01";
                _alternativaRepository.Add(model.Alternativa);
            }

            criterioViewModel.Alternativa.Criterio.IdeCriterio = model.Alternativa.Criterio.IdeCriterio;
            criterioViewModel.Alternativa.IdeAlternativa = model.Alternativa.IdeAlternativa;
            criterioViewModel.Alternativa.NombreAlternativa = model.Alternativa.NombreAlternativa;
            criterioViewModel.Alternativa.Peso = model.Alternativa.Peso;

            return View(criterioViewModel);
        }

        public ActionResult Edit(string ideCriterio)
        {

            var criterioViewModel = InicializarCriteriosEdit();
            return View(criterioViewModel);
        }

        public ActionResult BuscarCriterios()
        {
            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosIndex();
            
            return View("Index", model);
        }

        public ActionResult Nuevo()
        {
            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosEdit();
            model.Criterio.IndPagina = Accion.Nuevo;
            model.IndVisual = Visualicion.NO;
            
            Session["TipoModo"] = 1;
            
            return View("Edit", model);
        }

        public ActionResult Edicion(string id)
        {

            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosEdit();
            
            var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == Convert.ToInt32(id));
            model.Criterio.IdeCriterio = objCriterio.IdeCriterio;
            model.Criterio.TipoCalificacion = objCriterio.TipoCalificacion;
            model.Criterio.TipoCriterio = objCriterio.TipoCriterio;
            model.Criterio.TipoMedicion = objCriterio.TipoMedicion;
            model.Criterio.TipoModo = objCriterio.TipoModo;
            model.Criterio.IdeCriterio = objCriterio.IdeCriterio;
            model.Criterio.Pregunta = objCriterio.Pregunta;
            model.Criterio.IndPagina = Accion.Actualizar.ToString();
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

        public ActionResult EliminarCriterio(string id)
        {
            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosIndex();
            var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == Convert.ToInt32(id));
            _criterioRepository.Remove(objCriterio);
            return View("Index", model);

        }

        public ActionResult ConsultaCriterios(string id)
        {

            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosEdit();

            var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == Convert.ToInt32(id));
            model.Criterio.IdeCriterio = objCriterio.IdeCriterio;
            model.Criterio.TipoCalificacion = objCriterio.TipoCalificacion;
            model.Criterio.TipoCriterio = objCriterio.TipoCriterio;
            model.Criterio.TipoMedicion = objCriterio.TipoMedicion;
            model.Criterio.TipoModo = objCriterio.TipoModo;
            model.Criterio.IdeCriterio = objCriterio.IdeCriterio;
            model.Criterio.Pregunta = objCriterio.Pregunta;
            model.Criterio.IndPagina = Accion.Consultar;
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
                
                //obtiene el valor del criterio
               
                
               // int idCriterio = Convert.ToInt32(grid.rules[0].data);

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
        public ActionResult Edit(CriterioViewModel model, HttpPostedFileBase image)
        {
            var criterioViewModel = InicializarCriteriosEdit();
            byte[] data; 
            if (!ModelState.IsValid){
                criterioViewModel.Criterio = model.Criterio;
                return View(criterioViewModel);
            }

           
            model.Criterio.IndicadorActivo = IndicadorActivo.Activo;

            if (model.image != null)
            {
                //string filePath = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(model.image.FileName));
                //model.image.SaveAs(filePath);
               

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
                model.Criterio.IMAGENCRIT = data;

            }
            
            
            
            if (Accion.Nuevo.Equals(model.Criterio.IndPagina))
            {
                model.Criterio.FechaCreacion = DateTime.Now;
                model.Criterio.UsuarioCreacion = UsuarioActual.NombreUsuario;


                //var objCriterio = _criterioRepository.All();
                //int maxOrdenImp = (objCriterio.Select(d => d.OrdenImpresion).Max()) == null ? 0 : (objCriterio.Select(d => d.OrdenImpresion).Max());

                //maxOrdenImp = maxOrdenImp + 1;

                //model.Criterio.OrdenImpresion = maxOrdenImp;
                model.Criterio.OrdenImpresion = 0;

                _criterioRepository.Add(model.Criterio);
            }
            else
            {
                
                var objCriterio = _criterioRepository.GetSingle(x => x.IdeCriterio == model.Criterio.IdeCriterio);
                objCriterio.TipoCriterio = model.Criterio.TipoCriterio;
                objCriterio.TipoMedicion = model.Criterio.TipoMedicion;
                objCriterio.TipoModo = model.Criterio.TipoModo;
                objCriterio.TipoCalificacion = model.Criterio.TipoCalificacion;
                objCriterio.Pregunta = model.Criterio.Pregunta;
                objCriterio.FechaModificacion = DateTime.Now;
                objCriterio.UsuarioModificacion = UsuarioActual.NombreUsuario;

                if ("02".Equals(model.Criterio.TipoModo))
                {
                    objCriterio.IMAGENCRIT = model.Criterio.IMAGENCRIT;
                }
                
                _criterioRepository.Update(objCriterio);
            }

            criterioViewModel.IndVisual = Visualicion.SI;
            criterioViewModel.Criterio.TipoMedicion = model.Criterio.TipoMedicion;
            criterioViewModel.Criterio.TipoCriterio = model.Criterio.TipoCriterio;
            criterioViewModel.Criterio.TipoModo = model.Criterio.TipoModo;
            if("01".Equals(model.Criterio.TipoModo))
            {
                Session["TipoModo"] = "T";
            }
            else
	        {
                Session["TipoModo"] = "I";
                criterioViewModel.image = model.image;
	        }
            criterioViewModel.Criterio.Pregunta = model.Criterio.Pregunta;
            criterioViewModel.Criterio.TipoCalificacion = model.Criterio.TipoCalificacion;
            criterioViewModel.Criterio.IdeCriterio = model.Criterio.IdeCriterio;
           

            return View("Edit",criterioViewModel);

        }


        [HttpPost]
        public ActionResult Index(CriterioViewModel model)
        {
            var criterioViewModel = InicializarCriteriosIndex();

            criterioViewModel.Criterio = model.Criterio;
            
            //_criterioRepository.Add(model.Criterio);

            criterioViewModel.Criterio.TipoMedicion = model.Criterio.TipoMedicion;
            criterioViewModel.Criterio.TipoCriterio = model.Criterio.TipoCriterio;
            criterioViewModel.Criterio.TipoModo = model.Criterio.TipoModo;
            criterioViewModel.Criterio.Pregunta = model.Criterio.Pregunta;
            criterioViewModel.Criterio.TipoCalificacion = model.Criterio.TipoCalificacion;

            //criterioViewModel.Criterio.IdeCriterio = model.Criterio.IdeCriterio;


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


            criterioViewModel.TipoCalificacion =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCalificacion));
            criterioViewModel.TipoCalificacion.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            return criterioViewModel;
        }

        [HttpPost]
        public ActionResult EdicionCriterio(string selr)
        {

            //var alter = _alternativaRepository.GetSingle(x => x.IdeAlternativa == Convert.ToInt32(ideAlternativa));

            //_alternativaRepository.Remove(alter);
            return null;
        }
        

        // lista de criterios del popup
        [HttpPost]
        public ActionResult ListaCriterioxSubCategoria(GridTable grid)
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
                    where = DetachedCriteria.For<Criterio>();

                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        where.Add(Expression.Eq("TipoCriterio", grid.rules[0].data));
                    }
                    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                    {
                        where.Add(Expression.Eq("TipoMedicion", grid.rules[1].data));
                    }
                    if (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0")
                    {
                        where.Add(Expression.Like("Pregunta", '%' + grid.rules[2].data + '%'));
                    }
                    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                    {
                        where.Add(Expression.Eq("IndicadorActivo", grid.rules[3].data));
                    }
                }

                var generic = Listar(_criterioRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeCriterio.ToString(),
                    cell = new string[]
                            {
                                "",
                                item.IndicadorActivo,
                                item.IndicadorActivo,
                                item.Pregunta,
                                item.TipoMedicion,
                                item.TipoCriterio,
                                item.TipoModo,
                                item.TipoCalificacion
                               
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

        public ViewResult PopupListaCriterio(int id, string idSubCategoria)
        {
            try
            {
                CriterioViewModel objCriterioModel = new CriterioViewModel();
                objCriterioModel = InicializarCriteriosIndex();
                objCriterioModel.CriterioPorSubcategoria = new CriterioPorSubcategoria();
                objCriterioModel.CriterioPorSubcategoria.IDESUBCATEGORIA = Convert.ToInt32(idSubCategoria);
                
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
                       objCriterioxSubCategoria.ESTREGISTRO = "A";
                       objCriterioxSubCategoria.USRCREACION = "PRUEBA";
                       objCriterioxSubCategoria.USRMODIFICA = "PRUEBA2";
                       objCriterioxSubCategoria.FECCREACION = Hoy;
                       objCriterioxSubCategoria.FECMODIFICA = Hoy;

                       _criterioPorSubcategoriaRepository.Add(objCriterioxSubCategoria);


                   }

                    

                }
            }

            return null;
        }
       
    }
}
