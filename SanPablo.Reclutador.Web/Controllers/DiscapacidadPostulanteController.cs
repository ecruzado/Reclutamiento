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

    public class DiscapacidadPostulanteController : BaseController
    {
        private IDiscapacidadPostulanteRepository _discapacidadPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public DiscapacidadPostulanteController(IDiscapacidadPostulanteRepository discapacidadPostulanteRepository, IDetalleGeneralRepository detalleGeneralRepository)
        {
            _discapacidadPostulanteRepository = discapacidadPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                //var where = (Utils.GetWhere(grid.filters, grid.rules));
                var where = string.Empty;

                if (!string.IsNullOrEmpty(where))
                {
                    grid._search = true;

                    if (!string.IsNullOrEmpty(grid.searchString))
                    {
                        where = where + " and ";
                    }
                }

                var generic = Listar(_discapacidadPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeDiscapacidadPostulante.ToString(),
                        cell = new string[]
                            {
                                item.TipoDiscapacidad,
                                item.DescripcionDiscapacidad,
                              
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
            var discapacidadPostulanteViewModel = InicializarDiscapacidades();
            if (id == "0")
            {
                return View(discapacidadPostulanteViewModel);
            }
            else
            {
                int ideDiscapacidadEdit = Convert.ToInt32(id);
                var discapacidadResultado = new DiscapacidadPostulante();
                discapacidadResultado = _discapacidadPostulanteRepository.GetSingle(x => x.IdeDiscapacidadPostulante == ideDiscapacidadEdit);
                discapacidadPostulanteViewModel.Discapacidad = discapacidadResultado;
                return View(discapacidadPostulanteViewModel);
            }
        }


        [HttpPost]
        public JsonResult Edit([Bind(Prefix = "Discapacidad")]DiscapacidadPostulante discapacidadPostulante)
        {
            //var result = new JsonResult();


            if (!ModelState.IsValid)
            {
                return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
            }
            discapacidadPostulante.EstadoActivo = IndicadorActivo.Activo;
            _discapacidadPostulanteRepository.Add(discapacidadPostulante);
            return Json(new { msj = true }, JsonRequestBehavior.DenyGet);

        }

        public DiscapacidadPostulanteGeneralViewModel InicializarDiscapacidades()
        {
            var discapacidadPostulanteGeneralViewModel = new DiscapacidadPostulanteGeneralViewModel();
            discapacidadPostulanteGeneralViewModel.Discapacidad = new DiscapacidadPostulante();

            discapacidadPostulanteGeneralViewModel.TipoDiscapacidades = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoDiscapacidad));
            discapacidadPostulanteGeneralViewModel.TipoDiscapacidades.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return discapacidadPostulanteGeneralViewModel;
        }


        [HttpPost]
        public ActionResult eliminarDiscapacidad(int ideDiscapacidad)
        {
            ActionResult result = null;

            var discapacidadEliminar = new DiscapacidadPostulante();
            discapacidadEliminar = _discapacidadPostulanteRepository.GetSingle(x => x.IdeDiscapacidadPostulante == ideDiscapacidad);
            int antes = _discapacidadPostulanteRepository.CountBy();
            _discapacidadPostulanteRepository.Remove(discapacidadEliminar);
            int despues = _discapacidadPostulanteRepository.CountBy();

            return result;
        }


       
    }
}
