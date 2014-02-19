namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using SanPablo.Reclutador.Entity;
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

    [Authorize]
    public class SolicitudNuevoCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IAreaRepository _areaRepository;

      
        public SolicitudNuevoCargoController(ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                             IDetalleGeneralRepository detalleGeneralRepository,
                                             IDependenciaRepository dependenciaRepository,
                                             IDepartamentoRepository departamentoRepository,
                                             IAreaRepository areaRepository)
        {
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _areaRepository = areaRepository;
        }


        [HttpPost]
        public virtual JsonResult ListaSolicitudes(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<SolicitudNuevoCargo>();
                //where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_solicitudNuevoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeSolicitudNuevoCargo.ToString(),
                        cell = new string[]
                            {
                                item.EstadoActivo,
                                item.EstadoActivo,
                                item.CodigoCargo,
                                item.NombreCargo,
                                item.IdeArea.ToString(),
                                item.IdeArea.ToString(),
                                item.IdeArea.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.FechaCreacion.ToString(),
                                item.FechaExpiracion.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [AuthorizeUser]
        public ActionResult Index()
        {
            var solicitudnuevoViewModel = inicializarNuevaSolicitud();
            return View(solicitudnuevoViewModel);
        }

        [HttpPost]
        public ActionResult CambiarEstado(string id, string codEstado)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            SolicitudNuevoCargo solicitudNuevoCargo = new SolicitudNuevoCargo();
            try
            {
                solicitudNuevoCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == Convert.ToInt32(id));

                if (IndicadorActivo.Activo.Equals(codEstado))
                {
                    solicitudNuevoCargo.EstadoActivo = IndicadorActivo.Inactivo;
                    objJsonMessage.Mensaje = "La solicitud fue desactivada";
                }
                else
                {
                    solicitudNuevoCargo.EstadoActivo = IndicadorActivo.Activo;
                    objJsonMessage.Mensaje = "La solicitud fue activada";
                }
                _solicitudNuevoCargoRepository.Update(solicitudNuevoCargo);
                objJsonMessage.Resultado = true;
            }
            catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "ERROR: Ocurrio un error al cambiar el estado";
            }

            return Json(objJsonMessage);
        }


        public ActionResult Edit(string id)
        {
            var solicitudNuevoCargoViewModel = inicializarSolicitudNuevoCargo();
            if (id != "0")
            {
                var solNuevoCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == Convert.ToInt32(id));
                solicitudNuevoCargoViewModel.SolicitudNuevoCargo = solNuevoCargo;
            }
            return View(solicitudNuevoCargoViewModel);
        }

        public SolicitudNuevoCargoViewModel inicializarNuevaSolicitud()
        {
            var solicitudNuevoViewModel = new SolicitudNuevoCargoViewModel();
            
            solicitudNuevoViewModel.SolicitudNuevoCargo = new SolicitudNuevoCargo();

            solicitudNuevoViewModel.Cargos = new List<SolicitudNuevoCargo>(_solicitudNuevoCargoRepository.All());
            solicitudNuevoViewModel.Cargos.Insert(0, new SolicitudNuevoCargo { IdeSolicitudNuevoCargo = 0,NombreCargo="Seleccionar"});

            solicitudNuevoViewModel.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
            solicitudNuevoViewModel.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia ="Seleccionar"});

            solicitudNuevoViewModel.Departamentos = new List<Departamento>();
            solicitudNuevoViewModel.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            solicitudNuevoViewModel.Areas = new List<Area>();
            solicitudNuevoViewModel.Areas.Add(new Area { IdeArea = 0, NombreArea = "Selecionar" });

            return solicitudNuevoViewModel;
        }


        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "SolicitudNuevoCargo")]SolicitudNuevoCargo nuevoCargo)
        {
            var enviarMail = new SendMail();
            //int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var nuevoCargoViewModel = inicializarSolicitudNuevoCargo();
                    nuevoCargoViewModel.SolicitudNuevoCargo = nuevoCargo;
                    return View(nuevoCargoViewModel);
                }
                if (nuevoCargo.IdeSolicitudNuevoCargo == 0)
                {
                    nuevoCargo.EstadoActivo = "A";
                    nuevoCargo.FechaCreacion = FechaCreacion;
                    nuevoCargo.UsuarioCreacion = "YO";
                    nuevoCargo.FechaModificacion = FechaCreacion;
                    
                    _solicitudNuevoCargoRepository.Add(nuevoCargo);

                    enviarMail.EnviarCorreo("enviar", true, "Nuevo Requerimiento");
                }
               
                objJsonMessage.Mensaje = "Agregado Correctamente";
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }

        }
        public SolicitudNuevoCargoViewModel inicializarSolicitudNuevoCargo()
        {
            var solicitudCargoViewModel = new SolicitudNuevoCargoViewModel();
            solicitudCargoViewModel.SolicitudNuevoCargo = new SolicitudNuevoCargo();

            solicitudCargoViewModel.RangosSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            solicitudCargoViewModel.RangosSalariales.Insert(0, new DetalleGeneral { Valor ="00", Descripcion = "Seleccionar"});

            solicitudCargoViewModel.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x=>x.EstadoActivo==IndicadorActivo.Activo));
            solicitudCargoViewModel.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar"});

            solicitudCargoViewModel.Departamentos = new List<Departamento>();
            solicitudCargoViewModel.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar"});

            solicitudCargoViewModel.Areas = new List<Area>();
            solicitudCargoViewModel.Areas.Add(new Area { IdeArea=0, NombreArea ="Seleccionar"});

            return solicitudCargoViewModel;
        }

        [HttpPost]
        public ActionResult listaDepartamentos(int ideDependencia)
        {
            ActionResult result = null;

            var listaResultado = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == ideDependencia));
            result = Json(listaResultado);
            return result;
        }

        public ActionResult listaareas(int ideDepartamento)
        {
            ActionResult result = null;

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));
            result = Json(listaResultado);
            return result;
        }
        
        [HttpPost]
        public ActionResult recuperarCodigoSolicitud(string codigo)
        {
            
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {

                var solicitudCargo = new SolicitudNuevoCargo();
                solicitudCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.CodigoCargo == codigo);
                objJsonMessage.Mensaje = solicitudCargo.IdeSolicitudNuevoCargo.ToString();
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }


       
    }
}
