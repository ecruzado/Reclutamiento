using SanPablo.Reclutador.Repository.Interface;
using System;
using System.IO;
using System.Collections.Generic;
using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Entity.Validation;
using SanPablo.Reclutador.Web.Areas.Intranet.Models;
using SanPablo.Reclutador.Web.Core;
using SanPablo.Reclutador.Web.Models.JQGrid;
using FluentValidation.Results;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SanPablo.Reclutador.Web.Models;
using NHibernate.Criterion;
using FluentValidation.Results;
using FluentValidation;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    public class CriterioController : BaseController
    {
        private ICriterioRepository _criterioRepository;
        private IAlternativaRepository _AlternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public CriterioController(ICriterioRepository criterioRepository, IDetalleGeneralRepository detalleGeneralRepository, IAlternativaRepository alternativaRepository)
        {
            _criterioRepository = criterioRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _AlternativaRepository = alternativaRepository;
        }

        public CriterioViewModel inicializarCriteriosIndex()
        {
            var criterioViewModel = new CriterioViewModel();
            criterioViewModel.Criterio = new Criterio();

            criterioViewModel.TipoCriterio = 
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("TIPCRITERIO"));
            criterioViewModel.TipoCriterio.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            criterioViewModel.Medicion =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("MEDICION"));
            criterioViewModel.Medicion.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            criterioViewModel.Estado =
               new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("ESTADOCRIT"));
            criterioViewModel.Estado.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

         
            return criterioViewModel;
        }

        public CriterioViewModel inicializarCriteriosEdit()
        {
            var criterioViewModel = new CriterioViewModel();
            criterioViewModel.Criterio = new Criterio();
            
            criterioViewModel.TipoCriterio =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("TIPCRITERIO"));
            criterioViewModel.TipoCriterio.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            criterioViewModel.Medicion =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("MEDICION"));
            criterioViewModel.Medicion.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            criterioViewModel.TipoModo =
               new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("MODO"));
            criterioViewModel.TipoModo.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            criterioViewModel.TipoCalificacion =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("TIPCALIFICA"));
            criterioViewModel.TipoCalificacion.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            return criterioViewModel;
        }


        //
        [HttpPost]
        public ActionResult PopupCriterio([Bind(Prefix = "Alternativa")]Alternativa alternativa)
        {
            var criterioViewModel = new CriterioViewModel();
            criterioViewModel.Alternativa = new Alternativa();

            AlternativaValidator validator = new AlternativaValidator();
            ValidationResult result = validator.Validate(alternativa, "NombreAlternativa", "Peso");

            if (!result.IsValid)
            {
                return View(criterioViewModel);
            }

            if (alternativa.CodigoAlternativa != 0 && alternativa.CodigoAlternativa != null)
            {
                //_AlternativaRepository.Update(alternativa);
                var alter = _AlternativaRepository.GetSingle(x => x.CodigoAlternativa == alternativa.CodigoAlternativa);
                alter.NombreAlternativa = alternativa.NombreAlternativa;
                alter.Peso = alternativa.Peso;
                _AlternativaRepository.Update(alter);
            }
            else
            {
                _AlternativaRepository.Add(alternativa);
            }
           
            criterioViewModel.Alternativa.IdCriterio = alternativa.Criterio.IdeCriterio;
            criterioViewModel.Alternativa.CodigoAlternativa = alternativa.CodigoAlternativa;
            criterioViewModel.Alternativa.NombreAlternativa = alternativa.NombreAlternativa;
            criterioViewModel.Alternativa.Peso = alternativa.Peso;

            return View(criterioViewModel);
        }
        //

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


        public ActionResult Index()
        {

            var criterioViewModel = inicializarCriteriosIndex();

            return View(criterioViewModel);
        }



        public ActionResult Edit()
        {

            var criterioViewModel = inicializarCriteriosEdit();
            return View(criterioViewModel);
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
                // levanta un nuevo modelo
                return View(modelo);
            }
            else 
            {
                
                modelo.Alternativa.Criterio.IdeCriterio = IdCriterio;
                modelo.Alternativa.CodigoAlternativa = id;
                //obtener los datos de la alternativa seleccionada
                modelo.Alternativa = _AlternativaRepository.GetSingle(x => x.CodigoAlternativa == id);

                return View(modelo);

            }
            
        }
             

       /* [HttpPost]
        public ActionResult ListaCriterio(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[11]
        {
          "100001",
          "100001",
          "¿Cuál es la capital del Perú?",
          "Desempeño",
          "Examen",
          "Texto",
          "Manual",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[11]
        {
          "100002",
          "100002",
          "¿Cómo se llama el Presidente del Perú?",
          "Desempeño",
          "Examen",
          "Texto",
          "Manual",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin"
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[11]
        {
          "100003",
          "100003",
          "",
          "Desempeño",
          "Examen",
          "Imagen",
          "Automática",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin"        
        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }
        */
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


                var generic = Listar(_AlternativaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.CodigoAlternativa.ToString(),
                        cell = new string[]
                            {
                                "1",
                                item.CodigoAlternativa.ToString(),
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
            var criterioViewModel = inicializarCriteriosEdit();
           

            if (!ModelState.IsValid){
                criterioViewModel.Criterio = model.Criterio;
                return View(criterioViewModel);
            }

            
           // string result = new StreamReader(file.InputStream).ReadToEnd();
            
            _criterioRepository.Add(model.Criterio);

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
            var criterioViewModel = inicializarCriteriosIndex();

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
        public ActionResult ListaCriterio(GridTable grid)
        {
            try
            {
                CriterioViewModel model = new CriterioViewModel();
                model.Criterio = new Criterio();
                
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                //obtiene el valor del criterio


                // int idCriterio = Convert.ToInt32(grid.rules[0].data);

                DetachedCriteria where = DetachedCriteria.For<Alternativa>();
                where.Add(Expression.Eq("Criterio.IdeCriterio", model.Criterio.IdeCriterio));


                var generic = Listar(_AlternativaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.CodigoAlternativa.ToString(),
                        cell = new string[]
                            {
                                "1",
                                item.CodigoAlternativa.ToString(),
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
        public ActionResult Eliminar(string codigoAlternativa, string codigoCriterio)
        {

            //int nCodAlternativa = codigoAlternativa;
            
            CriterioViewModel model = new CriterioViewModel();
            model.Alternativa = new Alternativa();
            model.Alternativa.Criterio = new Criterio();

            model.Alternativa.Criterio.IdeCriterio = Convert.ToInt32(codigoCriterio);
            model.Alternativa.CodigoAlternativa = Convert.ToInt32(codigoAlternativa);


            var alter = _AlternativaRepository.GetSingle(x => x.CodigoAlternativa == Convert.ToInt32(codigoAlternativa));
            //alter.Criterio.IdeCriterio = model.Alternativa.Criterio.IdeCriterio;
            _AlternativaRepository.Remove(alter);
            return null;            
        }
    }
}
