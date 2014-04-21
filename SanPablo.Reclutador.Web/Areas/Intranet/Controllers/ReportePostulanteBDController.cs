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
                postulanteBD.AreaEstudio = (grid.rules[1].data == null ? "" : grid.rules[1].data);
                postulanteBD.RangoSalarial = (grid.rules[2].data == null ? "" : grid.rules[2].data);

                postulanteBD.IdeDepartamento = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                postulanteBD.IdeProvincia = (grid.rules[4].data == null ? 0 : Convert.ToInt32(grid.rules[4].data));
                postulanteBD.IdeDistrito = (grid.rules[5].data == null ? 0 : Convert.ToInt32(grid.rules[5].data));

                if (grid.rules[6].data != null && grid.rules[7].data != null)
                {
                    postulanteBD.FechaDesde = Convert.ToDateTime(grid.rules[6].data);
                    postulanteBD.FechaHasta = Convert.ToDateTime(grid.rules[7].data);
                }

                postulanteBD.EdadInicio = (grid.rules[8].data == null ? 0 : Convert.ToInt32(grid.rules[8].data));
                postulanteBD.EdadFin = (grid.rules[9].data == null ? 0 : Convert.ToInt32(grid.rules[9].data));

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
                                item.Edad==null?"":item.Edad.ToString(),
                                item.TipoEstudio==null?"":item.TipoEstudio,
                                item.AreaEstudio==null?"":item.AreaEstudio,
                                item.RangoSalarial==null?"":item.RangoSalarial,
                                item.IdePostulante==null?"":item.IdePostulante.ToString()
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

    }
}
