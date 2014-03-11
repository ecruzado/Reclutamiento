using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SanPablo.Reclutador.Web.Models.JQGrid;

namespace SanPablo.Reclutador.Web.Core
{
    /// <summary>
    /// Valida la Sesion
    /// </summary>
    public class ValidarSesion : ActionFilterAttribute
    {
        public string[] NombresValidar { get; set; }
        public TipoDevolucionError TipoDevolucionError { get; set; }
        public string TipoServicio { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Usuario = filterContext.HttpContext.Session[ConstanteSesion.Usuario];
            var Rol = filterContext.HttpContext.Session[ConstanteSesion.Rol];

            if (Usuario==null || Rol==null)
	        {
                LanzarExcepcion(filterContext);
	        }

            if (NombresValidar !=null)
            {
                foreach (var item in NombresValidar)
                {
                    var objSession = filterContext.HttpContext.Session[item];
                    if (objSession==null)
                    {
                        LanzarExcepcion(filterContext);
                    }
                        
                }    
            }
            
        }

        /// <summary>
        /// Lanza la excepcion
        /// </summary>
        /// <param name="filterContext"></param>
        private void LanzarExcepcion(ActionExecutingContext filterContext)
        {
            if (TipoDevolucionError == Core.TipoDevolucionError.Html)
            {
                
                if (!"E".Equals(TipoServicio))
                {
                    var routeValues = new System.Web.Routing.RouteValueDictionary();
                    routeValues["controller"] = "Seguridad";
                    routeValues["action"] = "Login";
                    routeValues["area"] = "Intranet";
                    filterContext.Result = new RedirectToRouteResult(routeValues);
                }
                else
                {
                    var routeValues = new System.Web.Routing.RouteValueDictionary();
                    routeValues["controller"] = "Seguridad";
                    routeValues["action"] = "Login";
                    filterContext.Result = new RedirectToRouteResult(routeValues);
                }

               
                
                

            }
            else if (TipoDevolucionError == Core.TipoDevolucionError.Json)
            {
                JsonMessage ObjJson = new JsonMessage();
                ObjJson.redirecciona = true;
                filterContext.Result = new JsonResult { Data = ObjJson };
               
            }
        }

       
    }
}