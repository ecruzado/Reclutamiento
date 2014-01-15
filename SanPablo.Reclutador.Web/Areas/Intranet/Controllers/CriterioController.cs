using SanPablo.Reclutador.Repository.Interface;
using System;
using System.IO;
using System.Collections.Generic;
using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Web.Areas.Intranet.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    public class CriterioController : Controller
    {
        private ICriterioRepository _criterioRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public CriterioController(ICriterioRepository criterioRepository, IDetalleGeneralRepository detalleGeneralRepository)
        {
            _criterioRepository = criterioRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
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


            return criterioViewModel;
        }


        //
        [HttpPost]
        public JsonResult PopupCriterio(CriterioViewModel model)
        {
            //CriterioViewModel objCriterioViewModel = new CriterioViewModel();
            String nombreAlternativa=null;
            int pesoAlternativa=0; 
            
            nombreAlternativa = model.Alternativa.NombreAlternativa;
            pesoAlternativa = model.Alternativa.Peso;

           
            ////_estudioPostulanteRepository.Add(estudioPostulante);
            //return Json("ddafd", JsonRequestBehavior.DenyGet);
            return null;
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

        public ViewResult PopupCriterio(int id)
        {
            if (id == 0)
            {
                CriterioViewModel modelo = new CriterioViewModel();
                // levanta un nuevo modelo
                
                return View(modelo);
            }
            else 
            {
                CriterioViewModel modelo = new CriterioViewModel();
                modelo.Criterio = new Criterio();//conexion bd
                //obtener los datos de la alternativa seleccionada

                return View(modelo);

            }
            
        }
             

        [HttpPost]
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

        [HttpPost]
        public ActionResult ListaAlternativaxCriterio(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "1",
          "Arequipa",
          "0",
          ""
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "2",
          "Cuzco",
          "0",
          ""
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[]
        {
          "3",
          "Lima",
          "5",
          ""
        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType2_4 = new
            {
                id = 4,
                cell = new string[]
        {
          "4",
          "La Paz",
          "0",
          ""
        }
            };
            list.Add((object)fAnonymousType2_4);
            var fAnonymousType2_5 = new
            {
                id = 5,
                cell = new string[]
        {
          "5",
          "Bogotá",
          "0",
          ""
        }
            };
            list.Add((object)fAnonymousType2_5);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult Edit(CriterioViewModel model, HttpPostedFileBase file)
        {
            return null;
        }
    }
}
