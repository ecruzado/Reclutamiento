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

    public class EstudioPostulanteController : BaseController
    {
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
               
        public EstudioPostulanteController(IEstudioPostulanteRepository estudioPostulanteRepository, IDetalleGeneralRepository detalleGeneralRepository)
        {
            _estudioPostulanteRepository = estudioPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaEstudios(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Universidad",
                          "Universidad de Lima",
                          "Administración",                          
                          "Pregrado",
                          "Completa",
                          "Ene/2005",
                          "Dic/2010",
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Colegio",
                          "Colegio la Recoleta",
                          "",                          
                          "Secundaria",
                          "Completa",
                          "Ene/2008",
                          "Dic/2012",                    
                }
            };
            lstFilas.Add(fila2);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
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
                
                var generic = Listar(_estudioPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeEstudiosPostulante.ToString(),
                        cell = new string[]
                            {
                                item.TipTipoInstitucion,
                                item.NombreInstitucion,
                                item.TipoArea,
                                item.TipoEducacion,
                                item.TipoNivelAlcanzado,
                                item.FechaEstudioInicio.ToString(),
                                item.FechaEstudioFin.ToString()
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

        public ViewResult Edit()
        {
            var estudioGeneralViewModel = InicializarEstudio();
            return View(estudioGeneralViewModel);
        }

        [HttpPost]
        public JsonResult Edit([Bind(Prefix = "Estudio")]EstudioPostulante estudioPostulante)
        {

            if (!ModelState.IsValid)
            {
                var estudioPostulanteModel = InicializarEstudio();
                estudioPostulanteModel.Estudio = estudioPostulante;
                return Json("dadf", JsonRequestBehavior.DenyGet);
            }
            _estudioPostulanteRepository.Add(estudioPostulante);
            return Json("ddafd", JsonRequestBehavior.DenyGet);
            
        }

        public EstudioPostulanteGeneralViewModel InicializarEstudio()
        {
            var estudioPostulanteGeneralViewModel = new EstudioPostulanteGeneralViewModel();
            estudioPostulanteGeneralViewModel.Estudio = new EstudioPostulante();
            estudioPostulanteGeneralViewModel.TipoTipoInstituciones = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("TIPTIPOINSTITUC"));
            estudioPostulanteGeneralViewModel.TipoTipoInstituciones.Insert(0,new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            estudioPostulanteGeneralViewModel.TipoNombreInstituciones = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("TIPNOMINSTITUC"));
            estudioPostulanteGeneralViewModel.TipoNombreInstituciones.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            estudioPostulanteGeneralViewModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("TIPAREA"));
            estudioPostulanteGeneralViewModel.AreasEstudio.Insert(0,new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            
            estudioPostulanteGeneralViewModel.TiposEducacion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("TIPEDUCACION"));
            estudioPostulanteGeneralViewModel.TiposEducacion.Insert(0,new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            
            estudioPostulanteGeneralViewModel.NivelesAlcanzados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla("NIVELALCANZADO"));
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Insert(0,new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            
            return estudioPostulanteGeneralViewModel;
        }
        


    }
}
