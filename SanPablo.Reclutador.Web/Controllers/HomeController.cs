using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading;
using SanPablo.Reclutador.Web.Repository.Interface;
using NHibernate.Criterion;
using SanPablo.Reclutador.Web.Entity;

namespace SanPablo.Reclutador.Web.Controllers
{
    public class HomeController : Controller
    {
        private ISedeRepository _sedeRepository;
        //
        // GET: /Home/
        public HomeController(ISedeRepository sedeRepository)
        {
            _sedeRepository = sedeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }
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
        }*/


    }
}
