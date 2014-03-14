

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
            IConocimientoGeneralPostulanteRepository conocimientoGeneralPostulanteRepository
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

           
            model.listaCargos = new List<Cargo>(_solReqPersonalRepository.GetTipCargo(0));
            model.listaCargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            
            model.listaHorario =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.listaHorario.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.listaSede = new List<Sede>(_sedeRepository.GetBy(x => x.EstadoRegistro == IndicadorActivo.Activo));
            model.listaSede.Insert(0, new Sede { CodigoSede = "0", DescripcionSede = "Seleccionar" });

            return model;
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

            model.solReqPersonal = _postulanteRepository.GetDatosSolGrupo(model.oportunidadLaboral);

            model.solReqPersonal.FechaInicioBus = Convert.ToDateTime(fechaInicio);
            model.solReqPersonal.FechaFinBus = Convert.ToDateTime(fechaFin);
            model.solReqPersonal.NumVacantes = (numVacantes == null ? 0 : Convert.ToInt32(numVacantes));
            model.solReqPersonal.IdeCargo = Convert.ToInt32(id);

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
                                item.CantidadAnhosExperiencia.ToString() + " AÑO(S)",
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
                                item.CantidadAnhosExperiencia.ToString() + " AÑO(S)",
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
        public ActionResult validaPostulacion(int id) 
        {

            JsonMessage ObjJson = new JsonMessage();
            Usuario usuario = new Usuario();
            OportunidadLaboral objOportunidad;
            int retorno=0;


            int idPostulante;

            var objUsuario = Session[ConstanteSesion.ObjUsuarioExtranet];
            if (objUsuario!=null)
            {
                usuario = (Usuario)objUsuario;
            }

            idPostulante = usuario.IdePostulante;
            if (idPostulante!=null && idPostulante>0)
            {

                objOportunidad = new OportunidadLaboral();
                objOportunidad.IdPostulante = idPostulante;
                retorno = _postulanteRepository.ValidaPostulacion(objOportunidad);

                if (retorno>0)
                {
                    if (retorno==1)
                    {
                        ObjJson.Resultado = false;
                        ObjJson.Mensaje = "Ingrese sus estudios";
                        return Json(ObjJson);

                    }

                    if (retorno == 2)
                    {
                        ObjJson.Resultado = false;
                        ObjJson.Mensaje = "Ingrese sus experiencias";
                        return Json(ObjJson);

                    }

                    if (retorno == 3)
                    {
                        ObjJson.Resultado = false;
                        ObjJson.Mensaje = "Ingrese sus conocimientos";
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

            if (retorno==0)
            {
                ObjJson.Mensaje = "Se realizo la postulación";
            }

            return Json(ObjJson);
        }



    }
}
