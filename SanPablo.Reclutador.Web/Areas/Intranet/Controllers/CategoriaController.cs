

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

    public class CategoriaController : BaseController
    {
        private ICategoriaRepository _categoriaRepository;
        private IAlternativaRepository _alternativaRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;


        public CategoriaController(ICategoriaRepository categoriaRepository, 
            IDetalleGeneralRepository detalleGeneralRepository, 
            IAlternativaRepository alternativaRepository)
        {
            _categoriaRepository = categoriaRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _alternativaRepository = alternativaRepository;
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
        public ActionResult ListaCategoria(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "200001",
          "Cat01",
          "Categoría 01",
          "Evaluación",
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
          "200002",
          "Cat02",
          "Categoría 02",
          "Entrevista",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[]
        {
          "200003",
          "Cat03",
          "Categoría 03",
          "Entrevista",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo",
          ""
        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaSubCategoria(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "200001",
          "SubCat01",
          "SubCategoría 01",
          "5",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "200002",
          "SubCat02",
          "SubCategoría 02",
          "4",
          "01/01/2013",
          "Admin",
          "10/10/2013",
          "Admin",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaCriterio(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "100001",
          "¿Cuál es la capital del Perú?",
          "10",
          "1",
          "Automática"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "100002",
          "¿Cómo se llama el Presidente del Perú?",
          "10",
          "1",
          "Automática"
        }
            };

            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }




        public ActionResult Nuevo()
        {
            CategoriaViewModel model = new CategoriaViewModel();
            model.Categoria = new Categoria();

            model = InicializarCategoriaEdit();
            //model.Categoria.i = Accion.Nuevo;
            model.IndVisual = Visualicion.NO;
            return View("Edit", model);
        }


        private CategoriaViewModel InicializarCategoriaEdit()
        {
            var categoriaViewModel = new CategoriaViewModel();
            categoriaViewModel.Categoria = new Categoria();

            
            categoriaViewModel.TipoCategoria =
                new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCategoria));
            categoriaViewModel.TipoCategoria.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            return categoriaViewModel;
        }

    
    }
}
