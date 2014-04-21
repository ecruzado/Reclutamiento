

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

    
    public class ReporteSeleccionController : BaseController
    {
        

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

        public ReporteSeleccionController(IDetalleGeneralRepository detalleGeneralRepository,
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
        /// inicializa la ventana de reporte de seleccion
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


            return View("Index",objReporteModel);
        }


        private ReporteViewModel Inicializar()
        {
            var objModel = new ReporteViewModel();

            var idSede = Session[ConstanteSesion.Sede];

            // motivo de reemplazo
            objModel.ListaMotivo =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVacante));
            objModel.ListaMotivo.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            objModel.ListaSede = new List<Sede>(_sedeRepository.GetByTipSede());
            objModel.ListaSede.Insert(0, new Sede { CodigoSede = "0", DescripcionSede = "TODOS" });


            objModel.ListaTipoSol =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSolicitud));
            objModel.ListaTipoSol.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            List<DetalleGeneral> listaSolicitud = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapa));
            objModel.ListaEstadoReq = listaSolicitud.Where(x => x.Valor == "04" || x.Valor == "08").ToList();
            objModel.ListaEstadoReq.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            //new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapaSolicitud));
            //objModel.ListaEstadoReq.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            
            //objModel.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
            //                                                             && x.IdeSede == UsuarioSede.IDESEDE));
            objModel.listaDependencia = new List<Dependencia>();
            objModel.listaDependencia.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            objModel.ListaDepartamento = new List<Departamento>();
            objModel.ListaDepartamento.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            objModel.ListaArea = new List<Area>();
            objModel.ListaArea.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            objModel.ListaAnalistaResp = new List<Usuario>();
            objModel.ListaAnalistaResp.Add(new Usuario { IdUsuario = 0, NombreUsuario = "Seleccionar" });

            return objModel;
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

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));
            result = Json(listaResultado);
            return result;
        }


        /// <summary>
        /// obtiene la lista de contratados
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getListContratados(GridTable grid)
        {

            Reporte objReporte;
            List<Reporte> listaReporte = new List<Reporte>();
            try
            {

                objReporte = new Reporte();

              
                var FecDesde = grid.rules[0].data;

                if (FecDesde!=null)
                {
                    objReporte.FechaInicio = FecDesde;
                }
                else
                {
                    objReporte.FechaInicio = "";
                }

                var FecHasta = grid.rules[1].data;

                if (FecHasta!=null)
                {
                    objReporte.FechaFin = FecHasta;
                }
                else
                {
                    objReporte.FechaFin = "";
                }

                var TipSol = grid.rules[2].data;

                if (TipSol!="0" && TipSol!=null)
                {
                    objReporte.Tipsol = TipSol;
                }
                else
                {
                    objReporte.Tipsol = "";
                }

                var EstadoReq = grid.rules[3].data;

                if (EstadoReq!=null && EstadoReq!="0")
                {
                    objReporte.EstadoProceso = EstadoReq;
                }
                else
                {
                    objReporte.EstadoProceso = "";
                }

                var IdResp = grid.rules[4].data;
                if (IdResp != null && IdResp!="0")
                {
                    objReporte.idAnalistaResp = Convert.ToInt32(IdResp);
                }
                else
                {
                    objReporte.idAnalistaResp = 0;
                }

                var IdDependencia = grid.rules[5].data;

                if (IdDependencia!=null && IdDependencia!="0")
                {
                    objReporte.idDependencia = Convert.ToInt32(IdDependencia);
                }
                else
                {
                    objReporte.idDependencia = 0;     
                }

                var IdDepartamento = grid.rules[6].data;

                if (IdDepartamento!=null && IdDepartamento!="0")
                {
                    objReporte.idDepartamento = Convert.ToInt32(IdDepartamento);
                }
                else
                {
                    objReporte.idDepartamento = 0;
                }

                var IdArea = grid.rules[7].data;

                if (IdArea!=null && IdArea!="0")
                {
                    objReporte.idArea = Convert.ToInt32(IdArea);
                }
                else
                {
                    objReporte.idArea = 0;
                }

                var MotivoReemplazo = grid.rules[8].data;


                if (MotivoReemplazo != null && MotivoReemplazo!="0")
                {
                    objReporte.MotivoReemplazo = MotivoReemplazo;
                }
                else
                {
                    objReporte.MotivoReemplazo = "";
                }

                var Sede = grid.rules[9].data;
                if (Sede!=null && Sede!="0")
                {
                    objReporte.idSede = Convert.ToInt32(Sede);
                }else
	            {
                    objReporte.idSede = 0;
	            }

                


                  //{ field: 'cDesde', data: $("#ReporteSol_FechaInicio").val() },
                  //     { field: 'cHasta', data: $("#ReporteSol_FechaFin").val() },
                  //     { field: 'cTipSol', data: $("#ReporteSol_idTipSol").val() },
                  //     { field: 'cEstadoReq', data: $("#ReporteSol_idEstadoReq").val() },
                  //     { field: 'nIdResp', data: $("#ReporteSol_idAnalistaResp").val() },
                  //     { field: 'nIdDependencia', data: $("#ReporteSol_idDependencia").val() },
                  //     { field: 'nIdDepartamento', data: $("#ReporteSol_idDepartamento").val() },
                  //     { field: 'nIdArea', data: $("#ReporteSol_idArea").val() },
                  //     { field: 'cMotivoreemp', data: $("#ReporteSol_idMotivoReemplazo").val() }


                //if ("0".Equals(reclutamientoPersona.EstPostulante))
                //{
                //    reclutamientoPersona.EstPostulante = "";
                //}

                

                Session[ConstanteSesion.ReporteSeleccion] = objReporte;


                listaReporte = _solReqPersonalRepository.GetListaReporteSeleccion(objReporte);

                var generic = GetListar(listaReporte,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = (item.IdeSolReqpersonal),
                    cell = new string[]
                            {
                                item.IdeSolReqpersonal==null?"":item.IdeSolReqpersonal,
                                item.EstadoProceso==null?"":item.EstadoProceso,
                                item.FechaRequerimiento==null?"":item.FechaRequerimiento,
                                item.DesSede==null?"":item.DesSede,
                                item.DesDependencia==null?"":item.DesDependencia,
                                item.DesDepartamento==null?"":item.DesDepartamento,
                                item.DesArea==null?"":item.DesArea,
                                item.Cargo==null?"":item.Cargo,
                                item.Jefe==null?"":item.Jefe,
                                item.Tipsol==null?"":item.Tipsol,
                                item.Reemplaza==null?"":item.Reemplaza,
                                item.FecReemplazo==null?"":item.FecReemplazo,
                                item.MotivoReemplazo ==null?"":item.MotivoReemplazo,
                                item.AnalistaResp ==null?"":item.AnalistaResp,
                                item.PersonaIngresa ==null?"":item.PersonaIngresa,
                                item.FechaContratacion ==null?"":item.FechaContratacion,
                                item.Dias ==null?"":item.Dias,
                                item.Numdocumento ==null?"":item.Numdocumento,
                                item.Fono ==null?"":item.Fono,
                                item.ObsPsicologo ==null?"":item.ObsPsicologo,
                                item.ObsEntrevista ==null?"":item.ObsEntrevista,
                                item.FecSuceso ==null?"":item.FecSuceso,
                                item.MotivoCirreSol==null?"":item.MotivoCirreSol

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
        public void ListaReporteSeleccion()
        {

            if (Session[ConstanteSesion.ReporteSeleccion] != null)
            {
                Reporte objReporte = null;

                objReporte = (Reporte)Session[ConstanteSesion.ReporteSeleccion];

                DataTable dtReporteSeleccion = _solReqPersonalRepository.ListaReporteSeleccion(objReporte);


                string fileName = System.Guid.NewGuid().ToString().Replace("-", "") + ".xls";
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
               
                //numero de columnas excel
                int cantCol = 21;

                objGeneraExcel.addTituloExcel(1, 1, 1, cantCol, "Reporte de Selección", styleTitulo);

                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 1, "ESTADO DEL PROCESO", 1);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 2, "FECHA DE REQUERIMIENTO", 2);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 3, "SEDE", 3);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 4, "DEPENDENCIA", 4);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 5, "DEPARTAMENTO", 5);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 6, "AREA", 6);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 7, "PUESTO", 7);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 8, "JEFE", 8);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 9, "TIPO DE REQUERIMIENTO", 9);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 10, "REEMPAZA A", 10);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 11, "F. CESE o REEMPLAZO", 11);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 12, "MOTIVO DE REEMPLAZO", 12);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 13, "ANALISTA RESPONSABLE", 13);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 14, "P. INGRESA (APELLIDOS Y NOMBRE)", 14);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 15, "FECHA DE CONTRATACION", 15);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 16, "TIEMPO ESPERA (DIAS)", 16);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 17, "DNI", 17);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 18, "CEL. / FIJO", 18);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 19, "OBSERVACIONES DEL PSICOLOGO", 19);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 20, "OBSERVACIONES DE LA ENTREVISTA", 20);
                objGeneraExcel.addDetalleLista(dtReporteSeleccion, 22, "MOTIVO DE FINALIZACION DEL REQ.", 21);


                // Se coloca de manera obligatoria
                objGeneraExcel.imprimirCabecera(5, styleNegrita);
                objGeneraExcel.imprimiDetalle(6, styleCadena);


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


    }
}
