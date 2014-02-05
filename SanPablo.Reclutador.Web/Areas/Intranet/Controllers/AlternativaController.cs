namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using FluentValidation;
    using FluentValidation.Results;
    using Newtonsoft.Json;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Core;
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using SanPablo.Reclutador.Web.Models.JQGrid;

   

    public class AlternativaController : BaseController
    {
        private IAlternativaRepository _alternativaRepository;

        public AlternativaController(IAlternativaRepository alternativaRepository)
        {
            _alternativaRepository = alternativaRepository;
        }

        /// <summary>
        /// Inicializa el los datos del Popup Criterio para mostrar las alternativas
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codCriterio"></param>
        /// <returns></returns>
        public ActionResult Editar(int ideCriterio, int ideAlternativa, string tipo)
        {
            AlternativaViewModel modelo = new AlternativaViewModel();
           
            modelo.Alternativa = new Alternativa();
            modelo.Alternativa.Criterio = new Criterio();
            modelo.tipoModel = tipo;

            
            if (ideAlternativa == 0)
            {

                modelo.Alternativa.Criterio.IdeCriterio = ideCriterio;
                return View("Edit",modelo);
            }
            else
            {

                modelo.Alternativa.Criterio.IdeCriterio = ideCriterio;
                modelo.Alternativa.IdeAlternativa = ideAlternativa;
                modelo.Alternativa = _alternativaRepository.GetSingle(x => x.IdeAlternativa == ideAlternativa);
               
                return View("Edit",modelo);

            }
        }

        /// <summary>
        /// PopupCriterio: obtiene los datos del Popup y los guarda en la DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="imagenAlternativa"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Editar(AlternativaViewModel model)
        {

            DateTime Hoy = DateTime.Today;

            AlternativaValidator validator = new AlternativaValidator();
            ValidationResult result = validator.Validate(model.Alternativa, "NombreAlternativa", "Peso");
            JsonMessage objJsonMensage = new JsonMessage();
            string fullPath = null;


            if (!result.IsValid)
            {
                return Json(objJsonMensage);
            }


            if ("02".Equals(model.tipoModel))
            {

                if (model.Alternativa.RutaDeImagen == null)
                {
                    objJsonMensage.Mensaje = "Ingrese una imagen";
                    objJsonMensage.Resultado = false;
                    return Json(objJsonMensage);     
                }
               
            }


            if (!string.IsNullOrEmpty(model.NombImagenAlternativa))
            {
                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = "Archivos\\Imagenes\\";
                fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, model.NombImagenAlternativa));

                using (Stream s = System.IO.File.OpenRead(fullPath))
                {
                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, (int)s.Length);
                    int len = (int)s.Length;
                    s.Close();
                    model.Alternativa.Image = buffer;
                }
            }

            if (model.Alternativa.IdeAlternativa != 0)
            {

                var alter = _alternativaRepository.GetSingle(x => x.IdeAlternativa == model.Alternativa.IdeAlternativa);
                alter.NombreAlternativa = model.Alternativa.NombreAlternativa;
                alter.Peso = model.Alternativa.Peso;
                model.Alternativa.FechaModificacion = Hoy;
                model.Alternativa.UsuarioModificacion = "Prueba 02";
                if (model.Alternativa.Image != null)
                {
                    alter.Image = model.Alternativa.Image;
                }

                _alternativaRepository.Update(alter);
                objJsonMensage.Resultado = true;
                objJsonMensage.Mensaje = "Se actualizo el registro correctamente";
            }
            else
            {
                model.Alternativa.FechaCreacion = Hoy;
                model.Alternativa.UsuarioCreacion = "Prueba 01";

                _alternativaRepository.Add(model.Alternativa);
                objJsonMensage.Resultado = true;
                objJsonMensage.Mensaje = "Se registro el registro correctamente";
            }

            if (fullPath!=null)
            {
                System.IO.File.Delete(fullPath);    
            }
            

                return Json(objJsonMensage);
            
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
                    string fullPath = Path.Combine(applicationPath, string.Format("{0}{1}{2}",directoryPath,nombreTemporalArchivo,extensionArchivo));

                    System.IO.File.WriteAllBytes(fullPath, content);

                   
                  
                    jsonResponse.Data = new { 
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
        /// GetImagePopup Muestra la Imagen de la alternativa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetImageAlternativa(int id)
        {
            var firstOrDefault = _alternativaRepository.GetSingle(c => c.IdeAlternativa == id);
            if (firstOrDefault.Image != null)
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
