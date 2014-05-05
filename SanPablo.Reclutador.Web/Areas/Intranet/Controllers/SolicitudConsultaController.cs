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
        private IListaSolicitudNuevoCargoVistaRepository _listaSolicitudes;
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;


        public SolicitudConsultaController(IDetalleGeneralRepository detalleGeneralRepository,
                                           ISolReqPersonalRepository solicitudAmpliacionPersonal,
                                           ICargoRepository cargoRepository,
                                           IAreaRepository areaRepository,
                                           IDependenciaRepository dependenciaRepository,
                                           IDepartamentoRepository departamentoRepository,
                                           IUsuarioRolSedeRepository usuarioRolSedeRepository,
                                           IUsuarioRepository usuarioRepository,
                                           IListaSolicitudNuevoCargoVistaRepository listaSolicitudes,
                                           ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solicitudAmpliacionPersonal = solicitudAmpliacionPersonal;
            _cargoRepository = cargoRepository;
            _areaRepository = areaRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _usuarioRepository = usuarioRepository;
            _listaSolicitudes = listaSolicitudes;
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
        }



        /// <summary>
        /// inicializa la busqueda de lista de reemplazo
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult Index()
        {
            SolicitudConsultaViewModel model;
            try
            {
                model = new SolicitudConsultaViewModel();


                var sede = Session[ConstanteSesion.Sede];
                if (sede != null)
                {
                    model = InicializarListaReemplazo(Convert.ToInt32(sede));
                    model.SolicitudRequerimiento = new SolReqPersonal();
                }

                int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                if (idRol != 0)
                {
                    if ((idRol == Roles.Encargado_Seleccion) || (idRol == Roles.Analista_Seleccion))
                    {
                        model.btnActivarDesactivar = Visualicion.SI;
                    }
                    else
                    {
                        model.btnActivarDesactivar = Visualicion.NO;
                    }
                }

                //accesos de botones
                idRol = (Session[ConstanteSesion.Rol] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Rol]));

                if (Roles.Administrador_Sistema.Equals(idRol))
                {
                    model.btnRanking = Visualicion.SI;
                    model.btnPreSeleccion = Visualicion.SI;
                }
                else if (Roles.Gerente_General_Adjunto.Equals(idRol))
                {
                    model.btnRanking = Visualicion.SI;
                    model.btnPreSeleccion = Visualicion.SI;
                }
                else if (Roles.Jefe_Corporativo_Seleccion.Equals(idRol))
                {
                    model.btnRanking = Visualicion.SI;
                    model.btnPreSeleccion = Visualicion.SI;
                }
                else if(Roles.Jefe.Equals(idRol))
                {
                    model.btnRanking = Visualicion.SI;
                    model.btnPreSeleccion = Visualicion.SI;
                }
                else if(Roles.Gerente.Equals(idRol))
                {
                    model.btnRanking = Visualicion.SI;
                    model.btnPreSeleccion = Visualicion.SI;
                }
                else
                {
                    model.btnRanking = Visualicion.NO;
                    model.btnPreSeleccion = Visualicion.NO;
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

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));

            foreach (Area item in listaResultado)
            {
                item.Departamento = null;
            }
            
            
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// iniciliza la pantalla de busqueda de reemplazo
        /// </summary>
        /// <param name="idSel"></param>
        /// <returns></returns>
        public SolicitudConsultaViewModel InicializarListaReemplazo(int idSel)
        {
            var model = new SolicitudConsultaViewModel();

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

            model.TiposSolicitudes = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSolicitud));
            model.TiposSolicitudes.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return model;
        }


        /// <summary>
        /// Lista de busqueda de reemplazo
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListBusqueda(GridTable grid)
        {

            SolReqPersonal solicitudRequerimiento;
            List<SolicitudConsulta> lista = new List<SolicitudConsulta>();
            try
            {
                solicitudRequerimiento = new SolReqPersonal();

                solicitudRequerimiento.IdeSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
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
                solicitudRequerimiento.TipoSolicitud = (grid.rules[10].data == "0"? "" : grid.rules[10].data);
                solicitudRequerimiento.CodSolReqPersonal = (grid.rules[11].data == null ? "" : grid.rules[11].data);

                lista = _listaSolicitudes.ListaSolicitudesRequerimientos(solicitudRequerimiento);


                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeSolicitud.ToString()+ item.CodigoSolicitud.ToString() + item.TipoSolicitud.ToString(),
                    cell = new string[]
                            {
                               
                                "1",
                                item.EstadoSolicitud==null?"":item.EstadoSolicitud,
                                item.IdeSolicitud==0?"0":item.IdeSolicitud.ToString(),
                                item.CodigoSolicitud==null?"":item.CodigoSolicitud.ToString(),
                                item.IdeCargo==0?"0":item.IdeCargo.ToString(),
                                item.NombreCargo==null?"":item.NombreCargo,
                                item.IdeDependencia==0?"0":item.IdeDependencia.ToString(),
                                item.NombreDependencia==null?"":item.NombreDependencia,
                                item.IdeDepartamento==0?"0":item.IdeDepartamento.ToString(),
                                item.NombreDepartamento==null?"":item.NombreDepartamento,
                                item.IdeArea==0?"0":item.IdeArea.ToString(),
                                item.NombreArea==null?"":item.NombreArea,
                                item.NumeroVacantes==0?"0":item.NumeroVacantes.ToString(),
                                item.Postulantes==0?"0":item.Postulantes.ToString(),
                                item.Preseleccionados==0?"":item.Preseleccionados.ToString(),
                                item.Evaluados==0?"0":item.Evaluados.ToString(),
                                item.Seleccionados==0?"0":item.Seleccionados.ToString(),
                                item.FechaCreacion==null?"":item.FechaCreacion.ToString(),
                                item.FechaExpiracion==null?"":item.FechaExpiracion.ToString(),
                               
                                item.IdeRolResponsable==0?"0":item.IdeRolResponsable.ToString(),
                                item.RolResponsable==null?"":item.RolResponsable,
                                item.NombreResponsable==null?"":item.NombreResponsable,
                                
                                item.Publicado==null?"":item.Publicado,
                                item.TipoEtapa==null?"":item.TipoEtapa,
                                item.Etapa==null?"":item.Etapa,
                                item.TipoSolicitud==null?"":item.TipoSolicitud,
                                item.NombreTipoSolicitud == null?"":item.NombreTipoSolicitud
                               
                            }
                }).ToArray();

                return Json(generic.Value);

            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }

        [HttpPost]
        public ActionResult ActivarDesactivar(string tipSol, string codEstado, string id)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            SolicitudNuevoCargo solicitudNuevoCargo = new SolicitudNuevoCargo();
            SolReqPersonal solicitudRemAmpCargo = new SolReqPersonal();
            try
            {

                if (tipSol == TipoSolicitud.Nuevo)
                {
                    solicitudNuevoCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == Convert.ToInt32(id));

                    if (IndicadorActivo.Inactivo.Equals(codEstado))
                    {
                        solicitudNuevoCargo.EstadoActivo = IndicadorActivo.Activo;
                        objJsonMessage.Mensaje = "La solicitud fue activada";
                        objJsonMessage.Accion = IndicadorActivo.Activo;
                    }
                    else
                    {
                        solicitudNuevoCargo.EstadoActivo = IndicadorActivo.Inactivo;
                        objJsonMessage.Mensaje = "La solicitud fue desactivada";
                        objJsonMessage.Accion = IndicadorActivo.Inactivo;
                    }
                    _solicitudNuevoCargoRepository.Update(solicitudNuevoCargo);

                    if (solicitudNuevoCargo.FechaPublicacion != null)
                    {
                        objJsonMessage.Objeto = Indicador.Si;
                    }
                    else
                    {
                        objJsonMessage.Objeto = Indicador.No;
                    }
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    if ((tipSol == TipoSolicitud.Ampliacion) || (tipSol == TipoSolicitud.Remplazo))
                    {
                        solicitudRemAmpCargo = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(id));

                        if (IndicadorActivo.Inactivo.Equals(codEstado))
                        {
                            solicitudRemAmpCargo.EstadoActivo = IndicadorActivo.Activo;
                            objJsonMessage.Mensaje = "La solicitud fue activada";
                            objJsonMessage.Accion = IndicadorActivo.Activo;
                            
                        }
                        else
                        {
                            solicitudRemAmpCargo.EstadoActivo = IndicadorActivo.Inactivo;
                            objJsonMessage.Mensaje = "La solicitud fue desactivada";
                            objJsonMessage.Accion = IndicadorActivo.Inactivo;
                        }
                        _solicitudAmpliacionPersonal.Update(solicitudRemAmpCargo);
                        //verficar la publicacion
                        if (solicitudRemAmpCargo.FecPublicacion != null)
                        {
                            objJsonMessage.Objeto = Indicador.Si;
                        }
                        else
                        {
                            objJsonMessage.Objeto = Indicador.No;
                        }
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "ERROR: Ocurrio un error al cambiar el estado de la solicitud";
                        objJsonMessage.Objeto = Indicador.No;
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
            }
            catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "ERROR: Ocurrio un error al cambiar el estado";
                return Json(objJsonMessage);
            }

        }

    }
}
