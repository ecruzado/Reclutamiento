

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
    
    public class OportunidadLaboralController : BaseController
    {
        
        
        /// <summary>
        /// Se inicializa los repositorios de base de datos
        /// </summary>
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
            IPostulanteRepository postulanteRepository
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
