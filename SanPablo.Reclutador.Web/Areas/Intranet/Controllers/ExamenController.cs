

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using System.Configuration;
    using CrystalDecisions.Shared;
    using CrystalDecisions.CrystalReports.Engine;
    using CrystalDecisions.CrystalReports;
    using CrystalDecisions.Web;

    public class ExamenController : BaseController
    {
        private ICategoriaRepository _categoriaRepository;
        private IAlternativaRepository _alternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISubcategoriaRepository _subcategoriaRepository;
        private ICriterioRepository _criterioRepository;
        private ICriterioPorSubcategoriaRepository _criterioPorSubcategoriaRepository;
        private IExamenRepository _examenRepository;
        private IExamenPorCategoriaRepository _examenPorCategoriaRepository;

        public ExamenController(ICategoriaRepository categoriaRepository, 
            IDetalleGeneralRepository detalleGeneralRepository, 
            IAlternativaRepository alternativaRepository,
            ISubcategoriaRepository subcategoriaRepository,
            ICriterioRepository criterioRepository,
            ICriterioPorSubcategoriaRepository criterioPorSubcategoriaRepository,
            IExamenRepository examenRepository,
            IExamenPorCategoriaRepository examenPorCategoriaRepository
            )
        {
            _categoriaRepository = categoriaRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _alternativaRepository = alternativaRepository;
            _subcategoriaRepository = subcategoriaRepository;
            _criterioRepository = criterioRepository;
            _criterioPorSubcategoriaRepository = criterioPorSubcategoriaRepository;
            _examenRepository = examenRepository;
            _examenPorCategoriaRepository = examenPorCategoriaRepository;
        }

        /// <summary>
        /// Nuevo :  regitra un examen
        /// </summary>
        /// <returns></returns>
        public ActionResult Nuevo()
        {
            
            ExamenViewModel model = new ExamenViewModel();

            model = InicializarExamenEdit();

            Session["Accion"] = Accion.Nuevo;

            return View("Edit", model);
        }

        
        /// <summary>
        /// Edicion : Edita un examen seleccionado
        /// </summary>
        /// <returns></returns>
       
        public ActionResult Edicion(string id)
        {
            ExamenViewModel model = new ExamenViewModel();
            model = InicializarExamenEdit();
            model.Examen = new Examen();
            model.Examen = _examenRepository.GetSingle(x => x.IdeExamen == Convert.ToInt32(id));
            
            Session["Accion"] = Accion.Editar;

            return View("Edit",model);
        }

        /// <summary>
        /// Consulta : se muestran todos los campos desabilitados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Consulta(string id)
        {
            ExamenViewModel model = new ExamenViewModel();
            model = InicializarExamenEdit();
            model.Examen = new Examen();
            model.Examen = _examenRepository.GetSingle(x => x.IdeExamen == Convert.ToInt32(id));
            
            Session["Accion"] = Accion.Consultar;

            return View("Edit",model);
        }

        


        private ExamenViewModel InicializarExamenEdit()
        {
            ExamenViewModel objExamenViewModel = new ExamenViewModel();
            objExamenViewModel.Examen = new Examen();

            objExamenViewModel.TipoExamen =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCriterio));
            objExamenViewModel.TipoExamen.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            objExamenViewModel.Examen = new Examen();
            return objExamenViewModel;
        }

        /// <summary>
        /// Inicializa los campos de la ventana inicial
        /// </summary>
        /// <returns></returns>
          private ExamenViewModel InicializarExamenIndex()
        {
            ExamenViewModel objExamenViewModel = new ExamenViewModel();
            objExamenViewModel.Examen = new Examen();

            objExamenViewModel.TipoExamen =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCriterio));
            objExamenViewModel.TipoExamen.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
           

            objExamenViewModel.TipoEstado =
              new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            objExamenViewModel.TipoEstado.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            

            return objExamenViewModel;
        }

        

        public ActionResult Index()
        {

            ExamenViewModel model = new ExamenViewModel();

            //model.Examen = new Examen();
            model = InicializarExamenIndex();
            model.Examen = new Examen();

            return View(model);
        }



        [HttpPost]
        public ActionResult Index(ExamenViewModel model)
        {

            ExamenViewModel objExamenModel = new ExamenViewModel();

            objExamenModel = InicializarExamenIndex();
            objExamenModel.Examen = new Examen();
            objExamenModel.Categoria = new Categoria();

            objExamenModel.Examen.TipExamen = model.Examen.TipExamen;
            objExamenModel.Examen.EstActivo = model.Examen.EstActivo;
            objExamenModel.Examen.DescExamen = model.Examen.DescExamen;

            return View(objExamenModel);

        }


        /// <summary>
        /// Edicion : Guarda los datos del Examen Asignado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Edicion(ExamenViewModel model)
        {

            ValidationResult result;
            ExamenValidator objExamenValid = new ExamenValidator();
            ExamenViewModel objExamenViewModal= new ExamenViewModel();
            objExamenViewModal.Examen = new Examen();
            DateTime Hoy = DateTime.Today;
            JsonMessage ObjJsonMessage = new JsonMessage();

            result = objExamenValid.Validate(model.Examen, "DescExamen", "NomExamen", "TipExamen");

            if (!result.IsValid)
            {
                return View(model);
            }

            objExamenViewModal = InicializarExamenEdit();
            objExamenViewModal.Examen.TipExamen = model.Examen.TipExamen;
            objExamenViewModal.Examen.DescExamen = model.Examen.DescExamen;
            objExamenViewModal.Examen.NomExamen = model.Examen.NomExamen;

            var dato = Session["Accion"];
            if ( Accion.Nuevo.Equals(Session["Accion"]))
            {
                
                objExamenViewModal.Examen.FecCreacion = Hoy;
                objExamenViewModal.Examen.UsrCreacion = "Prueba 01";
                objExamenViewModal.Examen.EstActivo = "A";
                objExamenViewModal.Examen.EstRegistro = "A";

                _examenRepository.Add(objExamenViewModal.Examen);
                Session["Accion"] = Accion.Editar;
            }
            else
            {
                if (model.Examen.IdeExamen != null && model.Examen.IdeExamen>0)
                {
                    var objExamen = _examenRepository.GetSingle(x => x.IdeExamen == model.Examen.IdeExamen);
                    objExamen.FecModificacion = Hoy;
                    objExamen.UsrModificacion = "Prueba 02";
                    objExamen.TipExamen = model.Examen.TipExamen;
                    objExamen.DescExamen = model.Examen.DescExamen;
                    objExamen.NomExamen = model.Examen.DescExamen;
                    _examenRepository.Update(objExamen);  
                    objExamenViewModal.Examen.IdeExamen = model.Examen.IdeExamen;
                      
                }
                
            }

            

            return RedirectToAction("Edicion", "Examen", new { id = objExamenViewModal.Examen.IdeExamen });
        }




        [HttpPost]
        public ActionResult ListaExamen(GridTable grid)
        {
            try
            {
                // int idCriterio = Convert.ToInt32(grid.rules[0].data);
                DetachedCriteria where = null;

                if ((!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                   )
                {
                    where = DetachedCriteria.For<Examen>();

                    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                    {
                        where.Add(Expression.Eq("TipExamen", grid.rules[1].data));
                    }
                    if (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data))
                    {
                        where.Add(Expression.Eq("EstActivo", grid.rules[2].data));
                    }
                    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                    {
                        where.Add(Expression.Like("DescExamen", '%' + grid.rules[3].data + '%'));
                    }

                }

                var generic = Listar(_examenRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeExamen.ToString(),
                    cell = new string[]
                            {
                                "1",
                                item.EstActivo,
                                item.IdeExamen.ToString(),
                                item.NomExamen,
                                item.DescExamen,
                                item.TipExamen,
                                item.TipExamenDes,
                                "1",
                                item.FecCreacion == DateTime.MinValue? "": item.FecCreacion.ToString("dd/MM/yyyy"),
                                item.UsrCreacion,
                                item.FecModificacion == DateTime.MinValue? "": item.FecModificacion.ToString("dd/MM/yyyy"),
                                item.UsrModificacion
                                
                               
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



        /// <summary>
        /// Obtiene las categorias seleccionadas
        /// </summary>
        /// <param name="test"></param>
        /// <param name="subCategoria"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetListaCategoria(List<int> selc, string codExamen)
        {
            DateTime Hoy = DateTime.Today;
            ExamenPorCategoria objExamenPorCategoria;
            JsonMessage objJson= new JsonMessage();
            int codigo = 0;
            int codCategoria = 0;

            if (codExamen!=null)
            {
                 codigo = Convert.ToInt32(codExamen);
            }
            else
            {
                codigo = 0;
            }

           if (selc != null && selc.Count > 0)
            {
                for (int i = 0; i < selc.Count; i++)
                {

                    objExamenPorCategoria = new ExamenPorCategoria();
                    objExamenPorCategoria.Examen = new Examen();
                    

                    codCategoria = selc[i]==null?0:selc[i];
                    var objCriterio = _examenPorCategoriaRepository.GetBy(x => x.Categoria.IDECATEGORIA == codCategoria
                                                                              && x.Examen.IdeExamen == codigo);

                    if (objCriterio!=null && objCriterio.Count>0)
                    {
                        continue;
                    }
                    else
                    {
                        objExamenPorCategoria = new ExamenPorCategoria();
                        objExamenPorCategoria.Examen = new Examen();
                        objExamenPorCategoria.Categoria = new Categoria();


                        objExamenPorCategoria.Examen.IdeExamen = codigo;
                        objExamenPorCategoria.Categoria.IDECATEGORIA = codCategoria;
                        objExamenPorCategoria.EstActivo = "A";
                        objExamenPorCategoria.FechaCreacion = Hoy;
                        objExamenPorCategoria.UsrCreacion = "Prueba 01";
                        objExamenPorCategoria.UsrModifica = "Prueba 02";
                        objExamenPorCategoria.FecModifica = Hoy;
                        _examenPorCategoriaRepository.Add(objExamenPorCategoria);
                        objJson.Mensaje = "";

                    }

                }
            }

           return Json(objJson); ;
        }


        /// <summary>
        /// ListaCatxExamen lista las categorias x Examen
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaCatxExamen(GridTable grid, int id)
        {
            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                //obtiene el valor del criterio


                // int idCriterio = Convert.ToInt32(grid.rules[0].data);

                DetachedCriteria where = DetachedCriteria.For<ExamenPorCategoria>();


                where.Add(Expression.Eq("Examen.IdeExamen", id));


                var generic = Listar(_examenPorCategoriaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExamenxCategoria.ToString(),
                        cell = new string[]
                            {
                                item.IdeExamenxCategoria.ToString(),
                                item.Categoria.IDECATEGORIA.ToString(),
                                item.Categoria.NOMCATEGORIA,
                                item.Categoria.DESCCATEGORIA,
                                ""
                                
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

        
        /// <summary>
        /// EliminarCategoriaxExamen Elimina la categoria del examen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codCat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EliminarCategoriaxExamen(string id, string codCat)
        {
            var jsonMessage = new JsonMessage();
            jsonMessage.Resultado = true;
            try
            {
                var objExamenxCat = _examenPorCategoriaRepository.GetSingle(x => x.Categoria.IDECATEGORIA == Convert.ToInt32(codCat)
                                                               && x.Examen.IdeExamen == Convert.ToInt32(id));

                _examenPorCategoriaRepository.Remove(objExamenxCat);
            }
            catch (Exception)
            {

                jsonMessage.Resultado = false;
                jsonMessage.Mensaje = "Error : No se permite eliminar la categoría";
            }
           

            
            return Json(jsonMessage);
        }


        /// <summary>
        /// ActivarDesactivar : activa y desactiva el estado del examen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codEstado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActivarDesactivar(string id, string codEstado)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            Examen objExamen = new Examen();
            try
            {
                objExamen = _examenRepository.GetSingle(x => x.IdeExamen == Convert.ToInt32(id));

                if (IndicadorActivo.Activo.Equals(codEstado))
                {
                    objExamen.EstActivo = IndicadorActivo.Inactivo;
                    objJsonMessage.Mensaje = "Se desactivado el examen";
                }
                else
                {
                    objExamen.EstActivo = IndicadorActivo.Activo;
                    objJsonMessage.Mensaje = "Se activo el examen";
                }

                objJsonMessage.Resultado = true;
                
               
             }
             catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "Error en actualizar el estado";
            }
            
            _examenRepository.Update(objExamen);

            return Json(objJsonMessage);
        }

        /// <summary>
        /// EliminarExamen : Elimina el examen seleccionado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EliminarExamen(string id)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            Examen objExamen = new Examen();
            try
            {
                objExamen = _examenRepository.GetSingle(x => x.IdeExamen == Convert.ToInt32(id));
                _examenRepository.Remove(objExamen);
                objJsonMessage.Resultado = true;
                objJsonMessage.Mensaje = "Se elimino el registro";

               
             }
             catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "Error al eliminar el registro";
            }
            
           

            return Json(objJsonMessage);
        }

        /// <summary>
        /// obtiene el archivo PDF
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
     
        public ActionResult GetExamenPDF(string id)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            string fullPath = null;
            ReportDocument rep = new ReportDocument();
            MemoryStream mem;
            //Examen objExamen;
            //Categoria objCategoria;
            //SubCategoria objSubCategoria;
            //Criterio objCriterio;
            //Alternativa objAlternativa;

            try
            {
               
                DataTable dtResultado = _examenRepository.getDataRepExamen(Convert.ToInt32(id));

                
                string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string directoryPath = ConfigurationManager.AppSettings["ReportIntranetPath"];
                string nomReporte = "ExamenReport.rpt";
                fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, nomReporte));

                rep.Load(fullPath);
                rep.Database.Tables["DtExamen"].SetDataSource(dtResultado);

                mem = (MemoryStream)rep.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
             
            }
            catch (Exception)
            {
                return MensajeError();
            }
            return File(mem, "application/pdf");   
           
        }

        

    }
}
