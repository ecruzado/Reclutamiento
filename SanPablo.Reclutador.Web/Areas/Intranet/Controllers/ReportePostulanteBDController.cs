namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{

    using CrystalDecisions.CrystalReports.Engine;
    using FluentValidation;
    using FluentValidation.Results;
    using Newtonsoft.Json;
    using NHibernate.Criterion;
    using NPOI.SS.UserModel;
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
    
    public class ReportePostulanteBDController : BaseController
    {
        

        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IUsuarioRepository _usuarioRepository;
        private ISedeRepository _sedeRepository;
        private IUbigeoRepository _ubigeoRepository;
        private IPostulanteRepository _postulanteRepository;

        public ReportePostulanteBDController(IDetalleGeneralRepository detalleGeneralRepository,
                                           ISolReqPersonalRepository solReqPersonalRepository,
                                           IUsuarioRepository usuarioRepository,
                                           ISedeRepository sedeRepository,
                                           IUbigeoRepository ubigeoRepository,
                                           IPostulanteRepository postulanteRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _usuarioRepository = usuarioRepository;
            _sedeRepository = sedeRepository;
            _ubigeoRepository = ubigeoRepository;
            _postulanteRepository = postulanteRepository;
        }

        /// <summary>
        /// inicializa la ventana de reporte de seleccion
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [ValidarSesion]
        public ActionResult Index()
        {

            ReportePostulanteViewModel reporteModel = new ReportePostulanteViewModel();

            reporteModel = Inicializar();

            return View("Index", reporteModel);
        }

        /// <summary>
        /// lista los postulantes BD
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaPostulantesBD(GridTable grid)
        {
            
            PostulanteBDReporte postulanteBD = new PostulanteBDReporte();
            List<PostulanteBDReporte> lista = new List<PostulanteBDReporte>();
            try
            {


                postulanteBD.Cargo = (grid.rules[0].data == null ? "" : grid.rules[0].data);
                postulanteBD.AreaEstudio = (grid.rules[1].data == "0" ? "" : grid.rules[1].data);
                postulanteBD.RangoSalarial = (grid.rules[2].data == "0" ? "" : grid.rules[2].data);

                postulanteBD.IdeDepartamento = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                postulanteBD.IdeProvincia = (grid.rules[4].data == null ? 0 : Convert.ToInt32(grid.rules[4].data));
                postulanteBD.IdeDistrito = (grid.rules[5].data == null ? 0 : Convert.ToInt32(grid.rules[5].data));

                if (grid.rules[6].data != null && grid.rules[7].data != null)
                {
                    postulanteBD.FechaDesde = Convert.ToDateTime(grid.rules[6].data);
                    postulanteBD.FechaHasta = Convert.ToDateTime(grid.rules[7].data);
                }

                postulanteBD.EdadInicio = (grid.rules[8].data == null ? 0 : Convert.ToInt32(grid.rules[8].data));
                postulanteBD.EdadFin = (grid.rules[9].data == null ? 100 : Convert.ToInt32(grid.rules[9].data));

                Session[ConstanteSesion.DatosReporte] = postulanteBD;

                lista = _postulanteRepository.ListaPostulantesBDReporte(postulanteBD);

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdePostulante.ToString(),
                    cell = new string[]
                            {
                               
                                "1",
                                item.FechaRegistro==null?"":item.FechaRegistro.ToString(),
                                item.Departamento==null?"":item.Departamento,
                                item.Provincia==null?"":item.Provincia,
                                item.Distrito==null?"":item.Distrito,
                                item.NombreCompleto==null?"":item.NombreCompleto,
                                item.TelefonoContacto==null?"":item.TelefonoContacto.ToString(),
                                item.Email==null?"":item.Email,
                                item.Cargo==null?"":item.Cargo,
                                item.Edad==0?"":item.Edad.ToString(),
                                item.TipoEstudio==null?"":item.TipoEstudio,
                                item.AreaEstudio==null?"":item.AreaEstudio,
                                item.RangoSalarial==null?"":item.RangoSalarial,
                                item.IdePostulante==0?"":item.IdePostulante.ToString()
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

        private ReportePostulanteViewModel Inicializar()
        {
            var reporteModel = new ReportePostulanteViewModel();
            reporteModel.Solicitud = new SolReqPersonal();

            //reporteModel.Departamento = new Ubigeo();
            reporteModel.Departamentos = new List<Ubigeo>();
            reporteModel.Departamentos = cargarDepartamentos();
            reporteModel.Departamentos.Insert(0, new Ubigeo { IdeUbigeo = 0, Nombre = "SELECCIONAR" });


            //reporteModel.Provincia = new Ubigeo();
            reporteModel.Provincias = new List<Ubigeo>();
            reporteModel.Provincias.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "SELECCIONAR" });

            reporteModel.Distrito = new Ubigeo();
            reporteModel.Distritos = new List<Ubigeo>();
            reporteModel.Distritos.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "SELECCIONAR" });

            //CARGAR AREAS DE ESTUDIO
            //reporteModel.AreaEstudio = new DetalleGeneral();
            reporteModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoArea));
            reporteModel.AreasEstudio.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONAR" });

            //CARGAR LOS RANGOS SALARIALES

            //reporteModel.RangoSalarial = new DetalleGeneral();
            reporteModel.RangosSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            reporteModel.RangosSalariales.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONAR" });


            return reporteModel;
        }

        [HttpPost]
        public ActionResult listarUbigeos(int ideUbigeoPadre)
        {
            ActionResult result = null;

            var listaResultado = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == ideUbigeoPadre));
            result = Json(listaResultado);
            return result;
        }

        public List<Ubigeo> cargarDepartamentos()
        {
            var departamentos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == null));
            return departamentos;

        }

        /// <summary>
        /// Obtiene el reporte en formato PDF
        /// </summary>
        /// <returns></returns>
        public ActionResult ObtenerReportePDF()
        {
            JsonMessage objJsonMessage = new JsonMessage();
            string fullPath = null;
            ReportDocument reporte = new ReportDocument();
            MemoryStream memory;

            PostulanteBDReporte postulanteReporte = new PostulanteBDReporte();

            try
            {
                postulanteReporte = (PostulanteBDReporte)Session[ConstanteSesion.DatosReporte];

                DataTable dtResultado = _postulanteRepository.DtPostulantesBDReporte(postulanteReporte);

                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = ConfigurationManager.AppSettings["ReportIntranetPath"];
                string nomReporte = "ReportePostulanteBD.rpt";
                fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, nomReporte));

                reporte.Load(fullPath);
                reporte.Database.Tables["dsReportePostulanteBD"].SetDataSource(dtResultado);

                memory = (MemoryStream)reporte.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            }
            catch (Exception)
            {
                return MensajeError();
            }
            return File(memory, "application/pdf");
        }


        /// <summary>
        /// genera el reporte en formato excel
        /// </summary>
        public void ObtenerReporteExcel()
        {

            if (Session[ConstanteSesion.DatosReporte] != null)
            {
                PostulanteBDReporte postulanteReporte = null;
                postulanteReporte = (PostulanteBDReporte)Session[ConstanteSesion.DatosReporte];
                DataTable dtResultado = _postulanteRepository.DtPostulantesBDReporte(postulanteReporte);


                string fileName = System.Guid.NewGuid().ToString().Replace("-", "") + ".xlsx";
                string pathApliacion = Server.MapPath(".");
                ReporteExcel objGeneraExcel = new ReporteExcel();
                ICellStyle styleTitulo, styleCadena, styleNegrita, styleNumero;

                objGeneraExcel.creaHoja("pagina 01");
                styleTitulo = objGeneraExcel.addEstiloTitulo(true, 14, "CENTER");
                styleCadena = objGeneraExcel.addEstiloCadena(false, 10, "LEFT");
                styleNegrita = objGeneraExcel.addEstiloCadenaNegrita(10, "LEFT");
                styleNumero = objGeneraExcel.addEstiloNumero(false, 10, "RIGHT");

                DateTime fecha = DateTime.Now;


                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = "\\Content\\images\\logo_san_pablo_png.png";
                string nombreTemporalArchivo = Guid.NewGuid().ToString();
                string fullPath = applicationPath + directoryPath;
                string dir = fullPath;

                //numero de columnas excel
                int cantCol = 21;

                objGeneraExcel.addTituloExcel(1, 1, 1, cantCol, "REPORTE DE POSTULANTES BASE DE DATOS", styleTitulo);

                objGeneraExcel.AdicionaLogoSanPablo(dir, 1, 2, 0, 4);
                objGeneraExcel.adicionaCamposCab(5, 1, "Reclutamiento y Selección de Personal", styleNegrita);

                //    objGeneraExcel.adicionaCamposCab(2, (cantCol / 2) - 2, "Sede :", styleCadena);
                //    objGeneraExcel.adicionaCamposCab(2, (cantCol / 2) - 1, sede, styleCadena);

                objGeneraExcel.adicionaCamposCab(2, cantCol - 1, "Fecha :", styleCadena);
                objGeneraExcel.adicionaCamposCab(2, cantCol, fecha.ToString("dd/MM/yyyy"), styleCadena);
                objGeneraExcel.adicionaCamposCab(3, cantCol - 1, "Hora :", styleCadena);
                objGeneraExcel.adicionaCamposCab(3, cantCol, fecha.ToString("HH:mm:ss tt"), styleCadena);
                objGeneraExcel.adicionaCamposCab(4, cantCol - 1, "Usuario :", styleCadena);
                objGeneraExcel.adicionaCamposCab(4, cantCol, UsuarioActual.NombreUsuario, styleCadena);

                objGeneraExcel.addDetalleLista(dtResultado, 1, "FECHA DE REGISTRO DE CV", 1);
                objGeneraExcel.addDetalleLista(dtResultado, 2, "DEPARTAMENTO", 2);
                objGeneraExcel.addDetalleLista(dtResultado, 3, "PROVINCIA", 3);
                objGeneraExcel.addDetalleLista(dtResultado, 4, "DISTRITO", 4);
                objGeneraExcel.addDetalleLista(dtResultado, 5, "NOMBRES Y APELLIDOS", 5);
                objGeneraExcel.addDetalleLista(dtResultado, 6, "CEL. / FIJO", 6);
                objGeneraExcel.addDetalleLista(dtResultado, 7, "EMAIL", 7);
                objGeneraExcel.addDetalleLista(dtResultado, 8, "CARGO", 8);
                objGeneraExcel.addDetalleLista(dtResultado, 9, "EDAD", 9);
                objGeneraExcel.addDetalleLista(dtResultado, 10, "TIPO DE ESTUDIOS", 10);
                objGeneraExcel.addDetalleLista(dtResultado, 11, "ÁREA DE ESTUDIOS", 11);
                objGeneraExcel.addDetalleLista(dtResultado, 12, "RANGO SALARIAL", 12);


                // Se coloca de manera obligatoria
                objGeneraExcel.imprimirCabecera(8, styleNegrita);
                objGeneraExcel.imprimiDetalle(9, styleCadena);


                MemoryStream exportData = new MemoryStream();
                using (exportData)
                {
                    exportData = objGeneraExcel.imprimeExcel(exportData);
                    string saveAsFileName = string.Format("Reporte Postulantes BD-{0:d}.xls", DateTime.Now).Replace("/", "-");

                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                    Response.Clear();
                    Response.BinaryWrite(exportData.GetBuffer());
                    Response.End();

                }
            }
        }  

    }

}
