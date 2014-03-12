using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Entity.Validation;
using SanPablo.Reclutador.Repository.Interface;
using SanPablo.Reclutador.Web.Core;
using SanPablo.Reclutador.Web.Areas.Intranet.Models;
using SanPablo.Reclutador.Web.Models.JQGrid;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using NHibernate.Criterion;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    [Authorize]
    public class SolicitudConsultaController : BaseController
    {

        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solicitudAmpliacionPersonal;
        private ICargoRepository _cargoRepository;
        private IAreaRepository _areaRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private IUsuarioRepository _usuarioRepository;


        public SolicitudConsultaController(IDetalleGeneralRepository detalleGeneralRepository,
                                           ISolReqPersonalRepository solicitudAmpliacionPersonal,
                                           ICargoRepository cargoRepository,
                                           IAreaRepository areaRepository,
                                           IDependenciaRepository dependenciaRepository,
                                           IDepartamentoRepository departamentoRepository,
                                           IUsuarioRolSedeRepository usuarioRolSedeRepository,
                                           IUsuarioRepository usuarioRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solicitudAmpliacionPersonal = solicitudAmpliacionPersonal;
            _cargoRepository = cargoRepository;
            _areaRepository = areaRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _usuarioRepository = usuarioRepository;
        }




        public SolicitudAmpliacionCargoViewModel inicializarAmpliacionCargo()
        {
            SolicitudAmpliacionCargoViewModel model = new SolicitudAmpliacionCargoViewModel();

            model.SolicitudRequerimiento = new SolReqPersonal();
            //model.SolicitudRequerimiento = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion);

            model.Cargos = new List<Cargo>(_cargoRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
            model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            model.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            model.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            model.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            model.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            model.Departamentos = new List<Departamento>();
            model.Areas = new List<Area>();
            if (Convert.ToInt32(Session[ConstanteSesion.Rol]) == Roles.Gerente_General_Adjunto)
            {
                model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
                model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccione" });

                model.Departamentos.Insert(0, new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccione" });

                model.Areas.Insert(0, new Area { IdeArea = 0, NombreArea = "Seleccione" });
            }
            else
            {
                var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
                model.Dependencias = new List<Dependencia>();
                model.Dependencias.Add(new Dependencia { IdeDependencia = usuarioSession.IDEDEPENDENCIA, NombreDependencia = usuarioSession.DEPENDENCIADES });

                model.Departamentos = new List<Departamento>();
                model.Departamentos.Add(new Departamento { IdeDepartamento = usuarioSession.IDEDEPARTAMENTO, NombreDepartamento = usuarioSession.DEPARTAMENTODES });

                model.Areas = new List<Area>();
                model.Areas.Add(new Area { IdeArea = usuarioSession.IDEAREA, NombreArea = usuarioSession.AREADES });
            }
            return model;
        }


        [ValidarSesion]
        public ActionResult Puesto(string ideSolicitud)
        {
            try
            {

                var perfilAmpliacionViewModel = inicializarPerfil();
                var usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                if (ideSolicitud != null)
                {
                    IdeSolicitudAmpliacion = Convert.ToInt32(ideSolicitud);
                    var cargoAmpliacion = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion);
                    perfilAmpliacionViewModel.SolicitudRequerimiento = cargoAmpliacion;
                }

                return PartialView(perfilAmpliacionViewModel);
            }
            catch (Exception)
            {
                //return View(perfilAmpliacionViewModel);
                return PartialView();
            }

        }


        public SolicitudAmpliacionCargoViewModel inicializarPerfil()
        {
            var ampliacionViewModel = new SolicitudAmpliacionCargoViewModel();
            ampliacionViewModel.SolicitudRequerimiento = new SolReqPersonal();


            return ampliacionViewModel;
        }

        public bool EnviarCorreo(Usuario usuarioDestinatario, string rolResponsable, string etapa, string observacion, string cargoDescripcion, string codCargo)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            try
            {
                SendMail enviarMail = new SendMail();
                enviarMail.Area = usuarioSession.AREADES;
                enviarMail.Sede = usuarioSession.SEDEDES;
                enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();
                enviarMail.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();

                enviarMail.EnviarCorreo(dir, etapa, rolResponsable, "Ampliación de cargo", observacion, cargoDescripcion, codCargo, usuarioDestinatario.Email, "suceso");

                return true;
            }
            catch (Exception Ex)
            {
                return false;

            }

        }

        public SolicitudAmpliacionCargoViewModel inicializarGeneral()
        {

            var cargoViewModel = new SolicitudAmpliacionCargoViewModel();
            cargoViewModel.SolicitudRequerimiento = new SolReqPersonal();

            cargoViewModel.Accion = Accion.Nuevo;

            cargoViewModel.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            cargoViewModel.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            cargoViewModel.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            cargoViewModel.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            cargoViewModel.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return cargoViewModel;
        }





        /// <summary>
        /// inicializa la busqueda de lista de reemplazo
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult Index()
        {
            SolicitudAmpliacionCargoViewModel model;
            try
            {
                model = new SolicitudAmpliacionCargoViewModel();


                var sede = Session[ConstanteSesion.Sede];
                if (sede != null)
                {
                    model = InicializarListaReemplazo(Convert.ToInt32(sede));
                    model.SolicitudRequerimiento = new SolReqPersonal();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View("Index", model);
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
        /// iniciliza la pantalla de busqueda de reemplazo
        /// </summary>
        /// <param name="idSel"></param>
        /// <returns></returns>
        public SolicitudAmpliacionCargoViewModel InicializarListaReemplazo(int idSel)
        {
            var model = new SolicitudAmpliacionCargoViewModel();

            model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                         && x.IdeSede == idSel));
            model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            model.Departamentos = new List<Departamento>();
            model.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            model.Areas = new List<Area>();
            model.Areas.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            model.Cargos = new List<Cargo>(_solicitudAmpliacionPersonal.GetTipCargo(0));
            model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.Roles = new List<Rol>(_usuarioRolSedeRepository.GetListaRol(0));
            model.Roles.Insert(0, new Rol { IdRol = 0, CodRol = "Seleccionar" });

            model.Etapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapaSolicitud));
            model.Etapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.Estados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            model.Estados.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.Puestos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.Puestos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return model;
        }


        /// <summary>
        /// Lista de busqueda de reemplazo
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListBusquedaAmpliacion(GridTable grid)
        {

            SolReqPersonal solicitudRequerimiento;
            List<SolReqPersonal> lista = new List<SolReqPersonal>();
            try
            {


                //if ((!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data) && !0.Equals(grid.rules[1].data)) ||
                //    (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data) && !0.Equals(grid.rules[2].data)) ||
                //    (!"".Equals(grid.rules[3].data) && !"0".Equals(grid.rules[3].data) && !0.Equals(grid.rules[3].data)) ||
                //    (!"".Equals(grid.rules[4].data) && !"0".Equals(grid.rules[4].data) && !0.Equals(grid.rules[4].data)) ||
                //    (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null) ||
                //    (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null) ||
                //    (!"".Equals(grid.rules[7].data) && !"0".Equals(grid.rules[7].data) && !0.Equals(grid.rules[7].data)) ||
                //    (!"".Equals(grid.rules[8].data) && grid.rules[8].data != null && !"0".Equals(grid.rules[8].data)) ||
                //    (!"".Equals(grid.rules[9].data) && grid.rules[9].data != null && !"0".Equals(grid.rules[9].data))

                //    )
                //{

                solicitudRequerimiento = new SolReqPersonal();

                solicitudRequerimiento.IdeCargo = (grid.rules[1].data == null ? 0 : Convert.ToInt32(grid.rules[1].data));
                solicitudRequerimiento.IdeDependencia = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));
                solicitudRequerimiento.IdeArea = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                solicitudRequerimiento.TipResponsable = (grid.rules[4].data == null ? "" : grid.rules[4].data);

                if (grid.rules[5].data != null && grid.rules[6].data != null)
                {
                    solicitudRequerimiento.FechaInicioBus = Convert.ToDateTime(grid.rules[5].data);
                    solicitudRequerimiento.FechaFinBus = Convert.ToDateTime(grid.rules[6].data);
                }

                solicitudRequerimiento.IdeDepartamento = (grid.rules[7].data == null ? 0 : Convert.ToInt32(grid.rules[7].data));
                solicitudRequerimiento.TipEtapa = (grid.rules[8].data == null ? "" : grid.rules[8].data);
                solicitudRequerimiento.TipEstado = (grid.rules[9].data == null ? "" : grid.rules[9].data);

                lista = _solicitudAmpliacionPersonal.GetListaSolReqPersonal(solicitudRequerimiento);
                //}


                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeSolReqPersonal.ToString(),
                    cell = new string[]
                            {
                               
                                "1",
                                item.TipEstado==null?"":item.TipEstado,
                                item.IdeSolReqPersonal==null?"":item.IdeSolReqPersonal.ToString(),
                                item.CodSolReqPersonal==null?"":item.CodSolReqPersonal.ToString(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.DesCargo==null?"":item.DesCargo,
                                item.IdeDependencia==null?"":item.IdeDependencia.ToString(),
                                item.Dependencia_des==null?"":item.Dependencia_des,
                                item.IdeDepartamento==null?"":item.IdeDepartamento.ToString(),
                                item.Departamento_des==null?"":item.Departamento_des,
                                item.IdeArea==null?"":item.IdeArea.ToString(),
                                item.Area_des==null?"":item.Area_des,
                                item.NumVacantes==null?"":item.NumVacantes.ToString(),
                                item.CantPostulante==null?"":item.CantPostulante.ToString(),
                                item.CantPreSelec==null?"":item.CantPreSelec.ToString(),
                                item.CantEvaluados==null?"":item.CantEvaluados.ToString(),
                                item.CantSeleccionados==null?"":item.CantSeleccionados.ToString(),
                                item.Feccreacion==null?"":item.Feccreacion.ToString(),
                                item.FecExpiracacion==null?"":item.FecExpiracacion.ToString(),
                               
                                item.idRolSuceso==null?"":item.idRolSuceso.ToString(),
                                item.DesRolSuceso==null?"":item.DesRolSuceso,
                                item.NomPersonReemplazo==null?"":item.NomPersonReemplazo,
                                
                                item.FlagPublicado==null?"":item.FlagPublicado,
                                item.TipEtapa==null?"":item.TipEtapa
                               
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
