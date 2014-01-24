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

    public class CriterioController : BaseController
    {
        private ICriterioRepository _criterioRepository;
        private IAlternativaRepository _alternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public CriterioController(ICriterioRepository criterioRepository, 
            IDetalleGeneralRepository detalleGeneralRepository, 
            IAlternativaRepository alternativaRepository)
        {
            _criterioRepository = criterioRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _alternativaRepository = alternativaRepository;
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
                // int idCriterio = Convert.ToInt32(grid.rules[0].data);
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
                                item.FechaCreacion.ToString(),
                                item.UsuarioCreacion,
                                item.FechaModificacion.ToString(),
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

                /*if (imagen.FileName.Length > 0)
                {
                    alter.Image = model.Alternativa.Image;
                }*/

                _alternativaRepository.Update(alter);


            }
            else
            {
                _alternativaRepository.Add(model.Alternativa);
            }

            criterioViewModel.Alternativa.Criterio.IdeCriterio = model.Alternativa.Criterio.IdeCriterio;
            criterioViewModel.Alternativa.IdeAlternativa = model.Alternativa.IdeAlternativa;
            criterioViewModel.Alternativa.NombreAlternativa = model.Alternativa.NombreAlternativa;
            criterioViewModel.Alternativa.Peso = model.Alternativa.Peso;

            return View(criterioViewModel);
        }

        [HttpPost]
        public virtual ActionResult UploadFile()
        {
            HttpPostedFileBase myFile = Request.Files["MyFile"];
            bool isUploaded = false;
            string message = "File upload failed";

            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/Uploads");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        isUploaded = true;
                        message = "File uploaded successfully!";
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }


        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
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

        protected JsonResult MensajeError(string mensaje = "Ocurrio un error al cargar...")
        {
            Response.StatusCode = 404;
            return Json(new JsonResponse { Message = mensaje }, JsonRequestBehavior.AllowGet);
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

        private CriterioViewModel InicializarCriteriosIndex()
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
                                "1",
                                item.IndicadorActivo,
                                item.IndicadorActivo,
                                item.Pregunta,
                                item.TipoMedicion,
                                item.TipoCriterio,
                                item.TipoModo,
                                item.TipoCalificacion,
                                item.FechaCreacion.ToString(),
                                item.UsuarioCreacion,
                                item.FechaModificacion.ToString(),
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


       
    }
}
