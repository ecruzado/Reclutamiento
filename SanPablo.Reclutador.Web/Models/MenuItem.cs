using SanPablo.Reclutador.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SanPablo.Reclutador.Web.Models
{
    public static class MenuExtensions
    {
        public static MvcHtmlString MenuItem(
            this HtmlHelper htmlHelper,
            string text,
            string action,
            string controller
        )
        {
            var li = new TagBuilder("li");
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");
            if (string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase))
            {
                li.AddCssClass("active");
            }

            li.InnerHtml = string.Format("<a href='{0}' ><i id='checkInfo' class='icon-ok' style='visibility:hidden' ></i> {1} </a>", Utils.RelativeWebRoot + controller + "/" + action, text, action, controller);
            return MvcHtmlString.Create(li.ToString());
        }
    }
}