
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class RolViewModel
    {

        public Rol rol { get; set; }
        public virtual List<DetalleGeneral> listaIndSede { get; set; }
        public string Accion { get; set; }


    }
}