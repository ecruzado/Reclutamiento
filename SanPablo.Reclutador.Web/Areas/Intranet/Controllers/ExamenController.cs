

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


        public ActionResult Nuevo()
        {

            ExamenViewModel model = new ExamenViewModel();

            model = InicializarExamenEdit();

            Session["Accion"] = Accion.Nuevo;

            return View("Edit", model);
        }



        private ExamenViewModel InicializarExamenEdit()
        {
            ExamenViewModel objExamenViewModel = new ExamenViewModel();

            objExamenViewModel.TipoExamen =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCriterio));
            objExamenViewModel.TipoExamen.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            objExamenViewModel.Examen = new Examen();
            return objExamenViewModel;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Edit(ExamenViewModel model)
        {

            ValidationResult result;
            ExamenValidator objExamenValid = new ExamenValidator();
            ExamenViewModel objExamenViewModal= new ExamenViewModel();
            objExamenViewModal.Examen = new Examen();
            DateTime Hoy = DateTime.Today;

            result = objExamenValid.Validate(model.Examen, "DescExamen", "NomExamen", "TipExamen");

            if (!result.IsValid)
            {
                return View(model);
            }

            objExamenViewModal = InicializarExamenEdit();
            objExamenViewModal.Examen.TipExamen = model.Examen.TipExamen;
            objExamenViewModal.Examen.DescExamen = model.Examen.DescExamen;
            objExamenViewModal.Examen.NomExamen = model.Examen.NomExamen;

            if ( Accion.Nuevo.Equals(Session["Accion"]))
            {
                
                objExamenViewModal.Examen.FecCreacion = Hoy;
                objExamenViewModal.Examen.UsrCreacion = "Prueba 01";
                objExamenViewModal.Examen.EstActivo = "A";
                objExamenViewModal.Examen.EstRegistro = "A";

                _examenRepository.Add(objExamenViewModal.Examen);
                
            }
            else
            {
                objExamenViewModal.Examen.FechaModificacion = Hoy;
                objExamenViewModal.Examen.UsrModificacion = "Prueba 02";
                _examenRepository.Update(objExamenViewModal.Examen);
            }

            return View("Edit", objExamenViewModal);

        }




        [HttpPost]
        public ActionResult ListaExamen(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "250001",
          "250001",
          "Exa01",
          "Examen 01",
          "Evaluación",
          "20",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "250002",
          "250002",
          "Exa02",
          "Examen 02",
          "Entrevista",
          "30",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
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

    }
}
