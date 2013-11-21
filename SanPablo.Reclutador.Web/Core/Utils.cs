namespace SanPablo.Reclutador.Web.Core
{
    using System;
    using System.Web;

    public class Utils
    {

        #region Manejo de URLs

        private static string relativeWebRoot;

        /// <summary>
        /// Retorna la ruta relativa al sitio
        /// </summary>
        public static string RelativeWebRoot
        {
            get { return relativeWebRoot ?? (relativeWebRoot = VirtualPathUtility.ToAbsolute("~/")); }
        }

        /// <summary>
        /// Retorna la ruta absoluta al sitio
        /// </summary>
        public static Uri AbsoluteWebRoot
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new System.Net.WebException("El actual HttpContext es nulo");

                if (context.Items["absoluteurl"] == null)
                    context.Items["absoluteurl"] = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeWebRoot);

                return context.Items["absoluteurl"] as Uri;
            }
        }

        #endregion Manejo de URLs

    }
}