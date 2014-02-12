
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class DiscapacidadCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public DiscapacidadCargo Discapacidad { get; set; }

        public List<DetalleGeneral> TipoDiscapacidad { get; set; }

    }
}
