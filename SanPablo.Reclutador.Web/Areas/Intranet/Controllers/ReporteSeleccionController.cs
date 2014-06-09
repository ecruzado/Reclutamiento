

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

            //accesos por roles

            int rolUsuario = (Session[ConstanteSesion.Rol]==null?0:Convert.ToInt32(Session[ConstanteSesion.Rol]));

            int SedeUSuario = (Session[ConstanteSesion.Sede]==null?0:Convert.ToInt32(Session[ConstanteSesion.Sede]));

            int IdUsuario =(Session[ConstanteSesion.Usuario]==null?0:Convert.ToInt32(Session[ConstanteSesion.Usuario]));


           // se obtiene los datos del usuario

            SedeNivel objSedeNivel = (SedeNivel)Session[Core.ConstanteSesion.UsuarioSede];

            objReporteModel.CampoDependencia = Visualicion.SI;
            objReporteModel.CampoDepartamento = Visualicion.SI;
            objReporteModel.CampoArea = Visualicion.SI;

            if (rolUsuario>0)
            {
                if (Roles.Encargado_Seleccion.Equals(rolUsuario))
                {
                    objReporteModel.ReporteSol.idSede = SedeUSuario;
                  
                    objReporteModel.CampoSede = Visualicion.NO;
                    objReporteModel.CampoAnalistaSeleccion = Visualicion.SI;
                }
                else if (Roles.Analista_Seleccion.Equals(rolUsuario))
                {
                    objReporteModel.ReporteSol.idSede = SedeUSuario;
                    objReporteModel.ReporteSol.idAnalistaResp = IdUsuario;

                    objReporteModel.CampoSede = Visualicion.NO;
                    objReporteModel.CampoAnalistaSeleccion = Visualicion.NO;
                    
                }
                else if (Roles.Consultor.Equals(rolUsuario))
                {
                    objReporteModel.CampoSede = Visualicion.SI;
                    objReporteModel.CampoAnalistaSeleccion = Visualicion.SI;

                    
                }
                else if (Roles.Administrador_Sistema.Equals(rolUsuario))
                {
                    objReporteModel.CampoSede = Visualicion.SI;
                    objReporteModel.CampoAnalistaSeleccion = Visualicion.SI;

                }
                else if (Roles.Jefe.Equals(rolUsuario))
                {
                    objReporteModel.ReporteSol.idSede = SedeUSuario;
                    objReporteModel.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                      && x.IdeSede == SedeUSuario));
                    objReporteModel.ReporteSol.idDependencia = objSedeNivel.IDEDEPENDENCIA;
                    
                    objReporteModel.ListaDepartamento = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == objSedeNivel.IDEDEPENDENCIA));
                    objReporteModel.ListaDepartamento.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });
                    objReporteModel.ReporteSol.idDepartamento = objSedeNivel.IDEDEPARTAMENTO;
                    objReporteModel.ListaArea = new List<SanPablo.Reclutador.Entity.Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == objSedeNivel.IDEDEPARTAMENTO));
                    objReporteModel.ListaArea.Add(new SanPablo.Reclutador.Entity.Area { IdeArea = 0, NombreArea = "Seleccionar" });

                    objReporteModel.ReporteSol.idArea = objSedeNivel.IDEAREA;

                    objReporteModel.CampoDependencia = Visualicion.NO;
                    objReporteModel.CampoDepartamento = Visualicion.NO;
                    objReporteModel.CampoArea = Visualicion.NO;


                    objReporteModel.CampoSede = Visualicion.NO;
                    objReporteModel.CampoAnalistaSeleccion = Visualicion.SI;

                }
                else if (Roles.Gerente.Equals(rolUsuario))
                {
                    objReporteModel.ReporteSol.idSede = SedeUSuario;
                    objReporteModel.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                      && x.IdeSede == SedeUSuario));
                    objReporteModel.ReporteSol.idDependencia = objSedeNivel.IDEDEPENDENCIA;

                    objReporteModel.ListaDepartamento = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == objSedeNivel.IDEDEPENDENCIA));
                    objReporteModel.ListaDepartamento.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });
                    objReporteModel.ReporteSol.idDepartamento = objSedeNivel.IDEDEPARTAMENTO;
                    objReporteModel.ListaArea = new List<SanPablo.Reclutador.Entity.Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == objSedeNivel.IDEDEPARTAMENTO));
                    objReporteModel.ListaArea.Add(new SanPablo.Reclutador.Entity.Area { IdeArea = 0, NombreArea = "Seleccionar" });

                    objReporteModel.ReporteSol.idArea = objSedeNivel.IDEAREA;

                    objReporteModel.CampoDependencia = Visualicion.NO;
                    objReporteModel.CampoDepartamento = Visualicion.NO;
                    objReporteModel.CampoArea = Visualicion.NO;


                    objReporteModel.CampoSede = Visualicion.NO;
                    objReporteModel.CampoAnalistaSeleccion = Visualicion.SI;

                }




                else {
                    objReporteModel.CampoSede = Visualicion.SI;
                    objReporteModel.CampoAnalistaSeleccion = Visualicion.SI;
                }
            }
            

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
           
            objModel.listaDependencia = new List<Dependencia>();
            objModel.listaDependencia.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            objModel.ListaDepartamento = new List<Departamento>();
            objModel.ListaDepartamento.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            objModel.ListaArea = new List<SanPablo.Reclutador.Entity.Area>();
            objModel.ListaArea.Add(new SanPablo.Reclutador.Entity.Area { IdeArea = 0, NombreArea = "Seleccionar" });

            int rolUsuario = (Session[ConstanteSesion.Rol] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Rol]));
            int SedeUSuario = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));


            if (Roles.Encargado_Seleccion.Equals(rolUsuario))
            {

                SolReqPersonal objSol = new SolReqPersonal();
                objSol.IdeSede = SedeUSuario;

                objModel.ListaAnalistaResp = new List<Usuario>(_usuarioRepository.GetAnalistaRespoanble(objSol));
                objModel.ListaAnalistaResp.Add(new Usuario { IdUsuario = 0, NombreUsuario = "Seleccionar" });
                
            //     objModel.listaDependencia = new List<Dependencia>();
            //objModel.listaDependencia.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

                objModel.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                     && x.IdeSede == SedeUSuario));

                objModel.listaDependencia.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });



            }
            else if (Roles.Analista_Seleccion.Equals(rolUsuario))
            {
                SolReqPersonal objSol = new SolReqPersonal();
                objSol.IdeSede = SedeUSuario;

                objModel.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                    && x.IdeSede == SedeUSuario));
                objModel.listaDependencia.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });


                objModel.ListaAnalistaResp = new List<Usuario>(_usuarioRepository.GetAnalistaRespoanble(objSol));
                objModel.ListaAnalistaResp.Add(new Usuario { IdUsuario = 0, NombreUsuario = "Seleccionar" });
            }
            else
            {
                objModel.ListaAnalistaResp = new List<Usuario>();
                objModel.ListaAnalistaResp.Add(new Usuario { IdUsuario = 0, NombreUsuario = "Seleccionar" });
            }




         
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

            foreach (Departamento item in listaResultado)
            {
                item.Dependencia = null;
            }
            
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

            foreach (SanPablo.Reclutador.Entity.Area item in listaResultado)
            {
                item.Departamento = null;
            }
            
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

                string indBusqueda = (grid.rules[10].data == null ? "N" : grid.rules[10].data);

                //if (Indicador.Si.Equals(indBusqueda))
                //{
                //    listaReporte = _solReqPersonalRepository.GetListaReporteSeleccion(objReporte);
                //}
                listaReporte = _solReqPersonalRepository.GetListaReporteSeleccion(objReporte);
                Session[ConstanteSesion.ReporteSeleccion] = objReporte;


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
                                item.Dias.ToString() ==null?"":item.Dias.ToString(),
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
        [ValidarSesion]
        public void ListaReporteSeleccion()
        {

            if (Session[ConstanteSesion.ReporteSeleccion] != null)
            {
                Reporte objReporte = null;

                objReporte = (Reporte)Session[ConstanteSesion.ReporteSeleccion];
                int cont = 0;
                // se obtiene los datos de la lista
                List<Reporte> ListaReporte = _solReqPersonalRepository.GetListaReporteSeleccion(objReporte);
                string Promedio = "";
                Int32 TotalDias = ListaReporte.Where(x => x.Dias != 0).ToList().Count();

                
                Int32 SumDias = ListaReporte.Where(x => x.Dias != 0).ToList().Sum(x => x.Dias);

               

                if (TotalDias > 0 && SumDias > 0)
                {
                    Promedio = String.Format("{0:0.##}", (SumDias / TotalDias));
                }
                
                
                string fileName = System.Guid.NewGuid().ToString().Replace("-", "") + ".xls";
                string pathApliacion = Server.MapPath(".");
                ReporteExcelv2 objGeneraExcel = new ReporteExcelv2();
                ICellStyle styleTitulo, styleCadena, styleNegrita, styleNumero;

                objGeneraExcel.creaHoja("pagina 01","S");

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
                string fullPath = applicationPath+directoryPath;
                string dir = fullPath;

                //numero de columnas excel
                int cantCol = 21;

                //adiciona el titulo excel
                objGeneraExcel.addTituloExcel(1, 1, 1, cantCol, "REPORTE DE SELECCION", styleTitulo);

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


                objGeneraExcel.addCelda(row, (cantCol / 2) , "Desde: ", styleCadena,"S");
                objGeneraExcel.addCelda(row, (cantCol / 2)+1, objReporte.FechaInicio, styleCadena,"S");
                objGeneraExcel.addCelda(row, (cantCol / 2) + 2, "Hasta: ", styleCadena, "S");
                objGeneraExcel.addCelda(row, (cantCol / 2)+3, objReporte.FechaFin, styleCadena, "S");

                //Fila++;
                //row = objGeneraExcel.addFila(Fila++);
                //objGeneraExcel.addCelda(row, 1, "Promedio de atención: "+Promedio, styleNegrita, "S");

                // se define la cabecera
                List<string> lista = new List<string>();

                lista.Add("ESTADO DEL PROCESO");
                lista.Add("FECHA DE PUBLICACION");
                lista.Add("SEDE");
                lista.Add("DEPENDENCIA");
                lista.Add("DEPARTAMENTO");
                lista.Add("AREA");
                lista.Add("PUESTO");
                lista.Add("JEFE");
                lista.Add("TIPO DE REQUERIMIENTO");
                lista.Add("REEMPAZA A");
                lista.Add("F. CESE o REEMPLAZO");
                lista.Add("MOTIVO DE REEMPLAZO");
                lista.Add("ANALISTA RESPONSABLE");
                lista.Add("P. INGRESA (APELLIDOS Y NOMBRE)");
                lista.Add("FECHA DE CONTRATACION");
                lista.Add("TIEMPO ESPERA (DIAS)");
                lista.Add("DNI");
                lista.Add("CEL. / FIJO");
                lista.Add("OBSERVACIONES DEL PSICOLOGO");
                lista.Add("OBSERVACIONES DE LA ENTREVISTA");
                lista.Add("MOTIVO DE FINALIZACION DEL REQ.");

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

                int posColumDias = 0;
                foreach (Reporte ItemReporte in ListaReporte)
                {
                    colCab = 0;
                    row = objGeneraExcel.addFila(Fila++);
                    
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.EstadoProceso, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.FechaRequerimiento, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.DesSede, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.DesDependencia, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.DesDepartamento, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.DesArea, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Cargo, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Jefe, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Tipsol, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Reemplaza, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.FecReemplazo, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.MotivoReemplazo, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.AnalistaResp, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.PersonaIngresa, styleCadena, "S");

                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.FechaContratacion, styleCadena, "S");
                    
                    colCab++;
                    posColumDias = colCab;
                    
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Dias.ToString(), styleCadena, "N");
                    
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Numdocumento, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.Fono, styleCadena, "S");
                    
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.ObsPsicologo, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.ObsEntrevista, styleCadena, "S");
                    colCab++;
                    objGeneraExcel.addCelda(row, colCab, ItemReporte.MotivoCirreSol, styleCadena, "S");
                }

                if (ListaReporte!=null)
                {
                    row = objGeneraExcel.addFila(Fila++);
                    objGeneraExcel.addCelda(row, posColumDias, Promedio, styleCadena, "S");
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
                objReporte = (Reporte)Session[ConstanteSesion.ReporteSeleccion];

                DataTable dtResultado = _solReqPersonalRepository.ListaReporteSeleccion(objReporte);

                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = ConfigurationManager.AppSettings["ReportIntranetPath"];
                string nomReporte = "ReporteSeleccion.rpt";
                fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, nomReporte));

                rep.Load(fullPath);
                rep.Database.Tables["DtReporteSeleccion"].SetDataSource(dtResultado);

                ParameterValues values1 = new ParameterValues();
                ParameterValues valFechaDesde = new ParameterValues();
                ParameterValues valFechaHasta = new ParameterValues();

                ParameterDiscreteValue discretevalue = new ParameterDiscreteValue();

                ParameterDiscreteValue discretevalueFechaDesde = new ParameterDiscreteValue();
                ParameterDiscreteValue discretevalueFechaHasta = new ParameterDiscreteValue();


                string NombUsuario = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                discretevalue.Value = NombUsuario;
                values1.Add(discretevalue);

                discretevalueFechaDesde.Value = objReporte.FechaInicio;
                discretevalueFechaHasta.Value = objReporte.FechaFin;

                valFechaDesde.Add(discretevalueFechaDesde);
                valFechaHasta.Add(discretevalueFechaHasta);

                rep.DataDefinition.ParameterFields["usuario_sesion"].ApplyCurrentValues(values1);

                rep.DataDefinition.ParameterFields["FechaDesde_sesion"].ApplyCurrentValues(valFechaDesde);
                rep.DataDefinition.ParameterFields["FechaHasta_sesion"].ApplyCurrentValues(valFechaHasta);



                mem = (MemoryStream)rep.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            }
            catch (Exception ex)
            {
                return MensajeError(ex.Message + fullPath);
            }
            return File(mem, "application/pdf");

        }



    }
}
