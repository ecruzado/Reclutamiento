
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class CentroEstudioViewModel
    {
        public Cargo Cargo { get; set; }
        public CentroEstudioCargo CentroEstudio { get; set; }
       
        public List<DetalleGeneral> TiposInstitucion { get; set; }
        public List<DetalleGeneral> Instituciones { get; set; }

    }
}
