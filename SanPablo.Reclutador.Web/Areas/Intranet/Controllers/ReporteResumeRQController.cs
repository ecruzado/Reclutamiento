﻿

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
    using System.Data;
    using NPOI.SS.UserModel;
    using System.Collections;
    using CrystalDecisions.CrystalReports.Engine;
    using NPOI.SS.UserModel;
    using CrystalDecisions.Shared;

    public class ReporteResumeRQController : BaseController
    {
        //
        // GET: /Intranet/ReporteResumeRQ/

        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IAreaRepository _areaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private ITipoRequerimiento _tipoRequerimiento;
        private ICargoRepository _cargoRepository;
        private IConocimientoGeneralRequerimientoRepository _ConocimientoGeneralRequerimientoRepository;
        private INivelAcademicoRequerimientoRepository _nivelAcademicoRequerimientoRepository;
        private ICompetenciaRequerimientoRepository _competenciaRequerimientoRepository;
        private IExperienciaRequerimientoRepository _experienciaRequerimientoRepository;
        private IOfrecemosRequerimientoRepository _ofrecemosRequerimientoRepository;
        private IUsuarioRepository _usuarioRepository;
        private ISedeRepository _sedeRepository;

        public ReporteResumeRQController(IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository,
                                         IDependenciaRepository dependenciaRepository,
                                         IAreaRepository areaRepository,
                                         IDepartamentoRepository departamentoRepository,
                                         IUsuarioRolSedeRepository usuarioRolSedeRepository,
                                         ITipoRequerimiento tipoRequerimiento,
                                         ICargoRepository cargoRepository,
            IConocimientoGeneralRequerimientoRepository conocimientoGeneralRequerimientoRepository,
            INivelAcademicoRequerimientoRepository nivelAcademicoRequerimientoRepository,
            ICompetenciaRequerimientoRepository CompetenciaRequerimientoRepository,
            IExperienciaRequerimientoRepository experienciaRequerimientoRepository,
            IOfrecemosRequerimientoRepository ofrecemosRequerimientoRepository,
            IUsuarioRepository usuarioRepository,
            ISedeRepository sedeRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _dependenciaRepository = dependenciaRepository;
            _areaRepository = areaRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _tipoRequerimiento = tipoRequerimiento;
            _cargoRepository = cargoRepository;
            _ConocimientoGeneralRequerimientoRepository = conocimientoGeneralRequerimientoRepository;
            _nivelAcademicoRequerimientoRepository = nivelAcademicoRequerimientoRepository;
            _competenciaRequerimientoRepository = CompetenciaRequerimientoRepository;
            _experienciaRequerimientoRepository = experienciaRequerimientoRepository;
            _ofrecemosRequerimientoRepository = ofrecemosRequerimientoRepository;
            _usuarioRepository = usuarioRepository;
            _sedeRepository = sedeRepository;
        }


        /// <summary>
        /// inicializa la pantala inicial
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [ValidarSesion]
        public ActionResult Index()
        {
            ReporteViewModel objReporteModel = new ReporteViewModel();

            objReporteModel = Inicializar();

            objReporteModel.Solicitud = new SolReqPersonal();
            objReporteModel.ReporteSol = new Reporte();


            return View("Index", objReporteModel);
            
        }

        /// <summary>
        /// Inicializa las listas 
        /// </summary>
        /// <returns></returns>
        private ReporteViewModel Inicializar()
        {
            var objModel = new ReporteViewModel();

            var idSede = Session[ConstanteSesion.Sede];

            //// motivo de reemplazo
            //objModel.ListaMotivo =
            //new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVacante));
            //objModel.ListaMotivo.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            
            objModel.ListaSede = new List<Sede>(_sedeRepository.GetByTipSede());
            objModel.ListaSede.Insert(0, new Sede { CodigoSede = "0", DescripcionSede = "Seleccionar" });

            //objModel.ListaTipoSol =
            //new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSolicitud));
            //objModel.ListaTipoSol.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            //List<DetalleGeneral> listaSolicitud = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapa));
            //objModel.ListaEstadoReq = listaSolicitud.Where(x => x.Valor == "04" || x.Valor == "08").ToList();
            //objModel.ListaEstadoReq.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            ////new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapaSolicitud));
            ////objModel.ListaEstadoReq.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            ////objModel.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
            ////                                                             && x.IdeSede == UsuarioSede.IDESEDE));
            //objModel.listaDependencia = new List<Dependencia>();
            //objModel.listaDependencia.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            //objModel.ListaDepartamento = new List<Departamento>();
            //objModel.ListaDepartamento.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            //objModel.ListaArea = new List<SanPablo.Reclutador.Entity.Area>();
            //objModel.ListaArea.Add(new SanPablo.Reclutador.Entity.Area { IdeArea = 0, NombreArea = "Seleccionar" });

            objModel.ListaAnalistaResp = new List<Usuario>();
            objModel.ListaAnalistaResp.Add(new Usuario { IdUsuario = 0, NombreUsuario = "Seleccionar" });

            return objModel;
        }



        /// <summary>
        /// lista de analista de responsables
        /// </summary>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaAnalistaResp(int ideSede)
        {
            ActionResult result = null;

            SolReqPersonal objSol = new SolReqPersonal();
            objSol.IdeSede = ideSede;

            var listaResultado = new List<Usuario>(_usuarioRepository.GetAnalistaRespoanble(objSol));
            result = Json(listaResultado);
            return result;
        }



        /// <summary>
        /// obtiene la lista resumen
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListResumen(GridTable grid)
        {

            Reporte objReporte;
            List<Reporte> listaReporte = new List<Reporte>();
            try
            {

                objReporte = new Reporte();


                  //{ field: 'cDesde', data: $("#ReporteSol_FechaInicio").val() },
                  //     { field: 'cHasta', data: $("#ReporteSol_FechaFin").val() },
                  //     { field: 'cSede', data: $("#ReporteSol_idSede").val() },
                  //     { field: 'cAnalista', data: $("#ReporteSol_idAnalistaResp").val() }

                var FecDesde = grid.rules[0].data;

                if (FecDesde != null)
                {
                    objReporte.FechaInicio = FecDesde;
                }
                else
                {
                    objReporte.FechaInicio = "";
                }

                var FecHasta = grid.rules[1].data;

                if (FecHasta != null)
                {
                    objReporte.FechaFin = FecHasta;
                }
                else
                {
                    objReporte.FechaFin = "";
                }

                var Sede = grid.rules[2].data;

                if (Sede != "0" && Sede != null)
                {
                    objReporte.idSede = Convert.ToInt32(Sede);
                }
                else
                {
                    objReporte.idSede = 0;
                }

                var idAnalista = grid.rules[3].data;

                if (idAnalista != null && idAnalista != "0")
                {
                    objReporte.idAnalistaResp = Convert.ToInt32(idAnalista);
                }
                else
                {
                    objReporte.idAnalistaResp = 0;
                }

                


                Session[ConstanteSesion.ReporteResumen] = objReporte;


                listaReporte = _solReqPersonalRepository.GetListaReporteResumen(objReporte);

                var generic = GetListar(listaReporte,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = (item.IdeSolReqpersonal),
                    cell = new string[]
                            {
                                item.idAnalistaResp==null?"":item.idAnalistaResp.ToString(),
                                item.AnalistaResp==null?"":item.AnalistaResp,
                                item.Saldo==null?"":item.Saldo.ToString(),
                                item.CantVacPubNuevo==null?"":item.CantVacPubNuevo.ToString(),
                                item.CantVacPubReemplazo==null?"":item.CantVacPubReemplazo.ToString(),
                                item.CantVacPubAmpliacion==null?"":item.CantVacPubAmpliacion.ToString(),
                                item.CantVacFinalNo==null?"":item.CantVacFinalNo.ToString(),
                                item.CantVacFinalSi==null?"":item.CantVacFinalSi.ToString(),
                                item.Total==null?"":item.Total.ToString()
                            }
                }).ToArray();

                return Json(generic.Value);
                //return null;

            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }



        /// <summary>
        /// genera el reporte en formato excel
        /// </summary>
        [ValidarSesion]
        public void ExcelReporteResumeRQ()
        {

            if (Session[ConstanteSesion.ReporteResumen] != null)
            {
                Reporte objReporte = null;

                objReporte = (Reporte)Session[ConstanteSesion.ReporteResumen];
              
                List<Reporte> ListaReporte = _solReqPersonalRepository.GetListaReporteResumen(objReporte);
              


                string fileName = System.Guid.NewGuid().ToString().Replace("-", "") + ".xls";
                string pathApliacion = Server.MapPath(".");
                ReporteExcelv2 objGeneraExcel = new ReporteExcelv2();
                ICellStyle styleTitulo, styleCadena, styleNegrita, styleNumero;

                objGeneraExcel.creaHoja("pagina 01", "S");

                //agrega los estilos
                styleTitulo = objGeneraExcel.addEstiloTitulo(true, 14, "CENTER");
                styleCadena = objGeneraExcel.addEstiloCadena(false, 10, "LEFT");
                styleNegrita = objGeneraExcel.addEstiloCadenaNegrita(10, "LEFT");
                styleNumero = objGeneraExcel.addEstiloNumero(false, 10, "RIGHT");

                DateTime fecha = DateTime.Now;

                //obtiene la ruta de la imagen del logo
                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = "\\Content\\images\\logo_san_pablo_png.png";
                string nombreTemporalArchivo = Guid.NewGuid().ToString();
                string fullPath = applicationPath + directoryPath;
                string dir = fullPath;

                //numero de columnas excel
                int cantCol = 8;

                //adiciona el titulo excel
                objGeneraExcel.addTituloExcel(1, 1, 1, cantCol, "Resumen de Requerimientos", styleTitulo);

                //adiciona la imagen
                objGeneraExcel.AdicionaLogoSanPablo(dir, 1, 2, 0, 4);

                //Se crea un contador de filas que debe ir auto incrementandose si crean nuevas filas


                int Fila = 2;
                IRow row;

                row = objGeneraExcel.addFila(Fila++);

                objGeneraExcel.addCelda(row, cantCol - 1, "Fecha :", styleCadena, "S");
                objGeneraExcel.addCelda(row, cantCol, fecha.ToString("dd/MM/yyyy"), styleCadena, "S");

                row = objGeneraExcel.addFila(Fila++);
                objGeneraExcel.addCelda(row, cantCol - 1, "Hora :", styleCadena, "S");
                objGeneraExcel.addCelda(row, cantCol, fecha.ToString("HH:mm:ss tt"), styleCadena, "S");

                row = objGeneraExcel.addFila(Fila++);
                objGeneraExcel.addCelda(row, cantCol - 1, "Usuario :", styleCadena, "S");
                objGeneraExcel.addCelda(row, cantCol, UsuarioActual.NombreUsuario, styleCadena, "S");
                objGeneraExcel.addCelda(row, 1, "Sistema de Reclutamiento de Personal", styleNegrita, "S");

                row = objGeneraExcel.addFila(Fila++);
               

                // se define la cabecera
                List<string> lista = new List<string>();

                lista.Add("PROFESIONAL");
                lista.Add("SALDO A LA FECHA");
                lista.Add("REQUERIMIENTOS DE NUEVO CARGO");
                lista.Add("REQUERIMIENTOS DE REEMPLAZO");
                lista.Add("REQUERIMIENTOS DE AMPLIACIÓN");
                lista.Add("REQUERIMIENTOS FINALIZADOS A NIVEL PARCIAL O TOTAL");
                lista.Add("PUESTOS CUBIERTOS");
                lista.Add("NUEVO SALDO A LA FECHA");
              
                int colCab = 0;
                Fila += 2;
                row = objGeneraExcel.addFila(Fila);
                foreach (String item in lista)
                {
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, item, styleNegrita, "S");
                }

                //imprime detalle
                colCab = 0;
                Fila += 1;
                foreach (Reporte ItemReporte in ListaReporte)
                {

                    row = objGeneraExcel.addFila(Fila++);
                    colCab = 0;
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.AnalistaResp, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Saldo.ToString(), styleCadena, "N");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.CantVacPubNuevo.ToString(), styleCadena, "N");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.CantVacPubReemplazo.ToString(), styleCadena, "N");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.CantVacPubAmpliacion.ToString(), styleCadena, "N");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.CantVacFinalNo.ToString(), styleCadena, "N");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.CantVacFinalSi.ToString(), styleCadena, "N");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Total.ToString(), styleCadena, "N");
                    
                }


                MemoryStream exportData = new MemoryStream();
                using (exportData)
                {
                    exportData = objGeneraExcel.imprimeExcel(exportData);
                    string saveAsFileName = string.Format("Reporte Seleccion-{0:d}.xls", DateTime.Now).Replace("/", "-");

                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                    Response.Clear();
                    Response.BinaryWrite(exportData.GetBuffer());
                    Response.End();

                }
            }
        }


        /// <summary>
        /// obtiene el reporte PDF
        /// </summary>
        /// <returns></returns>
        public ActionResult GetReportePDF()
        {
            JsonMessage objJsonMessage = new JsonMessage();
            string fullPath = null;
            ReportDocument rep = new ReportDocument();
            MemoryStream mem;

            Reporte objReporte = new Reporte();

            try
            {
                objReporte = (Reporte)Session[ConstanteSesion.ReporteResumen];

                DataTable dtResultado = _solReqPersonalRepository.ListaReporteResumen(objReporte);

                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = ConfigurationManager.AppSettings["ReportIntranetPath"];
                string nomReporte = "ReporteResumen.rpt";
                fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, nomReporte));

                rep.Load(fullPath);
                rep.Database.Tables["ResumenRQ"].SetDataSource(dtResultado);
                
                ParameterValues values1 = new ParameterValues();
                ParameterDiscreteValue discretevalue = new ParameterDiscreteValue();
                
                string NombUsuario = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                discretevalue.Value = NombUsuario;
                values1.Add(discretevalue);


                rep.DataDefinition.ParameterFields["usuario_sesion"].ApplyCurrentValues(values1);



                mem = (MemoryStream)rep.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            }
            catch (Exception)
            {
                return MensajeError();
            }
            return File(mem, "application/pdf");

        }




    }
}