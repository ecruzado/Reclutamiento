

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
    using System.Data;
    using System.Configuration;
    using CrystalDecisions.Shared;
    using CrystalDecisions.CrystalReports.Engine;
    using CrystalDecisions.CrystalReports;
    using CrystalDecisions.Web;

     //Itext
    using iTextSharp;
    using iTextSharp.text;
    using iTextSharp.text.pdf;



    [Authorize]
    public class ExamenController : BaseController
    {
        private ICategoriaRepository _categoriaRepository;
        private IAlternativaRepository _alternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISubcategoriaRepository _subcategoriaRepository;
        private ICriterioRepository _criterioRepository;
        private ICriterioPorSubcategoriaRepository _criterioPorSubcategoriaRepository;
        private IExamenRepository _examenRepository;
        private IExamenPorCategoriaRepository _examenPorCategoriaRepository;

        public ExamenController(ICategoriaRepository categoriaRepository, 
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

        /// <summary>
        /// Nuevo :  regitra un examen
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Nuevo()
        {
            
            ExamenViewModel model = new ExamenViewModel();

            model = InicializarExamenEdit();

            Session["Accion"] = Accion.Nuevo;

            return View("Edit", model);
        }

        
        /// <summary>
        /// Edicion : Edita un examen seleccionado
        /// </summary>
        /// <returns></returns>
       [ValidarSesion]
        public ActionResult Edicion(string id)
        {
            ExamenViewModel model = new ExamenViewModel();
            model = InicializarExamenEdit();
            model.Examen = new Examen();
            model.Examen = _examenRepository.GetSingle(x => x.IdeExamen == Convert.ToInt32(id));
            
            Session["Accion"] = Accion.Editar;

            return View("Edit",model);
        }

        /// <summary>
        /// Consulta : se muestran todos los campos desabilitados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Consulta(string id)
        {
            ExamenViewModel model = new ExamenViewModel();
            model = InicializarExamenEdit();
            model.Examen = new Examen();
            model.Examen = _examenRepository.GetSingle(x => x.IdeExamen == Convert.ToInt32(id));
            
            Session["Accion"] = Accion.Consultar;

            return View("Edit",model);
        }

        


        private ExamenViewModel InicializarExamenEdit()
        {
            ExamenViewModel objExamenViewModel = new ExamenViewModel();
            objExamenViewModel.Examen = new Examen();

            objExamenViewModel.TipoExamen =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCriterio));
            objExamenViewModel.TipoExamen.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            objExamenViewModel.Examen = new Examen();
            return objExamenViewModel;
        }

        /// <summary>
        /// Inicializa los campos de la ventana inicial
        /// </summary>
        /// <returns></returns>
          private ExamenViewModel InicializarExamenIndex()
        {
            ExamenViewModel objExamenViewModel = new ExamenViewModel();
            objExamenViewModel.Examen = new Examen();

            objExamenViewModel.TipoExamen =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCriterio));
            objExamenViewModel.TipoExamen.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
           

            objExamenViewModel.TipoEstado =
              new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            objExamenViewModel.TipoEstado.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            

            return objExamenViewModel;
        }



        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult Index()
        {

            ExamenViewModel model = new ExamenViewModel();

            //model.Examen = new Examen();
            model = InicializarExamenIndex();
            model.Examen = new Examen();

            return View(model);
        }



        [HttpPost]
        [ValidarSesion]
        public ActionResult Index(ExamenViewModel model)
        {

            ExamenViewModel objExamenModel = new ExamenViewModel();

            objExamenModel = InicializarExamenIndex();
            objExamenModel.Examen = new Examen();
            objExamenModel.Categoria = new Categoria();

            objExamenModel.Examen.TipExamen = model.Examen.TipExamen;
            objExamenModel.Examen.EstActivo = model.Examen.EstActivo;
            objExamenModel.Examen.DescExamen = model.Examen.DescExamen;

            return View(objExamenModel);
        }


        /// <summary>
        /// Edicion : Guarda los datos del Examen Asignado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        [ValidarSesion]
        public ActionResult Edicion(ExamenViewModel model)
        {

            ValidationResult result;
            ExamenValidator objExamenValid = new ExamenValidator();
            ExamenViewModel objExamenViewModal= new ExamenViewModel();
            objExamenViewModal.Examen = new Examen();
            DateTime Hoy = DateTime.Today;
            JsonMessage ObjJsonMessage = new JsonMessage();

            result = objExamenValid.Validate(model.Examen, "DescExamen", "NomExamen", "TipExamen");

            if (!result.IsValid)
            {
                return View(model);
            }

            objExamenViewModal = InicializarExamenEdit();
            objExamenViewModal.Examen.TipExamen = model.Examen.TipExamen;
            objExamenViewModal.Examen.DescExamen = model.Examen.DescExamen;
            objExamenViewModal.Examen.NomExamen = model.Examen.NomExamen;

            var dato = Session["Accion"];
            if ( Accion.Nuevo.Equals(Session["Accion"]))
            {
                
                objExamenViewModal.Examen.FecCreacion = Hoy;
                objExamenViewModal.Examen.UsrCreacion = UsuarioActual.NombreUsuario;
                objExamenViewModal.Examen.EstActivo = "A";
                objExamenViewModal.Examen.EstRegistro = "A";

                _examenRepository.Add(objExamenViewModal.Examen);
                Session["Accion"] = Accion.Editar;
            }
            else
            {
                if (model.Examen.IdeExamen != null && model.Examen.IdeExamen>0)
                {
                    var objExamen = _examenRepository.GetSingle(x => x.IdeExamen == model.Examen.IdeExamen);
                    objExamen.FecModificacion = Hoy;
                    objExamen.UsrModificacion = UsuarioActual.NombreUsuario;
                    objExamen.TipExamen = model.Examen.TipExamen;
                    objExamen.DescExamen = model.Examen.DescExamen;
                    objExamen.NomExamen = model.Examen.DescExamen;
                    _examenRepository.Update(objExamen);  
                    objExamenViewModal.Examen.IdeExamen = model.Examen.IdeExamen;
                      
                }
                
            }

            return RedirectToAction("Edicion", "Examen", new { id = objExamenViewModal.Examen.IdeExamen });
        }




        [HttpPost]
        public ActionResult ListaExamen(GridTable grid)
        {
            try
            {
                // int idCriterio = Convert.ToInt32(grid.rules[0].data);
                DetachedCriteria where = null;

                if ((!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                   )
                {
                    where = DetachedCriteria.For<Examen>();

                    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                    {
                        where.Add(Expression.Eq("TipExamen", grid.rules[1].data));
                    }
                    if (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data))
                    {
                        where.Add(Expression.Eq("EstActivo", grid.rules[2].data));
                    }
                    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                    {
                        where.Add(Expression.Like("DescExamen", '%' + grid.rules[3].data + '%'));
                    }

                }

                var generic = Listar(_examenRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeExamen.ToString(),
                    cell = new string[]
                            {
                                "1",
                                item.EstActivo,
                                item.IdeExamen.ToString(),
                                item.NomExamen,
                                item.DescExamen,
                                item.TipExamen,
                                item.TipExamenDes,
                                item.TiempoTotal.ToString(),
                                item.FecCreacion == DateTime.MinValue? "": item.FecCreacion.ToString("dd/MM/yyyy"),
                                item.UsrCreacion,
                                item.FecModificacion == DateTime.MinValue? "": item.FecModificacion.ToString("dd/MM/yyyy"),
                                item.UsrModificacion
                                
                               
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
        /// Obtiene las categorias seleccionadas
        /// </summary>
        /// <param name="test"></param>
        /// <param name="subCategoria"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetListaCategoria(List<int> selc, string codExamen)
        {
            DateTime Hoy = DateTime.Today;
            ExamenPorCategoria objExamenPorCategoria;
            JsonMessage objJson= new JsonMessage();
            int codigo = 0;
            int codCategoria = 0;

            if (codExamen!=null)
            {
                 codigo = Convert.ToInt32(codExamen);
            }
            else
            {
                codigo = 0;
            }

           if (selc != null && selc.Count > 0)
            {
                for (int i = 0; i < selc.Count; i++)
                {

                    objExamenPorCategoria = new ExamenPorCategoria();
                    objExamenPorCategoria.Examen = new Examen();
                    

                    codCategoria = selc[i]==null?0:selc[i];
                    var objCriterio = _examenPorCategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == codCategoria
                                                                              && x.Examen.IdeExamen == codigo);

                    if (objCriterio!=null && objCriterio.Count>0)
                    {
                        continue;
                    }
                    else
                    {
                        objExamenPorCategoria = new ExamenPorCategoria();
                        objExamenPorCategoria.Examen = new Examen();
                        objExamenPorCategoria.Categoria = new Categoria();


                        objExamenPorCategoria.Examen.IdeExamen = codigo;
                        objExamenPorCategoria.Categoria.IDECATEGORIA = codCategoria;
                        objExamenPorCategoria.EstActivo = "A";
                        objExamenPorCategoria.FechaCreacion = Hoy;
                        objExamenPorCategoria.UsrCreacion = "Prueba 01";
                        objExamenPorCategoria.UsrModifica = "Prueba 02";
                        objExamenPorCategoria.FecModifica = Hoy;
                        _examenPorCategoriaRepository.Add(objExamenPorCategoria);
                        objJson.Mensaje = "";

                    }

                }
            }

           return Json(objJson); ;
        }


        /// <summary>
        /// ListaCatxExamen lista las categorias x Examen
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaCatxExamen(GridTable grid, int id)
        {
            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                //obtiene el valor del criterio
             
              

                // int idCriterio = Convert.ToInt32(grid.rules[0].data);

                DetachedCriteria where = DetachedCriteria.For<ExamenPorCategoria>();


                where.Add(Expression.Eq("Examen.IdeExamen", id));


                var generic = Listar(_examenPorCategoriaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExamenxCategoria.ToString(),
                        cell = new string[]
                            {
                                item.IdeExamenxCategoria.ToString(),
                                item.Categoria.IDECATEGORIA.ToString(),
                                item.Categoria.NOMCATEGORIA,
                                item.Categoria.DESCCATEGORIA,
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

        
        /// <summary>
        /// EliminarCategoriaxExamen Elimina la categoria del examen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codCat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EliminarCategoriaxExamen(string id, string codCat)
        {
            var jsonMessage = new JsonMessage();
            jsonMessage.Resultado = true;
            try
            {
                var objExamenxCat = _examenPorCategoriaRepository.GetSingle(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(codCat)
                                                               && x.Examen.IdeExamen == Convert.ToInt32(id));

                _examenPorCategoriaRepository.Remove(objExamenxCat);
            }
            catch (Exception)
            {

                jsonMessage.Resultado = false;
                jsonMessage.Mensaje = "Error : No se permite eliminar la categoría";
            }
           

            
            return Json(jsonMessage);
        }


        /// <summary>
        /// ActivarDesactivar : activa y desactiva el estado del examen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codEstado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActivarDesactivar(string id, string codEstado)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            Examen objExamen = new Examen();
            try
            {
                objExamen = _examenRepository.GetSingle(x => x.IdeExamen == Convert.ToInt32(id));

                if (IndicadorActivo.Activo.Equals(codEstado))
                {
                    objExamen.EstActivo = IndicadorActivo.Inactivo;
                    objJsonMessage.Mensaje = "Se desactivado el examen";
                }
                else
                {
                    objExamen.EstActivo = IndicadorActivo.Activo;
                    objJsonMessage.Mensaje = "Se activo el examen";
                }

                objJsonMessage.Resultado = true;
                
               
             }
             catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "Error en actualizar el estado";
            }
            
            _examenRepository.Update(objExamen);

            return Json(objJsonMessage);
        }


        /// <summary>
        /// obtiene el tiempo del examen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getTiempoExamen(string id)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            Examen objExamen = new Examen();
            int tiempo;
            try
            {
               tiempo = _examenRepository.getTiempoExamen(Convert.ToInt32(id));
               objJsonMessage.Resultado = true;
               objJsonMessage.IdDato = tiempo;
               
             }
             catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "Error al eliminar el registro";
            }
            
           

            return Json(objJsonMessage);
        }

        /// <summary>
        /// EliminarExamen : Elimina el examen seleccionado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EliminarExamen(string id)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            Examen objExamen = new Examen();
            try
            {
                objExamen = _examenRepository.GetSingle(x => x.IdeExamen == Convert.ToInt32(id));
                _examenRepository.Remove(objExamen);
                objJsonMessage.Resultado = true;
                objJsonMessage.Mensaje = "Se elimino el registro";

               
             }
             catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "Error al eliminar el registro";
            }
            
           

            return Json(objJsonMessage);
        }

        /// <summary>
        /// obtiene el archivo PDF
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
     
        //public ActionResult GetExamenPDF(string id)
        //{
        //    JsonMessage objJsonMessage = new JsonMessage();
        //    string fullPath = null;
        //    ReportDocument rep = new ReportDocument();
        //    MemoryStream mem;
            
        //    try
        //    {
               
        //        DataTable dtResultado = _examenRepository.getDataRepExamen(Convert.ToInt32(id));

                
        //        string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        //        string directoryPath = ConfigurationManager.AppSettings["ReportIntranetPath"];
        //        string nomReporte = "ExamenReport.rpt";
        //        fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, nomReporte));

        //        rep.Load(fullPath);
        //        rep.Database.Tables["DtExamen"].SetDataSource(dtResultado);

        //        mem = (MemoryStream)rep.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
             
        //    }
        //    catch (Exception)
        //    {
        //        return MensajeError();
        //    }
        //    return File(mem, "application/pdf");   
           
        //}


        /// <summary>
        /// genera PDF con Itext
        /// </summary>
        public ActionResult GetExamenPDF(string id)
        {
            
            PdfExamen objPdfExamen;
            List<PdfExamen> listaCategoria = null;
            List<PdfExamen> listaSubCategoria = null;
            List<PdfExamen> listaCriterio = null;
            List<PdfExamen> listaAlternativa = null;
            
            
            string NombreExamen = "";
            string DesExamen = "";
            string Tipo = "";
            string Duracion = "";
            
            
            objPdfExamen = new PdfExamen();


            try
            {

            
            
            objPdfExamen.Ideexamen = Convert.ToInt32(id);
            List<PdfExamen> lista = new List<PdfExamen>();

            lista = _examenRepository.ObtenerPdfExamens(objPdfExamen);
            
            // tipo de letra
            Font normal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));
            Font negrita = new Font(FontFactory.GetFont("Arial", 12, Font.BOLD));
            Font subRayado = new Font(FontFactory.GetFont("Arial", 12, Font.UNDERLINE));

            // obtengo los datos de la cabecera
            if (lista !=null)
            {
                objPdfExamen = new PdfExamen();
                objPdfExamen = (PdfExamen)lista[0];

                NombreExamen = objPdfExamen.Nomexamen;
                DesExamen = objPdfExamen.Descexamen;
                Tipo = objPdfExamen.Tipcategoria;
                Duracion = objPdfExamen.Timpoexamen;
            }

            string ConcatenExamen = DesExamen + " - " + NombreExamen; 

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 30, 30, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
               
                
                
                //Se inicializa el archivo PDF
                doc.Open();

                
                
                //se crea la  1 tabla que contiene las celdas
                PdfPTable table1 = new PdfPTable(2);
                
                // se definen los anchos de la tabla
                float[] anchos1 = new float[] { 20.0f, 80.0f };
                table1.SetWidths(anchos1);
                // se crea la celda con el valor que contiene
                PdfPCell cellNombExamen = new PdfPCell(new Phrase("EXAMEN:", negrita));
                // se alinea a la derecha
                cellNombExamen.HorizontalAlignment = Element.ALIGN_RIGHT;
                // se quitan los bordes de la celda
                cellNombExamen.Border = Rectangle.NO_BORDER;
                // se agrega la celda a la tabla
                table1.AddCell(cellNombExamen);
               
                PdfPCell cellDesExamen = new PdfPCell(new Phrase(ConcatenExamen, normal));
                cellDesExamen.Border = Rectangle.NO_BORDER;
                cellDesExamen.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(cellDesExamen);
                
                //Espacios entre tablas
                table1.SpacingBefore = 20f;
                table1.SpacingAfter = 30f;

                doc.Add(table1);

                // se crea la segunda tabla
                PdfPTable table2 = new PdfPTable(2);
                float[] anchos2 = new float[] { 20.0f, 80.0f };
                table2.SetWidths(anchos2);
                
                PdfPCell CellTipoExamen = new PdfPCell(new Phrase("TIPO: ", normal));
                CellTipoExamen.HorizontalAlignment = Element.ALIGN_LEFT;
                CellTipoExamen.Border = Rectangle.NO_BORDER;
                table2.AddCell(CellTipoExamen);

                PdfPCell CellTipoExamenDes = new PdfPCell(new Phrase(Tipo, normal));
                CellTipoExamenDes.HorizontalAlignment = Element.ALIGN_LEFT;
                CellTipoExamenDes.Border = Rectangle.NO_BORDER;
                table2.AddCell(CellTipoExamenDes);


                PdfPCell CellDuracion = new PdfPCell(new Phrase("DURACION: ", normal));
                CellDuracion.HorizontalAlignment = Element.ALIGN_LEFT;
                CellDuracion.Border = Rectangle.NO_BORDER;
                table2.AddCell(CellDuracion);

                PdfPCell CellDuracionDes = new PdfPCell(new Phrase(Duracion, normal));
                CellDuracionDes.HorizontalAlignment = Element.ALIGN_LEFT;
                CellDuracionDes.Border = Rectangle.NO_BORDER;
                table2.AddCell(CellDuracionDes);

                //Espacios entre tablas
                table2.SpacingBefore = 15f;
                table2.SpacingAfter = 10f;

                doc.Add(table2);
               
                //codigos unicos
                var listaCodCategoria = lista.GroupBy(x => x.Idecategoria).Select(x => x.Key).ToList();
                


                // Agrupacion de categoria
                foreach (Int32 itemCategoria in listaCodCategoria)
                {
                    listaCategoria = new List<PdfExamen>();
                    listaCategoria = lista.Where(x => x.Idecategoria == itemCategoria).ToList();
                    


                    objPdfExamen = new PdfExamen();
                    objPdfExamen = (PdfExamen)listaCategoria[0];

                    //objPdfExamen = new PdfExamen();
                    //objPdfExamen = (PdfExamen)item;
                   

                    // se crea la tercera tabla que es una agrupacion por categoria

                    PdfPTable table3 = new PdfPTable(3);
                    float[] ancho3 = new float[] { 10.0f,80.0f,10.0f };
                    table3.SetWidths(ancho3);

                    // celda 1
                    PdfPCell CelltCategoria = new PdfPCell(new Phrase(objPdfExamen.Nomcategoria+" "+objPdfExamen.Tiempocat, normal));
                    CelltCategoria.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelltCategoria.Border = Rectangle.NO_BORDER;
                    // se le indica que ocupe las 3 columnas
                    CelltCategoria.Colspan = 3;
                    table3.AddCell(CelltCategoria);
                    
                    //celda 2 de instrucciones
                    PdfPCell CelltituloInstruccion = new PdfPCell(new Phrase("INSTRUCCIONES:",normal));
                    CelltituloInstruccion.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelltituloInstruccion.Colspan = 3;
                    CelltituloInstruccion.Border = Rectangle.NO_BORDER;
                    table3.AddCell(CelltituloInstruccion);
                   
                    // celda 3
                    PdfPCell CellInstruccion = new PdfPCell(new Phrase(objPdfExamen.Instrucciones, normal));
                    CellInstruccion.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    CellInstruccion.Border = Rectangle.NO_BORDER;
                    // se le indica que ocupe las 3 columnas
                    CellInstruccion.Colspan = 3;
                    //celda enblanco
                    PdfPCell CellBlanco2 = new PdfPCell(new Phrase("", normal));
                    CellBlanco2.Colspan = 3;
                    CellBlanco2.Border = Rectangle.NO_BORDER;
                    table3.AddCell(CellBlanco2);
                   
                    
                    table3.AddCell(CellInstruccion);

                    //celda 4
                    if (objPdfExamen.Tipoejemplo!=null && objPdfExamen.Tipoejemplo!="")
                    {
                         PdfPCell CellEjemploCat = null;

                            if ("02".Equals(objPdfExamen.Tipoejemplo))
                            {
                                if (objPdfExamen.Imagenejemplo!=null)
                                {
                                    Image EjemploCategoria = Image.GetInstance(objPdfExamen.Imagenejemplo);
                                    EjemploCategoria.ScaleToFit(180f, 180f);

                                    CellEjemploCat = new PdfPCell(EjemploCategoria);
                                    CellEjemploCat.HorizontalAlignment = Element.ALIGN_LEFT;
                                    CellEjemploCat.Border = Rectangle.NO_BORDER;
                                    // se le indica que ocupe las 3 columnas
                       
                                }
                                else
                                {
                                    CellEjemploCat = new PdfPCell(new Phrase("", normal));
                       
                                }
                            }

                            if ("01".Equals(objPdfExamen.Tipoejemplo))
                            {
                                CellEjemploCat = new PdfPCell(new Phrase(objPdfExamen.Textoejemplo, normal));
                                CellEjemploCat.HorizontalAlignment = Element.ALIGN_LEFT;
                                CellEjemploCat.Border = Rectangle.NO_BORDER;
                            }


                            CellEjemploCat.Border = Rectangle.NO_BORDER;
                            CellEjemploCat.Colspan = 3;
                            table3.AddCell(CellEjemploCat);
                    }

                    table3.SpacingBefore = 10f;
                    table3.SpacingAfter = 10f;

                    doc.Add(table3);

                    // se obtiene los codigos de la subcategoria unicos por categoria 
                    // la lista listaCategoria contiene solo los registros de dicha categoria
                    var listaCodSubcategoria = listaCategoria.GroupBy(x => x.Idesubcategoria).Select(x => x.Key).ToList();
                    //SUBCATEGORIA
                    foreach (Int32 itemSubCategoria in listaCodSubcategoria)
                    {
                        listaSubCategoria = new List<PdfExamen>();
                        listaSubCategoria = lista.Where(x => x.Idesubcategoria == itemSubCategoria 
                                                        && x.Idecategoria == itemCategoria).ToList();


                        PdfExamen objSubCategoria = new PdfExamen();
                        objSubCategoria = (PdfExamen)listaSubCategoria[0];

                        // se crea la cuarta tabla
                        PdfPTable table4 = new PdfPTable(3);
                        float[] ancho4 = new float[] { 10.0f, 80.0f, 10.0f };
                        table4.SetWidths(ancho4);

                        // se agregan las celdas a la tabla
                        // celda nombre la subcategoria
                        PdfPCell CellNombSubcAtegoria = new PdfPCell(new Phrase(objSubCategoria.Nomsubcategoria, normal));
                        CellNombSubcAtegoria.HorizontalAlignment = Element.ALIGN_LEFT;
                        CellNombSubcAtegoria.Colspan = 3;
                        CellNombSubcAtegoria.Border = Rectangle.NO_BORDER;
                        table4.SpacingBefore = 10f;
                        table4.SpacingAfter = 10f;
                        table4.AddCell(CellNombSubcAtegoria);
                        doc.Add(table4);

                        //CRITERIO
                        int cont = 1;
                        var listaCodCriterio = listaSubCategoria.GroupBy(x => x.Idecriterio).Select(x => x.Key).ToList();
                        foreach (var itemCriterio in listaCodCriterio)
                        {
                             
                            listaCriterio = new List<PdfExamen>();
                             listaCriterio = lista.Where(x => x.Idesubcategoria == itemSubCategoria
                                                        && x.Idecategoria == itemCategoria && x.Idecriterio == itemCriterio).ToList();

                            PdfExamen objCriterio = new PdfExamen();
                            // se obtiene el criterio 
                            objCriterio = (PdfExamen)listaCriterio[0];
                            
                            PdfPTable table5 = new PdfPTable(3);
                            float[] ancho5 = new float[] { 10.0f, 80.0f, 10.0f };
                            table5.SetWidths(ancho5);
                            PdfPCell CellPregunta = new PdfPCell(new Phrase("PREGUNTA NRO. "+cont, negrita));
                            CellPregunta.Colspan = 3;
                            CellPregunta.Border = Rectangle.NO_BORDER;
                            
                            CellPregunta.SpaceCharRatio = 10f; //espacio de celda
                            
                            table5.AddCell(CellPregunta);
                            

                            // si el codMod es 02 muestra imagen si no muestra texto

                            PdfPCell CellPreguntaCriterio = null;
                            if ("02".Equals(objCriterio.Codmod))
                            {
                                if (objCriterio.Imagencrit!=null)
                                {
                                    Image PreguntaCriterio = Image.GetInstance(objCriterio.Imagencrit);
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
                                CellPreguntaCriterio = new PdfPCell(new Phrase(objCriterio.Pregunta, normal));
                            }

                            cont = cont + 1;
                            CellPreguntaCriterio.Colspan = 3;
                            CellPreguntaCriterio.Border = Rectangle.NO_BORDER;
                            table5.AddCell(CellPreguntaCriterio);
                            table5.SpacingBefore = 2f;
                            table5.SpacingAfter = 2f;

                            doc.Add(table5);
                            
                            // ALTERNATIVAS
                            int nContAltenativa = 1;
                            string opcion = "";
                            var listaCodAlternativa = listaCriterio.GroupBy(x => x.Idealternativa).Select(x => x.Key).ToList();
                            foreach (Int32 itemAlternativa in listaCodAlternativa)
                            {
                                listaAlternativa= new List<PdfExamen>();

                                listaAlternativa = lista.Where(x => x.Idesubcategoria == itemSubCategoria
                                                       && x.Idecategoria == itemCategoria && x.Idecriterio == itemCriterio
                                                       && x.Idealternativa == itemAlternativa).ToList();

                                PdfExamen objAlternativa = new PdfExamen();

                                objAlternativa = (PdfExamen)listaAlternativa[0];
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

                                if ("02".Equals(objCriterio.Codmod))
                                {
                                    if (objAlternativa.Image != null)
                                    {
                                        Image OpcionImage = Image.GetInstance(objAlternativa.Image);
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
                                    CellOpcionAlternativa = new PdfPCell(new Phrase(objAlternativa.Alternativa, normal));
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


                        }

                    }

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
                        res = "A";
                        break;
                    case 2:
                        res = "B";
                        break;
                    case 3:
                        res = "C";
                        break;
                    case 4:
                        res = "D";
                        break;
                    case 5:
                        res = "E";
                        break;
                    case 6:
                        res = "F";
                        break;
                    case 7:
                        res = "G";
                        break;
                    case 8:
                        res = "H";
                        break;
                    case 9:
                        res = "I";
                        break;
                    case 10:
                        res = "J";
                        break;
                    case 11:
                        res = "K";
                        break;
                    case 12:
                        res = "L";
                        break;
                    case 13:
                        res = "M";
                        break;
                    case 14:
                        res = "N";
                        break;
                    case 15:
                        res = "O";
                        break;
                    case 16:
                        res = "P";
                        break;
                    case 17:
                        res = "Q";
                        break;
                    case 18:
                        res = "R";
                        break;
                    case 19:
                        res = "S";
                        break;
                    case 20:
                        res = "T";
                        break;
                    case 21:
                        res = "U";
                        break;
                    case 22:
                        res = "V";
                        break;
                    case 23:
                        res = "W";
                        break;
                    case 24:
                        res = "X";
                        break;
                    case 25:
                        res = "Y";
                        break;
                    case 26:
                        res = "Z";
                        break;
    

                }
                return res;
            }
        }


    }
}
