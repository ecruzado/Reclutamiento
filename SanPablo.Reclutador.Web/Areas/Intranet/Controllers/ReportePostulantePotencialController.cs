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
    
    public class ReportePostulantePotencialController : BaseController
    {
        

        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IUsuarioRepository _usuarioRepository;
        private ISedeRepository _sedeRepository;
        private IPostulanteRepository _postulanteRepository;
        private ICargoRepository _cargoRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IAreaRepository _areaRepository;

        public ReportePostulantePotencialController(IDetalleGeneralRepository detalleGeneralRepository,
                                                    ISolReqPersonalRepository solReqPersonalRepository,
                                                    IUsuarioRepository usuarioRepository,
                                                    ISedeRepository sedeRepository,
                                                    IPostulanteRepository postulanteRepository,
                                                    ICargoRepository cargoRepository,
                                                    IDependenciaRepository dependenciaRepository,
                                                    IDepartamentoRepository departamentoRepository,
                                                    IAreaRepository areaRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _usuarioRepository = usuarioRepository;
            _sedeRepository = sedeRepository;
            _postulanteRepository = postulanteRepository;
            _cargoRepository = cargoRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _areaRepository = areaRepository;
        }

       
        [Authorize]
        [ValidarSesion]
        public ActionResult Index()
        {

            ReportePostulantesPotencialesViewModel reporteModel = new ReportePostulantesPotencialesViewModel();

            reporteModel = Inicializar();

            return View("Index", reporteModel);
        }

        
        [HttpPost]
        public ActionResult listaPostulantesPotenciales(GridTable grid)
        {

            ReportePostulantePotencial postulantePotencial = new ReportePostulantePotencial();
            List<ReportePostulantePotencial> lista = new List<ReportePostulantePotencial>();
            try
            {


                postulantePotencial.IdeCargo = (grid.rules[0].data == null? 0 : Convert.ToInt32(grid.rules[0].data));
                postulantePotencial.AreaEstudio = (grid.rules[1].data == "0" ? "" : grid.rules[1].data);
                postulantePotencial.RangoSalarial = (grid.rules[2].data == "0" ? "" : grid.rules[2].data);

                postulantePotencial.IdeSede = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                postulantePotencial.IdeDependencia = (grid.rules[4].data == null ? 0 : Convert.ToInt32(grid.rules[4].data));
                postulantePotencial.IdeDepartamento = (grid.rules[5].data == null ? 0 : Convert.ToInt32(grid.rules[5].data));
                postulantePotencial.IdeArea = (grid.rules[6].data == null ? 0 : Convert.ToInt32(grid.rules[6].data));
                
                postulantePotencial.EdadInicio = (grid.rules[7].data == null ? 0 : Convert.ToInt32(grid.rules[8].data));
                postulantePotencial.EdadFin = (grid.rules[8].data == null ? 100 : Convert.ToInt32(grid.rules[9].data));

                if (grid.rules[9].data != null && grid.rules[10].data != null)
                {
                    postulantePotencial.FechaDesde = Convert.ToDateTime(grid.rules[9].data);
                    postulantePotencial.FechaHasta = Convert.ToDateTime(grid.rules[10].data);
                }

               
                Session[ConstanteSesion.PostulantePotencial] = postulantePotencial;

                lista = _postulanteRepository.ListaPostulantesPotenciales(postulantePotencial);

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeReclutaPersona.ToString(),
                    cell = new string[]
                            {
                               
                                "1",
                                item.FechaPostulantePotencial==null?"":item.FechaPostulantePotencial.ToString(),
                                item.Sede==null?"":item.Sede,
                                item.Dependencia==null?"":item.Dependencia,
                                item.Departamento==null?"":item.Departamento,
                                item.Area==null?"":item.Area,
                                item.NombreCompleto==null?"":item.NombreCompleto,
                                item.Cargo==null?"":item.Cargo,
                                item.TelefonoContacto==null?"":item.TelefonoContacto.ToString(),
                                item.Email==null?"":item.Email,
                                item.Edad==0?"":item.Edad.ToString(),
                                item.PuntajeCV==0?"":item.PuntajeCV.ToString(),
                                item.PuntajeSeleccion==0?"":item.PuntajeSeleccion.ToString(),
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

        private ReportePostulantesPotencialesViewModel Inicializar()
        {
            var reporteModel = new ReportePostulantesPotencialesViewModel();
            reporteModel.PostulantePotencial = new ReportePostulantePotencial();

            reporteModel.Cargos = new List<Cargo>(_cargoRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
            reporteModel.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "SELECCIONAR" });

            //CARGAR AREAS DE ESTUDIO
            reporteModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoArea));
            reporteModel.AreasEstudio.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONAR" });


            reporteModel.Sedes = new List<Sede>(_sedeRepository.GetBy(x=>x.EstadoRegistro == IndicadorActivo.Activo));
            reporteModel.Sedes.Insert(0, new Sede { CodigoSede = "0", DescripcionSede = "SELECCIONAR" });
            reporteModel.Sedes.Add(new Sede { CodigoSede = "999", DescripcionSede = "TODAS" });

            reporteModel.Dependencias = new List<Dependencia>();
            reporteModel.Dependencias.Add(new Dependencia { IdeDependencia = 0, NombreDependencia = "SELECCIONAR" });

            reporteModel.Departamentos = new List<Departamento>();
            reporteModel.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "SELECCIONAR" });

            reporteModel.Areas = new List<SanPablo.Reclutador.Entity.Area>();
            reporteModel.Areas.Add(new SanPablo.Reclutador.Entity.Area { IdeArea = 0, NombreArea = "SELECCIONAR" });
           
            //CARGAR LOS RANGOS SALARIALES

            reporteModel.RangosSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            reporteModel.RangosSalariales.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONAR" });


            return reporteModel;
        }

        /// <summary>
        /// lista de cargos respecto a los cargos
        /// </summary>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        public ActionResult listaCargos(int ideSede)
        {
            ActionResult result = null;
            var listaResultado = new List<Cargo>(_cargoRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                  && x.IdeSede == ideSede));
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// lista de dependencia
        /// </summary>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaDependencia(int ideSede)
        {
            ActionResult result = null;
            Dependencia objDepencia = new Dependencia();

            var listaResultado = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                         && x.IdeSede == ideSede));
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// lista de departamentos
        /// </summary>
        /// <param name="ideDependencia"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaDepartamentos(int ideDependencia)
        {
            ActionResult result = null;
            Dependencia objDepencia = new Dependencia();

            var listaResultado = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == ideDependencia));
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// lista de areas
        /// </summary>
        /// <param name="ideDepartamento"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaAreas(int ideDepartamento)
        {
            ActionResult result = null;

            var listaResultado = new List<SanPablo.Reclutador.Entity.Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));
            result = Json(listaResultado);
            return result;
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

            ReportePostulantePotencial postulanteReporte = new ReportePostulantePotencial();

            try
            {
                postulanteReporte = (ReportePostulantePotencial)Session[ConstanteSesion.PostulantePotencial];

                string usuario = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                string sede = Convert.ToString(Session[ConstanteSesion.SedeDes]);

                DataTable dtInforme = new DataTable();
                dtInforme.Columns.Add("USUARIO", typeof(string));
                dtInforme.Columns.Add("SEDE", typeof(string));
                dtInforme.Columns.Add("CARGOBUSCA", typeof(string));
                dtInforme.Rows.Add(usuario, sede, postulanteReporte.Cargo);
                  
                DataTable dtResultado = _postulanteRepository.DtReportePostulantesPotencial(postulanteReporte);

                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = ConfigurationManager.AppSettings["ReportIntranetPath"];
                string nomReporte = "ReportePostulantePotencial.rpt";
                fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, nomReporte));

                reporte.Load(fullPath);
                reporte.Database.Tables["dtPostulantePotencial"].SetDataSource(dtResultado);
                reporte.Database.Tables["dtDatosReporte"].SetDataSource(dtInforme);
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

            if (Session[ConstanteSesion.PostulantePotencial] != null)
            {
                ReportePostulantePotencial postulanteReporte = (ReportePostulantePotencial)Session[ConstanteSesion.PostulantePotencial];
                List<ReportePostulantePotencial> listaResultado = _postulanteRepository.ListaPostulantesPotenciales(postulanteReporte);

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
                int cantCol = 13;

                objGeneraExcel.addTituloExcel(1, 1, 1, cantCol, "REPORTE DE POSTULANTES BASE DE DATOS", styleTitulo);

                objGeneraExcel.AdicionaLogoSanPablo(dir, 1, 2, 0, 4);
                objGeneraExcel.adicionaCamposCab(5, 1, "Reclutamiento y Selección de Personal", styleNegrita);

                //    objGeneraExcel.adicionaCamposCab(2, (cantCol / 2) - 2, "Sede :", styleCadena);
                //    objGeneraExcel.adicionaCamposCab(2, (cantCol / 2) - 1, sede, styleCadena);

                //objGeneraExcel.adicionaCamposCab(2, cantCol - 1, "Fecha :", styleCadena);
                //objGeneraExcel.adicionaCamposCab(2, cantCol, fecha.ToString("dd/MM/yyyy"), styleCadena);
                //objGeneraExcel.adicionaCamposCab(3, cantCol - 1, "Hora :", styleCadena);
                //objGeneraExcel.adicionaCamposCab(3, cantCol, fecha.ToString("HH:mm:ss tt"), styleCadena);
                //objGeneraExcel.adicionaCamposCab(4, cantCol - 1, "Usuario :", styleCadena);
                //objGeneraExcel.adicionaCamposCab(4, cantCol, UsuarioActual.NombreUsuario, styleCadena);

                //objGeneraExcel.addDetalleLista(dtResultado, 1, "FECHA DE REGISTRO DE CV", 1);
                //objGeneraExcel.addDetalleLista(dtResultado, 2, "DEPARTAMENTO", 2);
                //objGeneraExcel.addDetalleLista(dtResultado, 3, "PROVINCIA", 3);
                //objGeneraExcel.addDetalleLista(dtResultado, 4, "DISTRITO", 4);
                //objGeneraExcel.addDetalleLista(dtResultado, 5, "NOMBRES Y APELLIDOS", 5);
                //objGeneraExcel.addDetalleLista(dtResultado, 6, "CEL. / FIJO", 6);
                //objGeneraExcel.addDetalleLista(dtResultado, 7, "EMAIL", 7);
                //objGeneraExcel.addDetalleLista(dtResultado, 8, "CARGO", 8);
                //objGeneraExcel.addDetalleLista(dtResultado, 9, "EDAD", 9);
                //objGeneraExcel.addDetalleLista(dtResultado, 10, "TIPO DE ESTUDIOS", 10);
                //objGeneraExcel.addDetalleLista(dtResultado, 11, "ÁREA DE ESTUDIOS", 11);
                //objGeneraExcel.addDetalleLista(dtResultado, 12, "RANGO SALARIAL", 12);


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

        public ActionResult autoCompletarCargo(string query)
        {
            var ideSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
            query = query.Replace(" ", "");
            if (query.Length > 1)
            {
                int op = query.LastIndexOf(",");
                query = query.Substring(op + 1);
            }
            //List<Cargo> obj = new List<Cargo>();
            var obj = _cargoRepository.GetBy(x => x.IdeSede == ideSede);

            var users = (from u in obj
                         where u.NombreCargo.Contains(query)
                         select u.NombreCargo).Distinct().ToArray();
            return Json(users, JsonRequestBehavior.AllowGet);
        }



    }

}
