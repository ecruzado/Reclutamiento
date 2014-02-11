
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class ConocimientoCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public ConocimientoGeneralCargo Conocimiento { get; set; }

        public List<DetalleGeneral> TipoConocimientos { get; set; }
        public List<DetalleGeneral> DescripcionConocimiento { get; set; }
        public List<DetalleGeneral> NivelesConocimientos { get; set; }


    }
}
