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
                //DetachedCriteria where = DetachedCriteria.For<Criterio>();
                //where.Add(Expression.Eq("Criterio.IdeCriterio", 1));


                var generic = Listar(_criterioRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                    {
                        id = item.IdeCriterio.ToString(),
                        cell = new string[]
                            {
                                "1",
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
        public ActionResult PopupCriterio([Bind(Prefix = "Alternativa")]Alternativa alternativa)
        {
            var criterioViewModel = new CriterioViewModel();
            criterioViewModel.Alternativa = new Alternativa();
            criterioViewModel.Alternativa.Criterio = new Criterio();

            AlternativaValidator validator = new AlternativaValidator();
            ValidationResult result = validator.Validate(alternativa, "NombreAlternativa", "Peso");

            if (!result.IsValid)
            {
                return View(criterioViewModel);
            }

            if (alternativa.IdeAlternativa != 0 && alternativa.IdeAlternativa != null)
            {
                
                var alter = _alternativaRepository.GetSingle(x => x.IdeAlternativa == alternativa.IdeAlternativa);
                alter.NombreAlternativa = alternativa.NombreAlternativa;
                alter.Peso = alternativa.Peso;
                _alternativaRepository.Update(alter);
            }
            else
            {
                _alternativaRepository.Add(alternativa);
            }
           
            criterioViewModel.Alternativa.Criterio.IdeCriterio = alternativa.Criterio.IdeCriterio;
            criterioViewModel.Alternativa.IdeAlternativa = alternativa.IdeAlternativa;
            criterioViewModel.Alternativa.NombreAlternativa = alternativa.NombreAlternativa;
            criterioViewModel.Alternativa.Peso = alternativa.Peso;

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

        public ActionResult Consultar(string id)
        {

            var criterioViewModel = InicializarCriteriosEdit();
            return View("Edit",criterioViewModel);
        }

        public ActionResult Nuevo()
        {
            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosEdit();
            model.Criterio.IndPagina = Accion.Nuevo;

            return View("Edit", model);
        }

        public ActionResult Edicion(string id)
        {

            CriterioViewModel model = new CriterioViewModel();
            model.Criterio = new Criterio();

            model = InicializarCriteriosEdit();
            
            var alter = _criterioRepository.GetSingle(x => x.IdeCriterio == Convert.ToInt32(id));
            model.Criterio.TipoCalificacion = alter.TipoCalificacion;
            model.Criterio.TipoCriterio = alter.TipoCriterio;
            model.Criterio.TipoMedicion = alter.TipoMedicion;
            model.Criterio.TipoModo = alter.TipoModo;
            model.Criterio.IdeCriterio = alter.IdeCriterio;
            model.Criterio.Pregunta = alter.Pregunta;
            model.Criterio.IndPagina = Accion.Nuevo.ToString();

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
                                "1",
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
        public ActionResult Edit(CriterioViewModel model, HttpPostedFileBase file)
        {
            var criterioViewModel = InicializarCriteriosEdit();
           

            if (!ModelState.IsValid){
                criterioViewModel.Criterio = model.Criterio;
                return View(criterioViewModel);
            }

            model.Criterio.IndicadorActivo = IndicadorActivo.Activo;

            if (model.Criterio.IndPagina.Equals(Accion.Nuevo))
            {
                _criterioRepository.Add(model.Criterio);
            }
            else
            {
                //actualiza
                _criterioRepository.Update(model.Criterio);
            }
            
            criterioViewModel.Criterio.TipoMedicion = model.Criterio.TipoMedicion;
            criterioViewModel.Criterio.TipoCriterio = model.Criterio.TipoCriterio;
            criterioViewModel.Criterio.TipoModo = model.Criterio.TipoModo;
            criterioViewModel.Criterio.Pregunta = model.Criterio.Pregunta;
            criterioViewModel.Criterio.TipoCalificacion = model.Criterio.TipoCalificacion;
            criterioViewModel.Criterio.IdeCriterio = model.Criterio.IdeCriterio;
            

            return View(criterioViewModel);

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
       
    }
}
