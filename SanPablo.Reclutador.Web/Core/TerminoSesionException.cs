using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Core
{
    public class TerminoSesionException:Exception
    {
        public TerminoSesionException()
        {
            TipoDevolucionError = Core.TipoDevolucionError.Html;
        }

        public TerminoSesionException(string message)
            : base(message)
        {
        }

        public TerminoSesionException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public TipoDevolucionError TipoDevolucionError { get; set; }
    }
}