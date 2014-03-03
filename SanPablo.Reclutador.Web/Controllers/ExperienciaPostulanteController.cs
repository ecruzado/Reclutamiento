namespace SanPablo.Reclutador.Web.Controllers
{
    
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using SanPablo.Reclutador.Web.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using NHibernate.Criterion;

    public class ExperienciaPostulanteController : BaseController
    {
        private IExperienciaPostulanteRepository _experienciaPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IPostulanteRepository _postulanteRepository;
        public ExperienciaPostulanteController(IExperienciaPostulanteRepository experienciaPostulanteRepository, 
                                               IDetalleGeneralRepository detalleGeneralRepository,
                                               IPostulanteRepository postulanteRepository )
        {
            _experienciaPostulanteRepository = experienciaPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _postulanteRepository = postulanteRepository;
        }

        public ActionResult Index()
        {
            var experienciaViewModel = InicializarExperiencia();
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            experienciaViewModel.Experiencia.Postulante = postulante;
            return View(experienciaViewModel);
        }

        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ExperienciaPostulante>();
                where.Add(Expression.Eq("Postulante.IdePostulante", IdePostulante));
                
                var generic = Listar(_experienciaPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaPostulante.ToString(),
                        cell = new string[]
                            {
                                item.NombreEmpresa.ToUpper(),
                                item.DescripcionCargoTrabajo.ToUpper(),
                                item.FechaTrabajoInicio.ToString(),
                                item.FechaTrabajoFin.ToString(),
                                item.IndicadorActualmenteTrabajo,
                                item.TiempoDeServicio,
                                item.DescripcionMotivoCese,
                                item.NombreReferente.ToUpper(),
                                item.DescripcionCargoReferente,
                                item.NumeroMovilReferencia.ToString(),
                                item.NumeroFijoInstitucionReferente.ToString(),
                                item.NumeroAnexoInstitucionReferente.ToString()
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

        public ViewResult Edit(string id)
        {
            var experienciaGeneralViewModel = InicializarExperiencia();
            if (id == "0")
            {
                return View(experienciaGeneralViewModel);
            }
            else
            {
                var experienciaResultado = new ExperienciaPostulante();
                experienciaResultado = _experienciaPostulanteRepository.GetSingle(x => x.IdeExperienciaPostulante == Convert.ToInt32(id));
                experienciaGeneralViewModel.Experiencia = experienciaResultado;
                return View(experienciaGeneralViewModel);
            }
        }

        [HttpPost]
        public JsonResult Edit([Bind(Prefix = "Experiencia")]ExperienciaPostulante experienciaPostulante)
        {

            if (!ModelState.IsValid)
            {
                return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
            }
            if (experienciaPostulante.IdeExperienciaPostulante == 0)
            {
                if (IdePostulante != 0)
                {
                    experienciaPostulante.EstadoActivo = IndicadorActivo.Activo;
                    var postulante = new Postulante();
                    postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                    postulante.agregarExperiencia(experienciaPostulante);
                    _experienciaPostulanteRepository.Add(experienciaPostulante);
                }
                else
                {
                    return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                var experienciaEdit = _experienciaPostulanteRepository.GetSingle(x => x.IdeExperienciaPostulante == experienciaPostulante.IdeExperienciaPostulante);
                experienciaEdit.TipoMotivoCese = experienciaPostulante.TipoMotivoCese;
                experienciaEdit.TipoCargoTrabajoReferente = experienciaPostulante.TipoCargoTrabajoReferente;
                experienciaEdit.TipoCargoTrabajo = experienciaPostulante.TipoCargoTrabajo;
                experienciaEdit.TiempoDeServicio = experienciaPostulante.TiempoDeServicio;
                experienciaEdit.NumeroMovilReferencia = experienciaPostulante.NumeroMovilReferencia;
                experienciaEdit.NumeroFijoInstitucionReferente = experienciaPostulante.NumeroFijoInstitucionReferente;
                experienciaEdit.NumeroAnexoInstitucionReferente = experienciaPostulante.NumeroAnexoInstitucionReferente;
                experienciaEdit.NombreReferente = experienciaPostulante.NombreReferente;
                experienciaEdit.NombreEmpresa = experienciaPostulante.NombreEmpresa;
                experienciaEdit.NombreCargoTrabajo = experienciaPostulante.NombreCargoTrabajo;
                experienciaEdit.IndicadorActualmenteTrabajo = experienciaPostulante.IndicadorActualmenteTrabajo;
                experienciaEdit.FechaTrabajoInicio = experienciaPostulante.FechaTrabajoInicio;
                experienciaEdit.FechaTrabajoFin = experienciaPostulante.FechaTrabajoFin;
                experienciaEdit.CorreoReferente = experienciaPostulante.CorreoReferente;

                _experienciaPostulanteRepository.Update(experienciaEdit);
            }
            return Json(new { msj = true }, JsonRequestBehavior.DenyGet);
        }

        public ExperienciaPostulanteGeneralViewModel InicializarExperiencia()
        {
            var experienciaPostulanteGeneralViewModel = new ExperienciaPostulanteGeneralViewModel();
            experienciaPostulanteGeneralViewModel.Experiencia = new ExperienciaPostulante();

            experienciaPostulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);

            //experienciaPostulanteGeneralViewModel.Experiencia.FechaTrabajoInicio = DateTime.Now;
            //experienciaPostulanteGeneralViewModel.Experiencia.FechaTrabajoFin = DateTime.Now;

            experienciaPostulanteGeneralViewModel.TipoCargos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCargo));
            experienciaPostulanteGeneralViewModel.TipoCargos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            experienciaPostulanteGeneralViewModel.TipoMotivosCese = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoMotivoCese));
            experienciaPostulanteGeneralViewModel.TipoMotivosCese.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            experienciaPostulanteGeneralViewModel.TipoCargosReferente = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCargo));
            experienciaPostulanteGeneralViewModel.TipoCargosReferente.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return experienciaPostulanteGeneralViewModel;
        }



    }
}
