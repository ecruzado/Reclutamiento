namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using NHibernate.Criterion;
    public class EstudioPostulanteController : BaseController
    {
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IPostulanteRepository _postulanteRepository;
               
        public EstudioPostulanteController(IEstudioPostulanteRepository estudioPostulanteRepository, 
                                           IDetalleGeneralRepository detalleGeneralRepository,
                                           IPostulanteRepository postulanteRepository )
        {
            _estudioPostulanteRepository = estudioPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _postulanteRepository = postulanteRepository;
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Index()
        {
            var estudioGeneralViewModel = InicializarEstudio();
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            estudioGeneralViewModel.Estudio.Postulante = postulante;
            return View(estudioGeneralViewModel);
        }
   
        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<EstudioPostulante>();
                where.Add(Expression.Eq("Postulante.IdePostulante",IdePostulante));

                var generic = Listar(_estudioPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeEstudiosPostulante.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoInstitucion,
                                item.DescripcionNombreInstitucion.ToUpper(),
                                item.DescripcionArea,
                                item.DescripcionEducacion,
                                item.DescripcionNivelAlcanzado,
                                item.FechaEstudioInicio.ToString(),
                                item.FechaEstudioFin.ToString()
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError("ERROR: "+ex.Message);
            }
        }
        public string datosDetalle(string codigo, int tipoDato)
        {
            var general = _detalleGeneralRepository.GetBy(x => x.IdeGeneral == tipoDato);
            var dato = general.Single(x => x.Valor == codigo);
            return dato.Descripcion;
        }

        
        public ViewResult Edit(string id)
        {
            var estudioGeneralViewModel = InicializarEstudio();
            if (id == "0")
            {
               return View(estudioGeneralViewModel);
            }
            else
            {
               var estudioResultado = new EstudioPostulante();
               estudioResultado = _estudioPostulanteRepository.GetSingle(x => x.IdeEstudiosPostulante == Convert.ToInt32(id));
               estudioGeneralViewModel.Estudio = estudioResultado;
               estudioGeneralViewModel = actualizarDatos(estudioGeneralViewModel, estudioResultado);
               return View(estudioGeneralViewModel);
           }
           
        }


        [HttpPost]
        public JsonResult Edit([Bind(Prefix = "Estudio")]EstudioPostulante estudioPostulante)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
                }
                if (estudioPostulante.IdeEstudiosPostulante == 0)
                {
                    if (IdePostulante != 0)
                    {
                        estudioPostulante.EstadoActivo = IndicadorActivo.Activo;
                        var postulante = new Postulante();
                        postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                        postulante.agregarEstudio(estudioPostulante);
                        _estudioPostulanteRepository.Add(estudioPostulante);
                    }
                    else
                    { return Json(new { msj = false }, JsonRequestBehavior.DenyGet); }
                }
                else
                {
                    var estudioEdit = _estudioPostulanteRepository.GetSingle(x => x.IdeEstudiosPostulante == estudioPostulante.IdeEstudiosPostulante);
                    estudioEdit.TipTipoInstitucion = estudioPostulante.TipTipoInstitucion;
                    estudioEdit.TipoNombreInstitucion = estudioPostulante.TipoNombreInstitucion;
                    estudioEdit.TipoNivelAlcanzado = estudioPostulante.TipoNivelAlcanzado;
                    estudioEdit.TipoEducacion = estudioPostulante.TipoEducacion;
                    estudioEdit.TipoArea = estudioPostulante.TipoArea;
                    estudioEdit.NombreInstitucion = estudioPostulante.NombreInstitucion;
                    estudioEdit.IndicadorActualmenteEstudiando = estudioPostulante.IndicadorActualmenteEstudiando;
                    estudioEdit.FechaEstudioInicio = estudioPostulante.FechaEstudioInicio;
                    estudioEdit.FechaEstudioFin = estudioPostulante.FechaEstudioFin;

                    _estudioPostulanteRepository.Update(estudioEdit);
                }
                return Json(new { msj = true }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                string error = string.Format("Error : {0}", ex.Message);
                Response.StatusCode = 500;
                Response.Write(error);
                return Json(new { msj = false, e = error }, JsonRequestBehavior.DenyGet);
            }
            
        }

        public EstudioPostulanteGeneralViewModel InicializarEstudio()
        {
            var estudioPostulanteGeneralViewModel = new EstudioPostulanteGeneralViewModel();
            estudioPostulanteGeneralViewModel.Estudio = new EstudioPostulante();

            estudioPostulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);

            estudioPostulanteGeneralViewModel.TipoTipoInstituciones = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoInstitucion));
            estudioPostulanteGeneralViewModel.TipoTipoInstituciones.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            estudioPostulanteGeneralViewModel.TipoNombreInstituciones = new List<DetalleGeneral>();
            estudioPostulanteGeneralViewModel.TipoNombreInstituciones.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
           
            estudioPostulanteGeneralViewModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoArea));
            estudioPostulanteGeneralViewModel.AreasEstudio.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            estudioPostulanteGeneralViewModel.TiposEducacion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEducacion));
            estudioPostulanteGeneralViewModel.TiposEducacion.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            estudioPostulanteGeneralViewModel.NivelesAlcanzados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.NivelAlcanzado));
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return estudioPostulanteGeneralViewModel;
        }
        

        [HttpPost]
        public ActionResult eliminarEstudio(int ideEstudio)
        {
            ActionResult result = null;

            var estudioEliminar = new EstudioPostulante();
            estudioEliminar = _estudioPostulanteRepository.GetSingle(x => x.IdeEstudiosPostulante == ideEstudio);
            _estudioPostulanteRepository.Remove(estudioEliminar);
            return result;
        }


        #region METODOS

        [HttpPost]
        public ActionResult listarNombreInstitucion(string tipoInstituto)
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();

            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoInstitucion,tipoInstituto));
            listaResultado.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTRO" });
            result = Json(listaResultado);
            return result;
        }

        public EstudioPostulanteGeneralViewModel actualizarDatos(EstudioPostulanteGeneralViewModel estudioPostulanteGeneralViewModel, EstudioPostulante estudioPostulante)
        {
             if (estudioPostulante != null)
            {
                var listaResultado = new List<DetalleGeneral>();
                listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoInstitucion, estudioPostulante.TipTipoInstitucion));
                listaResultado.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTRO" });
                estudioPostulanteGeneralViewModel.TipoNombreInstituciones = listaResultado;
            }
             return estudioPostulanteGeneralViewModel;
        }
        #endregion
    }
}
