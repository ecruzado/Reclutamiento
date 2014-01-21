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

    public class ExperienciaPostulanteController : BaseController
    {
        private IExperienciaPostulanteRepository _experienciaPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;

        public ExperienciaPostulanteController(IExperienciaPostulanteRepository experienciaPostulanteRepository, IDetalleGeneralRepository detalleGeneralRepository)
        {
            _experienciaPostulanteRepository = experienciaPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult ListaExperiencia(string sidx, string sord, int page, int rows)
        //{
        //    ActionResult result = null;
        //    List<object> lstFilas = new List<object>();

        //    var fila1 = new
        //    {
        //        id = 1,                 // ID único de la fila
        //        cell = new string[] {   // Array de celdas de la fila
        //                  "Universidad",
        //                  "Universidad de Lima",
        //                  "Administración",                          
        //                  "Pregrado",
        //                  "Completa",
        //                  "Ene/2005",
        //                  "Dic/2010",
        //        }
        //    };
        //    lstFilas.Add(fila1);

        //    var fila2 = new
        //    {
        //        id = 2,                 // ID único de la fila
        //        cell = new string[] {   // Array de celdas de la fila
        //                  "Colegio",
        //                  "Colegio la Recoleta",
        //                  "",                          
        //                  "Secundaria",
        //                  "Completa",
        //                  "Ene/2008",
        //                  "Dic/2012",                    
        //        }
        //    };
        //    lstFilas.Add(fila2);

        //    //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
        //    var data = new
        //    {
        //        //total = totalPag,       // Total de páginas
        //        //page = page,            // Página actual
        //        //records = totalReg,     // Total de registros (obtenido del modelo)
        //        rows = lstFilas
        //    };
        //    result = Json(data);

        //    return result;
        //}

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

                var generic = Listar(_experienciaPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaPostulante.ToString(),
                        cell = new string[]
                            {
                                item.NombreEmpresa,
                                item.TipoCargoTrabajo,
                                item.FechaTrabajoInicio.ToString(),
                                item.FechaTrabajoFin.ToString(),
                                item.IndicadorActualmenteTrabajo,
                                item.TiempoDeServicio,
                                item.TipoMotivoCese,
                                item.NombreReferente,
                                item.TipoCargoTrabajoReferente,
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
                int ideExperienciaEdit = Convert.ToInt32(id);
                var experienciaResultado = new ExperienciaPostulante();
                experienciaResultado = _experienciaPostulanteRepository.GetSingle(x => x.IdeExperienciaPostulante == ideExperienciaEdit);
                experienciaGeneralViewModel.Experiencia = experienciaResultado;
                return View(experienciaGeneralViewModel);
            }
        }

        [HttpPost]
        public ViewResult Edit([Bind(Prefix = "Experiencia")]ExperienciaPostulante experienciaPostulante)
        {

            if (!ModelState.IsValid)
            {
                var experienciaPostulanteModel = InicializarExperiencia();
                experienciaPostulanteModel.Experiencia = experienciaPostulante;
                return View(experienciaPostulanteModel);
            }
            experienciaPostulante.EstadoActivo = IndicadorActivo.Activo;
            _experienciaPostulanteRepository.Add(experienciaPostulante);
            return View("ddafd", JsonRequestBehavior.DenyGet);

        }

        public ExperienciaPostulanteGeneralViewModel InicializarExperiencia()
        {
            var experienciaPostulanteGeneralViewModel = new ExperienciaPostulanteGeneralViewModel();
            experienciaPostulanteGeneralViewModel.Experiencia = new ExperienciaPostulante();
            
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
