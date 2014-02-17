

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class SeguridadViewModel
    {
        public Usuario Usuario { get; set; }
        public Rol Rol { get; set; }
        public virtual List<Rol> listaRol { get; set; }
        public virtual List<Sede> listaSede { get; set; }
        public string Accion { get; set; }
    }
}