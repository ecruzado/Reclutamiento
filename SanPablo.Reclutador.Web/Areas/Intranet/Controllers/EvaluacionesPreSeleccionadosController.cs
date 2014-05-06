
namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{

    using CrystalDecisions.CrystalReports.Engine;
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
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;


    //Itext
    using iTextSharp;
    using iTextSharp.text;
    using iTextSharp.text.pdf;


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
        public ActionResult Index(string id, string idRecluPost, string idSol,string tipSol, string pagina, string ind)
        {
            var modelEvaluaciones = inicializarEvaluacionesPreseleccionados(idSol, tipSol,  pagina,  ind);

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

                var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];

                modelEvaluaciones.usuarioSession = usuarioSession.IDUSUARIO;

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
        public EvaluacionesPreSeleccionadosViewModel inicializarEvaluacionesPreseleccionados(string idSol,string tipSol, string pagina, string ind)
        {
            EvaluacionesPreSeleccionadosViewModel model = new EvaluacionesPreSeleccionadosViewModel();
            model.ReclutaPersona = new ReclutamientoPersona();
            model.PostulantePreSel = new Postulante();
            model.Solicitud = new SolReqPersonal();

            model.pagina = pagina;
            model.Tipsol = tipSol;
            model.IdeSolReqPersonal = Convert.ToInt32(idSol);
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
                                item.DescripcionTipoExamen == null?"":item.DescripcionTipoExamen,
                                item.FechaEvaluacion==null?"":String.Format("{0:dd/MM/yyyy}",item.FechaEvaluacion),
                                item.HoraEvaluacion==null?"":String.Format("{0:hh:mm tt}",item.HoraEvaluacion),
                                item.IdeUsuarioResponsable==0?"":item.IdeUsuarioResponsable.ToString(),
                                item.UsuarioResponsable==null?"":item.UsuarioResponsable,
                                item.TipoEstadoEvaluacion==null?"":item.TipoEstadoEvaluacion,
                                item.EstadoEvaluacion==null?"":item.EstadoEvaluacion,
                                item.IndicadorResultado==null?"N":item.IndicadorResultado,
                                (item.NotaFinal==0)||(item.NotaFinal ==-1)?"":item.NotaFinal.ToString(),
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
                reclutamientoExamenEditar.EsEntrevistaFinal = model.ReclutaPersonaExamen.EsEntrevistaFinal;

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
                    string nombre = reclutamientoExamen.nombreArchivo;
                    int posicion = nombre.IndexOf(".", nombre.Length - 5);
                    int nroCaract = nombre.Length - posicion - 1;
                    string extension = nombre.Substring(posicion+1, nroCaract);
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
                    var reclutaExamenEditar = _reclutamientoPersonaExamenRepository.GetSingle(x => x.IdeReclutamientoPersonaExamen == model.ReclutaPersonaExamen.IdeReclutamientoPersonaExamen);

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
                            //model.ReclutaPersonaExamen.Archivo = buffer;
                            //model.ReclutaPersonaExamen.rutaArchivo = model.ReclutaPersonaExamen.rutaArchivo;
                            reclutaExamenEditar.Archivo = buffer;
                            //reclutaExamenEditar.nombreArchivo = model.ReclutaPersonaExamen.nombreArchivo;

                            var nombreArchivo = model.ReclutaPersonaExamen.nombreArchivo;
                            if (nombreArchivo.Length > 150)
                            {
                                nombreArchivo = nombreArchivo.Substr(nombreArchivo.Length-150,150);
                            }
                            reclutaExamenEditar.nombreArchivo = nombreArchivo;
                        }
                    }

                    

                    if (reclutaExamenEditar.TipoEstadoEvaluacion == EstadoEvaluacion.Programado)
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
                    reclutaExamenEditar.NotaFinal = model.ReclutaPersonaExamen.NotaFinal;
                    
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


        [HttpPost]
        public ActionResult verificarProgramacion(string id)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            int idReclutaPersonaExamen = Convert.ToInt32(id);

            var reclutaPersExamen = _reclutamientoPersonaExamenRepository.GetSingle(x => x.IdeReclutamientoPersonaExamen == idReclutaPersonaExamen);

            //EstadoEvaluacion
            if (reclutaPersExamen.TipoExamen == TipoExamen.Examen)
            {
                objJsonMessage.Mensaje = "No requiere registro de resultado";
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
            else
            {
                if ((reclutaPersExamen.TipoExamen == TipoExamen.Evaluacion) &&
                    ((reclutaPersExamen.TipoEstadoEvaluacion == EstadoEvaluacion.Evaluado)||
                     (reclutaPersExamen.TipoEstadoEvaluacion == EstadoEvaluacion.Aprobado)||
                     (reclutaPersExamen.TipoEstadoEvaluacion == EstadoEvaluacion.Desaprobado)))
                {
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    if (reclutaPersExamen.IdeUsuarioResponsable != 0)
                    {
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "Requiere el registro de programación previa";
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                }
            }
            
        }

        #region MOSTRAR EVALUACIONES RENDIDAS


        public ActionResult ResultadoEvaluacion1(int idRP, int idRE)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            string fullPath = null;
            ReportDocument rep = new ReportDocument();
            MemoryStream mem;

            try
            {

                DataSet ds = _reclutamientoPersonaExamenRepository.ObtenerEvaluacionReporte(idRP, idRE);
                
                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = ConfigurationManager.AppSettings["ReportIntranetPath"];
                string nomReporte = "ExamenPostulanteReport.rpt";
                fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, nomReporte));

                rep.Load(fullPath);
                
                rep.Database.Tables["dtExamen"].SetDataSource(ds.Tables["Table"]);
                rep.Database.Tables["dtCategoriaExamen"].SetDataSource(ds.Tables["Table1"]);
                rep.Database.Tables["dtCategoriaSubCatego"].SetDataSource(ds.Tables["Table2"]);
                rep.Database.Tables["dtCriterioAlternativa"].SetDataSource(ds.Tables["Table3"]);
                rep.Database.Tables["dtAlternativas"].SetDataSource(ds.Tables["Table4"]);

                Response.Clear();
                mem = (MemoryStream)rep.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            }
            catch (Exception)
            {
                return MensajeError();
            }
            return File(mem, "application/pdf");

        }

        [HttpPost]
        public ActionResult existeResultado(int idRE)
        {
            JsonMessage objJsonMessage = new JsonMessage();

            var indicador = _reclutamientoPersonaExamenRepository.existeResultado(idRE);

            if (indicador == Indicador.No)
            {
                objJsonMessage.Resultado = false;
            }
            else
            {
                objJsonMessage.Resultado = true;
            }
           
            return Json(objJsonMessage);
 
        }

        public ActionResult ResultadoEvaluacion(int idRP, int idRE)
        {

            ResultadoExamen resultadoExamen = new ResultadoExamen();
            string NombreExamen = "";
            string DesExamen = "";
            string Nota = "";
            string NombrePostulante = "";
            
            try
            {

                resultadoExamen = _reclutamientoPersonaExamenRepository.ObtenerEvaluacionReportePdf(idRP, idRE);
            
                    // tipo de letra
            Font normal = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
            Font negrita = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
            Font subRayado = new Font(FontFactory.GetFont("Arial", 10, Font.UNDERLINE));

            Font letraTitulo = new Font(FontFactory.GetFont("Arial", 12, Font.BOLD));

            // obtengo los datos de la cabecera
            if (resultadoExamen != null)
            {

                NombreExamen = resultadoExamen.NombreExamen;
                DesExamen = resultadoExamen.DescripcionExamen;
                Nota =  resultadoExamen.NotaFinal.ToString();
                if (Nota == "-1")
                {
                    Nota = "PENDIENTE";
                }
                NombrePostulante = resultadoExamen.nombrePostulante;
            }

            string ConcatenExamen = DesExamen + " - " + NombreExamen; 

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 20, 30, 30, 20);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
               
                //Se inicializa el archivo PDF
                doc.Open();

                
                //se crea la  1 tabla que contiene las celdas
                PdfPTable table1 = new PdfPTable(2);
                
                // se definen los anchos de la tabla
                float[] anchos1 = new float[] { 20.0f, 80.0f };
                table1.SetWidths(anchos1);

                // se crea la celda con el valor que contiene
                string imagepath = Server.MapPath(@"~/Content/images");
                Image img = Image.GetInstance(imagepath + "/logo_sanpablo -prueba.png");
                img.ScalePercent(80f);
                PdfPCell logo = new PdfPCell(img);
                logo.HorizontalAlignment = Element.ALIGN_LEFT;
                // se quitan los bordes de la celda
                logo.Border = Rectangle.NO_BORDER;
                // se agrega la celda a la tabla
                table1.AddCell(logo);


                 PdfContentByte cb = writer.DirectContent;
 
                //insertar cuadro
                //cb.RoundRectangle(50f, 50f, 20f, 20f, 20f);
                //cb.Stroke();
         
                //cb.MoveTo(118f, 520f);
                //cb.LineTo(122f, 520f);
                //cb.Stroke();
                //cb.MoveTo(120f, 518f);
                //cb.LineTo(120f, 522f);
                //cb.Stroke();


                PdfPCell cellTituloReporte = new PdfPCell(new Phrase(" RESULTADO DE EVALUACION ", letraTitulo));
                // se alinea a la derecha
                cellTituloReporte.HorizontalAlignment = Element.ALIGN_CENTER;
                // se quitan los bordes de la celda
                cellTituloReporte.Border = Rectangle.NO_BORDER;
                // se agrega la celda a la tabla
                table1.AddCell(cellTituloReporte);

                //Espacios entre tablas
                table1.SpacingBefore = 20f;
                table1.SpacingAfter = 30f;

                doc.Add(table1);

                
                PdfPTable table2 = new PdfPTable(4);
                float[] anchos2 = new float[] { 20.0f, 65.0f , 10.0f, 5.0f  };
                table2.SetWidths(anchos2);

                PdfPCell cellNombExamen = new PdfPCell(new Phrase("EXAMEN :", negrita));
                // se alinea a la derecha
                cellNombExamen.HorizontalAlignment = Element.ALIGN_LEFT;
                // se quitan los bordes de la celda
                cellNombExamen.Border = Rectangle.NO_BORDER;
                // se agrega la celda a la tabla
                table2.AddCell(cellNombExamen);
               
                PdfPCell cellDesExamen = new PdfPCell(new Phrase(ConcatenExamen, normal));
                cellDesExamen.Border = Rectangle.NO_BORDER;
                cellDesExamen.HorizontalAlignment = Element.ALIGN_LEFT;
                table2.AddCell(cellDesExamen);
                
                
                PdfPCell CellTipoExamen = new PdfPCell(new Phrase("NOTA: ", negrita));
                CellTipoExamen.HorizontalAlignment = Element.ALIGN_LEFT;
                CellTipoExamen.Border = Rectangle.NO_BORDER;
                table2.AddCell(CellTipoExamen);

                PdfPCell CellTipoExamenDes = new PdfPCell(new Phrase(Nota, normal));
                CellTipoExamenDes.HorizontalAlignment = Element.ALIGN_LEFT;
                CellTipoExamenDes.Border = Rectangle.NO_BORDER;
                table2.AddCell(CellTipoExamenDes);


                //Espacios entre tablas
                table2.SpacingBefore = 20f;
                table2.SpacingAfter = 30f;

                PdfPCell CellDuracion = new PdfPCell(new Phrase("POSTULANTE: ", negrita));
                CellDuracion.HorizontalAlignment = Element.ALIGN_LEFT;
                //CellDuracion.Colspan = 2;
                CellDuracion.Border = Rectangle.NO_BORDER;
                table2.AddCell(CellDuracion);

                PdfPCell CellDuracionDes = new PdfPCell(new Phrase(NombrePostulante, normal));
                CellDuracionDes.HorizontalAlignment = Element.ALIGN_LEFT;
                CellDuracionDes.Border = Rectangle.NO_BORDER;
                // CellDuracion.Colspan = 3;
                table2.AddCell(CellDuracionDes);

                PdfPCell CellBlanco = new PdfPCell(new Phrase(""));
                CellBlanco.HorizontalAlignment = Element.ALIGN_LEFT;
                CellBlanco.Border = Rectangle.NO_BORDER;
                // CellDuracion.Colspan = 3;
                table2.AddCell(CellBlanco);

                PdfPCell CellBlanco1 = new PdfPCell(new Phrase(""));
                CellBlanco1.HorizontalAlignment = Element.ALIGN_LEFT;
                CellBlanco1.Border = Rectangle.NO_BORDER;
                // CellDuracion.Colspan = 3;
                table2.AddCell(CellBlanco1);

                //Espacios entre tablas
                table2.SpacingBefore = 15f;
                table2.SpacingAfter = 10f;

                doc.Add(table2);
               
                //codigos unicos

                var listaCategorias = resultadoExamen.Categorias;

                // Agrupacion de categoria
                foreach (ResultadoExamenCategoria itemCategoria in listaCategorias)
                {
                    // se crea la tercera tabla que es una agrupacion por categoria

                    PdfPTable table3 = new PdfPTable(3);
                    float[] ancho3 = new float[] { 40.0f,50.0f,10.0f };
                    table3.SetWidths(ancho3);

                    // celda 1

                    PdfPCell CellTituloCat = new PdfPCell(new Phrase("CATEGORIA : ", negrita));
                    CellTituloCat.HorizontalAlignment = Element.ALIGN_LEFT;
                    CellTituloCat.Border = Rectangle.NO_BORDER;
                    table3.AddCell(CellTituloCat);


                    PdfPCell CelltCategoria = new PdfPCell(new Phrase(itemCategoria.NombreCategoria+"  TIEMPO :"+itemCategoria.Tiempo +"min" , normal));
                    CelltCategoria.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelltCategoria.Border = Rectangle.NO_BORDER;
                    CelltCategoria.Colspan = 2;
                    table3.AddCell(CelltCategoria);
                    
                    //celda 2 de instrucciones
                    PdfPCell CelltituloInstruccion = new PdfPCell(new Phrase("NOTA CATEGORIA:",negrita));
                    CelltituloInstruccion.HorizontalAlignment = Element.ALIGN_LEFT;
                    //CelltituloInstruccion.Colspan = 3;
                    CelltituloInstruccion.Border = Rectangle.NO_BORDER;
                    table3.AddCell(CelltituloInstruccion);

                    ////celda enblanco
                    //PdfPCell CellBlanco2 = new PdfPCell(new Phrase("", normal));
                    //CellBlanco2.Colspan = 3;
                    //CellBlanco2.Border = Rectangle.NO_BORDER;
                    //table3.AddCell(CellBlanco2);

                    // celda 3
                    PdfPCell CellInstruccion = new PdfPCell(new Phrase(itemCategoria.NotaCategoria.ToString(), normal));
                    CellInstruccion.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    CellInstruccion.Border = Rectangle.NO_BORDER;
                    // se le indica que ocupe las 3 columnas
                    //CellInstruccion.Colspan = 2;
                    table3.AddCell(CellInstruccion);

                    table3.SpacingBefore = 10f;
                    table3.SpacingAfter = 10f;

                    doc.Add(table3);

                    #region subcategoria

                    // se obtiene los codigos de la subcategoria unicos por categoria 
                    // la lista listaCategoria contiene solo los registros de dicha categoria
                    
                    //SUBCATEGORIA
                    var listSubCategoria = resultadoExamen.SubCategorias.Where(x => x.IdeReclutamientoExamenCategoria == itemCategoria.IdeReclutamientoExamenCategoria);

                    foreach (ResultadoExamenSubCategoria itemSubCategoria in listSubCategoria)
                    {

                        // se crea la cuarta tabla
                        PdfPTable table4 = new PdfPTable(3);
                        float[] ancho4 = new float[] { 30.0f, 60.0f, 10.0f };
                        table4.SetWidths(ancho4);

                        // se agregan las celdas a la tabla
                        PdfPCell CellSubcAtegoria = new PdfPCell(new Phrase("SUBCATEGORIA : ", negrita));
                        CellSubcAtegoria.HorizontalAlignment = Element.ALIGN_LEFT;
                        //CellSubcAtegoria.Colspan = 3;
                        CellSubcAtegoria.Border = Rectangle.NO_BORDER;
                        table4.AddCell(CellSubcAtegoria);

                        // celda nombre la subcategoria
                        PdfPCell CellNombSubcAtegoria = new PdfPCell(new Phrase(itemSubCategoria.NombreSubCategoria, normal));
                        CellNombSubcAtegoria.HorizontalAlignment = Element.ALIGN_LEFT;
                        CellNombSubcAtegoria.Colspan = 2;
                        CellNombSubcAtegoria.Border = Rectangle.NO_BORDER;
                        table4.SpacingBefore = 10f;
                        table4.SpacingAfter = 10f;
                        table4.AddCell(CellNombSubcAtegoria);
                        doc.Add(table4);


                        #region criterios
                        //CRITERIO
                        int cont = 1;
                        var listaCodCriterio = resultadoExamen.Criterios.Where(x => x.IdeSubCategoria == itemSubCategoria.IdeSubCategoria);
                        foreach (var itemCriterio in listaCodCriterio)
                        {
                             
                            PdfPTable table5 = new PdfPTable(3);
                            float[] ancho5 = new float[] { 15.0f, 80.0f, 10.0f };
                            table5.SetWidths(ancho5);
                            PdfPCell CellPregunta = new PdfPCell(new Phrase("NRO. "+cont, negrita));
                            //CellPregunta.Colspan = 3;
                            CellPregunta.Border = Rectangle.NO_BORDER;
                            
                            CellPregunta.SpaceCharRatio = 10f; //espacio de celda
                            
                            table5.AddCell(CellPregunta);
                            

                            // si el codMod es 02 muestra imagen si no muestra texto

                            PdfPCell CellPreguntaCriterio = null;
                            if ("02".Equals(itemCriterio.TipoModo))
                            {
                                if (itemCriterio.ImagenCriterio!=null)
                                {
                                    Image PreguntaCriterio = Image.GetInstance(itemCriterio.ImagenCriterio);
                                    PreguntaCriterio.ScaleToFit(160f, 160f);

                                    CellPreguntaCriterio = new PdfPCell(PreguntaCriterio);
                                    CellPreguntaCriterio.HorizontalAlignment = Element.ALIGN_LEFT;
                                    
                                }
                                else
                                {
                                    CellPreguntaCriterio = new PdfPCell(new Phrase("", normal));
                                }
                            }
                            else
                            {
                                CellPreguntaCriterio = new PdfPCell(new Phrase(itemCriterio.Pregunta, normal));
                            }

                            cont = cont + 1;
                            CellPreguntaCriterio.Colspan = 2;
                            CellPreguntaCriterio.Border = Rectangle.NO_BORDER;
                            table5.AddCell(CellPreguntaCriterio);
                            table5.SpacingBefore = 2f;
                            table5.SpacingAfter = 2f;

                            doc.Add(table5);

                            #region Alternativas
                            // ALTERNATIVAS
                            int nContAltenativa = 1;
                            string opcion = "";
                            var listaCodAlternativa = resultadoExamen.Alternativas.Where(x => x.IdeReclutaPersonaCriterio == itemCriterio.IdeReclutaPersonaCriterio);
                           
                            foreach (var itemAlternativa in listaCodAlternativa)
                            {

                                
                                //ToLetras
                                opcion = ToLetras(nContAltenativa);

                                PdfPTable table6 = new PdfPTable(3);
                                float[] ancho6 = new float[] { 10.0f, 80.0f, 10.0f };
                                table6.SetWidths(ancho5);
                                
                                PdfPCell CellAlternativa = new PdfPCell(new Phrase(opcion+")", negrita));
                                CellAlternativa.HorizontalAlignment = Element.ALIGN_LEFT;
                                CellAlternativa.Colspan = 1;
                                CellAlternativa.Border = Rectangle.NO_BORDER;
                                table6.AddCell(CellAlternativa);


                                PdfPCell CellOpcionAlternativa = null;

                                if ("02".Equals(itemCriterio.TipoModo))
                                {
                                    if (itemAlternativa.ImagenAlternativa != null)
                                    {
                                        Image OpcionImage = Image.GetInstance(itemAlternativa.ImagenAlternativa);
                                        OpcionImage.ScaleToFit(160f, 160f);

                                        CellOpcionAlternativa = new PdfPCell(OpcionImage);
                                       

                                    }
                                    else
                                    {
                                        CellOpcionAlternativa = new PdfPCell(new Phrase("", normal));
                                    }
                                }
                                else
                                {
                                    if (itemAlternativa.Respuesta == "S")
                                    {
                                        CellOpcionAlternativa = new PdfPCell(new Phrase(itemAlternativa.Alternativa, subRayado));
                                    }
                                    else
                                    {
                                        CellOpcionAlternativa = new PdfPCell(new Phrase(itemAlternativa.Alternativa, normal));
                                    }
                                    
                                }

                                CellOpcionAlternativa.Colspan = 2;
                                CellOpcionAlternativa.Border = Rectangle.NO_BORDER;
                                CellOpcionAlternativa.HorizontalAlignment = Element.ALIGN_LEFT;

                                
                                table6.AddCell(CellOpcionAlternativa);

                                table6.SpacingBefore = 2f;
                                table6.SpacingAfter = 2f;
                                doc.Add(table6);

                                nContAltenativa = nContAltenativa + 1;

                            }

                            #endregion

                        }

                        #endregion

                    }

                    #endregion

                }

                // Se cierra el archivo Pdf
                doc.Close();


                writer.Close();
               
                return File(ms.ToArray(), "application/pdf");   
            }
            }
            catch (Exception)
            {
                
                    return MensajeError();
            }
        
        }



        /// <summary>
        /// convierte el numero a letras
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public String ToLetras(int n)
        {
            string res = "";

            if (n == 0)
            {

                return res;
            }
            else
            {

                switch (n)
                {
                    case 1:
                        res = " A";
                        break;
                    case 2:
                        res = " B";
                        break;
                    case 3:
                        res = " C";
                        break;
                    case 4:
                        res = " D";
                        break;
                    case 5:
                        res = " E";
                        break;
                    case 6:
                        res = " F";
                        break;
                    case 7:
                        res = " G";
                        break;
                    case 8:
                        res = " H";
                        break;
                    case 9:
                        res = " I";
                        break;
                    case 10:
                        res = " J";
                        break;
                    case 11:
                        res = " K";
                        break;
                    case 12:
                        res = " L";
                        break;
                    case 13:
                        res = " M";
                        break;
                    case 14:
                        res = " N";
                        break;
                    case 15:
                        res = " O";
                        break;
                    case 16:
                        res = " P";
                        break;
                    case 17:
                        res = " Q";
                        break;
                    case 18:
                        res = " R";
                        break;
                    case 19:
                        res = " S";
                        break;
                    case 20:
                        res = " T";
                        break;
                    case 21:
                        res = " U";
                        break;
                    case 22:
                        res = " V";
                        break;
                    case 23:
                        res = " W";
                        break;
                    case 24:
                        res = " X";
                        break;
                    case 25:
                        res = " Y";
                        break;
                    case 26:
                        res = " Z";
                        break;
    

                }
                return res;
            }
        }


    
        #endregion
    }
}
