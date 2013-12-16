using SanPablo.Reclutador.Web.Models;
using SanPablo.Reclutador.Web.Models.JQGrid;
using SanPablo.Reclutador.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Core
{
    public class BaseController : Controller
    {
        #region Paginacion

        protected GenericDouble<JQgrid, T> Listar<T>(IRepository<T> logic, string sidx, string sord, int pageIndex, int pageSize, bool search, string searchField, string searchOper, string searchString) where T : class
        {
            if (string.IsNullOrEmpty(sidx))
            {
                sidx = string.Empty;
            }
            if (string.IsNullOrEmpty(sord))
            {
                sord = "desc";
            }
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 100;
            }

            return Listar(logic, sidx, sord, pageIndex, pageSize, search, searchField, searchOper, searchString, string.Empty);
        }
        
        protected GenericDouble<JQgrid, T> Listar<T>(IRepository<T> logic, string sidx, string sord, int pageIndex, int pageSize, bool search, string searchField, string searchOper, string searchString, string @where) where T : class
        {
            var jqgrid = new JQgrid();
            IList<T> list;

            try
            {
                int totalPages = 0;

                if (search)
                {
                    @where = @where + GetWhere(searchField, searchOper, searchString);
                }

                var count = logic.CountBy();

                if (count > 0 && pageSize > 0)
                {
                    if (count % pageSize > 0)
                    {
                        totalPages = (int)Math.Ceiling((decimal)(count / pageSize));
                        totalPages = (int)(count / pageSize) + 1;
                    }
                    else
                    {
                        totalPages = (int)(count / pageSize);
                    }
                    totalPages = totalPages == 0 ? 1 : totalPages;
                }

                pageIndex = pageIndex > totalPages ? totalPages : pageIndex;

                var start = pageSize * pageIndex - pageSize;
                if (start < 0)
                {
                    start = 0;
                }

                jqgrid.total = totalPages;
                jqgrid.page = pageIndex;
                jqgrid.records = count;
                jqgrid.start = start;

                list = logic.GetPaging(sidx, true, pageIndex,pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return new GenericDouble<JQgrid, T>(jqgrid, list);
        }
        
        protected String GetWhere(string columna, string operacion, string valor)
        {
            var opciones = new Dictionary<string, string>();
            {
                opciones.Add("eq", "=");
                opciones.Add("ne", "<>");
                opciones.Add("lt", "<");
                opciones.Add("le", "<=");
                opciones.Add("gt", ">");
                opciones.Add("ge", ">=");
                opciones.Add("bw", "LIKE");
                opciones.Add("bn", "NOT LIKE");
                opciones.Add("in", "LIKE");
                opciones.Add("ni", "NOT LIKE");
                opciones.Add("ew", "LIKE");
                opciones.Add("en", "NOT LIKE");
                opciones.Add("cn", "LIKE");
                opciones.Add("nc", "NOT LIKE");
            }

            if (string.IsNullOrEmpty(operacion))
            {
                return string.Empty;
            }

            if (operacion.Equals("bw") || operacion.Equals("bn"))
            {
                valor = valor + "%";
            }
            if (operacion.Equals("ew") || operacion.Equals("en"))
            {
                valor = "%" + valor;
            }
            if (operacion.Equals("cn") || operacion.Equals("nc") || operacion.Equals("in") || operacion.Equals("ni"))
            {
                valor = "%" + valor + "%";
            }
            if (opciones.Take(6).ToDictionary(p => p.Key, p => p.Value).ContainsKey(operacion))
            {
                return string.IsNullOrEmpty(valor) ? string.Empty : (columna + " ") + opciones[operacion] + " " + valor;
            }

            return columna + " " + opciones[operacion] + " '" + valor + "'";
        }

        #endregion Paginacion


        #region Control Error

        protected override void OnException(ExceptionContext filterContext)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            //logger.Error(string.Format("Controlador:{0}  Action:{1}  Mensaje:{2}", controllerName, actionName, filterContext.Exception.Message));
            filterContext.Result = View("Error");
        }

        protected JsonResult MensajeError(string mensaje = "Ocurrio un error al cargar...")
        {
            Response.StatusCode = 404;
            return Json(new JsonResponse { Message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion Control Error


    }
}
