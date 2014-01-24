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

    public class ParientePostulanteController : BaseController
    {
        private IParientePostulanteRepository _parientePostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public ParientePostulanteController(IParientePostulanteRepository parientePostulanteRepository, IDetalleGeneralRepository detalleGeneralRepository)
        {
            _parientePostulanteRepository = parientePostulanteRepository;
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

                var generic = Listar(_parientePostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeParientePostulante.ToString(),
                        cell = new string[]
                            {
                                //item.IdeParientePostulante.ToString(),
                                item.ApellidoPaterno,
                                item.ApellidoMaterno,
                                item.Nombres,
                                item.TipoDeVinculo,
                                item.FechaNacimiento.ToString()
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
            var parientePostulanteViewModel = InicializarParientes();
            if (id == "0")
            {
                return View(parientePostulanteViewModel);
            }
            else
            {
                int ideParienteEdit = Convert.ToInt32(id);
                var parienteResultado = new ParientePostulante();
                parienteResultado = _parientePostulanteRepository.GetSingle(x => x.IdeParientePostulante == ideParienteEdit);
                parientePostulanteViewModel.Pariente = parienteResultado;
                return View(parientePostulanteViewModel);
            }
        }


        [HttpPost]
        public JsonResult Edit([Bind(Prefix = "Pariente")]ParientePostulante parientePostulante)
        {
            //var result = new JsonResult();


            if (!ModelState.IsValid)
            {
                return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
            }
            parientePostulante.EstadoActivo = IndicadorActivo.Activo;
            _parientePostulanteRepository.Add(parientePostulante);
            return Json(new { msj = true }, JsonRequestBehavior.DenyGet);

        }

        public ParientePostulanteGeneralViewModel InicializarParientes()
        {
            var parientePostulanteGeneralViewModel = new ParientePostulanteGeneralViewModel();
            parientePostulanteGeneralViewModel.Pariente = new ParientePostulante();

            parientePostulanteGeneralViewModel.TipoVinculos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVinculo));
            parientePostulanteGeneralViewModel.TipoVinculos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return parientePostulanteGeneralViewModel;
        }


        [HttpPost]
        public ActionResult eliminarPariente(int idePariente)
        {
            ActionResult result = null;

            var parienteEliminar = new ParientePostulante();
            parienteEliminar = _parientePostulanteRepository.GetSingle(x => x.IdeParientePostulante == idePariente);
            int antes = _parientePostulanteRepository.CountBy();
            _parientePostulanteRepository.Remove(parienteEliminar);
            int despues = _parientePostulanteRepository.CountBy();

            return result;
        }


        #region METODOS

        [HttpPost]
        public ActionResult listarNombreInstitucion(string tipoInstituto)
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();

            switch (tipoInstituto)
            {
                case "01": //es Universidad
                    listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNombreUnivesidad));
                    break;
                case "02": // es Instituto
                    listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNombreInstituto));
                    break;
                case "03": // es Colegio
                    listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNombreColegio));
                    break;
            }
            result = Json(listaResultado);
            return result;
        }
        public EstudioPostulanteGeneralViewModel actualizarDatos(EstudioPostulanteGeneralViewModel estudioPostulanteGeneralViewModel, EstudioPostulante estudioPostulante)
        {
            if (estudioPostulante != null)
            {
                string tipTipoInst = estudioPostulante.TipTipoInstitucion;
                switch (tipTipoInst)
                {
                    case "01":
                        estudioPostulanteGeneralViewModel.TipoNombreInstituciones = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNombreUnivesidad));
                        break;
                    case "02":
                        estudioPostulanteGeneralViewModel.TipoNombreInstituciones = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNombreInstituto));
                        break;
                    case "03":
                        estudioPostulanteGeneralViewModel.TipoNombreInstituciones = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNombreInstituto));
                        break;
                }

            }
            return estudioPostulanteGeneralViewModel;
        }
        #endregion
    }
}
