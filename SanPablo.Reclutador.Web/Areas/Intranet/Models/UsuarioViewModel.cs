

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    
    public class UsuarioViewModel
    {

        public Usuario Usuario { get; set; }
        public Rol rol { get; set; }
        public virtual List<DetalleGeneral> TipRol { get; set; }
        public virtual List<DetalleGeneral> TipSede { get; set; }
        public string Accion;

    }
}