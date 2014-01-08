using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading;
using SanPablo.Reclutador.Repository.Interface;
using NHibernate.Criterion;
using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Web.Models.JQGrid;
using SanPablo.Reclutador.Web.Core;
using NHibernate;

namespace SanPablo.Reclutador.Web.Controllers
{
    public class HomeController : BaseController
    {
        
        private ISedeRepository _sedeRepository;
        //
        // GET: /Home/
        public HomeController(ISedeRepository sedeRepository)
        {
            _sedeRepository = sedeRepository;
        }

        /*
        private ISession _session;
        public HomeController(ISession session)
        {
            _session = session;
        }*/

        public ActionResult Index()
        {
            var lista = _sedeRepository.GetPaging("CodigoSede", true, 0, 10);
            //foreach (var item in lista) 
            //{
            //    //item.CodigoExterno = "codigo ";

            //}

            //var test = _session.QueryOver<Sede>().List();
            

            return View();
        }

        /*
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

                var generic = Listar(_sedeRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.CodigoSede,
                        cell = new[]
                            {
                                item.CodigoSede,
                                item.DescripcionSede
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }*/

        /*        
        [HttpPost]
        public ActionResult GetDataTables(DataTable dataTable)
        {
            string sortField = "";
            bool ascending = false;

            List<List<string>> table = new List<List<string>>();
            var indexSortField = dataTable.iSortCols.FirstOrDefault();
            var order = dataTable.sSortDirs.FirstOrDefault();
            
            if (indexSortField == 0)
                sortField = "CodigoSede";
            else
                sortField = "DescripcionSede";
            
            if (order == DataTableSortDirection.Ascending)
                ascending = true;

            DetachedCriteria where = DetachedCriteria.For<Sede>();
            for (int i = 0; i < dataTable.sSearchs.Count; i++)
            {
                if (i == 0 && !string.IsNullOrEmpty(dataTable.sSearchs[i]))
                    where.Add(Expression.Like("CodigoSede", dataTable.sSearchs[i]));
                if (i == 1 && !string.IsNullOrEmpty(dataTable.sSearchs[i]))
                    where.Add(Expression.Like("DescripcionSede", dataTable.sSearchs[i]));
            }
            

            var list = _sedeRepository.GetPaging(sortField, ascending, dataTable.iDisplayStart, dataTable.iDisplayLength,where);
            
            var cuenta = _sedeRepository.CountBy();

            foreach (var item in list)
                table.Add(new List<string> {item.CodigoSede, item.DescripcionSede});

            var result = new DataTableResult(dataTable, cuenta, cuenta, table);
            result.ContentEncoding = Encoding.UTF8;
            return result;
        }
        */

    }
}
