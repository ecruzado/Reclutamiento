
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class NivelAcademicoViewModel
    {
        public Cargo Cargo { get; set; }
        public NivelAcademicoCargo NivelAcademico { get; set; }
        
        public List<DetalleGeneral> TiposEducacion { get; set; }
        public List<DetalleGeneral> AreasEstudio { get; set; }
        public List<DetalleGeneral> NivelesAlcanzados { get; set; }

        
    }
}
