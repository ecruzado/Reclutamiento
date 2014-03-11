using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SanPablo.Reclutador.Web.Core
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }
        //private string redirectUrl = "";

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{

        //    filterContext.HttpContext.Request.IsAuthenticated = true;
        //    if (filterContext.HttpContext.Request.IsAuthenticated)
        //    {
        //        string authUrl = this.redirectUrl; //passed from attribute

        //        var myListOp = (List<SanPablo.Reclutador.Entity.MenuItem>)filterContext.HttpContext.Session["ListaMenu"];

        //        string rutaAbsoluta = (filterContext.HttpContext.Request.Path).ToUpper();

        //        int indWeb = rutaAbsoluta.IndexOf("/INTRANET/");

        //        var tieneAcceso = myListOp.Where(x => x.DSCURL == filterContext.HttpContext.Request.Path).ToList();


        //        if null, get it from config
        //         if (String.IsNullOrEmpty(authUrl))
        //        authUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RolesAuthRedirectUrl"];
        //        if (tieneAcceso == null && tieneAcceso.Count==0)
        //        {
        //            if (indWeb != -1)
        //            {
        //                var routeValues = new System.Web.Routing.RouteValueDictionary();
        //                routeValues["controller"] = "Seguridad";
        //                routeValues["action"] = "Login";
        //                filterContext.Result = new RedirectToRouteResult(routeValues);
        //            }
        //            else
        //            {
        //                var routeValues = new System.Web.Routing.RouteValueDictionary();
        //                routeValues["controller"] = "Seguridad";
        //                routeValues["action"] = "Login";
        //                routeValues["area"] = "Intranet";
        //                filterContext.Result = new RedirectToRouteResult(routeValues);
        //            }
        //        }
        //        else
        //        {
        //            filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Path);
        //            filterContext.HttpContext.Response.Redirect(filterContext.HttpContext.Request.Path);
        //            base.OnAuthorization(filterContext.HttpContext.Request.Path);
        //        }
               
        //        if (!String.IsNullOrEmpty(authUrl))
        //        {
        //            var routeValues = new System.Web.Routing.RouteValueDictionary();
        //            routeValues["controller"] = "Seguridad";
        //            routeValues["action"] = "Login";
        //            routeValues["area"] = "Intranet";
        //            filterContext.Result = new RedirectToRouteResult(routeValues);
        //        }
        //        filterContext.HttpContext.Response.Redirect(authUrl);
        //    }

        //    else do normal process
        //    base.HandleUnauthorizedRequest(filterContext);
        //     base.OnAuthorization(filterContext);

        //}

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var isAuthorized = base.AuthorizeCore(httpContext);
                if (!isAuthorized)
                {
                    return false;
                }
                var myListOp = (List<SanPablo.Reclutador.Entity.MenuItem>)httpContext.Session["ListaMenu"];

                string rutaAbsoluta = (httpContext.Request.Path).ToUpper();

                int indWeb = rutaAbsoluta.IndexOf("/INTRANET/");
                //string indWeb = rutaAbsoluta.

                var tieneAcceso = myListOp.Where(x => x.DSCURL == httpContext.Request.Path).ToList();

                if (tieneAcceso != null && tieneAcceso.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                    //if (indWeb != -1)
                    //{
                    //    //intranet
                    //    // return false;
                    //    var routeValues = new System.Web.Routing.RouteValueDictionary();
                    //    routeValues["controller"] = "Seguridad";
                    //    routeValues["action"] = "Login";
                    //    routeValues["area"] = "Intranet";
                    //    httpContext.Response.RedirectToRoute(routeValues);

                    //    return base.AuthorizeCore(httpContext);
                    //}
                    //else
                    //{
                    //    //extranet
                    //    //return false;

                    //    var routeValues = new System.Web.Routing.RouteValueDictionary();
                    //    routeValues["controller"] = "Seguridad";
                    //    routeValues["action"] = "Login";
                       
                    //    httpContext.Response.RedirectToRoute(routeValues);

                    //    return base.AuthorizeCore(httpContext);
                    //}
                }
            }
            catch (Exception)
            {

                return false;
            }

        }



    }
}