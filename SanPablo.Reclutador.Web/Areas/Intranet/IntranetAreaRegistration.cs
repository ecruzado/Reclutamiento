using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet
{
    public class IntranetAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Intranet";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name:"Intranet_default",
                url:"Intranet/{controller}/{action}/{id}",
                defaults: new {controller="Seguridad",  action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "SanPablo.Reclutador.Web.Areas.Intranet.Controllers" }

            );
        }
    }
}
