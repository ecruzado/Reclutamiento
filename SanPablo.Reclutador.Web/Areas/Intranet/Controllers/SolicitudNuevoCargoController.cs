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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            var solicitudNuevoCargoViewModel = inicializarSolicitudNuevoCargo();
            //if ((id != "0")||(id!=null))
            //{
            //    var solNuevoCargo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == Convert.ToInt32(id));
            //    solicitudNuevoCargoViewModel.SolicitudNuevoCargo = solNuevoCargo;
            //}
            return View(solicitudNuevoCargoViewModel);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "SolicitudNuevoCargo")]SolicitudNuevoCargo nuevoCargo)
        {
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

        //[HttpPost]
        //public ActionResult eliminarHorario(int ideHorario)
        //{
        //    ActionResult result = null;

        //    var horarioEliminar = new HorarioCargo();
        //    horarioEliminar = _horarioCargoRepository.GetSingle(x => x.IdeHorarioCargo == ideHorario);
        //    _horarioCargoRepository.Remove(horarioEliminar);

        //    return result;
        //}


       
    }
}
