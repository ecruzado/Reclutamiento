
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
    using System.Web.Security;
    
    public class EvaluacionesPreSeleccionadosController : BaseController
    {
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICvPostulanteRepository _cvPostulanteRepository;
        private IReclutamientoPersonaRepository _reclutamientoPersonaRepository;
        private IPostulanteRepository _postulanteRepository;
        private IReclutamientoPersonaExamenRepository _reclutamientoPersonaExamenRepository;



        public EvaluacionesPreSeleccionadosController(IDetalleGeneralRepository detalleGeneralRepository,
                                                      ISolReqPersonalRepository solReqPersonalRepository,
                                                      ICvPostulanteRepository cvPostulanteRepository,
                                                      IReclutamientoPersonaRepository reclutamientoPersonaRepository,
                                                      IPostulanteRepository postulanteRepository,
                                                      IReclutamientoPersonaExamenRepository reclutamientoPersonaExamenRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _cvPostulanteRepository = cvPostulanteRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _postulanteRepository = postulanteRepository;
            _reclutamientoPersonaExamenRepository = reclutamientoPersonaExamenRepository;
           
        }



        /// <summary>
        /// inicializa las evaluaciones
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Index(string id, string idRecluPost)
        {
            var modelEvaluaciones = inicializarEvaluacionesPreseleccionados();

            if ((id != null) && (idRecluPost != null))
            {
                IdePostulantePreSeleccion = Convert.ToInt32(id);
                IdeReclutaPersona = Convert.ToInt32(idRecluPost);
            }
            if (IdePostulantePreSeleccion != 0)
            {
                var reclutaPersona = _reclutamientoPersonaRepository.GetSingle(x => x.IdeReclutaPersona == IdeReclutaPersona);
                var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == reclutaPersona.IdePostulante);

                modelEvaluaciones.ReclutaPersona.IdeReclutaPersona = IdeReclutaPersona;
                modelEvaluaciones.Solicitud.IdeSolReqPersonal = reclutaPersona.IdeSol;
                modelEvaluaciones.Solicitud.Tipsol = reclutaPersona.TipSol;
                modelEvaluaciones.PostulantePreSel = postulante;


                List<SolReqPersonal> listaSolicitud = _solReqPersonalRepository.GetDatosSol(modelEvaluaciones.Solicitud);

                if (listaSolicitud != null && listaSolicitud.Count > 0)
                {
                    modelEvaluaciones.Solicitud = (SolReqPersonal)listaSolicitud[0];
                }
            }

            return View("EvaluacionesPreSeleccionados", modelEvaluaciones);
        }

        /// <summary>
        /// Inicializar el model de la evaluacion de preseleccionados
        /// </summary>
        /// <returns></returns>
        public EvaluacionesPreSeleccionadosViewModel inicializarEvaluacionesPreseleccionados()
        {
            EvaluacionesPreSeleccionadosViewModel model = new EvaluacionesPreSeleccionadosViewModel();
            model.ReclutaPersona = new ReclutamientoPersona();
            model.PostulantePreSel = new Postulante();
            model.Solicitud = new SolReqPersonal();


            return model;
        }


        [HttpPost]
        public ActionResult ListEvaluacionesPost(GridTable grid)
        {

            List<ReclutamientoPersonaExamen> lista = new List<ReclutamientoPersonaExamen>();
            try
            {

                var ideRecluPersona = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
                var idePostulante = (grid.rules[1].data == null ? 0 : Convert.ToInt32(grid.rules[1].data));

                lista = _reclutamientoPersonaExamenRepository.obtenerEvaluacionesPostulante(idePostulante,ideRecluPersona,Session[ConstanteSesion.UsuarioDes].ToString());

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = (item.IdeReclutamientoPersonaExamen.ToString()),
                    cell = new string[]
                            {
                                item.IdeReclutamientoPersonaExamen==0?"":item.IdeReclutamientoPersonaExamen.ToString(),
                                item.IdeReclutamientoPersona==0?"":item.IdeReclutamientoPersona.ToString(),
                                item.IdeEvaluacion==0?"":item.IdeEvaluacion.ToString(),
                                item.DescripcionExamen==null?"":item.DescripcionExamen,
                                item.TipoExamen==null?"":item.TipoExamen,
                                item.FechaEvaluacion==null?"":String.Format("{0:dd/MM/yyyy}",item.FechaEvaluacion),
                                item.HoraEvaluacion==null?"":String.Format("{0:hh:mm tt}",item.HoraEvaluacion),
                                item.IdeUsuarioResponsable==0?"":item.IdeReclutamientoPersona.ToString(),
                                item.UsuarioResponsable==null?"":item.UsuarioResponsable,
                                item.TipoEstadoEvaluacion==null?"":item.TipoEstadoEvaluacion,
                                item.EstadoEvaluacion==null?"":item.EstadoEvaluacion,
                                item.NotaFinal==0?"":item.NotaFinal.ToString(),
                                item.ComentarioResultado==null?"":item.ComentarioResultado
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
        public ActionResult ProgramarEvaluacion(string id, string responsable)
        {
            EvaluacionesPreSeleccionadosViewModel modelEvaluacion = iniciarProgramarEvaluacion();
            int idReclutamientoEvaluacion = Convert.ToInt32(id);

            var reclutamientoEvaluacionExamen = _reclutamientoPersonaExamenRepository.GetSingle(x => x.IdeReclutamientoPersonaExamen == idReclutamientoEvaluacion);
            modelEvaluacion.ReclutaPersonaExamen = reclutamientoEvaluacionExamen;

            modelEvaluacion.nombreUsuario = responsable;
            
            return View("ProgramarEvaluacion",modelEvaluacion);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult ProgramarEvaluacion(EvaluacionesPreSeleccionadosViewModel model)
        {
            EvaluacionesPreSeleccionadosViewModel modelEvaluacion = iniciarProgramarEvaluacion();

            ReclutamientoPersonaExamenValidator validator = new ReclutamientoPersonaExamenValidator();
            ValidationResult result = validator.Validate(model.ReclutaPersonaExamen, "FechaEvaluacion", "HoraEvaluacion", "Observacion", "IdeUsuarioResponsable");

            if (!result.IsValid)
            {
                modelEvaluacion = iniciarProgramarEvaluacion();
                modelEvaluacion.ReclutaPersonaExamen = model.ReclutaPersonaExamen;
                return View("ProgramarEvaluacion", modelEvaluacion);
            }
            else
            {
                var reclutamientoExamenEditar = _reclutamientoPersonaExamenRepository.GetSingle(x => x.IdeReclutamientoPersonaExamen == model.ReclutaPersonaExamen.IdeReclutamientoPersonaExamen);

                reclutamientoExamenEditar.FechaEvaluacion = model.ReclutaPersonaExamen.FechaEvaluacion;
                reclutamientoExamenEditar.HoraEvaluacion = model.ReclutaPersonaExamen.HoraEvaluacion;
                reclutamientoExamenEditar.IdeUsuarioResponsable = model.ReclutaPersonaExamen.IdeUsuarioResponsable;
                reclutamientoExamenEditar.Observacion = model.ReclutaPersonaExamen.Observacion;
                reclutamientoExamenEditar.FechaModificacion = FechaModificacion;
                reclutamientoExamenEditar.TipoEstadoEvaluacion = EstadoEvaluacion.Programado;
                reclutamientoExamenEditar.UsuarioModificacion = Session[ConstanteSesion.UsuarioDes].ToString();

                _reclutamientoPersonaExamenRepository.Update(reclutamientoExamenEditar);

                return RedirectToAction("../EvaluacionesPreSeleccionados/Index");
            }
        }



        public EvaluacionesPreSeleccionadosViewModel iniciarProgramarEvaluacion()
        {
            EvaluacionesPreSeleccionadosViewModel model = new EvaluacionesPreSeleccionadosViewModel();
            model.ReclutaPersonaExamen = new ReclutamientoPersonaExamen();

            return model;
        }

        [ValidarSesion]
        public ActionResult PopupResultado(string id)
        {

            var modelResultado = iniciarPopupResultado();

            int idRecluPersoExamen = Convert.ToInt32(id);
            if (idRecluPersoExamen != 0)
            {
                var reclutamientoExamen = _reclutamientoPersonaExamenRepository.GetSingle(x => x.IdeReclutamientoPersonaExamen == idRecluPersoExamen);
                if (reclutamientoExamen.nombreArchivo != null)
                {
                    var nombre = reclutamientoExamen.nombreArchivo;
                    int posicion = nombre.IndexOf(".", nombre.Length - 5);
                    int nroCaract = nombre.Length - posicion - 1;
                    var extension = nombre.Substr(posicion, nroCaract);
                    modelResultado.tipoArchivo = extension;

                }
                modelResultado.ReclutaPersonaExamen = reclutamientoExamen;
            }

            return View("PopupResultado", modelResultado);
 
        }

        [HttpPost]
        public ActionResult PopupResultado(EvaluacionesPreSeleccionadosViewModel model)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var modelResultado = iniciarPopupResultado();
            string fullPath = null;
            try
            {
                ReclutamientoPersonaExamenValidator validator = new ReclutamientoPersonaExamenValidator();
                ValidationResult result = validator.Validate(model.ReclutaPersonaExamen, "ComentarioResultado", "TipoEstadoEvaluacion");

                if (!ModelState.IsValid)
                {
                    modelResultado.ReclutaPersonaExamen = model.ReclutaPersonaExamen;
                    return View("PopupResultado", modelResultado);
                }
                else
                {
                    
                    if (!string.IsNullOrEmpty(model.nombreTemporalArchivo))
                    {

                        string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                        string directoryPath = "Archivos\\Imagenes\\";
                        fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, model.nombreTemporalArchivo));

                        using (Stream s = System.IO.File.OpenRead(fullPath))
                        {
                            byte[] buffer = new byte[s.Length];
                            s.Read(buffer, 0, (int)s.Length);
                            int len = (int)s.Length;
                            s.Close();
                            model.ReclutaPersonaExamen.Archivo = buffer;
                            model.ReclutaPersonaExamen.rutaArchivo = model.ReclutaPersonaExamen.rutaArchivo;

                            var nombreArchivo = model.ReclutaPersonaExamen.rutaArchivo;
                            if (nombreArchivo.Length > 150)
                            {
                                nombreArchivo = nombreArchivo.Substr(nombreArchivo.Length-150,150);
                            }
                            model.ReclutaPersonaExamen.nombreArchivo = nombreArchivo;
                        }
                    }

                    var reclutaExamenEditar = _reclutamientoPersonaExamenRepository.GetSingle(x => x.IdeReclutamientoPersonaExamen == model.ReclutaPersonaExamen.IdeReclutamientoPersonaExamen);

                    if (reclutaExamenEditar.TipoEstadoEvaluacion == EstadoEvaluacion.Pendiente)
                    {
                        var reclutamientoPersona = _reclutamientoPersonaRepository.GetSingle(x => x.IdeReclutaPersona == reclutaExamenEditar.IdeReclutamientoPersona);
                        reclutamientoPersona.Evaluacion = reclutamientoPersona.Evaluacion + 1;
                        _reclutamientoPersonaRepository.Update(reclutamientoPersona);
                    }

                    if (model.ReclutaPersonaExamen.TipoEstadoEvaluacion == EstadoEvaluacion.Aprobado)
                    {
                        model.ReclutaPersonaExamen.NotaFinal = NotaEvaluacion.Aprobado;
                    }
                    else
                    {
                        model.ReclutaPersonaExamen.NotaFinal = NotaEvaluacion.Desaprobado;
                    }

                    reclutaExamenEditar.ComentarioResultado = model.ReclutaPersonaExamen.ComentarioResultado;
                    reclutaExamenEditar.TipoEstadoEvaluacion = model.ReclutaPersonaExamen.TipoEstadoEvaluacion;
                    reclutaExamenEditar.FechaModificacion = FechaModificacion;
                    reclutaExamenEditar.Archivo = model.ReclutaPersonaExamen.Archivo;
                    reclutaExamenEditar.NotaFinal = model.ReclutaPersonaExamen.NotaFinal;
                    reclutaExamenEditar.nombreArchivo = model.ReclutaPersonaExamen.nombreArchivo;
                    reclutaExamenEditar.UsuarioModificacion = Session[ConstanteSesion.UsuarioDes].ToString();

                    

                    _reclutamientoPersonaExamenRepository.Update(reclutaExamenEditar);

                    if (fullPath != null)
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
            }
            catch(Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }

        }


        public EvaluacionesPreSeleccionadosViewModel iniciarPopupResultado()
        {
            EvaluacionesPreSeleccionadosViewModel model = new EvaluacionesPreSeleccionadosViewModel();
            model.ReclutaPersonaExamen = new ReclutamientoPersonaExamen();

            model.ListaAprobadoDesaprobado = new List<DetalleGeneral>(_detalleGeneralRepository.GetBy(x => x.IdeGeneral == Convert.ToInt32(TipoTabla.EstadoEvaluacion) && x.Valor != EstadoEvaluacion.Pendiente 
                                                                                                      && x.Valor != EstadoEvaluacion.Evaluado && x.Valor != EstadoEvaluacion.Programado));
            
            return model;
        }

        
        public ActionResult GetImage(int id)
        {
            var firstOrDefault = _reclutamientoPersonaExamenRepository.GetSingle(x =>x.IdeReclutamientoPersonaExamen == id);
            if (firstOrDefault.Archivo != null)
            {
                byte[] image = firstOrDefault.Archivo;
                return File(image, "image/jpg/pdf");
            }
            else
            {
                return null;
            }
        }


        public ActionResult GetPdf(string id)
        {
            int idReclutaExamen = Convert.ToInt32(id);
           byte[] content = null;
		    try
		    {
                var datos = _reclutamientoPersonaExamenRepository.GetSingle(x => x.IdeReclutamientoPersonaExamen == idReclutaExamen);
                content = datos.Archivo;

                Response.Clear();
                MemoryStream ms = new MemoryStream(content);
                return File(ms, "application/pdf");

		    }
		    catch( Exception ex )
		    {
			    throw new Exception("error Reading data from blog Types Table", ex);
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

    }
}
