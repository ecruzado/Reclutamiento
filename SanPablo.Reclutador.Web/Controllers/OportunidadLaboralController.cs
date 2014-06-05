

namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models;
    using System.Collections.Generic;
    using SanPablo.Reclutador.Web.Core;
    using System.Web.Mvc;
    using System.Web;
    using System.Linq;
    using System.Web.Services;
    using SanPablo.Reclutador.Entity.Validation;
    using FluentValidation.Results;
    using FluentValidation;
    using System.IO;
    using System;
    using System.Drawing;
    using System.Web.Routing;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using NHibernate.Criterion;
    
    public class OportunidadLaboralController : BaseController
    {
        
        
        /// <summary>
        /// Se inicializa los repositorios de base de datos
        /// </summary>
        private IExperienciaCargoRepository _experienciaCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IAreaRepository _areaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private ITipoRequerimiento _tipoRequerimiento;
        private ICargoRepository _cargoRepository;
        private IConocimientoGeneralRequerimientoRepository _ConocimientoGeneralRequerimientoRepository;
        private IConocimientoGeneralCargoRepository _conocimientoCargoRepository;


        private INivelAcademicoRequerimientoRepository _nivelAcademicoRequerimientoRepository;
        private ICompetenciaRequerimientoRepository _competenciaRequerimientoRepository;
        private IExperienciaRequerimientoRepository _experienciaRequerimientoRepository;
        private IOfrecemosRequerimientoRepository _ofrecemosRequerimientoRepository;
        private IUsuarioRepository _usuarioRepository;
        private ISedeRepository _sedeRepository;
        private IPostulanteRepository _postulanteRepository;
        private INivelAcademicoCargoRepository _nivelAcademicoCargoRepository;
        private ICompetenciaCargoRepository _competenciaCargoRepository;
        private IOfrecemosCargoRepository _ofrecemosCargoRepository;
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IExperienciaPostulanteRepository _experienciaPostulanteRepository;
        private IConocimientoGeneralPostulanteRepository _conocimientoGeneralPostulanteRepository;
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;


        public OportunidadLaboralController(IDetalleGeneralRepository detalleGeneralRepository,
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
            ISedeRepository sedeRepository,
            IPostulanteRepository postulanteRepository,
            INivelAcademicoCargoRepository nivelAcademicoCargoRepository,
            IExperienciaCargoRepository experienciaCargoRepository,
            ICompetenciaCargoRepository competenciaCargoRepository,
            IOfrecemosCargoRepository ofrecemosCargoRepository,
            IEstudioPostulanteRepository estudioPostulanteRepository,
            IExperienciaPostulanteRepository experienciaPostulanteRepository,
            IConocimientoGeneralPostulanteRepository conocimientoGeneralPostulanteRepository,
            ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
            IConocimientoGeneralCargoRepository conocimientoCargoRepository
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
            _postulanteRepository = postulanteRepository;
            _nivelAcademicoCargoRepository = nivelAcademicoCargoRepository;
            _experienciaCargoRepository = experienciaCargoRepository;
            _competenciaCargoRepository = competenciaCargoRepository;
            _ofrecemosCargoRepository = ofrecemosCargoRepository;
            _experienciaPostulanteRepository = experienciaPostulanteRepository;
            _conocimientoGeneralPostulanteRepository = conocimientoGeneralPostulanteRepository;
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _conocimientoCargoRepository = conocimientoCargoRepository;
        }


        /// <summary>
        /// Realiza la validacion si tiene permisos para acceder a la pagina
        /// </summary>
        /// <returns></returns>
        public RouteValueDictionary verificaLogeo()
        {

            var myListOp = (List<SanPablo.Reclutador.Entity.MenuItem>)Session["ListaMenu"];

            string rutaAbsoluta = (Request.Path).ToUpper();

            int indWeb = rutaAbsoluta.IndexOf("/INTRANET/");
            var tieneAcceso = myListOp.Where(x => x.DSCURL == Request.Path).ToList();


            if (tieneAcceso != null)
            {
                return null;
            }
            else
            {
                if (indWeb != -1)
                {
                    //intranet
                    var routeValues = new RouteValueDictionary();
                    routeValues["controller"] = "Seguridad";
                    routeValues["action"] = "Login";
                    routeValues["area"] = "Intranet";
                    return routeValues;

                }
                else
                {
                    //extranet
                    var routeValues = new RouteValueDictionary();
                    routeValues["controller"] = "Seguridad";
                    routeValues["action"] = "Login";
                    return routeValues;

                }
            }
        }
        
        
        /// <summary>
        /// pagina Inicial de Oportunidades laborales
        /// </summary>
        /// <returns></returns>
        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Index()
        {

            RouteValueDictionary retorno = new RouteValueDictionary();
            retorno = verificaLogeo();

            if (retorno != null)
            {
                return RedirectToRoute(retorno);
            }

            OportunidadLaboralViewModel model = new OportunidadLaboralViewModel();
            
            model = InicializarListas();
            model.oportunidadLaboral = new OportunidadLaboral();



            return View("Index", model);
        }

        /// <summary>
        /// Inicializa las listas de la pagina oportunidades laborales
        /// </summary>
        /// <returns></returns>
        public OportunidadLaboralViewModel InicializarListas()
        {
            var model = new OportunidadLaboralViewModel();

           
           
            model.oportunidadLaboral = new OportunidadLaboral();
            model.oportunidadLaboral.IdeSede = 0;
            
            model.listaOportunidadLaboral = new List<OportunidadLaboral>(_postulanteRepository.GetCargosPublicados(model.oportunidadLaboral));
            
            //model.listaOportunidadLaboral = new List<OportunidadLaboral>();
            model.listaOportunidadLaboral.Insert(0, new OportunidadLaboral { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.listaHorario =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.listaHorario.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.listaSede = new List<Sede>(_sedeRepository.GetBy(x => x.EstadoRegistro == IndicadorActivo.Activo));
            model.listaSede.Insert(0, new Sede { CodigoSede = "0", DescripcionSede = "Seleccionar" });

            return model;
        }

        /// <summary>
        /// lista de oportunidades laborales por sede
        /// </summary>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaOportunidades(int ideSede)
        {
            ActionResult result = null;

            OportunidadLaboral objOportinudadLab = new OportunidadLaboral();
            objOportinudadLab.IdeSede = ideSede;

            var listaResultado = new List<OportunidadLaboral>(_postulanteRepository.GetCargosPublicados(objOportinudadLab));

            //foreach (Area item in listaResultado)
            //{
            //    item.Departamento = null;
            //}

            result = Json(listaResultado);
            return result;
        }


        /// <summary>
        /// obtiene las oportunidades laborales
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListOportunidadesLaborales(GridTable grid)
        {

            OportunidadLaboral oportunidadLaboral;
            List<OportunidadLaboral> lista = new List<OportunidadLaboral>();
            try
            {
                
                oportunidadLaboral = new OportunidadLaboral();

                oportunidadLaboral.IdeSede = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
                oportunidadLaboral.TipoHorario = grid.rules[1].data=="0"?"":grid.rules[1].data;
                oportunidadLaboral.IdeCargo = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));

                if (grid.rules[3].data != null && grid.rules[4].data != null)
                {
                    oportunidadLaboral.FecInicial = Convert.ToDateTime(grid.rules[3].data);
                    oportunidadLaboral.FecFinal = Convert.ToDateTime(grid.rules[4].data);
                }

                

                lista = _postulanteRepository.GetObtieneOpurtunidad(oportunidadLaboral);



                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = (item.IdeCargo.ToString() + '/' + item.IdeSede+"/"+item.TipoHorario + "/" + item.FecInicial.ToString()+"/"+item.FecFinal.ToString()),
                    cell = new string[]
                            {
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.IdeSede==null?"":item.IdeSede.ToString(),
                                item.SedeDes==null?"":item.SedeDes,
                                item.CargoDes==null?"":item.CargoDes,
                                item.FecInicial==null?"":String.Format("{0:dd/MM/yyyy}", item.FecInicial),
                                item.FecFinal==null?"":String.Format("{0:dd/MM/yyyy}", item.FecFinal),
                                item.TipoHorario==null?"":item.TipoHorario,
                                item.TipoHorarioDes==null?"":item.TipoHorarioDes,
                                item.NumVacantes.ToString()
                                
                                
                            }
                }).ToArray();

                return Json(generic.Value);

            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }

        /// <summary>
        /// inicializa la pagina del detalle del cargo
        /// </summary>
        /// <param name="idCargo">Id de cargo del grupo</param>
        /// <param name="idSede">Id de Sede del grupo</param>
        /// <param name="fechaInicio">Fecha min de Inicio del grupo</param>
        /// <param name="fechaFina">Fecha max del grupo</param>
        /// <param name="tipo">tipo de puesto</param>
        /// <param name="numVacantes">Numero de vacantes totales</param>
        /// <returns></returns>
        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult inicioDetalleCargo(string id,string idSede,string fechaInicio,string fechaFin,string tipo,string numVacantes)
        {

            OportunidadLaboralViewModel model;
            model = new OportunidadLaboralViewModel();
            model.oportunidadLaboral = new OportunidadLaboral();
            model.solReqPersonal = new SolReqPersonal();

            SolReqPersonal obj;
            obj = new SolReqPersonal();

            model.oportunidadLaboral.IdeCargo = Convert.ToInt32(id);
            model.oportunidadLaboral.IdeSede = Convert.ToInt32(idSede);
            model.oportunidadLaboral.TipoHorario = tipo;

            var objDetalle =    _detalleGeneralRepository.GetSingle(x => x.IdeGeneral==14 && x.Valor == tipo);
            if (objDetalle != null)
            {
                model.oportunidadLaboral.TipoHorarioDes = Convert.ToString(objDetalle.Descripcion);
            }

            model.solReqPersonal = _postulanteRepository.GetDatosSolGrupo(model.oportunidadLaboral);

            if (TipoSolicitud.Nuevo.Equals(tipo))
            {
                var objSol = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeCargo ==Convert.ToInt32(id) && x.EstadoActivo==IndicadorActivo.Activo);
                model.solReqPersonal.IndVerSalario = objSol.IndicadorVerSalario;

                
            }
            else
            {
                var objSolReq = _solReqPersonalRepository.GetSingle(x => x.IdeCargo == Convert.ToInt32(id) && 
                                                                    x.TipoSolicitud==tipo && x.EstadoActivo==IndicadorActivo.Activo);

                if (objSolReq!=null)
                {
                    model.solReqPersonal.IndVerSalario = (objSolReq.IndVerSalario == null ? "N" : objSolReq.IndVerSalario);
                }
                else
                {
                    model.solReqPersonal.IndVerSalario = "N";
                }
                
            }


            model.solReqPersonal.FechaInicioBus = Convert.ToDateTime(fechaInicio);
            model.solReqPersonal.FechaFinBus = Convert.ToDateTime(fechaFin);
            model.solReqPersonal.NumVacantes = (numVacantes == null ? 0 : Convert.ToInt32(numVacantes));
            model.solReqPersonal.IdeCargo = Convert.ToInt32(id);
            

            var objSede=_sedeRepository.GetSingle(x=>x.CodigoSede==idSede);
            model.solReqPersonal.Sede_des = objSede.DescripcionSede;

            


            return View("DetalleGrupoCargo", model);
        }


        /// <summary>
        /// Estudios del grupo del cargo
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Estudios(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            string tipSol = (grid.rules[1].data == null ? "" : grid.rules[1].data);
            int IdeCargo = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;
                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                if (!"N".Equals(tipSol))
                {

                    DetachedCriteria where = DetachedCriteria.For<NivelAcademicoRequerimiento>();
                    where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                    var generic = Listar(_nivelAcademicoRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeNivelAcademicoRequerimiento.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionAreaEstudio,
                            }
                        }).ToArray();

                    return Json(generic.Value);

                }
                else
                {
                    DetachedCriteria where = DetachedCriteria.For<NivelAcademicoCargo>();
                    where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                    var generic = Listar(_nivelAcademicoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeNivelAcademicoCargo.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionAreaEstudio,
                            }
                        }).ToArray();

                    return Json(generic.Value);


                }

                
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }


        /// <summary>
        /// Conomientos Generales del Cargo
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Conocimientos(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            string tipSol = (grid.rules[1].data == null ? "" : grid.rules[1].data);
            int IdeCargo = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;
                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                if (!"N".Equals(tipSol))
                {

                    DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralRequerimiento>();
                    where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));
                   
                    var generic = Listar(_ConocimientoGeneralRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionConocimientoOfimatica,
                                item.DescripcionNombreOfimatica,
                            }
                        }).ToArray();

                    return Json(generic.Value);

                }
                else
                {
                    DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                    where.Add(Expression.IsNotNull("TipoConocimientoOfimatica"));
                    where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                    var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeConocimientoGeneralCargo.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionConocimientoOfimatica,
                                item.DescripcionNombreOfimatica
                            }
                        }).ToArray();

                    return Json(generic.Value);


                }


            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }


        /// <summary>
        /// Idiomas
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Idiomas(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            string tipSol = (grid.rules[1].data == null ? "" : grid.rules[1].data);
            int IdeCargo = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;
                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                if (!"N".Equals(tipSol))
                {

                    DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralRequerimiento>();
                    where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));
                    where.Add(Expression.IsNotNull("TipoIdioma"));

                    var generic = Listar(_ConocimientoGeneralRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionIdioma,
                                item.DescripcionConocimientoIdioma,
                                item.DescripcionNivelConocimiento

                            }
                        }).ToArray();

                    return Json(generic.Value);

                }
                else
                {
                    DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                    where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));
                    where.Add(Expression.IsNotNull("TipoIdioma"));

                    var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeConocimientoGeneralCargo.ToString(),
                            cell = new string[]
                            {

                                item.DescripcionIdioma,
                                item.DescripcionConocimientoIdioma,
                                item.DescripcionNivelConocimiento

                            }
                        }).ToArray();

                    return Json(generic.Value);


                }


            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }


        /// <summary>
        /// Conocimientos Adicionales
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult ConocimientoAdicionales(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            string tipSol = (grid.rules[1].data == null ? "" : grid.rules[1].data);
            int IdeCargo = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;
                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                if (!"N".Equals(tipSol))
                {

                    DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralRequerimiento>();
                    where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));
                    where.Add(Expression.IsNotNull("TipoConocimientoGeneral"));

                    var generic = Listar(_ConocimientoGeneralRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionConocimientoGeneral,
                                item.DescripcionNombreConocimientoGeneral,
                                item.DescripcionNivelConocimiento

                            }
                        }).ToArray();

                    return Json(generic.Value);

                }
                else
                {
                    DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                    where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));
                    where.Add(Expression.IsNotNull("TipoConocimientoGeneral"));

                    var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeConocimientoGeneralCargo.ToString(),
                            cell = new string[]
                            {

                                item.DescripcionConocimientoGeneral,
                                item.DescripcionNombreConocimientoGeneral,
                                item.DescripcionNivelConocimiento

                            }
                        }).ToArray();

                    return Json(generic.Value);


                }


            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }



        /// <summary>
        /// Experiencia
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Experiencia(GridTable grid)
        {

            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            string tipSol = (grid.rules[1].data == null ? "" : grid.rules[1].data);
            int IdeCargo = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));


            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                if (!"N".Equals(tipSol))
                {
                    DetachedCriteria where = null;
                    where = DetachedCriteria.For<ExperienciaRequerimiento>();

                    where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                    var generic = Listar(_experienciaRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeExperienciaRequerimiento.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString() + " AÑO(S) y " +item.CantidadMesesExperiencia.ToString() + " MES(ES)" 
                            }
                        }).ToArray();

                    return Json(generic.Value);
                }
                else
                {
                    DetachedCriteria where = null;
                    where = DetachedCriteria.For<ExperienciaCargo>();

                    where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));


                    var generic = Listar(_experienciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeExperienciaCargo.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString() + " AÑO(S) y " +item.CantidadMesesExperiencia.ToString() + " MES(ES)" 
                            }
                        }).ToArray();

                    return Json(generic.Value);

                }



            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// lista de competencias
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCompetencias(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            string tipSol = (grid.rules[1].data == null ? "" : grid.rules[1].data);
            int IdeCargo = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));

           

            List<CompetenciaRequerimiento> lista = new List<CompetenciaRequerimiento>();
            try
            {
                if (!"N".Equals(tipSol))
                {

                    grid.page = (grid.page == 0) ? 1 : grid.page;

                    grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                    DetachedCriteria where = DetachedCriteria.For<CompetenciaRequerimiento>();
                    where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                    var generic = Listar(_competenciaRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeCompetenciaRequerimiento.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionCompetencia,
                            }
                        }).ToArray();

                    return Json(generic.Value);
                }
                else
                {
                    grid.page = (grid.page == 0) ? 1 : grid.page;

                    grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                    DetachedCriteria where = DetachedCriteria.For<CompetenciaCargo>();
                    where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                    var generic = Listar(_competenciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeCompetenciaCargo.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionCompetencia,
                            }
                        }).ToArray();

                    return Json(generic.Value);
                }



            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// Ofrecemos
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Ofrecemos(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            string tipSol = (grid.rules[1].data == null ? "" : grid.rules[1].data);
            int IdeCargo = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));

            try
            {
                if (!"N".Equals(tipSol))
                {
                    grid.page = (grid.page == 0) ? 1 : grid.page;

                    grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                    DetachedCriteria where = DetachedCriteria.For<OfrecemosRequerimiento>();
                    where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                    var generic = Listar(_ofrecemosRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeOfrecemosRequerimiento.ToString(),
                            cell = new string[]
                                {
                                    item.DescripcionOfrecimiento,
                                
                                }
                        }).ToArray();

                    return Json(generic.Value);
                }
                else
                {
                    grid.page = (grid.page == 0) ? 1 : grid.page;

                    grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                    DetachedCriteria where = DetachedCriteria.For<OfrecemosCargo>();
                    where.Add(Expression.Eq("Cargo.IdeCargo", IdeCargo));

                    var generic = Listar(_ofrecemosCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                    generic.Value.rows = generic.List
                        .Select(item => new Row
                        {
                            id = item.IdeOfrecemosCargo.ToString(),
                            cell = new string[]
                            {
                                item.DescripcionOfrecimiento,
                                
                            }
                        }).ToArray();

                    return Json(generic.Value);
                }
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }


        /// <summary>
        /// Realiza la validacion del postulante
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion]
        public ActionResult validaPostulacion(int id,int idSede,string idTipPuesto) 
        {

            JsonMessage ObjJson = new JsonMessage();
            Usuario usuario = new Usuario();
            OportunidadLaboral objOportunidad;
            string Mensaje = "";

            //int retorno=0;

            int idPostulante;

            var objUsuario = Session[ConstanteSesion.ObjUsuarioExtranet];
            if (objUsuario!=null)
            {
                usuario = (Usuario)objUsuario;
            }

            var objUsuario2 = _usuarioRepository.GetSingle(x => x.IdUsuario == usuario.IdUsuario);

            idPostulante = objUsuario2.IdePostulante;


            if (idPostulante!=null )
            {
                if (idPostulante>0)
                {
                    objOportunidad = new OportunidadLaboral();
                    objOportunidad.IdeCargo = id;
                    objOportunidad.IdPostulante = idPostulante;
                    objOportunidad.IdeSede = idSede;
                    objOportunidad.TipoHorario = idTipPuesto;

                    objOportunidad = _postulanteRepository.ValidaPostulacion(objOportunidad);

                    if (objOportunidad.retorno > 0)
                    {
                        if (objOportunidad.retorno== 1)
                        {
                            Mensaje = "Para postular, es necesario que complete sus datos en la(s) pestaña(s): " + objOportunidad.mensaje;
                        
                            ObjJson.Resultado = false;
                            ObjJson.Mensaje = Mensaje;
                            return Json(ObjJson);

                        }

                        if (objOportunidad.retorno == 2)
                        {
                            ObjJson.Resultado = false;
                            ObjJson.Mensaje = objOportunidad.mensaje;
                            return Json(ObjJson);

                        }
                    }
                }
                else
                {
                    ObjJson.Resultado = false;
                    ObjJson.Mensaje = "Debe registrase para postular";
                    return Json(ObjJson);
                }
               
            }
            else
            {
                ObjJson.Resultado = false;
                ObjJson.Mensaje = "Debe registrase para postular";
                return Json(ObjJson);

            }

            if (objOportunidad.retorno == 0)
            {
                Postulante objPostulante = new Postulante();
                objPostulante.IdCargo=id;
                objPostulante.IdSede = idSede;
                objPostulante.TipoPuesto = idTipPuesto;
                objPostulante.IdePostulante = idPostulante;
                
                _postulanteRepository.Postulacion(objPostulante);
                ObjJson.Mensaje = "Se realizo la postulación";
            }

            return Json(ObjJson);
        }

        /// <summary>
        /// inicializa la lista de mis postulaciones
        /// </summary>
        /// <returns></returns>
        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Postulaciones() 
        {
            OportunidadLaboralViewModel model;
            model = new OportunidadLaboralViewModel();
            model.oportunidadLaboral = new OportunidadLaboral();

            Usuario usuario = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];

            model.oportunidadLaboral.IdPostulante = usuario.IdePostulante;


            return View("Postulaciones", model);

        }


        /// <summary>
        /// obtiene las postulaciones del postulante
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListMisPostulaciones(GridTable grid)
        {

            OportunidadLaboral oportunidadLaboral;
            List<OportunidadLaboral> lista = new List<OportunidadLaboral>();
            try
            {
                //int idPostulante = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));

                var objUsuario = Session[ConstanteSesion.ObjUsuarioExtranet];

                int idPostulante=0;
                Usuario objUsuario2;
                Usuario objUsuario3;
                if (objUsuario!=null)
                {
                    objUsuario2 = new Usuario();
                    objUsuario2 = (Usuario)objUsuario;
                    
                    objUsuario3 = new Usuario();
                    objUsuario3 = _usuarioRepository.GetSingle(x => x.IdUsuario == objUsuario2.IdUsuario && x.TipUsuario==TipUsuario.Extranet);
                    idPostulante = objUsuario3.IdePostulante;
                }

               
                oportunidadLaboral = new OportunidadLaboral();
                oportunidadLaboral.IdPostulante = idPostulante;

                lista = _postulanteRepository.GetMisPostulaciones(oportunidadLaboral);

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = (item.IdeCargo.ToString() + '/' + item.IdeSede + "/" + item.TipoHorario + "/" + item.FechaCreacion.ToString() + "/" + item.FechaExpiracion.ToString()),
                    cell = new string[]
                            {
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.NombreCargo==null?"":item.NombreCargo,
                                item.IdeSede==null?"":item.IdeSede.ToString(),
                                item.SedeDes==null?"":item.SedeDes,
                                item.FechaCreacion==null?"":String.Format("{0:dd/MM/yyyy}", item.FechaCreacion),
                                item.FechaExpiracion==null?"":String.Format("{0:dd/MM/yyyy}", item.FechaExpiracion),
                                item.TipoHorarioDes==null?"":item.TipoHorarioDes
                                
                            }
                }).ToArray();

                return Json(generic.Value);

            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }


    }
}
