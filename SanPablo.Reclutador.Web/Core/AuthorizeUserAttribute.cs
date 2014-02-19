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

                var tieneAcceso = myListOp.Where(x => x.DSCURL == httpContext.Request.Path).ToList();
                if (tieneAcceso != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
           
        }
    }
}