

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
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoMotivoCese));
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

    }
}
