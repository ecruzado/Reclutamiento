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
        private IGeneralRepository _generalRepository;
               
        public EstudioPostulanteController(IEstudioPostulanteRepository estudioPostulanteRepository)
        {
            _estudioPostulanteRepository = estudioPostulanteRepository;
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

                var generic = Listar(_estudioPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

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
            //_estudioPostulanteRepository.Add(estudioPostulante);
            return Json("ddafd", JsonRequestBehavior.DenyGet);
            
        }

        public EstudioPostulanteGeneralViewModel InicializarEstudio()
        {
            var estudioPostulanteGeneralViewModel = new EstudioPostulanteGeneralViewModel();
            estudioPostulanteGeneralViewModel.Estudio = new EstudioPostulante();
            estudioPostulanteGeneralViewModel.TipoInstituciones = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "01", Descripcion = "Universidad" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "02", Descripcion = "Instituto" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "03", Descripcion = "Colegio" });

            estudioPostulanteGeneralViewModel.AreasEstudio = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "01", Descripcion = "Medicina General" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "02", Descripcion = "Ing. Informática y de Sistemas" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "03", Descripcion = "Enfemeria" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "04", Descripcion = "Medicina Pediatrica" });

            estudioPostulanteGeneralViewModel.Educacion = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "01", Descripcion = "Secundaria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "02", Descripcion = "Tecnica" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "03", Descripcion = "Universitaria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "04", Descripcion = "Maestria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "05", Descripcion = "Doctorado" });

            estudioPostulanteGeneralViewModel.NivelesAlcanzados = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "01", Descripcion = "Incompleta" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "02", Descripcion = "Completa" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "03", Descripcion = "En curso" });

            return estudioPostulanteGeneralViewModel;
        }
        


    }
}
